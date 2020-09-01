using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface ICountryService
    {
        Guid GetCountryGuidBy3DigitCode(string Code);
        IEnumerable<Country> GetCountryList();

        Country GetCountryByGuid(Guid guid);

        Guid GetCountryGuidByName(string countryName);

        string GetCountryAsString(string countryGuid);
        Country GetCountryByCountryName(string countryName);
    }
}
