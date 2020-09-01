using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class AttachmentImportService : IAttachmentImportService
    {
        private readonly IImportFileService _importFileService;
        private readonly IContractsService _contractsService;
        private readonly IContractModificationService _contractModificationService;
        private readonly IContractResourceFileService _contractResourceFileService;
        private readonly IConfiguration _configuration;
        private readonly IDocumentManagementService _documentManagementService;
        private string errorLogPath = string.Empty;
        private string rootPath = string.Empty;
        private Guid loggedUser = Guid.Empty;
        private Guid parentTaskOrderGuid = Guid.Empty;
        private string parentNodeID = string.Empty;
        private Guid previousTaskGuid = Guid.Empty;
        private bool isInitailCall = true;
        private FileConfiguration initialConfiguration;
        public AttachmentImportService(IImportFileService importFileService, IContractsService contractsService, IConfiguration configuration,
            IDocumentManagementService documentManagementService, IContractResourceFileService contractResourceFileService,IContractModificationService contractModificationService)
        {
            _importFileService = importFileService;
            _contractsService = contractsService;
            _configuration = configuration;
            _contractResourceFileService = contractResourceFileService;
            _documentManagementService = documentManagementService;
            _contractModificationService = contractModificationService;
            rootPath = _configuration.GetSection("Document:DocumentRoot").Value;
        }

        //Import attachment file
        private void ImportContractAttachment(List<string> fileList, string resurceType, Guid contractGuid, Guid resourceGuid, bool isDelete)
        {
            var destinationPath = string.Empty;
            foreach (var item in fileList)
            {
                var folder = _documentManagementService.GetFolderByKey("Contract", contractGuid, resurceType);
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(resurceType, contractGuid);
                if (contractResourceFile != null)
                {
                    destinationPath = rootPath + contractResourceFile.FilePath + "/";
                    var fileSize = new System.IO.FileInfo(item).Length.ToString();
                    var outPutPath = _importFileService.MoveAttachment(item, destinationPath,isDelete);
                    if (!string.IsNullOrWhiteSpace(outPutPath)) {
                        var isCSV = Path.GetExtension(item) == ".csv" ? true : false;
                        ContractResourceFile contractFile = new ContractResourceFile();
                        contractFile.ContractResourceFileGuid = Guid.NewGuid();
                        contractFile.ResourceGuid = contractGuid;
                        contractFile.CreatedBy = loggedUser;
                        contractFile.CreatedOn = DateTime.UtcNow;
                        contractFile.Keys = resurceType;
                        contractFile.IsActive = true;
                        contractFile.IsCsv = isCSV;
                        contractFile.IsDeleted = false;
                        contractFile.UpdatedBy = loggedUser;
                        contractFile.UpdatedOn = DateTime.Now;
                        contractFile.ContentResourceGuid = resourceGuid;
                        contractFile.IsFile = true;
                        contractFile.ResourceType = "Contract";
                        contractFile.UploadFileName = Path.GetFileName(item);
                        contractFile.FilePath = outPutPath.Replace(rootPath,"");
                        contractFile.FileSize = fileSize;
                        if (folder != null)
                            contractFile.ParentId = contractResourceFile.ContractResourceFileGuid;
                        _contractsService.InsertContractFile(contractFile);
                    }
                }
            }
        }

        //Import mod attachment
        private void ImportContractModAttachment(FileConfiguration configuration, int nodeID, Guid contractGuid,bool isDelete)
        {
            var directoryFolderList = _importFileService.GetAllFolder(configuration.SourcePath + $"\\{nodeID}\\Mods\\", this.errorLogPath);
            foreach (var mod in directoryFolderList)
            {
                //mod attachment migration
                var modSourcePath = configuration.SourcePath + $"\\{nodeID}\\Mods\\" + mod;
                var modList = _importFileService.GetAllFilesFromDirectory(modSourcePath, errorLogPath);
                var modDetail = _contractModificationService.GetModByContractGuidAndModNumber(contractGuid,mod);
                if (modDetail != null)
                {
                    ImportContractAttachment(modList, EnumGlobal.ResourceType.ContractModification.ToString(), contractGuid,modDetail.ContractModificationGuid, isDelete);
                }
                
            }
        }

        //Import attachment
        public void ImportAttachment(FileConfiguration configuration, Guid userGuid, string errorLogPath, bool isDelete)
        {
            this.errorLogPath = errorLogPath;
            this.loggedUser = userGuid;
            var directoryList = _importFileService.GetAllFolder(configuration.SourcePath, this.errorLogPath);
            var employeeSourcePath = string.Empty;
            var subContractorSourcePath = string.Empty;
            var wbsSourcePath = string.Empty;
            var revenueSourcePath = string.Empty;
            var attachmentList = new List<AttachmentModel>();
            var contractNumber = string.Empty;
            foreach (var node in directoryList)
            {
                int nodeID;
                if (int.TryParse(node, out nodeID))
                {
                    var contract = _contractsService.GetContractByTaskNodeID(nodeID);
                    if (contract != null)
                    {
                        //employee billing rate attachment migration
                        contractNumber = !string.IsNullOrWhiteSpace(contract.ContractNumber) ? contract.ContractNumber : "(NotProvided)";
                        employeeSourcePath = configuration.SourcePath + $"\\{nodeID}\\EmployeeBillingRates\\";
                        var employeeBillingRates = _importFileService.GetAllFilesFromDirectory(employeeSourcePath, errorLogPath);
                        ImportContractAttachment(employeeBillingRates, EnumGlobal.ResourceType.EmployeeBillingRates.ToString(), contract.ContractGuid, contract.ContractGuid,isDelete);

                        //sub contractor billing rates attachment migration
                        subContractorSourcePath = configuration.SourcePath + $"\\{nodeID}\\SubContractorBillingRates\\";
                        var subContractorBillingRateList = _importFileService.GetAllFilesFromDirectory(subContractorSourcePath, errorLogPath);
                        ImportContractAttachment(subContractorBillingRateList, EnumGlobal.ResourceType.LaborCategoryRates.ToString(), contract.ContractGuid, contract.ContractGuid, isDelete);

                        //work breakdown structure attachment migration
                        wbsSourcePath = configuration.SourcePath + $"\\{nodeID}\\WorkBreakDownStructure\\";
                        var wbsList = _importFileService.GetAllFilesFromDirectory(wbsSourcePath, errorLogPath);
                        ImportContractAttachment(wbsList, EnumGlobal.ResourceType.WorkBreakDownStructure.ToString(), contract.ContractGuid, contract.ContractGuid, isDelete);

                        //recenue recognization attachment migration
                        revenueSourcePath = configuration.SourcePath + $"\\{nodeID}\\RevenueRecognition\\";
                        var revenueList = _importFileService.GetAllFilesFromDirectory(revenueSourcePath, errorLogPath);
                        ImportContractAttachment(revenueList, EnumGlobal.ResourceType.RevenueRecognition.ToString(), contract.ContractGuid, contract.ContractGuid, isDelete);

                        //mod attachment migration
                        ImportContractModAttachment(configuration, nodeID, contract.ContractGuid,isDelete);

                        //import attachment for taskorder
                        if (isInitailCall)
                            initialConfiguration = configuration;
                        var isTaskAvailable = _contractsService.CheckTaskOdrderByContractGuid(contract.ContractGuid);
                        if (isTaskAvailable)
                        {
                            parentNodeID = node;
                            this.isInitailCall = false;
                            parentTaskOrderGuid = contract.ContractGuid;
                            var taskConfiguration = new FileConfiguration();
                            taskConfiguration.SourcePath = configuration.SourcePath;
                            taskConfiguration.DestinationPath = configuration.DestinationPath;
                            if (!configuration.SourcePath.Contains("TaskOrders"))
                                taskConfiguration.SourcePath = taskConfiguration.SourcePath + $"\\{parentNodeID}\\TaskOrders";
                            ImportAttachment(taskConfiguration, userGuid, errorLogPath,isDelete);
                        }
                        else if (!string.IsNullOrWhiteSpace(contract.TaskNodeID.ToString()) && parentNodeID == contract.MasterTaskNodeID.ToString())
                        {
                            continue;
                        }
                        else
                        {
                            parentTaskOrderGuid = contract.ContractGuid;
                            configuration = initialConfiguration;
                        }
                    }
                }
            }
        }
    }
}
