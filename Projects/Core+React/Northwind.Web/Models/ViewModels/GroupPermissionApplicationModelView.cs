using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class GroupPermissionApplicationModelView
    {
        public string Application { get; set; }
        public IEnumerable<GroupResourcePermission> Resources { get; set; }
    }
}
