using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "País")]
        [Index("Country_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Department> Departments { get; set; } //Relacion de uno a muchos este es el Mucho

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

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