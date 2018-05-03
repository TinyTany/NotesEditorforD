using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NotesEditerforD
{
    public partial class Form1 : Form
    {
        private ScoreRoot sRoot;
        private static NotesButton prevNotesButton;
        private int maxScore = 125;
        //List<MusicScore> scores2 = new List<MusicScore>();
        private string songID, title, artist, designer, wave, jacket, exDir, appName, pathName, dymsDataVersion, weStr;
        private int difficulty, longNoteNumber;
        private decimal BPM = 120.0m, playLevel, offset;
        private string fileName;
        private bool isEdited, isNew = true, isWhile = true;
        private const string dymsVersion = "0.7", Version = "0.7";
        //public bool slideRelay = false;
        public Form1()
        {
            InitializeComponent();
            sRoot = new ScoreRoot(this, maxScore, 0, false);
            ScoreRoot.StartBPM = BPM;
            BPMupdown.Maximum = 99999;
            BPMupdown.Minimum = 1;
            BPMButton.spVal_MAX = BPMupdown.Maximum;
            BPMButton.spVal_MIN = BPMupdown.Minimum;
            Speed.spVal_MAX = 10000;
            Speed.spVal_MIN = -10000;
            this.Controls.Add(sRoot);
            sRoot.update();
            checkSlideRelay.Checked = true;
            comboBoxBeat.SelectedIndex = 1;
            MusicScore.SelectedBeat = int.Parse(this.comboBoxBeat.Text);
            appName = " - NotesEditorforD " + Version;
            Text = "NewMusicScore" + appName;
            prevNotesButton = Tap;
            Tap.notesButtonActive();
            //prevSpecialButton = BPMButton;
            openMenuItem.Click += openMenuItem_Click;
            saveAsMenuItem.Click += saveAsMenuItem_Click;
            saveMenuItem.Click += saveMenuItem_Click;
            newMenuItem.Click += newMenuItem_Click;
            exportMenuItem.Click += exportMenuItem_Click;
            quitMenuItem.Click += quitMenuItem_Click;

            songID = "defaultID";
            title = "defaultTitle";
            artist = "defaultArtist";
            designer = "defaultDesigner";
            wave = "";
            jacket = "";
            weStr = "";
            exDir = Environment.CurrentDirectory;
            difficulty = 0;
            playLevel = 1.0m;
            offset = 0.0m;
            BPM = 120;

            fileName = "NewMusicScore.dyms";
            pathName = null;
            setEdited(false);

            longNoteNumber = 0;

            //KeyDownEventはすべてにおいて発火する
            foreach (Control c in Controls)
            {
                c.KeyDown += Form1_KeyDown;
            }

            Tap._Form1 = this;
            ExTap._Form1 = this;
            Flick._Form1 = this;
            HellTap._Form1 = this;
            Hold._Form1 = this;
            Slide._Form1 = this;
            SlideCurve._Form1 = this;
            AirUp._Form1 = this;
            AirDown._Form1 = this;
            AirLine._Form1 = this;
            Speed._Form1 = this;
            BPMButton._Form1 = this;
        }

        public decimal BPM_MAX
        {
            get { return BPMupdown.Maximum; }
        }

        public decimal BPM_MIN
        {
            get { return BPMupdown.Minimum; }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            if (!isEdited) return;
            if (isNew) { object _sender = new object(); EventArgs _e = new EventArgs(); saveAsMenuItem_Click(_sender, _e); }
            else
            {
                saveScores(fileName);
                setEdited(false);
            }
        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = fileName;
            sfd.DefaultExt = ".dyz";
            sfd.Filter = "dyzファイル(NotesEditorforD) (.dyz)|*.dyz";
            //*
            sfd.DefaultExt = ".dyms";
            sfd.Filter = "dymsファイル(NotesEditorforD) (.dyms)|*.dyms";
            //*/
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)//OKボタンがクリックされた時
            {
                saveScores(sfd.FileName);
                setEdited(false);
                Text = sfd.FileName + appName;
                fileName = sfd.FileName;
                pathName = sfd.FileName;
                isNew = false;
            }
        }

        private void serialize(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, sRoot);
            fs.Close();
        }

        private void saveScores(string path)
        {
            //serialize(path); return;
            int indx = 0;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
            sw.WriteLine("dymsVersion:" + dymsVersion);
            sw.WriteLine("SONGID:" + songID);
            sw.WriteLine("TITLE:" + title);
            sw.WriteLine("ARTIST:" + artist);
            sw.WriteLine("DESIGNER:" + designer);
            sw.WriteLine("DIFFICULTY:" + difficulty);
            sw.WriteLine("PLAYLEVEL:" + playLevel + ":" + weStr);
            sw.WriteLine("WAVE=" + wave);
            sw.WriteLine("WAVEOFFSET:" + offset);
            sw.WriteLine("JACKET=" + jacket);
            sw.WriteLine("BASEBPM:" + BPM);
            sw.WriteLine("LongNoteNumber:" + sRoot.LongNoteNumber);
            sw.WriteLine("ExportDir=" + exDir);
            sw.WriteLine("isWhile:" + isWhile);
            foreach (MusicScore mscore in sRoot.Scores)
            {
                foreach (ShortNote note in mscore.shortNotes)
                {
                    sw.WriteLine(
                        note.NoteStyle + "," +       //0
                        note.NoteSize + "," +        //1
                        note.StartSize + "," +       //2
                        note.EndSize + "," +         //3
                        note.NotePosition.X + "," +  //4
                        note.NotePosition.Y + "," +  //5
                        note.StartPosition.X + "," + //6
                        note.StartPosition.Y + "," + //7
                        note.EndPosition.X + "," +   //8
                        note.EndPosition.Y + "," +   //9
                        note.AirDirection + "," +    //10
                        note.LongNoteNumber + "," +  //11
                        indx + "," +                 //12
                        note.LocalPosition.Beat);    //13
                }
                indx++;
            }
            sw.WriteLine("#SpecialNotes"); indx = 0;
            foreach (MusicScore mscore in sRoot.Scores)
            {
                foreach (ShortNote note in mscore.specialNotes)
                {
                    sw.WriteLine(
                        note.NoteStyle + "," +       //0
                        note.NotePosition.X + "," +  //1
                        note.NotePosition.Y + "," +  //2
                        note.SpecialValue + "," +    //3
                        indx);                       //4
                }
                indx++;
            }
            sw.Close();
        }

        public int LongNoteNumber
        {
            get { return this.longNoteNumber; }
            set { this.longNoteNumber = value; }
        }

        /*
        public List<MusicScore> Scores2
        {
            get { return this.scores2; }
        }
        //*/

        public int MaxScore
        {
            get { return this.maxScore; }
        }

        private void quitMenuItem_Click(object sender, EventArgs e)//edt//
        {
            if (isEdited)
            {
                DialogResult result = MessageBox.Show("ファイルは変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    object _sender = new object(); EventArgs _e = new EventArgs(); saveAsMenuItem_Click(_sender, _e);
                    return;
                }
                else if (result == DialogResult.Cancel) return;
                else Application.Exit();
            }
            Application.Exit();
        }

        private void exportMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(this, sRoot);
            f.loadExportData(songID, title, artist, designer, wave, jacket, difficulty, playLevel, BPM, exDir, offset, isWhile, weStr);
            f.ShowDialog(this);
            setEdited(true);
        }

        private void BPMupdown_ValueChanged(object sender, EventArgs e)
        {
            BPM = BPMupdown.Value;
            ScoreRoot.StartBPM = BPM;
            sRoot.Scores[0].update();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Shift) { /*ActiveControl = null;*/ radioAdd.Checked = true; }
            else if (e.KeyCode == Keys.E && e.Modifiers == Keys.Shift) { /*ActiveControl = null;*/ radioEdit.Checked = true; }
            else if (e.KeyCode == Keys.D && e.Modifiers == Keys.Shift) { /*ActiveControl = null;*/ radioDelete.Checked = true; }
            else if (e.KeyCode == Keys.B && e.Modifiers == Keys.None)
            {
                //ActiveControl = null;
                if (comboBoxBeat.SelectedIndex == comboBoxBeat.Items.Count - 1) comboBoxBeat.SelectedIndex = 0;
                else comboBoxBeat.SelectedIndex++;
            }
            else if (e.KeyCode == Keys.B && e.Modifiers == Keys.Shift)
            {
                if (comboBoxBeat.SelectedIndex == 0) comboBoxBeat.SelectedIndex = comboBoxBeat.Items.Count - 1;
                else comboBoxBeat.SelectedIndex--;
            }
            else if (e.KeyCode == Keys.G && e.Modifiers == Keys.None)
            {
                if (comboBoxGrid.SelectedIndex == comboBoxGrid.Items.Count - 1) comboBoxGrid.SelectedIndex = 0;
                else comboBoxGrid.SelectedIndex++;
            }
            else if (e.KeyCode == Keys.G && e.Modifiers == Keys.Shift)
            {
                if (comboBoxGrid.SelectedIndex == 0) comboBoxGrid.SelectedIndex = comboBoxGrid.Items.Count - 1;
                else comboBoxGrid.SelectedIndex--;
            }
            else if (e.KeyCode == Keys.F1) { MusicScore.SelectedNoteStyle = "Tap"; activeNotesButton(Tap); }
            else if (e.KeyCode == Keys.F2) { MusicScore.SelectedNoteStyle = "ExTap"; activeNotesButton(ExTap); }
            else if (e.KeyCode == Keys.F3) { MusicScore.SelectedNoteStyle = "Flick"; activeNotesButton(Flick); }
            else if (e.KeyCode == Keys.F4) { MusicScore.SelectedNoteStyle = "HellTap"; activeNotesButton(HellTap); }
            else if (e.KeyCode == Keys.F5) { MusicScore.SelectedNoteStyle = "Hold"; activeNotesButton(Hold); }
            else if (e.KeyCode == Keys.F6) { MusicScore.SelectedNoteStyle = "Slide"; activeNotesButton(Slide); }
            else if (e.KeyCode == Keys.F7) { MusicScore.SelectedNoteStyle = "SlideCurve"; activeNotesButton(SlideCurve); }
            else if (e.KeyCode == Keys.F8) { MusicScore.SelectedNoteStyle = "AirUp"; activeNotesButton(AirUp); }
            else if (e.KeyCode == Keys.F9) { MusicScore.SelectedNoteStyle = "AirDown"; activeNotesButton(AirDown); }
            else if (e.KeyCode == Keys.F10) { MusicScore.SelectedNoteStyle = "AirLine"; activeNotesButton(AirLine); }
            else if (e.KeyCode == Keys.F11) { MusicScore.SelectedNoteStyle = "BPM"; activeNotesButton(BPMButton); }
            else if (e.KeyCode == Keys.F12) { MusicScore.SelectedNoteStyle = "Speed"; activeNotesButton(Speed); }
            else if (e.KeyCode == Keys.S)
            {
                sRoot.slideRelayInv();
                checkSlideRelay.Checked = !checkSlideRelay.Checked;
            }
            if (e.KeyCode == Keys.Delete) { }
            if (AirUp.IsActive)
            {
                if (e.KeyCode == Keys.L) AirUp.setDirection("Left");
                else if (e.KeyCode == Keys.C) AirUp.setDirection("Center");
                else if (e.KeyCode == Keys.R) AirUp.setDirection("Right");
                AirUp.setAirDirection();
            }
            if (AirDown.IsActive)
            {
                if (e.KeyCode == Keys.L) AirDown.setDirection("Left");
                else if (e.KeyCode == Keys.C) AirDown.setDirection("Center");
                else if (e.KeyCode == Keys.R) AirDown.setDirection("Right");
                AirDown.setAirDirection();
            }
            if (!(activeNotesButton().NotesName == "BPM" || activeNotesButton().NotesName == "Speed"))
            {
                if (e.KeyCode == Keys.OemPeriod && activeNotesButton().TrackBar_Size != 16)
                {
                    activeNotesButton().TrackBar_Size++;
                    activeNotesButton().trackBar1_Scroll(sender, e);
                }
                if (e.KeyCode == Keys.Oemcomma && activeNotesButton().TrackBar_Size != 1)
                {
                    activeNotesButton().TrackBar_Size--;
                    activeNotesButton().trackBar1_Scroll(sender, e);
                }
            }
            else
            {
                if (e.KeyCode == Keys.OemPeriod && activeNotesButton().NumUDValue != activeNotesButton().NumUDMax)
                {
                    activeNotesButton().NumUDValue += 0.1m;
                    activeNotesButton().numericUpDown1_ValueChanged(sender, e);
                }
                else if (e.KeyCode == Keys.Oemcomma && activeNotesButton().NumUDValue != activeNotesButton().NumUDMin)
                {
                    activeNotesButton().NumUDValue -= 0.1m;
                    activeNotesButton().numericUpDown1_ValueChanged(sender, e);
                }
            }
            //MessageBox.Show(activeNotesButton().NotesName);
            setEditStatus(sender, e);
            //MessageBox.Show("KeyDown");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            /*
            flowLayoutPanelMusicScore.Width = this.Width - 297;
            flowLayoutPanelMusicScore.Height = this.Height - 97;
            //*/
            flowLayoutPanelNotesButton.Height = this.Height - 72;
            sRoot.panelResize(Width, Height);
        }

        private NotesButton activeNotesButton()
        {
            if (Tap.IsActive) return Tap;
            else if (ExTap.IsActive) return ExTap;
            else if (Flick.IsActive) return Flick;
            else if (HellTap.IsActive) return HellTap;
            else if (Hold.IsActive) return Hold;
            else if (Slide.IsActive) return Slide;
            else if (SlideCurve.IsActive) return SlideCurve;
            else if (AirUp.IsActive) return AirUp;
            else if (AirDown.IsActive) return AirDown;
            else if (AirLine.IsActive) return AirLine;
            else if (BPMButton.IsActive) return BPMButton;
            else if (Speed.IsActive) return Speed;
            else return null;
        }

        private void checkSlideRelay_Click(object sender, EventArgs e)
        {
            sRoot.slideRelayInv();
            this.ActiveControl = null;
        }

        private void fileMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void saveExportData(string _songID, string _title, string _artist, string _designer, string _wave, string _jacket, int _difficulty, decimal _playLevel, decimal _BPM, string _exDir, decimal _offset, bool _isWhile, string _weStr)
        {
            songID = _songID;
            title = _title;
            artist = _artist;
            designer = _designer;
            wave = _wave;
            jacket = _jacket;
            difficulty = _difficulty;
            playLevel = _playLevel;
            BPM = _BPM;
            exDir = _exDir;
            offset = _offset;
            isWhile = _isWhile;
            weStr = _weStr;
        }

        private void newMenuItem_Click(object sender, EventArgs e)//edt
        {
            if (isEdited)
            {
                DialogResult result = MessageBox.Show("ファイルは変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    object _sender = new object(); EventArgs _e = new EventArgs(); saveAsMenuItem_Click(_sender, _e);
                    return;
                }
                else if (result == DialogResult.Cancel) return;
            }
            sRoot.newScore();
            setTotalNotes();
            songID = "defaultID";
            title = "defaultTitle";
            artist = "defaultArtist";
            designer = "defaultDesigner";
            wave = "";
            jacket = "";
            weStr = "";
            exDir = Environment.CurrentDirectory;
            difficulty = 0;
            playLevel = 1.0m;
            offset = 0.0m;
            BPM = 120;
            fileName = "NewMusicScore.dyms";
            longNoteNumber = 0;
            setEdited(false);
            pathName = null;
            isNew = true;
        }

        private void openMenuItem_Click(object sender, EventArgs e)//edt
        {
            if (isEdited)
            {
                DialogResult result = MessageBox.Show("ファイルは変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    object _sender = new object(); EventArgs _e = new EventArgs(); saveAsMenuItem_Click(_sender, _e);
                    return;
                }
                else if (result == DialogResult.Cancel) return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "default.dyms";
            ofd.Filter = "dymsファイル(NotesEditorforD) (.dyms)|*.dyms";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)//OKボタンがクリックされた時
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                fileName = ofd.SafeFileName;//MessageBox.Show(fileName);
                string dataLine;
                string[] noteData;
                int indx, msIndex;
                dataLine = sr.ReadLine();
                if (dataLine == "dymsVersion:0.5" || dataLine == "dymsVersion:0.6"　|| dataLine == "dymsVersion:0.7")//バージョン変更時に必ず変更
                {
                    noteData = dataLine.Split(':');
                    dymsDataVersion = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    songID = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    title = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    artist = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    designer = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    difficulty = int.Parse(noteData[1]);
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    playLevel = decimal.Parse(noteData[1]);
                    if(noteData.Length == 3) weStr = noteData[2];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split('=');
                    wave = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    offset = decimal.Parse(noteData[1]);
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split('=');
                    jacket = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    BPM = decimal.Parse(noteData[1]);
                    BPMupdown.Value = BPM;
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    longNoteNumber = int.Parse(noteData[1]);
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split('=');
                    exDir = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    isWhile = bool.Parse(noteData[1]);
                }
                else
                {
                    MessageBox.Show("対応していないバージョンのファイルです", "読み込みエラー");
                    return;
                }
                //initialize musicscore
                sRoot.newScore();
                setTotalNotes();
                //
                if (dymsDataVersion == "0.7") msIndex = 12;
                else msIndex = 10;//index
                bool flg = false;
                while (sr.Peek() > -1)
                {
                    dataLine = sr.ReadLine();
                    if (dataLine == "#SpecialNotes") { flg = true; break; }
                    noteData = dataLine.Split(',');
                    indx = int.Parse(noteData[msIndex]);
                    sRoot.Scores[indx].setNote(noteData, dymsDataVersion);
                }
                if (flg)
                {
                    while (sr.Peek() > -1)
                    {
                        dataLine = sr.ReadLine();
                        noteData = dataLine.Split(',');
                        indx = int.Parse(noteData[4]);
                        sRoot.Scores[indx].setSpecialNote(noteData);
                    }
                }
                for (int i = 0; i < maxScore; i++) sRoot.Scores[i].update();
                setEdited(false);
                isNew = false;
                sr.Close();
            }
            Text = ofd.SafeFileName + appName;
            fileName = ofd.FileName;
            pathName = ofd.FileName;
        }

        public void activeNotesButton(NotesButton notesButton)
        {
            if (prevNotesButton == null)
            {
                prevNotesButton = notesButton;
            }
            if (notesButton != prevNotesButton)
            {
                prevNotesButton.notesButtonInactive();
                prevNotesButton = notesButton;
            }
            notesButton.notesButtonActive();
            /*
            if(notesButton == BPMButton)
            {
                MusicScore2.SelectedBeat = 8;
                comboBoxBeat.SelectedIndex = 1;
                comboBoxBeat.Enabled = false;
            }
            else
            {
                comboBoxBeat.Enabled = true;
            }//*/
        }

        /*
        public static void activeNotesButton(SpecialButton notesButton)
        {
            if (prevNotesButton != null)
            {
                prevNotesButton.notesButtonInactive();
                prevNotesButton = null;
            }
        }//*/

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//edt//
        {
            if (isEdited)
            {
                DialogResult result = MessageBox.Show("ファイルは変更されています。保存しますか？", "確認", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    object _sender = new object(); EventArgs _e = new EventArgs(); saveAsMenuItem_Click(_sender, _e);
                    e.Cancel = true;
                    return;
                }
                else if (result == DialogResult.No) { setEdited(false); Application.Exit(); }
                else if (result == DialogResult.Cancel) e.Cancel = true;
            }
        }

        /*
        public void setNote(int indx, Note note)
        {
            scores[indx].setNote(note);
        }//*/

        /*
        public void deleteNote(int indx, Note note)
        {
            note.Dispose();
            scores[indx].notes.Remove(note);
        }//*/

        public void setTotalNotes()
        {
            //*
            int totalNotes = 0;
            foreach(MusicScore score in sRoot.Scores)
            {
                totalNotes += score.shortNotes.Count;
            }
            //*/
            labelTotalNotes.Text = "Total notes : " + totalNotes.ToString();
        }

        public void setEdited(bool b)
        {
            isEdited = b;
            //*
            if(isEdited) appName = "* - NotesEditorforD " + Version;
            else appName = " - NotesEditorforD " + Version;
            Text = fileName + appName;
            //*/
        }

        /*
        public int getBPM()
        {
            return (int)this.numericUpDownBPM.Value;
        }
        //*/

        private void setEditStatus(object sender, EventArgs e)
        {
            if (radioAdd.Checked) MusicScore.SelectedEditStatus = "Add";
            else if (radioEdit.Checked) MusicScore.SelectedEditStatus = "Edit";
            else MusicScore.SelectedEditStatus = "Delete";
        }

        private void comboBoxBeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            MusicScore.SelectedBeat = int.Parse(this.comboBoxBeat.Text);
        }

        private void comboBoxGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            MusicScore.SelectedGrid = int.Parse(this.comboBoxGrid.Text);
        }
    }
}