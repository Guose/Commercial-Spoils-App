using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System;
using System.Windows;

namespace Commercial_Spoils_App
{
    class ExcelFile
    {
        #region Prop_Ctor
        private int rowNumber = 2;

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

        public ExcelFile(string path)
        {
            Path = path;
        }

        public ExcelFile()
        {

        }
        #endregion Prop_Ctor

        private object missingValue = Missing.Value;
        private Excel.Application xclFile;
        private Excel.Workbook xclWBook;
        private Excel.Worksheet xclSheet;
        private string excelFileName = string.Empty;


        public void CreateExcelFile()
        {
            try
            {
                xclFile = new Excel.Application();

                xclWBook = xclFile.Workbooks.Add(missingValue);
                xclSheet = xclWBook.Worksheets.get_Item(1);
                xclSheet.Name = "Spoils_Audit";

                xclSheet.Cells[1, 1] = "Starting Number";
                xclSheet.Cells[1, 2] = "Ending Number";
                xclSheet.Cells[1, 3] = "Quantity";

                xclWBook.SaveAs(GetNewPathName(), Excel.XlFileFormat.xlWorkbookNormal, missingValue, missingValue, missingValue, missingValue, Excel.XlSaveAsAccessMode.xlExclusive, missingValue, missingValue, missingValue, missingValue, missingValue);
                xclWBook.Close(true, missingValue, missingValue);
                xclFile.Quit();

                ReleaseObject(xclFile);
                ReleaseObject(xclSheet);
                ReleaseObject(xclWBook);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal string GetNewPathName()
        {
            FileInfo fi = new FileInfo(path);

            excelFileName = FileSave.AddSuffix(path, ".xls");
            return excelFileName;
            //return fi.Directory.ToString() + "\\" + excelFileName;
        }


        public void OpenExcelFile(string path)
        {
            xclFile = null;
            xclFile = new Excel.Application(); //create Excel App
            xclFile.DisplayAlerts = false; //turn off alerts
            Path = path;

            try
            {
                xclWBook = xclFile.Workbooks.Open(GetNewPathName(), missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, 
                    missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue); //open existing excel file

                xclSheet = xclWBook.Worksheets[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void AddDataToExcel(long firstNum, long ?lastNum)
        {
            if (lastNum > 0)
            {
                xclSheet.Cells[rowNumber, 1] = firstNum.ToString();
                xclSheet.Cells[rowNumber, 2] = lastNum.ToString();
            }
            else
            {
                xclSheet.Cells[rowNumber, 1] = firstNum.ToString();
            }
            rowNumber++;
        }

        public void CloseExcelFile(string path)
        {
            Path = path;
            try
            {
                string format = xclFile.DefaultSaveFormat.ToString();

                xclWBook.SaveAs(GetNewPathName(), Excel.XlFileFormat.xlWorkbookNormal, missingValue, missingValue, missingValue, missingValue, 
                    Excel.XlSaveAsAccessMode.xlExclusive, missingValue, missingValue, missingValue, missingValue, missingValue);

                xclWBook.Close(true, path, missingValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                xclFile.Quit();
            }

            finally
            {
                if (xclFile != null)
                {
                    xclFile.Quit();
                    ReleaseObject(xclFile);
                }
            }
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("There was an error while releasing the object \n" + ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
