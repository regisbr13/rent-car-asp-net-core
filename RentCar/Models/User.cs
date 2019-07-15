using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace RentCar.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Cpf { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<Rent> Rents { get; set; }

        public Account Account { get; set; }
    }
}
