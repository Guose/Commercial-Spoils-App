using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace Commercial_Spoils_App
{
    class ExcelFile
    {
        private long firstNum;

        public long FirstNumber
        {
            get { return firstNum; }
            set { firstNum = value; }
        }

        private long lastNum;

        public long LastNumber
        {
            get { return lastNum; }
            set { lastNum = value; }
        }

        public ExcelFile(long first, long last)
        {
            FirstNumber = first;
            LastNumber = last;
        }

        public ExcelFile()
        {

        }



    }
}
