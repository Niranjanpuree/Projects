using System;

namespace Northwind.Core.Entities
{
    public class Country: BaseModel
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryISO { get; set; }
    }
}
