using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMQuestionaire
    {
        public string ProjectNumber { get; set; }
        public string ContractNumber { get; set; }
        public string GSAR { get; set; }
        public string GQAC { get; set; }
        public string ServiceContractReporting { get; set; }
        public string Warranties { get; set; }

        public string Answer { get; set; }

        public string Action { get; set; }
        public string ImportStatus { get; set; }
        public string Reason { get; set; }

    }

    public sealed class DMQuestionaireHeaderMap : ClassMap<DMQuestionaire>
    {
        public DMQuestionaireHeaderMap(Dictionary<string, string> headers)
        {
            if (headers.ContainsKey("ProjectNumber"))
                Map(m => m.ProjectNumber).Name(headers["ProjectNumber"]).Optional();
            if (headers.ContainsKey("ContractNumber"))
                Map(m => m.ContractNumber).Name(headers["ContractNumber"]).Optional();
            if (headers.ContainsKey("GSAR"))
                Map(m => m.GSAR).Name(headers["GSAR"]).Optional();
            if (headers.ContainsKey("GQAC"))
                Map(m => m.GQAC).Name(headers["GQAC"]).Optional();
            if (headers.ContainsKey("ProgressTowardsSB"))
                Map(m => m.ServiceContractReporting).Name(headers["ServiceContractReporting"]).Optional();
            if (headers.ContainsKey("Warranties"))
                Map(m => m.Warranties).Name(headers["Warranties"]).Optional();

            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
        }
    }
}
