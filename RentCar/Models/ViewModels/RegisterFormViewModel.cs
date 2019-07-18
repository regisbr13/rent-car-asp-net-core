using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models.ViewModels
{
    public class RegisterFormViewModel
    {
        [Display(Name = "Nome de usuário")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "o nome de usuário deve conter de {2} a {1} caracteres")]
        [Remote("UserNameExist", "Account")]
        public string UserName { get; set; }

        [Display(Name = "Nome completo")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "o nome de usuário deve conter de {2} a {1} caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "campo obrigatório")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "campo obrigatório")]
        [EmailAddress(ErrorMessage = "entre com um email válido")]
        [Remote("UserEmailExist", "Account")]
        public string Email { get; set; }

        [Display(Name = "Confirmar email")]
        [Required(ErrorMessage = "campo obrigatório")]
        [EmailAddress(ErrorMessage = "entre com um email válido")]
        [Compare("Email", ErrorMessage = "emails não correspondem")]
        public string EmailConf { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "campo obrigatório")]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "escolha uma senha de {2} a {1} caracteres")]
        public string Password { get; set; }

        [Display(Name = "Confirmar senha")]
        [Required(ErrorMessage = "campo obrigatório")]
        [Compare("Password", ErrorMessage = "senhas não correspondem")]
        [DataType(DataType.Password)]
        public string PasswordConf { get; set; }
    }
}
