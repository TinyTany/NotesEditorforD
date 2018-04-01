using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NotesEditerforD
{
    /// <summary>
    /// 矩形選択ようのやつ
    /// </summary>
    public class RectSelect
    {
        private Point rectUL, rectDR;
        private Rectangle rect;
        private List<ShortNote> notes, selectedNotes;
        private bool isPasted = false;
        //private Size rectSize;

        public RectSelect(Point _rectUL, Point _rectDR, List<ShortNote> _notes)
        {
            rectUL = _rectUL; rectDR = _rectDR;
            rect = new Rectangle(rectUL, new Size(rectDR.X - rectUL.X, rectDR.Y - rectUL.Y));
            notes = _notes;
            selectedNotes = new List<ShortNote>();
        }

        public bool IsPasted
        {
            get { return isPasted; }
            set { isPasted = value; }
        }

        public Point RectUL
        {
            get { return rectUL; }
            set { rectUL = value; }
        }

        public Point RectDR
        {
            get { return rectDR; }
            set { rectDR = value; }
        }

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        public Size rectSize
        {
            get { return rect.Size; }
            set { rect.Size = value; }
        }

        public List<ShortNote> SelectedNotes
        {
            get { return selectedNotes; }
            set { selectedNotes = value; }
        }

        public void update()
        {
            rect.Location = rectUL;
            rect.Size = new Size(rectDR.X - rectUL.X, rectDR.Y - rectUL.Y + 5);
            selectedNotes.Clear();
            foreach (ShortNote note in notes)
            {
                if (rect.Contains(new Rectangle(note.DestPoints[0], new Size(note.NoteSize * 10, 5))) &&
             note.NoteStyle != "HoldLine" && note.NoteStyle != "SlideLine" && note.NoteStyle != "AirLine")
                {
                    selectedNotes.Add(note);
                }
            }
        }

        /// <summary>
        /// 選択範囲をlocULを左上となるように移動します
        /// </summary>
        /// <param name="locUL">左上の座標</param>
        public void move(Point locUL)//
        {
            Size delta = new Size(locUL.X - rectUL.X, locUL.Y - rectUL.Y);
            ShortNote prev, next;
            foreach(ShortNote note in selectedNotes)
            {
                prev = next = null;
                if (new string[]{ "Slide", "SlideTap", "SlideRelay", "SlideEnd" }.Contains(note.NoteStyle))
                {
                    foreach(ShortNote _note in notes)
                    {
                        if (note.LongNoteNumber != _note.LongNoteNumber) continue;
                        if (_note == note) continue;
                        if (_note.NoteStyle != "SlideLine") continue;
                        if (note.NotePosition == _note.StartPosition) next = _note;
                        if (note.NotePosition == _note.EndPosition) prev = _note;
                    }
                }
                else if (new string[] { "Hold", "HoldEnd" }.Contains(note.NoteStyle))
                {
                    foreach (ShortNote _note in notes)
                    {
                        if (note.LongNoteNumber != _note.LongNoteNumber) continue;
                        if (_note == note) continue;
                        if (_note.NoteStyle != "HoldLine") continue;
                        if (note.NotePosition == _note.StartPosition) next = _note;
                        if (note.NotePosition == _note.EndPosition) prev = _note;
                    }
                }
                else if (new string[] { "AirBegin", "AirAction", "AirEnd" }.Contains(note.NoteStyle))
                {
                    foreach (ShortNote _note in notes)
                    {
                        if (note.LongNoteNumber != _note.LongNoteNumber) continue;
                        if (_note == note) continue;
                        if (_note.NoteStyle != "AirLine") continue;
                        if (note.NotePosition == _note.StartPosition) next = _note;
                        if (note.NotePosition == _note.EndPosition) prev = _note;
                    }
                }
                note.NotePosition = new Point(note.NotePosition.X + delta.Width, note.NotePosition.Y + delta.Height);
                if (prev != null) { prev.EndPosition = note.NotePosition; prev.update(); }
                if (next != null) { next.StartPosition = note.NotePosition; next.update(); }
                note.update();
            }
            rectUL = locUL;
            rect.Location = rectUL;
        }

        /// <summary>
        /// ノーツを左右反転します
        /// </summary>
        public void notesHorInv()
        {
            ShortNote prev, next;
            foreach (ShortNote note in selectedNotes)
            {
                prev = next = null;
                if (new string[] { "Slide", "SlideTap", "SlideRelay", "SlideEnd" }.Contains(note.NoteStyle))
                {
                    foreach (ShortNote _note in notes)
                    {
                        if (note.LongNoteNumber != _note.LongNoteNumber) continue;
                        if (_note == note) continue;
                        if (_note.NoteStyle != "SlideLine") continue;
                        if (note.NotePosition == _note.StartPosition) next = _note;
                        if (note.NotePosition == _note.EndPosition) prev = _note;
                    }
                }
                else if (new string[] { "Hold", "HoldEnd" }.Contains(note.NoteStyle))
                {
                    foreach (ShortNote _note in notes)
                    {
                        if (note.LongNoteNumber != _note.LongNoteNumber) continue;
                        if (_note == note) continue;
                        if (_note.NoteStyle != "HoldLine") continue;
                        if (note.NotePosition == _note.StartPosition) next = _note;
                        if (note.NotePosition == _note.EndPosition) prev = _note;
                    }
                }
                else if (new string[] { "AirBegin", "AirAction", "AirEnd" }.Contains(note.NoteStyle))
                {
                    foreach (ShortNote _note in notes)
                    {
                        if (note.LongNoteNumber != _note.LongNoteNumber) continue;
                        if (_note == note) continue;
                        if (_note.NoteStyle != "AirLine") continue;
                        if (note.NotePosition == _note.StartPosition) next = _note;
                        if (note.NotePosition == _note.EndPosition) prev = _note;
                    }
                }
                note.NotePosition = new Point(rectUL.X + rect.Size.Width - (note.NotePosition.X - rectUL.X) - note.NoteSize * 10, note.NotePosition.Y);
                if (prev != null) { prev.EndPosition = note.NotePosition; prev.update(); }
                if (next != null) { next.StartPosition = note.NotePosition; next.update(); }
                if (note.AirDirection == "Left") note.AirDirection = "Right";
                else if (note.AirDirection == "Right") note.AirDirection = "Left";
                note.update();
            }
        }

        /*
        /// <summary>
        /// 選択範囲内のノーツを全部消します
        /// </summary>
        public void removeSelectedNotes()//使わなさそう
        {
            notes.RemoveAll(x => rect.Contains(new Rectangle(x.DestPoints[0], new Size(x.NoteSize * 10, 5))) &&
            x.NoteStyle != "HoldLine" && x.NoteStyle != "SlideLine" && x.NoteStyle != "AirLine");
        }
        //*/
    }
}