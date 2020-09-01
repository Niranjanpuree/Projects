using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Web.Models.ViewModels.FarClause
{
    public class FarClauseViewModel
    {
        public Guid FarClauseGuid { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Title { get; set; }
        public string Paragraph { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UpdatedBy { get; set; }
        public List<FarContractTypeClauseViewModel> FarContractTypeClauseViewModels { get; set; }
    }
}
