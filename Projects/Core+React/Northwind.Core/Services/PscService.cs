using Microsoft.Extensions.Caching.Memory;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class PscService : IMemoryService<Psc>, IPscService
    {
        IPscRepository _repository;
        public PscService(IPscRepository repository, IMemoryCache memoryCache): base(memoryCache, IMemoryService<Psc>.PSCCode, 36000)
        {
            _repository = repository;
        }

        public IEnumerable<Psc> GetPscByCode(string code)
        {

            //later added to get psc by code from memory
            //var pscList = this.GetMemoryList();
            //if(pscList != null)
            //    return 

            return _repository.GetPscByCode(code);
        }

        public IEnumerable<Psc> GetPscByGuid(Guid pscGuid)
        {
            return _repository.GetPscByGuid(pscGuid);
        }

        public IEnumerable<Psc> GetPscs()
        {
            var pscs = this.GetMemoryList();
            if (pscs != null)
            {
                return pscs;
            }
            else
            {
                var result = _repository.GetPscs();
                this.setMemoryList(result);
                return result;
            }
        }

        public Psc GetPSCDetailByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _repository.GetPSCDetailByCode(code);
        }

        public IEnumerable<Psc> GetPscList(string searchText)
        {
            return _repository.GetPscList(searchText);
        }
    }
}
