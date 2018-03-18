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

namespace NotesEditerforD
{
    public partial class Form2 : Form
    {
        private Form1 form1;
        private ScoreRoot sRoot;
        private int measure, lastScore;
        private static int maxBeatDevide = 192;
        private string wavePath, jacketPath;
        private int topMargin, bottomMargin, leftMargin, rightMargin;
        public Form2(Form1 _form1, ScoreRoot _sRoot)
        {
            InitializeComponent();
            difficultyComboBox.SelectedIndex = 0;
            form1 = _form1;
            sRoot = _sRoot;
            topMargin = MusicScore.TopMargin;
            bottomMargin = MusicScore.BottomMargin;
            leftMargin = MusicScore.LeftMargin;
            rightMargin = MusicScore.RightMargin;

            BPMUpDown.Maximum = _form1.BPM_MAX;
            BPMUpDown.Minimum = _form1.BPM_MIN;

            object sender = new object();
            EventArgs e = new EventArgs();
            previewButton_Click(sender, e);

            previewBox.Controls.Add(previewTitle);
            previewBox.Controls.Add(previewArtist);
            previewBox.Controls.Add(previewLevel);
            previewBox.Controls.Add(previewDesigner);
            previewBox.Controls.Add(previewBPM);
            previewBox.Controls.Add(previewWELevel);
            previewTitle.Top -= previewBox.Top;
            previewTitle.Left -= previewBox.Left;
            previewArtist.Top -= previewBox.Top;
            previewArtist.Left -= previewBox.Left;
            previewLevel.Top -= previewBox.Top;
            previewLevel.Left -= previewBox.Left;
            previewDesigner.Top -= previewBox.Top;
            previewDesigner.Left -= previewBox.Left;
            previewBPM.Top -= previewBox.Top;
            previewBPM.Left -= previewBox.Left;
            previewWELevel.Top -= previewBox.Top;
            previewWELevel.Left -= previewBox.Left;
        }

        public void loadExportData(string _songID, string _title, string _artist, string _designer, string _wave, string _jacket, int _difficulty, decimal _playLevel, decimal _BPM, string _exDir, decimal _offset, bool isWhile, string _weStr)
        {
            textBoxID.Text = _songID;
            textBoxTitle.Text = _title;
            textBoxArtist.Text = _artist;
            textBoxDesigner.Text = _designer;
            wavePath = _wave;
            textBoxWAVE.Text = Path.GetFileName(_wave);
            jacketPath = _jacket;
            textBoxJacket.Text = Path.GetFileName(_jacket);
            difficultyComboBox.SelectedIndex = _difficulty;
            playLevelUpDown.Value = _playLevel;
            BPMUpDown.Value = (decimal)_BPM;
            textBoxExport.Text = _exDir;
            offsetUpDown.Value = _offset;
            checkBoxWhile.Checked = isWhile;
            textBoxWE.Text = _weStr;
        }

