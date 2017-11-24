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
        private int maxScore = 100;
        List<MusicScore> scores = new List<MusicScore>();
        List<MusicScore2> scores2 = new List<MusicScore2>();
        private string songID, title, artist, designer, wave, jacket, exDir, dymsVersion, Version, appName, pathName;
        private int difficulty, longNoteNumber;
        private decimal BPM, playLevel, offset;
        private string fileName;
        private bool isEdited;
        public Form1()
        {
            InitializeComponent();
            dymsVersion = "0.2.2";
            Version = "0.2.2";
            comboBoxBeat.SelectedIndex = 1;
            MusicScore2.SelectedBeat = int.Parse(this.comboBoxBeat.Text);
            for (int i = 0; i < maxScore; i++)
            {
                musicScore2 = new MusicScore2();
                musicScore2.form1 = this;
                musicScore2.Index = i;
                flowLayoutPanelMusicScore.Controls.Add(musicScore2);
                scores2.Add(musicScore2);
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
            setEdited(false);

            longNoteNumber = 0;
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            if (!isEdited) return;
            if(fileName.Length == 0) { object _sender = new object(); EventArgs _e = new EventArgs(); saveAsMenuItem_Click(_sender, _e);  }
            if (pathName != null) { saveScores(pathName); setEdited(false); }
            else
            {
                /*
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "NewMusicScore.dyms";
                sfd.DefaultExt = ".dyms";
                sfd.Filter = "dymsファイル(NotesEditorforD) (.dyms)|*.dyms";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)//OKボタンがクリックされた時
                {
                    saveScores(sfd.FileName);
                    setEdited(false);
                }
                Text = sfd.FileName + appName;
                fileName = sfd.FileName;
                //*/
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
            }
            Text = sfd.FileName + appName;
            fileName = sfd.FileName;
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
            sw.WriteLine("WAVE:" + wave);
            sw.WriteLine("WAVEOFFSET:" + offset);
            sw.WriteLine("JACKET:" + jacket);
            sw.WriteLine("BASEBPM:" + BPM);
            sw.WriteLine("LongNoteNumber:" + longNoteNumber);
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
            f.loadExportData(songID, title, artist, designer, wave, jacket, difficulty, playLevel, BPM, exDir, offset);
            f.ShowDialog(this);
            setEdited(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void saveExportData(string _songID, string _title, string _artist, string _designer, string _wave, string _jacket, int _difficulty, decimal _playLevel, decimal _BPM, string _exDir, decimal _offset)
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
        }

        private void newMenuItem_Click(object sender, EventArgs e)//edt//xxxxxxxxxxxxxxxxx//ok
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
        }

        private void openMenuItem_Click(object sender, EventArgs e)//edt//xxxxxxxxxxxxxxxxxx
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
                if(dataLine == "dymsVersion:0.2.2")
                {
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
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(':');
                    longNoteNumber = int.Parse(noteData[1]);
                }
                else if(dataLine == "dymsVersion:0.2")
                {
                    //dymsVersion = "0.2";
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
                }
                else
                {
                    //dymsVersion = "0.1";
                    noteData = dataLine.Split(',');
                    indx = int.Parse(noteData[12]);
                    scores2[indx].setNote(noteData, dymsVersion);
                }
                if (dymsVersion == "0.1") msIndex = 12;
                else msIndex = 10;
                while(sr.Peek() > -1)
                {
                    dataLine = sr.ReadLine();
                    noteData = dataLine.Split(',');
                    indx = int.Parse(noteData[msIndex]);
                    scores2[indx].setNote(noteData, dymsVersion);
                }
                for (int i = 0; i < maxScore; i++) scores2[i].update();
                setEdited(false);
            }
            Text = ofd.SafeFileName + appName;
            fileName = ofd.FileName;
            pathName = ofd.FileName;
        }

        public static void activeNotesButton(NotesButton notesButton)
        {
            if(notesButton != prevNotesButton)
            {
                prevNotesButton.notesButtonInactive();
                prevNotesButton = notesButton;
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

        public int getBPM()
        {
            return (int)this.numericUpDownBPM.Value;
        }

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