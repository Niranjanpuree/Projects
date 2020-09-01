using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Web.Models.ViewModels.FarClause
{
    public class FarContractTypeViewModel
    {
        public Guid FarContractTypeGuid { get; set; }
        [Required]
        public string Code { get; set; }
        public string DisplayCode { get { return Code.Replace("&", ""); } }
        [Required]
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
    }
}
