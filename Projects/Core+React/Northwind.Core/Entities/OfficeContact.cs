using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities
{
   public class OfficeContact:BaseModel
    {
        public Guid ContactGuid { get; set; }
        public Guid OfficeGuid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Guid ContactType { get; set; }
        public string PhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AltEmailAddress { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactTypeName { get; set; }
        public string OfficeName { get; set; }
    }
}
