using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPIPrueba.Models
{
    public class TimesDates
    {
        [Key]
        public int TimesDateId { get; set; }

        public int ProfessionalId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        //HORAS DE LA CITA

        public int seis00 { get; set; }

        public int seis15 { get; set; }

        public int seis30 { get; set; }

        public int seis45 { get; set; }


        public int siete00 { get; set; }

        public int siete15 { get; set; }

        public int siete30 { get; set; }

        public int siete45 { get; set; }


        public int ocho00 { get; set; }

        public int ocho15 { get; set; }

        public int ocho30 { get; set; }

        public int ocho45 { get; set; }


        public int nueve00 { get; set; }

        public int nueve15 { get; set; }

        public int nueve30 { get; set; }

        public int nueve45 { get; set; }


        public int diez00 { get; set; }

        public int diez15 { get; set; }

        public int diez30 { get; set; }

        public int diez45 { get; set; }


        public int once00 { get; set; }

        public int once15 { get; set; }

        public int once30 { get; set; }

        public int once45 { get; set; }


        public int doce00 { get; set; }

        public int doce15 { get; set; }

        public int doce30 { get; set; }

        public int doce45 { get; set; }


        public int uno00 { get; set; }

        public int uno15 { get; set; }

        public int uno30 { get; set; }

        public int uno45 { get; set; }


        public int dos00 { get; set; }

        public int dos15 { get; set; }

        public int dos30 { get; set; }

        public int dos45 { get; set; }


        public int tres00 { get; set; }

        public int tres15 { get; set; }

        public int tres30 { get; set; }

        public int tres45 { get; set; }


        public int cuatro00 { get; set; }

        public int cuatro15 { get; set; }

        public int cuatro30 { get; set; }

        public int cuatro45 { get; set; }


        public int cinco00 { get; set; }

        public int cinco15 { get; set; }

        public int cinco30 { get; set; }

        public int cinco45 { get; set; }


        public int seisp00 { get; set; }

        public int seisp15 { get; set; }

        public int seisp30 { get; set; }

        public int seisp45 { get; set; }


        public int sietep00 { get; set; }

        public int sietep15 { get; set; }

        public int sietep30 { get; set; }

        public int sietp45 { get; set; }


        public int ochop00 { get; set; }

        public int ochop15 { get; set; }

        public int ochop30 { get; set; }

        public int ochop45 { get; set; }


        public int nuevep00 { get; set; }

        public int nuevep15 { get; set; }

        public int nuevep30 { get; set; }

        public int nuevep45 { get; set; }

    }
}