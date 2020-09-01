using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.web.Helpers
{
    public static class FormatHelper
    {
        public static string FormatAddress(string addressLine, string addressLine1, string city, string state, string zipCode, string country)
        {
            if (!string.IsNullOrEmpty(addressLine1))
            {
                addressLine1 = string.Concat(", ", addressLine1);
            }
            if (!string.IsNullOrEmpty(city))
            {
                city = string.Concat(", ", city);
            }
            if (!string.IsNullOrEmpty(state))
            {
                state = string.Concat(", ", state);
            }
            if (!string.IsNullOrEmpty(zipCode))
            {
                zipCode = string.Concat(", ", zipCode);
            }
            if (!string.IsNullOrEmpty(country))
            {
                country = string.Concat(", ", country);
            }
            if (!string.IsNullOrEmpty(addressLine))
                addressLine = addressLine.Trim();
            var str = string.Format($"{addressLine}{addressLine1}{city}{state}{zipCode}{country}");
            return str;
        }
        public static string CustomerFormatAddress(string street1, string street2, string city, string state, string zipCode, string country)
        {
            var addressline1 = (street2 != "" ? (',' + street2) : "");
            var str = string.Format($"{street1} \n {addressline1}, {city}, {state}, {zipCode}, {country}");
            return str;
        }
        public static string FormatCustomerNameAndAddress(string officeName, string department, string street, string city, string state, string zipCode, string country)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(department))
            {
                str = string.Format($"{officeName}  {street}, {city}, {state}, {zipCode}, {country}");
            }
            else
            {
                str = string.Format($"{officeName},{department} {street}, {city} ,{state} ,{zipCode} ,{country}");
            }
            return str;
        }
        public static string FormatAutoCompleteData(string val1, string val2)
        {
            var str = string.Format($"{val1} - {val2}");
            return str;
        }
        public static string FormatFullName(string val1, string val2, string val3)
        {
            var str = string.Format($"{val1} {val2} {val3}");
            return str;
        }
        public static string FormatFullNamewithDesignation(string val1, string val2, string val3, string val4)
        {
            val4 = string.Concat(" (", val4, ")");
            var str = string.Format($"{val1} {val2} {val3} {val4}");
            return str;
        }
        public static string FormattwoValue(string val1, string val2)
        {
            var str = string.Format($"{val1}\n{val2}");
            return str;
        }
        public static string FormatJobRequestTitle(string projectNumber, string contractOrTaskOrder, string contractNumber, string contractTitle)
        {
            var str = string.Format(
                $"Project Number :{projectNumber}  for  \n {contractOrTaskOrder} - {contractNumber} - {contractTitle}");
            return str;
        }
        public static string FormatJobRequestValue(string val1, string val2)
        {
            var str = string.Format($"{val1}\n{val2}");
            return str;
        }
        public static string GetNewFileName(string val1)
        {
            var splitTheExtension = val1.ToUpper().Split(".CSV");
            var getTheCurrentCount = splitTheExtension[0].Substring(splitTheExtension[0].Length - 2,1);
            int countNumber = int.Parse(getTheCurrentCount) + 1;
            string newFileName = splitTheExtension[0].Remove(splitTheExtension[0].Length - 2)+ countNumber + ").CSV";
            return newFileName;
        }
        public static string FormatDateTime(string date, string time)
        {
            var str = string.Format($"{date} {time}");
            return str;
        }

        public static string ConcatTwoString(string string1, string string2)
        {
            var finalString = string.Empty;
            if (string.IsNullOrWhiteSpace(string1))
                finalString = string2;
            if (string.IsNullOrWhiteSpace(string2))
                finalString = string1;
            if (!string.IsNullOrWhiteSpace(string1) && !string.IsNullOrWhiteSpace(string2))
                finalString = string1 + ", " + string2;
            return finalString;
        }
        public static string ConcatResourceTypeAndAction(string resourceType, string resourceAction)
        {
            var str = string.Format($"{resourceType}.{resourceAction}");
            return str;
        }

        public static string ObectToJson(object ob)
        {
            var str = JsonConvert.SerializeObject(ob);
            return str;
        }
    }
}
