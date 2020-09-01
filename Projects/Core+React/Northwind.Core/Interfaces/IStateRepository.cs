using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface IStateRepository
    {
        IEnumerable<States> GetStateByCountryGuid(Guid countryGuid);
        IEnumerable<States> GetStatesByStateIds(string[] statesId);
        IEnumerable<States> GetStateByAbbreviation(string abb);
        IEnumerable<States> GetStatesList();
        States GetStateByStateName(string stateName);

        Guid GetStateByName(string stateName);

        States GetStateByAbbreviations(string abbreviation);
    }
}
