using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAPIPrueba
{
    public class Utilidades
    {
        public static bool isNumeric(string texto)
        {
            double num;
            return double.TryParse(texto.Trim(), out num);
        }

        public static bool isDate(string texto)
        {
            DateTime num;
            return DateTime.TryParse(texto.Trim(), out num);
        }

        public static bool emailIsValid(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}