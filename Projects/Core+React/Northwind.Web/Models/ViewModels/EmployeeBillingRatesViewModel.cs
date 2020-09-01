using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class EmployeeBillingRatesViewModel : BaseViewModel
    {
        public Guid BillingRateGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public Guid ContractResourceFileGuid { get; set; }
        public string UploadFileName { get; set; }
        public IFormFile FileToUpload { get; set; }
        public bool IsCsv { get; set; }
        public string Displayname { get; set; }

        public string LaborCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Rate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int sn { get; set; }

        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string UpdatedByDisplayname { get; set; }
    }
}
