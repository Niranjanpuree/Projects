using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMMods
    {
        public string ProjectNumber { get; set; }
        public string ModNumber { get; set; }
        public string ModTitle { get; set; }
        public string POPStart { get; set; }
        public string POPEnd { get; set; }
        public string AwardAmount { get; set; }
        public string FundingAmount { get; set; }
        public string Action { get; set; }

        public string ImportStatus { get; set; }
        public string Reason { get; set; }

        public Guid ContractGuid { get; set; }
        public Guid ContractModificationGuid { get; set; }
        public bool IsValid { get; set; }

    }

    public sealed class ModsHeaderMap : ClassMap<DMMods>
    {
        public ModsHeaderMap(Dictionary<string, string> headers)
        {
            if(headers.ContainsKey("ProjectNumber"))
                Map(m => m.ProjectNumber).Name(headers["ProjectNumber"]).Optional();
            if (headers.ContainsKey("ModNumber"))
                Map(m => m.ModNumber).Name(headers["ModNumber"]).Optional();
            if (headers.ContainsKey("ModTitle"))
                Map(m => m.ModTitle).Name(headers["ModTitle"]).Optional();
            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
            if (headers.ContainsKey("POPStart"))
                Map(m => m.POPStart).Name(headers["POPStart"]).Optional();
            if (headers.ContainsKey("POPEnd"))
                Map(m => m.POPEnd).Name(headers["POPEnd"]).Optional();
            if (headers.ContainsKey("AwardAmount"))
                Map(m => m.AwardAmount).Name(headers["AwardAmount"]).Optional();
            if (headers.ContainsKey("FundingAmount"))
                Map(m => m.FundingAmount).Name(headers["FundingAmount"]).Optional();
        }
    }
}
