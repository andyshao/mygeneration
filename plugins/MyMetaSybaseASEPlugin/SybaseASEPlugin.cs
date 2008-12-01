using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using MyMeta;

namespace MyMeta.Plugins
{
    public class SybaseASEPlugin : IMyMetaPlugin
	{

		private IMyMetaPluginContext context;

        public void Initialize(IMyMetaPluginContext context)
        {
            this.context = context;
        }

        public string ProviderName
        {
            get { return @"Sybase ASE"; }
        }

        public string ProviderUniqueKey
        {
            get { return @"SYBASEASE"; }
        }

        public string ProviderAuthorInfo
        {
            get { return @"Sybase ASE MyMeta Plugin Written by komma8komma1"; }
        }

        public Uri ProviderAuthorUri
        {
            get { return new Uri(@"http://www.mygenerationsoftware.com/"); }
        }

        public bool StripTrailingNulls
        {
            get { return false; }
        }

        public bool RequiredDatabaseName
        {
            get { return false; }
        }

        public string SampleConnectionString
        {
            get { return @"Provider=ASEOLEDB;Data Source=GREENMACHINE:5000;Catalog=pubs2;User Id=sa;Password=;"; }
        }

        public IDbConnection NewConnection
        {
            get
            {
                if (IsIntialized)
				{
                    OleDbConnection cn = new OleDbConnection(this.context.ConnectionString);
					return cn as IDbConnection;
				}
                else
                    return null;
            }
        }

        public string DefaultDatabase
        {
            get
            {
				return this.GetDatabaseName();
            }
        }

        public DataTable Databases
        {
            get
            {
                using (OleDbConnection cn = this.NewConnection as OleDbConnection)
                {
                    cn.Open();
                    DataTable dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs, new Object[] { null });
                    /*if (HasDefaultDefined)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["CATALOG_NAME"].ToString() != DefaultDatabase) 
                            {
                                row.Delete();
                            }
                        }
                        dt.AcceptChanges();
                    }*/
                    return dt;
                }
            }
        }

