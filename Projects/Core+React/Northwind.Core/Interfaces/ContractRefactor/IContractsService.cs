using Northwind.Core.Entities.ContractRefactor;
using System;
using System.Collections.Generic;
using Northwind.Core.Entities;
using System.Text;
using Northwind.Core.Models;
using static Northwind.Core.Entities.ContractRefactor.Contracts;

namespace Northwind.Core.Interfaces.ContractRefactor
{
    public interface IContractsService
    {
        #region Contract
        Contracts GetContractEntityByContractId(Guid contractId);
        Contracts GetDetailById(Guid id);
        string  GetOrgNameById(Guid id);
        BasicContractInfoModel GetBasicContractById(Guid id);
        Contracts GetAmountById(Guid id);
        ContractBasicDetails GetOnlyRequiredContractData(Guid id);
        AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode);
        IEnumerable<Contracts> GetContractList(string searchValue, string filterBy, Guid userGuid, int pageSize, int skip, int take, string orderBy, string dir);

        IEnumerable<Contracts> GetContractList(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);

        IEnumerable<Contracts> GetTaskByContractGuid(Guid contractParentGuid, Guid userGuid);

        int GetContractCount(string searchValue);

        int GetContractCountByFilter(string searchValue, string additionalFilter, Guid userGuid);

        int GetAdvanceContractSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);

        ValidationResult Save(Contracts contract);

        ValidationResult Validate(Contracts contract);

        bool Delete(Guid[] ids);

        bool Enable(Guid[] ids);

        bool Disable(Guid[] ids);

        Contracts GetDetailsForProjectByContractId(Guid contractGuid);

      

        bool UpdateProjectNumberByGuid(Guid contractGuid, string projectNumber);

        string GetContractNumberById(Guid contratGuid);

        string GetProjectNumberById(Guid contratGuid);

        Guid GetContractIdByProjectId(Guid contratGuid);

        Guid? GetPreviousTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter);

        Guid? GetNextTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter);

        bool IsIDIQContract(Guid contractGuid);

        Guid GetContractGuidByProjectNumber(string projectNumber);

        bool IsProjectNumberDuplicate(string projectNumber, Guid contractGuid);

        ValidationResult ImportContract(Contracts contract);

        Contracts GetContractByProjectNumber(string projectNumber);

        Contracts GetContractByTaskNodeID(int taskNodeID);

        Guid GetParentContractByMasterTaskNodeID(int masterTaskNodeID);

        bool CheckTaskOdrderByContractGuid(Guid parentContractGuid);
        #endregion Contract

        bool InsertContractUsers(List<ContractUserRole> contractUser);
        bool UpdateContractUsers(List<ContractUserRole> contractUser);

        IEnumerable<Contracts> GetAllProject(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);

        Contracts GetContractByContractNumber(string contractNumber);

        Guid GetContractByContractNumberProjectNumberAndTitle(string contractNumber, string projectNumber, string title);

        Guid GetParentContractByContractNumber(string contractNumber);
        Guid? GetParentContractGuidByContractGuid(Guid contractGuid);

        bool IsExistContractNumber(string contractNumber, Guid contractGuid);
        bool IsExistProjectNumber(string projectNumber, Guid contractGuid);
        bool IsExistContractTitle(string contractTitle, Guid contractGuid);

        #region Contract User Role
        IEnumerable<ContractUserRole> GetKeyPersonnelByContractGuid(Guid contractGuid);
        IEnumerable<ContractUserRole> GetKeyPersonnels(string keyPersonnelType);
        List<string> GetContractRoleByUserGuid(Guid contractGuid, Guid userGuid);
        bool UpdateAllUserByRole(Guid userGuid, string userRole);
        #endregion Contract User Role

        #region Contract Modification
        IEnumerable<ContractModification> GetAllContractMod(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(Guid contractGuid);
        int InsertMod(ContractModification contractModificationModel);
        int UpdateMod(ContractModification contractModificationModel);
        int DeleteMod(Guid[] ids);
        int DisableMod(Guid[] ids);
        bool EnableMod(Guid[] ids);
        ContractModification GetModDetailById(Guid id);
        bool IsExistModificationNumber(Guid contractGuid, string modificationNumber);
        #endregion Contract Modification

        #region ContractFiles
        IEnumerable<ContractResourceFile> GetFileListByContractGuid(Guid contractGuid);
        IEnumerable<ContractResourceFile> GetFileListByContentResourceGuid(Guid contractGuid);

        ContractResourceFile GetFilesByContractFileGuid(Guid contractGuid);

        IEnumerable<ContractResourceFile> GetFilesByContractResourceGuid(Guid contractGuid);

        IEnumerable<ContractResourceFile> GetFilesAndFolders(Guid contractGuid,string column);

        IEnumerable<ContractResourceFile> GetFilesAndFoldersByParentId(Guid contractGuid);

        ContractResourceFile GetFilesByContractGuid(Guid contractGuid, string formType);

        bool InsertContractFile(ContractResourceFile file);
        Guid InsertContractFileAndGetIdBack(ContractResourceFile file);
        bool DeleteContractFolder(Guid fileId);
        bool RenameContractFolder(Guid fileId,string FolderName);


        bool CheckAndInsertContractFile(ContractResourceFile file);
        bool UpdateContractFile(ContractResourceFile file);
        bool UpdateFile(ContractResourceFile file);

        bool DeleteContractFileById(Guid contractFileGuid);

        bool DeleteContractFileByContractGuid(Guid contractGuid, string formType);

        bool UpdateFileName(Guid contractFileGuid, string fileName);

        ContractResourceFile GetFileByResourceGuid(Guid resourceGuid, string formType);

        #endregion End of contract files

        ICollection<Organization> GetOrganizationData(string searchText);
        Organization GetOrganizationByOrgId(Guid orgId);
        Guid GetFarContractTypeGuidById(Guid id);
        int GetTotalCountProjectByContractId(Guid contractGuid);
        int HasChild(Guid projectGuid);
        Contracts GetInfoByContractGuid(Guid contractGuid);

        string GetContractType(Guid contratGuid);
        bool InsertRevenueRecognitionGuid(Guid revenueRecognition, Guid contractGuid);
        bool SetNullRevenueRecognitionGuid(Guid contractGuid, decimal? awardAmount, decimal? fundingAmount);

        bool CloseContractStatus(Guid contractGuid);
        QuestionaireUserAnswer GetAnswer(Guid contractGuid); 
    }
}
