﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MyMeta;

namespace MyMetaPluginTestTool
{
    public partial class MyMetaPluginUtility : Form
    {
        private FileInfo _cfgFile = null;

        public MyMetaPluginUtility()
        {
            InitializeComponent();
        }

        public FileInfo ConfigFile
        {
            get
            {
                if (_cfgFile == null)
                {
                    _cfgFile = new FileInfo(Assembly.GetEntryAssembly().Location + ".cfg");
                }
                return _cfgFile;
            }
        }

        private void MyMetaPluginUtility_Load(object sender, EventArgs e)
        {
            //dbRoot root = new dbRoot();
            foreach (IMyMetaPlugin plugin in dbRoot.Plugins.Values)
            {
                IMyMetaPlugin pluginTest = dbRoot.Plugins[plugin.ProviderUniqueKey] as IMyMetaPlugin;
                if (pluginTest == plugin)
                {
                    this.comboBoxPlugins.Items.Add(plugin.ProviderUniqueKey);
                }
            }

            if (ConfigFile.Exists)
            {
                string[] lines = File.ReadAllLines(ConfigFile.FullName);
                if (lines.Length > 1)
                {
                    int idx = this.comboBoxPlugins.FindStringExact(lines[0]); 
                    if (idx >= 0) this.comboBoxPlugins.SelectedIndex = idx;
                    this.textBoxConnectionString.Text = lines[1];
                }
            }
        }

