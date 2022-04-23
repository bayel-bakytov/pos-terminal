﻿using EatAndDrink.Models;
using EatAndDrink.ViewModels;

namespace EatAndDrink.Services
{
    public class TerminalService
    {
        private static TerminalDBContext TerminalDB;
        private List<Terminal> list;
        private List<int> destinct;

        public List<Terminal> moreAboutCardNumber(int card)
        {
            using (TerminalDB = new TerminalDBContext())
            {
                list = TerminalDB.Terminal.Where(x => x.CardNumber == card).ToList();
                return list;
            }
        }

        public List<Terminal> filter(string filterBy, string filter)
        {
            using (TerminalDB = new TerminalDBContext())
            {
                int filt;
                int n;

                if (filterBy.Equals("Curr") && !int.TryParse(filter, out n))
                {
                    list = TerminalDB.Terminal.Where(x => x.Curr.Equals(filter) || filter == null).ToList();
                }
                else
                {
                    filt = Convert.ToInt32(filter);
                    if (filterBy.Equals("DeviceCode") && int.TryParse(filter, out n))
                    {
                        list = TerminalDB.Terminal.Where(x => x.DeviceCode == filt || filt == null).ToList();
                    }
                    else if (filterBy.Equals("CardNumber") && int.TryParse(filter, out n))
                    {
                        list = TerminalDB.Terminal.Where(x => x.CardNumber == filt || filt == null).ToList();
                    }
                    else if (filterBy.Equals("Amnt") && int.TryParse(filter, out n))
                    {
                        list = TerminalDB.Terminal.Where(x => x.Amnt == filt || filt == null).ToList();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                return list;
            }
        }

        public List<TotalDevice> totalByDevice()
        {
            using (TerminalDB = new TerminalDBContext())
            {
                double totalKGS = 0;
                double totalUSD = 0;
                double totalKZT = 0;
                double totalEUR = 0;
                list = TerminalDB.Terminal.ToList();
                List<Terminal> devices = TerminalDB.Terminal.ToList();
                List<TotalDevice> totalDevices = new List<TotalDevice>();
                TotalDevice totalDevice;
                destinct = list.Select(c => c.DeviceCode).Distinct().ToList();

                for (int j = 0; j < destinct.Count; j++)
                {
                    totalKGS = 0;
                    totalUSD = 0;
                    totalKZT = 0;
                    totalEUR = 0;
                    devices = filter("DeviceCode", destinct[j].ToString());
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
        }

        //public List<TotalDevice> totalByCurrency(string curren)
        //{
        //    Console.WriteLine(curren);

        //    using (TerminalDB = new TerminalDBContext())
        //    {
        //        double total = 0;
        //        List<Terminal> devices = TerminalDB.Terminal.ToList();
        //        list = devices.Where(x => x.Curr.Equals(curren) || curren == null).ToList();

        //        List<TotalDevice> totalDevices = new List<TotalDevice>();
        //        TotalDevice totalDevice;

        //        destinct = list.Select(c => c.CardNumber).Distinct().ToList();

        //        for (int j = 0; j < destinct.Count; j++)
        //        {
        //            total = 0;
        //            devices = filter("CardNumber", destinct[j].ToString());


        //            for (int i = 0; i < list.Count; i++)
        //            {
        //                total += list[i].Amnt;

        //            }

        //            totalDevice = new TotalDevice();
        //            totalDevice.CardNumber = destinct[j];
        //            if (curren.Equals("KGS"))
        //            {
        //                totalDevice.TotalKgs = total;
        //            }
        //            else if (curren.Equals("EUR"))
        //            {
        //                totalDevice.TotalEur = total;
        //            }
        //            else if (curren.Equals("KZT"))
        //            {
        //                totalDevice.TotalKzt = total;
        //            }
        //            else if (curren.Equals("USD"))
        //            {
        //                totalDevice.TotalUsd = total;
        //            }


        //            totalDevices.Add(totalDevice);
        //        }
        //        return totalDevices;
        //    }
        //}


        public List<TotalByCurrency> totalByCurrency(string curren)
        {
            Console.WriteLine(curren);

            using (TerminalDB = new TerminalDBContext())
            {
                double total = 0;
                List<Terminal> devices = TerminalDB.Terminal.ToList();
                list = devices.Where(x => x.Curr.Equals(curren) || curren == null).ToList();

                List<TotalByCurrency> totalDevices = new List<TotalByCurrency>();
                TotalByCurrency totalDevice;

                destinct = list.Select(c => c.CardNumber).Distinct().ToList();

                for (int j = 0; j < destinct.Count; j++)
                {
                    total = 0;
                    devices = filter("CardNumber", destinct[j].ToString());


                    for (int i = 0; i < list.Count; i++)
                    {
                        total += list[i].Amnt;
                    }

                    totalDevice = new TotalByCurrency();
                    totalDevice.CardNumber = destinct[j];
                    totalDevice.Total = total;
                   // totalDevice.Curr = curren;
                    totalDevices.Add(totalDevice);
                }
                return totalDevices;
            }
        }
    }
}
