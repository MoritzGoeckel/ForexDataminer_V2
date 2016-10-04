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
            ranges.Add(0, new DistributionRange());
        }

        public DistributionRange getDecentRange()
        {
            if (ranges.Count == 0)
                throw new Exception("No range in this one... should have one...");

            if (ranges.ContainsKey(3))
                return ranges[3];

            if (ranges.ContainsKey(5))
                return ranges[5];

            if (ranges.ContainsKey(0))
                return ranges[0];

            return ranges.Values.First();
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
            ranges[0] = distCalcer.getRange().copy();

            distCalcer.dropPercent(3);
            ranges.Add(3, distCalcer.getRange().copy());

            //distCalcer.dropPercent(2);
            //ranges.Add(5, distCalcer.getRange().copy());

            //distCalcer.dropPercent(5);
            //ranges.Add(10, distCalcer.getRange().copy());

            distCalcer = null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void incOcurences(double value)
        {
            occurences++;
            ranges[0].checkValue(value);
        }

        public static string renderInfoList(List<DatasetInfo> infos)
        {
            StringBuilder dataInfoB = new StringBuilder("");
            string intendation = "     ";
            foreach (DatasetInfo info in infos)
            {
                dataInfoB.Append(info.id.getID() + Environment.NewLine);
                dataInfoB.Append(intendation + "Oc:" + info.occurences + Environment.NewLine);
                dataInfoB.Append(intendation + "In:" + info.instrument + Environment.NewLine);
                dataInfoB.Append(intendation + "Tf H:" + info.id.getTimeframeHours() + Environment.NewLine);
                dataInfoB.Append(intendation + "Tf M:" + info.id.getTimeframeMinutes() + Environment.NewLine);

                foreach (KeyValuePair<int, DistributionRange> range in info.ranges)
                    dataInfoB.Append(intendation + intendation + "Ra-" + range.Value.getDroppedPercent() + ":" + range.Value.getMin() + "~" + range.Value.getMax() + Environment.NewLine);

                dataInfoB.Append(Environment.NewLine);
            }

            return dataInfoB.ToString();
        }
    }
}
