using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesEditerforD
{
    /// <summary>
    /// 相対座標管理用のクラス
    /// </summary>
    public class PosInfo
    {
        private int x, measure, beat, beatNumber;

        public PosInfo()
        {
            
        }

        public PosInfo(int _x, int _measure, int _beat, int _beatNumber)
        {
            x = _x;  measure = _measure; beat = _beat; beatNumber = _beatNumber;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Measure
        {
            get { return measure; }
            set { measure = value; }
        }

        public int Beat
        {
            get { return beat; }
            set { beat = value; }
        }

        public int BeatNumber
        {
            get { return beatNumber; }
            set { beatNumber = value; }
        }
    }
}
