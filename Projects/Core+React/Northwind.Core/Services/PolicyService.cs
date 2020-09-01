using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class PolicyService : IPolicyService
    {
        IPolicyRepository _policyRepository;
        public PolicyService(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        } 

        public void Delete(PolicyEntity policyEntity)
        {
            _policyRepository.Delete(policyEntity);
        }

        public void Delete(Guid policyGuid)
        {
            _policyRepository.Delete(policyGuid);
        }

        public IEnumerable<Policy> GetPolicies(Guid userGuid)
        {
            return _policyRepository.GetPolicies(userGuid);
        }

        public IEnumerable<PolicyEntity> GetPolicyEntities()
        {
            return _policyRepository.GetPolicyEntities();
        }

        public PolicyEntity GetPolicyEntity(Guid policyGuid)
        {
            return _policyRepository.GetPolicyEntity(policyGuid);
        }

        public void Insert(PolicyEntity policyEntity)
        {
            _policyRepository.Insert(policyEntity);
        }

        public void Update(PolicyEntity policyEntity)
        {
            _policyRepository.Update(policyEntity);
        }
    }
}
