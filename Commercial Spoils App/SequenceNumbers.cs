using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commercial_Spoils_App
{
    class SequenceNumbers
    {
        private long _FirstNumberRange;
        private long _LastNumberRange;
        private long _SingleNumber;
        

        public long FirstNumberRange
        { get { return _FirstNumberRange; } set { _FirstNumberRange = value; } }

        public long LastNumberRange
        { get { return _LastNumberRange; } set { _LastNumberRange = value; } }

        public long SingleNumberScan
        { get { return _SingleNumber; } set { _SingleNumber = value; } }

        public SequenceNumbers() : this(0, 0, 0) { }


        public SequenceNumbers(long firstNum, long lastNum, long singleNum)
        {
            ScannedFirst(firstNum, lastNum);
            SingleNumberScan = singleNum;
        }

        private void ScannedFirst(long firstScanned, long lastScanned)
        {
            if (LastNumberRange <= FirstNumberRange)
            {
                firstScanned = LastNumberRange;
                lastScanned = FirstNumberRange;
            }
            else
            {
                firstScanned = FirstNumberRange;
                lastScanned = LastNumberRange;
            }
        }

    }
}