        private void comboBoxPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxPlugins.SelectedItem != null)
            {
                IMyMetaPlugin plugin = dbRoot.Plugins[this.comboBoxPlugins.SelectedItem.ToString()] as IMyMetaPlugin;
                if (plugin != null)
                {
                    this.textBoxConnectionString.Text = plugin.SampleConnectionString;
                }
            }
        }

        private void MyMetaPluginUtility_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(this.comboBoxPlugins.SelectedItem.ToString());
                sb.AppendLine(this.textBoxConnectionString.Text);

                File.WriteAllText(ConfigFile.FullName, sb.ToString());
            }
            catch {}
        }


        private void buttonTest_Click(object sender, EventArgs e)
        {
            bool hasErroredOut = false;
            this.textBoxResults.Clear();
            IMyMetaPlugin plugin = null;
            try
            {
                plugin = dbRoot.Plugins[this.comboBoxPlugins.SelectedItem.ToString()] as IMyMetaPlugin;

                IMyMetaPluginContext context = new MyMetaPluginContext(plugin.ProviderUniqueKey, this.textBoxConnectionString.Text);

                plugin.Initialize(context);
                using (IDbConnection conn = plugin.NewConnection)
                {
                    conn.Open();
                    conn.Close();
                }
                this.textBoxConnectionString.BackColor = Color.LightGreen;
                this.AppendLog("Connection Test Successful.");
            }
            catch (Exception ex)
            {
                hasErroredOut = true;
                this.textBoxConnectionString.BackColor = Color.Red;
                this.AppendLog("Error testing connection", ex);
            }

            if (!hasErroredOut)
            {
                Application.DoEvents();

                dbRoot root = new dbRoot();

                try
                {
                    root.Connect(this.comboBoxPlugins.SelectedItem.ToString(), textBoxConnectionString.Text);
                    this.AppendLog("MyMeta dbRoot Connection Successful.");
                }
                catch (Exception ex)
                {
                    hasErroredOut = true;
                    this.AppendLog("Error connecting to dbRoot", ex);
                }

                TestDatabases(plugin, root, ref hasErroredOut);
                TestTables(plugin, root, ref hasErroredOut);
                TestViews(plugin, root, ref hasErroredOut);
                TestProcedures(plugin, root, ref hasErroredOut);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="root"></param>
        /// <param name="hasErroredOut"></param>
        private void TestDatabases(IMyMetaPlugin plugin, dbRoot root, ref bool hasErroredOut)
        {
            string garbage;

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                try
                {
                    DataTable dt = plugin.Databases;

                    this.AppendLog(dt.Rows.Count + " databases found through Plugin.");

                }
                catch (Exception ex)
                {
                    hasErroredOut = true;
                    this.AppendLog("Plugin Databases Error", ex);
                }
            }

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                try
                {
                    foreach (IDatabase db in root.Databases)
                    {
                        garbage = db.Name;
                        garbage = db.SchemaName;
                        garbage = db.SchemaOwner;
                    }

                    this.AppendLog(root.Databases.Count + " databases traversed successfully through MyMeta.");
                }
                catch (Exception ex)
                {
                    hasErroredOut = true;
                    this.AppendLog("Error traversing databases through MyMeta", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="root"></param>
        /// <param name="hasErroredOut"></param>
        private void TestTables(IMyMetaPlugin plugin, dbRoot root, ref bool hasErroredOut)
        {
            string garbage;

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        DataTable dt = plugin.GetTables(db.Name);
                        this.AppendLog(dt.Rows.Count + " tables in database " + db.Name + " found through Plugin.");
                    }
                    catch (Exception ex)
                    {
                        hasErroredOut = true;
                        this.AppendLog("Plugin tables error in database " + db.Name, ex);
                    }
                }
            }

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        foreach (ITable tbl in db.Tables)
                        {

                            garbage = tbl.Name;
                            garbage = tbl.Schema;
                            garbage = tbl.DateCreated.ToString();
                            garbage = tbl.DateModified.ToString();
                            garbage = tbl.Description;
                            garbage = tbl.Guid.ToString();
                            garbage = tbl.PropID.ToString();
                            garbage = tbl.Type;
                        }

                        this.AppendLog(db.Tables.Count + " tables in database " + db.Name + " traversed successfully through MyMeta.");

                    }
                    catch (Exception ex)
                    {
                        hasErroredOut = true;
                        this.AppendLog("Error traversing tables in database " + db.Name + " through MyMeta", ex);
                    }
                }
            }


            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    foreach (ITable table in db.Tables)
                    {
                        try
                        {
                            DataTable dt = plugin.GetTableColumns(db.Name, table.Name);
                            this.AppendLog(dt.Rows.Count + " columns in table " + db.Name + "." + table.Name + " found through Plugin.");
                        }
                        catch (Exception ex)
                        {
                            hasErroredOut = true;
                            this.AppendLog("Plugin table column error in " + db.Name + "." + table.Name, ex);
                        }
                    }
                }
            }

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    foreach (ITable table in db.Tables)
                    {

                        try
                        {
                            foreach (IColumn column in table.Columns)
                            {
                                garbage = column.AutoKeyIncrement.ToString();
                                garbage = column.AutoKeySeed.ToString();
                                garbage = column.CharacterMaxLength.ToString();
                                garbage = column.CharacterOctetLength.ToString();
                                garbage = column.CharacterSetCatalog;
                                garbage = column.CharacterSetName;
                                garbage = column.CharacterSetSchema;
                                garbage = column.CompFlags.ToString();
                                garbage = column.DataType.ToString();
                                garbage = column.DataTypeName;
                                garbage = column.DataTypeNameComplete;
                                garbage = column.DateTimePrecision.ToString();
                                garbage = column.DbTargetType;
                                garbage = column.Default;
                                garbage = column.Description;
                                garbage = column.DomainCatalog;
                                garbage = column.DomainName;
                                garbage = column.DomainSchema;
                                garbage = column.Flags.ToString();
                                garbage = column.Guid.ToString();
                                garbage = column.HasDefault.ToString();
                                garbage = column.HasDomain.ToString();
                                garbage = column.IsAutoKey.ToString();
                                garbage = column.IsComputed.ToString();
                                garbage = column.IsInForeignKey.ToString();
                                garbage = column.IsInPrimaryKey.ToString();
                                garbage = column.IsNullable.ToString();
                                garbage = column.LanguageType;
                                garbage = column.LCID.ToString();
                                garbage = column.Name;
                                garbage = column.NumericPrecision.ToString();
                                garbage = column.NumericScale.ToString();
                                garbage = column.Ordinal.ToString();
                                garbage = column.PropID.ToString();
                                garbage = column.SortID.ToString();
                                //garbage = column.TDSCollation.ToString(); -- Null means empty?
                                garbage = column.TypeGuid.ToString();
                            }
                            this.AppendLog(table.Columns.Count + " table columns in database " + db.Name + "." + table.Name + " traversed successfully through MyMeta.");
                        }
                        catch (Exception ex)
                        {
                            hasErroredOut = true;
                            this.AppendLog("Error traversing table columns in " + db.Name + "." + table.Name + " through MyMeta", ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="root"></param>
        /// <param name="hasErroredOut"></param>
        private void TestViews(IMyMetaPlugin plugin, dbRoot root, ref bool hasErroredOut)
        {
            string garbage;

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        DataTable dt = plugin.GetViews(db.Name);
                        this.AppendLog(dt.Rows.Count + " views in database " + db.Name + " found through Plugin.");
                    }
                    catch (Exception ex)
                    {
                        hasErroredOut = true;
                        this.AppendLog("Plugin views error in database " + db.Name, ex);
                    }
                }
            }

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        foreach (IView view in db.Views)
                        {

                            garbage = view.Name;
                            garbage = view.Schema;
                            garbage = view.DateCreated.ToString();
                            garbage = view.DateModified.ToString();
                            garbage = view.Description;
                            garbage = view.Guid.ToString();
                            garbage = view.PropID.ToString();
                            garbage = view.Type;
                            garbage = view.IsUpdateable.ToString();
                            garbage = view.CheckOption.ToString();
                            garbage = view.ViewText;
                        }

                        this.AppendLog(db.Views.Count + " views in database " + db.Name + " traversed successfully through MyMeta.");

                    }
                    catch (Exception ex)
                    {
                        hasErroredOut = true;
                        this.AppendLog("Error traversing views in database " + db.Name + " through MyMeta", ex);
                    }
                }
            }

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    foreach (IView view in db.Views)
                    {
                        try
                        {
                            DataTable dt = plugin.GetViewColumns(db.Name, view.Name);
                            this.AppendLog(dt.Rows.Count + " columns in view " + db.Name + "." + view.Name + " found through Plugin.");
                        }
                        catch (Exception ex)
                        {
                            hasErroredOut = true;
                            this.AppendLog("Plugin view column error in " + db.Name + "." + view.Name, ex);
                        }
                    }
                }
            }

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    foreach (IView view in db.Views)
                    {

                        try
                        {
                            foreach (IColumn column in view.Columns)
                            {
                                garbage = column.AutoKeyIncrement.ToString();
                                garbage = column.AutoKeySeed.ToString();
                                garbage = column.CharacterMaxLength.ToString();
                                garbage = column.CharacterOctetLength.ToString();
                                garbage = column.CharacterSetCatalog;
                                garbage = column.CharacterSetName;
                                garbage = column.CharacterSetSchema;
                                garbage = column.CompFlags.ToString();
                                garbage = column.DataType.ToString();
                                garbage = column.DataTypeName;
                                garbage = column.DataTypeNameComplete;
                                garbage = column.DateTimePrecision.ToString();
                                garbage = column.DbTargetType;
                                garbage = column.Default;
                                garbage = column.Description;
                                garbage = column.DomainCatalog;
                                garbage = column.DomainName;
                                garbage = column.DomainSchema;
                                garbage = column.Flags.ToString();
                                garbage = column.Guid.ToString();
                                garbage = column.HasDefault.ToString();
                                garbage = column.HasDomain.ToString();
                                garbage = column.IsAutoKey.ToString();
                                garbage = column.IsComputed.ToString();
                                garbage = column.IsInForeignKey.ToString();
                                garbage = column.IsInPrimaryKey.ToString();
                                garbage = column.IsNullable.ToString();
                                garbage = column.LanguageType;
                                garbage = column.LCID.ToString();
                                garbage = column.Name;
                                garbage = column.NumericPrecision.ToString();
                                garbage = column.NumericScale.ToString();
                                garbage = column.Ordinal.ToString();
                                garbage = column.PropID.ToString();
                                garbage = column.SortID.ToString();
                                //garbage = column.TDSCollation.ToString(); -- Null means empty?
                                garbage = column.TypeGuid.ToString();
                            }
                            this.AppendLog(view.Columns.Count + " view columns in database " + db.Name + "." + view.Name + " traversed successfully through MyMeta.");
                        }
                        catch (Exception ex)
                        {
                            hasErroredOut = true;
                            this.AppendLog("Error traversing view columns in " + db.Name + "." + view.Name + " through MyMeta", ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="root"></param>
        /// <param name="hasErroredOut"></param>
        private void TestProcedures(IMyMetaPlugin plugin, dbRoot root, ref bool hasErroredOut)
        {
            string garbage;

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        DataTable dt = plugin.GetProcedures(db.Name);
                        this.AppendLog(dt.Rows.Count + " procedures in database " + db.Name + " found through Plugin.");
                    }
                    catch (Exception ex)
                    {
                        hasErroredOut = true;
                        this.AppendLog("Plugin procedures error in database " + db.Name, ex);
                    }
                }
            }

            //--------------------------------------------------------
            if (!hasErroredOut)
            {
                foreach (IDatabase db in root.Databases)
                {
                    try
                    {
                        foreach (IProcedure procedure in db.Procedures)
                        {

                            garbage = procedure.Name;
                            garbage = procedure.Schema;
                            garbage = procedure.DateCreated.ToString();
                            garbage = procedure.DateModified.ToString();
                            garbage = procedure.Description;
                            garbage = procedure.ProcedureText;
                            garbage = procedure.Type.ToString();
                        }

                        this.AppendLog(db.Procedures.Count + " procedures in database " + db.Name + " traversed successfully through MyMeta.");

                    }
                    catch (Exception ex)
                    {
                        hasErroredOut = true;
                        this.AppendLog("Error traversing procedures in database " + db.Name + " through MyMeta", ex);
                    }
                }
            }
        }

        private void AppendLog(string message)
        {
            this.textBoxResults.AppendText(DateTime.Now.ToString());
            this.textBoxResults.AppendText(" - " + message);
            this.textBoxResults.AppendText(Environment.NewLine);
        }

        private void AppendLog(string message, Exception ex)
        {
            this.textBoxResults.AppendText(DateTime.Now.ToString());
            this.textBoxResults.AppendText(" - " + message);
            this.textBoxResults.AppendText(": " + ex.Message + ex);
            this.textBoxResults.AppendText(Environment.NewLine);
        }
    }
}
