using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Utils
{
    public static class Logger
    {
        public static bool enableLogging = true;
        public static void log(string msg, string method = "")
        {
            if(enableLogging)
                Debug.WriteLine(DateTime.Now.ToShortTimeString() + " - " + msg + (method != "" ? " - " + method : "");
        }
    }
}
