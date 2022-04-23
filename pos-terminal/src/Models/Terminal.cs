namespace EatAndDrink.Models
{
    public class Terminal
    {
        public int Id { get; set; }
        public int DeviceCode { get; set; }
        public DateTime OperDateTime { get; set; }
        public string Curr { get; set; }
        public double Amnt { get; set; }
        public int CardNumber { get; set; }
    }
}
