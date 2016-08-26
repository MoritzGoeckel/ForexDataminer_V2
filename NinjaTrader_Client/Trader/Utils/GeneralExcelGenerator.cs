using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace NinjaTrader_Client.Trader.Datamining
{
    public class GeneralExcelGenerator
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;

        Dictionary<string, Excel.Worksheet> sheets = new Dictionary<string, Excel.Worksheet>();
        Dictionary<string, int> columnCountPerSheet = new Dictionary<string, int>();

        Dictionary<string, int> sheetRows = new Dictionary<string, int>();

        string path;
        public GeneralExcelGenerator(string path)
        {
            this.path = path;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(System.Reflection.Missing.Value);
        }

        public Excel.Worksheet CreateSheet(string name, string[] header)
        {
            Excel.Worksheet sheet = addWorksheet(name, xlWorkBook);
            sheets.Add(name, sheet);
            sheetRows.Add(name, 3);

            setHeaderRow(sheet, header);

            return sheet;
        }

        public void FinishSheet(string name)
        {
            Excel.Worksheet sheet = sheets[name];
            int rows = sheetRows[name];

            int cs = columnCountPerSheet[name];
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            FormatAsTable(sheet.get_Range("A2", alphabet.Substring(cs - 1, 1) + (rows - 1)), "TheTable");
        }

        public void save()
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

        private void setHeaderRow(string sheetName, string[] names)
        {
            setHeaderRow(sheets[sheetName], names);
        }

        private void setHeaderRow(Excel.Worksheet sheet, string[] names)
        {
            if (columnCountPerSheet.ContainsKey(sheet.Name))
                throw new Exception("Header is only possible to add once");

            columnCountPerSheet.Add(sheet.Name, names.Length);

            for (int colmn = 1; colmn < names.Length; colmn++)
                sheet.Cells[2, colmn] = names[colmn - 1];
        }

        public void addRow(string sheetName, string[] values)
        {
            addRow(sheets[sheetName], values);
        }

        private void addRow(Excel.Worksheet sheet, string[] values)
        {
            if (columnCountPerSheet[sheet.Name] != values.Length)
                throw new Exception("Values has to have the same amout of data then the header v" + values.Length + " h" + columnCountPerSheet[sheet.Name]);

            int row = sheetRows[sheet.Name];

            if (row == 0)
                throw new Exception("Row cant be 0");

            for (int colmn = 1; colmn < values.Length; colmn++)
                sheet.Cells[row, colmn] = values[colmn - 1];

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
