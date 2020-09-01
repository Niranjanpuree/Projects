using Microsoft.Extensions.Caching.Memory;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Core.Services
{
    public class NaicsService : IMemoryService<Naics>, INaicsService
    {
        INaicsRepository _repository;
        public NaicsService(INaicsRepository repository, IMemoryCache memoryCache) : base(memoryCache, IMemoryService<Naics>.NAICSCode, 36000)
        {
            _repository = repository;
        }

        public Naics GetNaicsByCode(string code)
        {
            var naicsList = this.GetNaicsList();
            return naicsList.Where(x => x.Code == code).FirstOrDefault();
        }

        public Naics GetNaicsByGuid(Guid naicsGuid)
        {
            var naicsList = this.GetNaicsList();
            return naicsList.Where(x => x.NAICSGuid == naicsGuid).FirstOrDefault();
        }

        public IEnumerable<Naics> GetNaicsList()
        {
            var naicsList = this.GetMemoryList();
            if (naicsList != null)
            {
                return naicsList;
            }
            else
            {
                var result = _repository.GetNaicsList();
                this.setMemoryList(result);
                return result;
            }
        }

        public ICollection<Naics> GetNaicsList(string searchText)
        {
            return _repository.GetNaicsList(searchText);
        }
    }
}
