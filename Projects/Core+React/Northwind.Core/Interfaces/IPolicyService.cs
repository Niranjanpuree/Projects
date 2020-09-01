using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface IPolicyService
    {
        IEnumerable<Policy> GetPolicies(Guid userGuid);
        void Insert(PolicyEntity policyEntity);
        void Update(PolicyEntity policyEntity);
        void Delete(PolicyEntity policyEntity);
        void Delete(Guid policyGuid);
        IEnumerable<PolicyEntity> GetPolicyEntities();
        PolicyEntity GetPolicyEntity(Guid policyGuid);
    }
}
