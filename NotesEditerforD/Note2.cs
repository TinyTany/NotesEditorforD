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
    /// ノーツそのもの
    /// </summary>
    public class ShortNote
    {
        private MusicScore musicscore;
        private Point position, startPosition, endPosition;
        private PosInfo pos = new PosInfo();
        private string noteStyle, airDirection;
        private ShortNote prevNote, nextNote;
        private int longNoteNumber, noteSize;
        private Point[] destPoints = new Point[3];//{ul, ur, ll}
        private decimal specialValue;
        //*
        private Bitmap noteImage;
        //*/

        //コンストラクタ
        public ShortNote(MusicScore _musicscore, Point _position, Point _startPosition, Point _endPosition, int _noteSize, string _noteStyle, string _airDirection, int _longNoteNumber)
        {
            musicscore = _musicscore;
            position = _position;
            startPosition = _startPosition;
            endPosition = _endPosition;
            noteSize = _noteSize;//1-16
            noteStyle = _noteStyle;
            airDirection = _airDirection;
            longNoteNumber = _longNoteNumber;
            //
            prevNote = null;
            nextNote = null;
            //destPoints = new Point[3];

            noteImage = setNoteImage();
            pos.Beat = MusicScore.SelectedBeat;
            setRelativePosition();
        }

        public ShortNote(MusicScore _musicscore, Point _position, int _noteSize, string _noteStyle, string _airDirection, int _longNoteNumber, int _beat)
        {
            musicscore = _musicscore;
            position = _position;
            startPosition = _position;
            endPosition = _position;
            noteSize = _noteSize;//1-16
            noteStyle = _noteStyle;
            airDirection = _airDirection;
            longNoteNumber = _longNoteNumber;
            //
            prevNote = null;
            nextNote = null;
            //destPoints = new Point[3];

            noteImage = setNoteImage();
            pos.Beat = _beat;
            setRelativePosition();
        }

        public ShortNote(MusicScore _musicscore, Point _position, string _noteStyle, decimal _value)
        {
            musicscore = _musicscore;
            position = _position;
            noteStyle = _noteStyle;
            specialValue = _value;
            noteSize = 16;
            //destPoints = new Point[3];

            noteImage = setNoteImage();
            pos.Beat = MusicScore.SelectedBeat;
            setRelativePosition();
        }
        //

        private void setRelativePosition()
        {
            pos.X = (position.X - MusicScore.LeftMargin) / 10;
            //pos.Beat = MusicScore.SelectedBeat;//20160;
            int localY = 778 - position.Y - MusicScore.BottomMargin;
            if (localY < 386)//2*n+1小節
            {
                pos.Measure = 2 * musicscore.Index + 1;
                pos.BeatNumber = (int)Math.Round((localY - 3) * pos.Beat / 384m);
            }
            else//2*(n+1)小節
            {
                localY -= 386;
                pos.Measure = 2 * musicscore.Index + 2;
                pos.BeatNumber = (int)Math.Round((localY - 2) * pos.Beat / 384m);
            }
            int beatGCD = GCD(pos.Beat, pos.BeatNumber);
            pos.Beat /= beatGCD; pos.BeatNumber /= beatGCD;
        }

        //*
        public void showRerativePosition()
        {
            MessageBox.Show(pos.X + "\n" + pos.Measure + "(" + pos.BeatNumber + "/" + pos.Beat + ")");
        }
        //*/

        private int GCD(int a, int b)
        {
            if (a == 0 || b == 0) return 1;
            if (a < b)
            {
                int tmp = a; a = b; b = tmp;
            }

            int r = a % b;
            while (r != 0)
            {
                a = b; b = r; r = a % b;
            }

            return b;
        }

        /////////////////////////////////////////
        ///*
        public Bitmap NoteImage
        {
            get { return this.noteImage; }
            set { this.noteImage = value; }
        }
        //*/

        public ShortNote PrevNote
        {
            get { return this.prevNote; }
            set { this.prevNote = value; }
        }

        public ShortNote NextNote
        {
            get { return this.nextNote; }
            set { this.nextNote = value; }
        }

        public Point[] DestPoints
        {
            get { return this.setDestPoints("MusicScore"); }
        }

        public string NoteStyle
        {
            get { return this.noteStyle; }
            set { this.noteStyle = value; }
        }

        public int NoteSize
        {
            get { return this.noteSize; }
            set { this.noteSize = value; }
        }

        public Point NotePosition
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public PosInfo LocalPosition
        {
            get { return pos; }
            set { pos = value; }
        }

        public Point StartPosition
        {
            get { return this.startPosition; }
            set { this.startPosition = value; }
        }

        public Point EndPosition
        {
            get { return this.endPosition; }
            set { this.endPosition = value; }
        }

        public string AirDirection
        {
            get { return this.airDirection; }
            set { this.airDirection = value; }
        }

        public int LongNoteNumber
        {
            get { return this.longNoteNumber; }
            set { this.longNoteNumber = value; }
        }

        public decimal SpecialValue
        {
            get { return this.specialValue; }
            set { this.specialValue = value; }
        }
        
        

        public void update()
        {
            noteImage = setNoteImage();
            pos.Beat = MusicScore.SelectedBeat;
            setRelativePosition();
        }

        private Bitmap setNoteImage()
        {
            System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();
            cm.Matrix00 = 1; cm.Matrix11 = 1; cm.Matrix22 = 1; cm.Matrix33 = 1; cm.Matrix44 = 1;
            Bitmap canvas;
            switch (noteStyle)
            {
                case "HoldLine":
                    canvas = new Bitmap(noteSize * 10, startPosition.Y - endPosition.Y <= 0 ? 1 : startPosition.Y - endPosition.Y);
                    cm.Matrix33 = 0.9f;
                    break;
                case "SlideLine":
                    canvas = new Bitmap(Math.Abs(startPosition.X - endPosition.X) + noteSize * 10, startPosition.Y - endPosition.Y <= 0 ? 1 : startPosition.Y - endPosition.Y);
                    cm.Matrix33 = 0.9f;
                    break;
                case "AirLine":
                    canvas = new Bitmap(noteSize * 10, startPosition.Y - endPosition.Y <= 0 ? 1 : startPosition.Y - endPosition.Y);
                    cm.Matrix33 = 0.9f;
                    break;
                case "AirUp":
                    canvas = new Bitmap(noteSize * 10, 30);
                    break;
                case "AirDown":
                    canvas = new Bitmap(noteSize * 10, 30);
                    break;
                default:
                    canvas = new Bitmap(noteSize * 10, 5);
                    break;
            }
            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
            ia.SetColorMatrix(cm);
            Graphics g = Graphics.FromImage(canvas);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Bitmap _noteImage = setNoteImage(noteStyle);
            _noteImage.MakeTransparent(Color.Black);
            destPoints = setDestPoints("ShortNote");
            //g.DrawImage(_noteImage ,destPoints, new Rectangle(new Point(0, 0), canvas.Size), GraphicsUnit.Pixel, ia);
            g.DrawImage(_noteImage, destPoints);
            g.Dispose();
            _noteImage.Dispose();
            return canvas;
        }
        //*
        private Bitmap setNoteImage(string _noteStyle)//ノーツ画像を指定
        {
            Bitmap noteImage;
            switch (_noteStyle)
            {
                case "Tap":
                    noteImage = Properties.Resources.Tap;
                    break;
                case "ExTap":
                    noteImage = Properties.Resources.ExTap;
                    break;
                case "Flick":
                    noteImage = Properties.Resources.Flick;
                    break;
                case "HellTap":
                    noteImage = Properties.Resources.HellTap;
                    break;
                case "Hold":
                    noteImage = Properties.Resources.Hold;
                    break;
                case "HoldLine":
                    noteImage = Properties.Resources.HoldLine;
                    break;
                case "HoldEnd":
                    noteImage = Properties.Resources.HoldEnd;
                    break;
                case "Slide":
                    noteImage = Properties.Resources.Slide;
                    break;
                case "SlideLine":
                    noteImage = Properties.Resources.SlideLine;
                    break;
                case "SlideTap":
                    
                case "SlideEnd":
                    noteImage = Properties.Resources.SlideTap;
                    break;
                case "SlideRelay":
                    noteImage = Properties.Resources.SlideRelay;
                    break;
                case "SlideCurve":
                    noteImage = Properties.Resources.SlideCurve;
                    break;
                case "AirBegin":
                    noteImage = Properties.Resources.BPM;
                    break;
                case "AirEnd":
                case "AirAction":
                    noteImage = Properties.Resources.AirAction;
                    break;
                case "AirLine":
                    noteImage = Properties.Resources.AirLine;
                    break;
                case "AirUp":
                    switch (airDirection)
                    {
                        case "Left":
                            noteImage = Properties.Resources.AirUpL;
                            break;
                        case "Center":
                            noteImage = Properties.Resources.AirUpC;
                            break;
                        case "Right":
                            noteImage = Properties.Resources.AirUpR;
                            break;
                        default:
                            noteImage = Properties.Resources.AirUpC;
                            break;
                    }
                    break;
                case "AirDown":
                    switch (airDirection)
                    {
                        case "Left":
                            noteImage = Properties.Resources.AirDownL;
                            break;
                        case "Center":
                            noteImage = Properties.Resources.AirDownC;
                            break;
                        case "Right":
                            noteImage = Properties.Resources.AirDownR;
                            break;
                        default:
                            noteImage = Properties.Resources.AirDownC;
                            break;
                    }
                    break;
                case "BPM":
                    noteImage = Properties.Resources.BPM;
                    break;
                case "Speed":
                    noteImage = Properties.Resources.Speed;
                    break;
                default:
                    noteImage = Properties.Resources.Tap;
                    break;
            }
            return noteImage;
        }
        //*/
        private Point[] setDestPoints(string state)
        {
            Point[] _destPoints = new Point[3];
            switch (noteStyle)
            {
                case "HoldEnd":
                    if(state == "ShortNote") _destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 5) };
                    else _destPoints = new Point[3] { new Point(startPosition.X, position.Y), new Point(startPosition.X + 10 * noteSize, position.Y), new Point(startPosition.X, position.Y + 5) };
                    break;
                case "HoldLine":
                    if (state == "ShortNote") _destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(2, startPosition.Y - endPosition.Y) };
                    else _destPoints = new Point[3] { new Point(startPosition.X + 2, endPosition.Y), new Point(startPosition.X + 10 * noteSize - 2, endPosition.Y), new Point(startPosition.X + 2, startPosition.Y) };
                    break;
                case "SlideLine":
                    if (state == "ShortNote")
                    {
                        if(startPosition.X > endPosition.X) _destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(startPosition.X - endPosition.X + 2, startPosition.Y - endPosition.Y) };
                        else _destPoints = new Point[3] { new Point(endPosition.X - startPosition.X + 2, 0), new Point(endPosition.X - startPosition.X + 10 * noteSize - 2, 0), new Point(2, startPosition.Y - endPosition.Y) };
                    }
                    else _destPoints = new Point[3] { new Point(endPosition.X + 2, endPosition.Y), new Point(endPosition.X + 10 * noteSize - 2, endPosition.Y), new Point(startPosition.X + 2, startPosition.Y) };
                    break;
                case "AirLine":
                    if (state == "ShortNote") _destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(2, startPosition.Y - endPosition.Y) };
                    else _destPoints = new Point[3] { new Point(startPosition.X + 5 * noteSize - 3, endPosition.Y), new Point(startPosition.X + 5 * noteSize + 3, endPosition.Y), new Point(startPosition.X + 5 * noteSize - 3, startPosition.Y) };
                    break;
                case "AirBegin":
                case "AirEnd":
                case "AirAction":
                    if (state == "ShortNote") _destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(2, 3) };
                    else _destPoints = new Point[3] { new Point(startPosition.X + 2, position.Y), new Point(startPosition.X + 10 * noteSize - 2, position.Y), new Point(startPosition.X + 2, position.Y + 3) };
                    break;
                case "AirUp":
                    if (state == "ShortNote") _destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 30) };
                    else _destPoints = new Point[3] { new Point(position.X, position.Y - 32), new Point(position.X + 10 * noteSize, position.Y - 32), new Point(position.X, position.Y + 30 - 32) };
                    break;
                case "AirDown":
                    if (state == "ShortNote") _destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 30) };
                    else _destPoints = new Point[3] { new Point(position.X, position.Y - 32), new Point(position.X + 10 * noteSize, position.Y - 32), new Point(position.X, position.Y + 30 - 32) };
                    break;
                default:
                    if (state == "ShortNote") _destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 5) };
                    else _destPoints = new Point[3] { position, new Point(position.X + 10 * noteSize, position.Y), new Point(position.X, position.Y + 5) };
                    break;
            }
            return _destPoints;
        }
    }
}