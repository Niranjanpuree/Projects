using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class CountryService : IMemoryService<Country>, ICountryService
    {
        private readonly ICountryRepository _icountryRepository;
        private IEnumerable<Country> countryList;
        public CountryService(ICountryRepository countryRepository, IMemoryCache memoryCache) : base(memoryCache, IMemoryService<Naics>.NAICSCode, 36000)
        {
            _icountryRepository = countryRepository;
            countryList = this.GetCountryList();
        }
        public Guid GetCountryGuidBy3DigitCode(string Code)
        {
            return _icountryRepository.GetCountryGuidBy3DigitCode(Code);
        }

        public IEnumerable<Country> GetCountryList()
        {
            var countryList = this.GetMemoryList();
            if (countryList != null)
            {
                return countryList;
            }
            else
            {
                var countryDbList = _icountryRepository.GetCountryList();
                this.setMemoryList(countryDbList);
                return countryDbList;
            }
        }

        public Country GetCountryByGuid(Guid guid)
        {
            return countryList.Where(x => x.CountryId == guid).FirstOrDefault();
        }

        public string GetCountryAsString(string countryGuid)
        {

            return _icountryRepository.GetCountryAsString(countryGuid);
        }

        public Guid GetCountryGuidByName(string countryName)
        {
            return _icountryRepository.GetCountryGuidByName(countryName);
        }

        public Country GetCountryByCountryName(string countryName)
        {
            return _icountryRepository.GetCountryByCountryName(countryName);
        }
    }
}
