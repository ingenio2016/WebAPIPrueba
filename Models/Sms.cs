using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Sms
    {
        [Key]
        public int SmsId { get; set; }

        [Display(Name = "Destinatarios")]
        public string To { get; set; }

        [Display(Name = "Mensaje")]
        public string Message { get; set; }

        public DateTime SmsDate { get; set; }
    }
}