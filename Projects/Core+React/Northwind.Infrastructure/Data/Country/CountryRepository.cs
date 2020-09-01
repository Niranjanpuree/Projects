using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Infrastructure.Data.Country
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDatabaseContext _context;

        public CountryRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public Guid GetCountryGuidBy3DigitCode(string Code)
        {
            var data = _context.Connection.QueryFirstOrDefault<Guid>("SELECT CountryId FROM Country where Alpha3Code = @Alpha3Code", new { Alpha3Code = Code });
            return data;
        }

        public IEnumerable<Core.Entities.Country> GetCountryList()
        {
            var data = _context.Connection.Query<Core.Entities.Country>("SELECT * FROM Country order by CountryName asc");
            return data;
        }

        public Core.Entities.Country GetCountryByGuid(Guid guid)
        {
            var data = _context.Connection.QueryFirstOrDefault<Core.Entities.Country>("SELECT * FROM Country WHERE CountryId = @countryGuid", new { countryGuid = guid });
            return data;
        }

        public string GetCountryAsString(string countryGuid)
        {
            var placeOfPerformanceSelected = string.Empty;
            // to fetch States name through state id array..
            if (!string.IsNullOrEmpty(countryGuid))
            {
                var stateIdArr = countryGuid.Split(',');
                var stateIdArrWithStringCote = stateIdArr.Select(x => string.Format("'" + x + "'"));
                var formatQuery = string.Join(",", stateIdArrWithStringCote);
                var stateQuery = $"select StateName from State where StateId in ({formatQuery})";
                var stateNameArr = _context.Connection.Query<string>(stateQuery);
                placeOfPerformanceSelected = string.Join(", ", stateNameArr);
            }
            return placeOfPerformanceSelected;
        }

        public Core.Entities.Country GetCountryByCountryName(string countryName)
        {
            var data = _context.Connection.QueryFirstOrDefault<Core.Entities.Country>("SELECT * FROM Country WHERE CountryName = @countryName", new { countryName = countryName });
            return data;
        }

        public Guid GetCountryGuidByName(string countryName)
        {
            var data = _context.Connection.QueryFirstOrDefault<Guid>("SELECT CountryId FROM Country WHERE (CountryName = @CountryName or Alpha3Code = @CountryName)", new { CountryName = countryName });
            return data;
        }
    }
}
