// #define RUN_AS_NON_ADMIN // if defined changes Directory layout "C:\Program Files\..." to "..\user\MyGeneration\..."

using System;
using System.Xml;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;
using Zeus;

namespace MyGeneration
{
    /// <History>
    /// 20070818 k3b
    ///     Refactored DefaultSettings. There are no more exceptions due to missing xml-items
    ///         Missing items are created on demand now.
    ///     MyGeneration Settings can be embedded inside a bigger xml-file
    ///         Reason: Required for plugins for MsVisualStudio or SharpDevelopper
    ///     Moved Writeoperations form "C:\Program Files\..." to "..\user\MyGeneration\..."
    ///         Reason: Otherwise MyGeneration requires Admin privileges to run
    ///         status: not finished yet
    ///         status: disabled via "#if RUN_AS_NON_ADMIN" until finished
    ///         Search for RUN_AS_NON_ADMIN, UserDataPath and ApplicationPath to see details
    ///  20070819 k3b
    ///     There is only one DefaultSettings for all modules (sigelton) instead of many instances
    ///         reason: improved loading speed, no risk to work with outdated data
    /// </History>
	/// <summary>
	/// Summary description for DefaultSettings.
	/// </summary>
	public class DefaultSettings
	{
		private const string MISSING = "*&?$%";

        #region members for content that is loaded on demand
        private string _appPath = null;
        private string _userDataPath = null;
        private Hashtable _savedConnections = null;
		private ArrayList _recentFiles = null;
        #endregion

        private XmlElement settingsRootNode = null;

        // use "DefaultSettings.Settings" instead of "new DefaultSettings()" to get the settings
        //  to discard chages without saving call settings.DiscardChanges()
		private DefaultSettings()
		{
            Load();
        }

        // k3b: there is only one setting for all modules (sigelton) 
        // reason: improved loading speed, no risk to work with outdated data
        private static DefaultSettings theSettings;
        public static DefaultSettings Instance
        {
            get 
            {
                if (theSettings == null)
                    theSettings = new DefaultSettings();
                return theSettings; 
            }
        }
        public void DiscardChanges()
        {
            _appPath = null;
            _userDataPath = null;
            _savedConnections = null;
		    _recentFiles = null;

            Load();
            // theSettings = null; // force reload
        }
        public static void Refresh()
        {
            instance = null;
        }


        private void Load()
        {
            XmlDocument xmlDoc = new XmlDocument();

            Assembly asmblyMyGen = Assembly.GetEntryAssembly();
            string version = asmblyMyGen.GetName().Version.ToString();

            settingsRootNode = null;
            try
            {
                string filename = SettingsFileName;
#if RUN_AS_NON_ADMIN
                // current user has no own settings yet
                if (!System.IO.File.Exists(filename))
                    filename = ApplicationPath + @"\Settings\DefaultSettings.xml";                
#endif
                xmlDoc.Load(filename);
            }
            catch
            {
                // Our file doesn't exist, or it is invalid xml. So let's (re-)create it
                xmlDoc = new XmlDocument();
                StringBuilder defaultXML = new StringBuilder();
                defaultXML.Append(@"<?xml version='1.0' encoding='utf-8'?>");
                defaultXML.Append(@"<DefaultSettings>");
                defaultXML.Append(@"</DefaultSettings>");

                xmlDoc.LoadXml(defaultXML.ToString());

            }

            // MyGeneration Settings can be embedded inside a bigger xml-file
            settingsRootNode = xmlDoc.SelectSingleNode("//DefaultSettings", null) as XmlElement;
            if (settingsRootNode == null)
            {
                settingsRootNode = xmlDoc.AppendChild(xmlDoc.CreateElement("DefaultSettings", null)) as XmlElement;
            }


            if (this.Version != version)
            {
                // Our Version has changed, or DefaultSettings were just created
                // write any new settings and their defaults
                this.FillInMissingSettings(version);
                Save();
            }

        }
	
