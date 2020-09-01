﻿using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class BaseViewModel
    {
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string SearchValue { get; set; }
        public string IsActiveStatus { get { return IsActive == true ? EnumGlobal.ActiveStatus.Active.ToString() : EnumGlobal.ActiveStatus.Inactive.ToString(); } }
    }
}
