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
    public enum DataGroup { Training = 0, Testing = 1, Validation = 2, All = 99 };

    public class AdvancedTickData
    {
        public long timestamp;
        public string instrument;

        [BsonId, JsonIgnore]
        public ObjectId _id;

        public Dictionary<string, double> values;

        [BsonIgnore, JsonIgnore]
        public bool changed = false;

        public DataGroup dataGroup;
        
        public AdvancedTickData(long timestamp, double last, double bid, double ask, string instrument)
        {
            this.timestamp = timestamp;

            this.values = new Dictionary<string, double>();
            values.Add("bid", bid);
            values.Add("ask", ask);
            values.Add("last", last);
            values.Add("mid", getMedianPrice());

            this.instrument = instrument;
        }

        public double getMedianPrice()
        {
            return (values["bid"] + values["ask"]) / 2;
        }

        public TickData ToTickdata()
        {
            return new TickData(timestamp, values["last"], values["bid"], values["ask"], instrument);
        }
    }
}