        private string settingsFileName = null;

        // defaults to user-directy\MyGeneration\DefaultSettings.xml if found 
        //  else app-directory\Settings\DefaultSettings.xml
        private string SettingsFileName
        {
            get 
            {
                if (settingsFileName == null)
                {
                    settingsFileName = UserDataPath + @"\Settings\DefaultSettings.xml";
                    return settingsFileName;
                }
                return settingsFileName; 
            }
            set { settingsFileName = value; }
        }
	
        /// <summary>
        /// k3b: all getters now have build in default values
        /// This method makes shure that every property is set
        /// </summary>
        /// <param name="version"></param>
        private void FillInMissingSettings(string version)
        {
            // Version
            this.Version = version;

            // foreach(property prop in this) 
            //      prop.Set(Prop.Get)
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                MethodInfo getter = prop.GetGetMethod();
                MethodInfo setter = prop.GetSetMethod();

                if ((getter != null) && (setter != null))
                {
                    object[] par = new Object[1];
                    par[0] = getter.Invoke(this, null);
                    setter.Invoke(this, par);
                }
            }
        }

		public void Save()
		{
			this.UpdateSavedConnections();
			this.UpdateRecentFiles();
            
			settingsRootNode.OwnerDocument.Save(SettingsFileName);
		}

		#region Recent Files Nodes
		public void UpdateRecentFiles() 
		{
			if (this._recentFiles != null)
			{
                XmlNode parentNode = settingsRootNode;

				ArrayList nodesToHack = new ArrayList();
                XmlNodeList nodes = settingsRootNode.GetElementsByTagName("RecentFile");
				
				foreach (XmlNode node in nodes) nodesToHack.Add(node);
				foreach (XmlNode node in nodesToHack) parentNode.RemoveChild(node);

				int i = 0;
				foreach (string path in _recentFiles) 
				{
					XmlNode node = settingsRootNode.OwnerDocument.CreateNode(XmlNodeType.Element, "RecentFile", null);
					node.InnerText = path;

					parentNode.AppendChild(node);

					if (++i > 20) break;
				}

				_recentFiles = null;
			}
		}

		public ArrayList RecentFiles
		{
			get 
			{ 
				if (_recentFiles == null) 
				{
					_recentFiles = new ArrayList();

                    XmlNodeList nodes = settingsRootNode.GetElementsByTagName("RecentFile");
					foreach (XmlNode node in nodes) 
					{
						if ((node.InnerText != null) && (node.InnerText != string.Empty)) 
						{
							if (File.Exists(node.InnerText))
							{
								_recentFiles.Add(node.InnerText);
							}
						}
					}
				}

				return _recentFiles; 
			}
		}
		#endregion

		#region Saved Connections Nodes
		public void UpdateSavedConnections() 
		{
			if (this._savedConnections != null)
			{
                XmlNode parentNode = settingsRootNode;

                XmlNodeList nodes = settingsRootNode.GetElementsByTagName("SavedSettings");
                ArrayList nodesToHack = new ArrayList();
				foreach (XmlNode node in nodes) 
				{
					if (!this.SavedConnections.ContainsKey( DefaultSettings.GetAttribute(node, "name", MISSING) ))
					{
						nodesToHack.Add(node);
					}
				}
				foreach (XmlNode node in nodesToHack) parentNode.RemoveChild(node);

				foreach (ConnectionInfo inf in _savedConnections.Values) 
				{
					string xPath = @"SavedSettings[@name='" + inf.Name.Replace("'", "&apos;") + "']";
                    XmlNode node = settingsRootNode.SelectSingleNode(xPath, null);
                    if (node == null)
                        node = parentNode.AppendChild(settingsRootNode.OwnerDocument.CreateNode(XmlNodeType.Element, "SavedSettings", null));

					SetAttribute(node, "name", inf.Name) ;
					SetAttribute(node, "driver", inf.Driver) ;
					SetAttribute(node, "connstr", inf.ConnectionString) ;
					SetAttribute(node, "dbtargetpath", inf.DbTargetPath) ;
					SetAttribute(node, "languagepath", inf.LanguagePath) ;
					SetAttribute(node, "usermetapath", inf.UserMetaDataPath) ;
					SetAttribute(node, "language", inf.Language) ;
					SetAttribute(node, "dbtarget", inf.DbTarget) ;
				}

				_savedConnections = null;
			}
		}

