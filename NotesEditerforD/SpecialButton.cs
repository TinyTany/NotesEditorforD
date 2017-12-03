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
    public partial class SpecialButton : UserControl
    {
        public string NoteName
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public decimal MaxValue
        {
            get { return numericUpDown1.Maximum; }
            set { numericUpDown1.Maximum = value; }
        }

        public decimal MinValue
        {
            get { return numericUpDown1.Minimum; }
            set { numericUpDown1.Minimum = value; }
        }

        public string Label
        {
            get { return this.label2.Text; }
            set { this.label2.Text = value; }
        }

        public SpecialButton()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.ActiveBorder;
            MusicScore2.SelectedNoteStyle = label1.Text;
            Form1.activeNotesButton(this);
        }

        public void notesButtonInactive()
        {
            this.BackColor = SystemColors.Control;
        }

        private void SpecialButton_Load(object sender, EventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
