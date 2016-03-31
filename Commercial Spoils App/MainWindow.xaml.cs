using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;
using System.Data;
using System.IO.Ports;
using SpoilsReportData;

namespace Commercial_Spoils_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartUpView();
            GetCOMPortName();
            
            DragEnter += new DragEventHandler(Form1_DragEnter);
            Drop += new DragEventHandler(Form1_DragDrop);            
        }

        private DataTable dt = new DataTable();
        private DataTable dt2 = new DataTable();
        private ExcelFile ex = new ExcelFile();
        

        #region DragNDrop

        void Form1_DragEnter(object sender, DragEventArgs e)
        {   
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            lblFileLoaded.Visibility = Visibility.Hidden;
        }

        
        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            string[] filepaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in filepaths)
            {
                if (File.Exists(file))
                {
                    using (TextReader tr = new StreamReader(file))
                    {
                        lblHeader.Content = tr.ReadLine();
                        lblPath.Content = file;
                    }
                }
            }

            cboComPort.Visibility = Visibility.Visible;
            lblScannerCOM.Visibility = Visibility.Visible;
            lblDragFileHere.Visibility = Visibility.Hidden;
            lblFileLoaded.Visibility = Visibility.Visible;
            dt = Seperator.DataFromTextFile(lblPath.Content.ToString(), '|');
            dt2 = dt.Clone();
            ex = new ExcelFile(lblPath.Content.ToString());

            ex.CreateExcelFile();
            Application.Current.MainWindow.Width = 1250;
            Cursor = Cursors.Arrow;
        }
        #endregion DragNDrop

        #region ScanningMethods
        public void SelectScanningMode(string scanType)
        {
            bool scanAddSpoil = false;
            bool scanRemoveSpoil = false;
            bool exitScanMode = false;
            bool insertionScan = false;

            if (scanType == "scanAddSpoil")
                scanAddSpoil = true;

            if (scanType == "scanRemoveSpoil")
                scanRemoveSpoil = true;

            if (scanType == "exitScanMode")
                exitScanMode = true;

            if (scanType == "insertionScan")
                insertionScan = true;

            ConnectToScanner();
        }

        private string colBCName = "";
        private SerialPort temp;
        public bool isSingle = false;
        public bool isRangeFirstScan = false;
        public bool isFirstScanCaptured = false;
        public bool wasScanned = false;

        public List<string> comPorts = new List<string>();


        private void GetCOMPortName()
        {                
                foreach (ComPort.ComPortInfo comPort in ComPort.ComPortInfo.GetComPortsInfo())
                {                    
                    comPorts.Add(string.Format("{0}-{1}", comPort.Name, comPort.Description));
                }
                cboComPort.ItemsSource = comPorts;            
        }

        private void ConnectToScanner()
        {
            try
            {
                temp.Close();
                temp.DataReceived -= temp_DataReceived;
            }
            catch {}          

            try
            {
                string selectedItem = cboComPort.Text;
                string value = selectedItem.Substring(0, 4);
                temp = new SerialPort();
                //temp.PortName = Properties.Settings.Default.PortName;
                temp.PortName = value;
                temp.DataBits = int.Parse("8");
                temp.BaudRate = int.Parse("115200");
                temp.Parity = Parity.None;
                temp.StopBits = StopBits.One;
                temp.DiscardNull = false;
                temp.NewLine = "\r\n";
                //temp.ReceivedBytesThreshold = 1;
                temp.Handshake = Handshake.None;
                temp.Open();
                temp.DiscardInBuffer();
                temp.DataReceived += temp_DataReceived;
            }
            catch
            {
                MessageBoxResult connectCom = MessageBox.Show("Are you using a Barcode Scanner?", "Scanner is NOT Detected", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (connectCom == MessageBoxResult.Yes)
                {
                    MessageBoxResult message = MessageBox.Show("Please select a COM Port to use scanner", "Select COM Port", MessageBoxButton.OK, MessageBoxImage.Hand);
                    if (message == MessageBoxResult.OK)
                    {
                        ChangeVisibility(true);
                        Application.Current.MainWindow.Width = 1250;
                    }

                }
                else
                { }
            }            
        }
                

        private void temp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            string dataRead = ((SerialPort)sender).ReadLine();
            try
            {
                if (isSingle)
                {
                    Dispatcher.BeginInvoke((Action)(() => txtSingleNum.Text = dataRead));
                    Dispatcher.BeginInvoke((Action)(() => btnSubmitSingle_Click(null, null)));
                    Dispatcher.BeginInvoke((Action)(() => txtSingleNum.SelectionStart = 0));
                    Dispatcher.BeginInvoke((Action)(() => txtSingleNum.SelectionLength = txtSingleNum.Text.Length));
                    wasScanned = true;
                }
                else
                {
                    if (isFirstScanCaptured == false)
                    {
                        isRangeFirstScan = true;
                        Dispatcher.BeginInvoke((Action)(() => txtFirstNum.Text = dataRead));
                        Dispatcher.BeginInvoke((Action)(() => txtLastNum.Focus()));
                        Dispatcher.BeginInvoke((Action)(() => txtLastNum.SelectionStart = 0));
                        Dispatcher.BeginInvoke((Action)(() => txtLastNum.SelectionLength = txtLastNum.Text.Length));
                        isFirstScanCaptured = true;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => txtLastNum.Text = dataRead));
                        Dispatcher.BeginInvoke((Action)(() => btnSubmitRange_Click(null, null)));
                        Dispatcher.BeginInvoke((Action)(() => txtFirstNum.SelectionStart = 0));
                        Dispatcher.BeginInvoke((Action)(() => txtFirstNum.SelectionLength = txtFirstNum.Text.Length));
                        isRangeFirstScan = false;
                        isFirstScanCaptured = false;
                        wasScanned = true;
                    }
                }
            }
            catch { }
        }

        #endregion ScanningMethods

        #region ButtonClicks
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult clearWarning = MessageBox.Show("Are you sure you want to clear the data?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (clearWarning == MessageBoxResult.Yes)
            {
                ClearAllData();
                lblDragFileHere.Visibility = Visibility.Visible;
                lblFileLoaded.Visibility = Visibility.Hidden;
                btnCompleteRange.Visibility = Visibility.Hidden;
                btnCompleteSingle.Visibility = Visibility.Hidden;
                btnSave.IsEnabled = false;
                ChangeVisibility(true);
            }
        }

        private void btnSingle_Click(object sender, RoutedEventArgs e)
        {
            isSingle = true;
            ChangeVisibility(false);
            stkSingle.Visibility = Visibility.Visible;
            txtSingleNum.Focus();
            SelectScanningMode("scanAddSpoil");
        }

        private void btnRange_Click(object sender, RoutedEventArgs e)
        {
            isSingle = false;
            ChangeVisibility(false);
            stkRange.Visibility = Visibility.Visible;
            txtFirstNum.Focus();
            SelectScanningMode("scanAddSpoil");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {            
            ClearAllData();
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string path = lblPath.Content.ToString();
            string header = lblHeader.Content.ToString();
            DataTable dtAsc = dt2.Clone();
            SpoilsData spoilsData = null;

            try
            {
                if (dtAsc.Columns[0].ColumnName == "KEY")
                {
                    FileSave.RemoveDuplicateRows(dt2, "KEY");
                    dtAsc = FileSave.SortAscending("KEY", dt2);
                }
                else if (dtAsc.Columns[0].ColumnName == "Count")
                {
                    FileSave.RemoveDuplicateRows(dt2, "Count");
                    dtAsc = FileSave.SortAscending("Count", dt2);
                }
                else if (dtAsc.Columns[1].ColumnName == "Sequence")
                {
                    FileSave.RemoveDuplicateRows(dt2, "Sequence");
                    dtAsc = FileSave.SortAscending("Sequence", dt2);
                }
                else
                {
                    FileSave.RemoveDuplicateRows(dt2, "Seq");
                    dtAsc = FileSave.SortAscending("Seq", dt2);
                }

                string newFilename = FileSave.SaveSpoilsFile(path, dtAsc, header);

                var lines = File.ReadAllLines(newFilename);
                File.WriteAllLines(newFilename, lines.Take(lines.Length - 1).ToArray());
                int recordCount = dtAsc.Rows.Count;
                bool isNewMailing = false;
                spoilsData = new SpoilsData(dtAsc);

                MessageBoxResult processQuestion = MessageBox.Show("Are you processing as new mailing?", "??? IS THIS A NEW MAILING ???", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (processQuestion != MessageBoxResult.Cancel)
                {
                    if (processQuestion == MessageBoxResult.Yes)
                    {
                        isNewMailing = true;
                        isNewMailing = spoilsData.NewMailing;
                    }

                    FileInfo emailFile = new FileInfo(newFilename);
                    FileSave.SendEmail_Outlook(emailFile, isNewMailing, recordCount);
                    MessageBox.Show(recordCount + " records have been saved!  \n\nEmail has been sent.", "'" + recordCount + "'" + " - RECORD(s) EXPORTED", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(recordCount + " records have been saved!", "'" + recordCount + "'" + " - RECORD(s) EXPORTED", MessageBoxButton.OK);
                }
                //spoilsData.PopulateReportDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ClearAllData();
                ChangeVisibility(true);
                
            }
        }

        private void btnCompleteRange_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibility(true);
        }

        private void btnCompleteSingle_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibility(true);
        }

        private void btnSubmitRange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long firstNum = long.Parse(txtFirstNum.Text);
                long lastNum = long.Parse(txtLastNum.Text);

                if (firstNum.ToString().Length <= 10 || lastNum.ToString().Length <= 10)
                {
                    dt2 = GetBarcode(colBCName, firstNum, lastNum);
                    spoilsGrid.DataContext = dt2;
                    GetCountOfDataGrid();
                    btnCompleteRange.Visibility = Visibility.Visible;
                    spoilsGrid.Visibility = Visibility.Visible;
                    lblFocusToBottom.Visibility = Visibility.Visible;
                    lblFocusToTop.Visibility = Visibility.Visible;
                    spoilsGrid.ScrollIntoView(spoilsGrid.Items[spoilsGrid.Items.Count - 2]);
                    txtFirstNum.Focus();
                    txtFindRec.Visibility = Visibility.Visible;
                    btnSubmitRange.IsEnabled = false;
                    btnSave.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Length of characters exceed allowed amount", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nRecord Number does not exist\n\nCheck to make sure data has been loaded to the program", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                ChangeVisibility(true);
            }
        }

        private void btnSubmitSingle_Click(object sender, RoutedEventArgs e)
        {
            long number = long.Parse(txtSingleNum.Text);

            try
            {
                if (number.ToString().Length <= 10)
                {
                    dt2 = GetBarcode(colBCName, number, number);
                    spoilsGrid.DataContext = dt2;
                    spoilsGrid.ScrollIntoView(spoilsGrid.Items[spoilsGrid.Items.Count - 2]);
                    GetCountOfDataGrid();
                    btnCompleteSingle.Visibility = Visibility.Visible;
                    spoilsGrid.Visibility = Visibility.Visible;
                    lblFocusToBottom.Visibility = Visibility.Visible;
                    lblFocusToTop.Visibility = Visibility.Visible;
                    btnSave.IsEnabled = true;
                    txtSingleNum.SelectionStart = 0;
                    txtSingleNum.SelectionLength = txtSingleNum.Text.Length;
                    txtFindRec.Visibility = Visibility.Visible;
                    btnSubmitSingle.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Length of characters exceed allowed amount ", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nRecord Number does not exist\n\nCheck to make sure data has been loaded to the program", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                ChangeVisibility(true);
            }
        }

        #endregion ButtonClicks

        #region Methods

        void GetCountOfDataGrid()
        {
            int count = spoilsGrid.Items.Count -1;
            lblDisplayCount.Content = count.ToString();
        }

        void ClearAllData()
        {
            lblHeader.Content = "";
            lblPath.Content = "";
            txtFirstNum.Text = "";
            txtLastNum.Text = "";
            txtSingleNum.Text = "";
            txtFindRec.Text = "Find";
            txtFindRec.Foreground = new SolidColorBrush(Colors.Gray);
            txtFindRec.Visibility = Visibility.Hidden;
            spoilsGrid.DataContext = null;
            dt = null;
            dt2 = null;
            spoilsGrid.Visibility = Visibility.Hidden;
            lblFocusToBottom.Visibility = Visibility.Hidden;
            lblFocusToTop.Visibility = Visibility.Hidden;
            cboComPort.Visibility = Visibility.Hidden;
            lblScannerCOM.Visibility = Visibility.Hidden;
            lblDragFileHere.Visibility = Visibility.Visible;
            lblFileLoaded.Visibility = Visibility.Hidden;
        }

        private void ChangeVisibility(bool original = false)
        {
            if (original == false)
            {
                Application.Current.MainWindow.Height = 600;
                Application.Current.MainWindow.Width = 1250;
                btnSingle.Visibility = Visibility.Hidden;
                btnRange.Visibility = Visibility.Hidden;
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                Application.Current.MainWindow.Height = 300;
                Application.Current.MainWindow.Width = 425;
                btnSingle.Visibility = Visibility.Visible;
                btnRange.Visibility = Visibility.Visible;
                btnBack.Visibility = Visibility.Hidden;
                stkRange.Visibility = Visibility.Hidden;
                stkSingle.Visibility = Visibility.Hidden;
                txtFirstNum.Text = "";
                txtLastNum.Text = "";
                txtSingleNum.Text = "";

                if (btnSubmitRange.IsEnabled == false || btnSubmitSingle.IsEnabled == false)
                {
                    btnCompleteRange.Visibility = Visibility.Hidden;
                    btnCompleteSingle.Visibility = Visibility.Hidden;
                }
            }
        }

        private void StartUpView()
        {
            spoilsGrid.Visibility = Visibility.Hidden;
            lblFileLoaded.Visibility = Visibility.Hidden;
            Height = 300;
            Width = 425;
            btnBack.Visibility = Visibility.Hidden;
            stkRange.Visibility = Visibility.Hidden;
            stkSingle.Visibility = Visibility.Hidden;
        }


        private DataTable GetBarcode(string ColumnToSearch, long firstNum, long lastNum)
        {
            //Check Condition for which record was entered first - big number or small number
            long fNumScanned;
            long lNumScanned;
            string IdUniqueToFind = string.Empty;
            bool doMainBreak = false;  

            if (lastNum < firstNum)
            {
                fNumScanned = lastNum;
                lNumScanned = firstNum;
            }
            else
            {
                fNumScanned = firstNum;
                lNumScanned = lastNum;
            }

            ex.OpenExcelFile(fNumScanned, lNumScanned);
            
            lNumScanned = lNumScanned + 1;

                if (wasScanned == false)
                {
                    IdUniqueToFind = fNumScanned.ToString();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName.Contains("SEQUENCE") || dc.ColumnName.Contains("Seq") || dc.ColumnName.Contains("Count") || dc.ColumnName.Contains("SEQ"))
                        {
                            ColumnToSearch = dc.ColumnName.ToString();

                            foreach (DataRow drv in dt.Rows)
                            {
                                if (drv[ColumnToSearch].ToString() == fNumScanned.ToString())
                                {
                                    dt2.Rows.Add(drv.ItemArray);


                                    fNumScanned++;
                                    if (fNumScanned >= lNumScanned)
                                    {
                                        doMainBreak = true;
                                        break;
                                    }
                                }
                            }
                            if (doMainBreak)
                                break;
                        }
                    }
                }
                else
                {
                    IdUniqueToFind = fNumScanned.ToString().PadLeft(10, '0');
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (doMainBreak)
                            break;
                        if (dc.ColumnName.Contains("BarCode") || dc.ColumnName.Contains("BARCODE") || dc.ColumnName.Contains("BALLOTBC") || dc.ColumnName.Contains("LETTER_2D")) //
                        {
                            ColumnToSearch = dc.ColumnName.ToString();
                            foreach (DataRow drv in dt.Rows)
                            {
                                if (drv[ColumnToSearch].ToString() == fNumScanned.ToString().PadLeft(10, '0'))
                                {
                                    dt2.Rows.Add(drv.ItemArray);
                                    fNumScanned++;
                                    if (fNumScanned >= lNumScanned)
                                    {
                                        doMainBreak = true;
                                        break;
                                    }
                                }
                            }
                            if (doMainBreak)
                                break;
                        }
                    }
                }
            return dt2;
        }

        #endregion Methods

        #region WindowAttributes

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ex.DeleteExcelFile();
        }

        void txtFirstNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               txtLastNum.SelectionStart = 0;
               txtLastNum.SelectionLength = txtLastNum.Text.Length;
                wasScanned = false;
                txtLastNum.Focus();                
            }
        }

        private void txtLastNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {                
                txtFirstNum.SelectionStart = 0;
                txtFirstNum.SelectionLength = txtFirstNum.Text.Length;
            }
        }

        private void txtLastNum_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSubmitRange.IsEnabled = true;
            }
        }

        private void txtSingleNum_KeyUp(object sender, KeyEventArgs e)
        {
            btnSubmitSingle.IsEnabled = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibility(true);
        }

        private void lblFocusToBottom_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void lblFocusToBottom_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void txtFindRec_KeyUp(object sender, KeyEventArgs e)
       {
            try
            {
                if (e.Key == Key.Enter)
                {
                    int findRec = int.Parse(txtFindRec.Text);
                    foreach (DataRowView row in spoilsGrid.Items)
                    {
                        if (row.Row.ItemArray[0].ToString() == findRec.ToString())
                        {
                            spoilsGrid.SelectedIndex = spoilsGrid.Items.IndexOf(row);
                            spoilsGrid.ScrollIntoView(row);
                            spoilsGrid.SelectedItem = spoilsGrid.Items.IndexOf(row);                            
                            break;
                        }
                    }
                    txtFindRec.SelectionStart = 0;
                    txtFindRec.SelectionLength = txtFindRec.Text.Length;
                }
            }
            catch { MessageBox.Show("Record not found"); }

        }

        private void lblFocusToBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            spoilsGrid.ScrollIntoView(spoilsGrid.Items[spoilsGrid.Items.Count - 2]);
        }

        private void lblFocusToTop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            spoilsGrid.ScrollIntoView(spoilsGrid.Items[spoilsGrid.SelectedIndex = 0]);
        }

        private void txtFindRec_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtFindRec.Text = "";
            txtFindRec.Foreground = Brushes.Black;
        }

        private void cboComPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            Properties.Settings.Default.PortName = cboComPort.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cboComPort.SelectedIndex = 1;
        }


        #endregion WindowAttributes
    }
}
