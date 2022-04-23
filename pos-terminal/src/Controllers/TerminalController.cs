using ClosedXML.Excel;
using EatAndDrink.Models;
using EatAndDrink.Services;
using EatAndDrink.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace EatAndDrink.Controllers
{
    public class TerminalController : Controller
    {
        private TerminalService terminalService = new TerminalService();
        private ExcelService excelService;
        private static TerminalDBContext TerminalDB;
        private List<Terminal> listOfTerminal;
        private List<TotalByCurrency> tot;
        private string help = "123";
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
            } catch (Exception e)
            {
                return RedirectToAction("NotFound");
            }
        }

      
        public ActionResult ExportFullReport()
        {
            ExcelService excelService = new ExcelService();
            return excelService.ExportFull(TerminalDB.Terminal.ToList());
        }

        public ActionResult ExportByTotalCurrency()
        {
            excelService = new ExcelService();
            return excelService.ExportByTotalCurrency(terminalService.totalByDevice());
        }

        public IActionResult TotalCurrency(string totalBy)
        {
            tot = terminalService.totalByCurrency(totalBy);
            //help = totalBy;
            //foreach (var a in tot)
            //{
            //    Console.WriteLine("this a " + a.TotalKgs);
            //}
            //excelService = new ExcelService();
            //excelService.ExportByCurrency(tot);
            return View(terminalService.totalByCurrency(totalBy));
        }
        public ActionResult ExportByCurrency()
        {
            //foreach (var a in tot)
            //{
            //    Console.WriteLine(a.TotalKgs);
            //}
            Console.WriteLine("Help" + help);
            excelService = new ExcelService();
            return excelService.ExportByCurrency(tot);
        }


    }
}
