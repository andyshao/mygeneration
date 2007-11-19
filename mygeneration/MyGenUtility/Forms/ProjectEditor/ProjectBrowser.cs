using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Zeus;
using Zeus.Projects;
using Zeus.Serializers;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;
using WeifenLuo.WinFormsUI.Docking;

namespace MyGeneration
{
    public partial class ProjectBrowser : DockContent, IMyGenDocument
    {
        private IMyGenerationMDI _mdi;

        public ProjectBrowser(IMyGenerationMDI mdi)
        {
            this._mdi = mdi;
            InitializeComponent();
        }

        protected override string GetPersistString()
        {
            return this.projectBrowserControl1.GetPersistString();
        }

        public bool CanClose(bool allowPrevent)
        {
            return projectBrowserControl1.CanClose(allowPrevent);
        }


        #region Load Project Tree
        public void CreateNewProject()
        {
            this.projectBrowserControl1.CreateNewProject();
        }

        public void LoadProject(string filename)
        {
            this.projectBrowserControl1.LoadProject(filename);
        }
        #endregion

        #region Main Menu Event Handlers
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Execute();
        }
        #endregion

        #region ToolStrip Event Handlers
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Save();
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {

            this.projectBrowserControl1.SaveAs();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Edit();
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            this.projectBrowserControl1.Execute();
        }
        #endregion

        #region IMyGenDocument Members

        public string DocumentIndentity
        {
            get
            {
                return this.projectBrowserControl1.DocumentIndentity;
            }
        }

        public ToolStrip ToolStrip
        {
            get { return this.toolStripOptions; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            //
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        public string TextContent
        {
            get { return this.projectBrowserControl1.CompleteFilePath; }
        }

        #endregion

        #region ProjectBrowser Event Handlers
        private void ProjectBrowser_MouseLeave(object sender, System.EventArgs e)
        {
            //this.toolTipProjectBrowser.SetToolTip(treeViewProject, string.Empty);
            this.projectBrowserControl1.SetToolTip(string.Empty);
        }

        void ProjectBrowser_FormClosing(object sender, FormClosingEventArgs e)
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
        #endregion
    }
}