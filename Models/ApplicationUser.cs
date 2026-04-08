using Microsoft.AspNetCore.Identity;

namespace Uniwear.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public DateTime CreatedAt { get; set; } 
        public ICollection<Order>? Orders { get; set; } //list of orders of order class
    }
}