        public DataTable GetTables(string database)
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                DataTable dt1 = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { database, null, null, null });
                if (context.IncludeSystemEntities)
                {
                    DataTable dt2 = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { database, null, null, "SYSTEM TABLE" });
                    foreach (DataRow r in dt2.Rows) dt1.ImportRow(r);
                }

                return dt1;
            }
        }

        public DataTable GetViews(string database)
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Views, new Object[] { database, null, null });
            }
		}

        public DataTable GetProcedures(string database)
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, new Object[] { database, null, null });
            }
        }

        public DataTable GetDomains(string database)
        {
            return this.context.CreateDomainsDataTable();
        }

        public DataTable GetProcedureParameters(string database, string procedure)
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Procedure_Parameters, new Object[] { database, null, procedure });
            }
        }

        public DataTable GetProcedureResultColumns(string database, string procedure)
        {
            return this.context.CreateResultColumnsDataTable();
        }

        public DataTable GetViewColumns(string database, string view)
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                DataTable meta = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { database, null, view });
                SetDataTypes(cn, database, view, meta, true);
                return meta;
            }
        }

        public DataTable GetTableColumns(string database, string table)
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                DataTable meta = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new Object[] { database, null, table });
                SetDataTypes(cn, database, table, meta, false);//COLUMN_NAME
                return meta;
            }
        }

        public DataTable GetProviderTypes()
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, DefaultDatabase);
                return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Provider_Types, null);
            }
        }

        public List<string> GetPrimaryKeyColumns(string database, string table)
        {
            List<string> fieldNames = new List<string>();

            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                DataTable dt = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new Object[] { database, null, null, null, table });
                foreach (DataRow row in dt.Rows) 
                {
                    if (Convert.ToBoolean(row["PRIMARY_KEY"]))
                    {
                        fieldNames.Add(row["COLUMN_NAME"].ToString());
                    }
                }
            }

            return fieldNames;
        }

        public List<string> GetViewSubViews(string database, string view)
        {
            return new List<string>();
        }

        public List<string> GetViewSubTables(string database, string view)
        {
            return new List<string>();
        }

        public DataTable GetTableIndexes(string database, string table)
        {
            using (OleDbConnection cn = this.NewConnection as OleDbConnection)
            {
                InitDatabase(cn, database);
                return cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new Object[] { database, null, null, null, table });
            }
        }

        public DataTable GetForeignKeys(string database, string tableName)
        {
            return this.context.CreateForeignKeysDataTable();
            //DataTable metaData = this.context.CreateForeignKeysDataTable();
			/*IVistaDBDatabase db = null;

			try
			{
				metaData = context.CreateForeignKeysDataTable();

				db = DDA.OpenDatabase(this.GetFullDatabaseName(), 
					VistaDBDatabaseOpenMode.NonexclusiveReadOnly, "");

				ArrayList tables = db.EnumTables(); 

				foreach (string table in tables) 
				{
					IVistaDBTableSchema tblStructure = db.TableSchema(table);

					foreach (IVistaDBRelationshipInformation relInfo in tblStructure.ForeignKeys) 
					{ 
						if(relInfo.ForeignTable != tableName && relInfo.PrimaryTable != tableName)
							continue;

						string fCols = relInfo.ForeignKey; 
						string pCols = String.Empty; 

						string primaryTbl  = relInfo.PrimaryTable; 
						string pkName = "";

						using (IVistaDBTableSchema pkTableStruct = db.TableSchema(primaryTbl)) 
						{ 
							foreach (IVistaDBIndexInformation idxInfo in pkTableStruct.Indexes) 
							{ 
								if (!idxInfo.Primary) 
								continue; 
								        
								pkName = idxInfo.Name;
								pCols = idxInfo.KeyExpression; 
								break; 
							} 
						} 

						string [] fColumns = fCols.Split(';'); 
						string [] pColumns = pCols.Split(';'); 

						for(int i = 0; i < fColumns.GetLength(0); i++)
						{
							DataRow row = metaData.NewRow();
							metaData.Rows.Add(row);

							row["PK_TABLE_CATALOG"] = GetDatabaseName();
							row["PK_TABLE_SCHEMA"]  = DBNull.Value;
							row["FK_TABLE_CATALOG"] = DBNull.Value;
							row["FK_TABLE_SCHEMA"]  = DBNull.Value;
							row["FK_TABLE_NAME"]    = tblStructure.Name;
							row["PK_TABLE_NAME"]    = relInfo.PrimaryTable;
							row["ORDINAL"]          = 0;
							row["FK_NAME"]          = relInfo.Name;
							row["PK_NAME"]          = pkName;
							row["PK_COLUMN_NAME"]   = pColumns[i]; 
							row["FK_COLUMN_NAME"]   = fColumns[i];

							row["UPDATE_RULE"]		= relInfo.UpdateIntegrity;
							row["DELETE_RULE"]		= relInfo.DeleteIntegrity;
						}
					} 
				}
			}
			finally
			{
				if(db != null) db.Close();
			}*/

			//return metaData;
        }

        private void SetDataTypes(OleDbConnection conn, string database, string entityName, DataTable metaData, bool isView)
        {
            string sql = @"SELECT C.colid, C.name, C.usertype, C.scale, C.prec as precision, 
							C.length, CONVERT(bit,(C.status & 0x08)) as CanBeNull, T.name as datatype, M.text as RowDefault, CONVERT(bit,(C.status & 0x80)) as IsIdentity
					FROM syscolumns C, sysobjects O, systypes T, syscomments M  
					WHERE O.name='" + entityName + @"'
						AND O.type='" + (isView ? "V" : "U") + @"' 
						AND C.id = O.id 
						AND C.usertype*=T.usertype
						AND M.id=*C.cdefault
					ORDER by C.colid";

            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            OleDbDataAdapter adapt = new OleDbDataAdapter(cmd);
            DataTable extraInfo = new DataTable();
            adapt.Fill(extraInfo);

            string schema = null;
            Dictionary<string, DataRow> metaHash = new Dictionary<string, DataRow>();
            foreach (DataRow row in metaData.Rows)
            {
                if (schema == null) schema = row["TABLE_SCHEMA"].ToString();
                metaHash.Add(row["COLUMN_NAME"].ToString(), row);
            }
            if (schema == null) schema = "dbo";

            if (!metaData.Columns.Contains("TYPE_NAME")) metaData.Columns.Add("TYPE_NAME");
            if (!metaData.Columns.Contains("TYPE_NAME_COMPLETE")) metaData.Columns.Add("TYPE_NAME_COMPLETE");
            if (!metaData.Columns.Contains("IS_AUTO_KEY"))
            {
                DataColumn col = new DataColumn("IS_AUTO_KEY", typeof(bool));
                col.DefaultValue = false;
                metaData.Columns.Add(col);
            }
            foreach (DataRow extraRow in extraInfo.Rows)
            {
                string colName = extraRow["name"].ToString();

                /*
                C.name, C.usertype, C.scale, C.prec as precision, 
                C.length, CONVERT(bit,(C.status & 0x08)) as CanBeNull, 
                T.name as datatype, M.text as RowDefault, CONVERT(bit,(C.status & 0x80)) as IsIdentity
                */

                string dataType = extraRow["datatype"].ToString();
                int usertype = Convert.ToInt32(extraRow["usertype"]);

                int? length = null, scale = null, precision = null;
                if (extraRow["length"] != DBNull.Value) length = Convert.ToInt32(extraRow["length"]);
                if (extraRow["scale"] != DBNull.Value) scale = Convert.ToInt32(extraRow["scale"]);
                if (extraRow["precision"] != DBNull.Value) precision = Convert.ToInt32(extraRow["precision"]);

                bool canBeNull = Convert.ToBoolean(extraRow["CanBeNull"]);
                string rowDefault = extraRow["RowDefault"].ToString();
                bool isIdentity = Convert.ToBoolean(extraRow["IsIdentity"]);
                bool hasDefault = (extraRow["RowDefault"] == DBNull.Value);
                int ordinal = Convert.ToInt32(extraRow["colid"]);

                DataRow row;
                if (metaHash.ContainsKey(colName))
                {
                    row = metaHash[colName];
                }
                else
                {
                    row = metaData.NewRow();

                    row["TABLE_SCHEMA"] = schema;
                    row["TABLE_NAME"] = dataType;
                    row["COLUMN_NAME"] = colName;
                    row["ORDINAL_POSITION"] = ordinal;
                    row["IS_NULLABLE"] = canBeNull;
                    row["DATA_TYPE"] = usertype;
                    if (length.HasValue) row["CHARACTER_MAXIMUM_LENGTH"] = length;
                    if (length.HasValue) row["CHARACTER_OCTET_LENGTH"] = length;
                    if (scale.HasValue) row["NUMERIC_SCALE"] = scale;
                    if (precision.HasValue) row["NUMERIC_PRECISION"] = precision;

                    metaData.Rows.Add(row);
                }

                row["TYPE_NAME"] = dataType;
                row["TYPE_NAME_COMPLETE"] = dataType;
                row["COLUMN_HASDEFAULT"] = hasDefault;
                row["COLUMN_DEFAULT"] = rowDefault;
                row["IS_AUTO_KEY"] = hasDefault;
            }
        }


        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }

        private bool IsIntialized { get { return (context != null); } }

		private string GetDatabaseName()
		{
            string dbname = string.Empty;
            string connstr = this.context.ConnectionString;
            int len = 8, idx = connstr.IndexOf("Catalog=", StringComparison.CurrentCultureIgnoreCase);
            if (idx < 0) 
            {
                len = 9;
                idx = connstr.IndexOf("Catalog =", StringComparison.CurrentCultureIgnoreCase);
            }

            if (idx >= 0) 
            {
                dbname = connstr.Substring(idx + len);
                idx = dbname.IndexOf(";", StringComparison.CurrentCultureIgnoreCase);
                if (idx > 0) dbname = dbname.Substring(0, idx);
            }
            return dbname;
		}

        private bool HasDefaultDefined
        {
            get
            {
                if ((this.context.ConnectionString.IndexOf("Catalog=", StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (this.context.ConnectionString.IndexOf("Catalog = ", StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    return true;
                }
                return false;
            }
        }

        private void InitDatabase(OleDbConnection connection, string dbName)
        {
            if (string.IsNullOrEmpty(dbName)) return;

            string newconnstr, connstr = connection.ConnectionString;
            int len = 8, idx = connstr.IndexOf("Catalog=", StringComparison.CurrentCultureIgnoreCase);
            if (idx < 0)
            {
                len = 9;
                idx = connstr.IndexOf("Catalog =", StringComparison.CurrentCultureIgnoreCase);
            }

            if (idx < 0)
            {
                newconnstr = connstr;
                if (!connstr.EndsWith(";")) newconnstr += ";";
                newconnstr += "Catalog=" + dbName;
            }
            else
            {
                idx += len;
                newconnstr = connstr.Substring(0, idx) + dbName;
                
                int end = connstr.IndexOf(";", idx);
                if (end > 0) newconnstr += connstr.Substring(end);
            }

            connection.ConnectionString = newconnstr;
            connection.Open();
            OleDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = "use [" + dbName + "];";
            cmd.ExecuteNonQuery();
        }
	}
}
