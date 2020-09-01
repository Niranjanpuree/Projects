using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace Northwind.Infrastructure.Data
{
    public class PolicyRepository : IPolicyRepository
    {
        IDatabaseSingletonContext _context;
        public PolicyRepository(IDatabaseSingletonContext context)
        {
            _context = context;
        }

        public void Delete(PolicyEntity policyEntity)
        {
            var sql = @"DELETE FROM Policy WHERE PolicyGuid=@PolicyGuid";
            _context.Connection.Execute(sql, new { PolicyGuid = policyEntity.PolicyGuid });
        }

        public void Delete(Guid policyGuid)
        {
            var sql = @"DELETE FROM Policy WHERE PolicyGuid=@PolicyGuid";
            _context.Connection.Execute(sql, new { PolicyGuid = policyGuid });
        }

        public IEnumerable<Policy> GetPolicies(Guid userGuid)
        {
            var policies = new List<Policy>();
            var policies1 = this.GetPolicyEntities(); 
            foreach(var p in policies1)
            {
                policies.Add(p.Policy);
            }
            return policies;
        }

        public IEnumerable<PolicyEntity> GetPolicyEntities()
        {
            var sql = @"SELECT * FROM Policy ORDER BY Name";
            return _context.Connection.Query<PolicyEntity>(sql);
        }

        public PolicyEntity GetPolicyEntity(Guid policyGuid)
        {
            var sql = @"SELECT * FROM Policy WHERE PolicyGuid=@PolicyGuid";
            return _context.Connection.QueryFirst<PolicyEntity>(sql, new { PolicyGuid = policyGuid });
        }

        public void Insert(PolicyEntity policyEntity)
        {
            var sql = @"INSERT INTO [Policy]
                                   ([PolicyGuid]
                                   ,[Name]
                                   ,[Title]
                                   ,[Description]
                                   ,[PolicyJson])
                            VALUES (@PolicyGuid, @Name, @Title, @Description, @PolicyJson)
                ";
            _context.Connection.Execute(sql, new { PolicyGuid = Guid.NewGuid(), policyEntity.Name, policyEntity.Title, policyEntity.Description, policyEntity.PolicyJson });
        }

        public void Update(PolicyEntity policyEntity)
        {
            var sql = @"UPDATE [dbo].[Policy]
                   SET [Name] = @Name
                      ,[Title] = @Title
                      ,[Description] = @Description
                      ,[PolicyJson] = @PolicyJson
                 WHERE PolicyGuid=@PolicyGuid
                ";
            _context.Connection.Execute(sql, new { PolicyGuid = Guid.NewGuid(), policyEntity.Name, policyEntity.Title, policyEntity.Description, policyEntity.PolicyJson });
        }
    }
}
