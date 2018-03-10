using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace NotesEditerforD
{
    /// <summary>
    /// 譜面をまとめるクラス
    /// </summary>
    [Serializable()]
    public class ScoreRoot : FlowLayoutPanel
    {
        private int longNotesNumber;
        private bool slideRelay;
        private Form1 form1;
        private MusicScore musicScore;
        private List<MusicScore> scores = new List<MusicScore>();
        private List<ShortNote> yankNotes = new List<ShortNote>();
        private RectSelect yankRect;
        private static decimal BPM;
        public ScoreRoot(Form1 _form1, int maxScore, int _longNotesNumber, bool _slideRelay)
        {
            form1 = _form1;
            longNotesNumber = _longNotesNumber;
            slideRelay = _slideRelay;
            for (int i = 0; i < maxScore; i++)
            {
                musicScore = new MusicScore();
                musicScore.sRoot = this;
                musicScore.Index = i;
                this.Controls.Add(musicScore);
                scores.Add(musicScore);
                //musicScore.update();
            }
            scores[0].NextScore = scores[1];
            scores[0].PrevScore = null;
            for (int i = 1; i < maxScore - 1; i++)
            {
                scores[i].NextScore = scores[i + 1];
                scores[i].PrevScore = scores[i - 1];
            }
            scores[maxScore - 1].NextScore = null;
            scores[maxScore - 1].PrevScore = scores[maxScore - 2];
            //
            this.Location = new Point(269, 51);
            this.Size = new Size(1103, 823);
            this.BackColor = Color.AliceBlue;
            this.FlowDirection = FlowDirection.LeftToRight;
            this.WrapContents = false;
            this.AutoScroll = true;
        }

        public void update()
        {
            foreach (MusicScore score in scores)
            {
                score.update();
            }
        }

        //*//test
        public void setLongNote(ShortNote note, MusicScore score)
        {
            Graphics g = Graphics.FromImage(score.StoreImage);
            g.DrawImage(Properties.Resources.MusicScore, new Point(0, 0));
            switch (note.NoteStyle)
            {
                case "Hold":

                    break;
                case "Slide":
                    ShortNote lastNote = null; int lastIndex = -1;
                    for (MusicScore cur = score; cur != null; cur = cur.NextScore)
                    {
                        g = Graphics.FromImage(cur.StoreImage);
                        var notes = cur.shortNotes.Where(x => x.LongNoteNumber == note.LongNoteNumber && x.NoteStyle != "SlideCurve" && x.NoteStyle != "SlideLine").OrderByDescending(x => x.NotePosition.Y).ToList();
                        if (!notes.Any()) continue;
                        if (lastNote != null) ;
                        for (int i = 0; i < notes.Count() - 1; i++)
                        {
                            g.DrawImage(Properties.Resources.HoldLine, new Point[] { notes[i + 1].NotePosition, new Point(notes[i + 1].NotePosition.X + notes[i + 1].NoteSize * 10, notes[i + 1].NotePosition.Y), notes[i].NotePosition });
                        }
                        //cur.storeImage = _img; //cur.update();
                        if (notes.Last().NoteStyle != "SlideEnd") { lastNote = notes.Last(); lastIndex = cur.Index; continue; }
                        break;
                    }
                    break;
                case "AirBegin":

                    break;
                default:
                    break;
            }
            g.Dispose();
        }
        //*/

        public List<MusicScore> Scores
        {
            get { return scores; }
            set { scores = value; }
        }

        public List<ShortNote> YankNotes
        {
            get { return yankNotes; }
            set { yankNotes = value; }
        }

        public RectSelect YankRect
        {
            get { return yankRect; }
            set { yankRect = value; }
        }

        public int LongNoteNumber
        {
            get { return longNotesNumber; }
            set { longNotesNumber = value; }
        }

        public bool SlideRelay
        {
            get { return slideRelay; }
            set { slideRelay = value; }
        }

        public static decimal StartBPM
        {
            get { return BPM; }
            set { BPM = value; }
        }

        public void slideRelayInv()
        {
            slideRelay = !slideRelay;
        }

        //*
        public void setTotalNotes()
        {
            form1.setTotalNotes();
        }
        //*/

        public void newScore()
        {
            foreach(MusicScore score in scores)
            {
                score.deleteAllNotes();
            }
        }

        public void setEdited(bool b)
        {
            form1.setEdited(b);
        }

        public void panelResize(int width, int height)
        {
            this.Width = width - 297;
            this.Height = height - 97;
        }
    }
}
