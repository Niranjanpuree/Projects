using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Northwind.Core.Utilities
{
    public static class FormatHelper
    {
        public static string FormatAddress(string addressLine, string addressLine1, string city, string state, string zipCode, string country)
        {
            if (string.IsNullOrEmpty(addressLine))
            {
                addressLine = "";
            }
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
            var str = string.Format($"{addressLine.Trim()}{addressLine1}{city}{state}{zipCode}{country}");
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
            if (string.IsNullOrEmpty(val1))
            {
                val1 = "";
            }
            if (!string.IsNullOrEmpty(val2))
            {
                val2 = string.Concat(" - ", val2);
            }
            var str = string.Format($"{val1} {val2}");
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
        public static string FormatJobRequestTitle(string projectNumber, string contractOrTaskOrder, string contractNumber, string contractTitle, string taskOrderNo)
        {
            //var str = string.Format(
            //    $"Project Number :{projectNumber}  for  \n {contractOrTaskOrder} - {contractNumber} - {contractTitle}");
            if (string.IsNullOrWhiteSpace(projectNumber))
                projectNumber = "N/A";
            if (string.IsNullOrWhiteSpace(taskOrderNo))
                taskOrderNo = "N/A";
            var str = string.Format(
               $"{projectNumber}: {contractTitle}  for  \n ({contractNumber} : {taskOrderNo})");

            //var str = string.Format(
            //  $"{projectNumber}: {contractTitle}  for  \n ({contractNumber})");
            return str;
        }
        public static string FormatJobRequestValue(string val1, string val2)
        {
            var str = string.Format($"{val1}\n{val2}");
            return str;
        }
        public static string FormatDateTime(string date, string time)
        {
            var str = string.Format($"{date} {time}");
            return str;
        }

        public static string ConcatResourceTypeAndAction(string resourceType, string resourceAction)
        {
            var str = string.Format($"{resourceType}.{resourceAction}");
            return str;
        }

        public static string GetValueWithinParentheses(string fullString)
        {
            if (string.IsNullOrWhiteSpace(fullString))
                return string.Empty;
            var value = Regex.Match(fullString, @"\((\w+)\)").Groups[1].Value;
            return value;
        }
    }
}
