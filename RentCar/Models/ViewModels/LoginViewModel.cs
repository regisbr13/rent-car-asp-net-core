using System.ComponentModel.DataAnnotations;

namespace RentCar.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Nome de usuário ou email")]
        [Required(ErrorMessage = "campo obrigatório")]
        public string UserName { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "campo obrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Mater-me conectado")]
        public bool Checkbox { get; set; }
    }
}
