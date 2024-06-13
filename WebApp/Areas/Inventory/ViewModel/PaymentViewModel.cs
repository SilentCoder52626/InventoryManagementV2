using System.ComponentModel.DataAnnotations;

namespace Inventory.ViewModels
{
    public class PaymentViewModel
    {
        [Required]
        [Display(Name = "Customer Id")]
        public long CusId { get; set; }
        [Display(Name = "Name")]
        public string? FullName { get; set; }
        [Display(Name = "Due")]
        public decimal DueAmount { get; set; }
        [Required]
        [Display(Name = "Paid Amount")]
        public decimal Amount { get; set; }

    }
}
