using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IWbsDictionaryRepository
    {
        WbsDictionary GetWbsDictionaryByGuid(Guid DictionaryGuid);
        IEnumerable<WbsDictionary> GetWbsDictionary(string ProjectNumber, string WbsNumber, string searchValue, int skip, int take, string orderBy, string dir);
        int GetWbsDictionaryCount(string ProjectNumber, string WbsNumber, string searchValue);
        void Add(WbsDictionary wbsDictionary);
        void Update(WbsDictionary wbsDictionary);
        void Delete(Guid wbsDictionaryGuid);
    }
}
