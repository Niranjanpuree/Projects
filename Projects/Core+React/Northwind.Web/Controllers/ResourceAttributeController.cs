using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Models.ViewModels;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class ResourceAttributeController : Controller
    {
        IResourceService _resourceService;
        INaicsService _naicsService;
        IPscService _pscService;
        ICountryService _countryService;
        IStateService _statesService;
        ICustomerService _customerService;
        ICustomerContactService _customerContactService;
        IContractsService _contractService;
        IResourceAttributeValueService _resourceAttributeValueService;
        IUserService _userService;
        ICompanyService _companyService;
        IRegionService _regionService;

        public ResourceAttributeController(IResourceService resourceService,
            INaicsService naicsService,
            IPscService pscService,
            ICountryService countryService,
            IStateService statesService,
            ICustomerService customerService,
            ICustomerContactService customerContactService,
            IContractsService contractService,
            IUserService userService,
            ICompanyService companyService,
            IRegionService regionService,
        IResourceAttributeValueService resourceAttributeValueService)
        {
            _resourceService = resourceService;
            _naicsService = naicsService;
            _pscService = pscService;
            _countryService = countryService;
            _statesService = statesService;
            _customerService = customerService;
            _customerContactService = customerContactService;
            _contractService = contractService;
            _userService = userService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _companyService = companyService;
            _regionService = regionService;
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
            if(postvalue.QueryOperator == OperatorName.IsEmpty)
            {
                var resource = _resourceService.GetResourceAttribute(postvalue.AttributeId).ToList();
                var result = new List<dynamic>();
                result.Add(new { id = resource[0].Name, Name = "Yes", Value = true });
                result.Add(new { id = resource[0].Name, Name = "No", Value = false });
                return Json(result);
            }
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
            if (isKeyPersonnel(entity))
            {
                return GetKeyPersonnels(entity, entity);
            }
            switch (entity)
            {
                case "NaicsCode":
                    return GetNaicsList(resourceName);
                case "OrgID":
                    return GetOrgIDs(resourceName);
                case "PSCCode":
                    return GetPSCList(resourceName);
                case "Country":
                    return GetCountries(resourceName);
                case "State":
                    return GetStates(resourceName);
                case "Customer":
                    return GetCustomers(resourceName);
                case "CustomerContact":
                    return GetCustomerContact(resourceName);
                case "User":
                    return GetUserList(resourceName);
                case "Company":
                    return GetCompany(resourceName);
                case "Region":
                    return GetRegion(resourceName);
            }
            return null;
        }

        private bool isKeyPersonnel(string name)
        {
            if (name == ContractUserRole._accountRepresentative)
                return true;
            else if (name == ContractUserRole._companyPresident)
                return true;
            else if (name == ContractUserRole._contractRepresentative)
                return true;
            else if (name == ContractUserRole._projectControls)
                return true;
            else if (name == ContractUserRole._projectManager)
                return true;
            else if (name == ContractUserRole._regionalManager)
                return true;
            else if (name == ContractUserRole._subContractAdministrator)
                return true;
            else if (name == ContractUserRole._purchasingRepresentative)
                return true;
            else if (name == ContractUserRole._humanResourceRepresentative)
                return true;
            else if (name == ContractUserRole._qualityRepresentative)
                return true;
            else if (name == ContractUserRole._safetyOfficer)
                return true;
            else if (name == ContractUserRole._operationManager)
                return true;
            return false;
        }

        private List<dynamic> GetNaicsList(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstNaics = _naicsService.GetNaicsList();
            foreach (var n in lstNaics)
            {
                lst.Add(new
                {
                    id = resourceName,
                    Name = string.Concat(n.Code + " " + n.Title),
                    Value = n.NAICSGuid
                });
            }
            return lst;
        }

        private List<dynamic> GetPSCList(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstPsc = _pscService.GetPscs();
            foreach (var n in lstPsc)
            {
                lst.Add(new
                {
                    id = resourceName,
                    Name = n.CodeDescription,
                    Value = n.PSCGuid
                });
            }
            return lst;
        }

        private List<dynamic> GetCountries(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstCountry = _countryService.GetCountryList();
            foreach (var n in lstCountry)
            {
                lst.Add(new
                {
                    id = resourceName,
                    Name = n.CountryName,
                    Value = n.CountryId
                });
            }
            return lst;
        }

        private List<dynamic> GetStates(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstState = _statesService.GetStatesList();
            foreach (var n in lstState)
            {
                lst.Add(new
                {
                    id = resourceName,
                    Name = n.StateName,
                    Value = n.StateId
                });
            }
            return lst;
        }

        private List<dynamic> GetCustomers(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstCustomer = _customerService.GetCustomerList();
            foreach (var n in lstCustomer)
            {
                lst.Add(new
                {
                    id = resourceName,
                    Name = n.CustomerName,
                    Value = n.CustomerGuid
                });
            }
            return lst;
        }

        private List<dynamic> GetUserList(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstUser = _userService.GetUsers();
            foreach (var n in lstUser)
            {
                lst.Add(new
                {
                    id = resourceName,
                    Name = FormatHelper.FormatFullName(n.Firstname, string.Empty, n.Lastname),
                    Value = n.UserGuid
                });
            }
            return lst;
        }

        private List<dynamic> GetCustomerContact(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstCustomerContact = _customerContactService.GetCustomerContactList();
            foreach (var n in lstCustomerContact)
            {
                var contactName = n.FirstName + n.LastName;
                lst.Add(new
                {
                    id = resourceName,
                    Name = string.Concat(n.FirstName + " " + n.MiddleName + " " + n.LastName),
                    Value = n.ContactGuid
                });
            }
            return lst;
        }

        private List<dynamic> GetOrgIDs(string resourceName)
        {
            var lst = new List<dynamic>();
            var lstOrg = _contractService.GetOrganizationData("");
            foreach (var n in lstOrg)
            {
                lst.Add(new
                {
                    id = resourceName,
                    Name = n.Name,
                    Value = n.OrgIDGuid
                });
            }
            return lst;
        }

        private List<dynamic> GetKeyPersonnels(string entity, string resourceName)
        {
            var lst = new List<dynamic>();
            var lstUser = _contractService.GetKeyPersonnels(entity);
            foreach (var n in lstUser)
            {
                if (!lst.Exists(c => c.Value == n.User.UserGuid.ToString()))
                {
                    lst.Add(new
                    {
                        id = resourceName,
                        Name = $"{n.User.Firstname} {n.User.Lastname}",
                        Value = n.User.UserGuid.ToString()
                    });
                }

            }
            return lst;
        }

        private List<dynamic> GetCompany(string resourceName)
        {
            var list = new List<dynamic>();
            var companyList = _companyService.GetCompanyList();
            foreach (var n in companyList)
            {
                list.Add(new
                {
                    id = resourceName,
                    Name = n.CompanyName + "(" + n.CompanyCode + ")",
                    Value = n.CompanyCode
                });
            }
            return list;
        }

        private List<dynamic> GetRegion(string resourceName)
        {
            var list = new List<dynamic>();
            var regionList = _regionService.GetRegionList();
            foreach (var region in regionList)
            {
                list.Add(new
                {
                    id = resourceName,
                    Name = region.RegionName + "(" + region.RegionCode + ")",
                    Value = region.RegionCode
                });
            }
            return list;
        }
    }
}