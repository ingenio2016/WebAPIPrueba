﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Commune
    {
        [Key]
        public int CommuneId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Index("Commune_Name_Index", 4, IsUnique = true)]
        [Display(Name = "Nombre Comuna")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Municipio")]
        [Index("Commune_Name_Index", 3, IsUnique = true)]
        public int CityId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Departamento")]
        [Index("Commune_Name_Index", 2, IsUnique = true)]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "País")]
        [Index("Commune_Name_Index", 1, IsUnique = true)]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual Department Department { get; set; }//Relacion de unos a muchos, este es el uno

        public virtual City City { get; set; }//Relacion de unos a muchos, este es el uno

        public virtual ICollection<Boss> Bosses { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public virtual ICollection<Coordinator> Coordinators { get; set; }

        public virtual ICollection<Leader> Leaders { get; set; }

        //public virtual ICollection<Voter> Voters { get; set; }


    }
}