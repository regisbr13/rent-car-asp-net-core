using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Display(Name ="Marca")]
        [Required(ErrorMessage ="campo obrigatório")]
        [StringLength(20, MinimumLength =3, ErrorMessage ="use de {2} a {1} caracteres")]
        public string Brand { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "use de {2} a {1} caracteres")]
        public string Model { get; set; }

        [Display(Name = "Foto")]
        public string ImgPath { get; set; }

        [Display(Name = "Preço da diária")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode =true)]
        public double DailyPrice { get; set; }

        public ICollection<Rent> Rents { get; set; }
    }
}
