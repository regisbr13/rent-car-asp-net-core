using System.Collections.Generic;

namespace RentCar.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string ImgPath { get; set; }

        public double DailyPrice { get; set; }

        public ICollection<Rent> Rents { get; set; }
    }
}
