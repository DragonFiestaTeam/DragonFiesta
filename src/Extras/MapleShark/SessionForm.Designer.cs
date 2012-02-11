namespace MapleShark
{
    partial class SessionForm
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
            this.mPacketList = new System.Windows.Forms.ListView();
            this.mTimestampColumn = new System.Windows.Forms.ColumnHeader();
            this.mDirectionColumn = new System.Windows.Forms.ColumnHeader();
            this.mLengthColumn = new System.Windows.Forms.ColumnHeader();
            this.mOpcodeColumn = new System.Windows.Forms.ColumnHeader();
            this.mNameColumn = new System.Windows.Forms.ColumnHeader();
            this.mPacketContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mPacketContextNameLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.mPacketContextNameBox = new System.Windows.Forms.ToolStripTextBox();
            this.mPacketContextSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mPacketContextIgnoreMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenu = new System.Windows.Forms.MenuStrip();
            this.mMainFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mFileSeparatorMenu = new System.Windows.Forms.ToolStripSeparator();
            this.mFileSaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mFileExportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewCommonScriptMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewSeparator1Menu = new System.Windows.Forms.ToolStripSeparator();
            this.mViewRefreshMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewSeparator2Menu = new System.Windows.Forms.ToolStripSeparator();
            this.mViewOutboundMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewInboundMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewIgnoredMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mViewSeparator3Menu = new System.Windows.Forms.ToolStripSeparator();
            this.mSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.mExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.mPacketContextMenu.SuspendLayout();
            this.mMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mPacketList
            // 
            this.mPacketList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mTimestampColumn,
            this.mDirectionColumn,
            this.mLengthColumn,
            this.mOpcodeColumn,
            this.mNameColumn});
            this.mPacketList.ContextMenuStrip = this.mPacketContextMenu;
            this.mPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPacketList.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mPacketList.FullRowSelect = true;
            this.mPacketList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mPacketList.HideSelection = false;
            this.mPacketList.Location = new System.Drawing.Point(0, 24);
            this.mPacketList.MultiSelect = false;
            this.mPacketList.Name = "mPacketList";
            this.mPacketList.Size = new System.Drawing.Size(611, 427);
            this.mPacketList.TabIndex = 0;
            this.mPacketList.UseCompatibleStateImageBehavior = false;
            this.mPacketList.View = System.Windows.Forms.View.Details;
            this.mPacketList.ItemActivate += new System.EventHandler(this.mPacketList_ItemActivate);
            this.mPacketList.SelectedIndexChanged += new System.EventHandler(this.mPacketList_SelectedIndexChanged);
            // 
            // mTimestampColumn
            // 
            this.mTimestampColumn.Text = "Timestamp";
            this.mTimestampColumn.Width = 75;
            // 
            // mDirectionColumn
            // 
            this.mDirectionColumn.Text = "Direction";
            this.mDirectionColumn.Width = 75;
            // 
            // mLengthColumn
            // 
            this.mLengthColumn.Text = "Length";
            this.mLengthColumn.Width = 75;
            // 
            // mOpcodeColumn
            // 
            this.mOpcodeColumn.Text = "Opcode";
            // 
            // mNameColumn
            // 
            this.mNameColumn.Text = "Name";
            this.mNameColumn.Width = 300;
            // 
            // mPacketContextMenu
            // 
            this.mPacketContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mPacketContextNameLabel,
            this.mPacketContextNameBox,
            this.mPacketContextSeparator,
            this.mPacketContextIgnoreMenu});
            this.mPacketContextMenu.Name = "mPacketContextMenu";
            this.mPacketContextMenu.Size = new System.Drawing.Size(211, 77);
            this.mPacketContextMenu.Opened += new System.EventHandler(this.mPacketContextMenu_Opened);
            this.mPacketContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.mPacketContextMenu_Opening);
            // 
            // mPacketContextNameLabel
            // 
            this.mPacketContextNameLabel.Enabled = false;
            this.mPacketContextNameLabel.Name = "mPacketContextNameLabel";
            this.mPacketContextNameLabel.Size = new System.Drawing.Size(210, 22);
            this.mPacketContextNameLabel.Text = "Name:";
            // 
            // mPacketContextNameBox
            // 
            this.mPacketContextNameBox.AcceptsReturn = true;
            this.mPacketContextNameBox.Name = "mPacketContextNameBox";
            this.mPacketContextNameBox.Size = new System.Drawing.Size(150, 21);
            this.mPacketContextNameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mPacketContextNameBox_KeyDown);
            // 
            // mPacketContextSeparator
            // 
            this.mPacketContextSeparator.Name = "mPacketContextSeparator";
            this.mPacketContextSeparator.Size = new System.Drawing.Size(207, 6);
            // 
            // mPacketContextIgnoreMenu
            // 
            this.mPacketContextIgnoreMenu.CheckOnClick = true;
            this.mPacketContextIgnoreMenu.Name = "mPacketContextIgnoreMenu";
            this.mPacketContextIgnoreMenu.Size = new System.Drawing.Size(210, 22);
            this.mPacketContextIgnoreMenu.Text = "Ignore";
            this.mPacketContextIgnoreMenu.CheckedChanged += new System.EventHandler(this.mPacketContextIgnoreMenu_CheckedChanged);
            // 
            // mMenu
            // 
            this.mMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMainFileMenu,
            this.mViewMenu});
            this.mMenu.Location = new System.Drawing.Point(0, 0);
            this.mMenu.Name = "mMenu";
            this.mMenu.Size = new System.Drawing.Size(611, 24);
            this.mMenu.TabIndex = 6;
            this.mMenu.Visible = false;
            // 
            // mMainFileMenu
            // 
            this.mMainFileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFileSeparatorMenu,
            this.mFileSaveMenu,
            this.mFileExportMenu});
            this.mMainFileMenu.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.mMainFileMenu.Name = "mMainFileMenu";
            this.mMainFileMenu.Size = new System.Drawing.Size(35, 20);
            this.mMainFileMenu.Text = "&File";
            // 
            // mFileSeparatorMenu
            // 
            this.mFileSeparatorMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mFileSeparatorMenu.MergeIndex = 2;
            this.mFileSeparatorMenu.Name = "mFileSeparatorMenu";
            this.mFileSeparatorMenu.Size = new System.Drawing.Size(152, 6);
            // 
            // mFileSaveMenu
            // 
            this.mFileSaveMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mFileSaveMenu.MergeIndex = 3;
            this.mFileSaveMenu.Name = "mFileSaveMenu";
            this.mFileSaveMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mFileSaveMenu.Size = new System.Drawing.Size(155, 22);
            this.mFileSaveMenu.Text = "&Save";
            this.mFileSaveMenu.Click += new System.EventHandler(this.mFileSaveMenu_Click);
            // 
            // mFileExportMenu
            // 
            this.mFileExportMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mFileExportMenu.MergeIndex = 4;
            this.mFileExportMenu.Name = "mFileExportMenu";
            this.mFileExportMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.mFileExportMenu.Size = new System.Drawing.Size(155, 22);
            this.mFileExportMenu.Text = "&Export";
            this.mFileExportMenu.Click += new System.EventHandler(this.mFileExportMenu_Click);
            // 
            // mViewMenu
            // 
            this.mViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mViewCommonScriptMenu,
            this.mViewSeparator1Menu,
            this.mViewRefreshMenu,
            this.mViewSeparator2Menu,
            this.mViewOutboundMenu,
            this.mViewInboundMenu,
            this.mViewIgnoredMenu,
            this.mViewSeparator3Menu});
            this.mViewMenu.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.mViewMenu.Name = "mViewMenu";
            this.mViewMenu.Size = new System.Drawing.Size(41, 20);
            this.mViewMenu.Text = "&View";
            // 
            // mViewCommonScriptMenu
            // 
            this.mViewCommonScriptMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewCommonScriptMenu.MergeIndex = 0;
            this.mViewCommonScriptMenu.Name = "mViewCommonScriptMenu";
            this.mViewCommonScriptMenu.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mViewCommonScriptMenu.Size = new System.Drawing.Size(175, 22);
            this.mViewCommonScriptMenu.Text = "Common Script";
            this.mViewCommonScriptMenu.Click += new System.EventHandler(this.mViewCommonScriptMenu_Click);
            // 
            // mViewSeparator1Menu
            // 
            this.mViewSeparator1Menu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewSeparator1Menu.MergeIndex = 1;
            this.mViewSeparator1Menu.Name = "mViewSeparator1Menu";
            this.mViewSeparator1Menu.Size = new System.Drawing.Size(172, 6);
            // 
            // mViewRefreshMenu
            // 
            this.mViewRefreshMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewRefreshMenu.MergeIndex = 2;
            this.mViewRefreshMenu.Name = "mViewRefreshMenu";
            this.mViewRefreshMenu.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mViewRefreshMenu.Size = new System.Drawing.Size(175, 22);
            this.mViewRefreshMenu.Text = "&Refresh";
            this.mViewRefreshMenu.Click += new System.EventHandler(this.mViewRefreshMenu_Click);
            // 
            // mViewSeparator2Menu
            // 
            this.mViewSeparator2Menu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewSeparator2Menu.MergeIndex = 3;
            this.mViewSeparator2Menu.Name = "mViewSeparator2Menu";
            this.mViewSeparator2Menu.Size = new System.Drawing.Size(172, 6);
            // 
            // mViewOutboundMenu
            // 
            this.mViewOutboundMenu.Checked = true;
            this.mViewOutboundMenu.CheckOnClick = true;
            this.mViewOutboundMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewOutboundMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewOutboundMenu.MergeIndex = 4;
            this.mViewOutboundMenu.Name = "mViewOutboundMenu";
            this.mViewOutboundMenu.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.mViewOutboundMenu.Size = new System.Drawing.Size(175, 22);
            this.mViewOutboundMenu.Text = "Outbound";
            this.mViewOutboundMenu.CheckedChanged += new System.EventHandler(this.mViewOutboundMenu_CheckedChanged);
            // 
            // mViewInboundMenu
            // 
            this.mViewInboundMenu.Checked = true;
            this.mViewInboundMenu.CheckOnClick = true;
            this.mViewInboundMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mViewInboundMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewInboundMenu.MergeIndex = 5;
            this.mViewInboundMenu.Name = "mViewInboundMenu";
            this.mViewInboundMenu.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.mViewInboundMenu.Size = new System.Drawing.Size(175, 22);
            this.mViewInboundMenu.Text = "Inbound";
            this.mViewInboundMenu.CheckedChanged += new System.EventHandler(this.mViewInboundMenu_CheckedChanged);
            // 
            // mViewIgnoredMenu
            // 
            this.mViewIgnoredMenu.CheckOnClick = true;
            this.mViewIgnoredMenu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewIgnoredMenu.MergeIndex = 6;
            this.mViewIgnoredMenu.Name = "mViewIgnoredMenu";
            this.mViewIgnoredMenu.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.mViewIgnoredMenu.Size = new System.Drawing.Size(175, 22);
            this.mViewIgnoredMenu.Text = "Ignored";
            this.mViewIgnoredMenu.CheckedChanged += new System.EventHandler(this.mViewIgnoredMenu_CheckedChanged);
            // 
            // mViewSeparator3Menu
            // 
            this.mViewSeparator3Menu.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mViewSeparator3Menu.MergeIndex = 7;
            this.mViewSeparator3Menu.Name = "mViewSeparator3Menu";
            this.mViewSeparator3Menu.Size = new System.Drawing.Size(172, 6);
            // 
            // mSaveDialog
            // 
            this.mSaveDialog.Filter = "MapleShark Binary Files|*.msb";
            this.mSaveDialog.RestoreDirectory = true;
            this.mSaveDialog.Title = "Save";
            // 
            // mExportDialog
            // 
            this.mExportDialog.Filter = "Text Files|*.txt";
            this.mExportDialog.RestoreDirectory = true;
            this.mExportDialog.Title = "Export";
            // 
            // SessionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 451);
            this.ControlBox = false;
            this.Controls.Add(this.mPacketList);
            this.Controls.Add(this.mMenu);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "SessionForm";
            this.mPacketContextMenu.ResumeLayout(false);
            this.mPacketContextMenu.PerformLayout();
            this.mMenu.ResumeLayout(false);
            this.mMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView mPacketList;
        private System.Windows.Forms.ColumnHeader mTimestampColumn;
        private System.Windows.Forms.ColumnHeader mDirectionColumn;
        private System.Windows.Forms.ColumnHeader mLengthColumn;
        private System.Windows.Forms.ColumnHeader mOpcodeColumn;
        private System.Windows.Forms.ColumnHeader mNameColumn;
        private System.Windows.Forms.MenuStrip mMenu;
        private System.Windows.Forms.ToolStripMenuItem mMainFileMenu;
        private System.Windows.Forms.ToolStripMenuItem mFileSaveMenu;
        private System.Windows.Forms.ToolStripMenuItem mViewMenu;
        private System.Windows.Forms.ToolStripMenuItem mViewOutboundMenu;
        private System.Windows.Forms.ToolStripMenuItem mViewInboundMenu;
        private System.Windows.Forms.ToolStripMenuItem mViewIgnoredMenu;
        private System.Windows.Forms.ToolStripSeparator mViewSeparator1Menu;
        private System.Windows.Forms.ToolStripMenuItem mViewRefreshMenu;
        private System.Windows.Forms.SaveFileDialog mSaveDialog;
        private System.Windows.Forms.ContextMenuStrip mPacketContextMenu;
        private System.Windows.Forms.ToolStripTextBox mPacketContextNameBox;
        private System.Windows.Forms.ToolStripMenuItem mPacketContextIgnoreMenu;
        private System.Windows.Forms.ToolStripMenuItem mPacketContextNameLabel;
        private System.Windows.Forms.ToolStripSeparator mPacketContextSeparator;
        private System.Windows.Forms.ToolStripMenuItem mViewCommonScriptMenu;
        private System.Windows.Forms.ToolStripSeparator mViewSeparator2Menu;
        private System.Windows.Forms.ToolStripMenuItem mFileExportMenu;
        private System.Windows.Forms.SaveFileDialog mExportDialog;
        private System.Windows.Forms.ToolStripSeparator mViewSeparator3Menu;
        private System.Windows.Forms.ToolStripSeparator mFileSeparatorMenu;
    }
}