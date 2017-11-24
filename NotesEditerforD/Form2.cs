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
    public partial class Form2 : Form
    {
        private Form1 form1;
        private int measure, lastScore;
        private static int maxBeatDevide = 192;
        public Form2(Form1 _form1)
        {
            InitializeComponent();
            difficultyComboBox.SelectedIndex = 0;
            form1 = _form1;
        }

        public void loadExportData(string _songID, string _title, string _artist, string _designer, string _wave, string _jacket, int _difficulty, decimal _playLevel, decimal _BPM, string _exDir, decimal _offset)
        {
            textBoxID.Text = _songID;
            textBoxTitle.Text = _title;
            textBoxArtist.Text = _artist;
            textBoxDesigner.Text = _designer;
            textBoxWAVE.Text = _wave;
            textBoxJacket.Text = _jacket;
            difficultyComboBox.SelectedIndex = _difficulty;
            playLevelUpDown.Value = (decimal)_playLevel;
            BPMUpDown.Value = (decimal)_BPM;
            textBoxExport.Text = _exDir;
            offsetUpDown.Value = _offset;
        }

        private void export_Click(object sender, EventArgs e)
        {
            form1.saveExportData(textBoxID.Text, textBoxTitle.Text, textBoxArtist.Text, textBoxDesigner.Text, textBoxWAVE.Text, textBoxJacket.Text, difficultyComboBox.SelectedIndex, playLevelUpDown.Value, BPMUpDown.Value, textBoxExport.Text, offsetUpDown.Value);
            if (textBoxExport.Text.Length == 0) MessageBox.Show("保存先を選択してください");
            else if (textBoxTitle.Text.Length == 0) MessageBox.Show("タイトルを入力してください");
            else
            {
                string path = textBoxExport.Text + "\\" + textBoxTitle.Text + ".sus";
                if (System.IO.File.Exists(path))
                {
                    DialogResult result = MessageBox.Show("ファイルを上書きしますか？", "確認", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        susExport(path);
                        this.Dispose();
                    }
                    else { }
                }else
                {
                    susExport(path);
                    this.Dispose();
                }
            }
        }

        public void susExport(string path)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
            Encoding.GetEncoding("UTF-8");
            sw.WriteLine("This file was exported by NotesEditorforD.");
            sw.WriteLine();
            sw.WriteLine("#SONGID " + '"' + textBoxID.Text + '"');
            sw.WriteLine("#TITLE " + '"' + textBoxTitle.Text + '"');
            sw.WriteLine("#ARTIST " + '"' + textBoxArtist.Text + '"');
            sw.WriteLine("#DESIGNER " + '"' + textBoxDesigner.Text + '"');
            sw.WriteLine("#DIFFICULTY " + difficultyComboBox.SelectedIndex);
            sw.WriteLine("#PLAYLEVEL " + playLevelUpDown.Value);
            sw.WriteLine("#WAVE " + '"' + textBoxWAVE.Text + '"');
            sw.WriteLine("#WAVEOFFSET " + offsetUpDown.Value);
            sw.WriteLine("#JACKET " + '"' + textBoxJacket.Text + '"');
            sw.WriteLine("#BASEBPM " + BPMUpDown.Value);
            sw.WriteLine("#BPM01: " + BPMUpDown.Value + ".0");
            sw.WriteLine("#00008:01");

            sw.WriteLine();

            for (lastScore = form1.MaxScore - 1; form1.Scores2[lastScore].shortNotes.Count == 0; lastScore--) if (lastScore < 1) { lastScore = 0; break; }
            string[,] lane1 = new string[16, maxBeatDevide];//lane, beat, odd
            string[,] lane2 = new string[16, maxBeatDevide];//lane, beat, even
            char[] sign = new char[26] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};//ロングレーン用の識別子 最大zまで拡張可能
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
                    _X = (note.NotePosition.X - 6) / 10;
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
                                else lane1[_X, _Y] = "4" + noteSize.ToString("x");
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
                        sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane1[i, j]); }
                        sw.Write(System.Environment.NewLine);
                    }
                }
                for (int i = 0; i < 16; i++)//even lane
                {
                    if (isModified(lane2, i))
                    {
                        sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "1" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane2[i, j]); }
                        sw.Write(System.Environment.NewLine);
                    }
                }
                ////////////////////////////////////////////////////////////↓Air
                for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) lane1[i, j] = "00";//initialize lane
                for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) lane2[i, j] = "00";//initialize lane

                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    _X = (note.NotePosition.X - 6) / 10;
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
                        sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane1[i, j]); }
                        sw.Write(System.Environment.NewLine);
                    }
                }
                for (int i = 0; i < 16; i++)//even lane
                {
                    if (isModified(lane2, i))
                    {
                        sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "5" + i.ToString("X") + ":");
                        for (int j = 0; j < maxBeatDevide; j++) { sw.Write(lane2[i, j]); }
                        sw.Write(System.Environment.NewLine);
                    }
                }
            }
            /////////////////////////////////////////////////↓Hold               
            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    _X = (note.NotePosition.X - 6) / 10;
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
                                        _X = (_note.NotePosition.X - 6) / 10;
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
                                        _X = (_note.NotePosition.X - 6) / 10;
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
                            sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(System.Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (isModified(longLane2, measure, i, _sgnindx))
                        {
                            sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "2" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(System.Environment.NewLine);
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
                    _X = (note.NotePosition.X - 6) / 10;
                    _Y = maxBeatDevide - (note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    if (_Y < 0)
                    {
                        _Y += maxBeatDevide;
                        switch (note.NoteStyle)
                        {
                            case "Slide"://この内部ではlane1とlane2で同じコード
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane1[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";
                                else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.EndPosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.EndPosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "SlideLine" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y == 64)
                                                {
                                                    if (noteSize == 16) longLane1[_measure + 1, _X, 0, sgnindx] = "5g";
                                                    else longLane2[_measure + 1, _X, 0, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                else
                                                {
                                                    if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                    else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        _X = (_note.NotePosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        if (_note.NoteStyle == "SlideTap" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        /*
                                        if (_note.NoteStyle == "SlideEnd" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
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
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                            flg = true;
                                            break;
                                        }
                                        //*/
                                    }
                                    //if (flg) break;
                                }
                                for (int i = form1.Scores2[endMeasure].shortNotes.Count - 1; i >= 0; i--)
                                {
                                    if(form1.Scores2[endMeasure].shortNotes[i].NoteStyle == "SlideTap" && form1.Scores2[endMeasure].shortNotes[i].LongNoteNumber == longNoteNumber)
                                    {
                                        _X = (form1.Scores2[endMeasure].shortNotes[i].NotePosition.X - 6) / 10;
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
                                            if (noteSize == 16) longLane2[endMeasure, _X, _Y, sgnindx] = "2g";
                                            else longLane2[endMeasure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            for (int j = startMeasure; j <= endMeasure; j++) isUsedLane2[j, sgnindx] = true;
                                        }
                                        break;
                                    }
                                }
                                break;//
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (note.NoteStyle)
                        {
                            case "Slide"://
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane2[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.EndPosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.EndPosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "SlideLine" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if(_Y == 64)
                                                {
                                                    if (noteSize == 16) longLane1[_measure + 1, _X, 0, sgnindx] = "5g";
                                                    else longLane2[_measure + 1, _X, 0, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                else
                                                {
                                                    if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                    else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        _X = (_note.NotePosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        if (_note.NoteStyle == "SlideTap" && _note.LongNoteNumber == longNoteNumber)//
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        /*
                                        if (_note.NoteStyle == "SlideEnd" && _note.LongNoteNumber == longNoteNumber)//
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
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                            flg = true;
                                            break;
                                        }
                                        //*/
                                    }
                                    //if (flg) break;
                                }
                                for (int i = form1.Scores2[endMeasure].shortNotes.Count - 1; i >= 0; i--)
                                {
                                    if (form1.Scores2[endMeasure].shortNotes[i].NoteStyle == "SlideTap" && form1.Scores2[endMeasure].shortNotes[i].LongNoteNumber == longNoteNumber)
                                    {
                                        _X = (form1.Scores2[endMeasure].shortNotes[i].NotePosition.X - 6) / 10;
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
                                            if (noteSize == 16) longLane2[endMeasure, _X, _Y, sgnindx] = "2g";
                                            else longLane2[endMeasure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                            for (int j = startMeasure; j <= endMeasure; j++) isUsedLane2[j, sgnindx] = true;
                                        }
                                        break;
                                    }
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
                            sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(System.Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (isModified(longLane2, measure, i, _sgnindx))
                        {
                            sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "3" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(System.Environment.NewLine);
                        }
                    }
                }
            }
            //
            /////////////////////////////////////////////////↓AirLine
            //[h,i,j,k]=lane,beat,layer
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane1[h, i, j, k] = "00";//initialize lane
            for (int h = 0; h < lastScore + 1; h++) for (int i = 0; i < 16; i++) for (int j = 0; j < maxBeatDevide; j++) for (int k = 0; k < sign.Length; k++) longLane2[h, i, j, k] = "00";//initialize lane
            List<int> checkedLongNoteNumber = new List<int>();
            for (measure = 0; measure <= lastScore; measure++)
            {
                foreach (ShortNote note in form1.Scores2[measure].shortNotes)//レーンを設定
                {
                    /*
                    _X = (note.NotePosition.X - 6) / 10;
                    _Y = maxBeatDevide - (note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    //*/
                    _X = (note.StartPosition.X - 6) / 10;
                    _Y = maxBeatDevide - (note.StartPosition.Y - 2) / (384 / maxBeatDevide);
                    noteSize = note.NoteSize;
                    //{
                        switch (note.NoteStyle)
                        {
                            case "AirLine"://この内部ではlane1とlane2で同じコード
                                if (checkedLongNoteNumber.Contains(note.LongNoteNumber)) continue;
                                else checkedLongNoteNumber.Add(note.LongNoteNumber);
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane1[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (-3 * maxBeatDevide <= _Y && _Y < -2 * maxBeatDevide)
                                {
                                    _Y += 3 * maxBeatDevide;
                                    if(longLane1[measure - 1, _X, _Y, sgnindx] == "00")
                                    {
                                        if (noteSize == 16) longLane1[measure - 1, _X, _Y, sgnindx] = "1g";
                                        else longLane1[measure - 1, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                    }
                                }
                                else if (-2 * maxBeatDevide <= _Y && _Y < -1 * maxBeatDevide)
                                {
                                    _Y += 2 * maxBeatDevide;
                                    if (longLane2[measure - 1, _X, _Y, sgnindx] == "00")
                                    {
                                        if (noteSize == 16) longLane2[measure - 1, _X, _Y, sgnindx] = "1g";
                                        else longLane2[measure - 1, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                    }
                                }
                                else if (-1 * maxBeatDevide <= _Y && _Y < 0)
                                {
                                    _Y += maxBeatDevide;
                                    if(longLane1[measure, _X, _Y, sgnindx] == "00")
                                    {
                                        if (noteSize == 16) longLane1[measure, _X, _Y, sgnindx] = "1g";
                                        else longLane1[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                    }
                                }
                                else
                                {
                                    if(longLane2[measure, _X, _Y, sgnindx] == "00")
                                    {
                                        if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                        else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                    } 
                                }    
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.EndPosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.EndPosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "AirLine" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y == 64)
                                                {
                                                    if (noteSize == 16) longLane1[_measure + 1, _X, 0, sgnindx] = "5g";
                                                    else longLane2[_measure + 1, _X, 0, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                else
                                                {
                                                    if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                    else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        _X = (_note.NotePosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        if (_note.NoteStyle == "AirAction" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        /*
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
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                            flg = true;
                                            break;
                                        }
                                        //*/
                                        
                                    }
                                    //if (flg) break;
                                }
                            for (int i = form1.Scores2[endMeasure].shortNotes.Count - 1; i >= 0; i--)
                            {
                                if (form1.Scores2[endMeasure].shortNotes[i].NoteStyle == "AirAction" && form1.Scores2[endMeasure].shortNotes[i].LongNoteNumber == longNoteNumber)
                                {
                                    _X = (form1.Scores2[endMeasure].shortNotes[i].NotePosition.X - 6) / 10;
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
                                        if (noteSize == 16) longLane2[endMeasure, _X, _Y, sgnindx] = "2g";
                                        else longLane2[endMeasure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                        for (int j = startMeasure; j <= endMeasure; j++) isUsedLane2[j, sgnindx] = true;
                                    }
                                    break;
                                }
                            }
                            break;//ここまで同じ
                            default:
                                break;
                        }
                    //}
                    /*
                    else
                    {
                        switch (note.NoteStyle)
                        {
                            case "AirLine":
                                sgnindx++; if (sgnindx > 25) sgnindx = 0;
                                //sgnindx = 0;
                                //if (isUsedLane2[measure, sgnindx] && sgnindx < 25) { sgnindx++; continue; }
                                if (noteSize == 16) longLane2[measure, _X, _Y, sgnindx] = "1g";
                                else longLane2[measure, _X, _Y, sgnindx] = "1" + noteSize.ToString("x");
                                int longNoteNumber = note.LongNoteNumber;
                                int startMeasure = measure, endMeasure = measure;
                                bool flg = false;
                                for (int _measure = measure; _measure < lastScore + 1; _measure++)
                                {
                                    foreach (ShortNote _note in form1.Scores2[_measure].shortNotes)
                                    {
                                        _X = (_note.EndPosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.EndPosition.Y - 2) / (384 / maxBeatDevide);
                                        noteSize = _note.NoteSize;
                                        if (_note.NoteStyle == "AirLine" && _note.LongNoteNumber == longNoteNumber)//lane1とlane2で同じ
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "5g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (_Y == 64)
                                                {
                                                    if (noteSize == 16) longLane1[_measure + 1, _X, 0, sgnindx] = "5g";
                                                    else longLane2[_measure + 1, _X, 0, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                else
                                                {
                                                    if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "5g";
                                                    else longLane2[_measure, _X, _Y, sgnindx] = "5" + noteSize.ToString("x");
                                                }
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        _X = (_note.NotePosition.X - 6) / 10;
                                        _Y = maxBeatDevide - (_note.NotePosition.Y - 2) / (384 / maxBeatDevide);
                                        if (_note.NoteStyle == "AirAction" && _note.LongNoteNumber == longNoteNumber)//
                                        {
                                            if (_Y < 0)
                                            {
                                                _Y += maxBeatDevide;
                                                if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane1[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                            }
                                            else
                                            {
                                                if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "3g";
                                                else longLane2[_measure, _X, _Y, sgnindx] = "3" + noteSize.ToString("x");
                                                endMeasure = _measure;
                                                //for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                            }
                                        }
                                        //
                                        //if (_note.NoteStyle == "AirEnd" && _note.LongNoteNumber == longNoteNumber)//
                                        //{
                                        //    if (_Y < 0)
                                        //    {
                                        //        _Y += maxBeatDevide;
                                        //        if (noteSize == 16) longLane1[_measure, _X, _Y, sgnindx] = "2g";
                                        //        else longLane1[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                        //        endMeasure = _measure;
                                        //        for (int i = startMeasure; i <= endMeasure; i++) isUsedLane1[i, sgnindx] = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (noteSize == 16) longLane2[_measure, _X, _Y, sgnindx] = "2g";
                                        //        else longLane2[_measure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                        //        endMeasure = _measure;
                                        //        for (int i = startMeasure; i <= endMeasure; i++) isUsedLane2[i, sgnindx] = true;
                                        //    }
                                        //    flg = true;
                                        //    break;
                                        //}
                                        //
                                        for (int i = form1.Scores2[endMeasure].shortNotes.Count - 1; i >= 0; i--)
                                        {
                                            if (form1.Scores2[endMeasure].shortNotes[i].NoteStyle == "AirAction" && form1.Scores2[endMeasure].shortNotes[i].LongNoteNumber == longNoteNumber)
                                            {
                                                _X = (form1.Scores2[endMeasure].shortNotes[i].NotePosition.X - 6) / 10;
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
                                                    if (noteSize == 16) longLane2[endMeasure, _X, _Y, sgnindx] = "2g";
                                                    else longLane2[endMeasure, _X, _Y, sgnindx] = "2" + noteSize.ToString("x");
                                                    for (int j = startMeasure; j <= endMeasure; j++) isUsedLane2[j, sgnindx] = true;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    //if (flg) break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    //*/
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
                            sw.Write("#" + (2 * measure + 1).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane1[measure, i, j, _sgnindx]); }
                            sw.Write(System.Environment.NewLine);
                        }
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int _sgnindx = 0; _sgnindx < sign.Length; _sgnindx++)
                    {
                        if (isModified(longLane2, measure, i, _sgnindx))
                        {
                            sw.Write("#" + (2 * (measure + 1)).ToString().PadLeft(3, '0') + "4" + i.ToString("X") + sign[_sgnindx] + ":");
                            for (int j = 0; j < maxBeatDevide; j++) { sw.Write(longLane2[measure, i, j, _sgnindx]); }
                            sw.Write(System.Environment.NewLine);
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

        private bool isModified(string[,,,] lane, int measure, int i, int sgnindx)
        {
            for (int j = 0; j < maxBeatDevide; j++) if (lane[measure, i, j, sgnindx] != "00") return true;
            return false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            form1.saveExportData(textBoxID.Text, textBoxTitle.Text, textBoxArtist.Text, textBoxDesigner.Text, textBoxWAVE.Text, textBoxJacket.Text, difficultyComboBox.SelectedIndex, playLevelUpDown.Value, BPMUpDown.Value, textBoxExport.Text, offsetUpDown.Value);
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
            }
        }

        private void buttonJacket_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "default";
            ofd.Filter = "画像ファイル(*.bmp;*.jpg;*.png)|*.bmp;*,jpg;*.png";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)//OKボタンがクリックされた時
            {
                textBoxJacket.Text = ofd.SafeFileName;
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