        /// <summary>
        /// robust replacement for parentNode.Attributes[name].Value = ...
        /// creates the attribute if it does not existent.
        /// </summary>
        internal static void SetAttribute(XmlNode parentNode, string name, string value)
        {
            if (parentNode != null)
            {
                XmlAttribute attr = GetOrCreateAttribute(parentNode, name);
                attr.Value = value;
            }
        }

        /// <summary>
        /// robust replacement for x = parentNode.Attributes[name].Value
        /// </summary>
        internal static string GetAttribute(XmlNode parentNode, string name, string notFoundValue)
        {
            if (parentNode != null)
            {
                XmlAttribute attr = parentNode.Attributes[name];
                if (attr != null)
                    return attr.Value;
            }
            return notFoundValue;
        }

        private static XmlAttribute GetOrCreateAttribute(XmlNode parentNode, string name)
        {
            if (parentNode == null)
                return null;
            XmlAttribute attr = parentNode.Attributes[name];
            if (attr == null)
                attr = parentNode.Attributes.Append(parentNode.OwnerDocument.CreateAttribute(name));
            return attr;
        }

		public Hashtable SavedConnections
		{
			get 
			{ 
				if (_savedConnections == null) 
				{
					_savedConnections = new Hashtable();

                    XmlNodeList nodes = settingsRootNode.GetElementsByTagName("SavedSettings");
					foreach (XmlNode node in nodes) 
					{
						try 
						{
							ConnectionInfo inf = new ConnectionInfo(node);
							_savedConnections[inf.Name] = inf;
						}
						catch {}
					}
				}

				return _savedConnections; 
			}
		}
		#endregion

		#region Attributes on <DefaultSettings>
		public string Version
		{
			get
			{
                return GetAttribute(settingsRootNode, "Version", MISSING);
			}

			set
			{
                SetAttribute(settingsRootNode, "Version", value);
			}
		}

		public bool FirstLoad
		{
			get
			{
                return (GetAttribute(settingsRootNode, "FirstTime", "true") == "true");
			}

			set
			{
                SetAttribute(settingsRootNode, "FirstTime", (value == true) ? "true" : "false");
			}
		}
        #endregion

        public string FontFamily
        {
            get { return this.GetSetting("FontFamily",""); }
            set { this.SetSetting("FontFamily", value); }
        }

        public string DbDriver
        {
            get { return this.GetSetting("DbDriver","SQL").ToUpper(); }
            set { this.SetSetting("DbDriver", value); }
        }

		public string ConnectionString
		{
            get { return this.GetSetting("ConnectionString", "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Data Source=localhost"); }
			set	{ this.SetSetting("ConnectionString", value); }
		}

		public string LanguageMappingFile
		{
            get { return this.GetSetting("LanguageMappingFile", ApplicationPath + @"\Settings\Languages.xml"); }
			set	{ this.SetSetting("LanguageMappingFile", value); }
		}

		public string Language
		{
            get { return this.GetSetting("Language", "C#"); }
			set	{ this.SetSetting("Language", value); }
		}

		public string DbTargetMappingFile
		{
            get { return this.GetSetting("DbTargetMappingFile", ApplicationPath + @"\Settings\DbTargets.xml"); }
			set	{ this.SetSetting("DbTargetMappingFile", value); }
		}

		public string DbTarget
		{
            get { return this.GetSetting("DbTarget", "SqlClient"); }
			set	{ this.SetSetting("DbTarget", value); }
		}

