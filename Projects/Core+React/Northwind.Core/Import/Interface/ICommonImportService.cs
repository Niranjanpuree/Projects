using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface ICommonImportService
    {
        Organization GetOrganizationByName(string orgIDName);

        Country GetCountryByCountryName(string countryName);

        States GetStateByStateName(string stateName);

        Naics GetNaicsByCode(string naicsCode);

        Psc GetPscByCode(string pscCode);

        ResourceAttributeValue GetContractTypeByName(string contractTypeName);

        ResourceAttributeValue GetResourceAttributeValueByResourceTypeNameValue(string resourceType, string attributeName, string nameValue);

        User GetUserByFirstAndLastName(string firstName, string lastName);
    }
}
