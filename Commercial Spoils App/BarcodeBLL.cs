using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;

namespace Commercial_Spoils_App
{
    class BarcodeBLL
    {
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

        private void ConnectToScanner()
        {
            try
            {
                temp.Close();
                //temp.DataReceived -= temp_DataReceived;
            }
            catch
            {
                //MessageBox.Show("Incorrect Parameters");
            }

            try
            {
                temp = new SerialPort();
                temp.PortName = "COM5";
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
                //temp.DataReceived += temp_DataReceived;
            }
            catch { }
        }

        private SerialPort temp;
        public bool isSingle = false;
        public bool isRangeFirstScan = false;
        public bool isFirstScanCaptured = true;

        //public void temp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    SerialPort port = (SerialPort)sender;
        //    string dataRead = ((SerialPort)sender).ReadLine();

        //    if (isSingle)
        //    {
        //        Dispatcher.BeginInvoke((Action)(() => txtSingleNum.Text = dataRead));
        //        Dispatcher.BeginInvoke((Action)(() => btnSubmitSingle_Click(null, null)));
        //    }
        //    else
        //    {
        //        if (isFirstScanCaptured)
        //        {
        //            isRangeFirstScan = true;
        //            Dispatcher.BeginInvoke((Action)(() => txtFirstNum.Text = dataRead));
        //            Dispatcher.BeginInvoke((Action)(() => txtLastNum.Focus()));
        //            isFirstScanCaptured = false;
        //        }
        //        else
        //        {
        //            Dispatcher.BeginInvoke((Action)(() => txtLastNum.Text = dataRead));
        //            Dispatcher.BeginInvoke((Action)(() => btnSubmitRange_Click(null, null)));
        //            isRangeFirstScan = false;
        //            isFirstScanCaptured = true;
        //        }
        //    }
        //}
    }
}
