using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IPscService
    {
        IEnumerable<Psc> GetPscs();
        IEnumerable<Psc> GetPscByGuid(Guid pscGuid);
        IEnumerable<Psc> GetPscByCode(string code);

        Psc GetPSCDetailByCode(string code);
        IEnumerable<Psc> GetPscList(string searchText);
    }
}
