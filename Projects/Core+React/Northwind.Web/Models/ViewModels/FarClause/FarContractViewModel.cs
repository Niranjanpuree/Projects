using Northwind.Core.Entities;
using Northwind.Web.Models.ViewModels.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.FarClause
{
    public class FarContractViewModel
    {
        public ContractQuestionaireViewModel ContractQuestionaires { get; set; }
        public List<FarContractDetail> RequiredFarClauses { get; set; }
        public List<FarContractDetail> ApplicableFarClauses { get; set; }
        public List<string> SelectedValuesList { get; set; }
        public Guid ContractGuid { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string FarContractTypeName { get; set; }
        public string FarContractTypeCode { get; set; }
     
        public IList<Questionaires> Questionniare { get; set; }
    }
}
