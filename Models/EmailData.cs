using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class EmailData
    {
        public List<EmailContact> emails { get; set; }

        public string subject { get; set; }

        public string message { get; set; }
    }
}