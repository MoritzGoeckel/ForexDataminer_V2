using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjaTrader_Client.Trader.Model;

namespace NinjaTrader_Client.Trader.Analysis.IndicatorCollections
{
    class FirstIndicatorCollection : IndicatorCollection
    {
        public override DataminingTickdata doCalculations(DataminingTickdata data)
        {
            throw new NotImplementedException(); //Todo: Calculate some indicators on the DataminingTickdata
        }
    }
}
