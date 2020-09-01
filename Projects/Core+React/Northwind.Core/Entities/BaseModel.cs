using System;
using System.Collections.Generic;

namespace Northwind.Core.Entities
{
    public class BaseModel
    {
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public String CreatedByName { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string SearchValue { get; set; }
    }
}
