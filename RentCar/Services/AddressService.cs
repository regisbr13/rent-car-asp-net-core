using RentCar.Data;
using RentCar.Models;
using RentCar.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentCar.Services
{
    public class AddressService : Service<Address>, IAddressService
    {
        public AddressService(Context context) : base(context)
        {
        }
    }
}
