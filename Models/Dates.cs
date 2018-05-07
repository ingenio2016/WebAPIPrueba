using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Dates
    {
        [Key]
        public int DateId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "You must be select a {0}")]
        [Index("Date_Index", 2, IsUnique = true)]
        public int ProfessionalId { get; set; }//Usuario que pide la cita

        public string organizador { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Index("Date_Index", 1, IsUnique = true)]

        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Index("Date_Index", 3, IsUnique = true)]
        public int HourId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe ser máximo de {1} caracteres")]
        public string Hour { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime SystemDate { get; set; }

        [MaxLength(400, ErrorMessage = "El campo {0} debe ser máximo de {1} caracteres")]
        public string Observation { get; set; }

        [MaxLength(400, ErrorMessage = "El campo {0} debe ser máximo de {1} caracteres")]
        public string Address { get; set; }

        public int PersonsNumber { get; set; }

        [MaxLength(16, ErrorMessage = "El campo {0} debe ser máximo de {1} caracteres")]
        public string Phone { get; set; }

        public bool Asistencia { get; set; }

        public string Moderator { get; set; }


    }
}