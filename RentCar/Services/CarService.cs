using RentCar.Data;
using RentCar.Models;
using RentCar.Services.Interfaces;

namespace RentCar.Services
{
    public class CarService : Service<Car>, ICarService
    {
        public CarService(Context context) : base(context)
        {
        }
    }
}
