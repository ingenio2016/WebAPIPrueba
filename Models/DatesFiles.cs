using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class DatesFiles
    {
        [Key]
        public int DatesFilesId { get; set; }

        [Required]
        public int DateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Ruta")]
        public string Path { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        public int CompanyId { get; set; }
    }
}