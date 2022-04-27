using EatAndDrink.Models;
using EatAndDrink.ViewModels;
using OfficeOpenXml;

namespace EatAndDrink.Services
{
    public class TerminalService
    {
        private static TerminalDBContext TerminalDB;
        private List<Terminal> list;
        private List<int> destinct;

        public List<Terminal> filter(List<Terminal> excelList, string filterBy, string filter)
        {

            //переменная для перевода из строки в число(filter -> filt)
            int filt;
            //переменная для проверки на число
            int n;

            //проверка на число
            if (filterBy.Equals("Curr") && !int.TryParse(filter, out n))
            {
                //Берем все записи где валюта = filter 
                list = excelList.Where(x => x.Curr.Equals(filter) || filter == null).ToList();
            }
            else
            {
                //перевод в число 
                filt = Convert.ToInt32(filter);

                if (filterBy.Equals("DeviceCode") && int.TryParse(filter, out n))
                {
                    //Берем все записи где номер терминала = filt 
                    list = excelList.Where(x => x.DeviceCode == filt || filt == null).ToList();
                }
                else if (filterBy.Equals("CardNumber") && int.TryParse(filter, out n))
                {
                    //Берем все записи где номер карты = filt
                    list = excelList.Where(x => x.CardNumber == filt || filt == null).ToList();
                }
                else if (filterBy.Equals("Amnt") && int.TryParse(filter, out n))
                {
                    //Берем все записи где сумма оплаты = filt
                    list = excelList.Where(x => x.Amnt == filt || filt == null).ToList();
                }
                else
                {
                    throw new Exception();
                }
            }
            return list;

        }

        public List<TotalDevice> totalByDevice(List<Terminal> listOfTerminal)
        {
            double totalKGS = 0;
            double totalUSD = 0;
            double totalKZT = 0;
            double totalEUR = 0;
            list = listOfTerminal;
            List<Terminal> devices = listOfTerminal;
            List<TotalDevice> totalDevices = new List<TotalDevice>();
            TotalDevice totalDevice;
            destinct = list.Select(c => c.DeviceCode).Distinct().ToList();

            for (int j = 0; j < destinct.Count; j++)
            {
                totalKGS = 0;
                totalUSD = 0;
                totalKZT = 0;
                totalEUR = 0;
                devices = filter(listOfTerminal, "DeviceCode", destinct[j].ToString());
                for (int i = 0; i < devices.Count; i++)
                {
                    if (devices[i].Curr.Equals("KGS"))
                    {
                        totalKGS += devices[i].Amnt;
                    }
                    else if (devices[i].Curr.Equals("KZT"))
                    {
                        totalKZT += devices[i].Amnt;
                    }
                    else if (devices[i].Curr.Equals("USD"))
                    {
                        totalUSD += devices[i].Amnt;
                    }
                    else if (devices[i].Curr.Equals("EUR"))
                    {
                        totalEUR += devices[i].Amnt;
                    }
                }
                totalDevice = new TotalDevice()
                {
                    DeviceCode = destinct[j],
                    TotalKgs = totalKGS,
                    TotalUsd = totalUSD,
                    TotalKzt = totalKZT,
                    TotalEur = totalEUR
                };
                totalDevices.Add(totalDevice);
            }
            return totalDevices;
        }

        public List<TotalByCurrency> totalByCurrency(List<Terminal> listOfTerminal, string curren)
        {
            Console.WriteLine(curren);
            double total = 0;
            List<Terminal> list = new List<Terminal>();

            List<Terminal> devices = listOfTerminal;

            list = devices.Where(x => x.Curr.Equals(curren) || curren == null).ToList();

            List<TotalByCurrency> totalDevices = new List<TotalByCurrency>();
            TotalByCurrency totalDevice;

            destinct = list.Select(c => c.CardNumber).Distinct().ToList();

            for (int j = 0; j < destinct.Count; j++)
            {
                total = 0;
                devices = filter(list, "CardNumber", destinct[j].ToString());
                for (int i = 0; i < devices.Count; i++)
                {
                    total += devices[i].Amnt;
                }
                totalDevice = new TotalByCurrency();
                totalDevice.CardNumber = destinct[j];
                totalDevice.Total = total;
                totalDevices.Add(totalDevice);
            }
            return totalDevices;
        }

        public List<TotalByCurrency> amountByCardNumber(List<Terminal> listOfTerminal)
        {

            List<TotalByCurrency> totalAmount = new List<TotalByCurrency>();
            CurrencyParse currParse = new CurrencyParse();
            double total = 0;
            List<Terminal> cardNumbers = listOfTerminal;
            List<Terminal> term;
            TotalByCurrency amount;
            destinct = cardNumbers.Select(c => c.CardNumber).Distinct().ToList();
            for (int i = 0; i < destinct.Count; i++)
            {
                total = 0;
                term = filter(listOfTerminal, "CardNumber", destinct[i].ToString());
                for (int j = 0; j < term.Count; j++)
                {
                    if (term[j].Curr.Equals("KGS"))
                    {
                        total += term[j].Amnt;
                    }
                    else if (term[j].Curr.Equals("KZD"))
                    {
                        total += term[j].Amnt * currParse.currentCurrency("KZD");
                    }
                    else if (term[j].Curr.Equals("USD"))
                    {
                        total += term[j].Amnt * currParse.currentCurrency("USD");
                        Console.WriteLine("----" + currParse.currentCurrency("KZD"));
                        Console.WriteLine("----" + currParse.currentCurrency("USD"));
                        Console.WriteLine("----" + currParse.currentCurrency("EUR"));
                    }
                    else if (term[j].Curr.Equals("EUR"))
                    {
                        total += term[j].Amnt * currParse.currentCurrency("EUR");
                    }
                }
                amount = new TotalByCurrency()
                {
                    CardNumber = destinct[i],
                    Total = total,
                };
                totalAmount.Add(amount);
            }
            foreach (var i in totalAmount)
            {
                Console.WriteLine(i.CardNumber + " " + i.Total);
            }
            return totalAmount;

        }

        public List<Terminal> filterByDate(List<Terminal> listOfTerminal, string fromDate, string toDate)
        {
            List<Terminal> result = new List<Terminal>();
            DateTime from = Convert.ToDateTime(fromDate);
            DateTime to = Convert.ToDateTime(toDate);
            for (int i = 0; i < listOfTerminal.Count; i++)
            {
                if (from < listOfTerminal[i].OperDateTime && to > listOfTerminal[i].OperDateTime)
                {
                    result.Add(listOfTerminal[i]);
                }
            }             
            return result;
        }
    }
}
