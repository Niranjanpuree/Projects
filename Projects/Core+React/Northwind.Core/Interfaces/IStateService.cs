using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface IStateService
    {
        IEnumerable<States> GetStateByCountryGuid(Guid countryGuid);
        IEnumerable<States> GetStateByAbbreviation(string abb);
        Guid GetStateByName(string stateName);
        IEnumerable<States> GetStatesByStateIds(string[] statesId);
        IEnumerable<States> GetStatesList();
        States GetStateByStateName(string stateName);

        States GetStateByAbbreviations(string abbreviation);
    }
}
