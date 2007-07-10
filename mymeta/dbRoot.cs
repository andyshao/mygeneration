using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections;
using System.Reflection;

using System.Diagnostics;

using Npgsql;
using FirebirdSql.Data.Firebird;
using System.Data.SQLite;

namespace MyMeta
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	using System.EnterpriseServices;



	/// <summary>
	/// MyMeta is the root of the MyMeta meta-data. MyMeta is an intrinsic object available to your script and configured based on the settings
	/// you have entered in the Default Settings dialog. It is already connected before you script execution begins.
	/// </summary>
	/// <remarks>
	///	MyMeta has 1 Collection:
	/// <list type="table">
	///		<item><term>Databases</term><description>Contains a collection of all of the databases in your system</description></item>
	///	</list>
	/// There is a property collection on every entity in your database, you can add key/value
	/// pairs to the User Meta Data either through the user interface of MyGeneration or 
	/// programmatically in your scripts.  User meta data is stored in XML and never writes to your database.
	///
	/// This can be very useful, you might need more meta data than MyMeta supplies, in fact,
	/// MyMeta will eventually offer extended meta data using this feature as well. The current plan
	/// is that any extended data added via MyGeneration will have a key that beings with "MyMeta.Something"
	/// where 'Something' equals the description. 
	/// </remarks>
	/// <example>
	///	VBScript - ****** NOTE ****** You never have to actually write this code, this is for education purposes only.
	///	<code>
	///	MyMeta.Connect "SQL", "Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost"
	///	
	///	MyMeta.DbTarget	= "SqlClient"
	///	MyMeta.DbTargetMappingFileName = "C:\Program Files\MyGeneration\Settings\DbTargets.xml"
	///	
	/// MyMeta.Language = "VB.NET"
	/// MyMeta.LanguageMappingFileName = "C:\Program Files\MyGeneration\Settings\Languages.xml"
	/// 
	/// MyMeta.UserMetaDataFileName = "C:\Program Files\MyGeneration\Settings\UserMetaData.xml"
	/// </code>
	///	JScript - ****** NOTE ****** You never have to actually write this code, this is for education purposes only.
	///	<code>
	///	MyMeta.Connect("SQL", "Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost")
	///	
	///	MyMeta.DbTarget	= "SqlClient";
	///	MyMeta.DbTargetMappingFileName = "C:\Program Files\MyGeneration\Settings\DbTargets.xml";
	///	
	/// MyMeta.Language = "VB.NET";
	/// MyMeta.LanguageMappingFileName = "C:\Program Files\MyGeneration\Settings\Languages.xml";
	/// 
	/// MyMeta.UserMetaDataFileName = "C:\Program Files\MyGeneration\Settings\UserMetaData.xml";
	/// </code>
	/// The above code is done for you long before you execute your script and the values come from the Default Settings Dialog.
	/// However, you can override these defaults as many of the sample scripts do. For instance, if you have a script that is for SqlClient
	/// only go ahead and set the MyMeta.DbTarget in your script thus overriding the Default Settings.
	/// </example>
	[GuidAttribute("147a73a3-5620-480e-bf38-379440fa086c"),ClassInterface(ClassInterfaceType.AutoDual)]
	public class dbRoot : ServicedComponent
#else
	public class dbRoot 
