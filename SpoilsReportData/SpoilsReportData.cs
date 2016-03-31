using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoilsReportData
{
    public class SpoilsData
    {
        private DataSet spoilsReport;
        private DataTable _DtReport = new DataTable();

        public bool NewMailing { get; set; } 
        public DataTable SpoilsDt { get; set; }
        public DataSet SpoilsReportDS
        {
            get { return spoilsReport; }
            set { spoilsReport = value; }
        }
        public DataTable ReportDT
        {
            get
            {
                _DtReport = spoilsReport.Tables.Add("SpoilsReport");
                DataColumn pkKeyId = _DtReport.Columns.Add("KEY");
                _DtReport.Columns.Add("SEQ");
                _DtReport.Columns.Add("FULLNAME");
                _DtReport.Columns.Add("BARCODE_2D");
                _DtReport.PrimaryKey = new DataColumn[] { pkKeyId };
                return _DtReport;
            }
            set { value = _DtReport; }
        }

        //public string SpoilsKey { get; set; }
        //public string SequenceNumber { get; set; }
        //public long FirstNumber { get; set; }
        //public long LastNumber { get; set; }
        //public string Name { get; set; }
        //public string Barcode { get; set; }        
          

        public SpoilsData(DataTable dt)
        {
            SpoilsDt = dt;
            this.PopulateReportDataTable();
        }

        public void PopulateReportDataTable()
        {
            foreach (DataRow row in SpoilsDt.Rows)
            {
                ReportDT.ImportRow(row);
            }
        }
    }
}
