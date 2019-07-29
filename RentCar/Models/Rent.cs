using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    public class Rent
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="campo obrigatório")]
        [Display(Name ="Data de início")]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [Display(Name = "Data de término")]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [Display(Name = "Total")]
        [Range(0, Double.MaxValue, ErrorMessage ="valor inválido")]
        [Remote("Saldo-suficiente", "Aluguel")]
        [DataType(DataType.Currency)]
        public double Value { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
