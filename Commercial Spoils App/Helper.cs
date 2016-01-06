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
using System.Threading;
using Spoils.Scan;

namespace Commercial_Spoils_App
{
    class Helper
    {
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(DoEvents_DoNothing));
        }

        private static void DoEvents_DoNothing()
        {

        }


    

        //private bool isSingle = true;
        //private bool wasScanned = false;
        //private bool isFirstScanCaptured = false;
        //private bool isRangeFirstScan;        

        //private void temp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{

        //    ScanService _scanService = new ScanService();
        //    SerialPort port = (SerialPort)sender;
        //    string dataRead = ((SerialPort)sender).ReadLine();
        //    try
        //    {
        //        if (isSingle)
        //        {
        //            bool isGood = _scanService.ProcessScan(dataRead, true);
        //            if(!isGood)
        //            {
        //                //if last scan received
        //                _scanService.Finalize();

        //            }
        //                //show val errors


        //            Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => SingleScan = dataRead));
        //            //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => btnSubmitSingle_Click(null, null)));
        //            //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => SingleScan.Select(Substring(0, 10))));
        //            ////Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => txtSingleNum.SelectionLength = txtSingleNum.Text.Length));
        //            //wasScanned = true;
                    
        //        }
        //        else
        //        {
        //            _scanService.ProcessScan(dataRead, false);

        //            if (isFirstScanCaptured == false)
        //            {
        //                //isRangeFirstScan = true;
        //                //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => FirstNumberScanned = dataRead));
        //                ////Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => txtLastNum.Focus()));
        //                ////Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => txtLastNum.SelectionStart = 0));
        //                ////Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => txtLastNum.SelectionLength = txtLastNum.Text.Length));
        //                //isFirstScanCaptured = true;
        //            }
        //            else
        //            {
        //                //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => LastNumberScanned = dataRead));
        //                //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => btnSubmitRange_Click(null, null)));
        //                ////Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => txtFirstNum.SelectionStart = 0));
        //                ////Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => txtFirstNum.SelectionLength = txtFirstNum.Text.Length));
        //                //isRangeFirstScan = false;
        //                //isFirstScanCaptured = false;
        //                //wasScanned = true;
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //private Func<char, object> Substring(int v1, int v2)
        //{
        //    throw new NotImplementedException();
        //}

        //private void btnSubmitRange_Click(object p1, object p2)
        //{
        //    throw new NotImplementedException();
        //}

        //private void btnSubmitSingle_Click(object p1, object p2)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
