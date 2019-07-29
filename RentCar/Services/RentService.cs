using RentCar.Data;
using RentCar.Models;
using RentCar.Services.Interfaces;

namespace RentCar.Services
{
    public class RentService : Service<Rent>, IRentService
    {
        public RentService(Context context) : base(context)
        {
        }
    }
}
