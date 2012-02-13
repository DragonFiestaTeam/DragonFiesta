namespace MapleShark
{
    partial class SearchForm
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
            this.mSplitter = new System.Windows.Forms.SplitContainer();
            this.mOpcodeSplitter = new System.Windows.Forms.SplitContainer();
            this.mOpcodeCombo = new System.Windows.Forms.ComboBox();
            this.mNextOpcodeButton = new System.Windows.Forms.Button();
            this.mSequenceSplitter = new System.Windows.Forms.SplitContainer();
            this.mSequenceHex = new System.Windows.Forms.HexBox();
            this.mNextSequenceButton = new System.Windows.Forms.Button();
            this.HeaderBox = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.Typebox = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mSplitter.Panel1.SuspendLayout();
            this.mSplitter.Panel2.SuspendLayout();
            this.mSplitter.SuspendLayout();
            this.mOpcodeSplitter.Panel1.SuspendLayout();
            this.mOpcodeSplitter.Panel2.SuspendLayout();
            this.mOpcodeSplitter.SuspendLayout();
            this.mSequenceSplitter.Panel1.SuspendLayout();
            this.mSequenceSplitter.Panel2.SuspendLayout();
            this.mSequenceSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSplitter
            // 
            this.mSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mSplitter.IsSplitterFixed = true;
            this.mSplitter.Location = new System.Drawing.Point(0, 0);
            this.mSplitter.Name = "mSplitter";
            this.mSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mSplitter.Panel1
            // 
            this.mSplitter.Panel1.Controls.Add(this.mOpcodeSplitter);
            // 
            // mSplitter.Panel2
            // 
            this.mSplitter.Panel2.Controls.Add(this.mSequenceSplitter);
            this.mSplitter.Size = new System.Drawing.Size(523, 102);
            this.mSplitter.SplitterDistance = 25;
            this.mSplitter.TabIndex = 9;
            this.mSplitter.TabStop = false;
            // 
            // mOpcodeSplitter
            // 
            this.mOpcodeSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mOpcodeSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mOpcodeSplitter.IsSplitterFixed = true;
            this.mOpcodeSplitter.Location = new System.Drawing.Point(0, 0);
            this.mOpcodeSplitter.Name = "mOpcodeSplitter";
            // 
            // mOpcodeSplitter.Panel1
            // 
            this.mOpcodeSplitter.Panel1.Controls.Add(this.mOpcodeCombo);
            // 
            // mOpcodeSplitter.Panel2
            // 
            this.mOpcodeSplitter.Panel2.Controls.Add(this.mNextOpcodeButton);
            this.mOpcodeSplitter.Size = new System.Drawing.Size(523, 25);
            this.mOpcodeSplitter.SplitterDistance = 392;
            this.mOpcodeSplitter.TabIndex = 5;
            this.mOpcodeSplitter.TabStop = false;
            // 
            // mOpcodeCombo
            // 
            this.mOpcodeCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mOpcodeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mOpcodeCombo.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mOpcodeCombo.FormattingEnabled = true;
            this.mOpcodeCombo.Location = new System.Drawing.Point(0, 0);
            this.mOpcodeCombo.Name = "mOpcodeCombo";
            this.mOpcodeCombo.Size = new System.Drawing.Size(392, 23);
            this.mOpcodeCombo.TabIndex = 4;
            this.mOpcodeCombo.SelectedIndexChanged += new System.EventHandler(this.mOpcodeCombo_SelectedIndexChanged);
            // 
            // mNextOpcodeButton
            // 
            this.mNextOpcodeButton.AutoSize = true;
            this.mNextOpcodeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mNextOpcodeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mNextOpcodeButton.Enabled = false;
            this.mNextOpcodeButton.Location = new System.Drawing.Point(0, 0);
            this.mNextOpcodeButton.Name = "mNextOpcodeButton";
            this.mNextOpcodeButton.Size = new System.Drawing.Size(127, 25);
            this.mNextOpcodeButton.TabIndex = 5;
            this.mNextOpcodeButton.Text = "Next &Opcode";
            this.mNextOpcodeButton.UseVisualStyleBackColor = true;
            this.mNextOpcodeButton.Click += new System.EventHandler(this.mNextOpcodeButton_Click);
            // 
            // mSequenceSplitter
            // 
            this.mSequenceSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSequenceSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mSequenceSplitter.IsSplitterFixed = true;
            this.mSequenceSplitter.Location = new System.Drawing.Point(0, 0);
            this.mSequenceSplitter.Name = "mSequenceSplitter";
            // 
            // mSequenceSplitter.Panel1
            // 
            this.mSequenceSplitter.Panel1.Controls.Add(this.label2);
            this.mSequenceSplitter.Panel1.Controls.Add(this.label1);
            this.mSequenceSplitter.Panel1.Controls.Add(this.Typebox);
            this.mSequenceSplitter.Panel1.Controls.Add(this.HeaderBox);
            this.mSequenceSplitter.Panel1.Controls.Add(this.mSequenceHex);
            // 
            // mSequenceSplitter.Panel2
            // 
            this.mSequenceSplitter.Panel2.Controls.Add(this.mNextSequenceButton);
            this.mSequenceSplitter.Size = new System.Drawing.Size(523, 73);
            this.mSequenceSplitter.SplitterDistance = 392;
            this.mSequenceSplitter.TabIndex = 7;
            this.mSequenceSplitter.TabStop = false;
            // 
            // mSequenceHex
            // 
            this.mSequenceHex.Dock = System.Windows.Forms.DockStyle.Top;
            this.mSequenceHex.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mSequenceHex.LineInfoForeColor = System.Drawing.Color.Empty;
            this.mSequenceHex.Location = new System.Drawing.Point(0, 0);
            this.mSequenceHex.Name = "mSequenceHex";
            this.mSequenceHex.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.mSequenceHex.Size = new System.Drawing.Size(392, 26);
            this.mSequenceHex.TabIndex = 6;
            this.mSequenceHex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mSequenceHex_KeyPress);
            // 
            // mNextSequenceButton
            // 
            this.mNextSequenceButton.AutoSize = true;
            this.mNextSequenceButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mNextSequenceButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.mNextSequenceButton.Enabled = false;
            this.mNextSequenceButton.Location = new System.Drawing.Point(0, 0);
            this.mNextSequenceButton.Name = "mNextSequenceButton";
            this.mNextSequenceButton.Size = new System.Drawing.Size(127, 24);
            this.mNextSequenceButton.TabIndex = 7;
            this.mNextSequenceButton.Text = "Next &Sequence";
            this.mNextSequenceButton.UseVisualStyleBackColor = true;
            this.mNextSequenceButton.Click += new System.EventHandler(this.mNextSequenceButton_Click);
            // 
            // HeaderBox
            // 
            // 
            // 
            // 
            this.HeaderBox.Border.Class = "TextBoxBorder";
            this.HeaderBox.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.HeaderBox.Location = new System.Drawing.Point(3, 32);
            this.HeaderBox.Name = "HeaderBox";
            this.HeaderBox.Size = new System.Drawing.Size(100, 20);
            this.HeaderBox.TabIndex = 7;
            // 
            // Typebox
            // 
            // 
            // 
            // 
            this.Typebox.Border.Class = "TextBoxBorder";
            this.Typebox.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Typebox.Location = new System.Drawing.Point(164, 32);
            this.Typebox.Name = "Typebox";
            this.Typebox.Size = new System.Drawing.Size(100, 20);
            this.Typebox.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "Header";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 14);
            this.label2.TabIndex = 10;
            this.label2.Text = "Type";
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 102);
            this.Controls.Add(this.mSplitter);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.MinimumSize = new System.Drawing.Size(531, 82);
            this.Name = "SearchForm";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockTop;
            this.Text = "Search";
            this.mSplitter.Panel1.ResumeLayout(false);
            this.mSplitter.Panel2.ResumeLayout(false);
            this.mSplitter.ResumeLayout(false);
            this.mOpcodeSplitter.Panel1.ResumeLayout(false);
            this.mOpcodeSplitter.Panel2.ResumeLayout(false);
            this.mOpcodeSplitter.Panel2.PerformLayout();
            this.mOpcodeSplitter.ResumeLayout(false);
            this.mSequenceSplitter.Panel1.ResumeLayout(false);
            this.mSequenceSplitter.Panel1.PerformLayout();
            this.mSequenceSplitter.Panel2.ResumeLayout(false);
            this.mSequenceSplitter.Panel2.PerformLayout();
            this.mSequenceSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mSplitter;
        private System.Windows.Forms.SplitContainer mOpcodeSplitter;
        private System.Windows.Forms.ComboBox mOpcodeCombo;
        private System.Windows.Forms.Button mNextOpcodeButton;
        private System.Windows.Forms.SplitContainer mSequenceSplitter;
        private System.Windows.Forms.HexBox mSequenceHex;
        private System.Windows.Forms.Button mNextSequenceButton;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.TextBoxX Typebox;
        public System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.TextBoxX HeaderBox;
    }
}