namespace NotesEditerforD
{
    partial class NotesButton
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_notes = new System.Windows.Forms.Label();
            this.trackBar_size = new System.Windows.Forms.TrackBar();
            this.label_size = new System.Windows.Forms.Label();
            this.label_sizevalue = new System.Windows.Forms.Label();
            this.radioL = new System.Windows.Forms.RadioButton();
            this.radioC = new System.Windows.Forms.RadioButton();
            this.radioR = new System.Windows.Forms.RadioButton();
            this.notesPreview = new System.Windows.Forms.PictureBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notesPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label_notes
            // 
            this.label_notes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_notes.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_notes.Location = new System.Drawing.Point(121, 11);
            this.label_notes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_notes.Name = "label_notes";
            this.label_notes.Size = new System.Drawing.Size(94, 23);
            this.label_notes.TabIndex = 1;
            this.label_notes.Text = "NotesStyle";
            this.label_notes.Click += new System.EventHandler(this.label_notes_Click);
            // 
            // trackBar_size
            // 
            this.trackBar_size.Location = new System.Drawing.Point(123, 48);
            this.trackBar_size.Margin = new System.Windows.Forms.Padding(2);
            this.trackBar_size.Maximum = 16;
            this.trackBar_size.Minimum = 1;
            this.trackBar_size.Name = "trackBar_size";
            this.trackBar_size.Size = new System.Drawing.Size(102, 45);
            this.trackBar_size.TabIndex = 2;
            this.trackBar_size.Value = 1;
            this.trackBar_size.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label_size
            // 
            this.label_size.AutoSize = true;
            this.label_size.Location = new System.Drawing.Point(121, 34);
            this.label_size.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_size.Name = "label_size";
            this.label_size.Size = new System.Drawing.Size(26, 12);
            this.label_size.TabIndex = 3;
            this.label_size.Text = "Size";
            // 
            // label_sizevalue
            // 
            this.label_sizevalue.AutoSize = true;
            this.label_sizevalue.Location = new System.Drawing.Point(182, 34);
            this.label_sizevalue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_sizevalue.Name = "label_sizevalue";
            this.label_sizevalue.Size = new System.Drawing.Size(33, 12);
            this.label_sizevalue.TabIndex = 4;
            this.label_sizevalue.Text = "4 /16";
            // 
            // radioL
            // 
            this.radioL.AutoSize = true;
            this.radioL.Location = new System.Drawing.Point(123, 79);
            this.radioL.Margin = new System.Windows.Forms.Padding(2);
            this.radioL.Name = "radioL";
            this.radioL.Size = new System.Drawing.Size(29, 16);
            this.radioL.TabIndex = 5;
            this.radioL.TabStop = true;
            this.radioL.Text = "L";
            this.radioL.UseVisualStyleBackColor = true;
            this.radioL.CheckedChanged += new System.EventHandler(this.radioL_CheckedChanged);
            this.radioL.Click += new System.EventHandler(this.radioLCR_Click);
            // 
            // radioC
            // 
            this.radioC.AutoSize = true;
            this.radioC.Location = new System.Drawing.Point(156, 79);
            this.radioC.Margin = new System.Windows.Forms.Padding(2);
            this.radioC.Name = "radioC";
            this.radioC.Size = new System.Drawing.Size(31, 16);
            this.radioC.TabIndex = 6;
            this.radioC.TabStop = true;
            this.radioC.Text = "C";
            this.radioC.UseVisualStyleBackColor = true;
            this.radioC.CheckedChanged += new System.EventHandler(this.radioC_CheckedChanged);
            this.radioC.Click += new System.EventHandler(this.radioLCR_Click);
            // 
            // radioR
            // 
            this.radioR.AutoSize = true;
            this.radioR.Location = new System.Drawing.Point(190, 79);
            this.radioR.Margin = new System.Windows.Forms.Padding(2);
            this.radioR.Name = "radioR";
            this.radioR.Size = new System.Drawing.Size(31, 16);
            this.radioR.TabIndex = 7;
            this.radioR.TabStop = true;
            this.radioR.Text = "R";
            this.radioR.UseVisualStyleBackColor = true;
            this.radioR.CheckedChanged += new System.EventHandler(this.radioR_CheckedChanged);
            this.radioR.Click += new System.EventHandler(this.radioLCR_Click);
            // 
            // notesPreview
            // 
            this.notesPreview.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.notesPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.notesPreview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.notesPreview.Location = new System.Drawing.Point(2, 11);
            this.notesPreview.Margin = new System.Windows.Forms.Padding(2);
            this.notesPreview.Name = "notesPreview";
            this.notesPreview.Size = new System.Drawing.Size(115, 83);
            this.notesPreview.TabIndex = 0;
            this.notesPreview.TabStop = false;
            this.notesPreview.Click += new System.EventHandler(this.label_notes_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(124, 55);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(97, 19);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // NotesButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.radioR);
            this.Controls.Add(this.radioC);
            this.Controls.Add(this.radioL);
            this.Controls.Add(this.label_sizevalue);
            this.Controls.Add(this.label_size);
            this.Controls.Add(this.trackBar_size);
            this.Controls.Add(this.label_notes);
            this.Controls.Add(this.notesPreview);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NotesButton";
            this.Size = new System.Drawing.Size(227, 105);
            this.Load += new System.EventHandler(this.NotesButton_Load);
            this.Click += new System.EventHandler(this.label_notes_Click);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notesPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox notesPreview;
        private System.Windows.Forms.TrackBar trackBar_size;
        private System.Windows.Forms.Label label_size;
        private System.Windows.Forms.Label label_sizevalue;
        protected System.Windows.Forms.Label label_notes;
        private System.Windows.Forms.RadioButton radioL;
        private System.Windows.Forms.RadioButton radioC;
        private System.Windows.Forms.RadioButton radioR;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}
