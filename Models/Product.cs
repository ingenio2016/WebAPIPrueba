using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Product
    {
        [Key]
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; }

        public string Proveedor { get; set; }

        public string Categoria { get; set; }

        public string CantidadPorUnidad { get; set; }

        public string PrecioUnidad { get; set; }

        public int UnidadesEnExistencia { get; set; }

    }
}