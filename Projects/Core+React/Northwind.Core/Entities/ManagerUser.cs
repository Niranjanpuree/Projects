using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class ManagerUser
    {
        public Guid ManagerUserGUID { get; set; }
        public Guid ManagerGUID { get; set; }
        public Guid UserGUID { get; set; }
    }
}
