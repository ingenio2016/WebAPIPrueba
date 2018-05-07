using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Index("Department_Name_Index", 2, IsUnique = true)]
        [Display(Name = "Departamento")]

        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "País")]
        [Index("Department_Name_Index", 1, IsUnique = true)]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }//Relacion de unos a muchos, este es el uno

        public virtual ICollection<City> Cities { get; set; } //Relacion de uno a muchos este es el Mucho

        public virtual ICollection<Company> Companies { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Boss> Bosses { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public virtual ICollection<Coordinator> Coordinators { get; set; }

        public virtual ICollection<Leader> Leaders { get; set; }

        public virtual ICollection<VotingPlace> VotingPlaces { get; set; }

        public virtual ICollection<Commune> Commune { get; set; }

        public virtual ICollection<Association> Association { get; set; }

        //public virtual ICollection<Voter> Voters { get; set; }

    }
}