        public string UserMetaDataFileName
		{
            get { return this.GetSetting("UserMetaDataFileName", UserDataPath + @"\Settings\UserMetaData.xml"); }
			set	{ this.SetSetting("UserMetaDataFileName", value); }
		}
		
		public bool EnableLineNumbering
		{
			get { return this.GetSetting("EnableLineNumbering",false); }
			set	{ this.SetSetting("EnableLineNumbering", value.ToString()); }
		}

		public bool EnableClipboard
		{
			get { return this.GetSetting("EnableClipboard",true); }
			set	{ this.SetSetting("EnableClipboard", value.ToString()); }
        }

        public int CodePage
        {
            get { return this.GetSetting("CodePage",-1); }
            set { this.SetSetting("CodePage", value.ToString()); }
        }

		public int Tabs
		{
			get { return this.GetSetting("Tabs",4); }
			set { this.SetSetting("Tabs", value.ToString()); }
		}

		public int ScriptTimeout
		{
			get { return this.GetSetting("ScriptTimeout",-1); }
			set { this.SetSetting("ScriptTimeout", value.ToString()); }
		}

        public string UserTemplateDirectory
        {
#if RUN_AS_NON_ADMIN        
            get
            {
                string result = this.GetSetting("UserTemplateDirectory");
                if ((MISSING == result) || (result == string.Empty))
                {
                    result = UserDataPath + @"\Templates\";
                    if (!Directory.Exists(result))
                        result = UserDataPath;

                    this.UserTemplateDirectory = result;
                }

                return result;
            }
            set { this.SetSetting("UserTemplateDirectory", value); }
#else
            get { return DefaultTemplateDirectory; }
            set { DefaultTemplateDirectory =value; }
#endif
        }

        public string DefaultTemplateDirectory
		{
			get 
            {
                string result = this.GetSetting("DefaultTemplateDirectory");
                if ((MISSING == result) || (result == string.Empty))
                {
                    result = ApplicationPath + @"\Templates\";
                    if (!Directory.Exists(result))
                        result = ApplicationPath;

                    this.DefaultTemplateDirectory = result;
                }

                return result; 
            }
			set	{ this.SetSetting("DefaultTemplateDirectory", value); }
		}

		public string DefaultOutputDirectory
		{
			get 
            {                 
                string result = this.GetSetting("DefaultOutputDirectory");
                if (MISSING == result)
                {
                    result = UserDataPath + @"\GeneratedCode\";
                    if (!Directory.Exists(result))
                        result = UserDataPath;

                    this.DefaultOutputDirectory = result;
                }

                return result;
            }
			set	{ this.SetSetting("DefaultOutputDirectory", value); }
		}

		public bool UseProxyServer
		{
			get 
			{ 
				return this.GetSetting("UseProxyServer",false); 
			}
			set	
			{ 
				this.SetSetting("UseProxyServer", value.ToString()); 
			}
		}

		public string ProxyServerUri
		{
			get 
			{
				return this.GetSetting("ProxyServerUri",string.Empty);
			}
			set	{ this.SetSetting("ProxyServerUri", value); }
		}

		public string ProxyAuthUsername
		{
			get 
			{
                return this.GetSetting("ProxyAuthUsername", string.Empty);
			}
			set	{ this.SetSetting("ProxyAuthUsername", value); }
		}

		public string ProxyAuthPassword
		{
			get 
			{
                return this.GetSetting("ProxyAuthPassword", string.Empty);
			}
			set	{ this.SetSetting("ProxyAuthPassword", value); }
		}

		public string ProxyAuthDomain
		{
			get 
			{
                return this.GetSetting("ProxyAuthDomain", string.Empty);
			}
			set	{ this.SetSetting("ProxyAuthDomain", value); }
		}

		public string WindowState
		{
            get { return this.GetSetting("WindowState", "Normal"); }
			set	{ this.SetSetting("WindowState", value); }
		}

		public string WindowPosTop
		{
            get { return this.GetSetting("WindowPosTop", "43"); }
			set	{ this.SetSetting("WindowPosTop", value); }
		}

