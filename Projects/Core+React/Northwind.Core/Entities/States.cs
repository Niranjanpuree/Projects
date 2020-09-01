using System;

namespace Northwind.Core.Entities
{
    public class States: BaseModel
    {
        public Guid StateId { get; set; }
        public Guid CountryId { get; set; }
        public string StateName { get; set; }
        public string Abbreviation { get; set; }
        public bool GRT { get; set; }
    }
}
