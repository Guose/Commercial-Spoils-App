using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commercial_Spoils_App
{
    class GetScannedRecord : SequenceNumbers
    {
        private DataTable _dt;
        private string _ColumnToSearch;
        private string _IdUniqueToFind;
        private bool _DoMainBreak;

        public string ColumnToSearch  { get; set; }
        public string IdUniqueToFind  { get; set; }
        public bool DoMainBreak  { get; set; }


        public GetScannedRecord(long firstNum, long lastNum, long singleNum) : base ()
        {

        }



    }
}
