using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class VotingPlace
    {
        [Key]
        public int VotingPlaceId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Index("VotingPlace_Name_Index", 4, IsUnique = true)]
        [Display(Name = "Puesto de Votación")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Municipio")]
        [Index("VotingPlace_Name_Index", 3, IsUnique = true)]
        public int CityId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Departamento")]
        [Index("VotingPlace_Name_Index", 2, IsUnique = true)]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "País")]
        [Index("VotingPlace_Name_Index", 1, IsUnique = true)]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual Department Department { get; set; }//Relacion de unos a muchos, este es el uno

        public virtual City City { get; set; }//Relacion de unos a muchos, este es el uno

        public virtual ICollection<Boss> Bosses { get; set; }

        //public virtual ICollection<Voter> Voters { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public virtual ICollection<Coordinator> Coordinators { get; set; }

        public virtual ICollection<Leader> leaders { get; set; }

    }
}