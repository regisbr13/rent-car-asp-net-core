using System;

namespace RentCar.Models
{
    public class Rent
    {
        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double Value { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
