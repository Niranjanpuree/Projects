using Northwind.Core.Entities;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Microsoft.AspNetCore.Http;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Core.Interfaces
{
    public interface IContractResourceFileService
    {
        ContractResourceFile GetFilePathByResourceIdAndKeys(string resourceKey, Guid resourceId);
    }
}