		public string WindowPosLeft
		{
            get { return this.GetSetting("WindowPosLeft", "150"); }
			set	{ this.SetSetting("WindowPosLeft", value); }
		}

		public string WindowPosWidth
		{
            get { return this.GetSetting("WindowPosWidth", "502"); }
			set	{ this.SetSetting("WindowPosWidth", value); }
		}

		public string WindowPosHeight
		{
            get { return this.GetSetting("WindowPosHeight", "743"); }
			set	{ this.SetSetting("WindowPosHeight", value); }
		}

		public bool CheckForNewBuild
		{
			get 
			{ 
				return this.GetSetting("CheckForNewBuild", true);
			}

			set	{ this.SetSetting("CheckForNewBuild", value.ToString()); }
		}

		public bool CompactMemoryOnStartup
		{
			get 
			{ 
				return this.GetSetting("CompactMemoryOnStartup", true);
			}

			set	{ this.SetSetting("CompactMemoryOnStartup", value.ToString()); }
		}

		public bool DomainOverride
		{
			get 
			{ 
				// This is true by default
				return this.GetSetting("DomainOverride", true);
			}
			set	
			{ 
				this.SetSetting("DomainOverride", value.ToString()); 
			}
		}

        // k3b: to allow MyGen to run as a non admin it should *not* write into C:\Program Files\
        private string UserDataPath
        {
            get
            {
#if RUN_AS_NON_ADMIN
                if (_userDataPath == null) // Load OnDemand
                {
                    _userDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                            , "MyGeneration");

                    if (!Directory.Exists(_userDataPath))
                    {
                        Directory.CreateDirectory(_userDataPath);
                        Directory.CreateDirectory(Path.Combine(_userDataPath, "Settings"));
                        Directory.CreateDirectory(Path.Combine(_userDataPath, "Templates"));
                    }
                }
                return _userDataPath;
                    
#else
                return ApplicationPath; // not implemented yet. Feature disabled until then
#endif
            }
        }

        // .... where all stuff comes from that ships with MyGeneration
		private string ApplicationPath 
		{
			get 
			{
				if (_appPath == null) 
				{
					_appPath = Assembly.GetEntryAssembly().Location;
					_appPath = _appPath.Substring(0, _appPath.LastIndexOf(@"\"));
				}
				return _appPath;
			}
		}

		
		public void PopulateZeusContext(IZeusContext context) 
		{
			DefaultSettings settings = new DefaultSettings();
			IZeusInput input = context.Input;

			if (!input.Contains("__version"))
			{
				Assembly ver = System.Reflection.Assembly.GetEntryAssembly();
				input["__version"] = ver.GetName().Version.ToString();
			}
			
			//-- BEGIN LEGACY VARIABLE SUPPORT -----
			if (!input.Contains("defaultTemplatePath")) 
				input["defaultTemplatePath"] = settings.DefaultTemplateDirectory;
			if (!input.Contains("defaultOutputPath")) 
				input["defaultOutputPath"] = settings.DefaultOutputDirectory;
			//-- END LEGACY VARIABLE SUPPORT -------

			if (!input.Contains("__defaultTemplatePath")) 
				input["__defaultTemplatePath"] = settings.DefaultTemplateDirectory;

			if (!input.Contains("__defaultOutputPath")) 
				input["__defaultOutputPath"] = settings.DefaultOutputDirectory;

			if (settings.DbDriver != string.Empty) 
			{
				//-- BEGIN LEGACY VARIABLE SUPPORT -----
				if (!input.Contains("dbDriver")) 
					input["dbDriver"] = settings.DbDriver;
				if (!input.Contains("dbConnectionString")) 
					input["dbConnectionString"] = settings.DomainOverride;
				//-- END LEGACY VARIABLE SUPPORT -------

				if (!input.Contains("__dbDriver"))
					input["__dbDriver"] = settings.DbDriver;
				
				if (!input.Contains("__dbConnectionString"))
					input["__dbConnectionString"] = settings.ConnectionString;
			
				if (!input.Contains("__domainOverride"))
					input["__domainOverride"] = settings.DomainOverride;

				if ( (settings.DbTarget != string.Empty) && (!input.Contains("__dbTarget")) )
					input["__dbTarget"] = settings.DbTarget;
			
				if ( (settings.DbTargetMappingFile != string.Empty) && (!input.Contains("__dbTargetMappingFileName")) ) 
					input["__dbTargetMappingFileName"] = settings.DbTargetMappingFile;

				if ( (settings.LanguageMappingFile != string.Empty) && (!input.Contains("__dbLanguageMappingFileName")) )
					input["__dbLanguageMappingFileName"] = settings.LanguageMappingFile;

				if ( (settings.Language != string.Empty) && (!input.Contains("__language")) )
					input["__language"] = settings.Language;

				if ( (settings.UserMetaDataFileName != string.Empty) && (!input.Contains("__userMetaDataFileName")) )
					input["__userMetaDataFileName"] = settings.UserMetaDataFileName;
			}
		}

		#region Internal Stuff
		private string GetSetting(string name)
		{
			string xPath = @"Setting[@Name='" + name + "']";
            XmlNode node = settingsRootNode.SelectSingleNode(xPath, null);

            return GetAttribute(node,"value",MISSING);
		}

        public string GetSetting(string name, string notFoundValue)
        {
            string result = this.GetSetting(name);
	        if (result != MISSING) 
                return result;
            return notFoundValue;
        }

        public bool GetSetting(string name, bool notFoundValue)
        {
            bool result;
            if (bool.TryParse(this.GetSetting(name), out result))
                return result;
            return notFoundValue;
        }

        public int GetSetting(string name, int notFoundValue)
        {
            int result;
            if (int.TryParse(this.GetSetting(name), out result))
                return result;
            return notFoundValue;
        }

        public void SetSetting(string name, string data)
		{
			string xPath = @"Setting[@Name='" + name + "']";
            XmlNode node = settingsRootNode.SelectSingleNode(xPath, null);

			if(node != null)
			{
				node.Attributes["value"].Value = data;
			}
			else
			{
				AddSetting(name, data);
			}
		}

		private void AddSetting(string name, string data)
		{
            XmlNode node = settingsRootNode;

			if(node != null)
			{
				XmlAttribute attr;
				XmlNode setting = settingsRootNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Setting", null);

				attr = settingsRootNode.OwnerDocument.CreateAttribute("Name", null);
				attr.Value = name;
				setting.Attributes.Append(attr);

				attr = settingsRootNode.OwnerDocument.CreateAttribute("value", null);
				attr.Value = data;
				setting.Attributes.Append(attr);

				node.AppendChild(setting);
			}
		}
		#endregion


    }

	public class ConnectionInfo 
	{
		public string Name;
		public string Driver;
		public string ConnectionString;
		public string DbTarget;
		public string DbTargetPath;
		public string Language;
		public string LanguagePath;
		public string UserMetaDataPath;

		public ConnectionInfo() {}

		public ConnectionInfo(XmlNode node) 
		{
            
			Name = DefaultSettings.GetAttribute(node, "name", ""); 
			Driver = DefaultSettings.GetAttribute(node, "driver", ""); 
			ConnectionString = DefaultSettings.GetAttribute(node, "connstr", ""); 
			DbTargetPath = DefaultSettings.GetAttribute(node, "dbtargetpath", ""); 
			LanguagePath = DefaultSettings.GetAttribute(node, "languagepath", ""); 
			UserMetaDataPath = DefaultSettings.GetAttribute(node, "usermetapath", ""); 
			DbTarget = DefaultSettings.GetAttribute(node, "dbtarget", ""); 
			Language = DefaultSettings.GetAttribute(node, "language", ""); 
		}

		public override string ToString()
		{
			return this.Name;
		}

	}
}
