using System;
using System.Collections.Generic;

namespace Northwind.Core.Entities
{

    public class Region : BaseModel
    {
        public Guid RegionGuid { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public Guid RegionalManager { get; set; }
        public Guid BusinessDevelopmentRegionalManager { get; set; }
        public Guid HSRegionalManager { get; set; }
        public Guid DeputyRegionalManager { get; set; }
        public string ManagerName { get; set; }
        public string DeputyManagerName { get; set; }
        public string BDManagerName { get; set; }
        public string HSManagerName { get; set; }
        public string RoleType { get; set; }

    }

    public class RegionUserRoleMapping
    {
        public Guid RegionUserRoleMappingGuid { get; set; }
        //public Guid RegionMappingGuid { get; set; }
        public Guid RegionGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string RoleType { get; set; }
        public string Keys { get; set; }
    }
}
