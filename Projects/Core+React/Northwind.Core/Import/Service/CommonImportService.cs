using Northwind.Core.Entities;
using Northwind.Core.Import.Interface;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class CommonImportService : ICommonImportService
    {
        ICompanyService _companyService;
        ICountryService _countryService;
        IStateService _stateService;
        INaicsService _naicsService;
        IPscService _pscService;
        IResourceAttributeValueService _resourceAttributeValueService;
        IUserService _userService;

        public CommonImportService(ICompanyService companyService, ICountryService countryService, IStateService stateService,
            INaicsService naicsService, IPscService pscService, IResourceAttributeValueService resourceAttributeValueService,
            IUserService userService)
        {
            _companyService = companyService;
            _countryService = countryService;
            _stateService = stateService;
            _naicsService = naicsService;
            _pscService = pscService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _userService = userService;
        }

        public Organization GetOrganizationByName(string orgIDName)
        {
            if (string.IsNullOrWhiteSpace(orgIDName))
                return null;
            return _companyService.GetOrganizationByName(orgIDName);
        }

        public Country GetCountryByCountryName(string countryName)
        {
            return _countryService.GetCountryByCountryName(countryName);
        }

        public States GetStateByStateName(string stateName)
        {
            return _stateService.GetStateByStateName(stateName);
        }

        public Naics GetNaicsByCode(string naicsCode)
        {
            return _naicsService.GetNaicsByCode(naicsCode);
        }

        public Psc GetPscByCode(string pscCode)
        {
            return _pscService.GetPscByCode(pscCode).FirstOrDefault();
        }

        public ResourceAttributeValue GetContractTypeByName(string contractTypeName)
        {
            return _resourceAttributeValueService.GetResourceAttributeValueByValue(contractTypeName);
        }

        public ResourceAttributeValue GetResourceAttributeValueByResourceTypeNameValue(string resourceType, string attributeName, string nameValue)
        {
            return _resourceAttributeValueService.GetResourceAttributeValueByResourceTypeNameValue(resourceType,attributeName,nameValue);
        }

        public User GetUserByFirstAndLastName(string firstName, string lastName)
        {
            return _userService.GetUserByFirstAndLastName(firstName,lastName);
        }
    }
}
