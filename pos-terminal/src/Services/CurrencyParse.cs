using System.Xml;
using WebSupergoo.ABCpdf;

namespace EatAndDrink.Services
{
    public class CurrencyParse
    {
        public double currentCurrency(string c)
        {
            XmlTextReader reader = new XmlTextReader("https://www.nbkr.kg/XML/daily.xml");

            double rez = 0;
            while (reader.Read())
            {
                if (reader.GetAttribute("ISOCode") == c)
                {
                    while (reader.Name != "Value")
                    {
                        reader.Read();
                    }
                    reader.Read();
                    rez = Convert.ToDouble(reader.Value);
                    reader.Close();
                    return rez;
                }
            }
            reader.Close();
            return rez;
        }
    }

}
