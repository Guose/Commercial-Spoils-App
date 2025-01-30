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

        private bool firstFileCreated;

        public bool FirstFileCreated
        {
            get { return firstFileCreated; }
            set { firstFileCreated = value; }
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
        private string fileExists = string.Empty;


        internal void CreateExcelFile()
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
                firstFileCreated = true;                

                xclWBook.SaveAs(GetNewPathName(), Excel.XlFileFormat.xlWorkbookNormal, missingValue, missingValue, missingValue, missingValue, 
                    Excel.XlSaveAsAccessMode.xlExclusive, missingValue, missingValue, missingValue, missingValue, missingValue);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                fileExists = FileSave.AddSuffix(path, ".txt", true);
                CloseExcelFile();
            }
        }

        internal void DeleteExcelFile()
        {
            if (!File.Exists(fileExists))
            {
                if (excelFileName != string.Empty)
                {
                    ReleaseObject(xclFile);
                    ReleaseObject(xclSheet);
                    ReleaseObject(xclWBook);

                    File.Delete(excelFileName);
                }
            }
        }

        private string GetNewPathName()
        {
            if (excelFileName == string.Empty)
            {
                excelFileName = FileSave.AddSuffix(path, ".xls", firstFileCreated);
            }
                        
            FileInfo fi = new FileInfo(excelFileName);

            return fi.Directory.ToString() + "\\" + fi.Name;
        }


        internal void OpenExcelFile(long first, long last)
        {
            FirstNumber = first;
            LastNumber = last;
            firstFileCreated = false;

            xclFile = null;
            xclFile = new Excel.Application(); //create Excel App
            xclFile.DisplayAlerts = false; //turn off alerts

            Path = GetNewPathName();                       

            try
            {
                xclWBook = xclFile.Workbooks._Open(Path, missingValue, missingValue, missingValue, missingValue, missingValue, 
                    missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue);

                xclSheet = xclWBook.Worksheets[1];

                AppendDataToExcel(FirstNumber, LastNumber);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseExcelFile();
            }

        }

        private void AppendDataToExcel(long firstNum, long lastNum)
        {
            long count = 1;

            if (firstNum < lastNum)
            {
                xclSheet.Cells[rowNumber, 1] = firstNum.ToString();
                xclSheet.Cells[rowNumber, 2] = lastNum.ToString();
                count = lastNum - firstNum +1;
                xclSheet.Cells[rowNumber, 3] = count.ToString();
            }
            else
            {
                xclSheet.Cells[rowNumber, 1] = firstNum.ToString();
                xclSheet.Cells[rowNumber, 3] = count.ToString();
            }
            rowNumber++;
        }

        private void CloseExcelFile()
        {
                if (xclFile != null)
                {
                    xclWBook.Close(true, path, missingValue);
                    xclFile.Quit();
                    ReleaseObject(xclFile);
                    ReleaseObject(xclSheet);
                    ReleaseObject(xclWBook);
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
