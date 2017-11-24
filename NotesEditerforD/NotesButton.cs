using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotesEditerforD
{
    public partial class NotesButton : UserControl
    {
        private string notesName = "";
        private Boolean isAir = true;
        private Boolean isActive = false;
        [Bindable(true)]
        [SettingsBindable(true)]
        public string NotesName { get { return notesName; } set { notesName = value; } }
        [SettingsBindable(true)]
        public Boolean IsAir { get { return isAir; } set { isAir = value; } }
        [SettingsBindable(true)]
        public Boolean IsActive { get { return isActive; } set { isActive = value; } }

        public NotesButton()
        {
            InitializeComponent();
            trackBar_size.Value = 4;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //最大値（16/16）、最小値（1/16）を設定
            trackBar_size.Maximum = 16;
            trackBar_size.Minimum = 1;

            //描写される目盛りの刻みを設定
            trackBar_size.TickFrequency = 1;

            //スライダーの移動量の設定
            trackBar_size.SmallChange = 1;
            trackBar_size.LargeChange = 4;

            label_sizevalue.Text = trackBar_size.Value.ToString() + " /16";

            if(isActive) MusicScore2.SelectedNoteSize = trackBar_size.Value;//ノーツボタンがアクティブなときにノーツサイズを指定
        }

        private void NotesButton_Load(object sender, EventArgs e)
        {
            setNotesName(notesName);
            setRadioButton(isAir);
        }

        private void NotesButton_Click(object sender, EventArgs e)
        {
            
        }
    
        public void setNotesName(string str)
        {
            this.label_notes.Text = str;
        }

        public void setRadioButton(Boolean bol)
        {
            this.radioR.Enabled = bol;
            this.radioC.Enabled = bol;
            this.radioL.Enabled = bol;
            if (bol) this.radioC.Checked = true;
        }

        private void setAirDirection()
        {
            if (radioR.Checked) MusicScore2.SelectedAirDirection = "Right";
            else if (radioL.Checked) MusicScore2.SelectedAirDirection = "Left";
            else MusicScore2.SelectedAirDirection = "Center";
        }

        private void radioLCR_Click(object sender, EventArgs e)
        {
            if (isActive) setAirDirection();
        }
        

        private void radioC_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label_notes_Click(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.ActiveBorder;
            MusicScore2.SelectedNoteSize = trackBar_size.Value;
            MusicScore2.SelectedNoteStyle = notesName;
            setAirDirection();
            isActive = true;
            Form1.activeNotesButton(this);
        }

        public void notesButtonActive()//for initialize only(maybe)
        {
            this.BackColor = SystemColors.ActiveBorder;
            MusicScore2.SelectedNoteSize = trackBar_size.Value;
            MusicScore2.SelectedNoteStyle = notesName;
            isActive = true;
        }

        public void notesButtonInactive()
        {
            this.BackColor = SystemColors.Control;
            isActive = false;
        }

        private void radioL_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioR_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
