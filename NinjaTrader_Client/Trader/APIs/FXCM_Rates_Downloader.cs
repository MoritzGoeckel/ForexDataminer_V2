using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTrader_Client.Trader.Utils;
using NinjaTrader_Client.Trader.Model;
using System.Xml.Linq;

namespace NinjaTrader_Client.Trader
{
    public class FXCM_Rates_Downloader
    {
        private Dictionary<string, Tickdata> saved_values = new Dictionary<string, Tickdata>();

        private Thread timerThread;

        private int interval = 1 * 5 * 1000;

        public int errors = 0;

        public delegate void SourceDataArrivedHandler(Tickdata data, string instrument);
        public event SourceDataArrivedHandler sourceDataArrived;

        public FXCM_Rates_Downloader()
        {
            timerThread = new Thread(timedFunction);
        }

        public void start()
        {
            timerThread.Start();
        }

        bool resume = true;
        private void timedFunction()
        {
            while(resume)
            {
                XDocument doc = XDocument.Load("http://rates.fxcm.com/RatesXML");

                IEnumerable<XElement> childList =
                from el in doc.Element("Rates").Elements()
                select el;

                foreach (XElement node in childList)
                {
                    string instrument = node.Attribute("Symbol").Value;
                    Tickdata data = new Tickdata(Timestamp.getNow(), 0, Double.Parse(node.Element("Bid").Value.Replace(".", ",")), Double.Parse(node.Element("Ask").Value.Replace(".", ",")));

                    if (saved_values.ContainsKey(instrument) == false)
                    {
                        saved_values.Add(instrument, data);
                        if (sourceDataArrived != null)
                            sourceDataArrived(data, instrument);
                    }
                    else {
                        Tickdata oldData = saved_values[instrument];
                        if (oldData.ask != data.ask || oldData.bid != data.bid)
                        {
                            if (sourceDataArrived != null)
                                sourceDataArrived(data, instrument);

                            saved_values[instrument] = data;
                        }
                    }
                }

                Thread.Sleep(interval);
            }
        }

        public void stop()
        {
            resume = false;
            timerThread.Abort();
        }
    }
}
