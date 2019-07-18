using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    public class Role : IdentityRole
    {
        [Display(Name="Descrição")]
        [StringLength(100, MinimumLength =3, ErrorMessage ="Use de {2} a {1} caracteres")]
        [Required(ErrorMessage ="nome obrigatório")]
        public string Description { get; set; }
    }
}
