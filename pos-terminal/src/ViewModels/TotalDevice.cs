using System.ComponentModel.DataAnnotations;

namespace EatAndDrink.ViewModels
{
    public class TotalDevice
    {
        [Required]
        [Display(Name = "Device Code")]
        public int DeviceCode { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        public int CardNumber { get; set; }

        [Required]
        [Display(Name = "Total Usd")]
        public double TotalUsd { get; set; }

        [Required]
        [Display(Name = "Total Kgs")]
        public double TotalKgs { get; set; }

        [Required]
        [Display(Name = "Total Eur")]
        public double TotalEur { get; set; }

        [Required]
        [Display(Name = "Total Kzt")]
        public double TotalKzt { get; set; }
    }
}
