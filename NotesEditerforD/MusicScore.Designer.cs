﻿namespace NotesEditerforD
{
    partial class MusicScore
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
            this.SuspendLayout();
            // 
            // MusicScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NotesEditerforD.Properties.Resources.MusicScore;
            this.Margin = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.Name = "MusicScore";
            this.Size = new System.Drawing.Size(170, 778);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MusicScore_MouseClick);
            this.MouseEnter += new System.EventHandler(this.MusicScore_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MusicScore_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MusicScore_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MusicScore_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
