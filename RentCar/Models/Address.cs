using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Display(Name ="Rua")]
        [Required(ErrorMessage ="campo obrigatório")]
        [StringLength(30, MinimumLength =3, ErrorMessage ="digite de {2} a {1} caracteres")]
        public string Street { get; set; }

        [Display(Name = "N°")]
        [Required(ErrorMessage = "campo obrigatório")]
        public int Number { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "digite de {2} a {1} caracteres")]
        public string District { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "digite de {2} a {1} caracteres")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "digite de {2} a {1} caracteres")]
        public string State { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
