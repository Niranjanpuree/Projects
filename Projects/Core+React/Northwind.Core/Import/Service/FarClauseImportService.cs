using CsvHelper;
using Northwind.Core.Entities;
using Northwind.Core.Import.Helper;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Northwind.Core.Import.Service
{
    public class FarClauseImportService : IFarClauseImportService
    {
        private readonly IImportFileService _importFileService;
        private readonly IExportCSVService _exportCsvService;
        private readonly IContractsService _contractsService;
        private readonly IFarContractService _farContractService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IFarContractTypeClauseService _farContractTypeClauseService;
        private readonly IFarClauseService _farClauseService;
        private readonly IQuestionaireMasterService _questionaireMasterService;
        private readonly IQuestionaireUserAnswerService _questionaireUserAnswerService;

        private Core.Entities.FarContract farContractEntity;
        private Core.Entities.QuestionaireUserAnswer questionaireUserAnswerEntity;
        private string inputFolderPath = string.Empty;
        private string logOutputPath = string.Empty;
        private string fileExtension = string.Empty;
        private string errorLogPath = string.Empty;
        private string errorFile = "FarClause.csv";
        private string fileNameToAppend = "FarClause_Import_Log";
        private string copyFileName = "FarClause_Original";
        private string otherFarContractTypeCode = "other";
        private string[] trueBooleanArray = { "1", "yes", "true", "y" };

        public FarClauseImportService(IImportFileService importFileService, IExportCSVService exportCSVService,
            IContractsService contractsService, IFarContractService farContractService, IFarContractTypeService farContractTypeService,
            IFarContractTypeClauseService farContractTypeClauseService, IFarClauseService farClauseService,IQuestionaireMasterService questionaireMasterService,
            IQuestionaireUserAnswerService questionaireUserAnswerService)
        {
            _importFileService = importFileService;
            _exportCsvService = exportCSVService;
            _contractsService = contractsService;
            _farContractService = farContractService;
            _farContractTypeService = farContractTypeService;
            _farContractTypeClauseService = farContractTypeClauseService;
            _farClauseService = farClauseService;
            _questionaireMasterService = questionaireMasterService;
            _questionaireUserAnswerService = questionaireUserAnswerService;
        }

        #region far clause
        private List<DMFarClause> GetFarClauseListFromCsvFile<DMFarClause>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    var mapper = new DMFarClauseHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);
                    var recordList = csv.GetRecords<DMFarClause>().ToList();
                    return recordList;
                }
            }
            catch (Exception e)
            {
                var errorMsg = e.Message;
                throw;
            }

        }

        private IList<DMFarClause> ImportFarClause(List<DMFarClause> farClauseList, Guid userGuid)
        {
            var exportFarClauseList = new List<DMFarClause>();
            foreach (var farClause in farClauseList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(farClause.ProjectNumber))
                    {
                        var contract = _contractsService.GetContractByProjectNumber(farClause.ProjectNumber.Trim());
                        if (contract != null)
                        {
                            farClause.FarContractTypeGuid = contract.FarContractTypeGuid;
                            var clauseNumber = string.Empty;
                            if (trueBooleanArray.Contains(farClause.CPARS))
                            {
                                clauseNumber = "42.15";
                                InsertFarContract(farClause, userGuid, contract.ContractGuid, clauseNumber);
                            }

                            if (trueBooleanArray.Contains(farClause.GovernmentFurnished))
                            {
                                clauseNumber = "52.245-1";
                                InsertFarContract(farClause, userGuid, contract.ContractGuid, clauseNumber);
                            }


                            if (trueBooleanArray.Contains(farClause.ProgressTowardsSB))
                            {
                                clauseNumber = "52.219-9";
                                InsertFarContract(farClause, userGuid, contract.ContractGuid, clauseNumber);
                            }


                            if (trueBooleanArray.Contains(farClause.ReportingExecutiveCompensation))
                            {
                                clauseNumber = "52.204-1";
                                InsertFarContract(farClause, userGuid, contract.ContractGuid, clauseNumber);
                            }
                        }
                        else
                        {
                            farClause.ImportStatus = ImportStatus.Fail.ToString();
                            farClause.Reason = $"Contract with project number {farClause.ProjectNumber} not found";
                        }
                    }
                    else
                    {
                        farClause.ImportStatus = ImportStatus.Fail.ToString();
                        farClause.Reason = "Project number is empty";
                    }

                    exportFarClauseList.Add(farClause);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return exportFarClauseList;
        }

        private void InsertFarContract(DMFarClause farClause, Guid userGuid, Guid contractGuid, string clauseNumber)
        {
            farContractEntity = new FarContract();
            var dbFarClause = _farClauseService.GetFarClauseByNumber(clauseNumber);
            var farClauseGuid = Guid.Empty;
            if (dbFarClause != null)
                farClauseGuid = dbFarClause.FarClauseGuid;

            if (farClause.FarContractTypeGuid == Guid.Empty)
            {
                var contractType = _farContractTypeService.GetByCode(this.otherFarContractTypeCode);
                if (contractType != null)
                    farClause.FarContractTypeGuid = contractType.FarContractTypeGuid;
            }

            var farContractTypeClause = _farContractTypeClauseService.GetByFarClauseFarContractTypeGuid(farClauseGuid, farClause.FarContractTypeGuid);
            if (farContractTypeClause != null)
            {
                farContractEntity.FarContractTypeClauseGuid = farContractTypeClause.FarContractTypeClauseGuid;
                farContractEntity.IsDeleted = false;
                farContractEntity.CreatedBy = userGuid;
                farContractEntity.UpdatedBy = userGuid;
                farContractEntity.CreatedOn = DateTime.UtcNow;
                farContractEntity.UpdatedOn = DateTime.UtcNow;
                farContractEntity.ContractGuid = contractGuid;
                _farContractService.Add(farContractEntity);
                farClause.ImportStatus = ImportStatus.Success.ToString();
                farClause.Reason = $"Far Clause added for project number {farClause.ProjectNumber} has been added successfully";
            }
            else
            {
                farClause.ImportStatus = ImportStatus.Fail.ToString();
                farClause.Reason += $"Far Clause mapping not found for project number {farClause.ProjectNumber} on {clauseNumber}.";
            }
        }

        public void ImportFarClauseData(object configuration, Guid userGuid, string errorPath, bool isDelete)
        {
            var configDictionary = _importFileService.GetFileUsingJToken(configuration.ToString());
            this.errorLogPath = errorPath + this.errorFile;
            var importConfigData = _importFileService.GetConfigurationSetting(configuration.ToString());

            if (importConfigData != null)
            {
                if (!string.IsNullOrWhiteSpace(importConfigData.InputFolderPath))
                {
                    this.inputFolderPath = importConfigData.InputFolderPath;
                    this.logOutputPath = importConfigData.LogOutputPath;

                    //get list of csv file
                    var csvFile = _importFileService.GetAllCsvFilesFromDirectory(this.inputFolderPath, errorLogPath);
                    if (csvFile != null && csvFile.Length > 0)
                    {
                        var farClauseHeader = _importFileService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        foreach (var file in csvFile)
                        {
                            var farClauseList = GetFarClauseListFromCsvFile<DMFarClause>(file, farClauseHeader);
                            var fileHeader = _importFileService.GetCSVHeaderFromFile(file, farClauseHeader);
                            //import customer contact
                            var importedList = ImportFarClause(farClauseList, userGuid);
                            var fileName = Path.GetFileNameWithoutExtension(file);
                            this.fileExtension = Path.GetExtension(file);
                            var exportFileName = _exportCsvService.GetExportFileName(fileName, this.fileNameToAppend, this.fileExtension);

                            //list for exporting to csv with status
                            var exportHeader = _exportCsvService.GetExportCSVHeader(fileHeader);
                            var exportList = MapperHelper.GetExportList(importedList, exportHeader);

                            var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList.ToList(), logOutputPath, exportFileName, exportHeader);
                            if (isFileSaved)
                                _importFileService.MoveFile(inputFolderPath, this.logOutputPath, fileName, fileExtension, isDelete);
                        }
                    }
                }
            }
        }
        #endregion far clause

        #region questionaire
        private List<DMQuestionaire> GetQuestionaireListFromCsvFile<DMQuestionaire>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    var mapper = new DMQuestionaireHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);
                    var recordList = csv.GetRecords<DMQuestionaire>().ToList();
                    return recordList;
                }
            }
            catch (Exception e)
            {
                var errorMsg = e.Message;
                throw;
            }

        }

        private IList<DMQuestionaire> ImportQuestionaire(List<DMQuestionaire> questionList, Guid userGuid)
        {
            var questionaireList = new List<DMQuestionaire>();
            foreach (var questionaire in questionList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(questionaire.ProjectNumber))
                    {
                        var contract = _contractsService.GetContractByProjectNumber(questionaire.ProjectNumber.Trim());
                        if (contract != null)
                        {
                            questionaire.Answer = "No";
                            var question = string.Empty;
                            if (trueBooleanArray.Contains(questionaire.GSAR))
                            {
                                question = "GSA Schedule Sales & Industrial Funding Fee (GSAR 552.238-74)";
                                questionaire.Answer = "Yes";
                                InsertQuestionaire(questionaire, userGuid, contract.ContractGuid, question);
                            }

                            if (trueBooleanArray.Contains(questionaire.GQAC))
                            {
                                question = "Government-Wide Acquisition Contracts (GQAC), examples: GSA – Alliant, STARS) or NIH CIO-SP3)";
                                questionaire.Answer = "Yes";
                                InsertQuestionaire(questionaire, userGuid, contract.ContractGuid, question);
                            }


                            if (trueBooleanArray.Contains(questionaire.ServiceContractReporting))
                            {
                                question = "Service Contract Reporting";
                                questionaire.Answer = "Yes";
                                InsertQuestionaire(questionaire, userGuid, contract.ContractGuid, question);
                            }

                            if (trueBooleanArray.Contains(questionaire.Warranties))
                            {
                                question = "Warranties";
                                questionaire.Answer = "Yes";
                                InsertQuestionaire(questionaire, userGuid, contract.ContractGuid, question);
                            }
                        }
                        else
                        {
                            questionaire.ImportStatus = ImportStatus.Fail.ToString();
                            questionaire.Reason = $"Contract with project number {questionaire.ProjectNumber} not found";
                        }
                    }
                    else
                    {
                        questionaire.ImportStatus = ImportStatus.Fail.ToString();
                        questionaire.Reason = "Project number is empty";
                    }

                    questionaireList.Add(questionaire);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return questionaireList;
        }

        private DMQuestionaire InsertQuestionaire(DMQuestionaire questionaire, Guid userGuid, Guid contractGuid, string question)
        {
            questionaireUserAnswerEntity = new QuestionaireUserAnswer();
            var questionaireMaster = _questionaireMasterService.GetQuestionaireByQuestion(question);
            if (questionaireMaster != null)
            {
                questionaireUserAnswerEntity.QuestionaireUserAnswerGuid = Guid.NewGuid();
                questionaireUserAnswerEntity.QuestionaireMasterGuid = questionaireMaster.QuestionaireMasterGuid;
                questionaireUserAnswerEntity.Questions = question;
                questionaireUserAnswerEntity.Answer = questionaire.Answer;
                questionaireUserAnswerEntity.CreatedBy = userGuid;
                questionaireUserAnswerEntity.UpdatedBy = userGuid;
                questionaireUserAnswerEntity.CreatedOn = DateTime.UtcNow;
                questionaireUserAnswerEntity.UpdatedOn = DateTime.UtcNow;
                questionaireUserAnswerEntity.ContractGuid = contractGuid;
                _questionaireUserAnswerService.Add(questionaireUserAnswerEntity);
                questionaire.ImportStatus = ImportStatus.Success.ToString();
                questionaire.Reason = $"Questionaire for project number {questionaire.ProjectNumber} has been added successfully";
            }
            else
            {
                questionaire.ImportStatus = ImportStatus.Fail.ToString();
                questionaire.Reason += $"Unable to add questionaire for project number {questionaire.ProjectNumber} on {question}.";
            }
            return questionaire;
        }

        public void ImportQuestionaireData(object configuration, Guid userGuid, string errorPath, bool isDelete)
        {
            var configDictionary = _importFileService.GetFileUsingJToken(configuration.ToString());
            this.errorLogPath = errorPath + this.errorFile;
            var importConfigData = _importFileService.GetConfigurationSetting(configuration.ToString());

            if (importConfigData != null)
            {
                if (!string.IsNullOrWhiteSpace(importConfigData.InputFolderPath))
                {
                    this.inputFolderPath = importConfigData.InputFolderPath;
                    this.logOutputPath = importConfigData.LogOutputPath;

                    //get list of csv file
                    var csvFile = _importFileService.GetAllCsvFilesFromDirectory(this.inputFolderPath, errorLogPath);
                    if (csvFile != null && csvFile.Length > 0)
                    {
                        var questionaireHeader = _importFileService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        foreach (var file in csvFile)
                        {
                            var questionaireList = GetQuestionaireListFromCsvFile<DMQuestionaire>(file, questionaireHeader);
                            var fileHeader = _importFileService.GetCSVHeaderFromFile(file, questionaireHeader);
                            //import customer contact
                            var importedList = ImportQuestionaire(questionaireList, userGuid);
                            var fileName = Path.GetFileNameWithoutExtension(file);
                            this.fileExtension = Path.GetExtension(file);
                            var exportFileName = _exportCsvService.GetExportFileName(fileName, this.fileNameToAppend, this.fileExtension);

                            //list for exporting to csv with status
                            var exportHeader = _exportCsvService.GetExportCSVHeader(fileHeader);
                            var exportList = MapperHelper.GetExportList(importedList, exportHeader);

                            var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList.ToList(), logOutputPath, exportFileName, exportHeader);
                            if (isFileSaved)
                                _importFileService.MoveFile(inputFolderPath, this.logOutputPath, fileName, fileExtension, isDelete);
                        }
                    }
                }
            }
        }
        #endregion questionaire
    }
}
