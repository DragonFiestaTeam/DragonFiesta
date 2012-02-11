namespace MapleShark
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mMenu = new System.Windows.Forms.MenuStrip();
            this.mFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mFileImportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mFileOpenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mFileSeparatorMenu = new System.Windows.Forms.ToolStripSeparator();
            this.mFileQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewSearchMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewDataMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewStructureMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewPropertiesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mTimer = new System.Windows.Forms.Timer(this.components);
            this.mImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.mOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.mDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.mMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMenu
            // 
            this.mMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFileMenu,
            this.mViewMenu});
            this.mMenu.Location = new System.Drawing.Point(0, 0);
            this.mMenu.Name = "mMenu";
            this.mMenu.Size = new System.Drawing.Size(947, 24);
            this.mMenu.TabIndex = 1;
            // 
            // mFileMenu
            // 
            this.mFileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFileImportMenu,
            this.mFileOpenMenu,
            this.mFileSeparatorMenu,
            this.mFileQuit});
            this.mFileMenu.Name = "mFileMenu";
            this.mFileMenu.Size = new System.Drawing.Size(37, 20);
            this.mFileMenu.Text = "&File";
            // 
            // mFileImportMenu
            // 
            this.mFileImportMenu.Name = "mFileImportMenu";
            this.mFileImportMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mFileImportMenu.Size = new System.Drawing.Size(147, 22);
            this.mFileImportMenu.Text = "&Import";
            this.mFileImportMenu.Click += new System.EventHandler(this.mFileImportMenu_Click);
            // 
            // mFileOpenMenu
            // 
            this.mFileOpenMenu.Name = "mFileOpenMenu";
            this.mFileOpenMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mFileOpenMenu.Size = new System.Drawing.Size(147, 22);
            this.mFileOpenMenu.Text = "&Open";
            this.mFileOpenMenu.Click += new System.EventHandler(this.mFileOpenMenu_Click);
            // 
            // mFileSeparatorMenu
            // 
            this.mFileSeparatorMenu.Name = "mFileSeparatorMenu";
            this.mFileSeparatorMenu.Size = new System.Drawing.Size(144, 6);
            // 
            // mFileQuit
            // 
            this.mFileQuit.Name = "mFileQuit";
            this.mFileQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mFileQuit.Size = new System.Drawing.Size(147, 22);
            this.mFileQuit.Text = "&Quit";
            this.mFileQuit.Click += new System.EventHandler(this.mFileQuit_Click);
            // 
            // mViewMenu
            // 
            this.mViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mViewSearchMenu,
            this.mViewDataMenu,
            this.mViewStructureMenu,
            this.mViewPropertiesMenu});
            this.mViewMenu.Name = "mViewMenu";
            this.mViewMenu.Size = new System.Drawing.Size(44, 20);
            this.mViewMenu.Text = "&View";
            this.mViewMenu.DropDownOpening += new System.EventHandler(this.mViewMenu_DropDownOpening);
            // 
            // mViewSearchMenu
            // 
            this.mViewSearchMenu.Checked = true;
            this.mViewSearchMenu.CheckOnClick = true;
            this.mViewSearchMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewSearchMenu.Name = "mViewSearchMenu";
            this.mViewSearchMenu.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.mViewSearchMenu.Size = new System.Drawing.Size(152, 22);
            this.mViewSearchMenu.Text = "Sea&rch";
            this.mViewSearchMenu.CheckedChanged += new System.EventHandler(this.mViewSearchMenu_CheckedChanged);
            // 
            // mViewDataMenu
            // 
            this.mViewDataMenu.Checked = true;
            this.mViewDataMenu.CheckOnClick = true;
            this.mViewDataMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewDataMenu.Name = "mViewDataMenu";
            this.mViewDataMenu.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.mViewDataMenu.Size = new System.Drawing.Size(152, 22);
            this.mViewDataMenu.Text = "&Data";
            this.mViewDataMenu.CheckedChanged += new System.EventHandler(this.mViewDataMenu_CheckedChanged);
            // 
            // mViewStructureMenu
            // 
            this.mViewStructureMenu.Checked = true;
            this.mViewStructureMenu.CheckOnClick = true;
            this.mViewStructureMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewStructureMenu.Name = "mViewStructureMenu";
            this.mViewStructureMenu.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.mViewStructureMenu.Size = new System.Drawing.Size(152, 22);
            this.mViewStructureMenu.Text = "&Structure";
            this.mViewStructureMenu.CheckedChanged += new System.EventHandler(this.mViewStructureMenu_CheckedChanged);
            // 
            // mViewPropertiesMenu
            // 
            this.mViewPropertiesMenu.Checked = true;
            this.mViewPropertiesMenu.CheckOnClick = true;
            this.mViewPropertiesMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewPropertiesMenu.Name = "mViewPropertiesMenu";
            this.mViewPropertiesMenu.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.mViewPropertiesMenu.Size = new System.Drawing.Size(152, 22);
            this.mViewPropertiesMenu.Text = "&Properties";
            this.mViewPropertiesMenu.CheckedChanged += new System.EventHandler(this.mViewPropertiesMenu_CheckedChanged);
            // 
            // mTimer
            // 
            this.mTimer.Tick += new System.EventHandler(this.mTimer_Tick);
            // 
            // mImportDialog
            // 
            this.mImportDialog.Filter = "PCap Files|*.pcap";
            this.mImportDialog.ReadOnlyChecked = true;
            this.mImportDialog.RestoreDirectory = true;
            this.mImportDialog.Title = "Import";
            // 
            // mOpenDialog
            // 
            this.mOpenDialog.Filter = "MapleShark Binary Files|*.msb";
            this.mOpenDialog.Multiselect = true;
            this.mOpenDialog.ReadOnlyChecked = true;
            this.mOpenDialog.RestoreDirectory = true;
            this.mOpenDialog.Title = "Open";
            // 
            // mDockPanel
            // 
            this.mDockPanel.ActiveAutoHideContent = null;
            this.mDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mDockPanel.DockBackColor = System.Drawing.SystemColors.ControlDark;
            this.mDockPanel.DockBottomPortion = 0.3;
            this.mDockPanel.DockLeftPortion = 0.3;
            this.mDockPanel.DockRightPortion = 0.3;
            this.mDockPanel.DockTopPortion = 75;
            this.mDockPanel.Location = new System.Drawing.Point(0, 24);
            this.mDockPanel.Name = "mDockPanel";
            this.mDockPanel.Size = new System.Drawing.Size(947, 449);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.mDockPanel.Skin = dockPanelSkin1;
            this.mDockPanel.TabIndex = 4;
            this.mDockPanel.ActiveDocumentChanged += new System.EventHandler(this.mDockPanel_ActiveDocumentChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 473);
            this.Controls.Add(this.mDockPanel);
            this.Controls.Add(this.mMenu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FiestaShark 1.0.1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.mMenu.ResumeLayout(false);
            this.mMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mMenu;
        private System.Windows.Forms.ToolStripMenuItem mFileMenu;
        private System.Windows.Forms.ToolStripMenuItem mFileImportMenu;
        private System.Windows.Forms.ToolStripMenuItem mFileOpenMenu;
        private System.Windows.Forms.Timer mTimer;
        private System.Windows.Forms.OpenFileDialog mImportDialog;
        private System.Windows.Forms.OpenFileDialog mOpenDialog;
        private WeifenLuo.WinFormsUI.Docking.DockPanel mDockPanel;
        private System.Windows.Forms.ToolStripMenuItem mViewMenu;
        private System.Windows.Forms.ToolStripMenuItem mViewPropertiesMenu;
        private System.Windows.Forms.ToolStripMenuItem mViewStructureMenu;
        private System.Windows.Forms.ToolStripSeparator mFileSeparatorMenu;
        private System.Windows.Forms.ToolStripMenuItem mFileQuit;
        private System.Windows.Forms.ToolStripMenuItem mViewDataMenu;
        private System.Windows.Forms.ToolStripMenuItem mViewSearchMenu;
    }
}

