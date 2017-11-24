namespace NotesEditerforD
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanelNotesButton = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelEditStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.radioAdd = new System.Windows.Forms.RadioButton();
            this.radioEdit = new System.Windows.Forms.RadioButton();
            this.radioDelete = new System.Windows.Forms.RadioButton();
            this.labelBPM = new System.Windows.Forms.Label();
            this.numericUpDownBPM = new System.Windows.Forms.NumericUpDown();
            this.labelBeat = new System.Windows.Forms.Label();
            this.comboBoxBeat = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanelMusicScore = new System.Windows.Forms.FlowLayoutPanel();
            this.labelGrid = new System.Windows.Forms.Label();
            this.comboBoxGrid = new System.Windows.Forms.ComboBox();
            this.labelTotalNotes = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Tap = new NotesEditerforD.NotesButton();
            this.ExTap = new NotesEditerforD.NotesButton();
            this.Flick = new NotesEditerforD.NotesButton();
            this.HellTap = new NotesEditerforD.NotesButton();
            this.Hold = new NotesEditerforD.NotesButton();
            this.Slide = new NotesEditerforD.NotesButton();
            this.AirUp = new NotesEditerforD.NotesButton();
            this.AirDown = new NotesEditerforD.NotesButton();
            this.AirLine = new NotesEditerforD.NotesButton();
            this.flowLayoutPanelNotesButton.SuspendLayout();
            this.flowLayoutPanelEditStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBPM)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 1;
            // 
            // flowLayoutPanelNotesButton
            // 
            this.flowLayoutPanelNotesButton.AutoScroll = true;
            this.flowLayoutPanelNotesButton.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanelNotesButton.Controls.Add(this.Tap);
            this.flowLayoutPanelNotesButton.Controls.Add(this.ExTap);
            this.flowLayoutPanelNotesButton.Controls.Add(this.Flick);
            this.flowLayoutPanelNotesButton.Controls.Add(this.HellTap);
            this.flowLayoutPanelNotesButton.Controls.Add(this.Hold);
            this.flowLayoutPanelNotesButton.Controls.Add(this.Slide);
            this.flowLayoutPanelNotesButton.Controls.Add(this.AirUp);
            this.flowLayoutPanelNotesButton.Controls.Add(this.AirDown);
            this.flowLayoutPanelNotesButton.Controls.Add(this.AirLine);
            this.flowLayoutPanelNotesButton.Location = new System.Drawing.Point(7, 26);
            this.flowLayoutPanelNotesButton.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanelNotesButton.Name = "flowLayoutPanelNotesButton";
            this.flowLayoutPanelNotesButton.Size = new System.Drawing.Size(254, 848);
            this.flowLayoutPanelNotesButton.TabIndex = 2;
            // 
            // flowLayoutPanelEditStatus
            // 
            this.flowLayoutPanelEditStatus.Controls.Add(this.radioAdd);
            this.flowLayoutPanelEditStatus.Controls.Add(this.radioEdit);
            this.flowLayoutPanelEditStatus.Controls.Add(this.radioDelete);
            this.flowLayoutPanelEditStatus.Location = new System.Drawing.Point(269, 26);
            this.flowLayoutPanelEditStatus.Name = "flowLayoutPanelEditStatus";
            this.flowLayoutPanelEditStatus.Size = new System.Drawing.Size(168, 22);
            this.flowLayoutPanelEditStatus.TabIndex = 4;
            // 
            // radioAdd
            // 
            this.radioAdd.AutoSize = true;
            this.radioAdd.Checked = true;
            this.radioAdd.Location = new System.Drawing.Point(3, 3);
            this.radioAdd.Name = "radioAdd";
            this.radioAdd.Size = new System.Drawing.Size(43, 16);
            this.radioAdd.TabIndex = 0;
            this.radioAdd.TabStop = true;
            this.radioAdd.Text = "Add";
            this.radioAdd.UseVisualStyleBackColor = true;
            this.radioAdd.Click += new System.EventHandler(this.setEditStatus);
            // 
            // radioEdit
            // 
            this.radioEdit.AutoSize = true;
            this.radioEdit.Location = new System.Drawing.Point(52, 3);
            this.radioEdit.Name = "radioEdit";
            this.radioEdit.Size = new System.Drawing.Size(43, 16);
            this.radioEdit.TabIndex = 1;
            this.radioEdit.Text = "Edit";
            this.radioEdit.UseVisualStyleBackColor = true;
            this.radioEdit.Click += new System.EventHandler(this.setEditStatus);
            // 
            // radioDelete
            // 
            this.radioDelete.AutoSize = true;
            this.radioDelete.Location = new System.Drawing.Point(101, 3);
            this.radioDelete.Name = "radioDelete";
            this.radioDelete.Size = new System.Drawing.Size(56, 16);
            this.radioDelete.TabIndex = 2;
            this.radioDelete.Text = "Delete";
            this.radioDelete.UseVisualStyleBackColor = true;
            this.radioDelete.Click += new System.EventHandler(this.setEditStatus);
            // 
            // labelBPM
            // 
            this.labelBPM.AutoSize = true;
            this.labelBPM.Location = new System.Drawing.Point(443, 31);
            this.labelBPM.Name = "labelBPM";
            this.labelBPM.Size = new System.Drawing.Size(29, 12);
            this.labelBPM.TabIndex = 5;
            this.labelBPM.Text = "BPM";
            // 
            // numericUpDownBPM
            // 
            this.numericUpDownBPM.Location = new System.Drawing.Point(478, 26);
            this.numericUpDownBPM.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownBPM.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownBPM.Name = "numericUpDownBPM";
            this.numericUpDownBPM.Size = new System.Drawing.Size(61, 19);
            this.numericUpDownBPM.TabIndex = 6;
            this.numericUpDownBPM.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // labelBeat
            // 
            this.labelBeat.AutoSize = true;
            this.labelBeat.Location = new System.Drawing.Point(556, 29);
            this.labelBeat.Name = "labelBeat";
            this.labelBeat.Size = new System.Drawing.Size(29, 12);
            this.labelBeat.TabIndex = 7;
            this.labelBeat.Text = "Beat";
            // 
            // comboBoxBeat
            // 
            this.comboBoxBeat.FormattingEnabled = true;
            this.comboBoxBeat.Items.AddRange(new object[] {
            "4",
            "8",
            "12",
            "16",
            "24",
            "32",
            "48",
            "64",
            "96",
            "128",
            "192"});
            this.comboBoxBeat.Location = new System.Drawing.Point(591, 26);
            this.comboBoxBeat.Name = "comboBoxBeat";
            this.comboBoxBeat.Size = new System.Drawing.Size(46, 20);
            this.comboBoxBeat.TabIndex = 8;
            this.comboBoxBeat.Text = "8";
            this.comboBoxBeat.SelectedIndexChanged += new System.EventHandler(this.comboBoxBeat_SelectedIndexChanged);
            // 
            // flowLayoutPanelMusicScore
            // 
            this.flowLayoutPanelMusicScore.AutoScroll = true;
            this.flowLayoutPanelMusicScore.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.flowLayoutPanelMusicScore.Location = new System.Drawing.Point(269, 51);
            this.flowLayoutPanelMusicScore.Name = "flowLayoutPanelMusicScore";
            this.flowLayoutPanelMusicScore.Size = new System.Drawing.Size(1103, 823);
            this.flowLayoutPanelMusicScore.TabIndex = 9;
            this.flowLayoutPanelMusicScore.WrapContents = false;
            // 
            // labelGrid
            // 
            this.labelGrid.AutoSize = true;
            this.labelGrid.Location = new System.Drawing.Point(652, 29);
            this.labelGrid.Name = "labelGrid";
            this.labelGrid.Size = new System.Drawing.Size(26, 12);
            this.labelGrid.TabIndex = 10;
            this.labelGrid.Text = "Grid";
            // 
            // comboBoxGrid
            // 
            this.comboBoxGrid.FormattingEnabled = true;
            this.comboBoxGrid.Items.AddRange(new object[] {
            "4",
            "8",
            "16"});
            this.comboBoxGrid.Location = new System.Drawing.Point(684, 25);
            this.comboBoxGrid.Name = "comboBoxGrid";
            this.comboBoxGrid.Size = new System.Drawing.Size(46, 20);
            this.comboBoxGrid.TabIndex = 11;
            this.comboBoxGrid.Text = "8";
            this.comboBoxGrid.SelectedIndexChanged += new System.EventHandler(this.comboBoxGrid_SelectedIndexChanged);
            // 
            // labelTotalNotes
            // 
            this.labelTotalNotes.AutoSize = true;
            this.labelTotalNotes.Location = new System.Drawing.Point(751, 31);
            this.labelTotalNotes.Name = "labelTotalNotes";
            this.labelTotalNotes.Size = new System.Drawing.Size(79, 12);
            this.labelTotalNotes.TabIndex = 12;
            this.labelTotalNotes.Text = "Total notes : 0";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1384, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.saveAsMenuItem,
            this.saveMenuItem,
            this.exportMenuItem,
            this.quitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(67, 20);
            this.fileMenuItem.Text = "ファイル(F)";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.ShortcutKeyDisplayString = "Ctrl+N";
            this.newMenuItem.Size = new System.Drawing.Size(184, 22);
            this.newMenuItem.Text = "新規作成(N)";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
            this.openMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openMenuItem.Text = "開く(O)...";
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.ShortcutKeyDisplayString = "";
            this.saveAsMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveAsMenuItem.Text = "名前をつけて保存(A)...";
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveMenuItem.Text = "上書き保存(S)";
            // 
            // exportMenuItem
            // 
            this.exportMenuItem.Name = "exportMenuItem";
            this.exportMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exportMenuItem.Text = "エクスポート(X)...";
            // 
            // quitMenuItem
            // 
            this.quitMenuItem.Name = "quitMenuItem";
            this.quitMenuItem.Size = new System.Drawing.Size(184, 22);
            this.quitMenuItem.Text = "終了(Q)";
            // 
            // Tap
            // 
            this.Tap.AutoSize = true;
            this.Tap.BackColor = System.Drawing.SystemColors.Control;
            this.Tap.Cursor = System.Windows.Forms.Cursors.Default;
            this.Tap.IsActive = true;
            this.Tap.IsAir = false;
            this.Tap.Location = new System.Drawing.Point(1, 1);
            this.Tap.Margin = new System.Windows.Forms.Padding(1);
            this.Tap.Name = "Tap";
            this.Tap.NotesName = "Tap";
            this.Tap.Size = new System.Drawing.Size(227, 97);
            this.Tap.TabIndex = 0;
            // 
            // ExTap
            // 
            this.ExTap.AutoSize = true;
            this.ExTap.BackColor = System.Drawing.SystemColors.Control;
            this.ExTap.IsActive = false;
            this.ExTap.IsAir = false;
            this.ExTap.Location = new System.Drawing.Point(1, 100);
            this.ExTap.Margin = new System.Windows.Forms.Padding(1);
            this.ExTap.Name = "ExTap";
            this.ExTap.NotesName = "ExTap";
            this.ExTap.Size = new System.Drawing.Size(227, 97);
            this.ExTap.TabIndex = 5;
            // 
            // Flick
            // 
            this.Flick.AutoSize = true;
            this.Flick.BackColor = System.Drawing.SystemColors.Control;
            this.Flick.IsActive = false;
            this.Flick.IsAir = false;
            this.Flick.Location = new System.Drawing.Point(1, 199);
            this.Flick.Margin = new System.Windows.Forms.Padding(1);
            this.Flick.Name = "Flick";
            this.Flick.NotesName = "Flick";
            this.Flick.Size = new System.Drawing.Size(227, 97);
            this.Flick.TabIndex = 4;
            // 
            // HellTap
            // 
            this.HellTap.AutoSize = true;
            this.HellTap.BackColor = System.Drawing.SystemColors.Control;
            this.HellTap.IsActive = false;
            this.HellTap.IsAir = false;
            this.HellTap.Location = new System.Drawing.Point(2, 299);
            this.HellTap.Margin = new System.Windows.Forms.Padding(2);
            this.HellTap.Name = "HellTap";
            this.HellTap.NotesName = "HellTap";
            this.HellTap.Size = new System.Drawing.Size(227, 97);
            this.HellTap.TabIndex = 10;
            // 
            // Hold
            // 
            this.Hold.AutoSize = true;
            this.Hold.BackColor = System.Drawing.SystemColors.Control;
            this.Hold.IsActive = false;
            this.Hold.IsAir = false;
            this.Hold.Location = new System.Drawing.Point(1, 399);
            this.Hold.Margin = new System.Windows.Forms.Padding(1);
            this.Hold.Name = "Hold";
            this.Hold.NotesName = "Hold";
            this.Hold.Size = new System.Drawing.Size(227, 97);
            this.Hold.TabIndex = 3;
            // 
            // Slide
            // 
            this.Slide.AutoSize = true;
            this.Slide.BackColor = System.Drawing.SystemColors.Control;
            this.Slide.IsActive = false;
            this.Slide.IsAir = false;
            this.Slide.Location = new System.Drawing.Point(1, 498);
            this.Slide.Margin = new System.Windows.Forms.Padding(1);
            this.Slide.Name = "Slide";
            this.Slide.NotesName = "Slide";
            this.Slide.Size = new System.Drawing.Size(227, 97);
            this.Slide.TabIndex = 2;
            // 
            // AirUp
            // 
            this.AirUp.AutoSize = true;
            this.AirUp.BackColor = System.Drawing.SystemColors.Control;
            this.AirUp.IsActive = false;
            this.AirUp.IsAir = true;
            this.AirUp.Location = new System.Drawing.Point(1, 597);
            this.AirUp.Margin = new System.Windows.Forms.Padding(1);
            this.AirUp.Name = "AirUp";
            this.AirUp.NotesName = "AirUp";
            this.AirUp.Size = new System.Drawing.Size(227, 97);
            this.AirUp.TabIndex = 6;
            // 
            // AirDown
            // 
            this.AirDown.AutoSize = true;
            this.AirDown.BackColor = System.Drawing.SystemColors.Control;
            this.AirDown.IsActive = false;
            this.AirDown.IsAir = true;
            this.AirDown.Location = new System.Drawing.Point(1, 696);
            this.AirDown.Margin = new System.Windows.Forms.Padding(1);
            this.AirDown.Name = "AirDown";
            this.AirDown.NotesName = "AirDown";
            this.AirDown.Size = new System.Drawing.Size(227, 97);
            this.AirDown.TabIndex = 7;
            // 
            // AirLine
            // 
            this.AirLine.AutoSize = true;
            this.AirLine.BackColor = System.Drawing.SystemColors.Control;
            this.AirLine.IsActive = false;
            this.AirLine.IsAir = false;
            this.AirLine.Location = new System.Drawing.Point(1, 795);
            this.AirLine.Margin = new System.Windows.Forms.Padding(1);
            this.AirLine.Name = "AirLine";
            this.AirLine.NotesName = "AirLine";
            this.AirLine.Size = new System.Drawing.Size(227, 97);
            this.AirLine.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1384, 886);
            this.Controls.Add(this.labelTotalNotes);
            this.Controls.Add(this.comboBoxGrid);
            this.Controls.Add(this.labelGrid);
            this.Controls.Add(this.comboBoxBeat);
            this.Controls.Add(this.labelBeat);
            this.Controls.Add(this.numericUpDownBPM);
            this.Controls.Add(this.labelBPM);
            this.Controls.Add(this.flowLayoutPanelEditStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanelNotesButton);
            this.Controls.Add(this.flowLayoutPanelMusicScore);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "NotesEditorforD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.flowLayoutPanelNotesButton.ResumeLayout(false);
            this.flowLayoutPanelNotesButton.PerformLayout();
            this.flowLayoutPanelEditStatus.ResumeLayout(false);
            this.flowLayoutPanelEditStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBPM)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelNotesButton;
        private NotesButton Slide;
        private NotesButton Hold;
        private NotesButton Flick;
        private NotesButton ExTap;
        private NotesButton AirUp;
        public NotesButton Tap;
        private NotesButton AirLine;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelEditStatus;
        private System.Windows.Forms.RadioButton radioAdd;
        private System.Windows.Forms.RadioButton radioEdit;
        private System.Windows.Forms.RadioButton radioDelete;
        private System.Windows.Forms.Label labelBPM;
        private System.Windows.Forms.NumericUpDown numericUpDownBPM;
        private System.Windows.Forms.Label labelBeat;
        private System.Windows.Forms.ComboBox comboBoxBeat;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMusicScore;
        private System.Windows.Forms.Label labelGrid;
        private System.Windows.Forms.ComboBox comboBoxGrid;
        private System.Windows.Forms.Label labelTotalNotes;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitMenuItem;
        private NotesButton AirDown;
        private NotesButton HellTap;
    }
}

