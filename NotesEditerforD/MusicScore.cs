﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NotesEditerforD
{
    /// <summary>
    /// 譜面クラス
    /// </summary>
    public class MusicScore : PictureBox
    {
        private static int selectedBeat, selectedGrid, selectedNoteSize, tmpLongNoteNumber;
        private static int topMargin = 5, bottomMargin = 5, leftMargin = 20, rightMargin = 30;
        private int index;
        private static string selectedNoteStyle, selectedEditStatus, selectedAirDirection;
        private static decimal selectedBPM, selectedSpeed;
        private Point startPosition, endPosition;
        private bool addSlideRelayFlag, previewVisible, rectSelectFlag, seRectFlag, hitLeftFlag, hitRightFlag;
        private Point rectSelectBegin, rectSelectEnd;
        private RectSelect seRect;
        public ScoreRoot sRoot;
        public List<ShortNote> shortNotes = new List<ShortNote>();
        public List<ShortNote> specialNotes = new List<ShortNote>();
        private Bitmap storeImage;
        private ShortNote previewNote, previewLongNote, startNote, selectedNote, selectedNote_prev, selectedNote_next;
        private MusicScore prevScore, nextScore;
        private MouseButtons eyedropperMouseButton = MouseButtons.Right;
        private ContextMenuStrip selectMenuStrip;//, scoreMenuStrip;
        private ToolStripMenuItem[] stripItem;
        private StringFormat sFormat = new StringFormat();

        public MusicScore()
        {
            selectedBeat = 8;
            selectedGrid = 8;
            selectedNoteSize = 4;
            selectedEditStatus = "Add";
            selectedAirDirection = "Center";
            Location = new Point(0, 0);
            Margin = new Padding(10, 7, 10, 7);
            Size = new Size(160 + leftMargin + rightMargin, 768 + topMargin + bottomMargin);
            BackgroundImage = Properties.Resources.MusicScore;
            storeImage = Properties.Resources.MusicScore;
            previewNote = null;
            previewLongNote = null;
            prevScore = null;
            nextScore = null;
            addSlideRelayFlag = false;
            previewVisible = true;
            selectMenuStrip = new ContextMenuStrip();
            stripItem = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("切り取り（未実装）", null, new EventHandler(menuStripCut)),
                new ToolStripMenuItem("コピー", null, new EventHandler(menuStripCopy)),
                new ToolStripMenuItem("貼り付け", null, new EventHandler(menuStripPaste)),
                new ToolStripMenuItem("削除", null, new EventHandler(menuStripRemove)),
                new ToolStripMenuItem("左右反転", null, new EventHandler(menuStripHorInv))
            };
            selectMenuStrip.Items.AddRange(stripItem);

            this.MouseDown += new MouseEventHandler(this_MouseDown);
            this.MouseMove += new MouseEventHandler(this_MouseMove);
            this.MouseUp += new MouseEventHandler(this_MouseUp);
            this.MouseEnter += new EventHandler(this_MouseEnter);
            this.MouseLeave += new EventHandler(this_MouseLeave);

            sFormat.Alignment = StringAlignment.Far;
        }

        public MusicScore PrevScore
        {
            get { return this.prevScore; }
            set { this.prevScore = value; }
        }

        public MusicScore NextScore
        {
            get { return this.nextScore; }
            set { this.nextScore = value; }
        }

        public static int SelectedBeat
        {
            get { return selectedBeat; }
            set { selectedBeat = value; }
        }

        public static int SelectedGrid
        {
            get { return selectedGrid; }
            set { selectedGrid = value; }
        }

        public static int SelectedNoteSize//1-16
        {
            get { return selectedNoteSize; }
            set { selectedNoteSize = value; }
        }

        public static string SelectedNoteStyle
        {
            get { return selectedNoteStyle; }
            set { selectedNoteStyle = value; }
        }

        public static string SelectedAirDirection
        {
            get { return selectedAirDirection; }
            set { selectedAirDirection = value; }
        }

        public static string SelectedEditStatus
        {
            get { return selectedEditStatus; }
            set { selectedEditStatus = value; }
        }

        public static decimal SelectedBPM
        {
            get { return selectedBPM; }
            set { selectedBPM = value; }
        }

        public static decimal SelectedSpeed
        {
            get { return selectedSpeed; }
            set { selectedSpeed = value; }
        }

        public static int TopMargin
        {
            get { return topMargin; }
        }

        public static int BottomMargin
        {
            get { return bottomMargin; }
        }

        public static int LeftMargin
        {
            get { return leftMargin; }
        }

        public static int RightMargin
        {
            get { return rightMargin; }
        }

        public Bitmap StoreImage
        {
            get { return storeImage; }
            set { storeImage = value; }
        }

        private void menuStripCut(object sender, EventArgs e)
        {
            //menuStripCopy(sender, e);
            //menuStripRemove(sender, e);
        }

        private void menuStripCopy(object sender, EventArgs e)//sRootに渡す
        {
            sRoot.YankNotes.Clear();
            foreach(ShortNote note in shortNotes)//矩形内に含まれてそうなショートノーツをコピーしてYankのリストにぶち込む
            {
                if(seRect.Rect.Contains(new Rectangle(note.DestPoints[0], new Size(note.NoteSize * 10 - 2, 5))) &&
            note.NoteStyle != "HoldLine" && note.NoteStyle != "SlideLine" && note.NoteStyle != "AirLine")
                {
                    sRoot.YankNotes.Add(
                         new ShortNote(this, note.NotePosition, note.NoteSize, note.NoteStyle, note.AirDirection, note.LongNoteNumber, note.LocalPosition.Beat));
                }
            }
            foreach(ShortNote note in sRoot.YankNotes.ToArray())//ロングノーツ系のやつは全体が含まれてるかチェック
            {
                if (note.LongNoteNumber == -1) continue;
                if(shortNotes.Where
                    (x => x.LongNoteNumber == note.LongNoteNumber && !(new string[] { "HoldLine", "SlideLine", "AirLine"}.Contains(x.NoteStyle))).Count()
                    != sRoot.YankNotes.Where(x => x.LongNoteNumber == note.LongNoteNumber).Count())//1譜面内でのロングノーツでの節が全て網羅されているか
                {
                    sRoot.YankNotes.RemoveAll(x => x.LongNoteNumber == note.LongNoteNumber);
                }
                else if (false) ;//始点と終点を共に含んでいるか
            }
            //Line系もYankNotesに入れる処理書け
            foreach(ShortNote note in shortNotes)//書きました
            {
                if (note.LongNoteNumber == -1) continue;
                if (!(new string[] { "HoldLine", "SlideLine", "AirLine" }.Contains(note.NoteStyle))) continue;
                if (!sRoot.YankNotes.Where(x => x.LongNoteNumber == note.LongNoteNumber).Any()) continue;
                if(note.NoteStyle == "SlideLine")
                {
                    sRoot.YankNotes.Insert(0,
                    new ShortNote(this, note.NotePosition, note.StartPosition, note.EndPosition, note.StartSize, note.EndSize, note.LongNoteNumber));
                }
                else
                    sRoot.YankNotes.Insert(0,
                    new ShortNote(this, note.NotePosition, note.StartPosition, note.EndPosition, note.NoteSize, note.NoteStyle, note.AirDirection, note.LongNoteNumber));
            }
            sRoot.YankRect = new RectSelect(seRect.RectUL, seRect.RectDR, shortNotes);
        }

        private void menuStripPaste(object sender, EventArgs e)//sRootからもらう//詰める//詰めた
        {
            List<ShortNote> selectedNotes = new List<ShortNote>();
            var objLNN = new[] { new { prevLNN = -1, nextLNN = -1 } }.Skip(1).ToList();
            
            foreach(ShortNote note in sRoot.YankNotes)
            {
                if(new string[] { "HoldLine", "AirLine" }.Contains(note.NoteStyle))
                {
                    selectedNotes.Add(
                         new ShortNote(this, note.NotePosition, note.StartPosition, note.EndPosition, note.NoteSize, note.NoteStyle, note.AirDirection, note.LongNoteNumber));
                }
                else if(note.NoteStyle == "SlideLine")
                {
                    selectedNotes.Add(
                         new ShortNote(this, note.NotePosition, note.StartPosition, note.EndPosition, note.StartSize, note.EndSize, note.LongNoteNumber));
                }
                else
                {
                    selectedNotes.Add(
                         new ShortNote(this, note.NotePosition, note.NoteSize, note.NoteStyle, note.AirDirection, note.LongNoteNumber, note.LocalPosition.Beat));
                }
            }
            //LongNotesNumber更新しろ
            foreach(ShortNote note in selectedNotes)//しました
            {
                if (note.LongNoteNumber == -1) continue;
                if(objLNN.Where(x => x.prevLNN == note.LongNoteNumber).Any())
                {
                    note.LongNoteNumber = objLNN.Find(x => x.prevLNN == note.LongNoteNumber).nextLNN;
                }
                else
                {
                    objLNN.Add(new { prevLNN = note.LongNoteNumber, nextLNN = sRoot.LongNoteNumber });
                    note.LongNoteNumber = sRoot.LongNoteNumber;
                    sRoot.LongNoteNumber++;
                }
            }
            shortNotes.AddRange(selectedNotes);
            seRect = new RectSelect(sRoot.YankRect.RectUL, sRoot.YankRect.RectDR, shortNotes);
            seRect.SelectedNotes = selectedNotes;
            seRect.move(new Point(21, 2));
            seRect.IsPasted = true;
        }

        private void menuStripRemove(object sender, EventArgs e)//ロングノーツの消し方を真面目にやれ//やった
        {
            if (seRect.IsPasted)
            {
                foreach(ShortNote note in shortNotes.ToArray())
                {
                    if (seRect.SelectedNotes.Contains(note)) deleteforLongNote(note); 
                }
                return;
            }
            List<ShortNote> selectedNotes = new List<ShortNote>();
            foreach (ShortNote note in shortNotes)//矩形内に含まれてそうなショートノーツそのものをSelectedNotesのリストに（も）ぶち込む
            {
                if (seRect.Rect.Contains(new Rectangle(note.DestPoints[0], new Size(note.NoteSize * 10, 5))) &&
            note.NoteStyle != "HoldLine" && note.NoteStyle != "SlideLine" && note.NoteStyle != "AirLine")
                {
                    selectedNotes.Add(note);
                }
            }
            foreach (ShortNote note in selectedNotes.ToArray())
            {
                deleteforLongNote(note);
            }
        }

        private void menuStripHorInv(object sender, EventArgs e)
        {
            seRect.notesHorInv();
        }

        public void setNote(string[] _noteData, string dymsVersion)//dymsVer変更時に必ず編集
        {
            Point notePosition;
            int noteSize, startSize, endSize, longNoteNumber, beat;
            string noteStyle, airDirection;
            ShortNote shortNote;
            if (dymsVersion == "0.7")
            {
                notePosition = new Point(int.Parse(_noteData[4]), int.Parse(_noteData[5]));
                startPosition = new Point(int.Parse(_noteData[6]), int.Parse(_noteData[7]));
                endPosition = new Point(int.Parse(_noteData[8]), int.Parse(_noteData[9]));
                noteSize = int.Parse(_noteData[1]);
                startSize = int.Parse(_noteData[2]);
                endSize = int.Parse(_noteData[3]);
                noteStyle = _noteData[0];
                airDirection = _noteData[10];
                longNoteNumber = int.Parse(_noteData[11]);
                beat = int.Parse(_noteData[13]);
                if (new string[] { "HoldLine", "AirLine" }.Contains(noteStyle))
                    shortNote = new ShortNote(this, notePosition, startPosition, endPosition, noteSize, noteStyle, airDirection, longNoteNumber);
                else if (noteStyle == "SlideLine")
                    shortNote = new ShortNote(this, notePosition, startPosition, endPosition, startSize, endSize, longNoteNumber);
                else
                    shortNote = new ShortNote(this, notePosition, noteSize, noteStyle, airDirection, longNoteNumber, beat);
                shortNotes.Add(shortNote);
                return;
            }
            else if (dymsVersion == "0.6")
            {
                notePosition = new Point(int.Parse(_noteData[2]), int.Parse(_noteData[3]));
                startPosition = new Point(int.Parse(_noteData[4]), int.Parse(_noteData[5]));
                endPosition = new Point(int.Parse(_noteData[6]), int.Parse(_noteData[7]));
                noteSize = int.Parse(_noteData[1]);
                noteStyle = _noteData[0];
                airDirection = _noteData[8];
                longNoteNumber = int.Parse(_noteData[9]);
                beat = int.Parse(_noteData[11]);
                if (new string[] { "HoldLine", "AirLine" }.Contains(noteStyle))
                    shortNote = new ShortNote(this, notePosition, startPosition, endPosition, noteSize, noteStyle, airDirection, longNoteNumber);
                else if (noteStyle == "SlideLine")
                    shortNote = new ShortNote(this, notePosition, startPosition, endPosition, noteSize, noteSize, longNoteNumber);
                else
                    shortNote = new ShortNote(this, notePosition, noteSize, noteStyle, airDirection, longNoteNumber, beat);
                shortNotes.Add(shortNote);
                return;
            }
            if (dymsVersion == "0.3" || dymsVersion == "0.4" || dymsVersion == "0.5")
            {
                notePosition = new Point(int.Parse(_noteData[2]), int.Parse(_noteData[3]));
                startPosition = new Point(int.Parse(_noteData[4]), int.Parse(_noteData[5]));
                endPosition = new Point(int.Parse(_noteData[6]), int.Parse(_noteData[7]));
                noteSize = int.Parse(_noteData[1]);
                noteStyle = _noteData[0];
                airDirection = _noteData[8];
                longNoteNumber = int.Parse(_noteData[9]);
                if(dymsVersion == "0.5")
                {
                    if (new string[] { "HoldLine", "SlideLine", "AirLine" }.Contains(noteStyle))
                        shortNote = new ShortNote(this, notePosition, startPosition, endPosition, noteSize, noteStyle, airDirection, longNoteNumber);
                    else if (noteStyle == "SlideLine")
                        shortNote = new ShortNote(this, notePosition, startPosition, endPosition, noteSize, noteSize, longNoteNumber);
                    else
                        shortNote = new ShortNote(this, notePosition, noteSize, noteStyle, airDirection, longNoteNumber, 192);
                    shortNotes.Add(shortNote);
                    return;
                }
            }
            else if (dymsVersion != "0.1")
            {
                notePosition = new Point(int.Parse(_noteData[2]) + leftMargin - 5, int.Parse(_noteData[3]));
                startPosition = new Point(int.Parse(_noteData[4]) + leftMargin - 5, int.Parse(_noteData[5]));
                endPosition = new Point(int.Parse(_noteData[6]) + leftMargin - 5, int.Parse(_noteData[7]));
                noteSize = int.Parse(_noteData[1]);
                noteStyle = _noteData[0];
                airDirection = _noteData[8];
                longNoteNumber = int.Parse(_noteData[9]);
            }
            else
            {
                notePosition = new Point(int.Parse(_noteData[3]) + leftMargin - 5, int.Parse(_noteData[4]));
                startPosition = new Point(int.Parse(_noteData[5]) + leftMargin - 5, int.Parse(_noteData[6]));
                endPosition = new Point(int.Parse(_noteData[7]) + leftMargin - 5, int.Parse(_noteData[8]));
                noteSize = int.Parse(_noteData[1]) / 10;
                noteStyle = _noteData[0];
                airDirection = _noteData[9];
                longNoteNumber = int.Parse(_noteData[10]);
                if (noteStyle == "AirLine") noteSize = int.Parse(_noteData[11]);
                if (noteStyle == "AirAction") { noteSize += 1; notePosition.X -= 2; }
                if (noteStyle == "AirUp" || noteStyle == "AirDown") notePosition.Y += 32;
            }
            shortNote = new ShortNote(this, notePosition, startPosition, endPosition, noteSize, noteStyle, airDirection, longNoteNumber);
            //shortNote.setRelativePosition();
            addNote(shortNote);
        }

        public void setSpecialNote(string[] _noteData)
        {
            Point notePosition;
            decimal specialValue;
            string noteStyle;
            notePosition = new Point(int.Parse(_noteData[1]), int.Parse(_noteData[2]));
            noteStyle = _noteData[0];
            specialValue = decimal.Parse(_noteData[3]);
            ShortNote shortNote = new ShortNote(this, notePosition, noteStyle, specialValue);
            //shortNote.setRelativePosition();
            specialNotes.Add(shortNote);
        }

        private void this_MouseDown(object sender, MouseEventArgs e)
        {
            if (selectedEditStatus == "Add")
            {
                bool deleteFlag = false;//ロングノーツ継ぎ足しの時trueにする
                if (e.Button == eyedropperMouseButton)//スポイト機能
                {
                    foreach (ShortNote note in shortNotes.Reverse<ShortNote>())
                    {
                        if (note.NoteStyle == "SlideLine" && isMouseCollisionSquare(note.DestPoints, e.Location) && note.Visible)
                        {
                            selectedNoteStyle = "Slide";
                            selectedNoteSize = note.StartSize;
                            break;
                        }
                        else if (note.NoteStyle != "SlideLine" && isMouseCollision(note.DestPoints, e.Location))
                        {
                            if (note.NoteStyle == "SlideLine" || note.NoteStyle == "SlideTap" || note.NoteStyle == "SlideRelay" || note.NoteStyle == "SlideEnd") selectedNoteStyle = "Slide";
                            else if (note.NoteStyle == "HoldLine" || note.NoteStyle == "HoldEnd") selectedNoteStyle = "Hold";
                            else if (note.NoteStyle == "AirAction" || note.NoteStyle == "AirBegin" || note.NoteStyle == "AirEnd") selectedNoteStyle = "AirLine";
                            else selectedNoteStyle = note.NoteStyle;
                            if (note.NoteStyle == "SlideCurve") selectedNoteSize = 4;
                            else selectedNoteSize = note.NoteSize;
                            selectedAirDirection = note.AirDirection;
                            break;
                        }
                    }
                    return;
                }
                ShortNote shortNote;
                startPosition = locationize(e.Location);
                if (selectedNoteStyle == "BPM")
                {
                    shortNote = new ShortNote(this, locationize(e.Location), selectedNoteStyle, selectedBPM);
                    if (shortNote.NotePosition.Y != 2 && ((772 - shortNote.NotePosition.Y) - 2) % 48 == 0) specialNotes.Add(shortNote);
                    update();
                    return;
                }
                if (selectedNoteStyle == "Speed")
                {
                    shortNote = new ShortNote(this, locationize(e.Location), selectedNoteStyle, selectedSpeed);
                    if (shortNote.NotePosition.Y != 2) specialNotes.Add(shortNote);
                    update();
                    return;
                }
                shortNote = new ShortNote(this, locationize(e.Location), startPosition, startPosition, selectedNoteSize, selectedNoteStyle, selectedAirDirection, -1);
                //addNote(shortNote);
                startNote = shortNote;
                if (selectedNoteStyle == "Hold")
                {
                    shortNote.LongNoteNumber = sRoot.LongNoteNumber;
                    tmpLongNoteNumber = shortNote.LongNoteNumber;
                    foreach (ShortNote _note in shortNotes.Reverse<ShortNote>())
                    {
                        if (_note.NotePosition == locationize(e.Location) && _note.NoteSize == selectedNoteSize && _note.NoteStyle == "HoldEnd")//末尾に継ぎ足し
                        {
                            selectedNote = new ShortNote(this, _note.NotePosition, _note.StartPosition, _note.EndPosition, _note.NoteSize, _note.NoteStyle, _note.AirDirection, _note.LongNoteNumber);//MouseUp内で使う
                            tmpLongNoteNumber = _note.LongNoteNumber;
                            deleteFlag = true;//deleteNote(startNote);
                            deleteNote(_note);
                            break;
                        }
                    }
                    previewLongNote = new ShortNote(this, locationize(e.Location), startPosition, new Point(startPosition.X, startPosition.Y - 1), selectedNoteSize, "HoldLine", selectedAirDirection, 0);
                }
                else if (selectedNoteStyle == "AirLine")
                {
                    startNote.NoteStyle = "AirBegin";
                    startNote.update();
                    shortNote.LongNoteNumber = sRoot.LongNoteNumber;
                    tmpLongNoteNumber = shortNote.LongNoteNumber;
                    foreach (ShortNote _note in shortNotes.Reverse<ShortNote>())
                    {
                        if (_note.NotePosition == locationize(e.Location) && _note.NoteSize == selectedNoteSize && _note.NoteStyle == "AirEnd")//末尾に継ぎ足し
                        {
                            deleteFlag = true;//deleteNote(startNote);
                            tmpLongNoteNumber = _note.LongNoteNumber;
                            _note.NoteStyle = "AirAction";
                            _note.update();
                            selectedNote = _note;//MouseUpで使う
                            if (_note.PrevNote != null)
                            {
                                _note.PrevNote.NoteStyle = "AirAction";
                                _note.PrevNote.update();
                                prevScore.update();
                            }
                            break;
                        }

                        if (_note.NoteStyle == "AirLine" && _note.NoteSize == selectedNoteSize && isMouseCollision(_note.DestPoints, e.Location))//間に節を追加
                        {
                            Point newPos = new Point(_note.EndPosition.X, locationize(e.Location).Y);
                            ShortNote airAction = new ShortNote(this, newPos, newPos, newPos, _note.NoteSize, "AirAction", "Center", _note.LongNoteNumber);
                            shortNotes.Insert(shortNotes.IndexOf(_note) + 1, airAction);
                            ShortNote next = _note;
                            ShortNote prev = new ShortNote(this, next.NotePosition, next.StartPosition, newPos, next.NoteSize, "AirLine", "Center", next.LongNoteNumber);
                            next.StartPosition = newPos;
                            shortNotes.Insert(shortNotes.IndexOf(_note), prev);
                            next.update();
                            addSlideRelayFlag = true;
                            break;
                        }
                    }
                    previewLongNote = new ShortNote(this, locationize(e.Location), startPosition, new Point(startPosition.X, startPosition.Y - 1), selectedNoteSize, "AirLine", selectedAirDirection, 0);
                }
                else if (selectedNoteStyle == "Slide")
                {
                    shortNote.LongNoteNumber = sRoot.LongNoteNumber;
                    tmpLongNoteNumber = shortNote.LongNoteNumber;
                    for (int i = shortNotes.Count - 1; i >= 0; i--)
                    {
                        if (shortNotes[i].NotePosition == locationize(e.Location) && shortNotes[i].NoteSize == selectedNoteSize && shortNotes[i].NoteStyle == "SlideEnd")//末尾から継ぎ足し
                        {
                            tmpLongNoteNumber = shortNotes[i].LongNoteNumber;
                            deleteFlag = true;//deleteNote(startNote);
                            if (sRoot.SlideRelay) shortNotes[i].NoteStyle = "SlideTap";
                            else shortNotes[i].NoteStyle = "SlideRelay";
                            shortNotes[i].update();
                            if (shortNotes[i].PrevNote != null)
                            {
                                shortNotes[i].PrevNote.NoteStyle = shortNotes[i].NoteStyle;
                                shortNotes[i].PrevNote.update();
                                prevScore.update();
                            }
                            selectedNote = shortNotes[i];//MouseUp内で使う
                            break;
                        }

                        if (shortNotes[i].NoteStyle == "SlideLine" && isMouseCollisionSquare(shortNotes[i].DestPoints, e.Location) && shortNotes[i].Visible)//間に節を追加
                        {
                            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) break;
                            deleteFlag = true;//deleteNote(startNote);
                            previewVisible = false;
                            ShortNote next = shortNotes[i];
                            ShortNote prev = new ShortNote(this, next.NotePosition, next.StartPosition, locationize(e.Location), next.StartSize, selectedNoteSize, next.LongNoteNumber);
                            next.StartPosition = locationize(e.Location);
                            next.StartSize = selectedNoteSize;
                            shortNotes.Insert(i, prev);
                            next.update();
                            ShortNote slide;
                            if (!sRoot.SlideRelay)
                            {
                                slide = new ShortNote(this, locationize(e.Location), locationize(e.Location), locationize(e.Location), next.StartSize, "SlideRelay", "Center", next.LongNoteNumber);
                            }
                            else
                            {
                                slide = new ShortNote(this, locationize(e.Location), locationize(e.Location), locationize(e.Location), next.StartSize, "SlideTap", "Center", next.LongNoteNumber);
                            }
                            //addNote(slide);
                            shortNotes.Insert(i + 1, slide);
                            selectedNote = slide;
                            selectedNote_next = next;
                            selectedNote_prev = prev;
                            //previewNote = null; previewLongNote = null;
                            addSlideRelayFlag = true;
                            break;
                        }
                    }
                    if (!addSlideRelayFlag) previewLongNote = new ShortNote(this, locationize(e.Location), startPosition, new Point(startPosition.X, startPosition.Y - 1), selectedNoteSize, selectedNoteSize, 0);
                }
                else if (selectedNoteStyle == "SlideCurve")
                {
                    deleteFlag = true;//deleteNote(shortNote);
                    foreach (ShortNote note in shortNotes.Reverse<ShortNote>())
                    {
                        if (note.NoteStyle == "SlideLine" && isMouseCollisionSquare(note.DestPoints, e.Location) && note.Visible)
                        {
                            //MessageBox.Show(note.DestPoints.Count().ToString());
                            shortNote = new ShortNote(this, locationize(e.Location), locationize(e.Location), locationize(e.Location), note.StartSize, "SlideCurve", "Center", note.LongNoteNumber);
                            addNote(shortNote);
                            selectedNote = shortNote;
                            addSlideRelayFlag = true;
                            previewVisible = false;
                            break;
                        }
                    }
                }
                if(!deleteFlag) addNote(shortNote);
                update();
                //shortNote.showRerativePosition();//テスト用
                //MessageBox.Show(locationize(e.Location).ToString());
            }
            else if (selectedEditStatus == "Delete")
            {
                bool flg = false;
                foreach (ShortNote _note in shortNotes.Reverse<ShortNote>())//普通のノーツ
                {
                    if (isMouseCollision(_note.DestPoints, e.Location))
                    {
                        //System.Diagnostics.Debug.WriteLine(_note.NoteStyle + " " + _note.NotePosition + " " + _note.EndPosition);
                        if (_note.NoteStyle == "HoldLine" || _note.NoteStyle == "SlideLine" || _note.NoteStyle == "AirLine") break;
                        else if (_note.NoteStyle == "SlideTap" || _note.NoteStyle == "SlideRelay")
                        {
                            ShortNote prev = null, next = null;
                            foreach (ShortNote _prev in shortNotes)
                            {
                                if (_prev.NoteStyle == "SlideLine" && _prev.LongNoteNumber == _note.LongNoteNumber && _prev.EndPosition == _note.NotePosition)
                                {
                                    prev = _prev;
                                    break;
                                }
                            }
                            foreach (ShortNote _next in shortNotes)
                            {
                                if (_next.NoteStyle == "SlideLine" && _next.LongNoteNumber == _note.LongNoteNumber && _next.StartPosition == _note.NotePosition)
                                {
                                    next = _next;
                                    break;
                                }
                            }
                            if (next != null)
                            {
                                prev.EndPosition = next.EndPosition;
                                prev.EndSize = next.EndSize;
                                prev.update();
                                deleteNote(next);
                                deleteNote(_note);
                            }
                        }
                        else if (_note.NoteStyle == "AirAction")
                        {
                            ShortNote prev = null, next = null;
                            foreach (ShortNote _prev in shortNotes)
                            {
                                if (_prev.NoteStyle == "AirLine" && _prev.LongNoteNumber == _note.LongNoteNumber && _prev.EndPosition == _note.NotePosition)
                                {
                                    prev = _prev;
                                    break;
                                }
                            }
                            foreach (ShortNote _next in shortNotes)
                            {
                                if (_next.NoteStyle == "AirLine" && _next.LongNoteNumber == _note.LongNoteNumber && _next.StartPosition == _note.NotePosition)
                                {
                                    next = _next;
                                    break;
                                }
                            }
                            if (next != null)
                            {
                                prev.EndPosition = next.EndPosition;
                                prev.update();
                                deleteNote(next);
                                deleteNote(_note);
                            }
                        }
                        else if (_note.NoteStyle == "Slide" || _note.NoteStyle == "Hold" || _note.NoteStyle == "AirBegin")
                        {
                            int number = _note.LongNoteNumber;
                            if (number == -1) { deleteNote(_note); return; }
                            string style;
                            if (_note.NoteStyle == "Slide") style = "SlideEnd";
                            else if (_note.NoteStyle == "Hold") style = "HoldEnd";
                            else style = "AirEnd";
                            bool isOver = false;
                            for (MusicScore score = this; score != null; score = score.nextScore)
                            {
                                foreach (ShortNote __note in score.shortNotes.ToArray())
                                {
                                    if (__note.LongNoteNumber == number && __note.NoteStyle == style) isOver = true;
                                    if (__note.LongNoteNumber == number) score.deleteNote(__note);
                                }
                                if (isOver) break; //{ MessageBox.Show("break!"); break; }
                            }
                            break;
                        }
                        else if (_note.NoteStyle == "SlideEnd" || _note.NoteStyle == "HoldEnd" || _note.NoteStyle == "AirEnd")
                        {
                            int number = _note.LongNoteNumber;
                            if (number == -1) { deleteNote(_note); return; }
                            string style;
                            if (_note.NoteStyle == "SlideEnd") style = "Slide";
                            else if (_note.NoteStyle == "HoldEnd") style = "Hold";
                            else style = "AirBegin";
                            bool isOver = false;
                            for (MusicScore score = this; score != null; score = score.prevScore)
                            {
                                if (score == null) break;
                                foreach (ShortNote __note in score.shortNotes.ToArray())
                                {
                                    if (__note.LongNoteNumber == number && __note.NoteStyle == style) isOver = true;
                                    if (__note.LongNoteNumber == number) score.deleteNote(__note);
                                }
                                if (isOver) break; //{ MessageBox.Show("Over!"); break; }
                            }
                            break;
                        }
                        deleteNote(_note);
                        flg = true;
                        break;
                    }
                }
                if (!flg)
                {
                    foreach (ShortNote _special in specialNotes.Reverse<ShortNote>())//特殊ノーツ
                    {
                        if (isMouseCollision(_special.DestPoints, e.Location))
                        {
                            //deleteNote(specialNotes[i]);
                            specialNotes.Remove(_special);
                            break;
                        }
                    }
                }
            }
            else if (selectedEditStatus == "Edit")
            {
                if (seRect != null && seRect.Rect.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Right)//選択矩形内での右クリックでコンテキストメニュー表示
                    {
                        for (int i = 0; i < stripItem.Count(); i++) stripItem[i].Enabled = true;
                        stripItem[2].Enabled = false;
                        selectMenuStrip.Show(this, e.Location);
                    }
                    else { seRectFlag = true; }//それ以外では移動用フラグを立てる
                }
                else if (e.Button == MouseButtons.Right)//選択矩形以外での右クリック
                {
                    for(int i=0; i < stripItem.Count(); i++) stripItem[i].Enabled = false;
                    if (sRoot.YankRect != null) stripItem[2].Enabled = true;
                    selectMenuStrip.Show(this, e.Location);
                }
                bool hitFlag = false;
                foreach(ShortNote _note in shortNotes.Reverse<ShortNote>())
                {
                    if(_note.NoteStyle != "SlideLine" && _note.NoteStyle != "HoldLine" && _note.NoteStyle != "AirLine" && isMouseCollision(_note.DestPoints, e.Location))
                    {
                        selectedNote = _note; //MessageBox.Show("Hit");
                        int hitWidth = Math.Min(3 * selectedNote.NoteSize, 12);
                        if (!(new string[] { "Hold", "HoldLine", "HoldEnd", "AirBegin", "AirLine", "AirAction", "AirEnd" }.Contains(_note.NoteStyle)))
                        {
                            if (e.Location.X < selectedNote.NotePosition.X + hitWidth) hitLeftFlag = true;//ノーツ左端を選択した場合　サイズ変更用フラグ
                            else if (selectedNote.NotePosition.X + selectedNote.NoteSize * 10 - hitWidth < e.Location.X) hitRightFlag = true;//ノーツ右端を選択した場合
                        }
                        hitFlag = true;
                        foreach(ShortNote __note in shortNotes)
                        {
                            if(__note != _note && __note.LongNoteNumber == _note.LongNoteNumber && __note.LongNoteNumber != -1 && __note.EndPosition == _note.NotePosition && _note.NoteStyle != "SlideCurve")
                            {
                                selectedNote_prev = __note;
                                break;
                            }
                        }
                        foreach (ShortNote __note in shortNotes)
                        {
                            if (__note != _note && __note.LongNoteNumber == _note.LongNoteNumber && __note.LongNoteNumber != -1 && __note.StartPosition == _note.NotePosition && _note.NoteStyle != "SlideCurve")
                            {
                                selectedNote_next = __note;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (seRectFlag) selectedNote = null;//矩形選択移動のときは単ノーツ移動はさせない
                if (!hitFlag && !seRectFlag && e.Button == MouseButtons.Left)//矩形選択
                {
                    rectSelectBegin = locationize(e.Location, 0); rectSelectEnd = rectSelectBegin;
                    seRect = new RectSelect(rectSelectBegin, rectSelectEnd, shortNotes);
                    rectSelectFlag = true;
                }
            }
            sRoot.setEdited(true);
        }

        private void this_MouseMove(object sender, MouseEventArgs e)
        {
            if (rectSelectFlag)
            {
                seRect.RectDR = locationize(e.Location, 0);
                seRect.update();
                update();
                return;
            }
            if (seRectFlag)
            {
                seRect.move(locationize(e.Location, seRect.rectSize));
                update();
                return;
            }
            if (seRect != null && seRect.Rect.Contains(e.Location)) Cursor = Cursors.SizeAll;
            else Cursor = Cursors.Default;
            if (selectedEditStatus == "Add" && e.Button == eyedropperMouseButton) return;//スポイト機能使用時
            if (selectedEditStatus == "Add" && !addSlideRelayFlag)
            {
                if (previewNote != null)
                {
                    //if (selectedNoteStyle == "AirLine") previewNote.NotePosition = new Point();
                    previewNote.NotePosition = locationize(e.Location);
                }
                if (previewLongNote != null)
                {
                    if (selectedNoteStyle == "Hold")
                    {
                        previewNote.NotePosition = locationize(new Point(startNote.NotePosition.X, e.Y - 1));
                    }
                    else if (selectedNoteStyle == "Slide")
                    {

                    }
                    else if (selectedNoteStyle == "AirLine")
                    {
                        //MessageBox.Show(startNote.NoteStyle);
                        previewNote.NotePosition = locationize(new Point(startNote.NotePosition.X, e.Y - 1));//locationize(new Point(previewLongNote.StartPosition.X, e.Y - 1));
                    }
                    if (previewLongNote.StartPosition.Y > e.Y)
                    {
                        Cursor.Current = Cursors.Arrow;
                        previewLongNote.EndPosition = locationize(e.Location);
                    }
                    else Cursor.Current = Cursors.No;
                }
                if (selectedEditStatus != "Add" && previewNote != null) previewNote = null;
                else if (selectedEditStatus == "Add" && previewNote == null)//ショートカットキーによるAddモードへの変更時の処理
                {
                    if (selectedNoteStyle == "AirLine")
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, "AirAction", selectedAirDirection, 0);
                    }
                    else
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, selectedNoteStyle, selectedAirDirection, 0);
                    }
                }
                else if (selectedEditStatus == "Add" && previewNote != null && previewNote.NoteStyle != selectedNoteStyle)//previewノーツにノーツスタイルを反映
                {
                    if (selectedNoteStyle == "AirLine")
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, "AirAction", selectedAirDirection, 0);
                    }
                    else
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, selectedNoteStyle, selectedAirDirection, 0);
                    }
                }
                else if (selectedEditStatus == "Add" && previewNote != null && previewNote.AirDirection != selectedAirDirection)//previewノーツにAirの向きを反映
                {
                    if (selectedNoteStyle == "AirLine")
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, "AirAction", selectedAirDirection, 0);
                    }
                    else
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, selectedNoteStyle, selectedAirDirection, 0);
                    }
                }
                else if (selectedEditStatus == "Add" && previewNote != null && previewNote.NoteSize != selectedNoteSize)//previewノーツにノーツサイズを反映
                {
                    if (selectedNoteStyle == "AirLine")
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, "AirAction", selectedAirDirection, 0);
                    }
                    else
                    {
                        previewNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, selectedNoteStyle, selectedAirDirection, 0);
                    }
                }
            }
            else if (hitLeftFlag)//Editモードでのノーツサイズ変更
            {
                ShortNote borderPrev = null, borderNext = null;
                if (selectedNote.NotePosition.Y == 2 && nextScore != null)
                {
                    borderNext = nextScore.shortNotes.Find(
                        x => x.NotePosition.Y == 770 && 
                        x.NoteStyle == selectedNote.NoteStyle && 
                        x.NoteSize == selectedNote.NoteSize && 
                        x.NotePosition.X == selectedNote.NotePosition.X);
                }
                else if (selectedNote.NotePosition.Y == 770 && prevScore != null)
                {
                    borderPrev = prevScore.shortNotes.Find(
                        x => x.NotePosition.Y == 2 &&
                        x.NoteStyle == selectedNote.NoteStyle &&
                        x.NoteSize == selectedNote.NoteSize &&
                        x.NotePosition.X == selectedNote.NotePosition.X);
                }
                Cursor.Current = Cursors.SizeWE;
                int endPos = 10 * selectedNote.NoteSize + selectedNote.NotePosition.X;
                if (endPos - e.Location.X < 160 / selectedGrid) selectedNote.NoteSize = 16 / selectedGrid;
                else
                {
                    selectedNote.NotePosition = new Point(locationize(e.Location, 0).X, selectedNote.NotePosition.Y);
                    selectedNote.NoteSize = (endPos - selectedNote.NotePosition.X) / 10;
                }
                selectedNote.update();
                if(new string[] { "Slide", "SlideTap", "SlideRelay", "SlideEnd" }.Contains(selectedNote.NoteStyle))
                {
                    ShortNote prev, next;
                    prev = shortNotes.Find(x => x.LongNoteNumber == selectedNote.LongNoteNumber && x.NoteStyle == "SlideLine" && x.EndPosition.Y == selectedNote.NotePosition.Y);
                    next = shortNotes.Find(x => x.LongNoteNumber == selectedNote.LongNoteNumber && x.NoteStyle == "SlideLine" && x.StartPosition.Y == selectedNote.NotePosition.Y);
                    if(prev != null) { prev.EndSize = selectedNote.NoteSize; prev.EndPosition = selectedNote.NotePosition; prev.update(); }
                    if(next != null) { next.StartSize = selectedNote.NoteSize; next.StartPosition = selectedNote.NotePosition; next.update(); }
                }
                if(borderNext != null)
                {
                    borderNext.NotePosition = new Point(selectedNote.NotePosition.X, borderNext.NotePosition.Y);
                    borderNext.NoteSize = selectedNote.NoteSize;
                    borderNext.update();
                    nextScore.update();
                }
                if (borderPrev != null)
                {
                    borderPrev.NotePosition = new Point(selectedNote.NotePosition.X, borderPrev.NotePosition.Y);
                    borderPrev.NoteSize = selectedNote.NoteSize;
                    borderPrev.update();
                    prevScore.update();
                }
            }
            else if (hitRightFlag)//Editモードでのノーツサイズ変更
            {
                ShortNote borderPrev = null, borderNext = null;
                if (selectedNote.NotePosition.Y == 2 && nextScore != null)
                {
                    borderNext = nextScore.shortNotes.Find(
                        x => x.NotePosition.Y == 770 &&
                        x.NoteStyle == selectedNote.NoteStyle &&
                        x.NoteSize == selectedNote.NoteSize &&
                        x.NotePosition.X == selectedNote.NotePosition.X);
                }
                else if (selectedNote.NotePosition.Y == 770 && prevScore != null)
                {
                    borderPrev = prevScore.shortNotes.Find(
                        x => x.NotePosition.Y == 2 &&
                        x.NoteStyle == selectedNote.NoteStyle &&
                        x.NoteSize == selectedNote.NoteSize &&
                        x.NotePosition.X == selectedNote.NotePosition.X);
                }
                Cursor.Current = Cursors.SizeWE;
                if (e.Location.X - selectedNote.NotePosition.X < 160 / selectedGrid) selectedNote.NoteSize = 16 / selectedGrid;
                else selectedNote.NoteSize = (locationize(e.Location, 0).X - selectedNote.NotePosition.X) / 10;
                selectedNote.update();
                if (new string[] { "Slide", "SlideTap", "SlideRelay", "SlideEnd" }.Contains(selectedNote.NoteStyle))
                {
                    ShortNote prev, next;
                    prev = shortNotes.Find(x => x.LongNoteNumber == selectedNote.LongNoteNumber && x.NoteStyle == "SlideLine" && x.EndPosition.Y == selectedNote.NotePosition.Y);
                    next = shortNotes.Find(x => x.LongNoteNumber == selectedNote.LongNoteNumber && x.NoteStyle == "SlideLine" && x.StartPosition.Y == selectedNote.NotePosition.Y);
                    if (prev != null) { prev.EndSize = selectedNote.NoteSize; prev.EndPosition = selectedNote.NotePosition; prev.update(); }
                    if (next != null) { next.StartSize = selectedNote.NoteSize; next.StartPosition = selectedNote.NotePosition; next.update(); }
                }
                if (borderNext != null)
                {
                    borderNext.NotePosition = new Point(selectedNote.NotePosition.X, borderNext.NotePosition.Y);
                    borderNext.NoteSize = selectedNote.NoteSize;
                    borderNext.update();
                    nextScore.update();
                }
                if (borderPrev != null)
                {
                    borderPrev.NotePosition = new Point(selectedNote.NotePosition.X, borderPrev.NotePosition.Y);
                    borderPrev.NoteSize = selectedNote.NoteSize;
                    borderPrev.update();
                    prevScore.update();
                }
            }
            else if (selectedEditStatus == "Edit" || addSlideRelayFlag)
            {
                if (previewNote != null) previewNote = null;
                int threshold = 384 / selectedBeat;
                if (selectedNote != null)
                {
                    if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                    {
                        selectedNote.NotePosition = new Point(selectedNote.NotePosition.X, locationize(e.Location).Y);
                    }
                    else
                    {
                        selectedNote.NotePosition = locationize(e.Location, selectedNote.NoteSize);
                    }
                    fixBorder(selectedNote);
                    selectedNote.update();
                }
                if (selectedNote_prev != null && selectedNote_next == null && selectedNote != null)//ロングノーツの終点
                {
                    if (selectedNote_prev.StartPosition.Y - selectedNote.NotePosition.Y >= threshold)
                    {
                        if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                        {
                            selectedNote_prev.EndPosition = new Point(selectedNote_prev.EndPosition.X, locationize(e.Location).Y);
                        }
                        else
                        {
                            selectedNote_prev.EndPosition = locationize(e.Location, selectedNote_prev.EndSize);
                        }
                    }
                    else
                    {
                        if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                        {
                            Point binded = new Point(selectedNote.NotePosition.X, locationize(e.Location).Y);
                            selectedNote.NotePosition = new Point(locationize(binded, selectedNote.NoteSize).X, selectedNote_prev.StartPosition.Y - threshold);
                        }
                        else
                        {
                            selectedNote.NotePosition = new Point(locationize(e.Location, selectedNote.NoteSize).X, selectedNote_prev.StartPosition.Y - threshold);
                        }
                        selectedNote_prev.EndPosition = selectedNote.NotePosition;
                    }
                    selectedNote_prev.NotePosition = selectedNote.NotePosition;
                    selectedNote_prev.update();
                    fixBorder(selectedNote_prev);
                    selectedNote.update();
                    //fixBorder(selectedNote);
                }
                else if (selectedNote_next != null && selectedNote_prev == null && selectedNote != null)//ロングノーツの始点
                {
                    if (selectedNote.NotePosition.Y - selectedNote_next.EndPosition.Y >= threshold)
                    {
                        if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                        {
                            selectedNote_next.StartPosition = new Point(selectedNote_next.StartPosition.X, locationize(e.Location).Y);
                        }
                        else
                        {
                            selectedNote_next.StartPosition = locationize(e.Location, selectedNote_next.StartSize);
                        }
                        selectedNote_next.update();
                    }
                    else
                    {
                        if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                        {
                            Point binded = new Point(selectedNote.NotePosition.X, locationize(e.Location).Y);
                            selectedNote.NotePosition = new Point(locationize(binded, selectedNote.NoteSize).X, selectedNote_next.EndPosition.Y + threshold);
                        }
                        else
                        {
                            selectedNote.NotePosition = new Point(locationize(e.Location, selectedNote.NoteSize).X, selectedNote_next.EndPosition.Y + threshold);
                        }
                        selectedNote_next.StartPosition = selectedNote.NotePosition;
                        selectedNote_next.update();
                    }
                    fixBorder(selectedNote_next);
                    selectedNote.update();
                    //fixBorder(selectedNote);
                }
                else if (selectedNote_prev != null && selectedNote_next != null && selectedNote != null)//ロングノーツの中間点
                {
                    if (selectedNote_prev.StartPosition.Y - selectedNote.NotePosition.Y < threshold)
                    {
                        if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                        {
                            Point binded = new Point(selectedNote.NotePosition.X, locationize(e.Location).Y);
                            selectedNote.NotePosition = new Point(locationize(binded, selectedNote.NoteSize).X, selectedNote_prev.StartPosition.Y - threshold);
                        }
                        else
                        {
                            selectedNote.NotePosition = new Point(locationize(e.Location, selectedNote.NoteSize).X, selectedNote_prev.StartPosition.Y - threshold);
                        }
                        selectedNote_prev.EndPosition = selectedNote.NotePosition;
                        selectedNote_prev.update();
                        selectedNote_next.StartPosition = selectedNote.NotePosition;
                        selectedNote_next.update();
                    }
                    else if (selectedNote.NotePosition.Y - selectedNote_next.EndPosition.Y < threshold)
                    {
                        if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                        {
                            Point binded = new Point(selectedNote.NotePosition.X, locationize(e.Location).Y);
                            selectedNote.NotePosition = new Point(locationize(binded, selectedNote.NoteSize).X, selectedNote_next.EndPosition.Y + threshold);
                        }
                        else
                        {
                            selectedNote.NotePosition = new Point(locationize(e.Location, selectedNote.NoteSize).X, selectedNote_next.EndPosition.Y + threshold);
                        }
                        selectedNote_next.StartPosition = selectedNote.NotePosition;
                        selectedNote_next.update();
                        selectedNote_prev.EndPosition = selectedNote.NotePosition;
                        selectedNote_prev.update();
                    }
                    else
                    {
                        if (selectedNote.NoteStyle == "Hold" || selectedNote.NoteStyle == "HoldEnd" || selectedNote.NoteStyle == "AirAction" || selectedNote.NoteStyle == "AirBegin" || selectedNote.NoteStyle == "AirEnd")
                        {
                            selectedNote_prev.EndPosition = new Point(selectedNote_prev.EndPosition.X, locationize(e.Location).Y);
                            selectedNote_next.StartPosition = new Point(selectedNote_next.StartPosition.X, locationize(e.Location).Y);
                        }
                        else
                        {
                            selectedNote_prev.EndPosition = locationize(e.Location, selectedNote_prev.EndSize);
                            selectedNote_next.StartPosition = locationize(e.Location, selectedNote_next.StartSize);
                        }
                        selectedNote_prev.update();
                        selectedNote_next.update();
                    }
                    //fixBorder(selectedNote);
                    fixBorder(selectedNote_prev);
                    fixBorder(selectedNote_next);
                    selectedNote.update();
                }
            }
            else if (selectedEditStatus == "Delete" && previewNote != null) previewNote = null;
            update();
        }

        private void this_MouseUp(object sender, MouseEventArgs e)
        {
            if (hitLeftFlag) { hitLeftFlag = false; Cursor = Cursors.Default; }
            if (hitRightFlag) { hitRightFlag = false; Cursor = Cursors.Default; }
            if (rectSelectFlag)
            {
                rectSelectFlag = false;
                //seRect.removeSelectedNotes();
                return;
            }
            if (seRectFlag)
            {
                seRectFlag = false;
                return;
            }
            previewVisible = true;
            if (selectedEditStatus == "Add")
            {
                if (e.Button == eyedropperMouseButton) return;//スポイト機能使用時
                ShortNote shortNote;
                endPosition = locationize(e.Location);
                if (endPosition.Y < 2) endPosition.Y = 2;
                if (endPosition.X < leftMargin + 1) endPosition.X = leftMargin + 1;
                if (endPosition.X > 161 + leftMargin - selectedNoteSize * 10) endPosition.X = 161 + leftMargin - selectedNoteSize * 10;
                switch (selectedNoteStyle)
                {
                    case "Hold":
                        if (endPosition.Y < startPosition.Y)
                        {
                            shortNote = new ShortNote(this, locationize(e.Location), startPosition, new Point(startPosition.X, endPosition.Y), selectedNoteSize, "HoldLine", selectedAirDirection, tmpLongNoteNumber);
                            addNote(shortNote);
                            shortNote = new ShortNote(this, new Point(startPosition.X, endPosition.Y), startPosition, new Point(startPosition.X, endPosition.Y), selectedNoteSize, "HoldEnd", selectedAirDirection, tmpLongNoteNumber);
                            //shortNote.setRelativePosition();
                            addNote(shortNote);
                            //shortNote.showRerativePosition();//debug
                            sRoot.LongNoteNumber++;
                        }
                        else if(selectedNote != null)//Holdが新たに生成されなかった時
                        {
                            //selectedNote.setRelativePosition();
                            addNote(selectedNote);
                            selectedNote = null;
                            deleteNote(startNote);
                        }
                        else deleteNote(startNote);
                        break;
                    case "Slide":
                        if (endPosition.Y < startPosition.Y && !addSlideRelayFlag)
                        {
                            shortNote = new ShortNote(this, locationize(e.Location), startPosition, endPosition, selectedNoteSize, selectedNoteSize, tmpLongNoteNumber);
                            addNote(shortNote);
                            shortNote = new ShortNote(this, endPosition, startPosition, endPosition, selectedNoteSize, "SlideEnd", selectedAirDirection, tmpLongNoteNumber);
                            //shortNote.setRelativePosition();
                            addNote(shortNote);
                            //shortNote.showRerativePosition();//debug
                            sRoot.LongNoteNumber++;
                        }
                        else if (selectedNote != null && !addSlideRelayFlag)//Slideが新たに生成されなかったときの後始末
                        {
                            selectedNote.NoteStyle = "SlideEnd";
                            selectedNote.update();
                            if(selectedNote.PrevNote != null)
                            {
                                selectedNote.PrevNote.NoteStyle = "SlideEnd";
                                selectedNote.PrevNote.update();
                            }
                            selectedNote = null;
                            deleteNote(startNote);
                        }
                        else deleteNote(startNote);
                        break;
                    case "AirLine":
                        if (endPosition.Y < startPosition.Y && !addSlideRelayFlag)
                        {
                            shortNote = new ShortNote(this, locationize(e.Location), startPosition, new Point(startPosition.X, endPosition.Y), selectedNoteSize, "AirLine", selectedAirDirection, tmpLongNoteNumber);
                            addNote(shortNote);
                            shortNote = new ShortNote(this, new Point(startPosition.X, endPosition.Y), startPosition, new Point(startPosition.X, endPosition.Y), selectedNoteSize, "AirEnd", selectedAirDirection, tmpLongNoteNumber);
                            //shortNote.setRelativePosition();
                            addNote(shortNote);
                            sRoot.LongNoteNumber++;
                        }
                        else if (selectedNote != null && !addSlideRelayFlag)//AirLineが新たに生成されなかったときの後始末
                        {
                            selectedNote.NoteStyle = "AirEnd";
                            selectedNote.update();
                            if (selectedNote.PrevNote != null)
                            {
                                selectedNote.PrevNote.NoteStyle = "AirEnd";
                                selectedNote.PrevNote.update();
                            }
                            selectedNote = null;
                        }
                        else deleteNote(startNote);
                        break;
                }
                previewLongNote = null;
                update();
            }
            else if (selectedEditStatus == "Edit")
            {
                
            }
            if(selectedNote != null) selectedNote = null;
            if(selectedNote_prev != null) selectedNote_prev = null;
            if(selectedNote_next != null) selectedNote_next = null;
            addSlideRelayFlag = false;
        }

        private void this_MouseEnter(object sender, EventArgs e)
        {
            if(selectedEditStatus == "Add")
            {
                if(SelectedNoteStyle == "AirLine")
                {
                    previewNote = new ShortNote(this, locationize(MousePosition), startPosition, endPosition, selectedNoteSize, "AirAction", selectedAirDirection, 0);
                }
                else
                {
                    previewNote = new ShortNote(this, locationize(MousePosition), startPosition, endPosition, selectedNoteSize, selectedNoteStyle, selectedAirDirection, 0);
                }
                update();
            }
        }

        private void this_MouseLeave(object sender, EventArgs e)
        {
            if(SelectedEditStatus == "Add" && previewNote != null)
            {
                previewNote = null;
                update();
            }
        }

        /// <summary>
        /// わからへん...（使わない多分）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_KeyDown(object sender, KeyEventArgs e)//わからへん...（使わない多分）
        {
            /*
            MessageBox.Show("Piyo");
            if (e.KeyData == Keys.Delete && seRect != null)
            {
                seRect.removeSelectedNotes();
                MessageBox.Show("Hoge");
            } 
            //*/
        }

        /// <summary>
        /// ノーツをリストにぶち込みます
        /// </summary>
        /// <param name="shortNote">ぶち込みたいノーツ</param>
        private void addNote(ShortNote shortNote)//, string position)
        {
            fixBorder(shortNote);
            shortNotes.Add(shortNote);
        }

        /// <summary>
        /// 譜面上下境界での処理をがんばります
        /// </summary>
        /// <param name="shortNote">処理したいノーツ</param>
        private void fixBorder(ShortNote shortNote)
        {
            if (shortNote.NotePosition.Y == 770 && prevScore != null && shortNote.PrevNote == null && shortNote.NextNote == null)
            {
                ShortNote prevShortNote = new ShortNote(prevScore, shortNote.NotePosition, shortNote.StartPosition, shortNote.EndPosition, shortNote.NoteSize, shortNote.NoteStyle, shortNote.AirDirection, shortNote.LongNoteNumber);
                prevShortNote.StartPosition = new Point(prevShortNote.StartPosition.X, prevShortNote.StartPosition.Y - 768);
                prevShortNote.EndPosition = new Point(prevShortNote.EndPosition.X, prevShortNote.EndPosition.Y - 768);
                prevShortNote.NotePosition = new Point(prevShortNote.NotePosition.X, prevShortNote.NotePosition.Y - 768);
                prevShortNote.update();////
                //prevShortNote.LongNoteNumber = shortNote.LongNoteNumber;
                prevShortNote.NextNote = shortNote;
                shortNote.PrevNote = prevShortNote;
                prevScore.shortNotes.Add(prevShortNote);
                prevScore.update();
            }
            else if (shortNote.NotePosition.Y == 2 && nextScore != null && shortNote.PrevNote == null && shortNote.NextNote == null)
            {
                ShortNote nextShortNote;
                //*
                if(shortNote.NoteStyle == "SlideLine")
                {
                    nextShortNote = new ShortNote(nextScore, shortNote.NotePosition, shortNote.StartPosition, shortNote.EndPosition, shortNote.StartSize, shortNote.EndSize, shortNote.LongNoteNumber);
                }
                else nextShortNote = new ShortNote(nextScore, shortNote.NotePosition, shortNote.StartPosition, shortNote.EndPosition, shortNote.NoteSize, shortNote.NoteStyle, shortNote.AirDirection, shortNote.LongNoteNumber);
                nextShortNote.StartPosition = new Point(nextShortNote.StartPosition.X, nextShortNote.StartPosition.Y + 768);
                nextShortNote.EndPosition = new Point(nextShortNote.EndPosition.X, nextShortNote.EndPosition.Y + 768);
                //nextShortNote.StartSize = shortNote.StartSize;
                //nextShortNote.EndSize = shortNote.EndSize;
                nextShortNote.NotePosition = new Point(nextShortNote.NotePosition.X, nextShortNote.NotePosition.Y + 768);
                nextShortNote.update();//
                //nextShortNote.LongNoteNumber = shortNote.LongNoteNumber;
                nextShortNote.PrevNote = shortNote;
                shortNote.NextNote = nextShortNote;
                nextScore.shortNotes.Add(nextShortNote);
                //nextShortNote.showRerativePosition();//debug
                nextScore.update();
                //*/
            }
            //*
            if (shortNote.NotePosition.Y != 770 && shortNote.PrevNote != null && shortNote.NoteStyle != "SlideLine")
            {
                prevScore.shortNotes.Remove(shortNote.PrevNote);
                shortNote.PrevNote = null;
                prevScore.update();
            }
            else if(shortNote.NotePosition.Y != 2 && shortNote.NextNote != null && shortNote.NoteStyle != "SlideLine")
            {
                nextScore.shortNotes.Remove(shortNote.NextNote);
                shortNote.NextNote = null;
                nextScore.update();
            }
            //*/
            if (shortNote.PrevNote != null)
            {
                shortNote.PrevNote.NotePosition = new Point(shortNote.NotePosition.X, shortNote.PrevNote.NotePosition.Y);
                prevScore.update();
            }
            if (shortNote.NextNote != null)
            {
                shortNote.NextNote.NotePosition = new Point(shortNote.NotePosition.X, shortNote.NextNote.NotePosition.Y);
                nextScore.update();
            } 
            return;
        }

        /// <summary>
        /// マウスポインタと3点で定義された平行四辺形の衝突判定をします
        /// </summary>
        /// <param name="_destPoints">3点で定義された平行四辺形</param>
        /// <param name="e">マウスポインタの位置</param>
        /// <returns></returns>
        private bool isMouseCollision(Point[] _destPoints, Point e)
        {
            Point upperLeft = _destPoints[0], upperRight = _destPoints[1], lowerLeft = _destPoints[2];
            Point vec_ULLL = new Point(lowerLeft.X - upperLeft.X, lowerLeft.Y - upperLeft.Y);//->ac
            Point vec_ULUR = new Point(upperRight.X - upperLeft.X, upperRight.Y - upperLeft.Y);//->ab
            Point vec_ULE = new Point(e.X - upperLeft.X, e.Y - upperLeft.Y);//->ap
            double s = (vec_ULE.X * vec_ULLL.Y - vec_ULLL.X * vec_ULE.Y) / (double)(vec_ULUR.X * vec_ULLL.Y - vec_ULLL.X * vec_ULUR.Y);
            double t = (vec_ULE.X * vec_ULUR.Y - vec_ULUR.X * vec_ULE.Y) / (double)(vec_ULLL.X * vec_ULUR.Y - vec_ULUR.X * vec_ULLL.Y);
            //MessageBox.Show("s = " + s + '\n' + "t = " + t);
            if (0 <= s && s <= 1 && 0 <= t && t <= 1) return true;
            return false;
        }

        /// <summary>
        /// マウスポインタと3点で定義された三角形の衝突判定をします
        /// </summary>
        /// <param name="_destPoints">3点で定義された三角形</param>
        /// <param name="e">マウスポインタの位置</param>
        /// <returns></returns>
        private bool isMouseCollisionTriangle(Point[] _destPoints, Point e)
        {
            Point upperLeft = _destPoints[0], upperRight = _destPoints[1], lowerLeft = _destPoints[2];
            Point vec_ULLL = new Point(lowerLeft.X - upperLeft.X, lowerLeft.Y - upperLeft.Y);//->ac
            Point vec_ULUR = new Point(upperRight.X - upperLeft.X, upperRight.Y - upperLeft.Y);//->ab
            Point vec_ULE = new Point(e.X - upperLeft.X, e.Y - upperLeft.Y);//->ap
            double s = (vec_ULE.X * vec_ULLL.Y - vec_ULLL.X * vec_ULE.Y) / (double)(vec_ULUR.X * vec_ULLL.Y - vec_ULLL.X * vec_ULUR.Y);
            double t = (vec_ULE.X * vec_ULUR.Y - vec_ULUR.X * vec_ULE.Y) / (double)(vec_ULLL.X * vec_ULUR.Y - vec_ULUR.X * vec_ULLL.Y);
            if (0 <= s && 0 <= t && s + t <= 1) return true;
            return false;
        }

        /// <summary>
        /// マウスポインタと4点で定義された四角形の衝突判定をします
        /// </summary>
        /// <param name="_destPoints">4点で定義された四角形</param>
        /// <param name="e">マウスポインタの位置</param>
        /// <returns></returns>
        private bool isMouseCollisionSquare(Point[] _destPoints, Point e)//{ul, ur, ll, lr}
        {
            return (isMouseCollisionTriangle(new Point[] { _destPoints[0], _destPoints[1], _destPoints[2] }, e) ||
                isMouseCollisionTriangle(new Point[] { _destPoints[1], _destPoints[2], _destPoints[3]}, e));
        }

        /// <summary>
        /// いろいろと更新します
        /// </summary>
        public void update()
        {
            //storeImage = imageLN;
            Graphics g = Graphics.FromImage(storeImage);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            /*
            g.DrawImage(Properties.Resources.SlideLine, new Point[]{ new Point(10, 10), new Point(30, 10), new Point(40, 30) });
            //*/
            foreach (ShortNote _note in specialNotes)//BPMノーツ，Speedノーツを描画
            {
                if (_note.NoteStyle == "BPM")
                {
                    g.DrawImage(_note.NoteImage, _note.NotePosition);
                    g.DrawString(_note.SpecialValue.ToString(), new Font("ＭＳ ゴシック", 8, FontStyle.Bold), Brushes.Lime, new Rectangle(0, _note.NotePosition.Y - 5, this.Width, 15), sFormat);//BPM
                }
                else
                {
                    g.DrawImage(_note.NoteImage, _note.NotePosition);
                    g.DrawString("x" + _note.SpecialValue.ToString(), new Font("ＭＳ ゴシック", 8, FontStyle.Bold), Brushes.Crimson, new Rectangle(0, _note.NotePosition.Y - 5, this.Width, 15), sFormat);//Speed
                }
            }
            foreach (ShortNote _note in shortNotes.Where(x => new string[] { "Hold", "HoldLine", "HoldEnd" }.Contains(x.NoteStyle)))//Hold描画
            {
                if (_note.NoteStyle == "HoldLine") g.DrawImage(_note.NoteImage, new Point(_note.StartPosition.X, _note.EndPosition.Y));
                else g.DrawImage(_note.NoteImage, _note.NotePosition);
            }
            foreach (ShortNote _note in shortNotes.Where(x => x.NoteStyle == "SlideCurve"))//曲線の描画
            {
                ShortNote prevNote = null, nextNote = null;
                ShortNote[] tmpNotes = shortNotes.Where(x => x.LongNoteNumber == _note.LongNoteNumber && x.NotePosition.Y > _note.NotePosition.Y && x.NoteStyle != "SlideLine" && x.NoteStyle != "SlideCurve").OrderBy(x => x.NotePosition.Y).ToArray();
                if (tmpNotes.Any()) prevNote = tmpNotes[0];
                tmpNotes = shortNotes.Where(x => x.LongNoteNumber == _note.LongNoteNumber && x.NotePosition.Y < _note.NotePosition.Y && x.NoteStyle != "SlideLine" && x.NoteStyle != "SlideCurve").OrderByDescending(x => x.NotePosition.Y).ToArray();
                if (tmpNotes.Any()) nextNote = tmpNotes[0];

                GraphicsPath gp = new GraphicsPath();

                if (prevNote != null && nextNote != null)
                {
                    Point prev = prevNote.NotePosition;
                    Point next = nextNote.NotePosition;
                    int curSize = _note.NoteSize * 10;
                    int prevSize = prevNote.NoteSize * 10;
                    int nextSize = nextNote.NoteSize * 10;

                    gp.AddLine(new Point(prev.X + 2, prev.Y), new Point(prev.X + prevSize - 2, prev.Y));
                    gp.AddLine(new Point(next.X + 2, next.Y), new Point(next.X + nextSize - 2, next.Y));

                    float ratio = 11 / 16f;
                    Pen pen = new Pen(Color.FromArgb(0, 224, 224), 1);
                    Point prevcur = new Point(prev.X + (int)((_note.NotePosition.X - prev.X + 5 * (_note.NoteSize - prevNote.NoteSize)) * ratio) + 2, prev.Y + (int)((_note.NotePosition.Y - prev.Y) * ratio));
                    Point curnext = new Point(next.X + (int)((_note.NotePosition.X - next.X + 5 * (_note.NoteSize - nextNote.NoteSize)) * ratio) + 2, next.Y + (int)((_note.NotePosition.Y - next.Y) * ratio));
                    prev = new Point(prev.X + 2, prev.Y);
                    next = new Point(next.X + 2, next.Y);
                    g.DrawBezier(pen, prev, prevcur, curnext, next);
                    gp.AddBezier(prev, prevcur, curnext, next);
                    prev = prevNote.NotePosition;
                    next = nextNote.NotePosition;
                    prevcur = new Point(prev.X + prevSize + (int)((_note.NotePosition.X - prev.X + 5 * (_note.NoteSize - prevNote.NoteSize)) * ratio) - 2, prev.Y + (int)((_note.NotePosition.Y - prev.Y) * ratio));
                    curnext = new Point(next.X + nextSize + (int)((_note.NotePosition.X - next.X + 5 * (_note.NoteSize - nextNote.NoteSize)) * ratio) - 2, next.Y + (int)((_note.NotePosition.Y - next.Y) * ratio));
                    prev = new Point(prev.X + prevSize - 2, prev.Y);
                    next = new Point(next.X + nextSize - 2, next.Y);
                    g.DrawBezier(pen, prev, prevcur, curnext, next);
                    gp.AddBezier(prev, prevcur, curnext, next);

                    Region reg = new Region(gp);

                    g.FillRegion(new SolidBrush(Color.FromArgb(0, 224, 224)), reg);

                    prev = prevNote.NotePosition;
                    next = nextNote.NotePosition;
                    prevcur = new Point(prev.X + prevSize / 2 + (int)((_note.NotePosition.X - prev.X + 5 * (_note.NoteSize - prevNote.NoteSize)) * ratio), prev.Y + (int)((_note.NotePosition.Y - prev.Y) * ratio));
                    curnext = new Point(next.X + nextSize / 2 + (int)((_note.NotePosition.X - next.X + 5 * (_note.NoteSize - nextNote.NoteSize)) * ratio), next.Y + (int)((_note.NotePosition.Y - next.Y) * ratio));
                    
                    pen = new Pen(Color.FromArgb(28, 255, 255), 4);
                    g.DrawBezier(pen, new Point(prev.X + prevSize / 2 - 1, prev.Y), prevcur, curnext, new Point(next.X + nextSize / 2 - 1, next.Y));
                }
            }
            foreach (ShortNote _note in shortNotes)//普通のノーツを描画
            {
                if (new string[] { "Hold", "HoldLine", "HoldEnd" }.Contains(_note.NoteStyle)) continue;
                if (_note.NoteStyle == "AirUp" || _note.NoteStyle == "AirDown") g.DrawImage(_note.NoteImage, new Point(_note.NotePosition.X, _note.NotePosition.Y - 32));
                else if (_note.NoteStyle == "AirLine") g.DrawImage(_note.NoteImage, new Point(_note.StartPosition.X, _note.EndPosition.Y));
                else if (_note.NoteStyle == "SlideLine")
                {
                    //間にSlideCurveがあるときは描画しない（invisible）
                    if (shortNotes.Where(x => x.LongNoteNumber == _note.LongNoteNumber && x.NoteStyle == "SlideCurve" && _note.EndPosition.Y < x.NotePosition.Y && x.NotePosition.Y < _note.StartPosition.Y).Any())
                    {
                        _note.Visible = false;
                    }
                    else
                    {
                        _note.Visible = true;
                        if (_note.StartPosition.X >= _note.EndPosition.X) g.DrawImage(_note.NoteImage, _note.EndPosition);
                        else g.DrawImage(_note.NoteImage, new Point(_note.StartPosition.X, _note.EndPosition.Y));
                    }
                }
                else g.DrawImage(_note.NoteImage, _note.NotePosition);
            }
            if (previewNote != null && previewVisible)//プレビュー用のノーツを描画
            {
                if (previewNote.NoteStyle == "AirUp" || previewNote.NoteStyle == "AirDown") g.DrawImage(previewNote.NoteImage, new Point(previewNote.NotePosition.X, previewNote.NotePosition.Y - 32));
                else g.DrawImage(previewNote.NoteImage, previewNote.NotePosition);
            }
            //*
            if (previewLongNote != null)//プレビュー用ロングノーツを描画
            {
                previewLongNote.update();
                if (previewLongNote.NoteStyle == "SlideLine")
                {
                    if(previewLongNote.NotePosition.X >= previewNote.NotePosition.X) g.DrawImage(previewLongNote.NoteImage, previewNote.NotePosition);
                    else g.DrawImage(previewLongNote.NoteImage, new Point(previewLongNote.StartPosition.X, previewNote.NotePosition.Y));
                }
                else g.DrawImage(previewLongNote.NoteImage, new Point(previewLongNote.StartPosition.X, previewLongNote.EndPosition.Y));
            }
            //*/
            if (seRect != null)//矩形選択を描画
            {
                if (selectedEditStatus == "Edit") g.DrawRectangle(new Pen(Color.White), seRect.Rect);
                else seRect = null;
            }
            g.DrawString((2 * index + 1).ToString().PadLeft(3, '0'), new Font("ＭＳ ゴシック", 8, FontStyle.Bold), Brushes.White, new Rectangle(0, 768 - 5, 30, 10));//MeasureNumber
            g.DrawString((2 * (index + 1)).ToString().PadLeft(3, '0'), new Font("ＭＳ ゴシック", 8, FontStyle.Bold), Brushes.White, new Rectangle(0, 384 - 5, 30, 10));//MeasureNumber
            if(index == 0) g.DrawString(ScoreRoot.StartBPM.ToString(), new Font("ＭＳ ゴシック", 8, FontStyle.Bold), Brushes.Lime, new Rectangle(0, 768 - 5, Width, 15), sFormat);//StartBPM
            BackgroundImage = storeImage;
            g.Dispose();
            this.Refresh();
            sRoot.setTotalNotes();
            storeImage = Properties.Resources.MusicScore;
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// shortNotesのリストから指定した_noteを消します
        /// ロングノーツの節などの前後の処理などはなされません
        /// </summary>
        /// <param name="_note">消したいノーツ</param>
        private void deleteNote(ShortNote _note)
        {
            if (_note.PrevNote != null && prevScore != null)
            {
                prevScore.shortNotes.Remove(_note.PrevNote);
                prevScore.update();
            }
            else if(_note.NextNote != null && nextScore != null)
            {
                nextScore.shortNotes.Remove(_note.NextNote);
                nextScore.update();
            }
            if(_note.NotePosition.Y == 2 && nextScore != null)
            {
                foreach(ShortNote __note in nextScore.shortNotes.Reverse<ShortNote>())
                {
                    if(__note.NotePosition.X == _note.NotePosition.X && __note.NotePosition.Y == 770 && __note.NoteStyle == _note.NoteStyle && __note.NoteSize == _note.NoteSize)
                    {
                        nextScore.shortNotes.Remove(__note);
                        nextScore.update();
                        break;
                    }
                }
            }
            else if(_note.NotePosition.Y == 770 && prevScore != null)
            {
                foreach (ShortNote __note in prevScore.shortNotes.Reverse<ShortNote>())
                {
                    if (__note.NotePosition.X == _note.NotePosition.X && __note.NotePosition.Y == 2 && __note.NoteStyle == _note.NoteStyle && __note.NoteSize == _note.NoteSize)
                    {
                        prevScore.shortNotes.Remove(__note);
                        prevScore.update();
                        break;
                    }
                }
            }
            shortNotes.Remove(_note);
            update();
        }

        /// <summary>
        /// ロングノーツの節が渡された場合でも前後のLineノーツの繋ぎ変えなどやってくれます
        /// </summary>
        /// <param name="_note">消したいノーツ</param>
        private void deleteforLongNote(ShortNote _note)
        {
            if (_note.NoteStyle == "HoldLine" || _note.NoteStyle == "SlideLine" || _note.NoteStyle == "AirLine") return;
            else if (_note.NoteStyle == "SlideTap" || _note.NoteStyle == "SlideRelay")
            {
                ShortNote prev = null, next = null;
                foreach (ShortNote _prev in shortNotes)
                {
                    if (_prev.NoteStyle == "SlideLine" && _prev.LongNoteNumber == _note.LongNoteNumber && _prev.EndPosition == _note.NotePosition)
                    {
                        prev = _prev;
                        break;
                    }
                }
                foreach (ShortNote _next in shortNotes)
                {
                    if (_next.NoteStyle == "SlideLine" && _next.LongNoteNumber == _note.LongNoteNumber && _next.StartPosition == _note.NotePosition)
                    {
                        next = _next;
                        break;
                    }
                }
                if (next != null)
                {
                    prev.EndPosition = next.EndPosition;
                    prev.EndSize = next.EndSize;
                    prev.update();
                    deleteNote(next);
                    deleteNote(_note);
                }
            }
            else if (_note.NoteStyle == "AirAction")
            {
                ShortNote prev = null, next = null;
                foreach (ShortNote _prev in shortNotes)
                {
                    if (_prev.NoteStyle == "AirLine" && _prev.LongNoteNumber == _note.LongNoteNumber && _prev.EndPosition == _note.NotePosition)
                    {
                        prev = _prev;
                        break;
                    }
                }
                foreach (ShortNote _next in shortNotes)
                {
                    if (_next.NoteStyle == "AirLine" && _next.LongNoteNumber == _note.LongNoteNumber && _next.StartPosition == _note.NotePosition)
                    {
                        next = _next;
                        break;
                    }
                }
                if (next != null)
                {
                    prev.EndPosition = next.EndPosition;
                    prev.update();
                    deleteNote(next);
                    deleteNote(_note);
                }
            }
            else if (_note.NoteStyle == "Slide" || _note.NoteStyle == "Hold" || _note.NoteStyle == "AirBegin")
            {
                int number = _note.LongNoteNumber;
                if (number == -1) { deleteNote(_note); return; }
                string style;
                if (_note.NoteStyle == "Slide") style = "SlideEnd";
                else if (_note.NoteStyle == "Hold") style = "HoldEnd";
                else style = "AirEnd";
                bool isOver = false;
                for (MusicScore score = this; score != null; score = score.nextScore)
                {
                    foreach (ShortNote __note in score.shortNotes.ToArray())
                    {
                        if (__note.LongNoteNumber == number && __note.NoteStyle == style) isOver = true;
                        if (__note.LongNoteNumber == number) score.deleteNote(__note);
                    }
                    if (isOver) break; //{ MessageBox.Show("break!"); break; }
                }
                return;
            }
            else if (_note.NoteStyle == "SlideEnd" || _note.NoteStyle == "HoldEnd" || _note.NoteStyle == "AirEnd")
            {
                int number = _note.LongNoteNumber;
                if (number == -1) { deleteNote(_note); return; }
                string style;
                if (_note.NoteStyle == "SlideEnd") style = "Slide";
                else if (_note.NoteStyle == "HoldEnd") style = "Hold";
                else style = "AirBegin";
                bool isOver = false;
                for (MusicScore score = this; score != null; score = score.prevScore)
                {
                    if (score == null) break;
                    foreach (ShortNote __note in score.shortNotes.ToArray())
                    {
                        if (__note.LongNoteNumber == number && __note.NoteStyle == style) isOver = true;
                        if (__note.LongNoteNumber == number) score.deleteNote(__note);
                    }
                    if (isOver) break; //{ MessageBox.Show("Over!"); break; }
                }
                return;
            }
            deleteNote(_note);
            return;
        }

        /// <summary>
        /// とりあえず全部ノーツを消します
        /// </summary>
        public void deleteAllNotes()
        {
            shortNotes.Clear();
            specialNotes.Clear();
            update();
        }

        /// <summary>
        /// ノーツの座標pをグリッド上に指定します
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Point locationize(Point p)//通常
        {
            int noteX, noteY;
            if (p.X > 160 + leftMargin - 10 * selectedNoteSize) noteX = 160 + leftMargin - 10 * selectedNoteSize;
            else if (p.X < leftMargin) noteX = leftMargin;
            else noteX = ((p.X - leftMargin) / (10 * (16 / selectedGrid))) * (10 * (16 / selectedGrid)) + leftMargin;
            if (p.Y < topMargin) noteY = topMargin;
            else if (p.Y > 768 + topMargin) noteY = 768 + topMargin;
            else noteY = 768 + topMargin + bottomMargin - (int)(((768 + bottomMargin - p.Y) / (768 / (2 * selectedBeat))) * (768 / (double)(2 * selectedBeat))) - topMargin;
            noteX += 1; noteY += -3;//描写の都合上の位置調整

            return new Point(noteX, noteY);
        }

        /// <summary>
        /// ノーツサイズ(幅)を考慮したlocationize関数です
        /// Editモードなどで主に使います
        /// </summary>
        /// <param name="p"></param>
        /// <param name="size">ノーツサイズ(1-16)</param>
        /// <returns></returns>
        private Point locationize(Point p, int size)//EditModeなどで
        {
            int noteX, noteY;
            if (p.X > 160 + leftMargin - 10 * size) noteX = 160 + leftMargin - 10 * size;
            else if (p.X < leftMargin) noteX = leftMargin;
            else noteX = ((p.X - leftMargin) / (10 * (16 / selectedGrid))) * (10 * (16 / selectedGrid)) + leftMargin;
            if (p.Y < topMargin) noteY = topMargin;
            else if (p.Y > 768 + topMargin) noteY = 768 + topMargin;
            else noteY = 768 + topMargin + bottomMargin - (int)(((768 + bottomMargin - p.Y) / (768 / (2 * selectedBeat))) * (768 / (double)(2 * selectedBeat))) - topMargin;
            noteX += 1; noteY += -3;//描写の都合上の位置調整

            return new Point(noteX, noteY);
        }

        /// <summary>
        /// 幅高さともに考慮したlocationize関数です
        /// 矩形選択で使います
        /// </summary>
        /// <param name="p"></param>
        /// <param name="size">矩形サイズ</param>
        /// <returns></returns>
        private Point locationize(Point p, Size size)//矩形選択で
        {
            int noteX, noteY;
            if (p.X > 160 + leftMargin - size.Width) noteX = 160 + leftMargin - size.Width;
            else if (p.X < leftMargin) noteX = leftMargin;
            else noteX = ((p.X - leftMargin) / (10 * (16 / selectedGrid))) * (10 * (16 / selectedGrid)) + leftMargin;
            if (p.Y < topMargin) noteY = topMargin;
            else if (p.Y > 768 + topMargin - size.Height) noteY = 768 + topMargin - size.Height;
            else noteY = 768 + topMargin + bottomMargin - (int)(((768 + bottomMargin - p.Y) / (768 / (2 * selectedBeat))) * (768 / (double)(2 * selectedBeat))) - topMargin;
            noteX += 1; noteY += -3;//描写の都合上の位置調整

            return new Point(noteX, noteY);
        }
    }
}
