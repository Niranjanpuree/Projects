using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Models;

namespace Northwind.Core.Services.ContractRefactor
{
    public class ContractsService : IContractsService
    {
        private readonly IContractsRepository _contractRepository;
        private readonly IValidationHelper _validationHelper;
        private readonly IPscService _pscService;
        private readonly INaicsService _naicsService;
        private readonly ICountryService _countryService;
        private readonly IEnumerable<Psc> pscList;
        private readonly IEnumerable<Naics> naicsList;
        private readonly IRegionService _regionService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly IOfficeService _officeService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IRecentActivityService _recentActivityService;
        public ContractsService(IContractsRepository contractRepository, IValidationHelper validationHelper, IPscService pscService,
            IFarContractTypeService farContractTypeService,
            INaicsService naicsService, ICountryService countryService, IRegionService regionService, ICompanyService companyService,
            IUserService userService, IOfficeService officeService, IRecentActivityService recentActivityService)
        {
            _contractRepository = contractRepository;
            _validationHelper = validationHelper;
            _pscService = pscService;
            _naicsService = naicsService;
            _countryService = countryService;
            _regionService = regionService;
            _companyService = companyService;
            _userService = userService;
            _officeService = officeService;
            _farContractTypeService = farContractTypeService;
            pscList = _pscService.GetPscs();
            naicsList = _naicsService.GetNaicsList();
            _recentActivityService = recentActivityService;
        }

        #region Contract  

        private ContractUserRole GetContractUserRoleObject(Guid contractGuid, Guid userGuid, string userRole)
        {
            var contractUserRole = new ContractUserRole()
            {
                ContractGuid = contractGuid,
                UserGuid = userGuid,
                UserRole = userRole
            };
            return contractUserRole;
        }

        private List<ContractUserRole> GetEmptyRegionalKeyPersonList(Guid contractGuid)
        {
            var regionalKeyPersonList = new List<ContractUserRole>();
            var regionlRole = GetContractUserRoleObject(contractGuid, Guid.Empty, Contracts._regionalManager);
            regionalKeyPersonList.Add(regionlRole);
            var bdManager = GetContractUserRoleObject(contractGuid, Guid.Empty, Contracts._bdregionalManager);
            regionalKeyPersonList.Add(bdManager);
            var hsManager = GetContractUserRoleObject(contractGuid, Guid.Empty, Contracts._hsregionalManager);
            regionalKeyPersonList.Add(hsManager);
            var deputyManager = GetContractUserRoleObject(contractGuid, Guid.Empty, Contracts._deputyregionalManager);
            regionalKeyPersonList.Add(deputyManager);
            return regionalKeyPersonList;
        }

        private List<ContractUserRole> GetRegionalKeyPersonList(Region region,Guid contractGuid)
        {
            var regionalKeyList = new List<ContractUserRole>();
            if (region != null)
            {
                if (region.RegionalManager != null)
                {
                    var regionManager = GetContractUserRoleObject(contractGuid, region.RegionalManager, Contracts._regionalManager);
                    regionalKeyList.Add(regionManager);
                }
                if (region.BusinessDevelopmentRegionalManager != null)
                {
                    var bdManager = GetContractUserRoleObject(contractGuid, region.BusinessDevelopmentRegionalManager, Contracts._bdregionalManager);
                    regionalKeyList.Add(bdManager);
                }
                if (region.HSRegionalManager != null)
                {
                    var hsManager = GetContractUserRoleObject(contractGuid, region.HSRegionalManager, Contracts._hsregionalManager);
                    regionalKeyList.Add(hsManager);
                }
                if (region.DeputyRegionalManager != null)
                {
                    var deputyManager = GetContractUserRoleObject(contractGuid, region.DeputyRegionalManager, Contracts._deputyregionalManager);
                    regionalKeyList.Add(deputyManager);
                }
            }
            return regionalKeyList;
        }

        private List<ContractUserRole> GetOfficeKeyPersonList(Office office, Guid contractGuid)
        {
            var officeKeyPersonList = new List<ContractUserRole>();
            if (office != null)
            {
                if (office.OperationManagerGuid != Guid.Empty)
                {
                    var operationManager = GetContractUserRoleObject(contractGuid, office.OperationManagerGuid, Contracts._operationManager);
                    officeKeyPersonList.Add(operationManager);
                }
            }
            return officeKeyPersonList;
        }

        private List<ContractUserRole> GetEmptyOfficeKeyPersonList(Guid contractGuid)
        {
            var officeKeyPersonList = new List<ContractUserRole>();
            officeKeyPersonList.Add(GetContractUserRoleObject(contractGuid,Guid.Empty,Contracts._operationManager));
            return officeKeyPersonList;
        }

        public Contracts GetContractEntityByContractId(Guid contractId)
        {
            return _contractRepository.GetContractEntityByContractId(contractId);
        }

        public AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode)
        {
            return _contractRepository.GetCompanyRegionAndOfficeNameByCode(entityCode);
        }

