using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Core.Utilities;

namespace Northwind.Web.Areas.IAM.Models.IAM.ViewModels
{
    public class UserGroupViewModel
    {
        public Guid UserGuid { get; set; }
        public string GroupGuid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
