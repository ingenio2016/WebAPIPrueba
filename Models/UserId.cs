using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class UserId
    {
        [Key]
        public int userId { get; set; }

        public string name { get; set; }

        public virtual ICollection<Coordinator> Coordinators { get; set; }

        public virtual ICollection<Leader> leaders { get; set; }

        public virtual ICollection<Voter> Voters { get; set; }

    }
}