using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMOffice
    {
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string PhysicalAddress { get; set; }
        public string PhysicalAddress1 { get; set; }
        public string PhysicalCountry { get; set; }
        public string PhysicalState { get; set; }
        public string PhysicalCity { get; set; }
        public string PhysicalZipCode { get; set; }
        public string MailingAddress { get; set; }
        public string MailingAddress1 { get; set; }
        public string MailingCountry { get; set; }
        public string MailingState { get; set; }
        public string MailingCity { get; set; }
        public string MailingZipCode { get; set; }
        public string Action { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string OperationManager { get; set; }

        public bool IsValid { get; set; }
        public bool IsPartialValid { get; set; }

        public string ImportStatus { get; set; }
        public string Reason { get; set; }
        public Guid PhysicalCountryGuid { get; set; }
        public Guid PhysicalStateGuid { get; set; }
        public Guid MailingCountryGuid { get; set; }
        public Guid MailingStateGuid { get; set; }
        public Guid OperationManagerGuid { get; set; }
    }

    public sealed class OfficeHeaderMap : ClassMap<DMOffice>
    {
        public OfficeHeaderMap(Dictionary<string, string> headers)
        {
            if (headers.ContainsKey("OfficeCode"))
                Map(m => m.OfficeCode).Name(headers["OfficeCode"]).Optional();
            if (headers.ContainsKey("OfficeName"))
                Map(m => m.OfficeName).Name(headers["OfficeName"]).Optional();
            if (headers.ContainsKey("PhysicalAddress"))
                Map(m => m.PhysicalAddress).Name(headers["PhysicalAddress"]).Optional();
            if (headers.ContainsKey("PhysicalAddress1"))
                Map(m => m.PhysicalAddress1).Name(headers["PhysicalAddress1"]).Optional();
            if (headers.ContainsKey("PhysicalCountry"))
                Map(m => m.PhysicalCountry).Name(headers["PhysicalCountry"]).Optional();
            if (headers.ContainsKey("PhysicalState"))
                Map(m => m.PhysicalState).Name(headers["PhysicalState"]).Optional();
            if (headers.ContainsKey("PhysicalCity"))
                Map(m => m.PhysicalCity).Name(headers["PhysicalCity"]).Optional();
            if (headers.ContainsKey("PhysicalZipCode"))
                Map(m => m.PhysicalZipCode).Name(headers["PhysicalZipCode"]).Optional();
            if (headers.ContainsKey("MailingAddress"))
                Map(m => m.MailingAddress).Name(headers["MailingAddress"]).Optional();
            if (headers.ContainsKey("MailingAddress1"))
                Map(m => m.MailingAddress1).Name(headers["MailingAddress1"]).Optional();
            if (headers.ContainsKey("MailingCountry"))
                Map(m => m.MailingCountry).Name(headers["MailingCountry"]).Optional();
            if (headers.ContainsKey("MailingState"))
                Map(m => m.MailingState).Name(headers["MailingState"]).Optional();
            //if (headers.ContainsKey("MailingCity"))
            //Map(m => m.MailingCity).Name(headers["MailingCity"]);
            if (headers.ContainsKey("MailingZipCode"))
                Map(m => m.MailingZipCode).Name(headers["MailingZipCode"]).Optional();
            if (headers.ContainsKey("Phone"))
                Map(m => m.Phone).Name(headers["Phone"]).Optional();
            if (headers.ContainsKey("Fax"))
                Map(m => m.Fax).Name(headers["Fax"]).Optional();
            if (headers.ContainsKey("OperationManager"))
                Map(m => m.OperationManager).Name(headers["OperationManager"]).Optional();
            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
        }
    }
}
