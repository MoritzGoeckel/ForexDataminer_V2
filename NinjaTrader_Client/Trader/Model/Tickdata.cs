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
    public class Tickdata
    {
        public long timestamp;
        public double bid, ask, last;

        [BsonId, JsonIgnore]
        public ObjectId _id;

        public Tickdata(long timestamp, double last, double bid, double ask)
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
    }
}
