using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface IJobRequestService
    {
        ICollection<KeyValuePairModel<Guid, string>> GetCompanyData();
        IEnumerable<JobRequest> GetAll(string searchValue, int pageSize, int skip, int take, string orderBy, string dir,string filterBy,Guid userGuid);
        int TotalRecord(string searchValue,string filterBy,Guid userGuid);
        Guid Add(JobRequest jobRequest);
        void InsertContractBasicInfo(BasicContractInfoModel jobRequest, Guid contractGuid);
        void InserContractKeyPersonel(KeyPersonnelModel jobRequest, Guid contractGuid);
        void Edit(JobRequest jobRequest);
        int Delete(Guid[] ids);
        int Enable(Guid[] ids);
        int Disable(Guid[] ids);
        JobRequest GetDetailsForJobRequestById(Guid id);
        JobRequest GetJobRequestEntityByJobRequestId(Guid id);
        int GetCurrentStatusByGuid(Guid id);

        string GetCompanyName(string id);

        int? GetJobRequestStatusByUserId(Guid loginUserGuid);
    }
}