#endif 
    {
		public dbRoot()
		{
			Reset();
		}

		private void Reset()
		{
			UserData = null;

			IgnoreCase = true;
			requiredDatabaseName = false;
			requiresSchemaName = false;
			StripTrailingNulls = false;
			TrailingNull = ((char)0x0).ToString();

			ClassFactory = null;

			_showSystemData = false;

			_driver = dbDriver.None;
			_driverString = "NONE";
			_databases = null;
			_connectionString = "";
			_theConnection = new OleDbConnection();
			_isConnected = false;
			_parsedConnectionString = null;
			_defaultDatabase = "";

			// Language
			_languageMappingFileName = string.Empty;
			_language = string.Empty;
			_languageDoc = null;
			LanguageNode = null;

			UserData = new XmlDocument();
			UserData.AppendChild(UserData.CreateNode(XmlNodeType.Element, "MyMeta", null));

			// DbTarget
			_dbTargetMappingFileName = string.Empty;
			_dbTarget = string.Empty;
			_dbTargetDoc = null;
			DbTargetNode = null;
		}

		#region Properties

		/// <summary>
		/// Contains all of the databases in your DBMS system.
		/// </summary>
		public IDatabases Databases
		{
			get
			{
				if(null == _databases)
				{
					if(this.ClassFactory != null)
					{
						_databases = (Databases)ClassFactory.CreateDatabases();
						_databases.dbRoot = this;
						_databases.LoadAll();
					}
				}

				return _databases;
			}
		}

		/// <summary>
		/// This is the default database as defined in your connection string, or if not provided your DBMS system may provide one.
		/// Finally, for single database systems like Microsoft Access it will be the default database.
		/// </summary>
		public IDatabase DefaultDatabase
		{
			get
			{
				IDatabase  defDatabase = null;
				try
				{
					Databases dbases = this.Databases as Databases;

					if(this._defaultDatabase != null && this._defaultDatabase != "")
						defDatabase =  dbases.GetByPhysicalName(this._defaultDatabase);				
					else
					{
						if(dbases.Count == 1)
						{
							defDatabase = dbases[0];
						}
					}
				}
				catch {}

				return defDatabase;
			}
		}

		public IProviderTypes ProviderTypes
		{
			get
			{
				if(null == _providerTypes)
				{
					_providerTypes = (ProviderTypes)ClassFactory.CreateProviderTypes();
					_providerTypes.dbRoot = this;
					_providerTypes.LoadAll();
				}

				return _providerTypes;
			}
		}

		#endregion

		#region Connection 

		[ComVisible(false)]
		public IDbConnection BuildConnection(string driver, string connectionString) 
		{
			IDbConnection conn = null;
			switch(driver.ToUpper())
			{
				case "MYSQL2":
					MyMeta.MySql5.MySql5Databases.LoadAssembly();
					conn = MyMeta.MySql5.MySql5Databases.CreateConnection(connectionString);
					break;

				case "POSTGRESQL":
				case "POSTGRESQL8":
					conn = new Npgsql.NpgsqlConnection(connectionString);
					break;

				case "FIREBIRD":
				case "INTERBASE":
					conn = new FirebirdSql.Data.Firebird.FbConnection(connectionString);
                    break;

                case "SQLITE":
                    conn = new SQLiteConnection(connectionString);
                    break;

				case "VISTADB":	
					try
					{
						MyMeta.VistaDB.MetaHelper mh = new MyMeta.VistaDB.MetaHelper();
						conn = mh.GetConnection(connectionString);
					}
					catch
					{
						throw new Exception("Invalid VistaDB connection or VistaDB not installed");
					}
					break;
				default:
                    if (Plugins.ContainsKey(driver))
                    {
                        conn = this.GetConnectionFromPlugin(driver, connectionString);
                    }
                    else
                    {
                        conn = new OleDbConnection(connectionString);
                    }
					break;
			}
			return conn;
        }

		/// <summary>
		/// This is how you connect to your DBMS system using MyMeta. This is already called for you before your script beings execution.
		/// </summary>
		/// <param name="driver">A string as defined in the remarks below</param>
		/// <param name="connectionString">A valid connection string for you DBMS</param>
		/// <returns>True if connected, False if not</returns>
		/// <remarks>
		/// These are the supported "drivers".
		/// <list type="table">
		///		<item><term>"ACCESS"</term><description>Microsoft Access 97 and higher</description></item>
		///		<item><term>"DB2"</term><description>IBM DB2</description></item>	
		///		<item><term>"MYSQL"</term><description>Currently limited to only MySQL running on Microsoft Operating Systems</description></item>
		///		<item><term>"MYSQL2"</term><description>Uses MySQL Connector/Net, Supports 4.x schema info on Windows or Linux</description></item>
		///		<item><term>"ORACLE"</term><description>Oracle 8i - 9</description></item>
		///		<item><term>"SQL"</term><description>Microsoft SQL Server 2000 and higher</description></item>	
		///		<item><term>"PERVASIVE"</term><description>Pervasive 9.00+ (might work on lower but untested)</description></item>		
		///		<item><term>"POSTGRESQL"</term><description>PostgreSQL 7.3+ (might work on lower but untested)</description></item>		
		///		<item><term>"POSTGRESQL8"</term><description>PostgreSQL 8.0+</description></item>	
		///		<item><term>"FIREBIRD"</term><description>Firebird</description></item>		
		///		<item><term>"INTERBASE"</term><description>Borland's InterBase</description></item>		
		///		<item><term>"SQLITE"</term><description>SQLite</description></item>		
		///		<item><term>"VISTADB"</term><description>VistaDB Database</description></item>		
        ///		<item><term>"ADVANTAGE"</term><description>Advantage Database Server</description></item>	
        ///		<item><term>"ISERIES"</term><description>iSeries (AS400)</description></item>	
		///	</list>
		/// Below are some sample connection strings. However, the "Data Link" dialog available on the Default Settings dialog can help you.
		/// <list type="table">
		///		<item><term>"ACCESS"</term><description>Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\access\newnorthwind.mdb;User Id=;Password=</description></item>
		///		<item><term>"DB2"</term><description>Provider=IBMDADB2.1;Password=sa;User ID=DB2Admin;Data Source=MyMeta;Persist Security Info=True</description></item>	
		///		<item><term>"MYSQL"</term><description>Provider=MySQLProv;Persist Security Info=True;Data Source=test;UID=griffo;PWD=;PORT=3306</description></item>
		///		<item><term>"MYSQL2"</term><description>Uses Database=Test;Data Source=Griffo;User Id=anonymous;</description></item>
		///		<item><term>"ORACLE"</term><description>Provider=OraOLEDB.Oracle.1;Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta</description></item>
		///		<item><term>"SQL"</term><description>Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=Northwind;Data Source=localhost</description></item>
		///		<item><term>"PERVASIVE"</term><description>Provider=PervasiveOLEDB.8.60;Data Source=demodata;Location=Griffo;Persist Security Info=False</description></item>		
		///		<item><term>"POSTGRESQL"</term><description>Server=www.myserver.com;Port=5432;User Id=myuser;Password=aaa;Database=mygeneration;</description></item>		
		///		<item><term>"POSTGRESQL8"</term><description>Server=www.myserver.com;Port=5432;User Id=myuser;Password=aaa;Database=mygeneration;</description></item>		
		///		<item><term>"FIREBIRD"</term><description>Database=C:\firebird\EMPLOYEE.GDB;User=SYSDBA;Password=wow;Dialect=3;Server=localhost</description></item>		
		///		<item><term>"INTERBASE"</term><description>Database=C:\interbase\EMPLOYEE.GDB;User=SYSDBA;Password=wow;Dialect=3;Server=localhost</description></item>		
		///		<item><term>"SQLITE"</term><description>Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3</description></item>		
		///		<item><term>"VISTADB"</term><description>DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb</description></item>		
		///		<item><term>"ADVANTAGE"</term><description>Provider=Advantage.OLEDB.1;Password="";User ID=AdsSys;Data Source=C:\task1;Initial Catalog=aep_tutorial.add;Persist Security Info=True;Advantage Server Type=ADS_LOCAL_SERVER;Trim Trailing Spaces=TRUE</description></item>		
        ///		<item><term>"ISERIES"</term><description>PROVIDER=IBMDA400; DATA SOURCE=MY_SYSTEM_NAME;USER ID=myUserName;PASSWORD=myPwd;DEFAULT COLLECTION=MY_LIBRARY;</description></item>		
		///	</list>
		///	</remarks>
		public bool Connect(string driver, string connectionString)
		{
			switch(driver.ToUpper())
			{
				case "SQL":
					return this.Connect(dbDriver.SQL, connectionString);

				case "ORACLE":
					return this.Connect(dbDriver.Oracle, connectionString);

				case "ACCESS":
					return this.Connect(dbDriver.Access, connectionString);

				case "MYSQL":
					return this.Connect(dbDriver.MySql, connectionString);

				case "MYSQL2":
					return this.Connect(dbDriver.MySql2, connectionString);

				case "DB2":
					return this.Connect(dbDriver.DB2, connectionString);

				case "ISERIES":
					return this.Connect(dbDriver.ISeries, connectionString);

				case "PERVASIVE":
					return this.Connect(dbDriver.Pervasive, connectionString);

				case "POSTGRESQL":
					return this.Connect(dbDriver.PostgreSQL, connectionString);

				case "POSTGRESQL8":
					return this.Connect(dbDriver.PostgreSQL8, connectionString);

				case "FIREBIRD":
					return this.Connect(dbDriver.Firebird, connectionString);

				case "INTERBASE":
					return this.Connect(dbDriver.Interbase, connectionString);

				case "SQLITE":
					return this.Connect(dbDriver.SQLite, connectionString);

				case "VISTADB":
                    return this.Connect(dbDriver.VistaDB, connectionString);

                case "ADVANTAGE":
                    return this.Connect(dbDriver.Advantage, connectionString);
                    
				case "NONE":
					return true;

				default:
                    if (Plugins.ContainsKey(driver))
                    {
                        return this.Connect(dbDriver.Plugin, driver, connectionString);
                    }
                    else
                    {
                        return false;
                    }
			}

		}

        public bool Connect(dbDriver driver, string connectionString)
        {
            return Connect(driver, string.Empty, connectionString);
        }

		/// <summary>
		/// Same as <see cref="Connect(string, string)"/>(string, string) only this uses an enumeration.  
		/// </summary>
		/// <param name="driver">The driver enumeration for you DBMS system</param>
		/// <param name="connectionString">A valid connection string for you DBMS</param>
		/// <returns></returns>
		public bool Connect(dbDriver driver, string pluginName, string connectionString)
		{
			Reset();

			string dbName;
			int index;

			this._connectionString = connectionString.Replace("\"", "");
			this._driver = driver;

			switch(_driver)
			{
				case dbDriver.SQL:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "SQL";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = true;
					ClassFactory = new MyMeta.Sql.ClassFactory();
					break;

				case dbDriver.Oracle:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "ORACLE";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = true;
					ClassFactory = new MyMeta.Oracle.ClassFactory();
					break;

				case dbDriver.Access:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "ACCESS";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.Access.ClassFactory();
					break;

				case dbDriver.MySql:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "MYSQL";
					this.StripTrailingNulls = true;
					this.requiredDatabaseName = true;
					ClassFactory = new MyMeta.MySql.ClassFactory();
					break;

				case dbDriver.MySql2:

					try
					{
						MyMeta.MySql5.MySql5Databases.LoadAssembly();
						IDbConnection conn = MyMeta.MySql5.MySql5Databases.CreateConnection(_connectionString);
						conn.Open();
						this._defaultDatabase = conn.Database;
						conn.Close();
						conn.Dispose();

						this._driverString = "MYSQL2";
						this.StripTrailingNulls = true;
						this.requiredDatabaseName = true;
						ClassFactory = new MyMeta.MySql5.ClassFactory();
					}
					catch (Exception ex)
					{
						throw ex;
					}
					break;

				case dbDriver.DB2:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "DB2";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.DB2.ClassFactory();
					break;

				case dbDriver.ISeries:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "ISERIES";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.ISeries.ClassFactory();
					break;

				case dbDriver.Pervasive:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "PERVASIVE";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.Pervasive.ClassFactory();
					break;

				case dbDriver.PostgreSQL:

					NpgsqlConnection cn = new Npgsql.NpgsqlConnection(_connectionString);
					cn.Open();
					this._defaultDatabase = cn.Database;
					cn.Close();
					
					this._driverString = "POSTGRESQL";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.PostgreSQL.ClassFactory();
					break;

				case dbDriver.PostgreSQL8:

					NpgsqlConnection cn8 = new Npgsql.NpgsqlConnection(_connectionString);
					cn8.Open();
					this._defaultDatabase = cn8.Database;
					cn8.Close();
					
					this._driverString = "POSTGRESQL8";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.PostgreSQL8.ClassFactory();
					break;

				case dbDriver.Firebird:

					FbConnection cn1 = new FirebirdSql.Data.Firebird.FbConnection(_connectionString);
					cn1.Open();
					dbName = cn1.Database;
					cn1.Close();

					try
					{
						index = dbName.LastIndexOfAny(new char[]{'\\'});
						if (index >= 0)
						{
							this._defaultDatabase = dbName.Substring(index + 1);
						}
					}
					catch {}
					
					this._driverString = "FIREBIRD";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.Firebird.ClassFactory();
					break;

				case dbDriver.Interbase:

					FbConnection cn2 = new FirebirdSql.Data.Firebird.FbConnection(_connectionString);
					cn2.Open();
					this._defaultDatabase = cn2.Database;
					cn2.Close();
					
					this._driverString = "INTERBASE";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.Firebird.ClassFactory();
					break;

				case dbDriver.SQLite:

					SQLiteConnection sqliteConn = new SQLiteConnection(_connectionString);
					sqliteConn.Open();
					dbName = sqliteConn.Database;
					sqliteConn.Close();
					this._driverString = "SQLITE";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.SQLite.ClassFactory();
					break;

				case dbDriver.VistaDB:

					try
					{
						MyMeta.VistaDB.MetaHelper mh = new MyMeta.VistaDB.MetaHelper();
						dbName = mh.LoadDatabases(_connectionString);

						if(dbName == "") return false;

						this._defaultDatabase = dbName;

						this._driverString = "VISTADB";
						this.StripTrailingNulls = false;
						this.requiredDatabaseName = false;
						ClassFactory = new MyMeta.VistaDB.ClassFactory();
					}
					catch
					{
						throw new Exception("Invalid VistaDB connection or VistaDB not installed");
					}

					break;

				case dbDriver.Advantage:

					ConnectUsingOleDb(_driver, _connectionString);
					this._driverString = "ADVANTAGE";
					this.StripTrailingNulls = false;
					this.requiredDatabaseName = false;
					ClassFactory = new MyMeta.Advantage.ClassFactory();
					string[] s = this._defaultDatabase.Split('.');
					this._defaultDatabase = s[0];
					break;

                case dbDriver.Plugin:

                    IMyMetaPlugin plugin;
                    IDbConnection connection = this.GetConnectionFromPlugin(pluginName, _connectionString, out plugin);
                    connection.Open();
                    dbName = connection.Database;
                    connection.Close();
                    this._driverString = pluginName;
                    //this.StripTrailingNulls = plugin.StripTrailingNulls;
                    //this.requiredDatabaseName = plugin.RequiredDatabaseName;
                    ClassFactory = new MyMeta.Plugin.ClassFactory(plugin);
                    break;

				case dbDriver.None:

					this._driverString = "NONE";
					break;
			}

			_isConnected = true;
			return true;
		}

		private void ConnectUsingOleDb(dbDriver driver, string connectionString)
		{
			try
			{
				OleDbConnection cn = new OleDbConnection(connectionString.Replace("\"", "")); 
				cn.Open();
				this._defaultDatabase = GetDefaultDatabase(cn, driver);
				cn.Close();
			}
			catch(OleDbException Ex)
			{
				throw Ex;
			}
		}


		internal OleDbConnection TheConnection
		{
			get
			{
				if(this._theConnection.State != ConnectionState.Open)
				{
					this._theConnection.ConnectionString = this._connectionString;
					this._theConnection.Open();
				}

				return this._theConnection;
			}
		}

		private string GetDefaultDatabase(OleDbConnection cn, dbDriver driver)
		{
			string databaseName = string.Empty;

			switch(driver)
			{
				case dbDriver.Access:

					int i = cn.DataSource.LastIndexOf(@"\");

					if(i == -1) 
						databaseName = cn.DataSource;
					else
						databaseName = cn.DataSource.Substring(++i);

					break;

				default:

					databaseName = cn.Database;
					break;
			}

			return databaseName;
		}

		/// <summary>
		/// True if MyMeta has been successfully connected to your DBMS, False if not.
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return  _isConnected;
			}
		}

		/// <summary>
		/// Returns MyMeta's current dbDriver enumeration value as defined by its current connection.
		/// </summary>
		public dbDriver Driver
		{
			get
			{
				return _driver;
			}
		}

		/// <summary>
		/// Returns MyMeta's current DriverString as defined by its current connection.
		/// </summary>
		/// <remarks>
		/// These are the current possible values.
		/// <list type="table">
		///		<item><term>"ACCESS"</term><description>Microsoft Access 97 and higher</description></item>
		///		<item><term>"DB2"</term><description>IBM DB2</description></item>	
		///		<item><term>"MYSQL"</term><description>Currently limited to only MySQL running on Microsoft Operating Systems</description></item>
		///		<item><term>"ORACLE"</term><description>Oracle 8i - 9</description></item>
		///		<item><term>"SQL"</term><description>Microsoft SQL Server 2000 and higher</description></item>	
		///		<item><term>"PostgreSQL"</term><description>PostgreSQL</description></item>	///		
		///	</list>
		///	</remarks>
		public string DriverString
		{
			get
			{
				return _driverString;
			}
		}

		/// <summary>
		/// Returns the current connection string. ** WARNING ** Currently the password is returned, the password will be stripped from this
		/// property in the very near future.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
		}

		internal Hashtable ParsedConnectionString
		{
			get
			{
				if(null == _parsedConnectionString)
				{
					string[] first = ConnectionString.Split(new char[] {';'});

					_parsedConnectionString = new Hashtable(first.GetUpperBound(0));

					string[] kv = null;

					for(int i = 0; i < first.GetUpperBound(0); i++)
					{
						kv = first[i].Split(new char[] {'='});

						if(1 == kv.GetUpperBound(0))
						{
							_parsedConnectionString.Add(kv[0].ToUpper(), kv[1]);
						}
						else
						{
							_parsedConnectionString.Add(kv[0].ToUpper(), "");
						}
					}
				}

				return _parsedConnectionString;
			}
		}

		#endregion

		#region Settings

		/// <summary>
		/// Determines whether system tables and views and alike are shown, the default is False. If True, ONLY system data is shown.
		/// </summary>
		public bool ShowSystemData
		{
			get	{ return _showSystemData;   }
			set	{ _showSystemData = value ; }
		}

		/// <summary>
		/// If this is true then four IColumn properties are actually supplied by the Domain, if the Column has an IDomain. 
		/// The four properties are DataTypeName, DataTypeNameComplete, LanguageType, and DbTargetType.
		/// </summary>
		public bool DomainOverride
		{
			get	{ return _domainOverride;   }
			set	{ _domainOverride = value ; }
		}

		#endregion

        #region Plugin Members
        /// <summary>
        /// A Plugin ConnectionString is a special feature for external assemblies.
        /// </summary>
        /// <param name="connectionString">Sample: PluginName;Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost</param>
        /// <returns></returns>
        private IDbConnection GetConnectionFromPlugin(string providerName, string pluginConnectionString)
        {
            IMyMetaPlugin plugin;

            return GetConnectionFromPlugin(providerName, pluginConnectionString, out plugin);
        }

        /// <summary>
        /// A Plugin ConnectionString is a special feature for external assemblies.
        /// </summary>
        /// <param name="pluginConnectionString">Sample: PluginName;Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost</param>
        /// <param name="plugin">Returns the plugin object.</param>
        /// <returns></returns>
        private IDbConnection GetConnectionFromPlugin(string providerName, string pluginConnectionString, out IMyMetaPlugin plugin)
        {
            MyMetaPluginContext pluginContext = new MyMetaPluginContext(providerName, pluginConnectionString);

            IDbConnection connection = null;
            if (!Plugins.ContainsKey(providerName))
            {
                throw new Exception("MyMeta Plugin \"" + providerName + "\" not registered.");
            }
            else
            {
                plugin = Plugins[providerName] as IMyMetaPlugin;
                plugin.Initialize(pluginContext);

                connection = plugin.NewConnection;
            }

            return connection;
        }

        private static Hashtable plugins;

        [ComVisible(false)]
        public static Hashtable Plugins
        {
            get
            {
                if (plugins == null)
                {
                    plugins = new Hashtable();
                    FileInfo info = new FileInfo(Assembly.GetCallingAssembly().Location);
                    if (info.Exists)
                    {
                    	StringBuilder fileNames = new StringBuilder();
                    	Exception err = null;
                        foreach (FileInfo dllFile in info.Directory.GetFiles("MyMeta.Plugins.*.dll"))
                        {
                        	try
                        	{
                        		loadPlugin(dllFile.FullName, plugins);
                        	} catch(Exception ex) {
                        		// Fix K3b 2007-06-27 if the current plugin cannot be loaded ignore it.
                        		//			i got the exception when loading a plugin that was linked against an old Interface
                        		//			the chatch ensures that the rest of the application-initialisation continues ...
                        		fileNames.AppendLine(dllFile.FullName);
                        		err = ex;
                        	}
                        }
                        
                        //TODO How to tell the caller that something is not ok. A Exception would result in a incomplete initialisation
//                        if (err != null)
//                        	throw new ApplicationException("Cannot load Plugin(s) " + fileNames.ToString(), err);
                    }
                    
                }

                return plugins;
            }
        }
        
		private static void loadPlugin(string filename, Hashtable plugins)
		{
			// MyMeta.Plugins.
			Assembly assembly = Assembly.LoadFile(filename);
			foreach (Type type in assembly.GetTypes())
			{
			    Type[] interfaces = type.GetInterfaces();
			    foreach (Type iface in interfaces)
			    {
			        if (iface == typeof(IMyMetaPlugin))
			        {
			            try
			            {
			                ConstructorInfo[] constructors = type.GetConstructors();
			                ConstructorInfo constructor = constructors[0];
			
			                IMyMetaPlugin plugin = constructor.Invoke(BindingFlags.CreateInstance, null, new object[] { }, null) as IMyMetaPlugin;
			                plugins[plugin.ProviderUniqueKey] = plugin;
			            }
			            catch {}
			        }
			    }
			}
		}
        
        #endregion
        
        #region XML User Data

		public string UserDataXPath
		{ 
			get
			{
				return @"//MyMeta";
			} 
		}

		internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				if(!UserData.HasChildNodes)
				{
					_xmlNode = UserData.CreateNode(XmlNodeType.Element, "MyMeta", null);
					UserData.AppendChild(_xmlNode);
				}
				else
				{
					_xmlNode = UserData.SelectSingleNode("./MyMeta");
				}
			}

			if(null != _xmlNode)
			{
				node = _xmlNode;
				success = true;
			}

			return success;
		}

		/// <summary>
		/// The full path of the XML file that contains the user defined meta data. See IPropertyCollection
		/// </summary>
		public string UserMetaDataFileName
		{
			get	{ return _userMetaDataFileName; }
			set
			{
				_userMetaDataFileName = value;

				try
				{
					UserData = new XmlDocument();
					UserData.Load(_userMetaDataFileName);
				}
				catch 
				{
					UserData = new XmlDocument();
				}
			}
		}

		/// <summary>
		/// Call this method to save any user defined meta data that you may have modified. See <see cref="UserMetaDataFileName"/>
		/// </summary>
		/// <returns>True if saved, False if not</returns>
		public bool SaveUserMetaData()
		{
			if(null != UserData && string.Empty != _userMetaDataFileName)
			{
				UserData.Save(_userMetaDataFileName);
				return true;
			}

			return false;
		}

		private string _userMetaDataFileName = "";

		#endregion

		#region XML Language Mapping

		/// <summary>
		/// The full path of the XML file that contains the language mappings. The data in this file plus the value you provide 
		/// to <see cref="Language"/> determine the value of IColumn.Language.
		/// </summary>
		public string LanguageMappingFileName
		{
			get { return _languageMappingFileName;	}
			set
			{
				try
				{
					_languageMappingFileName = value;

					_languageDoc = new XmlDocument();
					_languageDoc.Load(_languageMappingFileName);
					_language = string.Empty;;
					LanguageNode = null;
				}
				catch {}
			}
		}

		/// <summary>
		/// Returns all of the languages currently configured for the DBMS set when Connect was called.
		/// </summary>
		/// <returns>An array with all of the possible languages.</returns>
		public string[] GetLanguageMappings()
		{
			return GetLanguageMappings(_driverString);
		}

		/// <summary>
		/// Returns all of the languages for a given driver, regardless of MyMeta's current connection
		/// </summary>
		/// <returns>An array with all of the possible languages.</returns>
		public string[] GetLanguageMappings(string driverString)
		{
			

			string[] mappings = null;

			if ((null != _languageDoc) && (driverString != null))
			{
                driverString = driverString.ToUpper();
				string xPath = @"//Languages/Language[@From='" + driverString + "']";
				XmlNodeList nodes = _languageDoc.SelectNodes(xPath, null);

				if ((null != nodes) && (nodes.Count > 0))
				{
					int nodeCount = nodes.Count;
					mappings = new string[nodeCount];

					for(int i = 0; i < nodeCount; i++)
					{
						mappings[i] = nodes[i].Attributes["To"].Value;
					}
				}
			}

			return mappings;
		}

		/// <summary>
		/// Use this to choose your Language, for example, "C#". See <see cref="LanguageMappingFileName"/> for more information
		/// </summary>
		public string Language
		{
			get
			{
				return _language;
			}

			set
			{
				if(null != _languageDoc)
				{
					_language = value;
					string xPath = @"//Languages/Language[@From='" + _driverString + "' and @To='" + _language + "']";
					LanguageNode = _languageDoc.SelectSingleNode(xPath, null);
				}
			}
		}

		private string _languageMappingFileName = string.Empty;
		private string _language = string.Empty;
		private XmlDocument _languageDoc;
		internal XmlNode LanguageNode = null;

		#endregion

		#region XML DbTarget Mapping

		/// <summary>
		/// The full path of the XML file that contains the DbTarget mappings. The data in this file plus the value you provide 
		/// to <see cref="DbTarget"/> determine the value of IColumn.DbTarget.
		/// </summary>
		public string DbTargetMappingFileName
		{
			get	{ return _dbTargetMappingFileName; }
			set
			{
				try
				{
					_dbTargetMappingFileName = value;

					_dbTargetDoc = new XmlDocument();
					_dbTargetDoc.Load(_dbTargetMappingFileName);
					_dbTarget = string.Empty;;
					DbTargetNode = null;
				}
				catch {}
			}
		}

		/// <summary>
		/// Returns all of the dbTargets currently configured for the DBMS set when Connect was called.
		/// </summary>
		/// <returns>An array with all of the possible dbTargets.</returns>
		public string[] GetDbTargetMappings()
		{
			return GetDbTargetMappings(_driverString);
		}

		/// <summary>
		/// Returns all of the dbTargets for a given driver, regardless of MyMeta's current connection
		/// </summary>
		/// <returns>An array with all of the possible dbTargets.</returns>
		public string[] GetDbTargetMappings(string driverString)
		{
			

			string[] mappings = null;

			if ((null != _dbTargetDoc) && (driverString != null))
			{
                driverString = driverString.ToUpper();
				string xPath = @"//DbTargets/DbTarget[@From='" + driverString + "']";
				XmlNodeList nodes = _dbTargetDoc.SelectNodes(xPath, null);

				if(null != nodes && nodes.Count > 0)
				{
					int nodeCount = nodes.Count;
					mappings = new string[nodeCount];

					for(int i = 0; i < nodeCount; i++)
					{
						mappings[i] = nodes[i].Attributes["To"].Value;
					}
				}
			}

			return mappings;
		}

		/// <summary>
		/// Use this to choose your DbTarget, for example, "SqlClient". See <see cref="DbTargetMappingFileName"/>  for more information
		/// </summary>
		public string DbTarget
		{
			get
			{
				return _dbTarget;
			}

			set
			{
				if(null != _dbTargetDoc)
				{
					_dbTarget = value;
					string xPath = @"//DbTargets/DbTarget[@From='" + _driverString + "' and @To='" + _dbTarget + "']";
					DbTargetNode = _dbTargetDoc.SelectSingleNode(xPath, null);
				}
			}
		}

		private string _dbTargetMappingFileName = string.Empty;
		private string _dbTarget = string.Empty;
		private XmlDocument _dbTargetDoc;
		internal XmlNode DbTargetNode = null;

		#endregion
      
        internal XmlDocument UserData = new XmlDocument();

		internal bool IgnoreCase = true;
		internal bool requiredDatabaseName = false;
		internal bool requiresSchemaName = false;
		internal bool StripTrailingNulls = false;

		internal string TrailingNull = null;

		internal IClassFactory ClassFactory = null;

		private bool _showSystemData = false;

		private dbDriver _driver = dbDriver.None;
		private string _driverString = "NONE";
		private string _defaultDatabase = "";
		private Databases _databases = null;
		private ProviderTypes _providerTypes = null;
		private string _connectionString = "";
		private bool _isConnected = false;
		private Hashtable _parsedConnectionString = null;
        private bool _domainOverride = true;

		private XmlNode _xmlNode = null;

		private OleDbConnection _theConnection = new OleDbConnection();
	}

	/// <summary>
	/// The current list of support dbDrivers. Typically VBScript and JScript use the string version as defined by MyMeta.DriverString.
	/// </summary>
