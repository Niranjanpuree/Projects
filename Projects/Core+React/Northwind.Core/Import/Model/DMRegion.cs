using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMRegion
    {
        [Required]
        public string RegionName { get; set; }
        [Required]
        public string RegionCode { get; set; }
        [Required]
        public string Manager { get; set; }
        public string Action { get; set; }

        public Guid RegionalManagerGuid { get; set; }
        public string ImportStatus { get; set; }
        public string Reason { get; set; }

    }


    public sealed class RegionHeaderMap : ClassMap<DMRegion>
    {
        public RegionHeaderMap(Dictionary<string, string> headers)
        {
            if (headers.ContainsKey("RegionName"))
                Map(m => m.RegionName).Name(headers["RegionName"]).Optional();
            if (headers.ContainsKey("RegionCode"))
                Map(m => m.RegionCode).Name(headers["RegionCode"]).Optional();
            if (headers.ContainsKey("Manager"))
                Map(m => m.Manager).Name(headers["Manager"]).Optional();
            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();

        }
    }
}
