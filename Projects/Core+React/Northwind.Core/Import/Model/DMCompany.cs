using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMCompany
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string President { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }

        public string ImportStatus { get; set; }
        public string Reason { get; set; }
        public Guid PresidentGuid { get; set; }
        public bool IsValid { get; set; }
    }

    public sealed class CompanyHeaderMap : ClassMap<DMCompany>
    {
        public CompanyHeaderMap(Dictionary<string, string> headers)
        {
            if(headers.ContainsKey("Code"))
                Map(m => m.Code).Name(headers["Code"]).Optional();
            if (headers.ContainsKey("Name"))
                Map(m => m.Name).Name(headers["Name"]).Optional();
            if (headers.ContainsKey("President"))
                Map(m => m.President).Name(headers["President"]).Optional();
            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
            if (headers.ContainsKey("Abbreviation"))
                Map(m => m.Abbreviation).Name(headers["Abbreviation"]).Optional();
            if(headers.ContainsKey("Description"))
                Map(m => m.Description).Name(headers["Description"]).Optional();
        }
    }
}
