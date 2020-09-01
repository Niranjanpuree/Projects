using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.Core.Utilities;

namespace Northwind.Core.Entities
{
    public class Office : BaseModel
    {
        public Guid OfficeGuid { get; set; }
        public Guid OperationManagerGuid { get; set; }
        public string OfficeName { get; set; }
        public string PhysicalAddress { get; set; }
        public string PhysicalAddressLine1 { get; set; }
        public string PhysicalCity { get; set; }
        public string PhysicalZipCode { get; set; }
        public Guid? PhysicalStateId { get; set; }
        public Guid PhysicalCountryId { get; set; }
        public string MailingAddress { get; set; }
        public string MailingAddressLine1 { get; set; }
        public string MailingCity { get; set; }
        public string MailingZipCode { get; set; }
        public Guid? MailingStateId { get; set; }
        public Guid? MailingCountryId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string OfficeCode { get; set; }
        public string StatesName { get; set; }
        public string CountryName { get; set; }
        public string PhysicalStateName { get; set; }
        public string PhysicalCountryName { get; set; }
        public string MailingStateName { get; set; }
        public string MailingCountryName { get; set; }
        public string OperationManagerName { get; set; }
    }
}