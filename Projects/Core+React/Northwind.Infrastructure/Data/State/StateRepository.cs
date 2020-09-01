using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Infrastructure.Data.State
{
    public class StateRepository : IStateRepository
    {
        private readonly IDatabaseContext _context;

        public StateRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<States> GetStateByAbbreviation(string abb)
        {
            var data = _context.Connection.Query<States>($"SELECT * FROM State where Abbreviation =@Abbreviation order by stateName asc", new { Abbreviation = abb });
            return data;
        }

        public IEnumerable<States> GetStateByCountryGuid(Guid countryGuid)
        {
            var data = _context.Connection.Query<States>($"SELECT * FROM State where countryId =@countryGuid order by stateName asc", new { countryGuid = countryGuid });
            return data;
        }

        public States GetStateByStateName(string stateName)
        {
            var data = _context.Connection.QueryFirstOrDefault<States>("SELECT * FROM State WHERE StateName = @stateName", new { stateName = stateName });
            return data;
        }


        public Guid GetStateByName(string stateName)
        {
            var data = _context.Connection.QueryFirstOrDefault<Guid>
                ($"SELECT StateId FROM State where (StateName =@StateName or Abbreviation = @stateName ) order by stateName asc", new { StateName = stateName });
            return data;
        }

        public IEnumerable<States> GetStatesByStateIds(string[] statesId)
        {
            var statesIds = statesId.Select(x => string.Format("'" + x + "'"));
            var strStatesIds = string.Join(",", statesIds);
            var data = _context.Connection.Query<States>($"SELECT * FROM State where stateId in({strStatesIds}) order by stateName asc");
            return data;
        }
        public IEnumerable<States> GetStatesList()
        {
            var sql = @"SELECT *
                          FROM [dbo].[State] ORDER BY StateName asc
                      ";
            return _context.Connection.Query<States>(sql);
        }

        public States GetStateByAbbreviations(string abbreviation)
        {
            var query = @"SELECT * 
                        FROM State 
                        WHERE Abbreviation =@abbreviation";
            return _context.Connection.QueryFirstOrDefault<States>(query, new { abbreviation = abbreviation });
        }
    }
}
