using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "Nombre")]
        [Index("Company_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "Dirección")]

        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Logo de la Compañia")]

        public string Logo { get; set; }

        [NotMapped]
        public HttpPostedFileBase LogoFile { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "País")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Departamento")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Ciudad")]
        public int CityId { get; set; }

        

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Boss> Bosses { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public virtual ICollection<Coordinator> Coordinators { get; set; }

        public virtual ICollection<Leader> Leaders { get; set; }

        public virtual ICollection<Voter> Voters { get; set; }


    }
}