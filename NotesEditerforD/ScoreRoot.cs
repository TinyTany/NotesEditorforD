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
            SuspendLayout();
            for (int i = 0; i < maxScore; i++)
            {
                musicScore = new MusicScore();
                musicScore.sRoot = this;
                musicScore.Index = i;
                this.Controls.Add(musicScore);
                scores.Add(musicScore);
                //musicScore.update();
            }
            ResumeLayout();
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
            //System.Diagnostics.Debug.WriteLine(HorizontalScroll.Maximum);

            MouseWheel += ScoreRoot_MouseWheel;
        }

        private void ScoreRoot_MouseWheel(object sender, MouseEventArgs e)
        {
            if (VScroll && (ModifierKeys & Keys.Shift) == Keys.Shift) ;
            //System.Diagnostics.Debug.WriteLine(HorizontalScroll.Value);
        }

        public void update()
        {
            foreach (MusicScore score in scores)
            {
                score.update();
            }
        }

        /*
        public int setLNN()
        {
            

            return longNotesNumber;
        }
        //*/

        //*//test
        public void setLongNote(ShortNote note, MusicScore score)
        {
            Graphics g = Graphics.FromImage(score.StoreImage);
            //g.DrawImage(Properties.Resources.MusicScore, new Point(0, 0));
            switch (note.NoteStyle)
            {
                case "Hold":

                    break;
                case "Slide":
                case "SlideEnd":
                case "SlideTap":
                case "SlideRelay":
                    ShortNote nextNote = null, prevNote = null;
                    for (MusicScore iScore = score; iScore.NextScore != null && nextNote == null; iScore = iScore.NextScore)//次の中継点ノーツを探してnextNoteに
                    {
                        if(iScore == score)//初回
                        {
                            foreach(ShortNote iNote in score.shortNotes.Where(
                                x => x.NotePosition.Y < note.NotePosition.Y).OrderByDescending(x => x.NotePosition.Y))
                            {
                                if(iNote.LongNoteNumber == note.LongNoteNumber && iNote.NoteStyle != "SlideCurve")
                                {
                                    nextNote = iNote;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (ShortNote iNote in iScore.shortNotes.OrderByDescending(x => x.NotePosition.Y))
                            {
                                if (iNote.LongNoteNumber == note.LongNoteNumber && iNote.NoteStyle != "SlideCurve")
                                {
                                    nextNote = iNote;
                                    break;
                                }
                            }
                        }
                    }
                    for (MusicScore iScore = score; iScore.PrevScore != null && prevNote == null; iScore = iScore.PrevScore)//前の中継点ノーツを探してprevNoteに
                    {
                        if (iScore == score)//初回
                        {
                            foreach (ShortNote iNote in score.shortNotes.Where(
                                x => x.NotePosition.Y > note.NotePosition.Y && x.LongNoteNumber != -1).OrderBy(x => x.NotePosition.Y))
                            {
                                if (iNote.LongNoteNumber == note.LongNoteNumber && iNote.NoteStyle != "SlideCurve")
                                {
                                    prevNote = iNote;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (ShortNote iNote in iScore.shortNotes.OrderBy(x => x.NotePosition.Y))
                            {
                                if (iNote.LongNoteNumber == note.LongNoteNumber && iNote.NoteStyle != "SlideCurve")
                                {
                                    prevNote = iNote;
                                    break;
                                }
                            }
                        }
                    }
                    if (nextNote == null) return;
                    int nMeasure = nextNote.LocalPosition.Measure;
                    int cMeasure = note.LocalPosition.Measure;
                    int measureDiff = (nMeasure % 2 == 0 ? nMeasure - 1 : nMeasure) / 2 - (cMeasure % 2 == 0 ? cMeasure - 1 : cMeasure) / 2;
                    Point nPnt, cPnt; int nSize, cSize;
                    if (measureDiff == 0)//2ノーツは同じ譜面レーン上
                    {
                        //draw
                        nPnt = nextNote.NotePosition; nSize = nextNote.NoteSize;
                        cPnt = note.NotePosition; cSize = note.NoteSize;
                        Point[] ps = {new Point(nPnt.X + 2, nPnt.Y), new Point(cPnt.X + 2, cPnt.Y),
                            new Point(cPnt.X + cSize * 10 - 2, cPnt.Y), new Point(nPnt.X + nSize * 10 - 2, nPnt.Y)};
                        g.FillPolygon(Brushes.Aqua, ps);
                    }
                    else//2ノーツは異なるレーン上（境界を何度かまたぐ）
                    {
                        //draw(begin)
                        for(int i = 0; i < measureDiff - 1; i++)
                        {

                        }
                        //draw(end)
                    }
                    break;
                case "AirBegin":
                case "AirAction":

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
