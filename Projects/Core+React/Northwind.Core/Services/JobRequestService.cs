using System;
using System.Collections.Generic;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;

namespace Northwind.Core.Services
{
    public class JobRequestService : IJobRequestService
    {
        private IJobRequestRepository _jobRequestRepository;
        private IContractsService _contractService;
        public JobRequestService(IJobRequestRepository jobRequestRepository, IContractsService contractService)
        {
            _jobRequestRepository = jobRequestRepository;
            _contractService = contractService;
        }
        public ICollection<KeyValuePairModel<Guid, string>> GetCompanyData()
        {
            return _jobRequestRepository.GetCompanyData();
        }
        public IEnumerable<JobRequest> GetAll(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, string filterBy, Guid userGuid)
        {
            IEnumerable<JobRequest> getall = _jobRequestRepository.GetAll(searchValue, filterBy, pageSize, skip, take, orderBy, dir, userGuid);
            return getall;
        }

        public int TotalRecord(string searchValue, string filterBy, Guid userGuid)
        {
            return _jobRequestRepository.TotalRecord(searchValue, filterBy, userGuid);
        }

        public Guid Add(JobRequest jobRequest)
        {
            return _jobRequestRepository.Add(jobRequest);
        }
        public void Edit(JobRequest jobRequest)
        {
            _jobRequestRepository.Edit(jobRequest);
        }
        public int Delete(Guid[] ids)
        {
            return _jobRequestRepository.Delete(ids);
        }
        public int Disable(Guid[] ids)
        {
            return _jobRequestRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return _jobRequestRepository.Enable(ids);
        }
        public JobRequest GetDetailsForJobRequestById(Guid id)
        {
            var jobRequest = new JobRequest();
            var contracts = _contractService.GetDetailById(id);
            jobRequest.Contracts = contracts;
            return jobRequest;
        }
        public JobRequest GetJobRequestEntityByJobRequestId(Guid id)
        {
            var jobRequest = _jobRequestRepository.GetJobRequestEntityByJobRequestId(id);

            return jobRequest;
        }
        public string GetCompanyName(string id)
        {
            return _jobRequestRepository.GetCompanyName(id);
        }

        public int? GetJobRequestStatusByUserId(Guid loginUserGuid)
        {
            return _jobRequestRepository.GetJobRequestStatusByUserId(loginUserGuid);
        }

        public void InsertContractBasicInfo(BasicContractInfoModel jobRequest, Guid contractGuid)
        {
            _jobRequestRepository.InsertContractBasicInfo(jobRequest, contractGuid);
        }

        public void InserContractKeyPersonel(KeyPersonnelModel jobRequest, Guid contractGuid)
        {
            _jobRequestRepository.InserContractKeyPersonel(jobRequest, contractGuid);
        }

        public int GetCurrentStatusByGuid(Guid id)
        {
           return _jobRequestRepository.GetCurrentStatusByGuid(id);
        }
    }
}
