using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Core.DataAccess.Excel
{
    public static class OpenXml
    {
        public static DataSet ReadFile(string filename)
        {
            byte[] bin = File.ReadAllBytes(filename);

            return ReadFile(bin);
        }

        public static DataSet ReadFile(byte[] filedata, bool headers = true)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            DataSet data = new DataSet();

            using (MemoryStream stream = new MemoryStream(filedata))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                DataTable dt;

                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    dt = new DataTable();

                    var ColumnCount = worksheet.Dimension.Columns;
                    var rowCount = worksheet.Dimension.Rows;

                    for (int j = 0; j < ColumnCount; j++)
                    {
                        var cell = worksheet.Cells[1, j + 1];
                        var column = new DataColumn(headers ? cell.Value?.ToString() : string.Empty);

                        cell = worksheet.Cells[2, j + 1]; //Look for data type

                        //if (cell.Style.Numberformat.Format.StartsWith("#.") || cell.Style.Numberformat.Format.StartsWith("#,"))
                        //    column.DataType = typeof(decimal);

                        dt.Columns.Add(column);
                    }

                    for (int i = 0; i < rowCount; i++)
                    {
                        var r = dt.NewRow();
                        for (int j = 0; j < ColumnCount; j++)
                        {
                            var cell = worksheet.Cells[i + 1 + (headers ? 1 : 0), j + 1];

                            //if (cell.Style.Numberformat.Format.StartsWith("#.") || cell.Style.Numberformat.Format.StartsWith("#,"))
                            //    r[j] = cell.Value ?? cell.Value;
                            //else
                            r[j] = cell.Value;
                        }
                        dt.Rows.Add(r);
                    }

                    data.Tables.Add(dt);
                }
            }

            return data;
        }

        /// <summary>
        /// Row Start Index => 1 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="startRow"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
       
        public static string[] ReadFile(string fileName, bool firstRow = false, string separator = "|")
        {
            //old readFile ya ignora la cabecera de la cabecera. Si, eso mismo.
            var ds = ReadFile(fileName);
            var rowsToSkip = firstRow ? 1 : 0;

            string[] parsedLines = ds.Tables[0].Rows
                .Cast<DataRow>()
                .AsEnumerable()
                .Skip(rowsToSkip)
                .Where(row => row.ItemArray.Any(cell => !string.IsNullOrEmpty(cell?.ToString())))
                .Select(row => string.Join(separator, row.ItemArray))
                .ToArray();

            return parsedLines;
        }
    }
}
