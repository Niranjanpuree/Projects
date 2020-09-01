using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMCustomerContact
    {
        public string CustomerName { get; set; }
        public string ContactType { get; set; }
        public string ContactName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AltEmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string Notes { get; set; }
        public string Action { get; set; }

        public bool IsValid { get; set; } = true;
        public Guid ContactTypeGuid { get; set; }
        public Guid CustomerGuid { get; set; }

        public string ImportStatus { get; set; }
        public string Reason { get; set; }
        
    }

    public sealed class CustomerContactHeaderMap : ClassMap<DMCustomerContact>
    {
        public CustomerContactHeaderMap(Dictionary<string, string> headers)
        {
            if(headers.ContainsKey("CustomerName"))
                Map(m => m.CustomerName).Name(headers["CustomerName"]).Optional();
            if (headers.ContainsKey("ContactName"))
                Map(m => m.ContactName).Name(headers["ContactName"]).Optional();
            if (headers.ContainsKey("ContactType"))
                Map(m => m.ContactType).Name(headers["ContactType"]).Optional();
            if (headers.ContainsKey("FirstName"))
                Map(m => m.FirstName).Name(headers["FirstName"]).Optional();
            if (headers.ContainsKey("MiddleName"))
                Map(m => m.MiddleName).Name(headers["MiddleName"]).Optional();
            if (headers.ContainsKey("LastName"))
                Map(m => m.LastName).Name(headers["LastName"]).Optional();
            if (headers.ContainsKey("PhoneNumber"))
                Map(m => m.PhoneNumber).Name(headers["PhoneNumber"]).Optional();
            if (headers.ContainsKey("AltPhoneNumber"))
                Map(m => m.AltPhoneNumber).Name(headers["AltPhoneNumber"]).Optional();
            if (headers.ContainsKey("EmailAddress"))
                Map(m => m.EmailAddress).Name(headers["EmailAddress"]).Optional();
            if (headers.ContainsKey("AltEmailAddress"))
                Map(m => m.AltEmailAddress).Name(headers["AltEmailAddress"]).Optional();
            if (headers.ContainsKey("Notes"))
                Map(m => m.Notes).Name(headers["Notes"]).Optional();
            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
        }
    }
}
