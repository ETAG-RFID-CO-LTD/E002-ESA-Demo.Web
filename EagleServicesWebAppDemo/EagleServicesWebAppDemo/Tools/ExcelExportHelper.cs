using EagleServicesWebApp.Models.Enquiry;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;


namespace EagleServicesWebApp
{
   public class ExcelExportHelper
   {
      #region ExcelContentType
      public static string ExcelContentType
      {
         get
         { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
      }

        #endregion

        #region List to DataTable
        public static DataTable ListToDataTable<T>(List<T> data)
        {
            //string[] columns = { "ITEM#", "IPC REFERENCE", "NOMENCLATURE", "PART TYPE", "PWEL STANDARD", "PART STATUS", "VENDOR PN", "PW PN", "SN", "REMARKS", "Timestamp", "User", "Photo Link" };
            string[] columns = { "ITEM#", "IPC REFERENCE", "NOMENCLATURE", "PART TYPE", "PWEL STANDARD", "PART STATUS", "VENDOR PN", "PW PN", "SN", "REMARKS", "Timestamp", "User", "Status" };
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];

                //dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            for (int x = 0; x < columns.Length; x++)
            {
                dataTable.Columns.Add(columns[x]);
            }

            object[] values = new object[columns.Length];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        #endregion

        #region Export To Excel Data Table

        public static byte[] ExportExcel(List<Engine_REC> EngineData, DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {
            string PhotoLink = ConfigurationManager.AppSettings.Get("PhotoLink");
            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", "ItemExport"));
                workSheet.Cells["A:XFD"].Style.Font.Name = "Arial";
                workSheet.Cells["A:XFD"].Style.Font.Size = 10;

                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 5;

                int _ActualCurrentIndex = 0;

                #region First Grid
                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }

                // add the content into the Excel file
                //workSheet.Cells[startRowFrom, 1].Value = "GMS Details";
                //startRowFrom++;
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);


                // autofit width of cells with small content
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    //if (columnsToTake.Contains(column.ToString()))
                    //{
                    //   ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];

                    //   int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    //   if (maxLength < 12000)
                    //   {
                    workSheet.Column(columnIndex).AutoFit();
                    workSheet.Column(1).Width = 14.27;
                    workSheet.Column(2).Width = 26.82;
                    workSheet.Column(3).Width = 16.55;
                    workSheet.Column(4).Width = 16.18;
                    workSheet.Column(5).Width = 17.55;
                    workSheet.Column(6).Width = 16.45;
                    workSheet.Column(7).Width = 24;
                    workSheet.Column(8).Width = 23.64;
                    workSheet.Column(9).Width = 21;
                    workSheet.Column(10).Width = 22.73;
                    
                    //   }
                    //}

                    columnIndex++;
                }              

                // format header - bold, yellow on black
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1c4c74"));
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                //using (ExcelRange r = workSheet.Cells[startRowFrom, 1])
                //{
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                //}

                //using (ExcelRange r = workSheet.Cells[startRowFrom, 2])
                //{
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                //}

                //using (ExcelRange r = workSheet.Cells[startRowFrom, 7])
                //{
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                //}
                //using (ExcelRange r = workSheet.Cells[startRowFrom, 8])
                //{
                //    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                //}


