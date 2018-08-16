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
        private int longNoteNumber, noteSize, startSize, endSize;
        private Point[] destPoints = new Point[3];//{ul, ur, ll}
        //private Point[] destPointsSq = new Point[4];//{ul, ur, ll, lr}
        private decimal specialValue;
        //*
        private Bitmap noteImage;
        //*/
        private bool visible = true;//, collision = true;//for SlideLine

        /// <summary>
        /// 汎用コンストラクタです
        /// </summary>
        /// <param name="musicscore"></param>
        /// <param name="position"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <param name="noteSize"></param>
        /// <param name="noteStyle"></param>
        /// <param name="airDirection"></param>
        /// <param name="longNoteNumber"></param>
        public ShortNote(MusicScore musicscore, Point position, Point startPosition, Point endPosition, int noteSize, string noteStyle, string airDirection, int longNoteNumber)
        {
            this.musicscore = musicscore;
            this.position = position;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.noteSize = noteSize;//1-16
            //startSize = endSize = noteSize;
            this.noteStyle = noteStyle;
            this.airDirection = airDirection;
            this.longNoteNumber = longNoteNumber;
            //
            prevNote = null;
            nextNote = null;
            //destPoints = new Point[3];

            this.noteImage = setNoteImage();
            pos.Beat = MusicScore.SelectedBeat;
            setRelativePosition();
        }

        /// <summary>
        /// SlideLine専用コンストラクタです
        /// </summary>
        /// <param name="_musicscore"></param>
        /// <param name="_position"></param>
        /// <param name="_startPosition"></param>
        /// <param name="_endPosition"></param>
        /// <param name="_startSize"></param>
        /// <param name="_endSize"></param>
        /// <param name="_longNoteNumber"></param>
        public ShortNote(MusicScore musicscore, Point position, Point startPosition, Point endPosition, int startSize, int endSize, int longNoteNumber)
        {
            this.musicscore =musicscore;
            this.position = position;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.startSize = startSize;//1-16
            this.endSize = endSize;//1-16
            this.noteStyle = "SlideLine";
            this.airDirection = "Center";
            this.longNoteNumber = longNoteNumber;
            //this.visible = visible == 0 ? false : true;
            //
            this.prevNote = null;
            this.nextNote = null;
            //destPoints = new Point[3];

            this.noteImage = setNoteImage();
            this.pos.Beat = MusicScore.SelectedBeat;
            this.setRelativePosition();
        }


        /// <summary>
        /// 分数を指定できるショートノーツ用コンストラクタです
        /// </summary>
        /// <param name="_musicscore"></param>
        /// <param name="_position"></param>
        /// <param name="_noteSize"></param>
        /// <param name="_noteStyle"></param>
        /// <param name="_airDirection"></param>
        /// <param name="_longNoteNumber"></param>
        /// <param name="_beat"></param>
        public ShortNote(MusicScore musicscore, Point position, int noteSize, string noteStyle, string airDirection, int longNoteNumber, int beat)
        {
            this.musicscore = musicscore;
            this.position = position;
            this.startPosition = position;
            this.endPosition = position;
            this.noteSize = noteSize;//1-16
            this.noteStyle = noteStyle;
            this.airDirection = airDirection;
            this.longNoteNumber = longNoteNumber;
            //
            this.prevNote = null;
            this.nextNote = null;
            //destPoints = new Point[3];

            this.noteImage = setNoteImage();
            this.pos.Beat = beat;
            this.setRelativePosition();
        }

        /// <summary>
        /// SpecialNotes専用コンストラクタです
        /// </summary>
        /// <param name="_musicscore"></param>
        /// <param name="_position"></param>
        /// <param name="_noteStyle"></param>
        /// <param name="_value"></param>
        public ShortNote(MusicScore musicscore, Point position, string noteStyle, decimal value)
        {
            this.musicscore = musicscore;
            this.position = position;
            this.noteStyle = noteStyle;
            this.specialValue = value;
            this.noteSize = 16;
            //destPoints = new Point[3];

            this.noteImage = setNoteImage();
            this.pos.Beat = MusicScore.SelectedBeat;
            this.setRelativePosition();
        }
        //

        private void setRelativePosition()
        {
            pos.X = (position.X - MusicScore.LeftMargin) / 10;
            //pos.Beat = MusicScore.SelectedBeat;//20160;
            //int localY = 778 - position.Y - MusicScore.BottomMargin;
            int localY = 768 - (position.Y - MusicScore.BottomMargin + 3);
            if (localY < 384)//2*n+1小節
            {
                pos.Measure = 2 * musicscore.Index + 1;
                pos.BeatNumber = (int)Math.Round(localY * pos.Beat / 384m);
            }
            else//2*(n+1)小節
            {
                localY -= 384;
                pos.Measure = 2 * musicscore.Index + 2;
                pos.BeatNumber = (int)Math.Round(localY * pos.Beat / 384m);//
            }
            int beatGCD = GCD(pos.Beat, pos.BeatNumber);
            //pos.Beat /= beatGCD; pos.BeatNumber /= beatGCD;
        }

        //*
        public void ShowRerativePosition()
        {
            MessageBox.Show(pos.X + "\n" + pos.Measure + "(" + pos.BeatNumber + "/" + pos.Beat + ")");
        }
        //*/

        private static int GCD(int a, int b)
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

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /*
        public bool Collision
        {
            get { return collision; }
            set { collision = value; }
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

        public int StartSize
        {
            get { return startSize; }
            set { startSize = value; }
        }

        public int EndSize
        {
            get { return endSize; }
            set { endSize = value; }
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
        
        

        public void Update()
        {
            this.noteImage = setNoteImage();
            pos.Beat = (MusicScore.SelectedBeat * pos.Beat) / GCD(MusicScore.SelectedBeat, pos.Beat);
            setRelativePosition();
        }

        /// <summary>
        /// ノーツ画像を作成します
        /// </summary>
        /// <returns></returns>
        private Bitmap setNoteImage()
        {
            //System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();
            //cm.Matrix00 = 1; cm.Matrix11 = 1; cm.Matrix22 = 1; cm.Matrix33 = 1; cm.Matrix44 = 1;
            Bitmap canvas = new Bitmap(10, 10);
            try
            {
                switch (noteStyle)
                {
                    case "HoldLine":
                        canvas = new Bitmap(noteSize * 10, startPosition.Y - endPosition.Y <= 0 ? 1 : startPosition.Y - endPosition.Y);
                        //cm.Matrix33 = 0.9f;
                        break;
                    case "SlideLine":
                        canvas = new Bitmap(Math.Max(endPosition.X + endSize * 10, startPosition.X + startSize * 10) - Math.Min(endPosition.X, startPosition.X),
                            startPosition.Y - endPosition.Y <= 0 ? 1 : startPosition.Y - endPosition.Y);
                        //cm.Matrix33 = 0.9f;
                        break;
                    case "AirLine":
                        canvas = new Bitmap(noteSize * 10, startPosition.Y - endPosition.Y <= 0 ? 1 : startPosition.Y - endPosition.Y);
                        //cm.Matrix33 = 0.9f;
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
            }
            finally
            {
                //canvas.Dispose();
            }
            //System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
            //ia.SetColorMatrix(cm);
            Graphics g = Graphics.FromImage(canvas);//設定した大きさで背景を作成
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;//ノーツ画像を拡大縮小するときの方式を指定
            Bitmap noteImage = setNoteImage(noteStyle);//生のノーツ画像を持ってくる
            noteImage.MakeTransparent(Color.Black);//黒い部分は透明とみなす
            destPoints = setDestPoints("ShortNote");//ノーツの相対位置を設定
            //g.DrawImage(_noteImage ,destPoints, new Rectangle(new Point(0, 0), canvas.Size), GraphicsUnit.Pixel, ia);
            if (noteStyle == "SlideLine") g.DrawImage(noteImage, new Point());
            else g.DrawImage(noteImage, destPoints);
            g.Dispose();
            noteImage.Dispose();
            //this.noteImage = canvas;
            //canvas.Dispose();
            return canvas;
        }
        //*
        /// <summary>
        /// ノーツ画像を指定します
        /// </summary>
        /// <param name="_noteStyle"></param>
        /// <returns></returns>
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
                    //ここを各自で実装する
                    Size origin = new Size(Math.Min(startPosition.X, endPosition.X), endPosition.Y);
                    Bitmap canvas = new Bitmap(Properties.Resources.Black, Math.Max(endPosition.X + endSize * 10, startPosition.X + startSize * 10) - Math.Min(endPosition.X, startPosition.X),
                        startPosition.Y - endPosition.Y <= 0 ? 1 : startPosition.Y - endPosition.Y);//黒の背景を作成
                    Graphics g = Graphics.FromImage(canvas);
                    Point start = Point.Subtract(startPosition, origin);
                    Point end = Point.Subtract(EndPosition, origin);
                    Point[] ps = { new Point(end.X + 2, end.Y), new Point(start.X + 2, start.Y), new Point(start.X + startSize * 10 - 2, start.Y), new Point(end.X + endSize * 10 - 2, end.Y)};
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillPolygon(new SolidBrush(Color.FromArgb(0, 224, 224)), ps);
                    ps = new Point[] { new Point(end.X + endSize * 5 - 2, end.Y), new Point(start.X + startSize * 5 - 2, start.Y), new Point(start.X + startSize * 5 + 2, start.Y), new Point(end.X + endSize * 5 + 2, end.Y) };
                    g.FillPolygon(new SolidBrush(Color.FromArgb(28, 255, 255)), ps);
                    g.Dispose();
                    noteImage = canvas;
                    //canvas.Dispose();
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
            Point[] destPoints;// = new Point[3];
            switch (noteStyle)
            {
                case "HoldEnd":
                    if(state == "ShortNote") destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 5) };
                    else destPoints = new Point[3] { new Point(startPosition.X, position.Y), new Point(startPosition.X + 10 * noteSize, position.Y), new Point(startPosition.X, position.Y + 5) };
                    break;
                case "HoldLine":
                    if (state == "ShortNote") destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(2, startPosition.Y - endPosition.Y) };
                    else destPoints = new Point[3] { new Point(startPosition.X + 2, endPosition.Y), new Point(startPosition.X + 10 * noteSize - 2, endPosition.Y), new Point(startPosition.X + 2, startPosition.Y) };
                    break;
                case "SlideLine":
                    if (state == "ShortNote")
                    {
                        if(startPosition.X > endPosition.X) destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(startPosition.X - endPosition.X + 2, startPosition.Y - endPosition.Y) };
                        else destPoints = new Point[3] { new Point(endPosition.X - startPosition.X + 2, 0), new Point(endPosition.X - startPosition.X + 10 * noteSize - 2, 0), new Point(2, startPosition.Y - endPosition.Y) };
                    }
                    else destPoints = new Point[4] { new Point(endPosition.X + 2, endPosition.Y), new Point(endPosition.X + 10 * endSize - 2, endPosition.Y), new Point(startPosition.X + 2, startPosition.Y), new Point(startPosition.X + startSize * 10 - 2, startPosition.Y)};
                    break;
                case "AirLine":
                    if (state == "ShortNote") destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(2, startPosition.Y - endPosition.Y) };
                    else destPoints = new Point[3] { new Point(startPosition.X + 5 * noteSize - 3, endPosition.Y), new Point(startPosition.X + 5 * noteSize + 3, endPosition.Y), new Point(startPosition.X + 5 * noteSize - 3, startPosition.Y) };
                    break;
                case "AirBegin":
                case "AirEnd":
                case "AirAction":
                    if (state == "ShortNote") destPoints = new Point[3] { new Point(2, 0), new Point(10 * noteSize - 2, 0), new Point(2, 3) };
                    else destPoints = new Point[3] { new Point(startPosition.X + 2, position.Y), new Point(startPosition.X + 10 * noteSize - 2, position.Y), new Point(startPosition.X + 2, position.Y + 3) };
                    break;
                case "AirUp":
                    if (state == "ShortNote") destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 30) };
                    else destPoints = new Point[3] { new Point(position.X, position.Y - 32), new Point(position.X + 10 * noteSize, position.Y - 32), new Point(position.X, position.Y + 30 - 32) };
                    break;
                case "AirDown":
                    if (state == "ShortNote") destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 30) };
                    else destPoints = new Point[3] { new Point(position.X, position.Y - 32), new Point(position.X + 10 * noteSize, position.Y - 32), new Point(position.X, position.Y + 30 - 32) };
                    break;
                default:
                    if (state == "ShortNote") destPoints = new Point[3] { new Point(0, 0), new Point(10 * noteSize, 0), new Point(0, 5) };
                    else destPoints = new Point[3] { position, new Point(position.X + 10 * noteSize, position.Y), new Point(position.X, position.Y + 5) };
                    break;
            }
            return destPoints;
        }
    }
}