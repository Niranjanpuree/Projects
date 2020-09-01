using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.RevenueRecognition
{
    public class RevenueContractExtensionViewModel : BaseViewModel
    {
        public Guid RevenueGuid { get; set; }
        public Guid ContractExtensionGuid { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ExtensionDate { get; set; }
        public string ExtensionDateString { get { return (ExtensionDate != null ? ExtensionDate.Value.ToString("MM/dd/yyyy") : "N/A"); }}
    }
}
