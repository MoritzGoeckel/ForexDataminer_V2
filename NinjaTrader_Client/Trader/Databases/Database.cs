﻿using NinjaTrader_Client.Trader.Model;
using System.Collections.Generic;

namespace NinjaTrader_Client.Trader.MainAPIs
{
    public interface Database
    {
        Tickdata getPrice(long timestamp, string instrument, bool caching = true);
        List<Tickdata> getPrices(long startTimestamp, long endTimestamp, string instrument);
        void setPrice(Tickdata td, string instrument);
        void setData(TimeValueData data, string dataName, string instrument);
        TimeValueData getData(long timestamp, string dataName, string instrument);
        List<TimeValueData> getDataInRange(long startTimestamp, long endTimestamp, string dataName, string instrument);
        long getSetsCount();
        long getLastTimestamp();
        long getFirstTimestamp();

        void shutdown();

        //public abstract int getCacheingAccessPercent();
        //public abstract int getCacheFilledPercent();
        //private abstract Tickdata getPriceInternal(long timestamp, string instrument);
        //public abstract void exportData(long startTimestamp, string basePath);
        //private abstract BsonDocument getExportData(long startTime, MongoCollection<BsonDocument> collection);
        //public abstract void importData(string data);
        //public abstract void megrate();
    }
}
