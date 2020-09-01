using System;

namespace Northwind.Core.Entities
{
    public class CustomerType : BaseModel
    {
        public Guid CustomerTypeGuid { get; set; }
        public string CustomerTypeName { get; set; }
    }
}
