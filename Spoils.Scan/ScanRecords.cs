using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoils.Scan
{
    class ScanRecords
    {
        #region Constructors

        public ScanRecords()
        {

        }

        public ScanRecords(string firstNum, string lastNum) : this()
        {
            FirstNumberScanned = firstNum;
            LastNumberScanned = lastNum;
        }

        #endregion Constructors

        #region Properties
        private string _singleScan;

        public string SingleScan
        {
            get { return _singleScan; }
            set { _singleScan = value; }
        }
        private string _firstNumberScanned;

        public string FirstNumberScanned
        {
            get { return _firstNumberScanned; }
            set { _firstNumberScanned = value; }
        }
        private string _lastNumberScanned;

        public string LastNumberScanned
        {
            get { return _lastNumberScanned; }
            set { _lastNumberScanned = value; }
        }
        #endregion Properties

        internal class Scan
        {
            public string Scann { get; set; }
        }

        internal class Scans
        {
            public List<Scan> _scans;

            public Scans()
            {
                _scans = new List<Scan>();

            }
            //AddSingle method to instantiate new object of Scan
            public void AddSingle(string scan)
            {
                _scans.Add(new Scan { Scann = scan });
            }
        }

    }
}
