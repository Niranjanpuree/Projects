using Northwind.Core.Entities;

using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface IContractService
    {
        int TotalRecord();
        int Add(Contract contractModel);
        int Edit(Contract contractModel);
        int Delete(Guid[] contractGuidIds);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);
        ICollection<Organization> GetOrganizationData(string searchText);
        Organization GetOrganizationByOrgId(Guid orgId);
        ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType);
        ICollection<Naics> GetNaicsCodeData(string searchText);
        ICollection<Psc> GetPscCodeData(string searchText);
        AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode);
        Contract GetDetailById(Guid id);
        Contract GetInfoById(Guid id);
        bool IsExistContractNumber(string contractNumber, Guid contractGuid);
        bool IsExistProjectNumber(string projectNumber, Guid contractGuid);
        bool IsExistContractTitle(string contractTitle, Guid contractGuid);
        Contract GetContractByContractGuid(Guid contractGuid);
        Contract GetInfoByContractGuid(Guid contractGuid);
        string GetContractNumberById(Guid id);
        string GetParentContractNumberById(Guid id);
        IEnumerable<ContractForList> GetContract(string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        int GetContractCount(string searchValue);
        IEnumerable<ProjectForList> GetProjectByContractGuid(Guid contractGuid);
        Contract GetDetailsForProjectByContractId(Guid id);
        int GetTotalCountProjectByContractId(Guid id);
        int HasChild(Guid projectGuid);
        Guid? GetPreviousProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter);
        Guid? GetNextProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter);
        //string GetProjectNumberById(Guid id);
        int HasChild(Guid projectGuid);

        IEnumerable<Contract> GetAllProject(Guid ContractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        ContractQuestionaire GetQuestionariesFromContract(Guid id);

        User GetUsersDataByUserId(Guid id);
        CustomerContact GetContactsDataByContactId(Guid id);
        Customer GetCustomersDataByCustomerId(Guid id);

        string GetProjectNumberById(Guid id);
        Guid GetFarContractTypeGuidById(Guid id);

        //bool updateContractRevenueRecognitionGuid(Guid contractGuid, Guid revenueRecognitionGuid);
    }
}