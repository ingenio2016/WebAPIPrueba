using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class WorkPlace
    {
        [Key]
        public int WorkPlaceId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Index("WorkPlace_Name_Index", IsUnique = true)]
        [Display(Name = "Lugar de Trabajo")]
        public string Name { get; set; }

        //public virtual ICollection<Boss> Bosses { get; set; }
    }
}