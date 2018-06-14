using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class EmailSendStatus
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Send { get; set; }
    }
}