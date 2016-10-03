using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NinjaTrader_Client.Trader.Analysis.Datamining;
using NinjaTrader_Client.Trader.Datamining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader_Client.Trader.Model
{
    public class DatasetInfo
    {
        public string instrument;
        public DatasetId id;
        public int occurences = 0;

        [BsonId, JsonIgnore]
        public ObjectId _id;

        public Dictionary<int, DistributionRange> ranges = new Dictionary<int, DistributionRange>();

        public DatasetInfo(DatasetId id, string instrument)
        {
            this.instrument = instrument;
        }

        public void SetRange(DistributionRange range, int droppedPercent)
        {
            if (ranges.ContainsKey(droppedPercent) == false)
                ranges.Add(droppedPercent, range);
            else
                ranges[droppedPercent] = range;
        }

        [JsonIgnore, BsonIgnore, NonSerialized]
        private DistributionCalculater distCalcer = new DistributionCalculater();

        public void checkValueForRangesCalculation(double value)
        {
            distCalcer.addValue(value);
        }

        public void finishRangesCalculation()
        {
            ranges.Add(0, distCalcer.getRange().copy());

            distCalcer.dropPercent(3);
            ranges.Add(3, distCalcer.getRange().copy());

            //distCalcer.dropPercent(2);
            //ranges.Add(5, distCalcer.getRange().copy());

            //distCalcer.dropPercent(5);
            //ranges.Add(10, distCalcer.getRange().copy());

            distCalcer = null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void incOcurences()
        {
            occurences++;
        }
    }
}
