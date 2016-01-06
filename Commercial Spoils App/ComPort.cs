using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Commercial_Spoils_App
{
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