#if ENTERPRISE
	[GuidAttribute("9bb31988-13bd-481f-9913-7efc9a42bd11")]
#endif
	public enum dbDriver
	{
		/// <summary>
		/// String form is "SQL" for DriverString property
		/// </summary>
		SQL,

		/// <summary>
		/// String form is "ORACLE" for DriverString property
		/// </summary>
		Oracle,

		/// <summary>
		/// String form is "ACCESS" for DriverString property
		/// </summary>
		Access,

		/// <summary>
		/// String form is "MYSQL" for DriverString property
		/// </summary>
		MySql,

		/// <summary>
		/// String form is "MYSQL" for DriverString property
		/// </summary>
		MySql2,

		/// <summary>
		/// String form is "DB2" for DriverString property
		/// </summary>
		DB2,

		/// <summary>
		/// String form is "ISeries" for DriverString property
		/// </summary>
		ISeries,

		/// <summary>
		/// String form is "PERVASIVE" for DriverString property
		/// </summary>
		Pervasive,

		/// <summary>
		/// String form is "POSTGRESQL" for DriverString property
		/// </summary>
		PostgreSQL,

		/// <summary>
		/// String form is "POSTGRESQL8" for DriverString property
		/// </summary>
		PostgreSQL8,

		/// <summary>
		/// String form is "FIREBIRD" for DriverString property
		/// </summary>
		Firebird,

		/// <summary>
		/// String form is "INTERBASE" for DriverString property
		/// </summary>
		Interbase,

		/// <summary>
		/// String form is "SQLITE" for DriverString property
		/// </summary>
		SQLite,

		/// <summary>
		/// String form is "VISTADB" for DriverString property
		/// </summary>
		VistaDB,

		/// <summary>
		/// String form is "ADVANTAGE" for DriverString property
		/// </summary>
        Advantage,

        /// <summary>
        /// This is a placeholder for plugin providers
        /// </summary>
        Plugin,

		/// <summary>
		/// Use this if you want know connection at all
		/// </summary>
		None
	}
}

