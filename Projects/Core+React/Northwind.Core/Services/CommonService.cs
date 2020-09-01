using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Microsoft.AspNetCore.Http;

namespace Northwind.Core.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _iCommonRepository;
        public CommonService(ICommonRepository CommonRepository)
        {
            _iCommonRepository = CommonRepository;
        }
        public string FilePost(string location, IFormFile fileToUpload)
        {
            return _iCommonRepository.FilePost(location, fileToUpload);
        }
        public bool CheckReviewStage(Guid moduleGuid,Guid userGuid)
        {
            return _iCommonRepository.CheckReviewStage(moduleGuid, userGuid);
        }
    }
}
