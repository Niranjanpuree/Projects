using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.PFS.Web.Models.ViewModels
{
    public class WbsViewModel
    {
        public Guid WbsGuid { get; set; }
        public string ProjectNumber { get; set; }
        public string Wbs { get; set; }
        public string Description { get; set; }
        public bool AllowCharging { get; set; }
        public List<string> WbsDictionaryTitle { get; set; }
    }
}
