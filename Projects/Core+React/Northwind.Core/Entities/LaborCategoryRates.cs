using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities
{
    public class LaborCategoryRates:BaseModel
    {
        public Guid CategoryRateGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public string UploadFileName { get; set; }
        public IFormFile FileToUpload { get; set; }
        public bool IsCsv { get; set; }

        public string SubContractor { get; set; }
        public string LaborCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Rate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
