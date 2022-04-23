using System.ComponentModel.DataAnnotations;

namespace EatAndDrink.ViewModels
{
    public class TotalByCurrency
    {
        public static string Curr { get; set; }
        [Required]
        [Display(Name = "Card Number")]
        public int CardNumber { get; set; }

        [Required]
        [Display(Name = "Total")]
        public double Total { get; set; } = 0;

        
    }
}
