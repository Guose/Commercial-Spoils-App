using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;

namespace Commercial_Spoils_App
{
    class ExcelFile
    {
        #region Prop_Ctor
        private int rowNumber = 1;

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }

        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }


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
        #endregion Prop_Ctor

        private object missingValue = Missing.Value;
        Excel.Application xclFile;
        Excel.Workbook xclWBook;
        Excel.Worksheet xclSheet;

        public void CreateExcelFile()
        {
            xclFile = null;

            xclFile = new Excel.Application(); //create Excel App            

            xclFile.DisplayAlerts = false; // turn off alerts

            xclWBook = xclFile.Workbooks.Open(path, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, 
                missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue); //open existing excel file

            xclSheet = xclWBook.Worksheets[1];
            xclSheet.Name = "Spoils Entered";
        }

        public void AddDataToExcel(string firstNum, string lastNum)
        {
            xclSheet.Cells[rowNumber, "A"] = firstNum;
            xclSheet.Cells[rowNumber, "B"] = lastNum;
            rowNumber++;
        }

        public void CloseExcelFile(string fileName)
        {
            try
            {
                string name = "SpoilsAudit_" + fileName;
                string format = xclFile.DefaultSaveFormat.ToString();

                xclWBook.SaveAs(name, format, missingValue, missingValue, missingValue, Excel.XlSaveAsAccessMode.xlNoChange);

                xclWBook.Close(true, path, missingValue);
            }
            finally
            {
                if (xclFile != null)
                {
                    xclFile.Quit();
                }
            }
        }

    }
}
