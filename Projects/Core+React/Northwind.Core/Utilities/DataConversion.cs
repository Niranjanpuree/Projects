using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Utilities
{
    public static class DataConversion
    {
        #region Int methods

        /// <summary>
        ///Converts object to Integer if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static int ParseInt(this object value, int ifNone)
        {
            if (value == null)
            {
                return ifNone;
            }
            return ParseInt("" + value, ifNone);
        }

        /// <summary>
        ///Converts string to Integer if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static int ParseInt(this string value, int ifNone)
        {
            int i = ifNone;
            if (!int.TryParse(value, out i))
            {
                i = ifNone;
            }
            return i;
        }

        #endregion

        #region Dec methods

        /// <summary>
        ///Converts object to decimal if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static decimal ParseDec(this object value, decimal ifNone)
        {
            if (value == null)
            {
                return ifNone;
            }
            return ParseDec("" + value, ifNone);
        }

        /// <summary>
        ///Converts string to decimal if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static decimal ParseDec(this string value, decimal ifNone)
        {
            decimal d = ifNone;
            if (!decimal.TryParse(value, out d))
            {
                d = ifNone;
            }
            return d;
        }

        #endregion

        #region Date methods

        /// <summary>
        ///Converts string to datetime if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static DateTime ParseDate(this string date, DateTime ifNone)
        {
            DateTime res = ifNone;
            if (!DateTime.TryParse(date, out res))
            {
                res = ifNone;
            }
            return res;
        }

        #endregion

        #region Long methods

        /// <summary>
        ///Converts object to long if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static long ParseLong(this object value, long ifNone)
        {
            if (value == null)
            {
                return ifNone;
            }
            return ParseLong("" + value, ifNone);
        }

        /// <summary>
        ///Converts string to long if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static long ParseLong(this string value, long ifNone)
        {
            long i = ifNone;
            if (!long.TryParse(value, out i))
            {
                i = ifNone;
            }
            return i;
        }

        #endregion

        #region Bool Methods
        /// <summary>
        ///Converts string to bool if the value is valid, otherwise returns ifnone value.
        /// </summary>
        public static bool ParseBool(this string value, bool ifNone = false)
        {
            bool res = ifNone;
            value = "" + value;
            if (value.Length > 1)
            {
                value = value.Substring(0, 1);
            }
            value = value.ToLower();
            if (value == "1" || value == "t" || value == "y")
            {
                res = true;
            }
            else if (value == "0" || value == "f" || value == "n")
            {
                res = false;
            }
            return res;
        }

        #endregion
    }
}