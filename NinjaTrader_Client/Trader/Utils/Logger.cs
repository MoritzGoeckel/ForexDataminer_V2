using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
