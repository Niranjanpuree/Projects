using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.ViewModels;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _iProjectRepository;

        public ProjectService(IProjectRepository ProjectRepository)
        {
            _iProjectRepository = ProjectRepository;
        }

        public IEnumerable<ProjectViewModel> GetAll(Guid ContractGuid, string searchValue, int pageSize, int skip,
            string sortField, string sortDirection)
        {
            IEnumerable<ProjectViewModel> getall =
                _iProjectRepository.GetAll(ContractGuid, searchValue, pageSize, skip, sortField, sortDirection);
            return getall;
        }

        public int TotalRecord(Guid ContractGuid)
        {
            int totalRecord = _iProjectRepository.TotalRecord(ContractGuid);
            return totalRecord;
        }

        public int AddProject(Project ProjectModel)
        {
            return _iProjectRepository.AddProject(ProjectModel);
        }

        public int UpdateProject(Project ProjectModel)
        {
            return _iProjectRepository.UpdateProject(ProjectModel);
        }

        public int DeleteProject(Guid[] ProjectGuidIds)
        {
            return _iProjectRepository.DeleteProject(ProjectGuidIds);
        }

        public int DisableProject(Guid[] ids)
        {
            return _iProjectRepository.DisableProject(ids);
        }

        public int EnableProject(Guid[] ids)
        {
            return _iProjectRepository.EnableProject(ids);
        }

        public ICollection<Organization> GetOrganizationData(string searchText)
        {
            return _iProjectRepository.GetOrganizationData(searchText);
        }

        public ICollection<Customer> GetAwardingAgencyOfficeData(string searchText)
        {
            return _iProjectRepository.GetAwardingAgencyOfficeData(searchText);
        }

        public ICollection<Customer> GetFundingAgencyOfficeData(string searchText)
        {
            return _iProjectRepository.GetFundingAgencyOfficeData(searchText);
        }

        public ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(
            Guid customerId, string contactType)
        {
            return _iProjectRepository.GetAllContactByCustomer(customerId, contactType);
        }

        public ICollection<Naics> GetNAICSCodeData(string searchText)
        {
            return _iProjectRepository.GetNAICSCodeData(searchText);
        }

        public ICollection<Psc> GetPSCCodeData(string searchText)
        {
            return _iProjectRepository.GetPSCCodeData(searchText);
        }

        public AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode)
        {
            return _iProjectRepository.GetCompanyRegionOfficeNameByCode(entityCode);
        }

        public IEnumerable<User> GetUsersData(string searchText)
        {
            return _iProjectRepository.GetUsersData(searchText);
        }

        public ProjectViewModel GetDetailsById(Guid id)
        {
            return _iProjectRepository.GetDetailsById(id);
        }
        public string GetProjectNumberById(Guid id)
        {
            return _iProjectRepository.GetProjectNumberById(id);
        }
        public int GetTotalCountProjectByContractId(Guid id)
        {
            return _iProjectRepository.GetTotalCountProjectByContractId(id);
        }

        public Guid? GetPreviousProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            return _iProjectRepository.GetPreviousProjectOfContractByCounter(contractGuid, currentProjectCounter);
        }
        public Guid? GetNextProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            return _iProjectRepository.GetNextProjectOfContractByCounter(contractGuid, currentProjectCounter);
        }
        public Guid? GetContractIdByProjectId(Guid projectGuid)
        {
            return _iProjectRepository.GetContractIdByProjectId(projectGuid);
        }
        public int HasChild(Guid projectGuid)
        {
            return _iProjectRepository.HasChild(projectGuid);
        }
        public bool IsExistProjectNumber(string projectNumber)
        {
            return _iProjectRepository.IsExistProjectNumber(projectNumber);
        }

        public IEnumerable<ProjectForList> GetProjectByContractGuid(Guid contractGuid)
        {
            return _iProjectRepository.GetProjectByContractGuid(contractGuid);
        }

        #region FormsInDetail
        //ContractQuestionaires
        public int AddContractQuestionaires(ContractQuestionaire ContractQuestionaire)
        {
            return _iProjectRepository.AddContractQuestionaires(ContractQuestionaire);
        }

        public ContractQuestionaire GetContractQuestionariesById(Guid id)
        {
            return _iProjectRepository.GetContractQuestionaireById(id);
        }
        public ContractQuestionaire GetQuestionariesFromContract(Guid id)
        {
            return _iProjectRepository.GetQuestionariesFromContract(id);
        }
        public int UpdateContractQuestionairesById(ContractQuestionaire ContractQuestionaire)
        {
            return _iProjectRepository.UpdateContractQuestionairesById(ContractQuestionaire);
        }

        //EmployeeBillingRates
        public int AddEmployeeBillingRates(EmployeeBillingRates employeeBillingRates)
        {
            return _iProjectRepository.AddEmployeeBillingRates(employeeBillingRates);
        }
        public int UpdateEmployeeBillingRates(EmployeeBillingRates employeeBillingRates)
        {
            return _iProjectRepository.UpdateEmployeeBillingRates(employeeBillingRates);
        }
        public EmployeeBillingRates GetEmployeeBillingRatesById(Guid id)
        {
            return _iProjectRepository.GetEmployeeBillingRatesById(id);
        }

        public IEnumerable<EmployeeBillingRates> GetBillingRatesList(string path)
        {
            IEnumerable<EmployeeBillingRates> getBillingRates = _iProjectRepository.GetBillingRates(path);
            return getBillingRates;
        }
        public int DeleteEmployeeBillingRates(Guid id)
        {
            return _iProjectRepository.DeleteEmployeeBillingRates(id);
        }

        //ContractWBS
        public int AddContractWBS(ContractWBS ContractWBS)
        {
            return _iProjectRepository.AddContractWBS(ContractWBS);
        }
        public int UpdateContractWBS(ContractWBS ContractWBS)
        {
            return _iProjectRepository.UpdateContractWBS(ContractWBS);
        }
        public IEnumerable<ContractWBS> GetWBSList(string path)
        {
            IEnumerable<ContractWBS> getWBS = _iProjectRepository.GetWBSList(path);
            return getWBS;
        }
        public ContractWBS GetContractWBSById(Guid id)
        {
            return _iProjectRepository.GetContractWBSById(id);
        }
        public int DeleteContractWBS(Guid id)
        {
            return _iProjectRepository.DeleteContractWBS(id);
        }

        //LaborCategoryRates
        public int AddLaborCategoryRates(LaborCategoryRates laborCategoryRates)
        {
            return _iProjectRepository.AddLaborCategoryRates(laborCategoryRates);
        }
        public int UpdateLaborCategoryRates(LaborCategoryRates laborCategoryRates)
        {
            return _iProjectRepository.UpdateLaborCategoryRates(laborCategoryRates);
        }
        public LaborCategoryRates GetLaborCategoryRatesById(Guid id)
        {
            return _iProjectRepository.GetLaborCategoryRatesById(id);
        }

        public IEnumerable<LaborCategoryRates> GetCategoryRatesList(string path)
        {
            IEnumerable<LaborCategoryRates> getCategoryRates = _iProjectRepository.GetCategoryRates(path);
            return getCategoryRates;
        }
        public int DeleteLaborCategoryRates(Guid id)
        {
            return _iProjectRepository.DeleteLaborCategoryRates(id);
        }

        //TooltipDetails
        public User GetUsersDataByUserId(Guid id)
        {
            return _iProjectRepository.GetUsersDataByUserId(id);
        }
        public CustomerContact GetContactsDataByContactId(Guid id)
        {
            return _iProjectRepository.GetContactsDataByContactId(id);
        }
        public Customer GetCustomersDataByCustomerId(Guid id)
        {
            return _iProjectRepository.GetCustomersDataByCustomerId(id);
        }
        #endregion
    }
}
