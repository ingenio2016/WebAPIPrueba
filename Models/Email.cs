using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPIPrueba.Models
{
    public class Email
    {
        [Key]
        public int EmailId { get; set; }

        [Display(Name = "Destinatarios")]
        public string To { get; set; }

        [Required, AllowHtml]
        [Display(Name = "Cuerpo")]

        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime SmsDate { get; set; }
    }
}