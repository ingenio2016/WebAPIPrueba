using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class Refer
    {
        [Key]
        public int ReferId { get; set; }

        public int ReferType { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }

        public int Active { get; set; }

        //public virtual ICollection<Coordinator> Coordinators { get; set; }

        //public virtual ICollection<Boss> Bosses { get; set; }

        //public virtual ICollection<Link> Links { get; set; }

        //public virtual ICollection<Leader> leaders { get; set; }

        //public virtual ICollection<Voter> Voters { get; set; }

    }
}