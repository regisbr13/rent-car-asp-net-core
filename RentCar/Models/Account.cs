namespace RentCar.Models
{
    public class Account
    {
        public int Id { get; set; }

        public double Balance { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