        private void export_Click(object sender, EventArgs e)
        {
            form1.saveExportData(textBoxID.Text, textBoxTitle.Text, textBoxArtist.Text, textBoxDesigner.Text, wavePath, jacketPath, difficultyComboBox.SelectedIndex, playLevelUpDown.Value, BPMUpDown.Value, textBoxExport.Text, offsetUpDown.Value, checkBoxWhile.Checked, textBoxWE.Text);
            if (textBoxExport.Text.Length == 0) MessageBox.Show("保存先を選択してください");
            else if (textBoxTitle.Text.Length == 0) MessageBox.Show("タイトルを入力してください");
            else if (!File.Exists(wavePath) && textBoxWAVE.Text.Length != 0) MessageBox.Show("曲ファイルが見つかりません\nファイルを選択し直してください");
            else if (!File.Exists(jacketPath) && textBoxJacket.Text.Length != 0) MessageBox.Show("ジャケットファイルが見つかりません\nファイルを選択し直してください");
            else
            {
                string path = textBoxExport.Text + "\\" + textBoxTitle.Text + "\\" + textBoxTitle.Text + ".sus";//susファイルのパス
                if (!Directory.Exists(textBoxExport.Text + "\\" + textBoxTitle.Text)) Directory.CreateDirectory(textBoxExport.Text + "\\" + textBoxTitle.Text);
                if (System.IO.File.Exists(path))//すでに同名のsusがあるか
                {
                    DialogResult result = MessageBox.Show("ファイルを上書きしますか？", "確認", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        susExport(path);
                        this.Dispose();
                    }
                    else { }
                }
                else
                {
                    susExport(path);
                    this.Dispose();
                }
                
                if (!File.Exists(Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(wavePath))) && wavePath.Length != 0)
                {
                    FileInfo waveFI = new FileInfo(wavePath);
                    FileInfo copyWaveFile = waveFI.CopyTo(Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(wavePath)));
                }
                if (!File.Exists(Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(jacketPath))) && jacketPath.Length != 0)
                {
                    FileInfo jacketFI = new FileInfo(jacketPath);
                    FileInfo copyJacketFile = jacketFI.CopyTo(Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(jacketPath)));
                }
            }
        }

        public void susExport(string path)
        {
            string level = setLevel(playLevelUpDown.Value);
            int Y;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
            Encoding.GetEncoding("UTF-8");
            sw.WriteLine("This file was exported by NotesEditorforD.");
            sw.WriteLine();
            sw.WriteLine("#SONGID " + '"' + textBoxID.Text + '"');
            sw.WriteLine("#TITLE " + '"' + textBoxTitle.Text + '"');
            sw.WriteLine("#ARTIST " + '"' + textBoxArtist.Text + '"');
            sw.WriteLine("#DESIGNER " + '"' + textBoxDesigner.Text + '"');
            if(difficultyComboBox.SelectedIndex == 4)
            {
                sw.WriteLine("#DIFFICULTY " + difficultyComboBox.SelectedIndex + ":" + textBoxWE.Text);
                sw.WriteLine("#PLAYLEVEL " + (int)playLevelUpDown.Value);
            }
            else
            {
                sw.WriteLine("#DIFFICULTY " + difficultyComboBox.SelectedIndex);
                sw.WriteLine("#PLAYLEVEL " + level);
            }
            sw.WriteLine("//SCORECONSTANT " + playLevelUpDown.Value);
            sw.WriteLine("#WAVE " + '"' + textBoxWAVE.Text + '"');
            sw.WriteLine("#WAVEOFFSET " + offsetUpDown.Value);
            sw.WriteLine("#JACKET " + '"' + textBoxJacket.Text + '"');
            sw.WriteLine();
            //
            for (lastScore = form1.MaxScore - 1; sRoot.Scores[lastScore].specialNotes.Count == 0; lastScore--) if (lastScore < 1) { lastScore = 0; break; }
            //
            sw.WriteLine("#BPM01:" + BPMUpDown.Value);
            sw.WriteLine("#00008:01");
            char[] numAlpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string[] BPMStrArr = new string[1296];
            for (int i = 0; i < numAlpha.Length; i++)
                for (int j = 0; j < numAlpha.Length; j++)
                    BPMStrArr[numAlpha.Length * i + j] = numAlpha[i].ToString() + numAlpha[j].ToString();//"00" ~ "ZZ"
            int BPMBeatDevide = 8;
            var objBPM = new[] { new { BPM = BPMUpDown.Value, BPMStrArrIdx = 1} }.ToList(); int curBPMStrArrIdx = 1;
            string[] spLane1 = new string[BPMBeatDevide];
            string[] spLane2 = new string[BPMBeatDevide];
            
            for (measure = 0; measure <= lastScore; measure++)
            {
                for (int i = 0; i < BPMBeatDevide; i++) spLane1[i] = "00";//initialize lane
                for (int i = 0; i < BPMBeatDevide; i++) spLane2[i] = "00";//initialize lane
                foreach (ShortNote _note in sRoot.Scores[measure].specialNotes)
                {
                    if (_note.NoteStyle == "BPM")
                    {
                        Y = BPMBeatDevide - (_note.NotePosition.Y - 2) / (384 / BPMBeatDevide);
                        if (Y < 0)
                        {
                            Y += BPMBeatDevide;
                            if (!objBPM.Where(x => x.BPM == _note.SpecialValue).Any())
                            {
                                objBPM.Add(new { BPM = _note.SpecialValue, BPMStrArrIdx = ++curBPMStrArrIdx });
                                sw.WriteLine("#BPM" + BPMStrArr[curBPMStrArrIdx] + ":" + _note.SpecialValue);
                            }
                            spLane1[Y] = BPMStrArr[objBPM.Find(x => x.BPM == _note.SpecialValue).BPMStrArrIdx];
                        }
                        else
                        {
                            if (!objBPM.Where(x => x.BPM == _note.SpecialValue).Any())
                            {
                                objBPM.Add(new { BPM = _note.SpecialValue, BPMStrArrIdx = ++curBPMStrArrIdx });
                                sw.WriteLine("#BPM" + BPMStrArr[curBPMStrArrIdx] + ":" + _note.SpecialValue);
                            }
                            spLane2[Y] = BPMStrArr[objBPM.Find(x => x.BPM == _note.SpecialValue).BPMStrArrIdx];
                        }
                    }
                }
                //*
                string cur = "01";
                if(isBPMModified(spLane1, BPMBeatDevide)) for (int i = 0; i < BPMBeatDevide; i++)
                {
                    if (spLane1[i] == "00") spLane1[i] = cur;
                    else if (spLane1[i] != cur) cur = spLane1[i];
                }
                if (isBPMModified(spLane2, BPMBeatDevide)) for (int i = 0; i < BPMBeatDevide; i++)
                {
                    if (spLane2[i] == "00") spLane2[i] = cur;
                    else if (spLane2[i] != cur) cur = spLane2[i];
                }
                //*/
                if (isBPMModified(spLane1, BPMBeatDevide))
                {
                    if (checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "08:");
                    else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "08:");
                    for (int j = 0; j < BPMBeatDevide; j++) { sw.Write(spLane1[j]); }
                    sw.Write(Environment.NewLine);
                }
                if (isBPMModified(spLane2, BPMBeatDevide))
                {
                    if (checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "08:");
                    else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "08:");
                    for (int j = 0; j < BPMBeatDevide; j++) { sw.Write(spLane2[j]); }
                    sw.Write(Environment.NewLine);
                }
            }
            //
            sw.WriteLine();
            sw.Write("#TIL00:\"");
            
            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach(ShortNote _note in sRoot.Scores[measure].specialNotes)
                {
                    if(_note.NoteStyle == "Speed")
                    {
                        Y = (768 - (_note.NotePosition.Y - 2)) * 2;
                        if (Y < 768)
                        {
                            if(checkBoxWhile.Checked) sw.Write(2 * measure + 1 + "'" + Y + ":" + _note.SpecialValue + ",");
                            else sw.Write(2 * measure + "'" + Y + ":" + _note.SpecialValue + ",");
                        }
                        else
                        {
                            Y -= 768;
                            if(checkBoxWhile.Checked) sw.Write(2 * (measure + 1) + "'" + Y + ":" + _note.SpecialValue + ",");
                            else sw.Write(2 * measure + 1 + "'" + Y + ":" + _note.SpecialValue + ",");
                        }
                    }
                }
            }
            sw.WriteLine("\"");
            sw.WriteLine("#HISPEED 00");
            sw.WriteLine();

            for (lastScore = form1.MaxScore - 1; sRoot.Scores[lastScore].shortNotes.Count == 0; lastScore--) if (lastScore < 1) { lastScore = 0; break; }
            string[,] lane1 = new string[16, maxBeatDevide];//lane, beat, odd
            string[,] lane2 = new string[16, maxBeatDevide];//lane, beat, even
            char[] sign = "abcdefghijklmnopqrstuvwxyz".ToCharArray();//ロングレーン用の識別子 最大zまで拡張可能
            int sgnindx = 0;//for LongLane
            bool[,] isUsedLane1 = new bool[lastScore + 1, sign.Length];
            bool[,] isUsedLane2 = new bool[lastScore + 1, sign.Length];
            string[,,,] longLane1;// = new string[lastScore + 1, 16, maxBeatDevide, sign[sgnindx]];
            string[,,,] longLane2;// = new string[lastScore + 1, 16, maxBeatDevide, sign[sgnindx]];
            int noteSize;
            int _X, _Y;
            int _beatLCM1, _beatLCM2;

            //[h,i,j,k]=lane,beat,layer
            //for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            //for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane

            for (measure = 0; measure <= lastScore; measure++)
            {
                ////////////////////////////////////////////////↓Tap, ExTap, Flick, HellTap
                _beatLCM1 = beatLCM(sRoot.Scores[measure].shortNotes, 0, 1);//譜面下
                _beatLCM2 = beatLCM(sRoot.Scores[measure].shortNotes, 1, 1);//譜面上
                if(_beatLCM1 != 0)
                {
                    lane1 = new string[16, _beatLCM1];
                    for (int i = 0; i < 16; i++) for (int j = 0; j < lane1.GetLength(1); j++) lane1[i, j] = "00";//initialize lane
                }
                if(_beatLCM2 != 0)
                {
                    lane2 = new string[16, _beatLCM2];
                    for (int i = 0; i < 16; i++) for (int j = 0; j < lane2.GetLength(1); j++) lane2[i, j] = "00";//initialize lane
                }
                    
                foreach (ShortNote note in sRoot.Scores[measure].shortNotes)//レーンを設定
                {
                    _X = note.LocalPosition.X;
                    
                    noteSize = note.NoteSize;
                    if (note.LocalPosition.Measure % 2 != 0)//odd
                    {
                        _Y = note.LocalPosition.BeatNumber * _beatLCM1 / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "Tap":
                                if (noteSize == 16) lane1[_X, _Y] = "1g";
                                else lane1[_X, _Y] = "1" + noteSize.ToString("x");
                                break;
                            case "ExTap":
                                if (noteSize == 16) lane1[_X, _Y] = "2g";
                                else lane1[_X, _Y] = "2" + noteSize.ToString("x");
                                break;
                            case "Flick":
                                if (noteSize == 16) lane1[_X, _Y] = "3g";
                                else lane1[_X, _Y] = "3" + noteSize.ToString("x");
                                break;
                            case "HellTap":
                                if (noteSize == 16) lane1[_X, _Y] = "4g";
                                else lane1[_X, _Y] = "4" + noteSize.ToString("x");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (note.LocalPosition.Beat == note.LocalPosition.BeatNumber) continue;
                        _Y = note.LocalPosition.BeatNumber * _beatLCM2 / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "Tap":
                                if (noteSize == 16) lane2[_X, _Y] = "1g";
                                else lane2[_X, _Y] = "1" + noteSize.ToString("x");
                                break;
                            case "ExTap":
                                if (noteSize == 16) lane2[_X, _Y] = "2g";
                                else lane2[_X, _Y] = "2" + noteSize.ToString("x");
                                break;
                            case "Flick":
                                if (noteSize == 16) lane2[_X, _Y] = "3g";
                                else lane2[_X, _Y] = "3" + noteSize.ToString("x");
                                break;
                            case "HellTap":
                                if (noteSize == 16) lane2[_X, _Y] = "4g";
                                else lane2[_X, _Y] = "4" + noteSize.ToString("x");
                                break;
                            default:
                                break;
                        }
                    }
                }
                for (int i = 0; i < 16; i++)//レーンを出力//odd lane
                {
                    if (_beatLCM1 != 0 && isModified(lane1, i))
                    {
                        if (checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        for (int j = 0; j < lane1.GetLength(1); j++) { sw.Write(lane1[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }
                for (int i = 0; i < 16; i++)//even lane
                {
                    if (_beatLCM2 != 0 && isModified(lane2, i))
                    {
                        if (checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        for (int j = 0; j < lane2.GetLength(1); j++) { sw.Write(lane2[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }
                //*
                ////////////////////////////////////////////////////////////↓Air
                _beatLCM1 = beatLCM(sRoot.Scores[measure].shortNotes, 0, 5);//譜面下
                _beatLCM2 = beatLCM(sRoot.Scores[measure].shortNotes, 1, 5);//譜面上
                if (_beatLCM1 != 0)
                {
                    lane1 = new string[16, _beatLCM1];
                    for (int i = 0; i < 16; i++) for (int j = 0; j < lane1.GetLength(1); j++) lane1[i, j] = "00";//initialize lane
                }
                if (_beatLCM2 != 0)
                {
                    lane2 = new string[16, _beatLCM2];
                    for (int i = 0; i < 16; i++) for (int j = 0; j < lane2.GetLength(1); j++) lane2[i, j] = "00";//initialize lane
                }

                foreach (ShortNote note in sRoot.Scores[measure].shortNotes)//レーンを設定
                {
                    _X = note.LocalPosition.X;

                    noteSize = note.NoteSize;
                    if (note.LocalPosition.Measure % 2 != 0)
                    {
                        _Y = note.LocalPosition.BeatNumber * _beatLCM1 / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "AirUp":
                                switch (note.AirDirection)
                                {
                                    case "Center":
                                        if (noteSize == 16) lane1[_X, _Y] = "1g";
                                        else lane1[_X, _Y] = "1" + noteSize.ToString("x");
                                        break;
                                    case "Left":
                                        if (noteSize == 16) lane1[_X, _Y] = "3g";
                                        else lane1[_X, _Y] = "3" + noteSize.ToString("x");
                                        break;
                                    case "Right":
                                        if (noteSize == 16) lane1[_X, _Y] = "4g";
                                        else lane1[_X, _Y] = "4" + noteSize.ToString("x");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "AirDown":
                                switch (note.AirDirection)
                                {
                                    case "Center":
                                        if (noteSize == 16) lane1[_X, _Y] = "2g";
                                        else lane1[_X, _Y] = "2" + noteSize.ToString("x");
                                        break;
                                    case "Left":
                                        if (noteSize == 16) lane1[_X, _Y] = "5g";
                                        else lane1[_X, _Y] = "5" + noteSize.ToString("x");
                                        break;
                                    case "Right":
                                        if (noteSize == 16) lane1[_X, _Y] = "6g";
                                        else lane1[_X, _Y] = "6" + noteSize.ToString("x");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (note.LocalPosition.Beat == note.LocalPosition.BeatNumber) continue;
                        _Y = note.LocalPosition.BeatNumber * _beatLCM2 / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "AirUp":
                                switch (note.AirDirection)
                                {
                                    case "Center":
                                        if (noteSize == 16) lane2[_X, _Y] = "1g";
                                        else lane2[_X, _Y] = "1" + noteSize.ToString("x");
                                        break;
                                    case "Left":
                                        if (noteSize == 16) lane2[_X, _Y] = "3g";
                                        else lane2[_X, _Y] = "3" + noteSize.ToString("x");
                                        break;
                                    case "Right":
                                        if (noteSize == 16) lane2[_X, _Y] = "4g";
                                        else lane2[_X, _Y] = "4" + noteSize.ToString("x");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "AirDown":
                                switch (note.AirDirection)
                                {
                                    case "Center":
                                        if (noteSize == 16) lane2[_X, _Y] = "2g";
                                        else lane2[_X, _Y] = "2" + noteSize.ToString("x");
                                        break;
                                    case "Left":
                                        if (noteSize == 16) lane2[_X, _Y] = "5g";
                                        else lane2[_X, _Y] = "5" + noteSize.ToString("x");
                                        break;
                                    case "Right":
                                        if (noteSize == 16) lane2[_X, _Y] = "6g";
                                        else lane2[_X, _Y] = "6" + noteSize.ToString("x");
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                for (int i = 0; i < 16; i++)//レーンを出力//odd lane
                {
                    if (_beatLCM1 != 0 && isModified(lane1, i))
                    {
                        if(checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        for (int j = 0; j < lane1.GetLength(1); j++) { sw.Write(lane1[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }
                for (int i = 0; i < 16; i++)//even lane
                {
                    if (_beatLCM2 != 0 && isModified(lane2, i))
                    {
                        if(checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        for (int j = 0; j < lane2.GetLength(1); j++) { sw.Write(lane2[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }//*/
            }
            /////////////////////////////////////////////////↓Hold
            int[,] _beatLCMArray2x = new int[2, lastScore + 1];
            int[,] _beatLCMArray3x = new int[2, lastScore + 1];
            int[,] _beatLCMArray4x = new int[2, lastScore + 1];
            for (measure = 0; measure <= lastScore; measure++)
            {
                _beatLCMArray2x[0, measure] = beatLCM(sRoot.Scores[measure].shortNotes, 0, 2);//譜面下Hold
                _beatLCMArray2x[1, measure] = beatLCM(sRoot.Scores[measure].shortNotes, 1, 2);//譜面上Hold
                _beatLCMArray3x[0, measure] = beatLCM(sRoot.Scores[measure].shortNotes, 0, 3);//譜面下Slide
                _beatLCMArray3x[1, measure] = beatLCM(sRoot.Scores[measure].shortNotes, 1, 3);//譜面上Slide
                _beatLCMArray4x[0, measure] = beatLCM(sRoot.Scores[measure].shortNotes, 0, 4);//譜面下AirLine
                _beatLCMArray4x[1, measure] = beatLCM(sRoot.Scores[measure].shortNotes, 1, 4);//譜面上AirLine
            }
            longLane1 = new string[lastScore + 1, 16, 1000, sign[sgnindx]];
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < longLane1.GetLength(2); j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            longLane2 = new string[lastScore + 1, 16, 1000, sign[sgnindx]];
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < longLane2.GetLength(2); j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane
            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in sRoot.Scores[measure].shortNotes)//レーンを設定
                {
                    _X = note.LocalPosition.X;
                    noteSize = note.NoteSize;
                    if (note.LocalPosition.Measure % 2 != 0)//odd
                    {
                        _Y = note.LocalPosition.BeatNumber * _beatLCMArray2x[0, measure] / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "Hold":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";
                                else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in sRoot.Scores[_measure].shortNotes)//_note
                                    {
                                        _X = _note.LocalPosition.X;
                                        
                                        //noteSize = _note.NoteSize;//?
                                        if (_note.NoteStyle == "HoldEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray2x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray2x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            flg = true;
                                            break;
                                        }
                                    }
                                    if (flg) break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (note.LocalPosition.Beat == note.LocalPosition.BeatNumber) continue;
                        _Y = note.LocalPosition.BeatNumber * _beatLCMArray2x[1, measure] / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "Hold":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in sRoot.Scores[_measure].shortNotes)
                                    {
                                        _X = _note.LocalPosition.X;

                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "HoldEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray2x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray2x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            flg = true;
                                            break;
                                        }
                                    }
                                    if (flg) break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            for (measure = 0; measure <= lastScore; measure++)
            {
                for (int i = 0; i < 16; i++)//レーンを出力
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (_beatLCMArray2x[0, measure] != 0 && isModified(longLane1, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < _beatLCMArray2x[0, measure]; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (_beatLCMArray2x[1, measure] != 0 && isModified(longLane2, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < _beatLCMArray2x[1, measure]; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }
            /////////////////////////////////////////////////↓Slide      
            //[h,i,j,k]=lane,beat,layer
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < 1000; j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < 1000; j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane

            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in sRoot.Scores[measure].shortNotes)//レーンを設定
                {
                    _X = note.LocalPosition.X;
                    noteSize = note.NoteSize;
                    if (note.LocalPosition.Measure % 2 != 0)//odd
                    {
                        _Y = note.LocalPosition.BeatNumber * _beatLCMArray3x[0, measure] / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "Slide"://この内部ではlane1とlane2で同じコード...ではない
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";//コピペ後は変更
                                else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");//コピペ後は変更
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure;//, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in sRoot.Scores[_measure].shortNotes)
                                    {
                                        _X = _note.LocalPosition.X;
                                        
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "SlideRelay" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideTap" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideCurve" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            flg = true;
                                        }
                                    }
                                    if (flg) break;
                                }
                                break;//
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (note.LocalPosition.Beat == note.LocalPosition.BeatNumber) continue;
                        _Y = note.LocalPosition.BeatNumber * _beatLCMArray3x[1, measure] / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "Slide"://
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure;//, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in sRoot.Scores[_measure].shortNotes)
                                    {
                                        _X = _note.LocalPosition.X;

                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "SlideRelay" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideTap" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideCurve" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray3x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            }
                                            flg = true;
                                        }
                                    }
                                    if (flg) break;
                                }
                                break;//
                            default:
                                break;
                        }
                    }
                }
            }
            for (measure = 0; measure <= lastScore; measure++)
            {
                for (int i = 0; i < 16; i++)//レーンを出力
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (_beatLCMArray3x[0, measure] != 0 && isModified(longLane1, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < _beatLCMArray3x[0, measure]; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (_beatLCMArray3x[1, measure] != 0 && isModified(longLane2, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < _beatLCMArray3x[1, measure]; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }
            //
            /////////////////////////////////////////////////↓AirLine
            //[h,i,j,k]=lane,beat,layer
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < 1000; j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < 1000; j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane

            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in sRoot.Scores[measure].shortNotes)//レーンを設定
                {
                    _X = note.LocalPosition.X;
                    
                    noteSize = note.NoteSize;
                    if (note.LocalPosition.Measure % 2 != 0)//odd
                    {
                        _Y = note.LocalPosition.BeatNumber * _beatLCMArray4x[0, measure] / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "AirBegin":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";
                                else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in sRoot.Scores[_measure].shortNotes)
                                    {
                                        _X = _note.LocalPosition.X;
                                        
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "AirAction" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "AirEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                            flg = true;
                                            break;
                                        }
                                    }
                                    if (flg) break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (note.LocalPosition.Beat == note.LocalPosition.BeatNumber) continue;
                        _Y = note.LocalPosition.BeatNumber * _beatLCMArray4x[1, measure] / note.LocalPosition.Beat;
                        switch (note.NoteStyle)
                        {
                            case "AirBegin":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in sRoot.Scores[_measure].shortNotes)
                                    {
                                        _X = _note.LocalPosition.X;

                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "AirAction" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "AirEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_note.LocalPosition.Measure % 2 != 0)
                                            {
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[0, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_note.LocalPosition.Beat == _note.LocalPosition.BeatNumber) continue;
                                                _Y = _note.LocalPosition.BeatNumber * _beatLCMArray4x[1, _measure] / _note.LocalPosition.Beat;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                            flg = true;
                                            break;
                                        }
                                    }
                                    if (flg) break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            for (measure = 0; measure <= lastScore; measure++)
            {
                for (int i = 0; i < 16; i++)//レーンを出力
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (_beatLCMArray4x[0, measure] != 0 && isModified(longLane1, measure, i, _sgnindx))
                        {
                            if (checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < _beatLCMArray4x[0, measure]; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (_beatLCMArray4x[1, measure] != 0 && isModified(longLane2, measure, i, _sgnindx))
                        {
                            if (checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < _beatLCMArray4x[1, measure]; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }
            //
            sw.Close();
        }

        private int beatLCM(List<ShortNote> _notes, int sign, int noteType)
        {
            List<ShortNote> notes = new List<ShortNote>();
            foreach(ShortNote note in _notes)
            {
                if(sign == 0 && 778 - note.NotePosition.Y - MusicScore.BottomMargin < 386)//譜面下側
                {
                    switch (noteType)
                    {
                        case 1://Tap,ExTap,Flick,HellTap
                            if (new string[] { "Tap", "ExTap", "Flick", "HellTap" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 2://Hold
                            if (new string[] { "Hold", "HoldEnd" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 3://Slide
                            if (new string[] { "Slide", "SlideTap", "SlideRelay", "SlideCurve", "SlideEnd" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 4://AirLine
                            if (new string[] { "AirBegin", "AirAction", "AirEnd" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 5://Air
                            if (new string[] { "AirUp", "AirDown" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 8://BPM
                            if (note.NoteStyle == "BPM") notes.Add(note);
                            break;
                        default:
                            break;
                    }
                }
                else if(sign == 1 && 778 - note.NotePosition.Y - MusicScore.BottomMargin >= 386 && note.NotePosition.Y != 2)//譜面上側
                {
                    switch (noteType)
                    {
                        case 1://Tap,ExTap,Flick,HellTap
                            if (new string[] { "Tap", "ExTap", "Flick", "HellTap"}.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 2://Hold
                            if (new string[] { "Hold", "HoldEnd" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 3://Slide
                            if (new string[] { "Slide", "SlideTap", "SlideRelay", "SlideCurve", "SlideEnd" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 4://AirLine
                            if (new string[] { "AirBegin", "AirAction", "AirEnd" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 5://Air
                            if (new string[] { "AirUp", "AirDown" }.Contains(note.NoteStyle)) notes.Add(note);
                            break;
                        case 8://BPM
                            if (note.NoteStyle == "BPM") notes.Add(note);
                            break;
                        default:
                            break;
                    }
                }
            }
            if (notes.Count == 0) return 0;//ノーツがないときは0を返しておわり
            return LCM(notes);
        }

        private int LCM(List<ShortNote> _notes)
        {
            int listSize = _notes.Count;
            if (listSize == 1) return _notes[0].LocalPosition.BeatNumber == 0 ? 1 : _notes[0].LocalPosition.Beat;
            if (listSize == 2)
            {
                if (_notes[0].LocalPosition.BeatNumber == 0 && _notes[1].LocalPosition.BeatNumber == 0) return 1;
                else if (_notes[0].LocalPosition.BeatNumber == 0 && _notes[1].LocalPosition.BeatNumber != 0) return _notes[1].LocalPosition.Beat;
                else if (_notes[0].LocalPosition.BeatNumber != 0 && _notes[1].LocalPosition.BeatNumber == 0) return _notes[0].LocalPosition.Beat;
                return _notes[0].LocalPosition.Beat * _notes[1].LocalPosition.Beat / GCD(_notes[0].LocalPosition.Beat, _notes[1].LocalPosition.Beat);
            }
            else
            {
                return LCM2(LCM(_notes.Take(listSize/2).ToList<ShortNote>()), LCM(_notes.Skip(listSize/2).ToList<ShortNote>()));
            }
        }

        private int LCM2(int a, int b)
        {
            return a * b / GCD(a, b);
        }

        private int GCD(int a, int b)
        {
            if (a == 0 || b == 0) return 1;
            if (a < b)
            {
                int tmp = a; a = b; b = tmp;
            }

            int r = a % b;
            while(r != 0)
            {
                a = b; b = r; r = a % b;
            }

            return b;
        }

        private bool isModified(string[,] lane, int i)
        {
            for (int j = 0; j < lane.GetLength(1); j++) if (lane[i,j] != "00") return true;
            return false;
        }

        private bool isBPMModified(string[] lane, int beatDevide)//for BPM
        {
            for (int j = 0; j < beatDevide; j++) if (lane[j] != "00") return true;
            return false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void titleLabel_Click(object sender, EventArgs e)
        {

        }

        private void artistLabel_Click(object sender, EventArgs e)
        {

        }

        private void difficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (difficultyComboBox.SelectedIndex == 4) textBoxWE.Enabled = true;
            else textBoxWE.Enabled = false;
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            Bitmap canvas = new Bitmap(Properties.Resources.Black, 286, 396);
            Bitmap jacket = Properties.Resources.noimage;
            if(File.Exists(jacketPath)) jacket = new Bitmap(jacketPath);
            Graphics g = Graphics.FromImage(canvas);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            if(jacket.Width < jacket.Height)//縦長
            {
                g.DrawImage(jacket, 40 + (210 - jacket.Width * 210 / jacket.Height) / 2, 25, jacket.Width * 210 / jacket.Height, 210);
            }
            else//横長か正方形
            {
                g.DrawImage(jacket, 40, 25 + (210 - jacket.Height * 210 / jacket.Width) / 2, 210, jacket.Height * 210 / jacket.Width);
            }
            previewWELevel.Text = "";
            switch (difficultyComboBox.SelectedIndex)
            {
                case 0://BASIC
                    //previewBox.BackgroundImage = Properties.Resources.frameBasic;
                    g.DrawImage(Properties.Resources.frameBasic, new Point(0, 0));
                    previewLevel.Text = setLevel(playLevelUpDown.Value);
                    if (previewLevel.Text.IndexOf("+") != -1)
                    {
                        previewLevel.Text = ((int)playLevelUpDown.Value).ToString();
                        g.DrawString("+", new Font("ＭＳ ゴシック", 17, FontStyle.Bold), Brushes.Black, new Rectangle(43, 239, 30, 30));
                    }
                    break;
                case 1://ADVANCED
                    //previewBox.BackgroundImage = Properties.Resources.frameAdvanced;
                    g.DrawImage(Properties.Resources.frameAdvanced, new Point(0, 0));
                    previewLevel.Text = setLevel(playLevelUpDown.Value);
                    if (previewLevel.Text.IndexOf("+") != -1)
                    {
                        previewLevel.Text = ((int)playLevelUpDown.Value).ToString();
                        g.DrawString("+", new Font("ＭＳ ゴシック", 17, FontStyle.Bold), Brushes.Black, new Rectangle(43, 239, 30, 30));
                    }
                    break;
                case 2://EXPERT
                    //previewBox.BackgroundImage = Properties.Resources.frameExpert;
                    g.DrawImage(Properties.Resources.frameExpert, new Point(0, 0));
                    previewLevel.Text = setLevel(playLevelUpDown.Value);
                    if (previewLevel.Text.IndexOf("+") != -1)
                    {
                        previewLevel.Text = ((int)playLevelUpDown.Value).ToString();
                        g.DrawString("+", new Font("ＭＳ ゴシック", 17, FontStyle.Bold), Brushes.Black, new Rectangle(43, 239, 30, 30));
                    }
                    break;
                case 3://MASTER
                    //previewBox.BackgroundImage = Properties.Resources.frameMaster;
                    g.DrawImage(Properties.Resources.frameMaster, new Point(0, 0));
                    previewLevel.Text = setLevel(playLevelUpDown.Value);
                    if (previewLevel.Text.IndexOf("+") != -1)
                    {
                        previewLevel.Text = ((int)playLevelUpDown.Value).ToString();
                        g.DrawString("+", new Font("ＭＳ ゴシック", 17, FontStyle.Bold), Brushes.Black, new Rectangle(43, 239, 30, 30));
                    }
                    break;
                case 4://WORLD'S END
                    //previewBox.BackgroundImage = Properties.Resources.frameWE;
                    g.DrawImage(Properties.Resources.frameWE, new Point(0, 0));
                    previewLevel.Text = textBoxWE.Text;
                    for (int i = 0; i < (int)playLevelUpDown.Value; i++) previewWELevel.Text += "☆";
                    break;
                default:
                    break;
            }
            
            previewBox.BackgroundImage = canvas;
            previewTitle.Text = textBoxTitle.Text;
            previewArtist.Text = textBoxArtist.Text;
            previewDesigner.Text = textBoxDesigner.Text;
            previewBPM.Text = BPMUpDown.Value.ToString();
            
            g.Dispose();
        }

        private string setLevel(decimal d)
        {
            string level = ((int)d).ToString();
            if (d - (int)d >= 0.7m) level += "+";
            return level;
        }

        private bool isModified(string[,,,] lane, int measure, int i, int sgnindx)
        {
            for (int j = 0; j < lane.GetLength(2); j++) if (lane[measure, i, j, sgnindx] != "00") return true;
            return false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            form1.saveExportData(textBoxID.Text, textBoxTitle.Text, textBoxArtist.Text, textBoxDesigner.Text, textBoxWAVE.Text, textBoxJacket.Text, difficultyComboBox.SelectedIndex, playLevelUpDown.Value, BPMUpDown.Value, textBoxExport.Text, offsetUpDown.Value, checkBoxWhile.Checked, textBoxWE.Text);
            this.Dispose();
        }

        private void buttonWave_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "default";
            ofd.Filter = "音楽ファイル(*.wav;*.ogg;*.mp3)|*.wav;*.ogg;*.mp3";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)//OKボタンがクリックされた時
            {
                textBoxWAVE.Text = ofd.SafeFileName;
                wavePath = ofd.FileName;
            }
        }

        private void buttonJacket_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "default";
            ofd.Filter = "画像ファイル(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)//OKボタンがクリックされた時
            {
                textBoxJacket.Text = ofd.SafeFileName;
                jacketPath = ofd.FileName;
            }
        }

        private void buttonexdir_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示する説明テキストを指定する
            fbd.Description = "保存先フォルダを指定してください。";
            //ルートフォルダを指定する
            //デフォルトでDesktop
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            fbd.SelectedPath = @"C:\Windows";
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                textBoxExport.Text = fbd.SelectedPath;
            }
        }
    }
}
