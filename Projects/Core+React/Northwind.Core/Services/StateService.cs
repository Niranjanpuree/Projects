using System;
using System.Collections.Generic;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        public StateService(IStateRepository StateRepository)
        {
            _stateRepository = StateRepository;
        }

        public IEnumerable<States> GetStateByAbbreviation(string abb)
        {
            return _stateRepository.GetStateByAbbreviation(abb);
        }

        public IEnumerable<States> GetStateByCountryGuid(Guid countryGuid)
        {
            return _stateRepository.GetStateByCountryGuid(countryGuid);
        }

        public Guid GetStateByName(string stateName)
        {
            return _stateRepository.GetStateByName(stateName);
        }

        public States GetStateByStateName(string stateName)
        {
            return _stateRepository.GetStateByStateName(stateName);
        }

        public IEnumerable<States> GetStatesByStateIds(string[] statesId)
        {
            return _stateRepository.GetStatesByStateIds(statesId);
        }

        public IEnumerable<States> GetStatesList()
        {
            var result = _stateRepository.GetStatesList();
            return result;
        }

        public States GetStateByAbbreviationCode(string abbreviation)
        {
            if (string.IsNullOrWhiteSpace(abbreviation))
                return null;
            return null;
        }

        public States GetStateByAbbreviations(string abbreviation)
        {
            if (string.IsNullOrWhiteSpace(abbreviation))
                return null;
            return _stateRepository.GetStateByAbbreviations(abbreviation);
        }
    }
}
