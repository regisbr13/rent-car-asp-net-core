using Microsoft.AspNetCore.Identity;

namespace RentCar.Models
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}
