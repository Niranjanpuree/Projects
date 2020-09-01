using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Northwind.Core.Entities.ContractRefactor.Contracts;

namespace Northwind.Core.Interfaces.ContractRefactor
{
    public interface IContractsRepository
    {
        #region Contract

        Contracts GetContractEntityByContractId(Guid contractId);
        Contracts GetDetailById(Guid id);
        Contracts GetDetailByContractNumber(string contractNumber);
        BasicContractInfoModel GetBasicContractById(Guid id);

        AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode);

        ContractBasicDetails GetOnlyRequiredContractData(Guid id);

        string GetOrgNameById(Guid id);

        IEnumerable<Contracts> GetContractList(string searchValue, string additionalFilter, Guid userGuid, int pageSize, int skip, int take, string orderBy, string dir);

        IEnumerable<Contracts> GetContractList(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);

        bool Insert(Contracts contract);

        bool Update(Contracts contract);

        bool DeleteByGuid(Guid[] guid);

        bool EnableByGuid(Guid[] guid);

        bool DisableByGuid(Guid[] guid);

        int GetContractCount(string searchValue);

        int GetContractCountByFilter(string searchValue, string additionalFilter, Guid userGuid);

        int GetAdvanceContractSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);

        IEnumerable<Contracts> GetTaskByContractGuid(Guid guid);

        bool IsContractNumberValid(string contractNumber, Guid contractGuid);

        bool IsExistContractTitle(string contractTitle, Guid contractGuid);

        bool IsProjectNumberValid(string projectNumber);

        bool IsContractGuidValid(Guid contractGuid);

        Contracts GetTaskDetailByParentGuid(Guid guid);

        Guid? GetPreviousTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter);

        Guid? GetNextTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter);

        string GetCompanyCodeByContractId(Guid contractGuid);

        string GetRegionCodeByContractuid(Guid contractGuid);

        string GetOfficeCodeByContractGuid(Guid contractGuid);

        BasicContractInfoModel GetBasicContractInfoByContractGuid(Guid contractGuid);

        bool UpdateProjectNumberByGuid(Guid contractGuid, string projectNumber);

        //string GetContractNumberById(Guid contractGuid);

        bool IsIDIQContract(Guid contractGuid);

        string GetContractNumberByGuid(Guid contractGuid);

        string GetProjectNumberById(Guid contractGuid);

        Guid GetContractIdByProjectId(Guid contratGuid);

        Guid GetContractGuidByProjectNumber(string projectNumber);

        IEnumerable<Contracts> GetAllProject(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);

        Contracts GetContractByContractNumber(string contractNumber);

        Guid GetContractByContractNumberProjectNumberAndTitle(string contractNumber, string projectNumber, string title);

        Guid GetParentContractByContractNumber(string contractNumber);

        bool IsProjectNumberDuplicate(string projectNumber, Guid contractGuid);

        bool InsertContract(Contracts contract);

        Contracts GetContractByProjectNumber(string projectNumber);

        Contracts GetContractByTaskNodeID(int taskNodeID);

        Guid GetParentContractByMasterTaskNodeID(int masterTaskNodeID);

        bool CheckTaskOdrderByContractGuid(Guid parentContractGuid);

        bool IsExistContractNumber(string contractNumber, Guid contractGuid);
        bool IsExistProjectNumber(string projectNumber, Guid contractGuid);
        int HasChild(Guid projectGuid);
        Contracts GetInfoByContractGuid(Guid contractGuid);
        #endregion Contract

        #region Contract User
        IEnumerable<ContractUserRole> GetKeyPersonnelByContractGuid(Guid contractGuid);
        bool InsertContractUsersList(List<ContractUserRole> contractUserList);
        bool UpdateContractUsersList(List<ContractUserRole> contractUserList);
        bool InsertContractUser(ContractUserRole contractUser);
        bool UpdateContractUserByGuid(ContractUserRole contractUser);
        IEnumerable<ContractUserRole> GetKeyPersonnels(string keyPersonnelType);
        List<string> GetContractRoleByUserGuid(Guid contractGuid, Guid userGuid);
        bool UpdateAllUserByRole(Guid userGuid, string userRole);
        #endregion Contract User

        #region Contract Modification
        IEnumerable<ContractModification> GetAllContractMod(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        Contracts GetDetailsForProjectByContractId(Guid id);
       
        int TotalRecord(Guid contractGuid);
        int InsertMod(ContractModification contractModificationModel);
        int UpdateMod(ContractModification contractModificationModel);
        int DeleteMod(Guid[] ids);
        int DisableMod(Guid[] ids);
        int EnableMod(Guid[] ids);
        ContractModification GetModDetailById(Guid id);
        bool IsExistModificationNumber(Guid contractGuid, string modificationNumber);
        #endregion Contract Modification

        #region ContractFiles
        IEnumerable<ContractResourceFile> GetFileListByContractGuid(Guid contractGuid);
        IEnumerable<ContractResourceFile> GetFileListByContentResourceGuid(Guid contractGuid);
        ContractResourceFile GetFilesByContractFileGuid(Guid contractGuid);
        IEnumerable<ContractResourceFile> GetFilesByContractResourceFileGuid(Guid contractGuid);
        IEnumerable<ContractResourceFile> GetFilesAndFolders(Guid contractGuid,string column);
        IEnumerable<ContractResourceFile> GetFilesAndFoldersByParentId(Guid parentId);

        ContractResourceFile GetFilesByContractGuid(Guid contractGuid, string formType);

        bool InsertContractFile(ContractResourceFile file);
        bool CheckAndInsertContractFile(ContractResourceFile file);
        Guid InsertContractFileAndGetIdBack(ContractResourceFile file);
        bool DeleteContractFolder(Guid fileId);
        bool RenameContractFolder(Guid fileId,string FileName);

        bool UpdateContractFile(ContractResourceFile file);
        bool UpdateFile(ContractResourceFile file);

        bool DeleteContractFileById(Guid contractFileGuid);

        bool DeleteContractFileByContractGuid(Guid contractGuid, string formType);

        bool UpdateFileName(Guid contractFileGuid, string fileName);

        ContractResourceFile GetFileByResourceGuid(Guid resourceGuid, string formType);

        #endregion End of contract files

        ICollection<Organization> GetOrganizationData(string searchText);
        Organization GetOrganizationByOrgId(Guid orgId);

        string GetContractType(Guid contratGuid);
        bool InsertRevenueRecognitionGuid(Guid revenueRecognition, Guid contractGuid);
        bool SetNullRevenueRecognitionGuid(Guid contractGuid, decimal? awardAmount, decimal? fundingAmount);
        Contracts GetAmountById(Guid id);
        bool CloseContractStatus(Guid contractGuid);
        QuestionaireUserAnswer GetAnswer(Guid contractGuid);
        Guid? GetParentContractGuidByContractGuid(Guid contractGuid);

        Guid GetFarContractTypeGuidById(Guid id);
        int GetTotalCountProjectByContractId(Guid contractGuid);

        #region for PFS
        IEnumerable<Contracts> GetProjectListForPFS(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);
        int GetContractCountForPFS(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);
        #endregion PFS
    }
}
