using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Display(Name ="Saldo")]
        [Required(ErrorMessage ="campo obrigatório")]
        [DataType(DataType.Currency)]
        public double Balance { get; set; }

        [Display(Name = "Usuário")]
        public string UserId { get; set; }

        [Display(Name = "Usuário")]
        public User User { get; set; }
    }
}
