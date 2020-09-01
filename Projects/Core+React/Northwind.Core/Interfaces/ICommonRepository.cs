using Northwind.Core.Entities;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Microsoft.AspNetCore.Http;

namespace Northwind.Core.Interfaces
{
    public interface ICommonRepository
    {
        string FilePost(string location, IFormFile fileToUpload);
        bool CheckReviewStage(Guid moduleGuid,Guid userGuid);
    }
}