        public Contracts GetDetailById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            var contract = _contractRepository.GetDetailById(id);
            if (contract.ParentContractGuid != null)
            {
                if (contract.ParentContractGuid != Guid.Empty)
                    contract.ParentContractNumber = _contractRepository.GetContractNumberByGuid(contract.ParentContractGuid ?? Guid.Empty);
            }
            if (contract.NAICSCode != null)
                contract.NAICS = naicsList.Where(x => x.NAICSGuid == contract.NAICSCode).FirstOrDefault();

            if (contract.PSCCode != null)
                contract.PSC = pscList.Where(x => x.PSCGuid == contract.PSCCode).FirstOrDefault();
            if (contract.CountryOfPerformance != null)
                contract.Country = _countryService.GetCountryByGuid(contract.CountryOfPerformance);

            //for binding company detail
            var companyCode = _contractRepository.GetCompanyCodeByContractId(id);
            var company = _companyService.GetCompanyByCode(companyCode);
            if (company != null)
                contract.CompanyName = company.CompanyName;

            //for binding region
            var regionCode = _contractRepository.GetRegionCodeByContractuid(id);
            if (!string.IsNullOrWhiteSpace(regionCode))
            {
                var region = _regionService.GetRegionByCode(regionCode);
                if (region != null)
                {
                    var regionUser = _userService.GetUserByUserGuid(region.RegionalManager);
                    if (regionUser != null)
                        contract.RegionName = region.RegionName;
                }
            }


            //for binding office detail
            var officeCode = _officeService.GetOfficeCodeByContractGuid(id);
            if (!string.IsNullOrWhiteSpace(officeCode))
            {
                var office = _officeService.GetOfficeByCode(officeCode);
                if (office != null)
                    contract.OfficeName = office.OfficeName;
            }
            //contract.PlaceOfPerformanceSelectedIds = _countryService.GetCountryAsString(contract.PlaceOfPerformance);

            contract.BasicContractInfo = _contractRepository.GetBasicContractInfoByContractGuid(id);
            contract.PlaceOfPerformanceSelectedIds = contract.PlaceOfPerformance;
            contract.BasicContractInfo.PlaceOfPerformanceSelected = _countryService.GetCountryAsString(contract.PlaceOfPerformance);
            contract.PlaceOfPerformance = _countryService.GetCountryAsString(contract.PlaceOfPerformance);

