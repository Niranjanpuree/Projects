using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Interfaces.CrossSiteInterface;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models.ViewModels;

namespace Northwind.Web.Infrastructure
{
    public class ResourceAttributeController : Controller
    {
        IResourceService _resourceService;
        private readonly IContractServiceCrossSite _contractService;
        IResourceAttributeValueService _resourceAttributeValueService;
        IUserService _userService;
        ICompanyService _companyService;

        public ResourceAttributeController(IResourceService resourceService,
            IContractServiceCrossSite contractService,
            IUserService userService,
            ICompanyService companyService,
        IResourceAttributeValueService resourceAttributeValueService)
        {
            _resourceService = resourceService;
            _contractService = contractService;
            _userService = userService;
            _companyService = companyService;
            _resourceAttributeValueService = resourceAttributeValueService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get(List<string> resourceIds)
        {
            return Json(_resourceService.GetResourceAttributes(resourceIds));
        }

        [HttpPost("~/ResourceAttribute/GetOptions")]
        public IActionResult GetOptions([FromBody]AttributeSearchParam postvalue)
        {
            if (postvalue.IsEntityLookup == false)
            {
                var resource = _resourceService.GetResourceAttribute(postvalue.AttributeId);
                foreach (var res in resource)
                {
                    var result = _resourceAttributeValueService.GetResourceAttributeOptionsByResourceAttributeGuid(postvalue.AttributeId);
                    var result1 = new List<dynamic>();

                    foreach (var row in result)
                    {
                        result1.Add(new
                        {
                            id = res.Name,
                            Name = row.Name,
                            Value = row.Value
                        });
                    }
                    return Json(result1);
                }
            }
            else
            {
                var resource = _resourceService.GetResourceAttribute(postvalue.AttributeId);
                foreach (var res in resource)
                {
                    return Json(GetEntityList(postvalue.Entity, res.Name));
                }
            }
            return null;
        }

        private IEnumerable<dynamic> GetEntityList(string entity, string resourceName)
        {
            switch (entity)
            {
                case "Company":
                    return GetCompanies(resourceName);
            }
            return null;
        }

        private List<dynamic> GetCompanies(string resourceName)
        {
            var list = new List<dynamic>();
            //var result = _companyService.GetAllWithOrg("", 1000000, 0, "CompanyName", "asc");
            //foreach(var company in result)
            //{
            //    lst.Add(new {
            //        id = resourceName,
            //        Name = company.CompanyCode,
            //        Value = company.CompanyGuid
            //    });
            //}

            var companyList = _companyService.GetCompanyList();
            foreach (var n in companyList)
            {
                list.Add(new
                {
                    id = resourceName,
                    Name = n.CompanyCode,
                    Value = n.CompanyCode
                });
            }
            return list;
        }

    }
}