using ClosedXML.Excel;
using EatAndDrink.Models;
using EatAndDrink.Services;
using EatAndDrink.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;


namespace EatAndDrink.Controllers
{
    public class TerminalController : Controller
    {
        private TerminalService terminalService = new TerminalService();
        private ExcelService excelService;
        private static TerminalDBContext TerminalDB;
        private List<Terminal> listOfTerminal;

        [HttpGet]
        public IActionResult Index()
        {
            TerminalDB = new TerminalDBContext();
            return View(TerminalDB.Terminal.ToList());
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult currencyKGS()
        {
            return View(terminalService.totalByCurrency("KGS"));
        }

        public IActionResult currencyEUR()
        {

            return View(terminalService.totalByCurrency("EUR"));
        }

        public IActionResult currencyUSD()
        {
            return View(terminalService.totalByCurrency("USD"));
        }

        public IActionResult currencyKZT()
        {
            return View(terminalService.totalByCurrency("KZT"));
        }

        public IActionResult totalAmount()
        {

            return View(terminalService.amountByCardNumber());
        }
        [HttpGet]
        public IActionResult Report()
        {
            return View(terminalService.totalByDevice().ToList());
        }

        [HttpGet]
        public IActionResult More(int card)
        {
            try
            {
                listOfTerminal = terminalService.moreAboutCardNumber(card);
                Console.WriteLine(listOfTerminal.Count);
                if (listOfTerminal.Count == 0)
                {
                    return RedirectToAction("NotFound");
                }
                return View(listOfTerminal);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound");
            }
        }


        [HttpGet]
        public IActionResult Filter(string filterBy, string filter)
        {
            try
            {
                listOfTerminal = terminalService.filter(filterBy, filter);
                return View(listOfTerminal);
            }
            catch (Exception e)
            {
                return RedirectToAction("NotFound");
            }
        }

        public ActionResult ExportFullReport(ExcelService excelService)
        {
            ExcelService excelServic = new ExcelService();
            return excelServic.ExportFull(TerminalDB.Terminal.ToList());
        }

        public ActionResult ExportByTotalCurrency()
        {
            excelService = new ExcelService();
            return excelService.ExportByTotalCurrency(terminalService.totalByDevice());
        }

        public IActionResult TotalCurrency(string totalBy)
        {
            return View(terminalService.totalByCurrency(totalBy));
        }
        public ActionResult ExportByKGS()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency("KGS"));
        }

        public ActionResult ExportByKZT()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency("KZT"));
        }

        public ActionResult ExportByUSD()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency("USD"));
        }

        public ActionResult ExportByEUR()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency("EUR"));
        }

        public ActionResult ExportTotal()
        {
            excelService = new ExcelService();
            return excelService.ExportTotal(terminalService.amountByCardNumber());
        }


        public async Task<List<Terminal>> test(IFormFile file)
        {
            var list = new List<Terminal>();
            Terminal term = new Terminal();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowcount; row++)
                    {
                        
                        term = new Terminal()
                        {
                            DeviceCode = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                            Curr = Convert.ToString(worksheet.Cells[row, 2].Value),
                            Amnt = Convert.ToDouble(worksheet.Cells[row, 3].Value),
                            CardNumber = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                            OperDateTime = Convert.ToDateTime(worksheet.Cells[row, 5].Value)
                        };
                        list.Add(term);
                    }
                }
            }
            return list;
        }



        //public List<Terminal> test(IFormFile fileExcel)
        //{
        //    List<Terminal> terminals = new List<Terminal>();
        //    Terminal viewModel = null;
        //    using (XLWorkbook workBook = new XLWorkbook(fileExcel.OpenReadStream(), XLEventTracking.Disabled))
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
