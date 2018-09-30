using System;
using System.Collections.Generic;
using System.Text;

namespace TimedTasks.Utils
{
    public static class Strings
    {
        /// <summary>
        /// Převede Date část z DateTime na den slovně pokud je v daném rozmězí: 
        /// -2 dny = předevčírem, -1 den = včera, 0 = dnes, +1 = zítra, +2 = pozítří, nebo vrátí datum v podle defaultního formátu
        /// podle reference
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateReference"></param>
        /// <param name="defaultFormat"></param>
        /// <returns></returns>
        public static string DateTimeToString(DateTime date, DateTime dateReference, string defaultFormat) // TODO : Pořešit jazyk
        {
            var diff = date.Date - dateReference.Date;

            var days = new Dictionary<int, string>()
            {
                //{ -2,   "předevčírem" },
                { -1,   "včera" },
                { 0,    "dnes" },
                { 1,    "zítra" },
                //{ 2,    "pozítří" }
            };

            if (days.ContainsKey(diff.Days))
                return days[diff.Days];

            if (String.IsNullOrEmpty(defaultFormat))
                return date.ToString();

            return date.ToString(defaultFormat); ;
        }

        /// <summary>
        /// Zvětší první písmeno ve stringu a zbytek změnší.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            if (text.Length == 1)
                return text.ToUpper();

            return text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1).ToLower();
        }
    }
}
