using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;

namespace Commercial_Spoils_App
{
    public class RFIDReader
    {
        private SerialPort _serialPort;
        public event Action<string> OnTagRead; // Event to handle scanned RFID tags.

        public RFIDReader(string comPort, int baudRate = 9600)
        {
            _serialPort = new SerialPort(comPort, baudRate, Parity.None, 8, StopBits.One);
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string rfidTag = _serialPort.ReadLine().Trim();
                OnTagRead?.Invoke(rfidTag); // Fire event with scanned tag
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RFID Read Error: {ex.Message}");
            }
        }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
                Console.WriteLine($"Connected to RFID Reader on {_serialPort.PortName}");
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                Console.WriteLine("RFID Reader Disconnected.");
            }
        }
    }

    public class ComPort
    {
        private class ProcessConnection
        {
            public static ConnectionOptions ProcessConnectionOptions()
            {
                var options = new ConnectionOptions();
                options.Impersonation = ImpersonationLevel.Impersonate;
                options.Authentication = AuthenticationLevel.Default;
                options.EnablePrivileges = true;
                return options;
            }

            public static ManagementScope ConnectionScope(string machineName, ConnectionOptions options, string path)
            {
                var connectScope = new ManagementScope();
                connectScope.Path = new ManagementPath(@"\\" + machineName + path);
                connectScope.Options = options;
                connectScope.Connect();
                return connectScope;
            }
        }

        public class ComPortInfo
        {
            public string Name
            { get; set; }
            public string Description
            { get; set; }

            public ComPortInfo()
            { }

            public static IEnumerable<ComPortInfo> GetRFIDComPortsInfo()
            {
                var comPortInfoList = new List<ComPortInfo>();

                ConnectionOptions options = ProcessConnection.ProcessConnectionOptions();
                ManagementScope connectionScope = ProcessConnection.ConnectionScope(Environment.MachineName, options, @"\root\CIMV2");

                var objectQuery = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
                var comPortSearcher = new ManagementObjectSearcher(connectionScope, objectQuery);

                using (comPortSearcher)
                {
                    foreach (ManagementObject obj in comPortSearcher.Get())
                    {
                        if (obj != null)
                        {
                            object captionObj = obj["Caption"];
                            if (captionObj != null)
                            {
                                string caption = captionObj.ToString();

                                if (caption.Contains("(COM") && caption.ToLower().Contains("rfid")) // Look for RFID devices
                                {
                                    var comPortInfo = new ComPortInfo();
                                    comPortInfo.Name = caption.Substring(caption.LastIndexOf("(COM", StringComparison.Ordinal)).Replace("(", string.Empty).Replace(")", string.Empty);
                                    comPortInfo.Description = caption;
                                    comPortInfoList.Add(comPortInfo);
                                }
                            }
                        }
                    }
                }
                return comPortInfoList;
            }


            public static IEnumerable<ComPortInfo> GetComPortsInfo()
            {
                var comPortInfoList = new List<ComPortInfo>();

                ConnectionOptions options = ProcessConnection.ProcessConnectionOptions();
                ManagementScope connectionScope = ProcessConnection.ConnectionScope(Environment.MachineName, options, @"\root\CIMV2");

                var objectQuery = new ObjectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
                var comPortSearcher = new ManagementObjectSearcher(connectionScope, objectQuery);

                using (comPortSearcher)
                {
                    foreach (ManagementObject obj in comPortSearcher.Get())
                    {
                        if (obj != null)
                        {
                            object captionObj = obj["Caption"];
                            if (captionObj != null)
                            {
                                string caption = captionObj.ToString();

                                if (caption.Contains("(COM"))
                                {
                                    var comPortInfo = new ComPortInfo();
                                    comPortInfo.Name = caption.Substring(caption.LastIndexOf("(COM", StringComparison.Ordinal)).Replace("(", string.Empty).Replace(")", string.Empty);
                                    comPortInfo.Description = caption;
                                    comPortInfoList.Add(comPortInfo);
                                }
                            }
                        }
                    }
                }
                return comPortInfoList;
            }
        }
    }
}
