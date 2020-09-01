using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.PFS.Web.Models.ViewModels
{
    public class WbsDictionaryViewModel
    {
        public Guid WbsDictionaryGuid { get; set; }
        public string ProjectNumber { get; set; }
        public string WbsNumber { get; set; }
        public string WbsDictionaryTitle { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName  { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
