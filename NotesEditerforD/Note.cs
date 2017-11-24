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
    public partial class Note : UserControl
    {
        public MusicScore musicscore;
        private Point position, startPoint, endPoint;
        private Size size;
        private string noteStyle, airDirection;
        private Note childNote, parentNote;
        private bool isChildNote, isParentNote;
        private System.Drawing.Drawing2D.GraphicsPath path;
        private Point[] points;
        private int longNoteNumber, airLineSize;
        public List<Note> coes = new List<Note>();
        public Note()
        {
            InitializeComponent();
            position = new Point(0, 0);
            size = new Size(40, 5);
            noteStyle = "Tap";
            airDirection = "Center";
            childNote = null;
            parentNote = null;
            isChildNote = false;
            isParentNote = false;
            path = new System.Drawing.Drawing2D.GraphicsPath();
            points = new Point[4];
            longNoteNumber = -1;
        }

        public System.Drawing.Drawing2D.GraphicsPath Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        public int LongNoteNumber
        {
            get { return this.longNoteNumber; }
            set { this.longNoteNumber = value; }
        }

        public Point[] Points
        {
            get { return this.points; }
            set { this.points = value; }
        }

        public Note ChildNote
        {
            get { return this.childNote; }
            set { this.childNote = value; }
        }

        public Note ParentNote
        {
            get { return this.parentNote; }
            set { this.parentNote = value; }
        }

        public bool IsChildNote
        {
            get { return this.isChildNote; }
            set { this.isChildNote = value; }
        }

        public bool IsParentNote
        {
            get { return this.isParentNote; }
            set { this.isParentNote = value; }
        }

        public Point NotePosition
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Point StartPoint
        {
            get { return this.startPoint; }
            set { this.startPoint = value; }
        }

        public Point EndPoint
        {
            get { return this.endPoint; }
            set { this.endPoint = value; }
        }

        public Size NoteSize
        {
            get { return this.size; }
            set { this.size = value; }
        }

        public int AirLineSize
        {
            get { return this.airLineSize; }
            set { this.airLineSize = value; }
        }

        public string NoteStyle
        {
            get { return this.noteStyle; }
            set { this.noteStyle = value; }
        }

        public string AirDirection
        {
            get { return this.airDirection; }
            set { this.airDirection = value; }
        }

        private void Note_Click(object sender, MouseEventArgs e)
        {
            MusicScore.IsBackNote = true;
            musicscore.MusicScore_MouseClick(sender, e);
            MusicScore.IsBackNote = false;
            if (MusicScore.EditStatus == "Delete")
            {
                musicscore.deleteNote(this);
            }
        }

        private void Note_MouseMove(object sender, MouseEventArgs e)
        {
            MusicScore.IsBackNote = true;
            musicscore.MusicScore_MouseMove(sender, e);
            MusicScore.IsBackNote = false;
        }

        private void Note_MouseUp(object sender, MouseEventArgs e)
        {
            MusicScore.IsBackNote = true;
            musicscore.MusicScore_MouseUp(sender, e);
            MusicScore.IsBackNote = false;
        }
    }
}