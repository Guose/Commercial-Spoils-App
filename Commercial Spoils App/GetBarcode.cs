using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commercial_Spoils_App
{
    class GetBarcode
    {
        private long _firstNum;
        private long _lastNum;
        private string _idUniqueToFind;

        public long FirstNumber
        { get { return _firstNum; } set { _firstNum = value; } }

        public long LastNumber
        { get { return _lastNum; } set { _lastNum = value; } }

        public string IdUniqueToFind
        { get { return _idUniqueToFind; } set { _idUniqueToFind = value; } }

        public GetBarcode()
        {

        }
    }
}
