using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPIPrueba.Models
{
    public class Sms
    {
        [Key]
        public int SmsId { get; set; }

        [Display(Name = "Destinatarios")]
        public string To { get; set; }

        [Required, AllowHtml]
        [Display(Name = "Mensaje")]
        public string Message { get; set; }

        public DateTime SmsDate { get; set; }
    }
}