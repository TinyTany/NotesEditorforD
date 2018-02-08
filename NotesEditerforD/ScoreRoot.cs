using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace NotesEditerforD
{
    [Serializable()]
    public class ScoreRoot : FlowLayoutPanel
    {
        private int longNotesNumber;
        private bool slideRelay;
        private Form1 form1;
        private MusicScore musicScore;
        private List<MusicScore> scores = new List<MusicScore>();
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

        public List<MusicScore> Scores
        {
            get { return scores; }
            set { scores = value; }
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
