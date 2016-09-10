using NinjaTrader_Client.Trader.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace NinjaTrader_Client.Trader.MainAPIs
{
    public class SQLiteDatabase
    {
        string myConnectionString;
        string path;

        public SQLiteDatabase(string path)
        {
            this.path = path;
            if (File.Exists(path) == false)
                throw new Exception("Database not found: " + path);

            myConnectionString = "Data Source=" + Config.sqlitePath + ";Version=3;Journal Mode=Off;Synchronous=Off;Cache_Size=1000000;Pooling=True;Max Pool Size=100";
        }

        private int timeout = 10;

        private SQLiteConnection getConnection()
        {
            SQLiteConnection con = new SQLiteConnection(myConnectionString);
            con.Open();

            return con;
        }

        private void minorErrorOccurred()
        {
            Thread.Sleep(500);
        }

        public TickData getPrice(long timestamp, string instrument, bool caching = true)
        {
            SQLiteConnection connection = null;
            TickData output = null;

            bool done = false;
            while (done == false)
            {
                try
                {
                    connection = getConnection();

                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM prices WHERE instrument = @instrument AND timestamp < @timestamp AND timestamp > @timestampMin ORDER BY timestamp DESC LIMIT 1", connection);
                    command.Parameters.AddWithValue("@timestamp", timestamp);
                    command.Parameters.AddWithValue("@instrument", instrument);
                    command.Parameters.AddWithValue("@timestampMin", timestamp - (3 * 60 * 1000));
                    command.Prepare();

                    command.CommandTimeout = timeout;

                    SQLiteDataReader Reader = command.ExecuteReader();

                    if (Reader.Read())
                        output = new TickData((long)(decimal)Reader["timestamp"], (double)Reader["last"], (double)Reader["bid"], (double)Reader["ask"], (string)Reader["instrument"]);
                    else
                        output = null;

                    Reader.Close();
                    done = true;
                }
                catch(Exception) { minorErrorOccurred(); }
                finally
                {
                    try { connection.Close(); } catch (Exception) { }
                }
            }

            return output;
        }

        public List<TickData> getPrices(long startTimestamp, long endTimestamp, string instrument)
        {
            List<TickData> output = new List<TickData>();
            SQLiteConnection connection = null;

            bool done = false;
            while (done == false)
            {
                try
                {
                    connection = getConnection();

                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM prices WHERE instrument = @instrument AND timestamp > @startTimestamp AND timestamp < @endTimestamp ORDER BY timestamp DESC", connection);
                    command.Parameters.AddWithValue("@endTimestamp", endTimestamp);
                    command.Parameters.AddWithValue("@startTimestamp", startTimestamp);
                    command.Parameters.AddWithValue("@instrument", instrument);
                    command.Prepare();

                    command.CommandTimeout = timeout;

                    SQLiteDataReader Reader = command.ExecuteReader();
                    while (Reader.Read())
                    {
                        long ts = (long)(decimal)Reader["timestamp"];
                        double bid = (double)Reader["bid"];

                        output.Add(new TickData(ts, (double)Reader["last"], bid, (double)Reader["ask"], (string)Reader["instrument"]));
                    }
                    Reader.Close();
                    done = true;
                }
                catch (Exception) { minorErrorOccurred();  }
                finally
                {
                    try { connection.Close(); } catch (Exception) { }
                }
            }

            return output;
        }

        public void setPrice(TickData td)
        {
            if (td.instrument == null)
                throw new Exception("Instrumen is null");

            SQLiteConnection connection = getConnection();

            SQLiteCommand command = new SQLiteCommand("INSERT INTO prices (instrument, timestamp, bid, ask, last) VALUES (@instrument, @timestamp, @bid, @ask, @last)", connection);
            command.Parameters.AddWithValue("@last", format(td.last));
            command.Parameters.AddWithValue("@ask", format(td.ask));
            command.Parameters.AddWithValue("@bid", format(td.bid));
            command.Parameters.AddWithValue("@timestamp", format(td.timestamp));
            command.Parameters.AddWithValue("@instrument", td.instrument);
            command.Prepare();

            command.CommandTimeout = timeout;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public TimeValueData getData(long timestamp, string dataName, string instrument)
        {
            SQLiteConnection connection = getConnection();

            SQLiteCommand command = new SQLiteCommand("SELECT * FROM timevaluepair WHERE instrument = @instrument AND name = @name AND timestamp < @timestamp AND timestamp > @timestampMin ORDER BY timestamp DESC LIMIT 1", connection);
            command.Parameters.AddWithValue("@timestamp", timestamp);
            command.Parameters.AddWithValue("@name", dataName);
            command.Parameters.AddWithValue("@instrument", instrument);
            command.Parameters.AddWithValue("@timestampMin", timestamp - (30 * 60 * 1000));
            command.Prepare();

            command.CommandTimeout = timeout;

            SQLiteDataReader Reader = command.ExecuteReader();

            TimeValueData output = null;

            if (Reader.Read())
                output =  new TimeValueData((long)(decimal)Reader["timestamp"], (double)Reader["value"]);

            Reader.Close();
            connection.Close();

            return output;
        }

        public List<TimeValueData> getDataInRange(long startTimestamp, long endTimestamp, string dataName, string instrument)
        {
            SQLiteConnection connection = getConnection();

            List<TimeValueData> output = new List<TimeValueData>();

            SQLiteCommand command = new SQLiteCommand("SELECT * FROM timevaluepair WHERE instrument = @instrument AND name = @name AND timestamp > @startTimestamp AND timestamp < @endTimestamp ORDER BY timestamp DESC", connection);
            command.Parameters.AddWithValue("@startTimestamp", startTimestamp);
            command.Parameters.AddWithValue("@endTimestamp", endTimestamp);
            command.Parameters.AddWithValue("@name", dataName);
            command.Parameters.AddWithValue("@instrument", instrument);
            command.Prepare();

            command.CommandTimeout = timeout;

            SQLiteDataReader Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                output.Add(new TimeValueData((long)(decimal)Reader["timestamp"], (double)Reader["value"]));
            }
            Reader.Close();
            connection.Close();

            return output;
        }

        public void setData(TimeValueData data, string dataName, string instrument)
        {
            SQLiteConnection connection = getConnection();

            SQLiteCommand command = new SQLiteCommand("INSERT INTO timevaluepair(instrument, name, timestamp, value) VALUES(@instrument, @dataName, @timestamp, @value)", connection);
            command.Parameters.AddWithValue("@value", format(data.value));
            command.Parameters.AddWithValue("@timestamp", data.timestamp);
            command.Parameters.AddWithValue("@dataName", dataName);
            command.Parameters.AddWithValue("@instrument", instrument);
            command.Prepare();

            command.CommandTimeout = timeout;

            command.ExecuteNonQuery();

            connection.Close();
        }

        public long getFirstTimestamp()
        {
            SQLiteConnection connection = getConnection();
            
            SQLiteCommand command = new SQLiteCommand("SELECT MIN(timestamp) FROM prices", connection);
            command.Prepare();

            command.CommandTimeout = timeout;

            SQLiteDataReader Reader = command.ExecuteReader();
            Reader.Read();
            long ts = (long)Reader["MIN(timestamp)"];
            Reader.Close();

            connection.Close();

            return ts;
        }

        public long getLastTimestamp()
        {
            SQLiteConnection connection = getConnection();

            SQLiteCommand command = new SQLiteCommand("SELECT MAX(timestamp) FROM prices", connection);
            command.Prepare();

            command.CommandTimeout = timeout;

            SQLiteDataReader Reader = command.ExecuteReader();
            Reader.Read();
            long output = (long)Reader["MAX(timestamp)"];
            Reader.Close();

            connection.Close();

            return output;
        }

        public long getSetsCount()
        {
            SQLiteConnection connection = getConnection();

            SQLiteCommand command = new SQLiteCommand("SELECT COUNT(*) FROM prices", connection);
            command.Prepare();

            command.CommandTimeout = timeout;

            SQLiteDataReader Reader = command.ExecuteReader();
            Reader.Read();

            long count = (long)Reader["COUNT(*)"];

            Reader.Close();
            connection.Close();

            return count;
        }

        private string format(double d)
        {
            return d.ToString().Replace(',', '.');
        }
    }
}
