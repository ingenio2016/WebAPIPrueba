using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Voter
    {
        [Key]
        public int VoterId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "Documento")]
        [Index("Voters_UserName_Index", IsUnique = true)]
        public double Document { get; set; }

        //[DataType(DataType.EmailAddress)]
        [MaxLength(250, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        //[Index("Voter_UserName_Index", 2, IsUnique = true)]
        [Display(Name = "Correo")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }


        public string Fname { get; set; }

       

        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "País")]
        public string CountryId { get; set; }


        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Departamento")]
        public string DepartmentId { get; set; }

        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Ciudad")]
        public string CityId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Compañía")]
        public int CompanyId { get; set; }

        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Comuna donde vota")]
        public string CommuneId { get; set; }

        [Display(Name = "Barrio")]
        public string Barrio { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]

        public DateTime DateBorn { get; set; }

        [Display(Name = "Profesion")]
        public string Profesion { get; set; }
        
        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Lugar de Votación")]
        public string VotingPlaceId { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        [Display(Name = "Jefe")]
        public int BossId { get; set; }

        [Display(Name = "Enlace")]
        public int LinkId { get; set; }

        [Display(Name = "Coordinador")]
        public int CoordinatorId { get; set; }

        [Display(Name = "Líder")]
        public int LeaderId { get; set; }

        public int userId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Perfil del referido")]
        public int PerfilId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Referido")]
        public int ReferId { get; set; }

        //public virtual Department Department { get; set; }

        //public virtual City City { get; set; }

        public virtual Company Company { get; set; }

        //public virtual Country Country { get; set; }

        //public virtual Commune Commune { get; set; }

        //public virtual VotingPlace VotingPlace { get; set; }

        public virtual UserId User { get; set; }

        //public virtual Refer Refer { get; set; }
    }
}