using ClosedXML.Excel;
using EatAndDrink.Models;
using EatAndDrink.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace EatAndDrink.Services
{
    public class ExcelService
    {
        public FileContentResult ExportFull(List<Terminal> list)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Brands");

                worksheet.Cell("A1").Value = "Device code";
                worksheet.Cell("B1").Value = "Currency";
                worksheet.Cell("C1").Value = "Amount";
                worksheet.Cell("D1").Value = "Card number";
                worksheet.Cell("E1").Value = "Date";
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = list[i].DeviceCode;
                    worksheet.Cell(i + 2, 2).Value = list[i].Curr;
                    worksheet.Cell(i + 2, 3).Value = list[i].Amnt;
                    worksheet.Cell(i + 2, 4).Value = list[i].CardNumber;
                    worksheet.Cell(i + 2, 5).Value = list[i].OperDateTime;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"brands_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public FileContentResult ExportByCurrency(List<TotalByCurrency> list)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Brands");

                worksheet.Cell("A1").Value = "Device code";
                worksheet.Cell("B1").Value = "Total";
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = list[i].CardNumber;
                    worksheet.Cell(i + 2, 2).Value = list[i].Total;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"brands_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public FileContentResult ExportByTotalCurrency(List<TotalDevice> list)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Brands");

                worksheet.Cell("A1").Value = "Device code";
                worksheet.Cell("B1").Value = "Total KGS";
                worksheet.Cell("C1").Value = "Total EUR";
                worksheet.Cell("D1").Value = "Total KZT";
                worksheet.Cell("E1").Value = "Total USD";
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = list[i].DeviceCode;
                    worksheet.Cell(i + 2, 2).Value = list[i].TotalKgs;
                    worksheet.Cell(i + 2, 3).Value = list[i].TotalKzt;
                    worksheet.Cell(i + 2, 4).Value = list[i].TotalUsd;
                    worksheet.Cell(i + 2, 5).Value = list[i].TotalEur;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"brands_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public FileContentResult ExportTotal(List<TotalByCurrency> list)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Brands");

                worksheet.Cell("A1").Value = "Card Number";
                worksheet.Cell("B1").Value = "Total by KGS";
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = list[i].CardNumber;
                    worksheet.Cell(i + 2, 2).Value = list[i].Total;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"brands_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        //public List<Terminal> Import(HttpPostedFileBase fileExcel)
        //{
        //    List<Terminal> terminals = new List<Terminal>();
        //    Terminal viewModel = null;
        //    using (XLWorkbook workBook = new XLWorkbook(fileExcel, XLEventTracking.Disabled))
        //    {
        //        foreach (IXLWorksheet worksheet in workBook.Worksheets)
        //        {


        //            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
        //            {
        //                try
        //                {
        //                    viewModel = new Terminal();
        //                    viewModel.DeviceCode = Convert.ToInt32(row.Cell(1).Value);
        //                    viewModel.Curr = row.Cell(2).Value.ToString();
        //                    viewModel.Amnt = Convert.ToInt32(row.Cell(2).Value);
        //                    viewModel.CardNumber = Convert.ToInt32(row.Cell(2).Value);
        //                    viewModel.OperDateTime = Convert.ToDateTime(row.Cell(2).Value);
        //                }
        //                catch (Exception e)
        //                {
        //                    //logging
        //                }
        //                terminals.Add(viewModel);

        //            }
        //        }
        //    }
        //    return terminals;
        //}
    }
}
