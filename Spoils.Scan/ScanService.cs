using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Spoils.Scan
{
    public class ScanService
    {
        public ScanService()
        {

        }
        
        internal string Scan { get; private set; }
        internal bool IsSingle { get; private set; }
        internal ScanRecords.Scans Scans { get; private set; }

        public bool ProcessScan(string scan, bool isSingle)
        {
            //logic
            //return false;
            
            Scans.AddSingle(scan);

            return true;
        }

        public bool Finalize()
        {
            foreach (var scan in Scans._scans)
            {

            }
            return true;
        }
    }

}
