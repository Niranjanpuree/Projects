using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface INaicsService
    {
        IEnumerable<Naics> GetNaicsList();
        Naics GetNaicsByGuid(Guid naicsGuid);
        Naics GetNaicsByCode(string code);
        ICollection<Naics> GetNaicsList(string searchText);
    }
}
