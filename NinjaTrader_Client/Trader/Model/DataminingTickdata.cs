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
    public class DataminingTickdata
    {
        public long timestamp;

        [BsonId, JsonIgnore]
        public ObjectId _id;

        public Dictionary<string, double> values;

        [BsonIgnore, JsonIgnore]
        public bool changed = false;

        public DataminingTickdata(long timestamp, double last, double bid, double ask)
        {
            this.timestamp = timestamp;

            this.values = new Dictionary<string, double>();
            values.Add("bid", bid);
            values.Add("ask", ask);
            values.Add("last", last);
            values.Add("mid", getMedianPrice());
        }

        public double getMedianPrice()
        {
            return (values["bid"] + values["ask"]) / 2;
        }
    }
}
