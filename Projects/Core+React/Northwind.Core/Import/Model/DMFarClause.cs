using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMFarClause
    {
        public string ProjectNumber { get; set; }
        public string ContractNumber { get; set; }
        public string CPARS { get; set; }
        public string GovernmentFurnished { get; set; }
        public string ProgressTowardsSB { get; set; }
        public string ReportingExecutiveCompensation { get; set; }

        public string Action { get; set; }
        public string ImportStatus { get; set; }
        public string Reason { get; set; }

        public Guid FarContractTypeGuid { get; set; }

        public string FarClause { get; set; }
        public string IsApplicableFarClause { get; set; }
    }

    public sealed class DMFarClauseHeaderMap : ClassMap<DMFarClause>
    {
        public DMFarClauseHeaderMap(Dictionary<string, string> headers)
        {
            if (headers.ContainsKey("ProjectNumber"))
                Map(m => m.ProjectNumber).Name(headers["ProjectNumber"]).Optional();
            if (headers.ContainsKey("ContractNumber"))
                Map(m => m.ContractNumber).Name(headers["ContractNumber"]).Optional();
            if (headers.ContainsKey("CPARS"))
                Map(m => m.CPARS).Name(headers["CPARS"]).Optional();
            if (headers.ContainsKey("GovernmentFurnished"))
                Map(m => m.GovernmentFurnished).Name(headers["GovernmentFurnished"]).Optional();
            if (headers.ContainsKey("ProgressTowardsSB"))
                Map(m => m.ProgressTowardsSB).Name(headers["ProgressTowardsSB"]).Optional();
            if (headers.ContainsKey("ReportingExecutiveCompensation"))
                Map(m => m.ReportingExecutiveCompensation).Name(headers["ReportingExecutiveCompensation"]).Optional();

            if (headers.ContainsKey("FarClause"))
                Map(m => m.FarClause).Name(headers["FarClause"]).Optional();
            if (headers.ContainsKey("IsApplicableFarClause"))
                Map(m => m.IsApplicableFarClause).Name(headers["IsApplicableFarClause"]).Optional();

            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
        }
    }
}
