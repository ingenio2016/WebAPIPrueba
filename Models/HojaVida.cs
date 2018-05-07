using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class HojaVida
    {
        [Key]
        public int HojaVidaId { get; set; }

        [Required]
        public int RolId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Ruta")]
        public string Path { get; set; }

    }
}