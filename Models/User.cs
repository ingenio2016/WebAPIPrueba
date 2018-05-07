using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        //[DataType(DataType.EmailAddress)]
        [MaxLength(250, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Index("Usery_UserName_Index", IsUnique = true)]
        [Display(Name = "Correo")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [MaxLength(20, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener mínimo {1} caracteres")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Género")]
        public int Genero { get; set; }


        [DataType(DataType.ImageUrl)]
        [Display(Name = "Foto")]

        public string Photo { get; set; }

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

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar {0}")]
        [Display(Name = "Compañía")]
        public int CompanyId { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get {return string.Format("{0} {1}", FirstName, LastName); } }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual Company Company { get; set; }

        public virtual Country Country { get; set; }


    }
}