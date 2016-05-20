using NinjaTrader_Client.Trader.BacktestBase;
using NinjaTrader_Client.Trader.MainAPIs;
using NinjaTrader_Client.Trader.Model;
using NinjaTrader_Client.Trader.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NinjaTrader_Client.Trader.Backtests
{
    class DedicatedStrategyBacktestForm : BacktestForm
    {
        List<string> parameterList = new List<string>();
        int nextStratId = 0;

        List<Strategy> strats = new List<Strategy>();

        public DedicatedStrategyBacktestForm(Database database)
            : base(database, 3 * 30 * 24)
        {
            /*OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Config.startupPath;

            while (ofd.ShowDialog() != DialogResult.OK);

            parameterList = BacktestFormatter.getParametersFromFile(ofd.FileName);*/

            /*strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.2, 0.2, 21600000, 58, 62, 96, 100));
            strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.3, 0.3, 21600000, 58, 62, 96, 100));
            strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.4, 0.4, 21600000, 58, 62, 96, 100));
            strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.5, 0.5, 21600000, 58, 62, 96, 100));*/

            strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.6, 0.6, 21600000, 58, 62, 96, 100));
            strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.7, 0.7, 21600000, 58, 62, 96, 100));
            strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.8, 0.8, 21600000, 58, 62, 96, 100));
            strats.Add(new StochStrategy_OneSide(database, "EURUSD", 0.9, 0.9, 21600000, 58, 62, 96, 100));

        }

        protected override void backtestResultArrived(Dictionary<string, string> parameters, Dictionary<string, string> result)
        {
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void getNextStrategyToTest(ref Strategy strategy, ref long resolutionInSeconds, ref bool continueBacktesting)
        {
            
            resolutionInSeconds = 10;

            /*if (nextStratId < parameterList.Count)
                continueBacktesting = true;
            else
            {
                continueBacktesting = false;
                return;
            }

            BacktestFormatter.getStrategyFromString(database, parameterList[nextStratId], ref strategy);
            continueBacktesting = (strategy != null);*/

            if (nextStratId >= strats.Count)
            {
                continueBacktesting = false;
                return;
            }

            continueBacktesting = true;
            strategy = strats[nextStratId];
            
            nextStratId++;
        }
    }
}
