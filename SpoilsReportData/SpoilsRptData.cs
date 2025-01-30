using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoilsReportData
{
    public class SpoilsRptData
    {
        private DataSet spoilsReport = new DataSet("Report");
        private DataTable _DtReport = new DataTable();
        private string[] column = null;

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
                DataColumn pkKeyId = _DtReport.Columns.Add("KEY");
                _DtReport.Columns.Add("SEQUENCE");
                _DtReport.Columns.Add("NAME");
                _DtReport.Columns.Add("BARCODE");
                _DtReport.PrimaryKey = new DataColumn[] { pkKeyId };
                _DtReport = spoilsReport.Tables.Add("SpoilsReport");
                return _DtReport;
            }
            set { value = _DtReport; }
        }

        public string SpoilsKey { get; set; }
        public string SequenceNumber { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }

        public SpoilsRptData()
        {

        }

        public SpoilsRptData(DataTable dt) : this()
        {           
            SpoilsDt = dt;
            this.PopulateReportDataTable();
        }

        public void PopulateReportDataTable()
        {
            int index = 0;

            column = new string[4];            

            foreach (DataColumn dc in SpoilsDt.Columns)
            {
                if (dc.ColumnName.Contains("KEY") || dc.ColumnName.Contains("Seq"))
                {
                    index = dc.Ordinal;
                    dc.ColumnName = "KEY";
                    column[0] = dc.ColumnName;
                }
                else if (dc.ColumnName.Contains("SEQ") || dc.ColumnName.Contains("SEQUENCE") || dc.ColumnName.Contains("Seq"))
                {
                    index = dc.Ordinal;
                    dc.ColumnName = "SEQUENCE";
                    column[1] = dc.ColumnName;
                }
                else if (dc.ColumnName.Contains("NAME") || dc.ColumnName.Contains("FULLNAME") || dc.ColumnName.Contains("Name"))
                {
                    index = dc.Ordinal;
                    dc.ColumnName = "NAME";
                    column[2] = dc.ColumnName;
                }
                else if (dc.ColumnName.Contains("BARCODE") || dc.ColumnName.Contains("BARCODE_2D") || dc.ColumnName.Contains("BarCode"))
                {
                    index = dc.Ordinal;
                    dc.ColumnName = "BARCODE";
                    column[3] = dc.ColumnName;
                    break;
                }
            }
            SpoilsReportDS.Tables.Add(SpoilsDt.DefaultView.ToTable("ReportData",false, column[0], column[1], column[2], column[3]));
        }
    }
}