                // format cells - add borders
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    // r.Formula = @"=HYPERLINK(""file:///C:\Users\Sandar\Desktop\AMS\"")";
                    //excel++;
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Gray);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Gray);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Gray);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Gray);

                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    r.Style.WrapText = true;
                }

                _ActualCurrentIndex = startRowFrom + dataTable.Rows.Count;

                // removed ignored columns
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        //workSheet.DeleteColumn(i + 1);
                    }
                }
                #endregion

                #region Heading
                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["B1"].Value = "ENGINE SERIAL NUMBER:";
                    workSheet.Cells["B1"].Style.Font.Size = 10;
                    workSheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    workSheet.Cells["C1"].Value = EngineData[0].EneSerialNo;
                    workSheet.Cells["C1"].Style.Font.Size = 10;
                    //workSheet.Cells["C1"].Style.Font.Bold = true;
                    //workSheet.Cells["A1:C1"].Merge = true;

                    workSheet.Cells["B2"].Value = "ENGINE MODEL:";
                    workSheet.Cells["B2"].Style.Font.Size = 10;
                    //workSheet.Cells["B2"].Style.Font.Bold = true;
                    workSheet.Cells["B2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    workSheet.Cells["C2"].Value = EngineData[0].Model;
                    workSheet.Cells["C2"].Style.Font.Size = 10;
                    //workSheet.Cells["C2"].Style.Font.Bold = true;
                    //workSheet.Cells["A1:C1"].Merge = true;

                    workSheet.Cells["B3"].Value = "ENGINE CSN:";
                    workSheet.Cells["B3"].Style.Font.Size = 10;
                    //workSheet.Cells["B3"].Style.Font.Bold = true;
                    workSheet.Cells["B3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    workSheet.Cells["C3"].Value = EngineData[0].CSN;
                    workSheet.Cells["C3"].Style.Font.Size = 10;
                    //workSheet.Cells["C3"].Style.Font.Bold = true;
                    //workSheet.Cells["A1:C1"].Merge = true;

                    workSheet.Cells["B4"].Value = "ENGINE TSN:";
                    workSheet.Cells["B4"].Style.Font.Size = 10;
                    //workSheet.Cells["B4"].Style.Font.Bold = true;
                    workSheet.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    workSheet.Cells["C4"].Value = EngineData[0].TSN;
                    workSheet.Cells["C4"].Style.Font.Size = 10;
                    //workSheet.Cells["C4"].Style.Font.Bold = true;
                    //workSheet.Cells["A1:C1"].Merge = true;

                }
                #endregion

                result = package.GetAsByteArray();
            }

            return result;
        }

      public static byte[] ExportExcelStockOut(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
      {
         byte[] result = null;
         using (ExcelPackage package = new ExcelPackage())
         {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", "StockOutEnquiry"));
            workSheet.Cells["A:XFD"].Style.Font.Name = "Arial";
            workSheet.Cells["A:XFD"].Style.Font.Size = 10;

            //int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;
            int startRowFrom = 1;
            int _ActualCurrentIndex = 0;

            #region First Grid
            if (showSrNo)
            {
               DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
               dataColumn.SetOrdinal(0);
               int index = 1;
               foreach (DataRow item in dataTable.Rows)
               {
                  item[0] = index;
                  index++;
               }
            }

            // add the content into the Excel file
            workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

            // autofit width of cells with small content
            int columnIndex = 1;
            foreach (DataColumn column in dataTable.Columns)
            {
               if (columnsToTake.Contains(column.ToString()))
               {
                  ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];

                  int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                  if (maxLength < 100000)
                  {
                     workSheet.Column(columnIndex).AutoFit();
                  }

               }
               columnIndex++;
            }

            // format header - bold, yellow on black
            using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
            {
               r.Style.Font.Color.SetColor(System.Drawing.Color.Black);
               r.Style.Font.Bold = true;
               r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
               r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
            }

            // format cells - add borders
            using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
            {
               r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
               r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
               r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
               r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

               r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Gray);
               r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Gray);
               r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Gray);
               r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Gray);
            }

            _ActualCurrentIndex = startRowFrom + dataTable.Rows.Count;

            // removed ignored columns
            for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
            {
               if (i == 0 && showSrNo)
               {
                  continue;
               }
               if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
               {
                  workSheet.DeleteColumn(i + 1);
               }
            }
            #endregion

            #region Heading
            //if (!String.IsNullOrEmpty(heading))
            //{

            //    workSheet.Cells["A1"].Value = "Stock Out Enquiry";
            //    workSheet.Cells["A1"].Style.Font.Size = 15;

            //    workSheet.InsertColumn(1, 1);
            //    workSheet.InsertRow(1, 1);
            //    workSheet.Column(1).Width = 2;


            //}
            #endregion
            result = package.GetAsByteArray();
         }

         return result;
      }
      #endregion

      #region ExportExcel

      public static byte[] ExchangeExportExcel<T>(List<Engine_REC> EngineData,List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
      {
         return ExportExcel(EngineData, ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
      }

      public static byte[] ExchangeExportExcelStockOut<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
      {
         return ExportExcelStockOut(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
      }
      #endregion
   }
}