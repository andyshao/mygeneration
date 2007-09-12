namespace MyGeneration
{
    partial class MyGenerationMDI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyGenerationMDI));
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.defaultSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButtonNew = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTemplateBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonMyMetaBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMyMetaProperties = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLangMappings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDbTargetMappings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLocalAliases = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGlobalAliases = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOptions = new System.Windows.Forms.ToolStripButton();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripMain
            // 
            this.statusStripMain.Location = new System.Drawing.Point(0, 544);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStripMain.Size = new System.Drawing.Size(792, 22);
            this.statusStripMain.TabIndex = 2;
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(792, 24);
            this.menuStripMain.TabIndex = 3;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.recentFilesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.defaultSettingsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.MergeIndex = 0;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.MergeIndex = 1;
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.recentFilesToolStripMenuItem.Text = "&Recent Files";
            this.recentFilesToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.MergeIndex = 12;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(159, 6);
            // 
            // defaultSettingsToolStripMenuItem
            // 
            this.defaultSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("defaultSettingsToolStripMenuItem.Image")));
            this.defaultSettingsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.defaultSettingsToolStripMenuItem.MergeIndex = 13;
            this.defaultSettingsToolStripMenuItem.Name = "defaultSettingsToolStripMenuItem";
            this.defaultSettingsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.defaultSettingsToolStripMenuItem.Text = "&Default Settings";
            this.defaultSettingsToolStripMenuItem.Click += new System.EventHandler(this.defaultSettingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.MergeIndex = 14;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(159, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.MergeIndex = 15;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.windowToolStripMenuItem.Text = "&Window";
            this.windowToolStripMenuItem.DropDownOpening += new System.EventHandler(this.windowToolStripMenuItem_DropDownOpening);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.MergeIndex = 8;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.AllowEndUserNestedDocking = false;
            this.dockPanel.AutoSize = true;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dockPanel.Location = new System.Drawing.Point(0, 49);
            this.dockPanel.Margin = new System.Windows.Forms.Padding(2);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(792, 495);
            this.dockPanel.TabIndex = 6;
            this.dockPanel.ActiveContentChanged += new System.EventHandler(this.dockPanel_ActiveContentChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonNew,
            this.toolStripButtonOpen,
            this.toolStripButtonTemplateBrowser,
            this.toolStripSeparator1,
            this.toolStripButtonMyMetaBrowser,
            this.toolStripButtonMyMetaProperties,
            this.toolStripButtonLangMappings,
            this.toolStripButtonDbTargetMappings,
            this.toolStripButtonLocalAliases,
            this.toolStripButtonGlobalAliases,
            this.toolStripSeparator2,
            this.toolStripButtonOptions});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStripNew";
            // 
            // toolStripDropDownButtonNew
            // 
            this.toolStripDropDownButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonNew.Image")));
            this.toolStripDropDownButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonNew.Name = "toolStripDropDownButtonNew";
            this.toolStripDropDownButtonNew.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonNew.Text = "New";
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpen.Text = "Open";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonTemplateBrowser
            // 
            this.toolStripButtonTemplateBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTemplateBrowser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTemplateBrowser.Image")));
            this.toolStripButtonTemplateBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTemplateBrowser.Name = "toolStripButtonTemplateBrowser";
            this.toolStripButtonTemplateBrowser.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTemplateBrowser.Text = "Template Browser";
            this.toolStripButtonTemplateBrowser.Click += new System.EventHandler(this.toolStripButtonTemplateBrowser_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonMyMetaBrowser
            // 
            this.toolStripButtonMyMetaBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMyMetaBrowser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMyMetaBrowser.Image")));
            this.toolStripButtonMyMetaBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMyMetaBrowser.Name = "toolStripButtonMyMetaBrowser";
            this.toolStripButtonMyMetaBrowser.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonMyMetaBrowser.Text = "MyMeta Browser";
            this.toolStripButtonMyMetaBrowser.Click += new System.EventHandler(this.toolStripButtonMyMetaBrowser_Click);
            // 
            // toolStripButtonMyMetaProperties
            // 
            this.toolStripButtonMyMetaProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMyMetaProperties.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMyMetaProperties.Image")));
            this.toolStripButtonMyMetaProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMyMetaProperties.Name = "toolStripButtonMyMetaProperties";
            this.toolStripButtonMyMetaProperties.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonMyMetaProperties.Text = "MyMeta Properties";
            this.toolStripButtonMyMetaProperties.Click += new System.EventHandler(this.toolStripButtonMyMetaProperties_Click);
            // 
            // toolStripButtonLangMappings
            // 
            this.toolStripButtonLangMappings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLangMappings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLangMappings.Image")));
            this.toolStripButtonLangMappings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLangMappings.Name = "toolStripButtonLangMappings";
            this.toolStripButtonLangMappings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLangMappings.Text = "Language Mappings";
            this.toolStripButtonLangMappings.Click += new System.EventHandler(this.toolStripButtonLangMappings_Click);
            // 
            // toolStripButtonDbTargetMappings
            // 
            this.toolStripButtonDbTargetMappings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDbTargetMappings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDbTargetMappings.Image")));
            this.toolStripButtonDbTargetMappings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDbTargetMappings.Name = "toolStripButtonDbTargetMappings";
            this.toolStripButtonDbTargetMappings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDbTargetMappings.Text = "DB Target Mappings";
            this.toolStripButtonDbTargetMappings.Click += new System.EventHandler(this.toolStripButtonDbTargetMappings_Click);
            // 
            // toolStripButtonLocalAliases
            // 
            this.toolStripButtonLocalAliases.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLocalAliases.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLocalAliases.Image")));
            this.toolStripButtonLocalAliases.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLocalAliases.Name = "toolStripButtonLocalAliases";
            this.toolStripButtonLocalAliases.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLocalAliases.Text = "Local MetaData Mappings";
            this.toolStripButtonLocalAliases.Click += new System.EventHandler(this.toolStripButtonLocalAliases_Click);
            // 
            // toolStripButtonGlobalAliases
            // 
            this.toolStripButtonGlobalAliases.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGlobalAliases.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGlobalAliases.Image")));
            this.toolStripButtonGlobalAliases.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGlobalAliases.Name = "toolStripButtonGlobalAliases";
            this.toolStripButtonGlobalAliases.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonGlobalAliases.Text = "Global MetaData Mappings";
            this.toolStripButtonGlobalAliases.Click += new System.EventHandler(this.toolStripButtonGlobalAliases_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonOptions
            // 
            this.toolStripButtonOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOptions.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOptions.Image")));
            this.toolStripButtonOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOptions.Name = "toolStripButtonOptions";
            this.toolStripButtonOptions.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOptions.Text = "Default Settings";
            this.toolStripButtonOptions.Click += new System.EventHandler(this.toolStripButtonOptions_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.MergeIndex = 7;
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.pluginsToolStripMenuItem.Text = "&Plugins";
            this.pluginsToolStripMenuItem.Visible = false;
            // 
            // MyGenerationMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MyGenerationMDI";
            this.Text = "MyGeneration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyGenerationMDI_FormClosing);
            this.Load += new System.EventHandler(this.MyGenerationMDI_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTemplateBrowser;
        private System.Windows.Forms.ToolStripButton toolStripButtonOptions;
        private System.Windows.Forms.ToolStripButton toolStripButtonMyMetaBrowser;
        private System.Windows.Forms.ToolStripButton toolStripButtonMyMetaProperties;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonNew;
        private System.Windows.Forms.ToolStripMenuItem defaultSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripButton toolStripButtonLangMappings;
        private System.Windows.Forms.ToolStripButton toolStripButtonDbTargetMappings;
        private System.Windows.Forms.ToolStripButton toolStripButtonLocalAliases;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonGlobalAliases;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
    }
}