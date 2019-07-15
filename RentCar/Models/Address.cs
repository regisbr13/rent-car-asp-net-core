namespace RentCar.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public int Number { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
