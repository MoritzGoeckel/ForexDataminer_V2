using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Model
{
    public class TickData
    {
        public long timestamp;
        public double bid, ask, last;
        public string instrument;

        [BsonId, JsonIgnore]
        public ObjectId _id;

        public TickData(long timestamp, double last, double bid, double ask, string instrument)
        {
            this.timestamp = timestamp;
            this.bid = bid;
            this.ask = ask;
            this.last = last;
        }

        public double getAvgPrice()
        {
            return (ask + bid) / 2;
        }

        internal AdvancedTickData toAdvancedTickData()
        {
            return new AdvancedTickData(timestamp, last, bid, ask, instrument);
        }
    }
}
