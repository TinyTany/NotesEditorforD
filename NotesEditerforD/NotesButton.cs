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
        private bool isNumUpDown;
        [Bindable(true)]
        [SettingsBindable(true)]
        public string NotesName { get { return notesName; } set { notesName = value; } }
        [SettingsBindable(true)]
        public Boolean IsAir { get { return isAir; } set { isAir = value; } }
        [SettingsBindable(true)]
        public Boolean IsActive { get { return isActive; } set { isActive = value; } }
        public Image CImage { get { return notesPreview.Image; } set { notesPreview.Image = value; } }
        public bool IsNumUpDown { get { return isNumUpDown; } set { isNumUpDown = value; } }


        public NotesButton()
        {
            InitializeComponent();
            trackBar_size.Value = 4;
        }

        public int TrackBar_Size
        {
            get { return trackBar_size.Value; }
            set { trackBar_size.Value = value; }
        }

        public decimal NumUDMax
        {
            get { return numericUpDown1.Maximum; }
            set { numericUpDown1.Maximum = value; }
        }

        public decimal NumUDMin
        {
            get { return numericUpDown1.Minimum; }
            set { numericUpDown1.Minimum = value; }
        }

        public decimal NumUDValue
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar_size.Enabled)
            {
                label_sizevalue.Text = trackBar_size.Value.ToString() + " /16";
                if (isActive) MusicScore2.SelectedNoteSize = trackBar_size.Value;//ノーツボタンがアクティブなときにノーツサイズを指定
            }
        }

        private void NotesButton_Load(object sender, EventArgs e)
        {
            if (isNumUpDown)
            {
                trackBar_size.Enabled = false;
                trackBar_size.Visible = false;
            }
            else
            {
                numericUpDown1.Enabled = false;
                numericUpDown1.Visible = false;
            }
            setNotesName(notesName);
            setRadioButton(isAir);
            if(notesName == "BPM")
            {
                numericUpDown1.Maximum = 300;
                numericUpDown1.Minimum = 30;
                numericUpDown1.DecimalPlaces = 1;
                numericUpDown1.Increment = 0.1m;
                numericUpDown1.Value = 120.0m;
                MusicScore2.SelectedBPM = numericUpDown1.Value;
                label_size.Text = "";
                label_sizevalue.Text = numericUpDown1.Value.ToString();
            }
            else if(notesName == "Speed")
            {
                numericUpDown1.Maximum = 10;
                numericUpDown1.Minimum = -10;
                numericUpDown1.DecimalPlaces = 1;
                numericUpDown1.Increment = 0.1m;
                numericUpDown1.Value = 1.0m;
                MusicScore2.SelectedSpeed = numericUpDown1.Value;
                label_size.Text = "";
                label_sizevalue.Text = "x" + numericUpDown1.Value.ToString();
            }
        }

        public void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(notesName == "BPM")
            {
                label_sizevalue.Text = numericUpDown1.Value.ToString();
                MusicScore2.SelectedBPM = numericUpDown1.Value;
            }
            else if(notesName == "Speed")
            {
                label_sizevalue.Text = "x" + numericUpDown1.Value.ToString();
                MusicScore2.SelectedSpeed = numericUpDown1.Value;
            }
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

        public void setDirection(string direction)
        {
            if (direction == "Left") radioL.Checked = true;
            else if (direction == "Center") radioC.Checked = true;
            else if (direction == "Right") radioR.Checked = true;
            else radioC.Checked = true;
        }

        public void setAirDirection()
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
            //this.BackColor = SystemColors.ActiveBorder;
            //MusicScore2.SelectedNoteSize = trackBar_size.Value;
            //MusicScore2.SelectedNoteStyle = notesName;
            //setAirDirection();
            //isActive = true;
            //notesButtonActive();
            Form1.activeNotesButton(this);
            if (label_notes.Text == "BPM" || label_notes.Text == "Speed") MusicScore2.SelectedNoteSize = 16;
        }

        public void notesButtonActive()//for initialize only(maybe)
        {
            this.BackColor = SystemColors.ActiveBorder;
            MusicScore2.SelectedNoteSize = trackBar_size.Value;
            MusicScore2.SelectedNoteStyle = notesName;
            setAirDirection();
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
