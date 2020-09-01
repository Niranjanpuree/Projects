using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Areas.IAM.Models.IAM.ViewModels
{
    public class GroupViewModel
    {
        public Guid GroupGuid { get; set; }
        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter group name")]
        public string CN { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string UserGuid { get; set; }
    }
}
