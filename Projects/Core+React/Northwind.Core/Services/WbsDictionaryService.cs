using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class WbsDictionaryService : IWbsDictionaryService
    {
        IWbsDictionaryRepository _wbsDictionaryRepository;
        public WbsDictionaryService(IWbsDictionaryRepository wbsDictionaryRepository)
        {
            _wbsDictionaryRepository = wbsDictionaryRepository;
        }

        public void Add(WbsDictionary wbsDictionary)
        {
            _wbsDictionaryRepository.Add(wbsDictionary);
        }

        public void Delete(Guid wbsDictionaryGuid)
        {
            _wbsDictionaryRepository.Delete(wbsDictionaryGuid);
        }

        public IEnumerable<WbsDictionary> GetWbsDictionary(string ProjectNumber, string WbsNumber, string searchValue, int skip, int take, string orderBy, string dir)
        {
            return _wbsDictionaryRepository.GetWbsDictionary(ProjectNumber, WbsNumber, searchValue, skip, take, orderBy, dir);
        }

        public WbsDictionary GetWbsDictionaryByGuid(Guid DictionaryGuid)
        {
            return _wbsDictionaryRepository.GetWbsDictionaryByGuid(DictionaryGuid);
        }

        public int GetWbsDictionaryCount(string ProjectNumber, string WbsNumber, string searchValue)
        {
            return _wbsDictionaryRepository.GetWbsDictionaryCount(ProjectNumber, WbsNumber, searchValue);
        }

        public void Update(WbsDictionary wbsDictionary)
        {
            _wbsDictionaryRepository.Update(wbsDictionary);
        }
    }
}
