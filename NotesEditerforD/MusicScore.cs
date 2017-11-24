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
    public partial class MusicScore : UserControl
    {
        private static int beat, grid, noteSize;
        private static string noteStyle, editStatus, airDirection;
        private static Boolean isBackNote;
        private Point startPoint, endPoint;
        private Boolean mouseStatus;
        private Note note, backNote, startNote;
        private Note[] prvNote;
        private int indx, tmpLongNotesNumber;
        public Form1 form1;
        public List<Note> notes = new List<Note>();
        public MusicScore()
        {
            InitializeComponent();
            beat = 8;
            grid = 8;
            noteSize = 4;
            editStatus = "Add";
            airDirection = "Center";
            mouseStatus = false;
            isBackNote = false;
            prvNote = new Note[2];//[0] for LineNotes preview, [1] for Note preview in MusicScore
        }

        public int Indx
        {
            get { return this.indx; }
            set { this.indx = value; }
        }

        public static int Beat
        {
            get { return beat; }
            set { beat = value; }
        }

        public static int Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        public static int NoteSize
        {
            get { return noteSize; }
            set { noteSize = value; }
        }

        public static string NoteStyle
        {
            get { return noteStyle; }
            set { noteStyle = value; }
        }

        public static string EditStatus
        {
            get { return editStatus; }
            set { editStatus = value; }
        }

        public static string AirDirection
        {
            get { return airDirection; }
            set { airDirection = value; }
        }

        public static Boolean IsBackNote
        {
            get { return isBackNote; }
            set { isBackNote = value; }
        }

        public void MusicScore_MouseClick(object sender, MouseEventArgs e)
        {
            form1.setEdited(true);
            if (editStatus == "Add")
            {
                if (sender.ToString() == "NotesEditerforD.Note") backNote = (Note)sender;
                if (noteStyle != "AirLine") setNote(e, noteStyle, true);
                mouseStatus = true;
                startPoint = locationize(e.Location);
                switch (noteStyle)
                {
                    case "Hold":
                        this.startNote = note;
                        startNote.LongNoteNumber = form1.LongNoteNumber;
                        tmpLongNotesNumber = startNote.LongNoteNumber;
                        foreach (Note note in notes)
                        {
                            if (note.NotePosition == locationize(e.Location) && note.NoteSize.Width == 10 * noteSize && note.NoteStyle == "HoldEnd" && note != startNote)
                            {
                                tmpLongNotesNumber = note.LongNoteNumber;
                                deleteNote(note);
                                deleteNote(startNote);
                                break;
                            }
                        }
                        if (editStatus == "Add")
                        {
                            setNote(e, "HoldLine", false);//NotePreview
                            prvNote[0] = note;
                        }
                        break;
                    case "Slide":
                        this.startNote = note;
                        startNote.LongNoteNumber = form1.LongNoteNumber;
                        tmpLongNotesNumber = startNote.LongNoteNumber;
                        foreach (Note note in notes)
                        {
                            if (note.NotePosition == locationize(e.Location) && note.NoteSize.Width == 10 * noteSize && note.NoteStyle == "SlideEnd" && note != startNote)//
                            {
                                tmpLongNotesNumber = note.LongNoteNumber;
                                note.NoteStyle = "SlideTap";//later
                                deleteNote(startNote);
                                break;
                            }
                        }
                        if (editStatus == "Add")
                        {
                            setNote(e, "SlideLine", false);//notepreview
                            prvNote[0] = note;
                        }
                        break;
                    case "AirLine":
                        setNote(e, "Tap", true);
                        Note startNote2 = note;////////
                        setNote(e, "AirUp", true);
                        this.startNote = note;
                        tmpLongNotesNumber = form1.LongNoteNumber;
                        foreach (Note note in notes)
                        {
                            if (note.NotePosition == new Point(locationize(e.Location).X + 2, locationize(e.Location).Y) && note.NoteSize.Width == 10 * noteSize - 4 && note.NoteStyle == "AirEnd" && note != startNote)//
                            {
                                tmpLongNotesNumber = note.LongNoteNumber;
                                note.NoteStyle = "AirAction";//later
                                deleteNote(startNote);
                                deleteNote(startNote2);
                                break;
                            }
                        }
                        if (editStatus == "Add")
                        {
                            setNote(e, "AirLine", false);//notepreview
                            prvNote[0] = note;
                        }
                        break;
                }
            }
        }

        public void MusicScore_MouseMove(object sender, MouseEventArgs e)
        {
            if (prvNote[0] != null && mouseStatus && editStatus == "Add")
            {
                switch (prvNote[0].NoteStyle)
                {
                    case "HoldLine":
                        prvNote[0].NoteSize = new Size(10 * noteSize, startPoint.Y - locationize(e.Location).Y); prvNote[0].Size = prvNote[0].NoteSize;//
                        prvNote[0].BackgroundImage = Properties.Resources.HoldLine;
                        if (!isBackNote)
                        {
                            prvNote[0].NotePosition = new Point(startPoint.X, locationize(e.Location).Y);
                            prvNote[0].Location = prvNote[0].NotePosition;
                        }
                        else
                        {
                            //prvNote[0].NotePosition = new Point(startPoint.X + backNote.Location.X - 6, locationize(e.Location).Y + backNote.Location.Y - 2);
                            //prvNote[0].Location = prvNote[0].NotePosition;
                        }
                        break;
                    case "SlideLine":
                        prvNote[0].BackgroundImage = Properties.Resources.SlideLine;
                        if (startPoint.X < locationize(e.Location).X)
                        {
                            prvNote[0].SetBounds(startPoint.X, locationize(e.Location).Y, this.Width, this.Height);
                            prvNote[0].Points = new Point[]
                                { new Point(locationize(e.Location).X - startPoint.X + 2, 0),
                              new Point(0 + 2, startPoint.Y - locationize(e.Location).Y),
                              new Point(10 * noteSize - 2, startPoint.Y - locationize(e.Location).Y),
                              new Point(locationize(e.Location).X - startPoint.X + 10 * noteSize - 2, 0) };
                        }
                        else
                        {
                            prvNote[0].SetBounds(locationize(e.Location).X, locationize(e.Location).Y, this.Width, this.Height);
                            prvNote[0].Points = new Point[]
                                { new Point(0 + 2, 0),
                              new Point(startPoint.X - locationize(e.Location).X + 2, startPoint.Y - locationize(e.Location).Y),
                              new Point(startPoint.X - locationize(e.Location).X + 10 * noteSize - 2, startPoint.Y - locationize(e.Location).Y),
                              new Point(10 * noteSize - 2, 0) };
                        }
                        byte[] types =
                            { (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line };
                        prvNote[0].Path = new System.Drawing.Drawing2D.GraphicsPath(prvNote[0].Points, types);
                        prvNote[0].Region = new Region(prvNote[0].Path);

                        if (!isBackNote)
                        {
                            if (startPoint.X < locationize(e.Location).X) prvNote[0].NotePosition = new Point(startPoint.X, locationize(e.Location).Y);
                            else prvNote[0].NotePosition = new Point(locationize(e.Location).X, locationize(e.Location).Y);
                            prvNote[0].Location = prvNote[0].NotePosition;
                        }
                        else
                        {
                            //if (startPoint.X < locationize(e.Location).X) prvNote[0].NotePosition = new Point(startPoint.X + backNote.Location.X - 6, locationize(e.Location).Y + backNote.Location.Y - 2);
                            //else prvNote[0].NotePosition = new Point(locationize(e.Location).X + backNote.Location.X - 6, locationize(e.Location).Y + backNote.Location.Y - 2);
                            //prvNote[0].Location = prvNote[0].NotePosition;
                        }
                        break;
                    case "AirLine":
                        if (!isBackNote)
                        {
                            prvNote[0].NotePosition = new Point(startPoint.X + 5 * noteSize - 4, locationize(e.Location).Y);
                            prvNote[0].Location = prvNote[0].NotePosition;
                        }
                        else
                        {
                            //note.NotePosition = new Point(startPoint.X + backNote.Location.X - 6 + 5 * noteSize - 4, endPoint.Y + backNote.Location.Y - 2);
                            //note.Location = note.NotePosition;
                        }
                        prvNote[0].NoteSize = new Size(8, startPoint.Y - locationize(e.Location).Y); prvNote[0].Size = prvNote[0].NoteSize;//
                        break;
                    default:
                        break;
                }
            }
            else if (!mouseStatus && prvNote[1] != null)
            {
                if (noteStyle == "AirUp" || noteStyle == "AirDown") prvNote[1].Location = new Point(locationize(e.Location).X, locationize(e.Location).Y - 32);
                else prvNote[1].Location = locationize(e.Location);
            }
        }

        public void MusicScore_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseStatus)
            {
                if (sender.ToString() == "NotesEditerforD.Note") backNote = (Note)sender;
                endPoint = locationize(e.Location);
                if (noteStyle == "Slide")
                {
                    if (startPoint.Y <= endPoint.Y) deleteNote(startNote);
                    else
                    {
                        setNote(e, "SlideLine", true);
                        //note.LongNoteNumber = tmpLongNotesNumber;
                        setNote(e, "SlideEnd", true);
                        //note.LongNoteNumber = tmpLongNotesNumber;
                        form1.LongNoteNumber++;
                    }
                }
                else if (noteStyle == "Hold")
                {
                    if (startPoint.Y <= endPoint.Y) deleteNote(startNote);
                    else
                    {
                        setNote(e, "HoldLine", true);
                        //note.LongNoteNumber = tmpLongNotesNumber;
                        setNote(e, "HoldEnd", true);
                        //note.LongNoteNumber = tmpLongNotesNumber;
                        form1.LongNoteNumber++;
                    }
                }
                else if (noteStyle == "AirLine")
                {
                    setNote(e, "AirLine", true);
                    //note.LongNoteNumber = tmpLongNotesNumber;
                    setNote(e, "AirEnd", true);
                    //note.LongNoteNumber = tmpLongNotesNumber;
                    form1.LongNoteNumber++;
                }
            }
            mouseStatus = false;
            if (prvNote[0] != null) prvNote[0].Dispose();
        }

        private void MusicScore_MouseEnter(object sender, EventArgs e)
        {
            if(editStatus == "Add") {
                MouseEventArgs me = new MouseEventArgs(MouseButtons.Left, 0, 5, 100, 0);
                if (noteStyle == "AirLine") setNote(me, "Tap", false);
                else setNote(me, noteStyle, false);
                prvNote[1] = note;
            }
        }

        private void MusicScore_MouseLeave(object sender, EventArgs e)
        {
            if(prvNote[1] != null) prvNote[1].Dispose();
        }

        private void NotesLayer_Click(object sender, EventArgs e)
        {

        }

        public int getNotesCount()
        {
            return notes.Count;
        }

        private void setNote(MouseEventArgs e, string noteStyle, bool isListed)
        {
            this.note = new Note();

            note.NoteSize = new Size(10 * noteSize, 5);
            note.Size = note.NoteSize;
            note.StartPoint = startPoint;
            note.EndPoint = endPoint;

            if (!isBackNote) { note.NotePosition = locationize(e.Location); note.Location = note.NotePosition; }
            else
            {
                note.NotePosition = new Point(locationize(e.Location).X + backNote.Location.X - 6, locationize(e.Location).Y + backNote.Location.Y - 2);
                note.Location = note.NotePosition;
            }

            note.NoteStyle = noteStyle;
            note.AirDirection = airDirection;

            switch (noteStyle)
            {
                case "Tap":
                    note.BackgroundImage = Properties.Resources.Tap;
                    break;
                case "ExTap":
                    note.BackgroundImage = Properties.Resources.ExTap;
                    break;
                case "Flick":
                    note.BackgroundImage = Properties.Resources.Flick;
                    break;
                case "Slide":
                    note.BackgroundImage = Properties.Resources.Slide;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    break;
                case "SlideTap":
                    note.BackgroundImage = Properties.Resources.SlideTap;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    break;
                case "SlideEnd"://
                    note.BackgroundImage = Properties.Resources.SlideTap;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    break;
                case "Hold":
                    note.BackgroundImage = Properties.Resources.Hold;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    break;
                case "HoldEnd"://
                    note.BackgroundImage = Properties.Resources.HoldEnd;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    if (!isBackNote) { note.NotePosition = new Point(startPoint.X, endPoint.Y); note.Location = note.NotePosition; }
                    else
                    {
                        note.NotePosition = new Point(startPoint.X + backNote.Location.X - 6, endPoint.Y + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }
                    break;
                case "AirAction":
                    note.BackgroundImage = Properties.Resources.AirAction;
                    note.NoteSize = new Size(10 * noteSize - 4, 3); note.Size = note.NoteSize;
                    if (!isBackNote) { note.NotePosition = new Point(locationize(e.Location).X + 2, locationize(e.Location).Y ); note.Location = note.NotePosition; }
                    else
                    {
                        note.NotePosition = new Point(locationize(e.Location).X + backNote.Location.X - 6 + 2, locationize(e.Location).Y + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }
                    break;
                case "AirEnd":
                    note.BackgroundImage = Properties.Resources.AirAction;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    note.NoteSize = new Size(10 * noteSize - 4, 3); note.Size = note.NoteSize;
                    if (!isBackNote) { note.NotePosition = new Point(startPoint.X + 2, locationize(e.Location).Y); note.Location = note.NotePosition; }
                    else
                    {
                        note.NotePosition = new Point(startPoint.X + backNote.Location.X - 6 + 2, locationize(e.Location).Y + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }
                    break;
                case "AirUp":
                    note.NoteSize = new Size(10 * noteSize, 30); note.Size = note.NoteSize;
                    if (!isBackNote)
                    {
                        note.NotePosition = new Point(locationize(e.Location).X, locationize(e.Location).Y - 32);
                        note.Location = note.NotePosition;
                    }
                    else
                    {
                        note.NotePosition = new Point(locationize(e.Location).X + backNote.Location.X - 6, locationize(e.Location).Y - 32 + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }
                    switch (airDirection)
                    {
                        case "Center":
                            note.BackgroundImage = Properties.Resources.AirUpC;
                            break;
                        case "Left":
                            note.BackgroundImage = Properties.Resources.AirUpL;
                            break;
                        case "Right":
                            note.BackgroundImage = Properties.Resources.AirUpR;
                            break;
                        default:
                            note.BackgroundImage = Properties.Resources.AirUpC;
                            break;
                    }
                    break;
                case "AirDown":
                    note.NoteSize = new Size(10 * noteSize, 30); note.Size = note.NoteSize;
                    if (!isBackNote)
                    {
                        note.NotePosition = new Point(locationize(e.Location).X, locationize(e.Location).Y - 32);
                        note.Location = note.NotePosition;
                    }
                    else
                    {
                        note.NotePosition = new Point(locationize(e.Location).X + backNote.Location.X - 6, locationize(e.Location).Y - 32 + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }
                    switch (airDirection)
                    {

                        case "Center":
                            note.BackgroundImage = Properties.Resources.AirDownC;
                            break;
                        case "Left":
                            note.BackgroundImage = Properties.Resources.AirDownL;
                            break;
                        case "Right":
                            note.BackgroundImage = Properties.Resources.AirDownR;
                            break;
                        default:
                            note.BackgroundImage = Properties.Resources.AirUpC;
                            break;
                    }
                    break;
                case "HoldLine":
                    note.NoteSize = new Size(10 * noteSize, startPoint.Y - endPoint.Y); note.Size = note.NoteSize;//
                    note.BackgroundImage = Properties.Resources.HoldLine;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    if (!isBackNote)
                    {
                        note.NotePosition = new Point(startPoint.X, endPoint.Y);
                        note.Location = note.NotePosition;
                    }
                    else
                    {
                        note.NotePosition = new Point(startPoint.X + backNote.Location.X - 6, endPoint.Y + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }
                    break;
                case "SlideLine":
                    note.BackgroundImage = Properties.Resources.SlideLine;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    if (startPoint.X < endPoint.X)
                    {
                        note.SetBounds(startPoint.X, endPoint.Y, this.Width, this.Height);
                        note.Points = new Point[]
                            { new Point(endPoint.X - startPoint.X + 2, 0),
                              new Point(0 + 2, startPoint.Y - endPoint.Y),
                              new Point(10 * noteSize - 2, startPoint.Y - endPoint.Y),
                              new Point(endPoint.X - startPoint.X + 10 * noteSize - 2, 0) };
                    }
                    else
                    {
                        note.SetBounds(endPoint.X, endPoint.Y, this.Width, this.Height);
                        note.Points = new Point[]
                            { new Point(0 + 2, 0),
                              new Point(startPoint.X - endPoint.X + 2, startPoint.Y - endPoint.Y),
                              new Point(startPoint.X - endPoint.X + 10 * noteSize - 2, startPoint.Y - endPoint.Y),
                              new Point(10 * noteSize - 2, 0) };
                    }
                    byte[] types =
                        { (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line };
                    note.Path = new System.Drawing.Drawing2D.GraphicsPath(note.Points, types);
                    note.Region = new Region(note.Path);

                    if (!isBackNote)
                    {
                        if (startPoint.X < endPoint.X) note.NotePosition = new Point(startPoint.X, endPoint.Y);
                        else note.NotePosition = new Point(endPoint.X, endPoint.Y);
                        note.Location = note.NotePosition;
                    }
                    else
                    {
                        if (startPoint.X < endPoint.X) note.NotePosition = new Point(startPoint.X + backNote.Location.X - 6, endPoint.Y + backNote.Location.Y - 2);
                        else note.NotePosition = new Point(endPoint.X + backNote.Location.X - 6, endPoint.Y + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }

                    break;
                case "AirLine":
                    note.BackgroundImage = Properties.Resources.AirLine;
                    note.LongNoteNumber = tmpLongNotesNumber;
                    if (!isBackNote)
                    {
                        note.NotePosition = new Point(startPoint.X + 5 * noteSize - 4, endPoint.Y);
                        note.Location = note.NotePosition;
                    }
                    else
                    {
                        note.NotePosition = new Point(startPoint.X + backNote.Location.X - 6 + 5 * noteSize - 4, endPoint.Y + backNote.Location.Y - 2);
                        note.Location = note.NotePosition;
                    }
                    note.NoteSize = new Size(8, startPoint.Y - endPoint.Y); note.Size = note.NoteSize;//
                    note.AirLineSize = noteSize;
                    break;
                default:
                    note.BackgroundImage = Properties.Resources.Tap;
                    break;
            }

            note.musicscore = this;
            if(isListed) notes.Add(this.note);
            this.Controls.Add(this.note);
            this.note.BringToFront();
            form1.setTotalNotes();

            if (indx != form1.MaxScore - 1 && note.NotePosition.Y == 2 && note.NoteStyle != "SlideLine" && note.NoteStyle != "HoldLine" && note.NoteStyle != "AirLine")//上端
            {
                //
                Note parentnote = new Note();
                parentnote = cloneNote(note);
                parentnote.IsParentNote = true;
                note.IsChildNote = true;
                note.ParentNote = parentnote;
                parentnote.ChildNote = note;
                tmpLongNotesNumber = note.LongNoteNumber;
                notes.Remove(note);
                //
                parentnote.NotePosition = new Point(note.NotePosition.X, 770);
                parentnote.Location = parentnote.NotePosition;
                parentnote.LongNoteNumber = tmpLongNotesNumber;
                form1.setNote(indx + 1, parentnote);
            }
            else if (indx != 0 && note.NotePosition.Y == 770)//下端
            {
                //
                Note childnote = new Note();
                childnote = cloneNote(note);
                childnote.IsChildNote = true;
                note.IsParentNote = true;
                note.ChildNote = childnote;
                childnote.ParentNote = note;
                //
                childnote.NotePosition = new Point(note.NotePosition.X, 2);
                childnote.Location = childnote.NotePosition;
                form1.setNote(indx - 1, childnote);
            }
        }

        private Note cloneNote(Note _note)
        {
            Note note = new Note();
            note.NotePosition = _note.NotePosition;
            note.NoteSize = _note.NoteSize;
            note.AirLineSize = _note.AirLineSize;
            note.StartPoint = _note.StartPoint;
            note.EndPoint = _note.EndPoint;
            note.IsChildNote = _note.IsChildNote;
            note.ChildNote = _note.ChildNote;
            note.IsParentNote = _note.IsParentNote;
            note.NoteStyle = _note.NoteStyle;
            note.AirDirection = _note.AirDirection;
            return note;
        }

        public void setNote(Note note)
        {
            //this.note = new Note();

            //note.NoteSize = _note.NoteSize;
            note.Size = note.NoteSize;
            //note.StartPoint = _note.StartPoint;
            //note.EndPoint = _note.EndPoint;
            //note.IsChildNote = _note.IsChildNote;
            //note.ChildNote = _note.ChildNote;

            //note.NotePosition = _note.NotePosition;
            note.Location = note.NotePosition;
            
            //note.NoteStyle = _note.NoteStyle;
            //note.AirDirection = _note.AirDirection;

            switch (note.NoteStyle)
            {
                case "Tap":
                    note.BackgroundImage = Properties.Resources.Tap;
                    break;
                case "ExTap":
                    note.BackgroundImage = Properties.Resources.ExTap;
                    break;
                case "Flick":
                    note.BackgroundImage = Properties.Resources.Flick;
                    break;
                case "Slide":
                    note.BackgroundImage = Properties.Resources.Slide;
                    break;
                case "SlideTap":
                    note.BackgroundImage = Properties.Resources.SlideTap;
                    break;
                case "SlideEnd":
                    note.BackgroundImage = Properties.Resources.SlideTap;
                    break;
                case "Hold":
                    note.BackgroundImage = Properties.Resources.Hold;
                    break;
                case "HoldEnd"://
                    note.BackgroundImage = Properties.Resources.HoldEnd;
                    break;
                case "AirAction":
                    note.BackgroundImage = Properties.Resources.AirAction;
                    break;
                case "AirEnd":
                    note.BackgroundImage = Properties.Resources.AirAction;
                    break;
                case "AirUp":
                    switch (note.AirDirection)
                    {
                        case "Center":
                            note.BackgroundImage = Properties.Resources.AirUpC;
                            break;
                        case "Left":
                            note.BackgroundImage = Properties.Resources.AirUpL;
                            break;
                        case "Right":
                            note.BackgroundImage = Properties.Resources.AirUpR;
                            break;
                    }
                    break;
                case "AirDown":
                    switch (note.AirDirection)
                    {

                        case "Center":
                            note.BackgroundImage = Properties.Resources.AirDownC;
                            break;
                        case "Left":
                            note.BackgroundImage = Properties.Resources.AirDownL;
                            break;
                        case "Right":
                            note.BackgroundImage = Properties.Resources.AirDownR;
                            break;
                    }
                    break;
                case "HoldLine":
                    note.BackgroundImage = Properties.Resources.HoldLine;
                    break;
                case "SlideLine":
                    note.BackgroundImage = Properties.Resources.SlideLine;
                    Point[] points;
                    if (note.StartPoint.X < note.EndPoint.X)
                    {
                        note.SetBounds(note.StartPoint.X, note.EndPoint.Y, this.Width, this.Height);
                        points = new Point[]
                            { new Point(note.EndPoint.X - note.StartPoint.X + 2, 0),
                              new Point(0 + 2, note.StartPoint.Y - note.EndPoint.Y),
                              new Point(note.NoteSize.Width - 2, note.StartPoint.Y - note.EndPoint.Y),
                              new Point(note.EndPoint.X - note.StartPoint.X + note.NoteSize.Width - 2, 0) };
                    }
                    else
                    {
                        note.SetBounds(note.EndPoint.X, note.EndPoint.Y, this.Width, this.Height);
                        points = new Point[]
                            { new Point(0 + 2, 0),
                              new Point(note.StartPoint.X - note.EndPoint.X + 2, note.StartPoint.Y - note.EndPoint.Y),
                              new Point(note.StartPoint.X - note.EndPoint.X + note.NoteSize.Width - 2, note.StartPoint.Y - note.EndPoint.Y),
                              new Point(note.NoteSize.Width - 2, 0) };
                    }
                    byte[] types =
                        { (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line,
                          (byte)System.Drawing.Drawing2D.PathPointType.Line };
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(points, types);
                    note.Region = new Region(path);

                    //note.NotePosition = _note.NotePosition;
                    note.Location = note.NotePosition;
                    
                    break;
                case "AirLine":
                    note.BackgroundImage = Properties.Resources.AirLine;
                    break;
                default:
                    note.BackgroundImage = Properties.Resources.Tap;
                    break;
            }

            note.musicscore = this;
            if (!note.IsChildNote) notes.Add(note);
            this.Controls.Add(note);
            note.BringToFront();
            form1.setTotalNotes();
        }

        public void setNote(string[] noteData)
        {
            Note note = new Note();
            note.NoteStyle = noteData[0];//
            note.NoteSize = new Size(int.Parse(noteData[1]), int.Parse(noteData[2]));//
            note.NotePosition = new Point(int.Parse(noteData[3]), int.Parse(noteData[4]));//
            note.StartPoint = new Point(int.Parse(noteData[5]), int.Parse(noteData[6]));//
            note.EndPoint = new Point(int.Parse(noteData[7]), int.Parse(noteData[8]));//
            note.AirDirection = noteData[9];//
            note.LongNoteNumber = int.Parse(noteData[10]);//
            note.AirLineSize = int.Parse(noteData[11]);//
            setNote(note);

            if (indx != 99 && note.NotePosition.Y == 2)
            {
                //
                Note childnote = new Note();
                childnote = cloneNote(note);
                childnote.IsChildNote = true;
                note.IsParentNote = true;
                note.ChildNote = childnote;
                childnote.ParentNote = note;
                //
                childnote.NotePosition = new Point(note.NotePosition.X, 770);
                childnote.Location = childnote.NotePosition;
                form1.setNote(indx + 1, childnote);
            }
            else if (indx != 0 && note.NotePosition.Y == 770)
            {
                //
                Note childnote = new Note();
                childnote = cloneNote(note);
                childnote.IsChildNote = true;
                note.IsParentNote = true;
                note.ChildNote = childnote;
                childnote.ParentNote = note;
                //
                childnote.NotePosition = new Point(note.NotePosition.X, 2);
                childnote.Location = childnote.NotePosition;
                form1.setNote(indx - 1, childnote);
            }
        }

        public void deleteNote(Note note)
        {
            form1.setEdited(true);
            if (note.ChildNote != null) note.ChildNote.Dispose();
            if (note.ParentNote != null)
            {
                if (note.Location.Y == 2) form1.deleteNote(indx + 1, note.ParentNote);
                else form1.deleteNote(indx - 1, note.ParentNote);
            }
            notes.Remove(note);
            note.Dispose();
            form1.setTotalNotes();
        }

        private Point locationize(int X, int Y)
        {
            int noteX, noteY;
            if (X <= 165 - 10 * NoteSize) noteX = ((X - 5) / (10 * (16 / grid))) * (10 * (16 / grid)) + 5;
            else noteX = 165 - 10 * NoteSize;
            noteY = 778 - ((778 - Y - 5) / (768 / (2 * beat))) * (768 / (2 * beat)) - 5;
            noteX += 1; noteY += -3;//描写の都合上の位置調整
            return new Point(noteX, noteY);
        }

        private Point locationize(Point p)
        {
            int noteX, noteY;
            if (p.X <= 165 - 10 * NoteSize) noteX = ((p.X - 5) / (10 * (16 / grid))) * (10 * (16 / grid)) + 5;
            else noteX = 165 - 10 * NoteSize;
            noteY = 778 - ((778 - p.Y - 5) / (768 / (2 * beat))) * (768 / (2 * beat)) - 5;
            noteX += 1; noteY += -3;//描写の都合上の位置調整
            return new Point(noteX, noteY);
        }
    }
}
