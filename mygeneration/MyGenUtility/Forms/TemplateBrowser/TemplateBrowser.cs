using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Zeus;
using Zeus.UserInterface;
using Zeus.UserInterface.WinForms;
using MyMeta;

namespace MyGeneration
{
    public partial class TemplateBrowser : DockContent, IMyGenContent
    {
        private IMyGenerationMDI _mdi;
        private ZeusProcessStatusDelegate _executionCallback;

        public TemplateBrowser(IMyGenerationMDI mdi)
        {
            this._mdi = mdi;
            this._executionCallback = new ZeusProcessStatusDelegate(ExecutionCallback);
            this.DockPanel = mdi.DockPanel;

            InitializeComponent();

            this.templateBrowserControl.Initialize();
            if (DefaultSettings.Instance.ExecuteFromTemplateBrowserAsync)
            {
                this.templateBrowserControl.ExecuteTemplateOverride = new ExecuteTemplateDelegate(ExecuteTemplateOverride);
            }
        }

        private bool ExecuteTemplateOverride(TemplateOperations operation, ZeusTemplate template, ZeusSavedInput input, ShowGUIEventHandler guiEventHandler)
        {
            switch (operation)
            {
                case TemplateOperations.Execute:
                    ZeusProcessManager.ExecuteTemplate(template.FullFileName, _executionCallback);
                    break;
                case TemplateOperations.ExecuteLoadedInput:
                    ZeusProcessManager.ExecuteSavedInput(input.FilePath, _executionCallback);
                    break;
                case TemplateOperations.SaveInput:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Zues Input Files (*.zinp)|*.zinp";
                    saveFileDialog.FilterIndex = 0;
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ZeusProcessManager.RecordTemplateInput(template.FullFileName, saveFileDialog.FileName, _executionCallback);
                    }
                    break;
            }
            return true;
        }

        private void ExecutionCallback(ZeusProcessStatusEventArgs args)
        {
            if (args.Message != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(_executionCallback, args);
                }
                else
                {
                    this._mdi.WriteConsole(args.Message);
                }
            }
        }

        private void templateBrowserControl_ExecutionStatusUpdate(bool isRunning, string message)
        {
            if (!this._mdi.Console.DockContent.Visible) this._mdi.Console.DockContent.Show(_mdi.DockPanel);
            this._mdi.WriteConsole(message);
        }

        private void templateBrowserControl_ErrorsOccurred(object sender, EventArgs e)
        {
            if (sender is Exception)
            {
                this._mdi.ErrorsOccurred(sender as Exception);
            }
        }

        private void templateBrowserControl_TemplateOpen(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this._mdi.OpenDocuments(sender.ToString());
            }
        }

        private void templateBrowserControl_TemplateUpdate(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this._mdi.SendAlert(this, "UpdateTemplate", sender.ToString());
            }
        }

        private void templateBrowserControl_TemplateDelete(object sender, EventArgs e)
        {
            if (sender != null)
            {
                this._mdi.SendAlert(this, "DeleteTemplate", sender.ToString());
            }
        }

        #region IMyGenContent Members

        public ToolStrip ToolStrip
        {
            get { return null; }
        }

        public void ProcessAlert(IMyGenContent sender, string command, params object[] args)
        {
            DefaultSettings settings = DefaultSettings.Instance;
            if (command.Equals("UpdateDefaultSettings", StringComparison.CurrentCultureIgnoreCase))
            {
                bool doRefresh = false;

                if (DefaultSettings.Instance.ExecuteFromTemplateBrowserAsync)
                    this.templateBrowserControl.ExecuteTemplateOverride = new ExecuteTemplateDelegate(ExecuteTemplateOverride);
                else
                    this.templateBrowserControl.ExecuteTemplateOverride = null;

                try
                {
                    if (this.templateBrowserControl.TreeBuilder.DefaultTemplatePath != settings.DefaultTemplateDirectory)
                        doRefresh = true;
                }
                catch
                {
                    doRefresh = true;
                }

                if (doRefresh)
                    templateBrowserControl.RefreshTree();
            }
        }

        public bool CanClose(bool allowPrevent)
        {
            return true;
        }

        public DockContent DockContent
        {
            get { return this; }
        }

        #endregion
    }
}