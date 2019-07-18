using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentCar.Models.ViewModels
{
    public class RoleFormViewModel : IdentityRole
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "nome obrigatório")]
        [Remote("RoleExist", "Roles")]
        public override string Name { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Use de {2} a {1} caracteres")]
        [Required(ErrorMessage = "nome obrigatório")]
        public string Description { get; set; }
    }
}
