namespace NotesEditerforD
{
    partial class Form2
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
            this.export = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.artistLabel = new System.Windows.Forms.Label();
            this.designerLabel = new System.Windows.Forms.Label();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.levelLabel = new System.Windows.Forms.Label();
            this.songidLabel = new System.Windows.Forms.Label();
            this.waveLabel = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.jacketLabel = new System.Windows.Forms.Label();
            this.bpmLabel = new System.Windows.Forms.Label();
            this.previewLabel = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxArtist = new System.Windows.Forms.TextBox();
            this.textBoxDesigner = new System.Windows.Forms.TextBox();
            this.difficultyComboBox = new System.Windows.Forms.ComboBox();
            this.BPMUpDown = new System.Windows.Forms.NumericUpDown();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.textBoxWAVE = new System.Windows.Forms.TextBox();
            this.textBoxJacket = new System.Windows.Forms.TextBox();
            this.buttonWave = new System.Windows.Forms.Button();
            this.buttonJacket = new System.Windows.Forms.Button();
            this.offsetUpDown = new System.Windows.Forms.NumericUpDown();
            this.labelexdir = new System.Windows.Forms.Label();
            this.buttonexdir = new System.Windows.Forms.Button();
            this.textBoxExport = new System.Windows.Forms.TextBox();
            this.checkBoxWhile = new System.Windows.Forms.CheckBox();
            this.playLevelUpDown = new System.Windows.Forms.NumericUpDown();
            this.textBoxWE = new System.Windows.Forms.TextBox();
            this.previewButton = new System.Windows.Forms.Button();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.previewTitle = new System.Windows.Forms.Label();
            this.previewLevel = new System.Windows.Forms.Label();
            this.previewDesigner = new System.Windows.Forms.Label();
            this.previewBPM = new System.Windows.Forms.Label();
            this.previewArtist = new System.Windows.Forms.Label();
            this.previewWELevel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.BPMUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playLevelUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(411, 322);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(75, 23);
            this.export.TabIndex = 0;
            this.export.Text = "エクスポート";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(492, 322);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "キャンセル";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(254, 17);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(28, 12);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Title";
            this.titleLabel.Click += new System.EventHandler(this.titleLabel_Click);
            // 
            // artistLabel
            // 
            this.artistLabel.AutoSize = true;
            this.artistLabel.Location = new System.Drawing.Point(254, 42);
            this.artistLabel.Name = "artistLabel";
            this.artistLabel.Size = new System.Drawing.Size(34, 12);
            this.artistLabel.TabIndex = 3;
            this.artistLabel.Text = "Artist";
            this.artistLabel.Click += new System.EventHandler(this.artistLabel_Click);
            // 
            // designerLabel
            // 
            this.designerLabel.AutoSize = true;
            this.designerLabel.Location = new System.Drawing.Point(254, 67);
            this.designerLabel.Name = "designerLabel";
            this.designerLabel.Size = new System.Drawing.Size(80, 12);
            this.designerLabel.TabIndex = 4;
            this.designerLabel.Text = "NotesDesigner";
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(254, 92);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(52, 12);
            this.difficultyLabel.TabIndex = 5;
            this.difficultyLabel.Text = "Difficulty";
            // 
            // levelLabel
            // 
            this.levelLabel.AutoSize = true;
            this.levelLabel.Location = new System.Drawing.Point(254, 117);
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.Size = new System.Drawing.Size(54, 12);
            this.levelLabel.TabIndex = 6;
            this.levelLabel.Text = "PlayLevel";
            // 
            // songidLabel
            // 
            this.songidLabel.AutoSize = true;
            this.songidLabel.Location = new System.Drawing.Point(254, 143);
            this.songidLabel.Name = "songidLabel";
            this.songidLabel.Size = new System.Drawing.Size(41, 12);
            this.songidLabel.TabIndex = 7;
            this.songidLabel.Text = "SongID";
            // 
            // waveLabel
            // 
            this.waveLabel.AutoSize = true;
            this.waveLabel.Location = new System.Drawing.Point(254, 168);
            this.waveLabel.Name = "waveLabel";
            this.waveLabel.Size = new System.Drawing.Size(37, 12);
            this.waveLabel.TabIndex = 8;
            this.waveLabel.Text = "WAVE";
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(254, 196);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(87, 12);
            this.offsetLabel.TabIndex = 9;
            this.offsetLabel.Text = "WAVEOffset (s)";
            // 
            // jacketLabel
            // 
            this.jacketLabel.AutoSize = true;
            this.jacketLabel.Location = new System.Drawing.Point(254, 222);
            this.jacketLabel.Name = "jacketLabel";
            this.jacketLabel.Size = new System.Drawing.Size(40, 12);
            this.jacketLabel.TabIndex = 10;
            this.jacketLabel.Text = "Jacket";
            // 
            // bpmLabel
            // 
            this.bpmLabel.AutoSize = true;
            this.bpmLabel.Location = new System.Drawing.Point(254, 250);
            this.bpmLabel.Name = "bpmLabel";
            this.bpmLabel.Size = new System.Drawing.Size(54, 12);
            this.bpmLabel.TabIndex = 11;
            this.bpmLabel.Text = "StartBPM";
            // 
            // previewLabel
            // 
            this.previewLabel.AutoSize = true;
            this.previewLabel.Location = new System.Drawing.Point(12, 9);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Size = new System.Drawing.Size(45, 12);
            this.previewLabel.TabIndex = 12;
            this.previewLabel.Text = "Preview";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(354, 14);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(213, 19);
            this.textBoxTitle.TabIndex = 13;
            this.textBoxTitle.Text = "default title";
            // 
            // textBoxArtist
            // 
            this.textBoxArtist.Location = new System.Drawing.Point(354, 39);
            this.textBoxArtist.Name = "textBoxArtist";
            this.textBoxArtist.Size = new System.Drawing.Size(213, 19);
            this.textBoxArtist.TabIndex = 14;
            this.textBoxArtist.Text = "default artist";
            // 
            // textBoxDesigner
            // 
            this.textBoxDesigner.Location = new System.Drawing.Point(354, 64);
            this.textBoxDesigner.Name = "textBoxDesigner";
            this.textBoxDesigner.Size = new System.Drawing.Size(213, 19);
            this.textBoxDesigner.TabIndex = 15;
            this.textBoxDesigner.Text = "default designer";
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.FormattingEnabled = true;
            this.difficultyComboBox.Items.AddRange(new object[] {
            "BASIC",
            "ADVANCED",
            "EXPERT",
            "MASTER",
            "WORLD\'S END"});
            this.difficultyComboBox.Location = new System.Drawing.Point(401, 89);
            this.difficultyComboBox.Name = "difficultyComboBox";
            this.difficultyComboBox.Size = new System.Drawing.Size(121, 20);
            this.difficultyComboBox.TabIndex = 16;
            this.difficultyComboBox.SelectedIndexChanged += new System.EventHandler(this.difficultyComboBox_SelectedIndexChanged);
            // 
            // BPMUpDown
            // 
            this.BPMUpDown.DecimalPlaces = 1;
            this.BPMUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.BPMUpDown.Location = new System.Drawing.Point(447, 248);
            this.BPMUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BPMUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BPMUpDown.Name = "BPMUpDown";
            this.BPMUpDown.Size = new System.Drawing.Size(121, 19);
            this.BPMUpDown.TabIndex = 18;
            this.BPMUpDown.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(354, 140);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(213, 19);
            this.textBoxID.TabIndex = 19;
            this.textBoxID.Text = "default ID";
            // 
            // textBoxWAVE
            // 
            this.textBoxWAVE.Location = new System.Drawing.Point(354, 165);
            this.textBoxWAVE.Name = "textBoxWAVE";
            this.textBoxWAVE.ReadOnly = true;
            this.textBoxWAVE.Size = new System.Drawing.Size(184, 19);
            this.textBoxWAVE.TabIndex = 20;
            // 
            // textBoxJacket
            // 
            this.textBoxJacket.Location = new System.Drawing.Point(354, 219);
            this.textBoxJacket.Name = "textBoxJacket";
            this.textBoxJacket.ReadOnly = true;
            this.textBoxJacket.Size = new System.Drawing.Size(184, 19);
            this.textBoxJacket.TabIndex = 21;
            // 
            // buttonWave
            // 
            this.buttonWave.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonWave.Location = new System.Drawing.Point(544, 165);
            this.buttonWave.Name = "buttonWave";
            this.buttonWave.Size = new System.Drawing.Size(23, 23);
            this.buttonWave.TabIndex = 22;
            this.buttonWave.Text = "...";
            this.buttonWave.UseVisualStyleBackColor = true;
            this.buttonWave.Click += new System.EventHandler(this.buttonWave_Click);
            // 
            // buttonJacket
            // 
            this.buttonJacket.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonJacket.Location = new System.Drawing.Point(544, 219);
            this.buttonJacket.Name = "buttonJacket";
            this.buttonJacket.Size = new System.Drawing.Size(23, 23);
            this.buttonJacket.TabIndex = 23;
            this.buttonJacket.Text = "...";
            this.buttonJacket.UseVisualStyleBackColor = true;
            this.buttonJacket.Click += new System.EventHandler(this.buttonJacket_Click);
            // 
            // offsetUpDown
            // 
            this.offsetUpDown.DecimalPlaces = 3;
            this.offsetUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.offsetUpDown.Location = new System.Drawing.Point(447, 194);
            this.offsetUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.offsetUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.offsetUpDown.Name = "offsetUpDown";
            this.offsetUpDown.Size = new System.Drawing.Size(120, 19);
            this.offsetUpDown.TabIndex = 24;
            // 
            // labelexdir
            // 
            this.labelexdir.AutoSize = true;
            this.labelexdir.Location = new System.Drawing.Point(254, 278);
            this.labelexdir.Name = "labelexdir";
            this.labelexdir.Size = new System.Drawing.Size(83, 12);
            this.labelexdir.TabIndex = 25;
            this.labelexdir.Text = "Export Directry";
            // 
            // buttonexdir
            // 
            this.buttonexdir.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonexdir.Location = new System.Drawing.Point(544, 273);
            this.buttonexdir.Name = "buttonexdir";
            this.buttonexdir.Size = new System.Drawing.Size(23, 23);
            this.buttonexdir.TabIndex = 26;
            this.buttonexdir.Text = "...";
            this.buttonexdir.UseVisualStyleBackColor = true;
            this.buttonexdir.Click += new System.EventHandler(this.buttonexdir_Click);
            // 
            // textBoxExport
            // 
            this.textBoxExport.Location = new System.Drawing.Point(354, 275);
            this.textBoxExport.Name = "textBoxExport";
            this.textBoxExport.ReadOnly = true;
            this.textBoxExport.Size = new System.Drawing.Size(184, 19);
            this.textBoxExport.TabIndex = 27;
            // 
            // checkBoxWhile
            // 
            this.checkBoxWhile.AutoSize = true;
            this.checkBoxWhile.Checked = true;
            this.checkBoxWhile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWhile.Location = new System.Drawing.Point(395, 300);
            this.checkBoxWhile.Name = "checkBoxWhile";
            this.checkBoxWhile.Size = new System.Drawing.Size(173, 16);
            this.checkBoxWhile.TabIndex = 29;
            this.checkBoxWhile.Text = "開始時に1小節分の間を空ける";
            this.checkBoxWhile.UseVisualStyleBackColor = true;
            // 
            // playLevelUpDown
            // 
            this.playLevelUpDown.DecimalPlaces = 1;
            this.playLevelUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.playLevelUpDown.Location = new System.Drawing.Point(447, 115);
            this.playLevelUpDown.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.playLevelUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.playLevelUpDown.Name = "playLevelUpDown";
            this.playLevelUpDown.Size = new System.Drawing.Size(120, 19);
            this.playLevelUpDown.TabIndex = 30;
            this.playLevelUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBoxWE
            // 
            this.textBoxWE.Enabled = false;
            this.textBoxWE.Location = new System.Drawing.Point(528, 89);
            this.textBoxWE.Name = "textBoxWE";
            this.textBoxWE.Size = new System.Drawing.Size(39, 19);
            this.textBoxWE.TabIndex = 31;
            // 
            // previewButton
            // 
            this.previewButton.Location = new System.Drawing.Point(256, 322);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(90, 23);
            this.previewButton.TabIndex = 33;
            this.previewButton.Text = "プレビュー更新";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // previewBox
            // 
            this.previewBox.BackgroundImage = global::NotesEditerforD.Properties.Resources.frameBasic;
            this.previewBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.previewBox.Location = new System.Drawing.Point(12, 24);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(230, 318);
            this.previewBox.TabIndex = 32;
            this.previewBox.TabStop = false;
            // 
            // previewTitle
            // 
            this.previewTitle.BackColor = System.Drawing.Color.Transparent;
            this.previewTitle.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewTitle.ForeColor = System.Drawing.Color.Black;
            this.previewTitle.Location = new System.Drawing.Point(25, 254);
            this.previewTitle.Name = "previewTitle";
            this.previewTitle.Size = new System.Drawing.Size(202, 19);
            this.previewTitle.TabIndex = 34;
            this.previewTitle.Text = "label1";
            this.previewTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // previewLevel
            // 
            this.previewLevel.BackColor = System.Drawing.Color.Transparent;
            this.previewLevel.Font = new System.Drawing.Font("Arial Unicode MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.previewLevel.ForeColor = System.Drawing.Color.Black;
            this.previewLevel.Location = new System.Drawing.Point(22, 217);
            this.previewLevel.Name = "previewLevel";
            this.previewLevel.Size = new System.Drawing.Size(40, 35);
            this.previewLevel.TabIndex = 34;
            this.previewLevel.Text = "1";
            this.previewLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // previewDesigner
            // 
            this.previewDesigner.BackColor = System.Drawing.Color.Transparent;
            this.previewDesigner.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewDesigner.ForeColor = System.Drawing.Color.White;
            this.previewDesigner.Location = new System.Drawing.Point(94, 321);
            this.previewDesigner.Name = "previewDesigner";
            this.previewDesigner.Size = new System.Drawing.Size(93, 17);
            this.previewDesigner.TabIndex = 34;
            this.previewDesigner.Text = "label1";
            this.previewDesigner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // previewBPM
            // 
            this.previewBPM.BackColor = System.Drawing.Color.Transparent;
            this.previewBPM.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewBPM.ForeColor = System.Drawing.Color.White;
            this.previewBPM.Location = new System.Drawing.Point(209, 321);
            this.previewBPM.Name = "previewBPM";
            this.previewBPM.Size = new System.Drawing.Size(34, 17);
            this.previewBPM.TabIndex = 34;
            this.previewBPM.Text = "120";
            this.previewBPM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // previewArtist
            // 
            this.previewArtist.BackColor = System.Drawing.Color.Transparent;
            this.previewArtist.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewArtist.ForeColor = System.Drawing.Color.Black;
            this.previewArtist.Location = new System.Drawing.Point(28, 272);
            this.previewArtist.Name = "previewArtist";
            this.previewArtist.Size = new System.Drawing.Size(198, 15);
            this.previewArtist.TabIndex = 34;
            this.previewArtist.Text = "label1";
            this.previewArtist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // previewWELevel
            // 
            this.previewWELevel.BackColor = System.Drawing.Color.Transparent;
            this.previewWELevel.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.previewWELevel.ForeColor = System.Drawing.Color.Gold;
            this.previewWELevel.Location = new System.Drawing.Point(18, 207);
            this.previewWELevel.Name = "previewWELevel";
            this.previewWELevel.Size = new System.Drawing.Size(209, 10);
            this.previewWELevel.TabIndex = 35;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 354);
            this.Controls.Add(this.previewWELevel);
            this.Controls.Add(this.previewLevel);
            this.Controls.Add(this.previewBPM);
            this.Controls.Add(this.previewDesigner);
            this.Controls.Add(this.previewArtist);
            this.Controls.Add(this.previewTitle);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.textBoxWE);
            this.Controls.Add(this.playLevelUpDown);
            this.Controls.Add(this.checkBoxWhile);
            this.Controls.Add(this.textBoxExport);
            this.Controls.Add(this.buttonexdir);
            this.Controls.Add(this.labelexdir);
            this.Controls.Add(this.offsetUpDown);
            this.Controls.Add(this.buttonJacket);
            this.Controls.Add(this.buttonWave);
            this.Controls.Add(this.textBoxJacket);
            this.Controls.Add(this.textBoxWAVE);
            this.Controls.Add(this.textBoxID);
            this.Controls.Add(this.BPMUpDown);
            this.Controls.Add(this.difficultyComboBox);
            this.Controls.Add(this.textBoxDesigner);
            this.Controls.Add(this.textBoxArtist);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.previewLabel);
            this.Controls.Add(this.bpmLabel);
            this.Controls.Add(this.jacketLabel);
            this.Controls.Add(this.offsetLabel);
            this.Controls.Add(this.waveLabel);
            this.Controls.Add(this.songidLabel);
            this.Controls.Add(this.levelLabel);
            this.Controls.Add(this.difficultyLabel);
            this.Controls.Add(this.designerLabel);
            this.Controls.Add(this.artistLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.export);
            this.Name = "Form2";
            this.Text = "エクスポート";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BPMUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playLevelUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button export;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label artistLabel;
        private System.Windows.Forms.Label designerLabel;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.Label levelLabel;
        private System.Windows.Forms.Label songidLabel;
        private System.Windows.Forms.Label waveLabel;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.Label jacketLabel;
        private System.Windows.Forms.Label bpmLabel;
        private System.Windows.Forms.Label previewLabel;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxArtist;
        private System.Windows.Forms.TextBox textBoxDesigner;
        private System.Windows.Forms.ComboBox difficultyComboBox;
        private System.Windows.Forms.NumericUpDown BPMUpDown;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.TextBox textBoxWAVE;
        private System.Windows.Forms.TextBox textBoxJacket;
        private System.Windows.Forms.Button buttonWave;
        private System.Windows.Forms.Button buttonJacket;
        private System.Windows.Forms.NumericUpDown offsetUpDown;
        private System.Windows.Forms.Label labelexdir;
        private System.Windows.Forms.Button buttonexdir;
        private System.Windows.Forms.TextBox textBoxExport;
        private System.Windows.Forms.CheckBox checkBoxWhile;
        private System.Windows.Forms.NumericUpDown playLevelUpDown;
        private System.Windows.Forms.TextBox textBoxWE;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Label previewTitle;
        private System.Windows.Forms.Label previewLevel;
        private System.Windows.Forms.Label previewDesigner;
        private System.Windows.Forms.Label previewBPM;
        private System.Windows.Forms.Label previewArtist;
        private System.Windows.Forms.Label previewWELevel;
    }
}