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
    public class FXCMRatesDownloader
    {
        private Dictionary<string, TickData> saved_values = new Dictionary<string, TickData>();

        private Thread timerThread;

        private int interval = 1 * 5 * 1000;

        public int errors = 0;

        public delegate void SourceDataArrivedHandler(TickData data);
        public event SourceDataArrivedHandler sourceDataArrived;

        public FXCMRatesDownloader()
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
                try {
                    XDocument doc = XDocument.Load("http://rates.fxcm.com/RatesXML");

                    IEnumerable<XElement> childList =
                    from el in doc.Element("Rates").Elements()
                    select el;

                    foreach (XElement node in childList)
                    {
                        string instrument = node.Attribute("Symbol").Value;
                        TickData data = new TickData(Timestamp.getNow(), -1, Double.Parse(node.Element("Bid").Value.Replace(".", ",")), Double.Parse(node.Element("Ask").Value.Replace(".", ",")), instrument);

                        if (instrument == null)
                            throw new Exception("Instrument is null");

                        if (saved_values.ContainsKey(instrument) == false)
                        {
                            saved_values.Add(instrument, data);
                            if (sourceDataArrived != null)
                                sourceDataArrived(data);
                        }
                        else {
                            TickData oldData = saved_values[instrument];
                            if (oldData.ask != data.ask || oldData.bid != data.bid)
                            {
                                if (sourceDataArrived != null)
                                    sourceDataArrived(data);

                                saved_values[instrument] = data;
                            }
                        }
                    }
                }
                catch { }

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
