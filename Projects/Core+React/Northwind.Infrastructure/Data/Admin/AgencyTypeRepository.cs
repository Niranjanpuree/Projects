using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Helpers;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data.Admin
{
    public class AgencyTypeRepository : IAgencyTypeRepository
    {

        private string connectionString;
        public AgencyTypeRepository()
        {
            connectionString = new ConnectionHelper().DbConnectionString;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public int CreateAgencyType(AgencyType agencyModel)
        {
            throw new System.NotImplementedException();
        }

        public int DeleteAgencyType(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<AgencyType> GetAgencyType()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<AgencyType>("SELECT * FROM AgencyType");
            }
        }

        public int UpdateAgencyType(AgencyType agencyModel)
        {
            throw new System.NotImplementedException();
        }
        public ICollection<KeyValuePairModel<Guid, string>> GetAgencyTypeDropDown()
        {
            var model = new List<KeyValuePairModel<Guid, string>>();
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var data = dbConnection.Query<AgencyType>("SELECT * FROM AgencyType");
                foreach (var item in data)
                {
                    model.Add(new KeyValuePairModel<Guid, string> { Keys = item.AgencyTypeId, Values = item.AgencyTypeName.ToString() });
                }
            }
            return model;
        }
    }
}
