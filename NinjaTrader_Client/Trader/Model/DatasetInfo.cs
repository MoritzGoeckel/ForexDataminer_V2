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
    [Serializable]
    public class DatasetInfo
    {
        public string instrument;
        public DatasetId id;
        public int occurences = 0;

        [BsonId, JsonIgnore]
        public ObjectId _id;

        public Dictionary<string, DistributionRange> ranges = new Dictionary<string, DistributionRange>();

        public DatasetInfo(DatasetId id, string instrument)
        {
            this.id = id;
            this.instrument = instrument;
            ranges.Add("0", new DistributionRange());
        }

        public DistributionRange getDecentRange()
        {
            if (ranges.Count == 0)
                throw new Exception("No range in this one... should have one...");

            if (ranges.ContainsKey("3"))
                return ranges["3"];

            if (ranges.ContainsKey("5"))
                return ranges["5"];

            if (ranges.ContainsKey("0"))
                return ranges["0"];

            return ranges.Values.First();
        }

        public void SetRange(DistributionRange range, int droppedPercent)
        {
            if (ranges.ContainsKey(droppedPercent.ToString()) == false)
                ranges.Add(droppedPercent.ToString(), range);
            else
                ranges[droppedPercent.ToString()] = range;
        }

        [JsonIgnore, BsonIgnore, NonSerialized]
        private DistributionCalculater distCalcer = new DistributionCalculater();

        public void finishRangesCalculation()
        {
            ranges["0"] = distCalcer.getRange().copy();

            distCalcer.dropPercent(3);
            ranges.Add("3", distCalcer.getRange().copy());

            //distCalcer.dropPercent(2);
            //ranges.Add(5, distCalcer.getRange().copy());

            //distCalcer.dropPercent(5);
            //ranges.Add(10, distCalcer.getRange().copy());

            distCalcer = null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void incOcurences(double value, bool enlistForRangeCalculations = false)
        {
            occurences++;
            ranges["0"].checkValue(value);

            if (enlistForRangeCalculations)
                distCalcer.addValue(value);
        }

        public static string renderInfoList(List<DatasetInfo> infos)
        {
            if (infos == null)
                return "No data";

            StringBuilder dataInfoB = new StringBuilder("");
            string intendation = "     ";
            foreach (DatasetInfo info in infos)
            {
                dataInfoB.Append(info.id.id + Environment.NewLine);
                dataInfoB.Append(intendation + "Oc:" + info.occurences + Environment.NewLine);
                dataInfoB.Append(intendation + "In:" + info.instrument + Environment.NewLine);
                dataInfoB.Append(intendation + "Tf H:" + info.id.getTimeframeHours() + Environment.NewLine);
                dataInfoB.Append(intendation + "Tf M:" + info.id.getTimeframeMinutes() + Environment.NewLine);

                foreach (KeyValuePair<string, DistributionRange> range in info.ranges)
                    dataInfoB.Append(intendation + intendation + "Ra-" + range.Value.getDroppedPercent() + ":" + range.Value.getMin() + "~" + range.Value.getMax() + Environment.NewLine);

                dataInfoB.Append(Environment.NewLine);
            }

            return dataInfoB.ToString();
        }
    }
}
