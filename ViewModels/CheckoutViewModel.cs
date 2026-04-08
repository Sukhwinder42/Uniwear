using System.ComponentModel.DataAnnotations;

namespace Uniwear.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public List<CartViewModel> CartItems { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
