using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Globalization;

namespace WebAPIPrueba
{
    public class ViewHelper
    {
        public static string UCWords(string str)
        {
            if (str.Length > 2)
            {
                return str.Trim().Substring(0, 1).ToUpper() + str.Trim().Substring(1).ToLower();
            }
            else
            {
                return str;
            }
        }
        public static string getDate(bool time = false)
        {
            if (time) return System.DateTime.Now.ToString("yyyy/MM/dd H:mm:ss");
            else return System.DateTime.Now.ToString("yyyy/MM/dd");
        }
        public static string getNameForFiles()
        {
            Random rnd = new Random();

            int Random = rnd.Next(1, 10000);
            return System.DateTime.Now.ToString("yyyyMMddHHmmss"+Random);
        }
        public static string ConvertToDate(string d)
        {
            if (d == null) return "";
            if (d.Length < 10) return "";

            var fecha1 = d.Substring(0, 10);
            var hora = "";

            string[] fecha = fecha1.Split('/');
            if (d.Length > 10)
            {
                hora = d.Substring(11, 8);
            }
            fecha1 = fecha[2] + "/" + fecha[1] + "/" + fecha[0] + " " + hora;
            return fecha1.Trim();
        }
        public static string getShortMonthName(int m)
        {
            string mes = "";
            switch ((m))
            {
                case 1:
                    mes = "Ene";
                    break;
                case 2:
                    mes = "Feb";
                    break;
                case 3:
                    mes = "Mar";
                    break;
                case 4:
                    mes = "Abr";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Jun";
                    break;
                case 7:
                    mes = "Jul";
                    break;
                case 8:
                    mes = "Ago";
                    break;
                case 9:
                    mes = "Sept";
                    break;
                case 10:
                    mes = "Oct";
                    break;
                case 11:
                    mes = "Nov";
                    break;
                case 12:
                    mes = "Dic";
                    break;
            }

            return mes;
        }
        public static string getMonthName(int m)
        {
            string mes = "";
            switch ((m))
            {
                case 1:
                    mes = "Enero";
                    break;
                case 2:
                    mes = "Febrero";
                    break;
                case 3:
                    mes = "Marzo";
                    break;
                case 4:
                    mes = "Abril";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Junio";
                    break;
                case 7:
                    mes = "Julio";
                    break;
                case 8:
                    mes = "Agosto";
                    break;
                case 9:
                    mes = "Setiembre";
                    break;
                case 10:
                    mes = "Octubre";
                    break;
                case 11:
                    mes = "Noviembre";
                    break;
                case 12:
                    mes = "Diciembre";
                    break;
            }

            return mes;
        }
        public static bool isAjaxRequest()
        {
            if (HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string toMoneyFormat(string str)
        {
            decimal valor = default(decimal);
            if (decimal.TryParse(str, out valor))
            {
                return string.Format("{0:n}", valor);
            }
            else
            {
                return "0.00";
            }
        }
        public static string toMoneyFormat(string str, decimal decimals)
        {
            decimal valor = default(decimal);
            string zero = "";
            for (int i = 1; i <= decimals; i++)
            {
                zero += "0";
            }
            if (decimal.TryParse(str, out valor))
            {
                return string.Format("{0:0,0." + zero + "}", valor);
            }
            else
            {
                return "0.000";
            }
        }
        public static string CurrentController() 
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
        }
        public static string CurrentAction()
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();
        }
        public static string CurrentRouteValue(string value)
        {
            object _value = HttpContext.Current.Request.RequestContext.RouteData.Values[value];
            return _value != null ? _value.ToString().ToLower() : "";
        }
        public static string ConvertNameToUrl(params string[] words)
        {
            string str = string.Join("-", words);

            if (str == null) return "";

            str = str.ToLower();
            str = str.Trim();

            str = str.Replace(",", "");
            str = str.Replace("&", "");
            str = str.Replace(" ", "-");
            str = str.Replace("--", "-");

            // Tildes, ñ, caracteres raros
            str = str.Replace("ñ", "n");
            str = str.Replace("á", "a");
            str = str.Replace("é", "e");
            str = str.Replace("í", "i");
            str = str.Replace("ó", "o");
            str = str.Replace("ú", "u");

            return str;
        }
        public static string BaseUrl(string path = "", bool encode = false) 
        {
            string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/" + path;

            if (encode) return HttpContext.Current.Server.UrlEncode(url);
            else return url;
        }
        public static string EncodeUrl(string url)
        {
            return HttpContext.Current.Server.UrlEncode(url);
        }
        public static string TimeFormat(string d)
        {
            if (d == "") return "";
            else return Convert.ToDateTime(d).ToString("hh:mmtt", new System.Globalization.CultureInfo("en-US"));
        }
        public static string TimeDiff(string hinicio, string hfin)
        {
            if (hinicio == "") return "0";
            if (hfin == "") return "0";

            DateTime a = ViewHelper.DateTimeFromTime(hinicio);
            DateTime b = ViewHelper.DateTimeFromTime(hfin);
            TimeSpan c = b - a;

            return c.ToString(@"hh\:mm");
        }
        public static DateTime DateTimeFromTime(string d)
        {
            return Convert.ToDateTime(d);
        }
        public static string CutPhrases(string frase, int lenght) 
        {
            if (frase == null) return "";
            if (frase.Length < lenght) return frase;
            return frase.Substring(0, lenght) + " ...";
        }
        public static string CapitalizeAll(string frase) 
        {
            if (frase == null)
            {
                return "";
            }
            else
            {
                String resultado = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(frase);
                //String resultado2 = new CultureInfo("en-US", false).TextInfo.ToTitleCase(frase);
                frase = resultado;
            }
            return frase;
        }
        public static string FormatToDateLong(string d)
        {
            if (d == "") return "";
            DateTime dt = Convert.ToDateTime(d);

            return dt.ToString("hh:mmtt", new System.Globalization.CultureInfo("en-US"));
        }
        public static string TimeAgo(string d) 
        {
            if (d == "") return "";

            bool time = true;

            if (d.Length == 10) time = false;

            DateTime dt = Convert.ToDateTime(d);

            TimeSpan timeSince = DateTime.Now.Subtract(dt);
            if (timeSince.TotalMilliseconds < 1)
                return "Todavía";
            if (timeSince.TotalMinutes < 1) 
            {
                if (time)
                {
                    int s = Convert.ToInt32(timeSince.TotalSeconds);
                    return "Hace " + (s >= 0 && s <= 15 ? "un instante" : s + " segundos");
                }
                else 
                {
                    return "Hoy";
                }
            }
            if (timeSince.TotalMinutes < 2) 
            {
                if (time)
                {
                    return "Hace un minuto";
                }
                else
                {
                    return "Hoy";
                }
            }                
            if (timeSince.TotalMinutes < 60)
            {
                if (time)
                {
                    return string.Format("Hace {0} minutos", timeSince.Minutes);
                }
                else
                {
                    return "Hoy";
                }
            }
            if (timeSince.TotalMinutes < 120)
            {
                if (time)
                {
                    return "Hace una hora";
                }
                else
                {
                    return "Hoy";
                }
            }                
            if (timeSince.TotalHours < 24)
            {
                if (time)
                {
                    return string.Format("Hace {0} horas", timeSince.Hours);
                }
                else
                {
                    return "Hoy";
                }
            }                
            if (timeSince.TotalDays < 2)
            {
                if (time)
                {
                    return "Ayer a la(s) " + dt.ToString("hh:mmtt").Replace(".", "");
                }
                else
                {
                    return "Ayer";
                }
            } 
                
            if (timeSince.TotalDays < 365)
            {
                if (time)
                {
                    return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower() + " a la(s) " + dt.ToString("hh:mmtt").Replace(".", "");
                }
                else
                {
                    return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower();
                }
            }

            if (time)
            {
                return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower() + " del " + dt.Year + " a la(s) " + dt.ToString("hh:mmtt").Replace(".", "");
            }
            else 
            {
                return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower() + " del " + dt.Year;
            }
        }
        public static string GetDayName(int id, bool _short = false)
        {
            switch(id)
            {
                case 1:
                    return _short ? "Lun" :  "Lunes";
                case 2:
                    return _short ? "Mar" : "Martes";
                case 3:
                    return _short ? "Mie" : "Miercoles";
                case 4:
                    return _short ? "Jue" : "Jueves";
                case 5:
                    return _short ? "Vie" : "Viernes";
                case 6:
                    return _short ? "Sab" : "Sábado";;
                case 0:
                    return _short ? "Dom" : "Domingo";
                default:
                    return "";
            };
        }
        public static string GetYouTubeID(string YoutubeUrl = null)
        {
            if (YoutubeUrl != null) 
            {
                //Setup the RegEx Match and give it 
                Match regexMatch = Regex.Match(YoutubeUrl, "^[^v]+v=(.{11}).*", RegexOptions.IgnoreCase);
                if (regexMatch.Success) return regexMatch.Groups[1].Value;
            }
            
            return null;
        }
        public static int GetIdFromUrl(string url) 
        {
            if (url == null) return 0;

            int id = 0;

            if (int.TryParse(url.Split('-')[url.Split('-').Count() - 1], out id)) return id;
            else return 0;
        }
        public static string ReplaceHttpInWord(string word) 
        {
            if (word == null) return "";

            if (!word.Contains("http")) return "http://" + word;
            return word;
        }
        public static string RemoveHTMLFromString(string html)
        {
            return Regex.Replace(html, "<.+?>", string.Empty);
        }
        public static int GetAge(string d) 
        {
            if (String.IsNullOrEmpty(d)) return 0;

            DateTime bday = Convert.ToDateTime(d);

            DateTime today = DateTime.Today;
            int age = today.Year - bday.Year;
            if (bday > today.AddYears(-age)) age--;

            return age;
        }
    }
}
