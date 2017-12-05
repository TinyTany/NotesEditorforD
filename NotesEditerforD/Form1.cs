using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotesEditerforD
{
    public partial class Form1 : Form
    {
        MusicScore2 musicScore2;
        private static NotesButton prevNotesButton;
        private static SpecialButton prevSpecialButton;
        private int maxScore = 100;
        List<MusicScore> scores = new List<MusicScore>();
        List<MusicScore2> scores2 = new List<MusicScore2>();
        private string songID, title, artist, designer, wave, jacket, exDir, appName, pathName, dymsDataVersion;
        private int difficulty, longNoteNumber;
        private decimal BPM = 120.0m, playLevel, offset;
        private string fileName;
        private bool isEdited, isNew = true, isWhile = true;
        private const string dymsVersion = "0.3", Version = "0.3.1";
        public Form1()
        {
            InitializeComponent();
            comboBoxBeat.SelectedIndex = 1;
            MusicScore2.SelectedBeat = int.Parse(this.comboBoxBeat.Text);
            for (int i = 0; i < maxScore; i++)
            {
                musicScore2 = new MusicScore2();
                musicScore2.form1 = this;
                musicScore2.Index = i;
                flowLayoutPanelMusicScore.Controls.Add(musicScore2);
                scores2.Add(musicScore2);
                musicScore2.update();
            }
            ///*
            scores2[0].NextScore = scores2[1];
            scores2[0].PrevScore = null;
            for(int i = 1; i < maxScore - 1; i++)
            {
                scores2[i].NextScore = scores2[i + 1];
                scores2[i].PrevScore = scores2[i - 1];
            }
            scores2[maxScore - 1].NextScore = null;
            scores2[maxScore - 1].PrevScore = scores2[maxScore - 2];
            //*/
            appName = " - NotesEditorforD " + Version;
            Text = "NewMusicScore" + appName;
            prevNotesButton = Tap;
            Tap.notesButtonActive();
            prevSpecialButton = BPMButton;
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
            exDir = System.Environment.CurrentDirectory;
            difficulty = 0;
            playLevel = 1.0m;
            offset = 0.0m;
            BPM = 120;

            fileName = "NewMusicScore.dyms";
            pathName = null;
            setEdited(false);

            longNoteNumber = 0;
        }

        public decimal StartBPM
        {
            get { return BPM; }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            if (!isEdited) return;
            if(isNew) { object _sender = new object(); EventArgs _e = new EventArgs(); saveAsMenuItem_Click(_sender, _e);  }
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
            sfd.DefaultExt = ".dyms";
            sfd.Filter = "dymsファイル(NotesEditorforD) (.dyms)|*.dyms";
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

        private void saveScores(string path)
        {
            int indx = 0;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
            sw.WriteLine("dymsVersion:" + dymsVersion);
            sw.WriteLine("SONGID:" + songID);
            sw.WriteLine("TITLE:" + title);
            sw.WriteLine("ARTIST:" + artist);
            sw.WriteLine("DESIGNER:" + designer);
            sw.WriteLine("DIFFICULTY:" + difficulty);
            sw.WriteLine("PLAYLEVEL:" + playLevel);
            sw.WriteLine("WAVE=" + wave);
            sw.WriteLine("WAVEOFFSET:" + offset);
            sw.WriteLine("JACKET=" + jacket);
            sw.WriteLine("BASEBPM:" + BPM);
            sw.WriteLine("LongNoteNumber:" + longNoteNumber);
            sw.WriteLine("ExportDir=" + exDir);
            sw.WriteLine("isWhile:" + isWhile);
            foreach (MusicScore2 mscore in scores2)
            {
                foreach (ShortNote note in mscore.shortNotes)
                {
                    sw.WriteLine(
                        note.NoteStyle + "," +       //0
                        note.NoteSize + "," +        //1
                        note.NotePosition.X + "," +  //2
                        note.NotePosition.Y + "," +  //3
                        note.StartPosition.X + "," + //4
                        note.StartPosition.Y + "," + //5
                        note.EndPosition.X + "," +   //6
                        note.EndPosition.Y + "," +   //7
                        note.AirDirection + "," +    //8
                        note.LongNoteNumber + "," +  //9
                        indx);                       //10
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

        public List<MusicScore> Scores
        {
            get { return this.scores; }
        }

        public List<MusicScore2> Scores2
        {
            get { return this.scores2; }
        }

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
            Form2 f = new Form2(this);
            f.loadExportData(songID, title, artist, designer, wave, jacket, difficulty, playLevel, BPM, exDir, offset, isWhile);
            f.ShowDialog(this);
            setEdited(true);
        }

        private void BPMupdown_ValueChanged(object sender, EventArgs e)
        {
            BPM = BPMupdown.Value;
            scores2[0].update();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Shift) radioAdd.Checked = true;
            else if (e.KeyCode == Keys.E && e.Modifiers == Keys.Shift) radioEdit.Checked = true;
            else if (e.KeyCode == Keys.D && e.Modifiers == Keys.Shift) radioDelete.Checked = true;
            else if (e.KeyCode == Keys.B && e.Modifiers == Keys.None)
            {
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
            else if (e.KeyCode == Keys.F1) { MusicScore2.SelectedNoteStyle = "Tap"; activeNotesButton(Tap); }
            else if (e.KeyCode == Keys.F2) { MusicScore2.SelectedNoteStyle = "ExTap"; activeNotesButton(ExTap); }
            else if (e.KeyCode == Keys.F3) { MusicScore2.SelectedNoteStyle = "Flick"; activeNotesButton(Flick); }
            else if (e.KeyCode == Keys.F4) { MusicScore2.SelectedNoteStyle = "HellTap"; activeNotesButton(HellTap); }
            else if (e.KeyCode == Keys.F5) { MusicScore2.SelectedNoteStyle = "Hold"; activeNotesButton(Hold); }
            else if (e.KeyCode == Keys.F6) { MusicScore2.SelectedNoteStyle = "Slide"; activeNotesButton(Slide); }
            else if (e.KeyCode == Keys.F7) { MusicScore2.SelectedNoteStyle = "AirUp"; activeNotesButton(AirUp); }
            else if (e.KeyCode == Keys.F8) { MusicScore2.SelectedNoteStyle = "AirDown"; activeNotesButton(AirDown); }
            else if (e.KeyCode == Keys.F9) { MusicScore2.SelectedNoteStyle = "AirLine"; activeNotesButton(AirLine); }
            else if (AirUp.IsActive)
            {
                if (e.KeyCode == Keys.L) AirUp.setDirection("Left");
                else if (e.KeyCode == Keys.C) AirUp.setDirection("Center");
                else if (e.KeyCode == Keys.R) AirUp.setDirection("Right");
                AirUp.setAirDirection();
            }
            else if (AirDown.IsActive)
            {
                if (e.KeyCode == Keys.L) AirDown.setDirection("Left");
                else if (e.KeyCode == Keys.C) AirDown.setDirection("Center");
                else if (e.KeyCode == Keys.R) AirDown.setDirection("Right");
                AirDown.setAirDirection();
            }
            else if (e.KeyCode == Keys.OemPeriod && activeNotesButton().TrackBar_Size != 16)
            {
                activeNotesButton().TrackBar_Size++;
                activeNotesButton().trackBar1_Scroll(sender, e);
            }
            else if (e.KeyCode == Keys.Oemcomma && activeNotesButton().TrackBar_Size != 1)
            {
                activeNotesButton().TrackBar_Size--;
                activeNotesButton().trackBar1_Scroll(sender, e);
            }
            //MessageBox.Show(activeNotesButton().NotesName);
            setEditStatus(sender, e);
            //MessageBox.Show("KeyDown");
        }

        private NotesButton activeNotesButton()
        {
            if (Tap.IsActive) return Tap;
            else if (ExTap.IsActive) return ExTap;
            else if (Flick.IsActive) return Flick;
            else if (HellTap.IsActive) return HellTap;
            else if (Hold.IsActive) return Hold;
            else if (Slide.IsActive) return Slide;
            else if (AirUp.IsActive) return AirUp;
            else if (AirDown.IsActive) return AirDown;
            else if (AirLine.IsActive) return AirLine;
            else return null;
        }

        private void fileMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void saveExportData(string _songID, string _title, string _artist, string _designer, string _wave, string _jacket, int _difficulty, decimal _playLevel, decimal _BPM, string _exDir, decimal _offset, bool _isWhile)
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
            for (int i = 0; i < scores2.Count(); i++)
            {
                scores2[i].deleteAllNotes();
            }
            setTotalNotes();
            songID = "defaultID";
            title = "defaultTitle";
            artist = "defaultArtist";
            designer = "defaultDesigner";
            wave = "";
            jacket = "";
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
                //initialize musicscore
                for (int i = 0; i < scores2.Count(); i++)
                {
                    scores2[i].deleteAllNotes();
                }
                setTotalNotes();
                //
                System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                fileName = ofd.SafeFileName;//MessageBox.Show(fileName);
                string dataLine;
                string[] noteData;
                int indx, msIndex;
                dataLine = sr.ReadLine();
                if (dataLine == "dymsVersion:0.3")
                {
                    dymsDataVersion = "0.3";
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
                else if (dataLine == "dymsVersion:0.2.2")
                {
                    dymsDataVersion = "0.2.2";
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
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    wave = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    offset = decimal.Parse(noteData[1]);
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    jacket = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    BPM = decimal.Parse(noteData[1]);
                    BPMupdown.Value = BPM;
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    longNoteNumber = int.Parse(noteData[1]);
                }
                else if(dataLine == "dymsVersion:0.2")
                {
                    dymsDataVersion = "0.2";
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
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    wave = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    offset = decimal.Parse(noteData[1]);
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    jacket = noteData[1];
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    BPM = decimal.Parse(noteData[1]);
                    BPMupdown.Value = BPM;
                }
                else
                {
                    dymsDataVersion = "0.1";
                    noteData = dataLine.Split(',');
                    indx = int.Parse(noteData[12]);
                    scores2[indx].setNote(noteData, "0.1");
                }
                if (dymsDataVersion == "0.1") msIndex = 12;
                else msIndex = 10;
                while(sr.Peek() > -1)
                {
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(',');
                    indx = int.Parse(noteData[msIndex]);
                    scores2[indx].setNote(noteData, dymsDataVersion);
                }
                for (int i = 0; i < maxScore; i++) scores2[i].update();
                setEdited(false);
                isNew = false;
                sr.Close();
            }
            Text = ofd.SafeFileName + appName;
            fileName = ofd.FileName;
            pathName = ofd.FileName;
        }

        public static void activeNotesButton(NotesButton notesButton)
        {
            if(prevNotesButton == null)
            {
                prevSpecialButton.notesButtonInactive();
                prevNotesButton = notesButton;
            }
            if(notesButton != prevNotesButton)
            {
                prevNotesButton.notesButtonInactive();
                prevNotesButton = notesButton;
            }
            notesButton.notesButtonActive();
        }

        public static void activeNotesButton(SpecialButton notesButton)
        {
            if (prevNotesButton != null)
            {
                prevNotesButton.notesButtonInactive();
                prevNotesButton = null;
            }
        }

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

        public void setNote(int indx, Note note)
        {
            scores[indx].setNote(note);
        }

        public void deleteNote(int indx, Note note)
        {
            note.Dispose();
            scores[indx].notes.Remove(note);
        }

        public void setTotalNotes()
        {
            int totalNotes = 0;
            foreach(MusicScore2 score in scores2)
            {
                totalNotes += score.shortNotes.Count;
            }
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
            if (radioAdd.Checked) MusicScore2.SelectedEditStatus = "Add";
            else if (radioEdit.Checked) MusicScore2.SelectedEditStatus = "Edit";
            else MusicScore2.SelectedEditStatus = "Delete";
        }

        private void comboBoxBeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            MusicScore2.SelectedBeat = int.Parse(this.comboBoxBeat.Text);
        }

        private void comboBoxGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            MusicScore2.SelectedGrid = int.Parse(this.comboBoxGrid.Text);
        }
    }
}