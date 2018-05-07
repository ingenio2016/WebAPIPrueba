using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
   public class DateHours
        {
            [Key]
            public int DateHoursId { get; set; }

            public string Name { get; set; }
        }
}