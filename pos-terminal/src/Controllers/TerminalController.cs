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

        private static TerminalDBContext TerminalDB = new TerminalDBContext();

        private static List<Terminal> listOfTerminal = TerminalDB.Terminal.ToList();

        [HttpGet]
        public IActionResult Index()
        {
            CurrencyParse c = new CurrencyParse();
            TerminalDB = new TerminalDBContext();
            listOfTerminal = TerminalDB.Terminal.ToList();
            return View(listOfTerminal);
        }

        public IActionResult Import(IFormFile file)
        {
            excelService = new ExcelService();
            listOfTerminal = excelService.ImportExcel(file);
            return View(listOfTerminal);
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult currencyKGS()
        {
            return View(terminalService.totalByCurrency(listOfTerminal,"KGS"));
        }

        public IActionResult currencyEUR()
        {
            return View(terminalService.totalByCurrency(listOfTerminal,"EUR"));
        }

        public IActionResult currencyUSD()
        {
            return View(terminalService.totalByCurrency(listOfTerminal,"USD"));
        }

        public IActionResult currencyKZT()
        {
            return View(terminalService.totalByCurrency(listOfTerminal,"KZT"));
        }

        public IActionResult totalAmount()
        {

            return View(terminalService.amountByCardNumber(listOfTerminal));
        }
        [HttpGet]
        public IActionResult Report()
        {
            return View(terminalService.totalByDevice(listOfTerminal).ToList());
        }

        [HttpGet]
        public IActionResult Filter(string filterBy, string filter)
        {
            try
            {
                return View(terminalService.filter(listOfTerminal, filterBy, filter));
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
            return excelService.ExportByTotalCurrency(terminalService.totalByDevice(listOfTerminal));
        }

        public IActionResult TotalCurrency(string totalBy)
        {
            return View(terminalService.totalByCurrency(listOfTerminal, totalBy));
        }
        public ActionResult ExportByKGS()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency(listOfTerminal,"KGS"));
        }

        public ActionResult ExportByKZT()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency(listOfTerminal,"KZT"));
        }

        public ActionResult ExportByUSD()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency(listOfTerminal,"USD"));
        }

        public ActionResult ExportByEUR()
        {
            excelService = new ExcelService();
            return excelService.ExportByCurrency(terminalService.totalByCurrency(listOfTerminal,"EUR"));
        }

        public ActionResult ExportTotal()
        {
            excelService = new ExcelService();
            return excelService.ExportTotal(terminalService.amountByCardNumber(listOfTerminal));
        }
    }
}
