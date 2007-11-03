using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration.UI.Plugins.SqlTool
{
    public partial class SqlToolForm : DockContent, IMyGenDocument
    {
        private IMyGenerationMDI _mdi;
        private string _filename = string.Empty;

        public SqlToolForm(IMyGenerationMDI mdi)
        {
            this._mdi = mdi;

            InitializeComponent();

            sqlToolUserControl1.Initialize(mdi);
        }

        public void Open(string fileName)
        {
            this._filename = fileName;
            this.sqlToolUserControl1.Open(fileName);
            if (!string.IsNullOrEmpty(sqlToolUserControl1.FileName)) this.TabText = sqlToolUserControl1.FileName;
        }

        public void Save()
        {
            this.sqlToolUserControl1.Save();
            if (!string.IsNullOrEmpty(sqlToolUserControl1.FileName)) this.TabText = sqlToolUserControl1.FileName;
        }

        public void SaveAs()
        {
            this.sqlToolUserControl1.SaveAs();
            if (!string.IsNullOrEmpty(sqlToolUserControl1.FileName)) this.TabText = sqlToolUserControl1.FileName;
        }

        public void Execute()
        {
            this.sqlToolUserControl1.Execute();
        }

        protected override string GetPersistString()
        {
            if (!string.IsNullOrEmpty(_filename))
            {
                return GetType().ToString() + "," + this.sqlToolUserControl1.FileName;
            }
            else
            {
                return "type," + SqlToolEditorManager.SQL_FILE;
            }
        }

        private bool PromptForSave(bool allowPrevent)
        {
            bool canClose = true;

            if (sqlToolUserControl1.IsDirty)
            {
                DialogResult result;

                if (allowPrevent)
                {
                    result = MessageBox.Show("This file has been modified, Do you wish to save before closing?",
                        sqlToolUserControl1.FileName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                }
                else
                {
                    result = MessageBox.Show("This file has been modified, Do you wish to save before closing?",
                        sqlToolUserControl1.FileName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                switch (result)
                {
                    case DialogResult.Yes:
                        this.Save();
                        break;
                    case DialogResult.Cancel:
                        canClose = false;
                        break;
                }
            }

            return canClose;
        }

        private void SqlToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) ||
                (e.CloseReason == CloseReason.FormOwnerClosing))
            {
                if (!this.CanClose(true))
                {
                    e.Cancel = true;
                }
            }
        }

        #region IMyGenDocument Members

        public ToolStrip ToolStrip
        {
            get { return this.toolStripSqlTool; }
        }

        public string DocumentIndentity
        {
            get { return sqlToolUserControl1.DocumentIndentity; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //
        }

        public bool CanClose(bool allowPrevent)
        {
            return PromptForSave(allowPrevent);
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        public string TextContent
        {
            get { return this.sqlToolUserControl1.TextContent; }
        }

        #endregion

        #region Toolstrip Events
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            this.Execute();
        }
        #endregion

        #region Menu Events
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Execute();
        }
        #endregion

    }
}