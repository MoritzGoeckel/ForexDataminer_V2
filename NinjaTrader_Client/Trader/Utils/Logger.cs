using System;
using System.Collections.Specialized;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NinjaTrader_Client.Trader.Utils
{
    public static class Logger
    {
        public static bool enableLogging = true;
        public static string logPath = null;

        public static void log(string msg, string method = "")
        {
            if (enableLogging)
            {
                string line = DateTime.Now.ToShortTimeString() + " - " + msg + (method != "" ? " - " + method : "");
                Debug.WriteLine(line);

                if (logPath != null)
                    File.AppendAllText(logPath, line + Environment.NewLine);
            }
        }

        public static void sendImportantMessage(string msg)
        {
            new Thread(delegate () {
                try {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://moritzg.serpens.uberspace.de/n/insertmsg/1"); //Todo: put in config
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\"userid\":-1," +
                                        "\"content\":\"" + msg + "\"}";

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
                catch
                {
                    //I guess that is okay
                }
            }).Start();
        }
    }
}
