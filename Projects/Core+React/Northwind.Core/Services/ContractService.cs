using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _iContractRepository;
        public ContractService(IContractRepository ContractRepository)
        {
            _iContractRepository = ContractRepository;
        }

        public Contract GetContractByContractGuid(Guid contractGuid)
        {
            return _iContractRepository.GetContractByContractGuid(contractGuid);
        }
        public Contract GetInfoByContractGuid(Guid contractGuid)
        {
            return _iContractRepository.GetInfoByContractGuid(contractGuid);
        }


        public int TotalRecord()
        {
            int totalRecord = _iContractRepository.TotalRecord();
            return totalRecord;
        }
        public int Add(Contract contractModel)
        {
            return _iContractRepository.Add(contractModel);
        }
        public int Edit(Contract contractModel)
        {
            return _iContractRepository.Edit(contractModel);
        }
        public int Delete(Guid[] contractGuidIds)
        {
            return _iContractRepository.Delete(contractGuidIds);
        }
        public int Disable(Guid[] ids)
        {
            return _iContractRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return _iContractRepository.Enable(ids);
        }
        public ICollection<Organization> GetOrganizationData(string searchText)
        {
            return _iContractRepository.GetOrganizationData(searchText);
        }
        public Organization GetOrganizationByOrgId(Guid orgId)
        {
            return _iContractRepository.GetOrganizationByOrgId(orgId);
        }
        public ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType)
        {
            return _iContractRepository.GetAllContactByCustomer(customerId, contactType);
        }
        public ICollection<Naics> GetNaicsCodeData(string searchText)
        {
            return _iContractRepository.GetNaicsCodeData(searchText);
        }
        public ICollection<Psc> GetPscCodeData(string searchText)
        {
            return _iContractRepository.GetPscCodeData(searchText);
        }
        public AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode)
        {
            return _iContractRepository.GetCompanyRegionOfficeNameByCode(entityCode);
        }

        public Contract GetInfoById(Guid id)
        {
            return _iContractRepository.GetInfoById(id);
        }
        public Contract GetDetailById(Guid id)
        {
            return _iContractRepository.GetDetailById(id);
        }
        public string GetContractNumberById(Guid id)
        {
            return _iContractRepository.GetContractNumberById(id);
        }

        public IEnumerable<ProjectForList> GetProjectByContractGuid(Guid contractGuid)
        {
            return _iContractRepository.GetProjectByContractGuid(contractGuid);
        }
        public Contract GetDetailsForProjectByContractId(Guid contractGuid)
        {
            return _iContractRepository.GetDetailsForProjectByContractId(contractGuid);
        }
        public bool IsExistContractNumber(string contractNumber, Guid contractGuid)
        {
            return _iContractRepository.IsExistContractNumber(contractNumber, contractGuid);
        }
        public bool IsExistProjectNumber(string projectNumber, Guid contractGuid)
        {
            return _iContractRepository.IsExistProjectNumber(projectNumber, contractGuid);
        }

        public bool IsExistContractTitle(string contractTitle, Guid contractGuid)
        {
            return _iContractRepository.IsContractTitleValid(contractTitle,contractGuid);
        }

        public IEnumerable<ContractForList> GetContract(string searchValue, int pageSize, int skip, int take, string orderBy, string dir)
        {
            return _iContractRepository.GetContract(searchValue, pageSize, skip, take, orderBy, dir);
        }

        public int GetContractCount(string searchValue)
        {
            return _iContractRepository.GetContractCount(searchValue);
        }
        public int GetTotalCountProjectByContractId(Guid contractGuid)
        {
            return _iContractRepository.GetTotalCountProjectByContractId(contractGuid);
        }
        public Guid? GetPreviousProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            return _iContractRepository.GetPreviousProjectOfContractByCounter(contractGuid, currentProjectCounter);
        }
        public Guid? GetNextProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            return _iContractRepository.GetNextProjectOfContractByCounter(contractGuid, currentProjectCounter);
        }
        public int HasChild(Guid projectGuid)
        {
            return _iContractRepository.HasChild(projectGuid);
        }
        public IEnumerable<Contract> GetAllProject(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            return _iContractRepository.GetAllProject(contractGuid, searchValue, pageSize, skip, sortField, sortDirection);
        }
        public ContractQuestionaire GetQuestionariesFromContract(Guid id)
        {
            return _iContractRepository.GetQuestionariesFromContract(id);
        }

        //TooltipDetails
        public User GetUsersDataByUserId(Guid id)
        {
            return _iContractRepository.GetUsersDataByUserId(id);
        }
        public CustomerContact GetContactsDataByContactId(Guid id)
        {
            return _iContractRepository.GetContactsDataByContactId(id);
        }
        public Customer GetCustomersDataByCustomerId(Guid id)
        {
            return _iContractRepository.GetCustomersDataByCustomerId(id);
        }
       
        public string GetParentContractNumberById(Guid id)
        {
            return _iContractRepository.GetParentContractNumberById(id);
        }

        public string GetProjectNumberById(Guid id)
        {
            return _iContractRepository.GetProjectNumberById(id);
        }

        public Guid GetFarContractTypeGuidById(Guid id)
        {
            return _iContractRepository.GetFarContractTypeGuidById(id);
        }

        //public bool updateContractRevenueRecognitionGuid(Guid contractGuid, Guid revenueRecognitionGuid)
        //{
        //    return _iContractRepository.updateContractRevenueRecognitionGuid(contractGuid, revenueRecognitionGuid);
        //}
    }
}
