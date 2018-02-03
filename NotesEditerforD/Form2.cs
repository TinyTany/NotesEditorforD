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
        private int measure, lastScore;
        private static int maxBeatDevide = 192;
        private string wavePath, jacketPath;
        private int topMargin, bottomMargin, leftMargin, rightMargin;
        public Form2(Form1 _form1)
        {
            InitializeComponent();
            difficultyComboBox.SelectedIndex = 0;
            form1 = _form1;
            topMargin = MusicScore2.TopMargin;
            bottomMargin = MusicScore2.BottomMargin;
            leftMargin = MusicScore2.LeftMargin;
            rightMargin = MusicScore2.RightMargin;
        }

        public void loadExportData(string _songID, string _title, string _artist, string _designer, string _wave, string _jacket, int _difficulty, decimal _playLevel, decimal _BPM, string _exDir, decimal _offset, bool isWhile)
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
        }

        private void export_Click(object sender, EventArgs e)
        {
            form1.saveExportData(textBoxID.Text, textBoxTitle.Text, textBoxArtist.Text, textBoxDesigner.Text, wavePath, jacketPath, difficultyComboBox.SelectedIndex, playLevelUpDown.Value, BPMUpDown.Value, textBoxExport.Text, offsetUpDown.Value, checkBoxWhile.Checked);
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
            string level;
            if (playLevelUpDown.Value < 7.7m) level = ((int)playLevelUpDown.Value).ToString();
            else if (playLevelUpDown.Value < 8m) level = "7+";
            else if (playLevelUpDown.Value < 8.7m) level = "8";
            else if (playLevelUpDown.Value < 9m) level = "8+";
            else if (playLevelUpDown.Value < 9.7m) level = "9";
            else if (playLevelUpDown.Value < 10m) level = "9+";
            else if (playLevelUpDown.Value < 10.7m) level = "10";
            else if (playLevelUpDown.Value < 11m) level = "10+";
            else if (playLevelUpDown.Value < 11.7m) level = "11";
            else if (playLevelUpDown.Value < 12m) level = "11+";
            else if (playLevelUpDown.Value < 12.7m) level = "12";
            else if (playLevelUpDown.Value < 13m) level = "12+";
            else if (playLevelUpDown.Value < 13.7m) level = "13";
            else if (playLevelUpDown.Value < 14m) level = "13+";
            else if (playLevelUpDown.Value < 14.7m) level = "14";
            else if (playLevelUpDown.Value < 15m) level = "14+";
            else level = "15";
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
            for (lastScore = form1.MaxScore - 1; form1.Scores2[lastScore].specialNotes.Count == 0; lastScore--) if (lastScore < 1) { lastScore = 0; break; }
            //
            sw.WriteLine("#BPM01:" + BPMUpDown.Value);
            sw.WriteLine("#00008:01");
            char[] numAlpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int BPMNum = 2;//<=35
            string[] BPMArr = new string[3001];
            for (int i = 0; i < BPMArr.Count(); i++) BPMArr[i] = "00";
            int BPMBeatDevide = 8;
            BPMArr[(int)(BPMUpDown.Value * 10)] = "01";
            string[] spLane1 = new string[BPMBeatDevide];
            string[] spLane2 = new string[BPMBeatDevide];
            string cur = "01";
            for (measure = 0; measure <= lastScore; measure++)
            {
                for (int i = 0; i < BPMBeatDevide; i++) spLane1[i] = "00";//initialize lane
                for (int i = 0; i < BPMBeatDevide; i++) spLane2[i] = "00";//initialize lane
                foreach (ShortNote _note in form1.Scores2[measure].specialNotes)
                {
                    if (_note.NoteStyle == "BPM")
                    {
                        Y = BPMBeatDevide - (_note.NotePosition.Y - 2) / (384 / BPMBeatDevide);
                        if (Y < 0)
                        {
                            Y += BPMBeatDevide;
                            if (BPMArr[(int)(_note.SpecialValue * 10)] == "00")
                            {
                                BPMArr[(int)(_note.SpecialValue * 10)] = "0" + numAlpha[BPMNum];
                                sw.WriteLine("#BPM0" + numAlpha[BPMNum] + ":" + _note.SpecialValue);
                                BPMNum++;
                            }
                            spLane1[Y] = BPMArr[(int)(_note.SpecialValue * 10)].ToString().PadLeft(2, '0');
                        }
                        else
                        {
                            if (Y >= BPMBeatDevide) continue;
                            if (BPMArr[(int)(_note.SpecialValue * 10)] == "00")
                            {
                                BPMArr[(int)(_note.SpecialValue * 10)] = "0" + numAlpha[BPMNum];
                                sw.WriteLine("#BPM0" + numAlpha[BPMNum] + ":" + _note.SpecialValue);
                                BPMNum++;
                            }
                            spLane2[Y] = BPMArr[(int)(_note.SpecialValue * 10)];
                        }
                    }
                }
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
                foreach(ShortNote _note in form1.Scores2[measure].specialNotes)
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

            for (lastScore = form1.MaxScore - 1; form1.Scores2[lastScore].shortNotes.Count == 0; lastScore--) if (lastScore < 1) { lastScore = 0; break; }
            string[,] lane1 = new string[16, maxBeatDevide];//lane, beat, odd
            string[,] lane2 = new string[16, maxBeatDevide];//lane, beat, even
            char[] sign = "abcdefghijklmnopqrstuvwxyz".ToCharArray();//ロングレーン用の識別子 最大zまで拡張可能
            int sgnindx = 0;//for LongLane
            bool[,] isUsedLane1 = new bool[lastScore + 1, sign.Length];
            bool[,] isUsedLane2 = new bool[lastScore + 1, sign.Length];
            string[,,,] longLane1 = new string[lastScore + 1, 16, maxBeatDevide, sign[sgnindx]];
            string[,,,] longLane2 = new string[lastScore + 1, 16, maxBeatDevide, sign[sgnindx]];
            int noteSize;
            int _X, _Y;

            //[h,i,j,k]=lane,beat,layer
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane

            for (measure = 0; measure <= lastScore; measure++)
            {
                ////////////////////////////////////////////////↓Tap, ExTap, Flick, HellTap
                for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) lane1[i, j] = "00";//initialize lane
                for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) lane2[i, j] = "00";//initialize lane

                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    _X = (note.NotePosition.X - (leftMargin + 1)) / 10;
                    _Y = maxBeatDevide - (note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    if (_Y < 0)
                    {
                        _Y += maxBeatDevide;
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
                        if (_Y >= maxBeatDevide) continue;
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
                    if (isModified(lane1, i))
                    {
                        if(checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane1[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }
                for (int i = 0; i < 16; i++)//even lane
                {
                    if (isModified(lane2, i))
                    {
                        if (checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane2[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }
                ////////////////////////////////////////////////////////////↓Air
                for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) lane1[i, j] = "00";//initialize lane
                for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) lane2[i, j] = "00";//initialize lane

                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    _X = (note.NotePosition.X - (leftMargin + 1)) / 10;
                    _Y = maxBeatDevide - (note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    if (_Y < 0)
                    {
                        _Y += maxBeatDevide;
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
                        //for (int i = 1; i < noteSize; i++) lane1[_X + i, _Y] = "l0";
                    }
                    else
                    {
                        if (_Y >= maxBeatDevide) continue;
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
                    if (isModified(lane1, i))
                    {
                        if(checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane1[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }
                for (int i = 0; i < 16; i++)//even lane
                {
                    if (isModified(lane2, i))
                    {
                        if(checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane2[i, j]); }
                        sw.Write(Environment.NewLine);
                    }
                }
            }
            /////////////////////////////////////////////////↓Hold               
            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    _X = (note.NotePosition.X - (leftMargin + 1)) / 10;
                    _Y = maxBeatDevide - (note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    if (_Y < 0)
                    {
                        _Y += maxBeatDevide;
                        switch (note.NoteStyle)
                        {
                            case "Hold":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane1[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";
                                else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "HoldEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
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
                        if (_Y >= maxBeatDevide) continue;
                        switch (note.NoteStyle)
                        {
                            case "Hold":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane2[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "HoldEnd" && _note.LongNoteNumber == longNoteNumber)//
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
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
                        if (isModified(longLane1, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (isModified(longLane2, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }
            /////////////////////////////////////////////////↓Slide      
            //[h,i,j,k]=lane,beat,layer
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane

            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    _X = (note.NotePosition.X - (leftMargin + 1)) / 10;
                    _Y = maxBeatDevide - (note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    if (_Y < 0)
                    {
                        _Y += maxBeatDevide;
                        switch (note.NoteStyle)
                        {
                            case "Slide"://この内部ではlane1とlane2で同じコード、ではない
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane1[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";//コピペ後は変更
                                else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");//コピペ後は変更
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure;//, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "SlideRelay" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                /*
                                                if (_Y == maxBeatDevide)
                                                {
                                                    if (noteSize == 16) longLane1[_measure + 1, _X, 0, sgnindx] = "5g";
                                                    else longLane1[_measure + 1, _X, 0, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                else
                                                {
                                                    if (_Y >= maxBeatDevide) continue;
                                                    if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                    else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                //*/
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        //_X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        //_Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        if (_note.NoteStyle == "SlideTap" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideCurve" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                            flg = true;
                                        }
                                    }
                                    if (flg) break;
                                }
                                /*
                                for (int i = form1.Scores2[endMeasure].shortNotes.Count - 1; i >= 0; i--)
                                {
                                    if(form1.Scores2[endMeasure].shortNotes[i].NoteStyle == "SlideTap" && form1.Scores2[endMeasure].shortNotes[i].LongNoteNumber == longNoteNumber)
                                    {
                                        _X = (form1.Scores2[endMeasure].shortNotes[i].NotePosition.X - (leftMargin + 1)) / 10;
                                        _Y = maxBeatDevide - (form1.Scores2[endMeasure].shortNotes[i].NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = form1.Scores2[endMeasure].shortNotes[i].NoteSize;
                                        if (_Y < 0)
                                        {
                                            _Y += maxBeatDevide;
                                            if (noteSize == 16) longLane1[endMeasure, _X, _Y, sgnindx] = "2g";
                                            else longLane1[endMeasure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            for (int j = startMeasure; j <= endMeasure; j++) isUsedLane1[j, sgnindx] = true;
                                        }
                                        else
                                        {
                                            if (_Y >= maxBeatDevide) continue;
                                            if (noteSize == 16) longLane2[endMeasure, _X, _Y, sgnindx] = "2g";
                                            else longLane2[endMeasure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            for (int j = startMeasure; j <= endMeasure; j++) isUsedLane2[j, sgnindx] = true;
                                        }
                                        break;
                                    }
                                }//*/
                                break;//
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (_Y >= maxBeatDevide) continue;
                        switch (note.NoteStyle)
                        {
                            case "Slide"://
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane1[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure;//, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "SlideRelay" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                /*
                                                if (_Y == maxBeatDevide)
                                                {
                                                    if (noteSize == 16) longLane1[_measure + 1, _X, 0, sgnindx] = "5g";
                                                    else longLane1[_measure + 1, _X, 0, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                else
                                                {
                                                    if (_Y >= maxBeatDevide) continue;
                                                    if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                    else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                //*/
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        //_X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        //_Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        if (_note.NoteStyle == "SlideTap" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideCurve" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "4g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "4" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "SlideEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                //endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
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
                        //for (int i = 1; i < noteSize; i++) lane2[_X + i, _Y] = "l0";
                    }
                }
            }
            for (measure = 0; measure <= lastScore; measure++)
            {
                for (int i = 0; i < 16; i++)//レーンを出力
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (isModified(longLane1, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (isModified(longLane2, measure, i, _sgnindx))
                        {
                            if(checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }
            //
            /////////////////////////////////////////////////↓AirLine
            //[h,i,j,k]=lane,beat,layer
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane
            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    _X = (note.NotePosition.X - (leftMargin + 1)) / 10;
                    _Y = maxBeatDevide - (note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    if (_Y < 0)
                    {
                        _Y += maxBeatDevide;
                        switch (note.NoteStyle)
                        {
                            case "AirBegin":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane1[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";
                                else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "AirAction" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "AirEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
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
                        if (_Y >= maxBeatDevide) continue;
                        switch (note.NoteStyle)
                        {
                            case "AirBegin":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane2[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.NotePosition.X - (leftMargin + 1)) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "AirAction" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        if (_note.NoteStyle == "AirEnd" && _note.LongNoteNumber == longNoteNumber)//
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y >= maxBeatDevide) continue;
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
                        if (isModified(longLane1, measure, i, _sgnindx))
                        {
                            if (checkBoxWhile.Checked) sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (isModified(longLane2, measure, i, _sgnindx))
                        {
                            if (checkBoxWhile.Checked) sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            else sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(Environment.NewLine);
                        }
                    }
                }
            }
            //
            sw.Close();
        }

        private bool isModified(string[,] lane, int i)
        {
            for (int j = 0; j < maxBeatDevide; j++) if (lane[i,j] != "00") return true;
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

        private bool isModified(string[,,,] lane, int measure, int i, int sgnindx)
        {
            for (int j = 0; j < maxBeatDevide; j++) if (lane[measure, i, j, sgnindx] != "00") return true;
            return false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            form1.saveExportData(textBoxID.Text, textBoxTitle.Text, textBoxArtist.Text, textBoxDesigner.Text, textBoxWAVE.Text, textBoxJacket.Text, difficultyComboBox.SelectedIndex, playLevelUpDown.Value, BPMUpDown.Value, textBoxExport.Text, offsetUpDown.Value, checkBoxWhile.Checked);
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