            return contract;
        }

        public IEnumerable<Contracts> GetContractList(string searchValue, string filterBy, Guid userGuid, int pageSize, int skip, int take, string orderBy, string dir)
        {
            var contractList = _contractRepository.GetContractList(searchValue, filterBy, userGuid, pageSize, skip, take, orderBy, dir);

            var list = new List<Contracts>();
            foreach (var contract in contractList)
            {
                //resource attribute value() get from cache..
                if (contract.NAICSCode != null)
                    contract.NAICS = naicsList.Where(x => x.NAICSGuid == contract.NAICSCode).FirstOrDefault();
                contract.IsFavorite = _recentActivityService.IsFavouriteActivity(contract.ContractGuid, "Contract", userGuid);
                if (contract.PSCCode != null)
                    contract.PSC = pscList.Where(x => x.PSCGuid == contract.PSCCode).FirstOrDefault();
                if (contract.CountryOfPerformance != null)
                    contract.Country = _countryService.GetCountryByGuid(contract.CountryOfPerformance);
                list.Add(contract);
            }
            return list;

        }

        public int GetContractCount(string searchValue)
        {
            return _contractRepository.GetContractCount(searchValue);
        }

        public int GetContractCountByFilter(string searchValue, string additionalFilter, Guid userGuid)
        {
            return _contractRepository.GetContractCountByFilter(searchValue, additionalFilter, userGuid);
        }

        public int GetAdvanceContractSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            return _contractRepository.GetAdvanceContractSearchCount(searchValue, postValue, userGuid, additionalFilter, isTaskOrder);
        }

        public ValidationResult Validate(Contracts contract)
        {
            var validationResult = new ValidationResult();
            var attributePair = new Dictionary<string, object>();
            List<string> message = new List<string>();

            //Check null for Contract Number
            if (_validationHelper.CheckNull(contract.ContractNumber) == false)
            {
                message.Add("Contract Number cannot be empty.");
                attributePair.Add("contractNumber", message);
                validationResult.Message = attributePair;
                validationResult.StatusName = ValidationStatus.Fail;
                return validationResult;
            }

            //Check null for Contract Title
            if (_validationHelper.CheckNull(contract.ContractTitle) == false)
            {
                message.Add("Contract Title cannot be empty");
                attributePair.Add("contractTitle", message);
                validationResult.Message = attributePair;
                validationResult.StatusName = ValidationStatus.Fail;
                return validationResult;
            }

            //string duplicateNumberMsg = "The Contract number  already exists. Please enter a different number.";
            //string duplicateTitleMsg = "The Contract title already exists. Please enter a different title.";

            //if (contract.ParentContractGuid != null)
            //{
            //    if (contract.ParentContractGuid != Guid.Empty)
            //    {
            //        duplicateNumberMsg = "The Task Order number  already exists. Please enter a different number.";
            //        duplicateTitleMsg = "The Task Order title already exists. Please enter a different title.";
            //    }
            //}

            ////Check for duplication of Contract Number
            //if (_contractRepository.IsContractNumberValid(contract.ContractNumber, contract.ContractGuid) == false)
            //{
            //    message.Add(duplicateNumberMsg);
            //    attributePair.Add("contractNumber", message);
            //    validationResult.Message = attributePair;
            //    validationResult.StatusName = ValidationStatus.Fail;
            //    return validationResult;
            //}

            ////Check for duplication of Contract Title
            //if (_contractRepository.IsContractTitleValid(contract.ContractTitle, contract.ContractGuid) == false)
            //{
            //    message.Add(duplicateTitleMsg);
            //    attributePair.Add("contractTitle", message);
            //    validationResult.Message = attributePair;
            //    validationResult.StatusName = ValidationStatus.Fail;
            //    return validationResult;
            //}

            validationResult.StatusName = ValidationStatus.Success;
            return validationResult;
        }

        public ValidationResult Save(Contracts contract)
        {
            var attributePair = new Dictionary<string, object>();
            List<string> message = new List<string>();
            var validationResult = Validate(contract);

            if (validationResult.StatusName.ToString() == "Success")
            {
                if (contract.ContractGuid == null || contract.ContractGuid == Guid.Empty)
                {
                    Guid id = Guid.NewGuid();
                    contract.ContractGuid = id;
                    //                    contract.CreatedOn = DateTime.Now;
                    //                    contract.UpdatedOn = DateTime.Now;
                    //                    contract.IsActive = true;
                    //                    contract.IsDeleted = false;

                    if (_contractRepository.Insert(contract))
                    {
                        //for binding company president
                        var companyCode = _contractRepository.GetCompanyCodeByContractId(id);
                        if (!string.IsNullOrWhiteSpace(companyCode))
                        {
                            var companyDetail = _companyService.GetCompanyByCode(companyCode);
                            if (companyDetail != null)
                            {
                                var companyPresident = companyDetail.President;
                                var presidentRole = new ContractUserRole()
                                {
                                    ContractGuid = id,
                                    UserGuid = companyPresident,
                                    UserRole = Contracts._companyPresident
                                };
                                contract.ContractUserRole.Add(presidentRole);
                            }
                        }

                        var officeCode = _contractRepository.GetOfficeCodeByContractGuid(contract.ContractGuid);
                    if (!string.IsNullOrWhiteSpace(officeCode))
                    {
                        var officedetail = _officeService.GetOfficeByCode(officeCode);
                        if (officedetail != null)
                        {
                            contract.ContractUserRole.AddRange(GetOfficeKeyPersonList(officedetail, contract.ContractGuid));
                        }
                        else
                        {
                            contract.ContractUserRole.AddRange(GetEmptyOfficeKeyPersonList(contract.ContractGuid));
                        }
                    }
                    else
                    {
                        contract.ContractUserRole.AddRange(GetEmptyOfficeKeyPersonList(contract.ContractGuid));
                    }

                        //for binding region manager
                        var regionCode = _contractRepository.GetRegionCodeByContractuid(id);
                        if (!string.IsNullOrWhiteSpace(regionCode))
                        {
                            var regiondetail = _regionService.GetRegionByCode(regionCode);
                            if (regiondetail != null)
                            {
                                contract.ContractUserRole.AddRange(GetRegionalKeyPersonList(regiondetail,id));
                            }
                            else
                            {
                                contract.ContractUserRole.AddRange(GetEmptyRegionalKeyPersonList(id));
                            }
                        }
                        else
                        {
                            contract.ContractUserRole.AddRange(GetEmptyRegionalKeyPersonList(id));
                        }
                        contract.ContractUserRole.ForEach(x => x.ContractGuid = id);
                        _contractRepository.InsertContractUsersList(contract.ContractUserRole);
                    }

                    message.Add("Contract has been successfully added.");
                    attributePair.Add("contract", message);
                    attributePair.Add("contractGuid", contract.ContractGuid);
                    validationResult.Message = attributePair;
                }
                else
                {
                    //                    contract.UpdatedOn = DateTime.Now;
                    if (contract.ContractUserRole.Count > 0)
                        contract.ContractUserRole.ForEach(x => x.ContractGuid = contract.ContractGuid);
                    var isUpdated = _contractRepository.Update(contract);
                    if (isUpdated && contract.ContractUserRole.Count > 0)
                    {
                        _contractRepository.UpdateContractUsersList(contract.ContractUserRole);
                    }
                    //for binding company president
                    var companyCode = _contractRepository.GetCompanyCodeByContractId(contract.ContractGuid);
                    if (!string.IsNullOrWhiteSpace(companyCode))
                    {
                        var companyDetail = _companyService.GetCompanyByCode(companyCode);
                        if (companyDetail != null)
                        {
                            var companyPresident = companyDetail.President;
                            var presidentRole = new ContractUserRole()
                            {
                                ContractGuid = contract.ContractGuid,
                                UserGuid = companyPresident,
                                UserRole = Contracts._companyPresident
                            };
                            contract.ContractUserRole.Add(presidentRole);
                        }
                        else
                        {
                            contract.ContractUserRole.Add(GetContractUserRoleObject(contract.ContractGuid, Guid.Empty, Contracts._companyPresident));
                        }
                    }
                    else
                    {
                        contract.ContractUserRole.Add(GetContractUserRoleObject(contract.ContractGuid, Guid.Empty, Contracts._companyPresident));
                    }

                    //for binding operation manager from office
                    var officeCode = _contractRepository.GetOfficeCodeByContractGuid(contract.ContractGuid);
                    if (!string.IsNullOrWhiteSpace(officeCode))
                    {
                        var officedetail = _officeService.GetOfficeByCode(officeCode);
                        if (officedetail != null)
                        {
                            contract.ContractUserRole.AddRange(GetOfficeKeyPersonList(officedetail, contract.ContractGuid));
                        }
                        else
                        {
                            contract.ContractUserRole.AddRange(GetEmptyOfficeKeyPersonList(contract.ContractGuid));
                        }
                    }
                    else
                    {
                        contract.ContractUserRole.AddRange(GetEmptyOfficeKeyPersonList(contract.ContractGuid));
                    }

                    //for binding region manager
                    var regionCode = _contractRepository.GetRegionCodeByContractuid(contract.ContractGuid);
                    if (!string.IsNullOrWhiteSpace(regionCode))
                    {
                        var regiondetail = _regionService.GetRegionByCode(regionCode);
                        if (regiondetail != null)
                        {
                            contract.ContractUserRole.AddRange(GetRegionalKeyPersonList(regiondetail, contract.ContractGuid));
                        }
                        else
                        {
                            contract.ContractUserRole.AddRange(GetEmptyRegionalKeyPersonList(contract.ContractGuid));
                        }
                    }
                    else
                    {
                        contract.ContractUserRole.AddRange(GetEmptyRegionalKeyPersonList(contract.ContractGuid));
                    }
                    _contractRepository.InsertContractUsersList(contract.ContractUserRole);
                    message.Add("Contract has been successfully updated.");
                    attributePair.Add("contract", message);
                    validationResult.Message = attributePair;
                }
            }
            return validationResult;
        }

        public bool IsExistContractNumber(string contractNumber, Guid contractGuid)
        {
            return _contractRepository.IsExistContractNumber(contractNumber, contractGuid);
        }

        public bool IsExistProjectNumber(string projectNumber, Guid contractGuid)
        {
            return _contractRepository.IsExistProjectNumber(projectNumber, contractGuid);
        }

        public bool IsExistContractTitle(string contractTitle, Guid contractGuid)
        {
            return _contractRepository.IsExistContractTitle(contractTitle, contractGuid);
        }

        public ValidationResult ImportValidation(Contracts contract)
        {
            var validationResult = new ValidationResult();
            var attributePair = new Dictionary<string, object>();
            List<string> message = new List<string>();

            //Check null for project Number
            if (_validationHelper.CheckNull(contract.ProjectNumber) == false)
            {
                message.Add("Project Number cannot be empty.");
                attributePair.Add("projectNumber", message);
                validationResult.Message = attributePair;
                validationResult.StatusName = ValidationStatus.Fail;
                return validationResult;
            }

            //Check null for Contract Title
            if (_validationHelper.CheckNull(contract.ContractTitle) == false)
            {
                message.Add("Contract Title cannot be empty");
                attributePair.Add("contractTitle", message);
                validationResult.Message = attributePair;
                validationResult.StatusName = ValidationStatus.Fail;
                return validationResult;
            }

            string duplicateProjectNumberMsg = "The project number  already exists. Please enter a different number.";
            string duplicateTitleMsg = "The Contract title already exists. Please enter a different title.";

            if (contract.ParentContractGuid != null)
            {
                if (contract.ParentContractGuid != Guid.Empty)
                {
                    duplicateProjectNumberMsg = "The Task Order number  already exists. Please enter a different number.";
                    duplicateTitleMsg = "The Task Order title already exists. Please enter a different title.";
                }
            }

            //Check for duplication of Contract Number
            if (_contractRepository.IsProjectNumberDuplicate(contract.ContractNumber, contract.ContractGuid) == false)
            {
                message.Add(duplicateProjectNumberMsg);
                attributePair.Add("contractNumber", message);
                validationResult.Message = attributePair;
                validationResult.StatusName = ValidationStatus.Fail;
                return validationResult;
            }

            //Check for duplication of Contract Title
            if (_contractRepository.IsExistContractTitle(contract.ContractTitle, contract.ContractGuid) == false)
            {
                message.Add(duplicateTitleMsg);
                attributePair.Add("contractTitle", message);
                validationResult.Message = attributePair;
                validationResult.StatusName = ValidationStatus.Fail;
                return validationResult;
            }

            validationResult.StatusName = ValidationStatus.Success;
            return validationResult;
        }

        public ValidationResult ImportContract(Contracts contract)
        {
            var attributePair = new Dictionary<string, object>();
            List<string> message = new List<string>();
            var validationResult = ImportValidation(contract);

            if (validationResult.StatusName.ToString() == "Success")
            {
                if (contract.ContractGuid == null || contract.ContractGuid == Guid.Empty)
                {
                    Guid id = Guid.NewGuid();
                    contract.ContractGuid = id;
                    if (_contractRepository.InsertContract(contract))
                    {
                        //for binding company president
                        var companyCode = _contractRepository.GetCompanyCodeByContractId(id);
                        if (!string.IsNullOrWhiteSpace(companyCode))
                        {
                            var companyDetail = _companyService.GetCompanyByCode(companyCode);
                            if (companyDetail != null)
                            {
                                var companyPresident = companyDetail.President;
                                var presidentRole = new ContractUserRole()
                                {
                                    ContractGuid = id,
                                    UserGuid = companyPresident,
                                    UserRole = Contracts._companyPresident
                                };
                                contract.ContractUserRole.Add(presidentRole);
                            }
                        }

                        //for binding region manager
                        var regionCode = _contractRepository.GetRegionCodeByContractuid(id);
                        if (!string.IsNullOrWhiteSpace(regionCode))
                        {
                            var regiondetail = _regionService.GetRegionByCode(regionCode);
                            if (regiondetail != null)
                            {
                                var regionManager = regiondetail.RegionalManager;
                                var regiontRole = new ContractUserRole()
                                {
                                    ContractGuid = id,
                                    UserGuid = regionManager,
                                    UserRole = Contracts._regionalManager
                                };
                                contract.ContractUserRole.Add(regiontRole);
                            }
                        }
                        contract.ContractUserRole.ForEach(x => x.ContractGuid = id);
                        _contractRepository.InsertContractUsersList(contract.ContractUserRole);
                    }



                    message.Add("Contract has been successfully added.");
                    attributePair.Add("contract", message);
                    attributePair.Add("contractGuid", contract.ContractGuid);
                    validationResult.Message = attributePair;
                }
                else
                {
                    //                    contract.UpdatedOn = DateTime.Now;
                    if (contract.ContractUserRole.Count > 0)
                        contract.ContractUserRole.ForEach(x => x.ContractGuid = contract.ContractGuid);
                    var isUpdated = _contractRepository.Update(contract);
                    if (isUpdated && contract.ContractUserRole.Count > 0)
                    {
                        _contractRepository.UpdateContractUsersList(contract.ContractUserRole);
                    }
                    //for binding company president
                    var companyCode = _contractRepository.GetCompanyCodeByContractId(contract.ContractGuid);
                    if (!string.IsNullOrWhiteSpace(companyCode))
                    {
                        var companyDetail = _companyService.GetCompanyByCode(companyCode);
                        if (companyDetail != null)
                        {
                            var companyPresident = companyDetail.President;
                            var presidentRole = new ContractUserRole()
                            {
                                ContractGuid = contract.ContractGuid,
                                UserGuid = companyPresident,
                                UserRole = Contracts._companyPresident
                            };
                            contract.ContractUserRole.Add(presidentRole);
                        }
                    }

                    //for binding region manager
                    var regionCode = _contractRepository.GetRegionCodeByContractuid(contract.ContractGuid);
                    if (!string.IsNullOrWhiteSpace(regionCode))
                    {
                        var regiondata = _regionService.GetRegionByCode(regionCode);
                        if (regiondata != null)
                        {
                            var regiontRole = new ContractUserRole()
                            {
                                ContractGuid = contract.ContractGuid,
                                UserGuid = regiondata.RegionalManager,
                                UserRole = Contracts._regionalManager
                            };
                            contract.ContractUserRole.Add(regiontRole);
                        }
                    }
                    _contractRepository.InsertContractUsersList(contract.ContractUserRole);
                    message.Add("Contract has been successfully updated.");
                    attributePair.Add("contract", message);
                    validationResult.Message = attributePair;
                }
            }
            return validationResult;
        }


        public IEnumerable<Contracts> GetTaskByContractGuid(Guid contractParentGuid, Guid userGuid)
        {
            var contractList = _contractRepository.GetTaskByContractGuid(contractParentGuid);
            var list = new List<Contracts>();
            foreach (var contract in contractList)
            {
                //contract.AwardAmount = contract.CumulativeAwardAmount;
                //contract.FundingAmount = contract.CumulativeFundingAmount;
                //resource attribute value() get from cache..
                if (contract.NAICSCode != null)
                    contract.NAICS = naicsList.Where(x => x.NAICSGuid == contract.NAICSCode).FirstOrDefault();
                contract.IsFavorite = _recentActivityService.IsFavouriteActivity(contract.ContractGuid, "Contract", userGuid);
                if (contract.PSCCode != null)
                    contract.PSC = pscList.Where(x => x.PSCGuid == contract.PSCCode).FirstOrDefault();
                if (contract.CountryOfPerformance != null)
                    contract.Country = _countryService.GetCountryByGuid(contract.CountryOfPerformance);
                if (!string.IsNullOrWhiteSpace(contract.PlaceOfPerformance))
                    contract.PlaceOfPerformance = _countryService.GetCountryAsString(contract.PlaceOfPerformance);
                list.Add(contract);
            }
            return list;

        }

        public bool Delete(Guid[] ids)
        {
            return _contractRepository.DeleteByGuid(ids);
        }

        public bool Enable(Guid[] ids)
        {
            return _contractRepository.EnableByGuid(ids);
        }

        public bool Disable(Guid[] ids)
        {
            return _contractRepository.DisableByGuid(ids);
        }

        public Contracts GetDetailsForProjectByContractId(Guid contractGuid)
        {
            var contract = _contractRepository.GetDetailsForProjectByContractId(contractGuid);
            if (contract.NAICSCode != null)
                contract.NAICS = naicsList.Where(x => x.NAICSGuid == contract.NAICSCode).FirstOrDefault();

            if (contract.PSCCode != null)
                contract.PSC = pscList.Where(x => x.PSCGuid == contract.PSCCode).FirstOrDefault();
            if (contract.CountryOfPerformance != null)
                contract.Country = _countryService.GetCountryByGuid(contract.CountryOfPerformance);

            //for binding company detail
            var companyCode = _contractRepository.GetCompanyCodeByContractId(contractGuid);
            var company = _companyService.GetCompanyByCode(companyCode);
            if (company != null)
                contract.CompanyName = company.CompanyName;

            //for binding region
            var regionCode = _contractRepository.GetRegionCodeByContractuid(contractGuid);
            if (!string.IsNullOrWhiteSpace(regionCode))
            {
                var region = _regionService.GetRegionByCode(regionCode);
                if (region != null)
                {
                    var regionUser = _userService.GetUserByUserGuid(region.RegionalManager);
                    contract.RegionName = region.RegionName;
                }
            }


            //for binding office detail
            var officeCode = _officeService.GetOfficeCodeByContractGuid(contractGuid);
            if (!string.IsNullOrWhiteSpace(officeCode))
            {
                var office = _officeService.GetOfficeByCode(officeCode);
                if (office != null)
                    contract.OfficeName = office.OfficeName;
            }
            if (contract.ParentContractGuid != Guid.Empty)
                contract.ParentContractNumber = _contractRepository.GetContractNumberByGuid(contract.ParentContractGuid ?? Guid.Empty);
            contract.BasicContractInfo = _contractRepository.GetBasicContractInfoByContractGuid(contractGuid);
            return contract;
        }

        public string GetContractNumberById(Guid id)
        {
            return _contractRepository.GetContractNumberByGuid(id);
        }

        public Guid GetContractIdByProjectId(Guid contratGuid)
        {
            return _contractRepository.GetContractIdByProjectId(contratGuid);
        }

        public Guid? GetPreviousTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            return _contractRepository.GetPreviousTaskOfContractByCounter(contractGuid, currentProjectCounter);
        }

        public Guid? GetNextTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            return _contractRepository.GetNextTaskOfContractByCounter(contractGuid, currentProjectCounter);
        }

        public bool IsIDIQContract(Guid contractGuid)
        {
            return _contractRepository.IsIDIQContract(contractGuid);
        }

        public Guid GetContractGuidByProjectNumber(string projectNumber)
        {
            return _contractRepository.GetContractGuidByProjectNumber(projectNumber);
        }

        public IEnumerable<Contracts> GetAllProject(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            return _contractRepository.GetAllProject(contractGuid, searchValue, pageSize, skip, sortField, sortDirection);
        }

        public Contracts GetContractByContractNumber(string contractNumber)
        {
            if (string.IsNullOrWhiteSpace(contractNumber))
                return null;
            return _contractRepository.GetContractByContractNumber(contractNumber);
        }

        public Guid GetContractByContractNumberProjectNumberAndTitle(string contractNumber, string projectNumber, string title)
        {
            if (string.IsNullOrWhiteSpace(contractNumber) || string.IsNullOrWhiteSpace(projectNumber) || string.IsNullOrWhiteSpace(title))
                return Guid.Empty;
            return _contractRepository.GetContractByContractNumberProjectNumberAndTitle(contractNumber, projectNumber, title);
        }

        public Guid GetParentContractByContractNumber(string contractNumber)
        {
            if (string.IsNullOrWhiteSpace(contractNumber))
                return Guid.Empty;
            return _contractRepository.GetParentContractByContractNumber(contractNumber);
        }

        public bool IsProjectNumberDuplicate(string projectNumber, Guid contractGuid)
        {
            if (string.IsNullOrWhiteSpace(projectNumber))
                return false;
            return _contractRepository.IsProjectNumberDuplicate(projectNumber, contractGuid);
        }

        public Contracts GetContractByProjectNumber(string projectNumber)
        {
            if (string.IsNullOrWhiteSpace(projectNumber))
                return null;
            return _contractRepository.GetContractByProjectNumber(projectNumber);
        }

        public Contracts GetContractByTaskNodeID(int taskNodeID)
        {
            return _contractRepository.GetContractByTaskNodeID(taskNodeID);
        }

        public Guid GetParentContractByMasterTaskNodeID(int masterTaskNodeID)
        {
            return _contractRepository.GetParentContractByMasterTaskNodeID(masterTaskNodeID);
        }

        public bool CheckTaskOdrderByContractGuid(Guid parentContractGuid)
        {
            if (parentContractGuid == Guid.Empty)
                return false;
            return _contractRepository.CheckTaskOdrderByContractGuid(parentContractGuid);
        }
        #endregion Contract

        #region Contract User Role
        public IEnumerable<ContractUserRole> GetKeyPersonnelByContractGuid(Guid contractGuid)
        {
            return _contractRepository.GetKeyPersonnelByContractGuid(contractGuid);
        }
        public bool InsertContractUsers(List<ContractUserRole> contractUser)
        {
            return _contractRepository.InsertContractUsersList(contractUser);
        }

        public bool UpdateContractUsers(List<ContractUserRole> contractUser)
        {
            return _contractRepository.UpdateContractUsersList(contractUser);
        }

        public bool UpdateProjectNumberByGuid(Guid contractGuid, string projectNumber)
        {
            return _contractRepository.UpdateProjectNumberByGuid(contractGuid, projectNumber);
        }

        public IEnumerable<ContractUserRole> GetKeyPersonnels(string keyPersonnelType)
        {
            return _contractRepository.GetKeyPersonnels(keyPersonnelType);
        }

        public List<string> GetContractRoleByUserGuid(Guid contractGuid, Guid userGuid)
        {
            if ((contractGuid == null || contractGuid == Guid.Empty) && (userGuid == null || userGuid == Guid.Empty))
            {
                return null;
            }
            return _contractRepository.GetContractRoleByUserGuid(contractGuid, userGuid);
        }

        public bool UpdateAllUserByRole(Guid userGuid, string userRole)
        {
            if (userGuid == Guid.Empty || string.IsNullOrWhiteSpace(userRole))
                return false;
            _contractRepository.UpdateAllUserByRole(userGuid,userRole);
            return true;
        }
        #endregion

        #region Contract Modification
        public IEnumerable<ContractModification> GetAllContractMod(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            return _contractRepository.GetAllContractMod(contractGuid, searchValue, pageSize, skip, sortField, sortDirection);
        }

        public int TotalRecord(Guid contractGuid)
        {
            return _contractRepository.TotalRecord(contractGuid);
        }

        public int InsertMod(ContractModification contractModificationModel)
        {
            return _contractRepository.InsertMod(contractModificationModel);
        }

        public int UpdateMod(ContractModification contractModificationModel)
        {
            return _contractRepository.UpdateMod(contractModificationModel);
        }

        public int DeleteMod(Guid[] ids)
        {
            return _contractRepository.DeleteMod(ids);
        }

        public int DisableMod(Guid[] ids)
        {
            return _contractRepository.DeleteMod(ids);
        }

        public bool EnableMod(Guid[] ids)
        {
            return _contractRepository.EnableByGuid(ids);
        }

        public ContractModification GetModDetailById(Guid id)
        {
            return _contractRepository.GetModDetailById(id);
        }

        public bool IsExistModificationNumber(Guid contractGuid, string modificationNumber)
        {
            return _contractRepository.IsExistModificationNumber(contractGuid, modificationNumber);
        }
        #endregion Contract Modification

        #region ContractFiles
        public IEnumerable<ContractResourceFile> GetFileListByContractGuid(Guid contractGuid)
        {
            return _contractRepository.GetFileListByContractGuid(contractGuid);
        }
        public IEnumerable<ContractResourceFile> GetFileListByContentResourceGuid(Guid contractGuid)
        {
            return _contractRepository.GetFileListByContentResourceGuid(contractGuid);
        }

        public ContractResourceFile GetFilesByContractFileGuid(Guid contractGuid)
        {
            return _contractRepository.GetFilesByContractFileGuid(contractGuid);


        }

        public IEnumerable<ContractResourceFile> GetFilesByContractResourceGuid(Guid contractGuid)
        {
            return _contractRepository.GetFilesByContractResourceFileGuid(contractGuid);


        }

        public IEnumerable<ContractResourceFile> GetFilesAndFolders(Guid contractGuid, string column)
        {
            return _contractRepository.GetFilesAndFolders(contractGuid, column);


        }

        public IEnumerable<ContractResourceFile> GetFilesAndFoldersByParentId(Guid parentId)
        {
            return _contractRepository.GetFilesAndFoldersByParentId(parentId);


        }

        public ContractResourceFile GetFilesByContractResourceFileGuid(Guid contractGuid)
        {
            var test = _contractRepository.GetFilesByContractFileGuid(contractGuid);
            return test;

        }

        public ContractResourceFile GetFilesByContractGuid(Guid contractGuid, string formType)
        {
            return _contractRepository.GetFilesByContractGuid(contractGuid, formType);
        }

        public bool InsertContractFile(ContractResourceFile file)
        {
            return _contractRepository.InsertContractFile(file);
        }

        public bool CheckAndInsertContractFile(ContractResourceFile file)
        {
            return _contractRepository.CheckAndInsertContractFile(file);
        }

        public Guid InsertContractFileAndGetIdBack(ContractResourceFile file)
        {
            return _contractRepository.InsertContractFileAndGetIdBack(file);
        }

        public bool DeleteContractFolder(Guid fileId)
        {
            return _contractRepository.DeleteContractFolder(fileId);
        }

        public bool RenameContractFolder(Guid fileId, string FolderName)
        {
            return _contractRepository.RenameContractFolder(fileId, FolderName);
        }
        //special case update file..
        public bool UpdateContractFile(ContractResourceFile file)
        {
            return _contractRepository.UpdateContractFile(file);
        }

        public bool UpdateFile(ContractResourceFile file)
        {
            return _contractRepository.UpdateFile(file);
        }

        public bool DeleteContractFileById(Guid contractFileGuid)
        {
            return _contractRepository.DeleteContractFileById(contractFileGuid);
        }

        public bool DeleteContractFileByContractGuid(Guid contractGuid, string formType)
        {
            return _contractRepository.DeleteContractFileByContractGuid(contractGuid, formType);
        }

        public bool UpdateFileName(Guid contractFileGuid, string filename)
        {
            return _contractRepository.UpdateFileName(contractFileGuid, filename);
        }

        public IEnumerable<Contracts> GetContractList(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            var contractList = _contractRepository.GetContractList(searchValue, pageSize, skip, take, orderBy, dir, postValue, userGuid, additionalFilter, isTaskOrder);
            var list = new List<Contracts>();
            foreach (var contract in contractList)
            {
                //resource attribute value() get from cache..
                //contract.AwardAmount = contract.CumulativeAwardAmount;
                //contract.FundingAmount = contract.CumulativeFundingAmount;
                if (contract.NAICSCode != null)
                    contract.NAICS = naicsList.Where(x => x.NAICSGuid == contract.NAICSCode).FirstOrDefault();
                contract.IsFavorite = _recentActivityService.IsFavouriteActivity(contract.ContractGuid, "Contract", userGuid);
                if (contract.PSCCode != null)
                    contract.PSC = pscList.Where(x => x.PSCGuid == contract.PSCCode).FirstOrDefault();
                if (contract.CountryOfPerformance != null)
                    contract.Country = _countryService.GetCountryByGuid(contract.CountryOfPerformance);
                if (!string.IsNullOrWhiteSpace(contract.PlaceOfPerformance))
                    contract.PlaceOfPerformance = _countryService.GetCountryAsString(contract.PlaceOfPerformance);
                list.Add(contract);
            }
            return list;
        }

        public ICollection<Organization> GetOrganizationData(string searchText)
        {
            return _contractRepository.GetOrganizationData(searchText);
        }

        public Organization GetOrganizationByOrgId(Guid orgId)
        {
            return _contractRepository.GetOrganizationByOrgId(orgId);
        }

        public string GetContractType(Guid contratGuid)
        {
            return _contractRepository.GetContractType(contratGuid);
        }

        public bool InsertRevenueRecognitionGuid(Guid revenueRecognition, Guid contractGuid)
        {
            return _contractRepository.InsertRevenueRecognitionGuid(revenueRecognition, contractGuid);
        }

        public bool SetNullRevenueRecognitionGuid(Guid contractGuid, decimal? awardAmount, decimal? fundingAmount)
        {
            return _contractRepository.SetNullRevenueRecognitionGuid(contractGuid, awardAmount, fundingAmount);
        }

        public Contracts GetAmountById(Guid id)
        {
            return _contractRepository.GetAmountById(id);
        }

        public Contracts.ContractBasicDetails GetOnlyRequiredContractData(Guid id)
        {
            return _contractRepository.GetOnlyRequiredContractData(id);
        }

        public bool CloseContractStatus(Guid contractGuid)
        {
            return _contractRepository.CloseContractStatus(contractGuid);
        }
        public QuestionaireUserAnswer GetAnswer(Guid contractGuid)
        {
            return _contractRepository.GetAnswer(contractGuid);
        }

        public Guid? GetParentContractGuidByContractGuid(Guid contractGuid)
        {
            return _contractRepository.GetParentContractGuidByContractGuid(contractGuid);
        }

        public BasicContractInfoModel GetBasicContractById(Guid id)
        {
            return _contractRepository.GetBasicContractById(id);
        }

        public string GetProjectNumberById(Guid contratGuid)
        {
            return _contractRepository.GetProjectNumberById(contratGuid);
        }

        public string GetOrgNameById(Guid id)
        {
            return _contractRepository.GetOrgNameById(id);
        }

        public ContractResourceFile GetFileByResourceGuid(Guid resourceGuid, string formType)
        {
            return _contractRepository.GetFileByResourceGuid(resourceGuid, formType);
        }

        #endregion End of contract files

        public Guid GetFarContractTypeGuidById(Guid id)
        {
            return _contractRepository.GetFarContractTypeGuidById(id);
        }

        public int GetTotalCountProjectByContractId(Guid contractGuid)
        {
            return _contractRepository.GetTotalCountProjectByContractId(contractGuid);
        }

        public int HasChild(Guid projectGuid)
        {
            return _contractRepository.HasChild(projectGuid);
        }

        public Contracts GetInfoByContractGuid(Guid contractGuid)
        {
            return _contractRepository.GetInfoByContractGuid(contractGuid);
        }
    }
}
