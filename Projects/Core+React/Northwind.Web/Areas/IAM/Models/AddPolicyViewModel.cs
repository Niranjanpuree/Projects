using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Areas.IAM.Models
{
    public class AddPolicyViewModel
    {
        public string Resource { get; set; }
        public List<SelectListItem> Resources { get; set; }
        public AddPolicyViewModel()

        {
            Resources = new List<SelectListItem>();
            Resources.Add(new SelectListItem("Select Resources...", "0"));

        }
    }
}
