using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Exceptions
{
    public class MyBaseException : Exception
    {
        public MyBaseException()
        {
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            Debug.WriteLine("############# Stacktrace #############");
            Debug.WriteLine(t.ToString());
            Debug.WriteLine("############# Stacktrace #############");
        }
    }
}
