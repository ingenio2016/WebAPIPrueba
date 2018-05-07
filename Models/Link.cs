using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Link
    {
        [Key]
        public int LinkId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        //[MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "Documento")]
        [Index("Link_UserName_Index", IsUnique = true)]
        public double Document { get; set; }

        //[DataType(DataType.EmailAddress)]
        [MaxLength(250, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        //[Index("Link_UserName_Index", 2, IsUnique = true)]
        [Display(Name = "Correo")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }


        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Lugar de Trabajo")]
        public int WorkPlaceId { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Foto")]

        public string Photo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "País")]
        public int CountryId { get; set; }

        [Display(Name = "Observación")]
        public string Observation { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Departamento")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Ciudad")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Compañía")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Comuna donde vota")]
        public int CommuneId { get; set; }

        [Display(Name = "Asociación")]
        public string Asociacion { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Jefe")]
        public int BossId { get; set; }

        //[Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Reunión")]
        public DateTime Date { get; set; }

        //[Required(ErrorMessage = "El campo {0} es requerido")]
        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Lugar de Votación")]
        public int VotingPlaceId { get; set; }

        [Display(Name = "Barrio")]
        public string Barrio { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]

        public DateTime DateBorn { get; set; }

        [Display(Name = "Profesion")]
        public string Profesion { get; set; }

        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; }

        [Display(Name = "Años de Experiencia")]
        public string TiempoExperiencia { get; set; }

        [Display(Name = "Zona encargada")]
        public string zonaEncargada { get; set; }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual Company Company { get; set; }

        public virtual Country Country { get; set; }

        public virtual Boss Boss { get; set; }

        public virtual Commune Commune { get; set; }

        //public virtual Association Associacion { get; set; }

        public virtual VotingPlace VotingPlace { get; set; }

        //public virtual Refer Refer { get; set; }


    }
}