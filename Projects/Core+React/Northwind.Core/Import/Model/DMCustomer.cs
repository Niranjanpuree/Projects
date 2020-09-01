using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMCustomer
    {

        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public Guid? StateId { get; set; }
        public Guid CountryId { get; set; }
        public Guid CustomerTypeGuid { get; set; }

        public string CustomerDescription { get; set; }
        public string PrimaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        public string Abbreviations { get; set; }
        public string Tags { get; set; }
        public string Url { get; set; }
        public string Department { get; set; }
        public string CustomerCode { get; set; }
        public string Agency { get; set; }
        public string CustomerType { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Action { get; set; }
        public string ImportStatus { get; set; }
        public string Reason { get; set; }

        public bool IsValid { get; set; }
        public Guid CustomerGuid { get; set; }
    }

    public sealed class CustomerHeaderMap : ClassMap<DMCustomer>
    {
        public CustomerHeaderMap(Dictionary<string, string> headers)
        {
            if(headers.ContainsKey("CustomerName"))
                Map(m => m.CustomerName).Name(headers["CustomerName"]).Optional();
            if (headers.ContainsKey("Address"))
                Map(m => m.Address).Name(headers["Address"]).Optional();
            //Map(m => m.AddressLine1).Name(headers["AddressLine1"]).Optional();
            if (headers.ContainsKey("City"))
                Map(m => m.City).Name(headers["City"]).Optional();
            if (headers.ContainsKey("ZipCode"))
                Map(m => m.ZipCode).Name(headers["ZipCode"]).Optional();
            if (headers.ContainsKey("CustomerDescription"))
                Map(m => m.CustomerDescription).Name(headers["CustomerDescription"]).Optional();
            if (headers.ContainsKey("PrimaryPhone"))
                Map(m => m.PrimaryPhone).Name(headers["PrimaryPhone"]).Optional();
            if (headers.ContainsKey("PrimaryEmail"))
                Map(m => m.PrimaryEmail).Name(headers["PrimaryEmail"]).Optional();
            if (headers.ContainsKey("Abbreviations"))
                Map(m => m.Abbreviations).Name(headers["Abbreviations"]).Optional();
            if (headers.ContainsKey("Department"))
                Map(m => m.Department).Name(headers["Department"]).Optional();
            if (headers.ContainsKey("Agency"))
                Map(m => m.Agency).Name(headers["Agency"]).Optional();
            if (headers.ContainsKey("CustomerType"))
                Map(m => m.CustomerType).Name(headers["CustomerType"]).Optional();
            if (headers.ContainsKey("State"))
                Map(m => m.State).Name(headers["State"]).Optional();
            if (headers.ContainsKey("Country"))
                Map(m => m.Country).Name(headers["Country"]).Optional();
            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
        }
    }
}
