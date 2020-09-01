using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Northwind.Core.Interfaces;
using System.Reflection;
using Northwind.Core.Import.Model;
using System.Dynamic;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Core.Utilities
{
    public class ValidationHelper : IValidationHelper
    { 
        public static bool AlphaNum(string alpha)
        {
            Regex alphaNum = new Regex("^[a-zA-Z0-9]+$");
            Match match = alphaNum.Match(alpha);
            if (match.Success)
                return true;
            return false;
        }

        public bool ReservedChar(string reserve)
        {
            Regex reservedChar = new Regex("^[^<>:|_\\`$%!z@\\#*+\"]+$");
            Match match = reservedChar.Match(reserve);
            if (match.Success)
                return true;
            return false;
        }

        public bool CheckNull(string reqString)
        {
            if (string.IsNullOrWhiteSpace(reqString))
                return false;    
            return true;   
        }

        public static bool IsNumeric(string alpha)
        {
            if (string.IsNullOrWhiteSpace(alpha))
                return false;
            Regex alphaNum = new Regex("^[Z0-9]+$");
            Match match = alphaNum.Match(alpha);
            if (match.Success)
                return true;
            return false;
        }

        public static bool IsValidDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return false;
            DateTime dateTime;
            if (DateTime.TryParse(date, out dateTime))
                return true;
            else
                return false;
        }

        public static bool IsValidDecimal(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return false;
            decimal number;
            if (Decimal.TryParse(stringValue, out number))
                return true;
            else
                return false;
        }

        public static ValidationResult NullValidation(object dataObject)
        {
            var validation = new ValidationResult();
            validation.IsValid = true;
            foreach (PropertyInfo property in dataObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dataObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        validation.IsValid = false;
                        validation.Message += property.Name + " is empty. ";
                        //return validation;
                    }
                }
            }
            return validation;
        }

        public static Dictionary<string,string> CreateDataObjectFromObject(object dataObject)
        {
            var dynamicObject = new Dictionary<string, string>();
            foreach (PropertyInfo property in dataObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dataObject);
                    if (!string.IsNullOrEmpty(value))
                    {
                        dynamicObject.Add(property.Name, value);
                    }
                }
            }
            return dynamicObject;
        }

        public static ValidationResult DecimalValidation(object dataObject)
        {
            var validation = new ValidationResult();
            validation.IsValid = true;
            foreach (PropertyInfo property in dataObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dataObject);
                    if (!string.IsNullOrEmpty(value))
                    {
                        var isValid = IsValidDecimal(value);
                        if (!isValid)
                        {
                            validation.IsValid = false;
                            validation.Message += property.Name + " is not valid.";
                            //return validation;
                        }
                        
                    }
                }
            }
            return validation;
        }

        public static ValidationResult DateValidation(object dataObject)
        {
            var validation = new ValidationResult();
            validation.IsValid = true;
            foreach (PropertyInfo property in dataObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dataObject);
                    if (!string.IsNullOrEmpty(value))
                    {
                        var isValid = IsValidDate(value);
                        if (!isValid)
                        {
                            validation.IsValid = false;
                            validation.Message += property.Name + " is not valid.";
                            //return validation;
                        }
                    }
                }
            }
            return validation;
        }

    }
}
