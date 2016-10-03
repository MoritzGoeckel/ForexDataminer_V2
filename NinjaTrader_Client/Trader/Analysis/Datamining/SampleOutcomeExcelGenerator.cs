using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class SampleOutcomeExcelGenerator
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;

        Dictionary<string, Excel.Worksheet> sheets = new Dictionary<string, Excel.Worksheet>();
        Dictionary<string, int> sheetRows = new Dictionary<string, int>();

        string path;
        public SampleOutcomeExcelGenerator(string path)
        {
            this.path = path;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(System.Reflection.Missing.Value);
        }

        public Excel.Worksheet CreateSheet(string name)
        {
            Excel.Worksheet sheet = addWorksheet(name, xlWorkBook);
            sheets.Add(name, sheet);
            sheetRows.Add(name, 3);

            addHeaderRow(sheet);

            return sheet;
        }

        public void FinishSheet(string name)
        {
            Excel.Worksheet sheet = sheets[name];

            int rows = sheetRows[name];

            //Table
            FormatAsTable(sheet.get_Range("A2", "H" + (rows - 1)), "TheTable");

            //Chart
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)sheet.ChartObjects(Type.Missing);
            Excel.ChartObject chartObj = (Excel.ChartObject)xlCharts.Add(500, 10, 800, 300);
            chartObj.Chart.ChartType = Excel.XlChartType.xlXYScatterLinesNoMarkers;

            //Erste Series
            addChartSeries(chartObj, sheet.get_Range("A3", "A" + (rows - 1)), sheet.get_Range("G3", "G" + (rows - 1)), "avgActual");

            //Zweite Series
            addChartSeries(chartObj, sheet.get_Range("A3", "A" + (rows - 1)), sheet.get_Range("H3", "H" + (rows - 1)), "avgDiff");

            //Count Chart
            Excel.ChartObject countChart = (Excel.ChartObject)xlCharts.Add(500, 330, 800, 300);
            countChart.Chart.ChartType = Excel.XlChartType.xlXYScatterLinesNoMarkers;

            //Erste Series
            addChartSeries(countChart, sheet.get_Range("A3", "A" + (rows - 1)), sheet.get_Range("C3", "C" + (rows - 1)), "Count");
        }

        public void FinishDoc()
        {
            xlWorkBook.SaveAs(
                path,
                Excel.XlFileFormat.xlWorkbookNormal,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value,
                Excel.XlSaveAsAccessMode.xlExclusive,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value);

            xlWorkBook.Close(true, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            xlApp.Quit();

            foreach(KeyValuePair<string, Excel.Worksheet> pair in sheets)
                releaseObject(pair.Value);

            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        public void ShowDocument()
        {
            System.Diagnostics.Process.Start(path);
        }

        private Excel.Worksheet addWorksheet(string name, Excel.Workbook book)
        {
            Excel.Worksheet sheet;
            sheet = (Excel.Worksheet)book.Worksheets.Add();

            sheet.Name = name;

            sheet.Cells[1, 1] = name;

            return sheet;
        }

        private void addHeaderRow(string sheetName)
        {
            addHeaderRow(sheets[sheetName]);
        }

        private void addHeaderRow(Excel.Worksheet sheet)
        {
            sheet.Cells[2, 1] = "valueMin";
            sheet.Cells[2, 2] = "valueMax";
            sheet.Cells[2, 3] = "count";
            sheet.Cells[2, 4] = "maxAvg";
            sheet.Cells[2, 5] = "minAvg";
            sheet.Cells[2, 6] = "minVsMax";
            sheet.Cells[2, 7] = "actualAvg";
            sheet.Cells[2, 8] = "avgDiff";
        }

        public void addRow(string sheetName, double valueMin, double valueMax, int count, double maxAvg, double minAvg, double minVsMax, double actualAvg, double avgDiff)
        {
            addRow(sheets[sheetName], valueMin, valueMax, count, maxAvg, minAvg, minVsMax, actualAvg, avgDiff);
        }

        private void addRow(Excel.Worksheet sheet, double valueMin, double valueMax, int count, double maxAvg, double minAvg, double minVsMax, double actualAvg, double avgDiff)
        {
            int row = sheetRows[sheet.Name];

            if (row == 0)
                throw new Exception("Row cant be 0");

            sheet.Cells[row, 1] = GeneralExcelGenerator.getAsValidNumberString(valueMin);
            sheet.Cells[row, 2] = GeneralExcelGenerator.getAsValidNumberString(valueMax);
            sheet.Cells[row, 3] = GeneralExcelGenerator.getAsValidNumberString(count);
            sheet.Cells[row, 4] = GeneralExcelGenerator.getAsValidNumberString(maxAvg);
            sheet.Cells[row, 5] = GeneralExcelGenerator.getAsValidNumberString(minAvg);
            sheet.Cells[row, 6] = GeneralExcelGenerator.getAsValidNumberString(minVsMax);
            sheet.Cells[row, 7] = GeneralExcelGenerator.getAsValidNumberString(actualAvg);
            sheet.Cells[row, 8] = GeneralExcelGenerator.getAsValidNumberString(avgDiff);

            sheetRows[sheet.Name]++;
        }

        public void addChartSeries(Excel.ChartObject chartObj, Excel.Range xRange, Excel.Range yRange, string name)
        {
            Excel.Series ser = chartObj.Chart.SeriesCollection(System.Reflection.Missing.Value).NewSeries();

            ser.XValues = xRange;
            ser.Values = yRange;
            ser.Name = name;
        }

        public void FormatAsTable(Excel.Range SourceRange, string TableName)
        {
            Excel.ListObject table = SourceRange.Worksheet.ListObjects.Add(Excel.XlListObjectSourceType.xlSrcRange,
            SourceRange, System.Type.Missing, Excel.XlYesNoGuess.xlYes, System.Type.Missing);

            table.Name = TableName;
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
