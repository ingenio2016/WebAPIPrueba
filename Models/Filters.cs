using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Filters
    {
        [Key]
        public int FiltersId { get; set; }

        public string name { get; set; }
    }
}