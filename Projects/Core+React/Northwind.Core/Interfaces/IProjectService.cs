
using Northwind.Core.Entities;
using Northwind.Core.ViewModels;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectViewModel> GetAll(Guid ContractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(Guid ContractGuid);
        int AddProject(Project ProjectModel);
        int UpdateProject(Project ProjectModel);
        int DeleteProject(Guid[] ProjectGuidIds);
        int DisableProject(Guid[] ids);
        int EnableProject(Guid[] ids);
        ICollection<Organization> GetOrganizationData(string searchText);
        ICollection<Customer> GetAwardingAgencyOfficeData(string searchText);
        ICollection<Customer> GetFundingAgencyOfficeData(string searchText);
        ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType);
        ICollection<Naics> GetNAICSCodeData(string searchText);
        ICollection<Psc> GetPSCCodeData(string searchText);
        AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode);
        IEnumerable<User> GetUsersData(string searchText);
        ProjectViewModel GetDetailsById(Guid id);
        int GetTotalCountProjectByContractId(Guid id);
        Guid? GetPreviousProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter);
        Guid? GetNextProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter);
        Guid? GetContractIdByProjectId(Guid projectGuid);
        int HasChild(Guid projectGuid);
        bool IsExistProjectNumber(string projectNumber);

        string GetProjectNumberById(Guid id);

        IEnumerable<ProjectForList> GetProjectByContractGuid(Guid ContractGuid);

        #region FormsInDetails
        int AddContractQuestionaires(ContractQuestionaire ContractQuestionaire);
        ContractQuestionaire GetContractQuestionariesById(Guid id);
        ContractQuestionaire GetQuestionariesFromContract(Guid id);
        int UpdateContractQuestionairesById(ContractQuestionaire ContractQuestionaire);

        int AddContractWBS(ContractWBS ContractWBS);
        int UpdateContractWBS(ContractWBS ContractWBS);
        int DeleteContractWBS(Guid id);
        ContractWBS GetContractWBSById(Guid id);
        IEnumerable<ContractWBS> GetWBSList(string path);

        int AddEmployeeBillingRates(EmployeeBillingRates employeeBillingRates);
        int UpdateEmployeeBillingRates(EmployeeBillingRates employeeBillingRates);
        int DeleteEmployeeBillingRates(Guid id);
        EmployeeBillingRates GetEmployeeBillingRatesById(Guid id);
        IEnumerable<EmployeeBillingRates> GetBillingRatesList(string path);

        int AddLaborCategoryRates(LaborCategoryRates LaborCategoryRates);
        int UpdateLaborCategoryRates(LaborCategoryRates LaborCategoryRates);
        int DeleteLaborCategoryRates(Guid id);
        LaborCategoryRates GetLaborCategoryRatesById(Guid id);
        IEnumerable<LaborCategoryRates> GetCategoryRatesList(string path);

        User GetUsersDataByUserId(Guid id);
        CustomerContact GetContactsDataByContactId(Guid id);
        Customer GetCustomersDataByCustomerId(Guid id);
        #endregion
    }
}