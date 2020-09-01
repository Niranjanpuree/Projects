using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data.Fakes
{
    public class FakePolicyRepository : IPolicyRepository
    {
        public void Delete(PolicyEntity policyEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid policyGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Policy> GetPolicies(Guid userGuid)
        {
            var policyJson = Northwind.Core.Internationalization.Resources.SimplePolicyExample;
            var newPol = JsonConvert.DeserializeObject<Policy>(policyJson);
            List<Policy> pol = new List<Policy>
            {
                newPol
            };
            return pol;
        }

        public IEnumerable<PolicyEntity> GetPolicyEntities()
        {
            throw new NotImplementedException();
        }

        public PolicyEntity GetPolicyEntity(Guid policyGuid)
        {
            throw new NotImplementedException();
        }

        public void Insert(PolicyEntity policyEntity)
        {
            throw new NotImplementedException();
        }

        public void Update(PolicyEntity policyEntity)
        {
            throw new NotImplementedException();
        }
    }
}
