using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System.Linq;
using System.Net.Sockets;
using System.Text;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Infrastructure.Data.Contract
{
    public class ContractRepository : IContractRepository
    {
        IDatabaseContext _context;
        public ContractRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int TotalRecord()
        {
            string sql = "SELECT Count(1) FROM contract WHERE IsDeleted = 0";
            var result = _context.Connection.QuerySingle<int>(sql);
            return result;
        }

        public int Add(Core.Entities.Contract contractModel)
        {
            string insertQuery = $@"INSERT INTO [dbo].[Contract]
																   (
																		ContractGuid					                            ,
																		ParentContractGuid					                        ,
																		IsIDIQContract						                        ,
																		IsPrimeContract						                        ,
																		ContractNumber				                                ,
																		SubContractNumber			                                ,
																		ORGID						                                ,
																		ProjectNumber				                                ,
																		ContractTitle				                                ,
																		Description				                                    ,
																		CountryOfPerformance			                            ,
																		PlaceOfPerformance			                                ,
																		POPStart						                            ,
																		POPEnd						                                ,
																		NAICSCode					                                ,
																		PSCCode						                                ,
																		CPAREligible					                            ,
																		QualityLevelRequirements		                            ,
																		QualityLevel					                            ,
																																	
																		CompanyPresident					                        ,
																		RegionalManager					                            ,
																		ProjectManager					                            ,
																		ProjectControls					                            ,
																		AccountingRepresentative			                        ,
																		ContractRepresentative			                            ,
																																	
																		AwardingAgencyOffice				                        ,
																		OfficeContractRepresentative		                        ,
																		OfficeContractTechnicalRepresent                           ,
																		FundingAgencyOffice				                            ,
																		FundingOfficeContractRepresentative				        ,
																		FundingOfficeContractTechnicalRepresent				    ,
																																	
																		SetAside						                            ,
																		SelfPerformancePercent			                            ,
																		SBA								                            ,
																		Competition						                            ,
																		ContractType					                            ,
																		OverHead						                            ,
																		GAPercent						                            ,
																		FeePercent						                            ,
																		Currency						                            ,
																		BlueSkyAwardAmount				                            ,
																		AwardAmount						                            ,
																		FundingAmount					                            ,
																		BillingAddress					                            ,
																		BillingFrequency				                            ,
																		InvoiceSubmissionMethod			                            ,
																		PaymentTerms					                            ,
																		AppWageDetermineDavisBaconAct				                ,
																		AppWageDetermineServiceContractAct				            ,
																		BillingFormula					                            ,
																		RevenueFormula					                            ,
																		RevenueRecognitionEACPercent	                            ,
																		OHonsite						                            ,
																		OHoffsite						                            ,
																		ProjectCounter						                        ,
																		CreatedOn						                            ,
																		UpdatedOn						                            ,
																		CreatedBy						                            ,
																		UpdatedBy						                            ,
																		IsActive						                            ,
																		IsDeleted                                                   
																	)
								  VALUES (
																		@ContractGuid                                       ,
																		@ParentContractGuid                                ,
																		@IsIDIQContract                                     ,
																		@IsPrimeContract                                    ,
																		@ContractNumber                                     ,
																		@SubContractNumber                                  ,
																		@ORGID                                              ,
																		@ProjectNumber                                      ,
																		@ContractTitle                                      ,
																		@Description                                        ,
																		@CountryOfPerformance                               ,
																		@PlaceOfPerformanceSelectedIds                      ,
																		@POPStart                                           ,
																		@POPEnd                                             ,
																		@NAICSCode                                          ,
																		@PSCCode                                            ,
																		@CPAREligible                                       ,
																		@QualityLevelRequirements                           ,
																		@QualityLevel                                       ,
																																						
																		@CompanyPresident                                   ,
																		@RegionalManager                                    ,
																		@ProjectManager                                     ,
																		@ProjectControls                                    ,
																		@AccountingRepresentative                           ,
																		@ContractRepresentative                             ,
																																						 
																		@AwardingAgencyOffice                             ,	
																		@OfficeContractRepresentative                    ,	
																		@OfficeContractTechnicalRepresent                ,
																		@FundingAgencyOffice                              ,
																		@FundingOfficeContractRepresentative             ,
																		@FundingOfficeContractTechnicalRepresent         ,
																		
																		@setAside                                         ,
																		@SelfPerformancePercent                          ,
																		@SBA                                              ,
																		@Competition                                      ,
																		@ContractType                                     ,
																		@OverHead                                         ,
																		@GAPercent                                      ,
																		@FeePercent                                      ,
																		@Currency                                         ,
																		@BlueSkyAwardAmount                              ,
																		@AwardAmount                                      ,
																		@FundingAmount                                    ,
																		@BillingAddress                                   ,
																		@BillingFrequency                                 ,
																		@InvoiceSubmissionMethod                          ,
																		@PaymentTerms                                     ,
																		@AppWageDetermineDavisBaconAct                   ,
																		@AppWageDetermineServiceContractAct              ,
																		@BillingFormula                                   ,
																		@RevenueFormula                                   ,
																		@RevenueRecognitionEACPercent                    ,
																		@OHonsite                                         ,
																		@OHoffsite                                        ,
																		@ProjectCounter                                   ,
																		@CreatedOn                                        ,
																		@UpdatedOn                                        ,
																		@CreatedBy                                        ,
																		@UpdatedBy                                        ,
																		@IsActive                                         ,
																		@IsDeleted                                        
																)";
            return _context.Connection.Execute(insertQuery, contractModel);
        }

        public int Edit(Core.Entities.Contract contractModel)
        {
            string updateQuery = $@"Update Contract       set 
																	  ParentContractGuid						=           @ParentContractGuid                                                 ,
																	  IsIDIQContract						    =           @IsIDIQContract                                    ,
																	  IsPrimeContract						    =           @IsPrimeContract                                   ,
																	  ContractNumber				            =           @ContractNumber                                    ,
																	  SubContractNumber			                =           @SubContractNumber                                 ,
																	  ORGID						                =           @ORGID                                             ,
																	  ProjectNumber				                =           @ProjectNumber                                     ,
																	  ContractTitle				                =           @ContractTitle                                     ,
																	  Description				                =           @Description                                       ,
																	  CountryOfPerformance			            =           @CountryOfPerformance                              ,
																	  PlaceOfPerformance			            =           @PlaceOfPerformanceSelectedIds                     ,
																	  POPStart						            =           @POPStart                                          ,
																	  POPEnd						            =           @POPEnd                                            ,
																	  NAICSCode					                =           @NAICSCode                                         ,
																	  PSCCode						            =           @PSCCode                                           ,
																	  CPAREligible					            =           @CPAREligible                                      ,
																	  QualityLevelRequirements		            =           @QualityLevelRequirements                          ,
																	  QualityLevel					            =           @QualityLevel                                      ,
																																																		   
																	  CompanyPresident					        =           @CompanyPresident                                       ,
																	  RegionalManager					        =           @RegionalManager                                        ,
																	  ProjectManager					        =           @ProjectManager                                         ,
																	  ProjectControls					        =           @ProjectControls                                        ,
																	  AccountingRepresentative			        =           @AccountingRepresentative                               ,
																	  ContractRepresentative			        =           @ContractRepresentative                                 ,
																																																		   
																	  AwardingAgencyOffice				        =           @AwardingAgencyOffice                            ,	
																	  OfficeContractRepresentative		        =           @OfficeContractRepresentative                   ,	
																	  OfficeContractTechnicalRepresent         =           @OfficeContractTechnicalRepresent               ,
																	  FundingAgencyOffice				        =           @FundingAgencyOffice                             ,
																	  FundingOfficeContractRepresentative		=	        @FundingOfficeContractRepresentative            ,
																	  FundingOfficeContractTechnicalRepresent	=	        @FundingOfficeContractTechnicalRepresent        ,
																															
																	  SetAside						            =           @setAside                                       ,
																	  SelfPerformancePercent			        =           @SelfPerformancePercent                        ,
																	  SBA								        =           @SBA                                            ,
																	  Competition						        =           @Competition                                    ,
																	  ContractType					            =           @ContractType                                   ,
																	  OverHead						            =           @OverHead                                       ,
																	  GAPercent						        =           @GAPercent                                    ,
																	  Fee_Percent						        =           @FeePercent                                    ,
																	  Currency						            =           @Currency                                       ,
																	  BlueSkyAwardAmount				        =           @BlueSkyAwardAmount                            ,
																	  AwardAmount						        =           @AwardAmount                                    ,
																	  FundingAmount					            =           @FundingAmount                                  ,
																	  BillingAddress					        =           @BillingAddress                                 ,
																	  BillingFrequency				            =           @BillingFrequency                               ,
																	  InvoiceSubmissionMethod			        =           @InvoiceSubmissionMethod                        ,
																	  PaymentTerms					            =           @PaymentTerms                                   ,
																	  AppWageDetermineDavisBaconAct			=	        @AppWageDetermineDavisBaconAct                 ,
																	  AppWageDetermineServiceContractAct		=		    @AppWageDetermineServiceContractAct            ,
																	  BillingFormula					        =           @BillingFormula                                 ,
																	  RevenueFormula					        =           @RevenueFormula                                 ,
																	  RevenueRecognitionEACPercent	            =           @RevenueRecognitionEACPercent                  ,
																	  OHonsite						            =           @OHonsite                                       ,
																	  OHoffsite						            =           @OHoffsite                                      ,
																	  UpdatedOn						            =           @UpdatedOn                                                           ,
																	  UpdatedBy						            =           @UpdatedBy                                                           ,
																	  IsActive						            =           @IsActive                                                            ,
																	  IsDeleted                                 =           @IsDeleted                                                           
																	  where ContractGuid = @ContractGuid ";
            return _context.Connection.Execute(updateQuery, contractModel);
        }

        public int Delete(Guid[] ids)
        {
            foreach (var contractGuid in ids)
            {
                var contract = new
                {
                    ContractGuid = contractGuid
                };
                string disableQuery = @"Update Contract set 
											   IsDeleted   = 1
											   where (ContractGuid = @ContractGuid or ParentContractGuid = @ContractGuid)";
                _context.Connection.Execute(disableQuery, contract);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int Disable(Guid[] ids)
        {
            foreach (var contractGuid in ids)
            {
                var contract = new
                {
                    ContractGuid = contractGuid
                };
                string disableQuery = @"Update Contract set 
											IsActive   = 0
											where ContractGuid = @ContractGuid ";
                _context.Connection.Execute(disableQuery, contract);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int Enable(Guid[] ids)
        {
            foreach (var contractGuid in ids)
            {
                var contract = new
                {
                    ContractGuid = contractGuid
                };
                string disableQuery = @"Update Contract set 
											IsActive   = 1
											where ContractGuid = @ContractGuid ";
                _context.Connection.Execute(disableQuery, contract);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }

        public ICollection<Organization> GetOrganizationData(string searchText)
        {
            if (searchText != "")
            {
                var organizationDataQuery = string.Format($@"
				select * from OrgID
				where Name like '%{@searchText}%' or Description like '%{@searchText}%' Order by Name ");
                var organizationData = _context.Connection.Query<Organization>(organizationDataQuery).ToList();
                return organizationData;
            }
            else
            {
                var organizationDataQuery = string.Format($@"
				select * from OrgID
				Order by Name ");
                var organizationData = _context.Connection.Query<Organization>(organizationDataQuery).ToList();
                return organizationData;
            }

        }

        public Organization GetOrganizationByOrgId(Guid orgId)
        {
            var organizationDataQuery = string.Format($@"
				select * from OrgID where orgIdGuid = @orgId");
            var organizationData = _context.Connection.QueryFirstOrDefault<Organization>(organizationDataQuery, new { orgId = orgId });
            return organizationData;
        }

        public ICollection<Naics> GetNaicsCodeData(string searchText)
        {
            var nAicsCodeQuery = string.Format($@"
			select * from NAICS
				where code like '%{@searchText}%' or title like '%{@searchText}%' ");
            var nAicsCode = _context.Connection.Query<Naics>(nAicsCodeQuery).ToList();
            return nAicsCode;
        }

        public ICollection<Psc> GetPscCodeData(string searchText)
        {
            var pScCodeQuery = string.Format($@"
			select * from PSC
				where code like '%{@searchText}%' or CodeDescription like '%{@searchText}%' ");
            var pScSCode = _context.Connection.Query<Psc>(pScCodeQuery).ToList();
            return pScSCode;
        }

        public AssociateUserList GetCompanyRegionOfficeNameByCode(EntityCode entityCode)
        {
            AssociateUserList userList = new AssociateUserList();
            var companyPresidentQuery = string.Format($@"select top 1 Users.Displayname , Users.UserGuid , Users.JobTitle from Company left join Users on Company.President = Users.UserGuid where Company.CompanyCode = @CompanyCode");
            var operationManagerQuery = string.Format($@"select top 1 Users.Displayname , Users.UserGuid , Users.JobTitle from Office left join Users on Office.OperationManagerGuid = Users.UserGuid where Office.OfficeCode = @OfficeCode");
            var regionalManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._regionalManager}'");
            var deputyManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._deputyregionalManager}'");
            var HSManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._hsregionalManager}'");
            var BDManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._bdregionalManager}'");
            var companyNameQuery = string.Format($@"select top 1 Company.CompanyName from Company where CompanyCode = @CompanyCode and IsDeleted = 0");
            var regionNameQuery = string.Format($@"select top 1 Region.RegionName from Region where RegionCode = @RegionCode and IsDeleted = 0");
            var officeNameQuery = string.Format($@"select top 1 Office.OfficeName from Office where OfficeCode = @OfficeCode and IsDeleted = 0");

            //QueryFirst changed to QueryFirstOrDefault as data may not be available to match with column
            userList.CompanyPresident = _context.Connection.QueryFirstOrDefault<User>(companyPresidentQuery, new { CompanyCode = entityCode.CompanyCode });
            userList.OperationManager = _context.Connection.QueryFirstOrDefault<User>(operationManagerQuery, new { OfficeCode = entityCode.OfficeCode });
            userList.RegionManager = _context.Connection.QueryFirstOrDefault<User>(regionalManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.DeputyRegionManager = _context.Connection.QueryFirstOrDefault<User>(deputyManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.HealthAndSafetyRegionManager = _context.Connection.QueryFirstOrDefault<User>(HSManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.BusinessDevelopmentRegionManager = _context.Connection.QueryFirstOrDefault<User>(BDManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.CompanyName = _context.Connection.QueryFirstOrDefault<string>(companyNameQuery, new { CompanyCode = entityCode.CompanyCode }) ?? "";
            userList.RegionName = _context.Connection.QueryFirstOrDefault<string>(regionNameQuery, new { RegionCode = entityCode.RegionCode }) ?? "";
            userList.OfficeName = _context.Connection.QueryFirstOrDefault<string>(officeNameQuery, new { OfficeCode = entityCode.OfficeCode }) ?? "";
            return userList;
        }

        public ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType)
        {
            var model = new List<KeyValuePairWithDescriptionModel<Guid, string, string>>();
            var sql =
                $@"SELECT CustomerContact.* FROM CustomerContact 
				join CustomerContactType on CustomerContact.ContactTypeGuid = CustomerContactType.ContactTypeGuid
				where CustomerContactType.ContactType = @ContactType and CustomerGuid = @CustomerId order by FirstName asc";
            var data = _context.Connection.Query<CustomerContact>(sql, new { ContactType = contactType, CustomerId = customerId });
            foreach (var item in data)
            {
                var fullName = FormatHelper.FormatFullName(item.FirstName, item.MiddleName, item.LastName);
                var values = FormatHelper.FormatAutoCompleteData(fullName, item.PhoneNumber);
                model.Add(new KeyValuePairWithDescriptionModel<Guid, string, string> { Keys = item.ContactGuid, Values = values, Descriptions = "" });
            }
            return model;
        }

        public Core.Entities.Contract GetDetailById(Guid id)
        {
            Core.Entities.Contract contractEntity = new Core.Entities.Contract();
            string companyCodeSql = $"select SUBSTRING(OrgID.Name,1,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var companyCode = _context.Connection.Query<string>(companyCodeSql, new { ContractGuid = id }).FirstOrDefault();

            string companyGuidSql = $"select Company.President from Company where CompanyCode = @CompanyCode";
            var companyGuid = _context.Connection.Query<Guid>(companyGuidSql, new { CompanyCode = companyCode }).FirstOrDefault();

            string companyPresidentSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = @CompanyGuid";
            var companyPresident = _context.Connection.Query<string>(companyPresidentSql, new { CompanyGuid = companyGuid }).FirstOrDefault();

            string companyNameSql = $"select Company.CompanyName from Company where CompanyCode = @CompanyCode and IsDeleted = 0";
            var companyName = _context.Connection.Query<string>(companyNameSql, new { CompanyCode = companyCode }).FirstOrDefault();

            string regionCodeSql = $@"select SUBSTRING(OrgID.Name,4,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var regionCode = _context.Connection.Query<string>(regionCodeSql, new { ContractGuid = id }).FirstOrDefault();

            string regionGuidSql = $@"select Region.RegionalManager from Region where RegionCode = @RegionCode";
            var regionGuid = _context.Connection.Query<Guid>(regionGuidSql, new { RegionCode = regionCode }).FirstOrDefault();

            string regionalManagerSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = @RegionGuid";
            var regionalManager = _context.Connection.Query<string>(regionalManagerSql, new { RegionGuid = regionGuid }).FirstOrDefault();

            string regionNameSql = $"select Region.RegionName from Region where RegionCode = @RegionCode and IsDeleted = 0";
            var regionName = _context.Connection.Query<string>(regionNameSql, new { RegionCode = regionCode }).FirstOrDefault();

            string officeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var officeCode = _context.Connection.Query<string>(officeCodeSql, new { RegionCode = regionCode, ContractGuid = id }).FirstOrDefault();

            string officeNameSql = $"select Office.OfficeName from Office where OfficeCode = @OfficeCode and IsDeleted = 0";
            var officeName = _context.Connection.Query<string>(officeNameSql, new { OfficeCode = officeCode }).FirstOrDefault();

            string questionary = $" select questionaire.ContractQuestionaireGuid,users.Displayname,questionaire.UpdatedOn from ContractQuestionaire questionaire " +
                   $"left join Users users on users.UserGuid = questionaire.UpdatedBy where questionaire.ContractGuid = @ContractGuid and Isdeleted = 0";
            var contractQuestionaire = _context.Connection.Query<ContractQuestionaire>(questionary, new { ContractGuid = id }).FirstOrDefault();

            string billingrates = $"select * from EmployeeBillingRates where ContractGuid = @ContractGuid and Isdeleted = 0";
            var employeeeBillingRates = _context.Connection.Query<EmployeeBillingRates>(billingrates, new { ContractGuid = id }).FirstOrDefault();

            string contractWBSSql = $"select * from ContractWBS where ContractGuid = @ContractGuid and Isdeleted = 0";
            var contractWBS = _context.Connection.Query<ContractWBS>(contractWBSSql, new { ContractGuid = id }).FirstOrDefault();

            string categoryrates = $"select * from LaborCategoryRates where ContractGuid = @ContractGuid and Isdeleted = 0";
            var laborCategoryRates = _context.Connection.Query<LaborCategoryRates>(categoryrates, new { ContractGuid = id }).FirstOrDefault();

            string revenue = $"select * from RevenueRecognization where ContractGuid = @ContractGuid and Isdeleted = 0";
            var revenueRecognitionModel = _context.Connection.Query<Northwind.Core.Entities.RevenueRecognition>(revenue, new { ContractGuid = id }).FirstOrDefault();

            string requestForm = $"select * from JobRequest where ContractGuid = @ContractGuid and Isdeleted = 0";
            var requestFormModel = _context.Connection.Query<Core.Entities.JobRequest>(requestForm, new { ContractGuid = id }).FirstOrDefault();

            string basicContractSql = @"select
							Contract.IsPrimeContract, 
							Contract.IsIDIQContract, 
							Contract.ProjectNumber, 
							Contract.ContractNumber, 
							Contract.SubContractNumber, 
							(OrgID.Name + ' ' + OrgID.Title) OrganizationName,
							Contract.ORGID,
							Contract.ContractTitle, 
							Country.CountryId CountryOfPerformance,
							Country.CountryName CountryOfPerformanceSelected,
							--State.StateName PlaceOfPerformanceSelected,
							Contract.PlaceOfPerformance PlaceOfPerformanceSelectedIds,
							Contract.POPStart, 
							Contract.POPEnd, 
							(NAICS.Code +' '+ NAICS.Title) NAICSCodeName, 
							Contract.NAICSCode,
							PSC.CodeDescription PSCCodeName,
							Contract.PSCCode,
							Contract.CPAREligible, 
							Contract.QualityLevelRequirements, 
							QualityLevel.Name QualityLevelName,
							Contract.QualityLevel,
							Contract.Description

							from Contract

							left join
							OrgID on Contract.ORGID = OrgID.OrgIDGuid
							left join
							Country on Contract.CountryOfPerformance = Country.CountryId
							left join
							State on Contract.PlaceOfPerformance = State.StateId
							left join
							NAICS on Contract.NAICSCode = NAICS.NAICSGuid
							left join
							PSC on Contract.PSCCode = PSC.PSCGuid
							left join
							ResourceAttributeValue QualityLevel on Contract.QualityLevel = QualityLevel.Value
				
							WHERE ContractGuid =  @ContractGuid;";
            var basicContractInfo = _context.Connection.QuerySingle<BasicContractInfoModel>(basicContractSql, new { ContractGuid = id });

            string keyPersonnelSql = @"select
							(ProjectManager.Displayname+' (' +ProjectManager.JobTitle +')') ProjectManagerName, 
							Contract.CompanyPresident,
							Contract.RegionalManager,
							Contract.ProjectManager,
							(ProjectControl.Displayname  +' (' +ProjectControl.JobTitle +')')ProjectControlName,
							Contract.ProjectControls,
							(AccountingRepresentative.Displayname  +' (' +AccountingRepresentative.JobTitle +')') AccountingRepresentativeName,
							Contract.AccountingRepresentative,
							(ContractRepresentative.Displayname +' (' +ContractRepresentative.JobTitle +')') ContractRepresentativeName,
							Contract.ContractRepresentative

							from Contract

							left join
							Users ProjectManager on Contract.ProjectManager = ProjectManager.UserGuid
							left join
							Users ProjectControl on Contract.ProjectControls = ProjectControl.UserGuid
							left join
							Users AccountingRepresentative on Contract.AccountingRepresentative = AccountingRepresentative.UserGuid
							left join	
							Users ContractRepresentative on Contract.ContractRepresentative = ContractRepresentative.UserGuid

							WHERE ContractGuid =  @ContractGuid;";
            var keyPersonnel = _context.Connection.QuerySingle<KeyPersonnelModel>(keyPersonnelSql, new { ContractGuid = id });

            string customerInfoSql = @"select
							AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							OfficeContractRepresentative.ContactGuid OfficeContractRepresentative,
							(OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) OfficeContractRepresentativeName,
							OfficeContractTechnicalRepresent.ContactGuid OfficeContractTechnicalRepresent,
							(OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) OfficeContractTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
							FundingOfficeContractRepresentative.ContactGuid FundingOfficeContractRepresentative,
							(FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingOfficeContractRepresentativeName,
							FundingOfficeContractTechnicalRepresent.ContactGuid FundingOfficeContractTechnicalRepresent,
							(FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingOfficeContractTechnicalRepresentName
							
							from Contract

							left join
							Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
							left join
							CustomerContact OfficeContractRepresentative on Contract.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
							left join
							CustomerContact OfficeContractTechnicalRepresent on  Contract.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid

							left join
							Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
							left join
							CustomerContact FundingOfficeContractRepresentative on Contract.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
							left join
							CustomerContact FundingOfficeContractTechnicalRepresent on Contract.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid

							WHERE ContractGuid =  @ContractGuid;";
            var customerInformation = _context.Connection.QuerySingle<CustomerInformationModel>(customerInfoSql, new { ContractGuid = id });

            string financialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformancePercent,
							Contract.SBA,
							Competition.Name CompetitionType,
							Contract.Competition,
							ContractType.Name ContractTypeName,
							Contract.ContractType,
							Contract.OverHead,
							Contract.GAPercent,
							Contract.FeePercent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAwardAmount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							AppWageDetermineDavisBaconAct.Name AppWageDetermineavisBaconActType,
							Contract.AppWageDetermineDavisBaconAct,
							AppWageDetermineServiceContractAct.Name AppWageDetermineServiceContractActType,
							Contract.AppWageDetermineServiceContractAct,
							Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEACPercent,
							Contract.OHonsite,
							Contract.OHoffsite

							from Contract
					   
							left join
							ResourceAttributeValue SetAside on Contract.SetAside = SetAside.Value
							left join
							ResourceAttributeValue Competition on Contract.Competition = Competition.Value
							left join
							ResourceAttributeValue ContractType on Contract.ContractType = ContractType.Value
							left join
							ResourceAttributeValue Currency on Contract.Currency = Currency.Value
							left join
							ResourceAttributeValue BillingFrequency on Contract.BillingFrequency = BillingFrequency.Value
							left join
							ResourceAttributeValue InvoiceSubmissionMethod on Contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
							left join
							ResourceAttributeValue PaymentTerms on Contract.PaymentTerms = PaymentTerms.Value
							left join
							ResourceAttributeValue AppWageDetermineDavisBaconAct on Contract.AppWageDetermineDavisBaconAct = AppWageDetermineDavisBaconAct.Value
							left join
							ResourceAttributeValue AppWageDetermineServiceContractAct on Contract.AppWageDetermineServiceContractAct = AppWageDetermineServiceContractAct.Value
 
							WHERE ContractGuid =  @ContractGuid;";
            var financialInformation = _context.Connection.QuerySingle<FinancialInformationModel>(financialInfoSql, new { ContractGuid = id });

            string baseModelSql = @"select Contract.IsActive from Contract WHERE ContractGuid =  @ContractGuid;";
            var baseModel = _context.Connection.QuerySingle<BaseModel>(baseModelSql, new { ContractGuid = id });

            // to fetch States name through state id array..
            if (!string.IsNullOrEmpty(basicContractInfo.PlaceOfPerformanceSelectedIds))
            {
                var stateIdArr = basicContractInfo.PlaceOfPerformanceSelectedIds.Split(',');
                foreach (var item in stateIdArr)
                {
                    var stateQuery = $"select StateName from State where StateId =@StateId";
                    var stateNameArr = _context.Connection.Query<string>(stateQuery, new { StateId = item });
                    basicContractInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);
                }
            }

            string parent_ContractGuidQuery = $"select parentContractGuid from contract where contractGuid = @ContractGuid";
            contractEntity.ParentContractGuid = _context.Connection.QuerySingle<Guid?>(parent_ContractGuidQuery, new { ContractGuid = id });

            contractEntity.BasicContractInfo = basicContractInfo;
            contractEntity.BasicContractInfo.CompanyName = companyName;
            contractEntity.BasicContractInfo.RegionName = regionName;
            contractEntity.BasicContractInfo.OfficeName = officeName;
            contractEntity.KeyPersonnel = keyPersonnel;
            contractEntity.KeyPersonnel.CompanyPresidentName = companyPresident;
            contractEntity.KeyPersonnel.RegionalManagerName = regionalManager;
            contractEntity.CustomerInformation = customerInformation;
            contractEntity.ContractGuid = id;
            contractEntity.FinancialInformation = financialInformation;
            contractEntity.IsActive = new BaseModel().IsActive;
            contractEntity.ContractQuestionaire = contractQuestionaire;
            contractEntity.EmployeeBillingRates = employeeeBillingRates;
            contractEntity.ContractWBS = contractWBS;
            contractEntity.LaborCategoryRates = laborCategoryRates;
            contractEntity.JobRequest = requestFormModel;
            contractEntity.RevenueRecognition = revenueRecognitionModel;

            return contractEntity;
        }

        public Core.Entities.Contract GetDetailsForProjectByContractId(Guid id)
        {
            Core.Entities.Contract contractEntity = new Core.Entities.Contract();
            string companyCodeSql = $"select SUBSTRING(OrgID.Name,1,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var companyCode = _context.Connection.Query<string>(companyCodeSql, new { ContractGuid = id }).FirstOrDefault();

            string companyGuidSql = $"select Company.President from Company where CompanyCode = @CompanyCode";
            var companyGuid = _context.Connection.Query<Guid>(companyGuidSql, new { CompanyCode = companyCode }).FirstOrDefault();

            string companyPresidentSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = @CompanyGuid";
            var companyPresident = _context.Connection.Query<string>(companyPresidentSql, new { CompanyGuid = companyGuid }).FirstOrDefault();

            string companyNameSql = $"select Company.CompanyName from Company where CompanyCode = @CompanyCode and IsDeleted = 0";
            var companyName = _context.Connection.Query<string>(companyNameSql, new { CompanyCode = companyCode }).FirstOrDefault();

            string regionCodeSql = $@"select SUBSTRING(OrgID.Name,4,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var regionCode = _context.Connection.Query<string>(regionCodeSql, new { ContractGuid = id }).FirstOrDefault();

            string regionGuidSql = $@"select Region.RegionalManager from Region where RegionCode = @RegionCode";
            var regionGuid = _context.Connection.Query<Guid>(regionGuidSql, new { RegionCode = regionCode }).FirstOrDefault();

            string regionalManagerSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = @RegionGuid";
            var regionalManager = _context.Connection.Query<string>(regionalManagerSql, new { RegionGuid = regionGuid }).FirstOrDefault();

            string regionNameSql = $"select Region.RegionName from Region where RegionCode = @RegionCode and IsDeleted = 0";
            var regionName = _context.Connection.Query<string>(regionNameSql, new { RegionCode = regionCode }).FirstOrDefault();

            string officeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var officeCode = _context.Connection.Query<string>(officeCodeSql, new { RegionCode = regionCode }).FirstOrDefault();

            string officeNameSql = $"select Office.OfficeName from Office where OfficeCode = @OfficeCode and IsDeleted = 0";
            var officeName = _context.Connection.Query<string>(officeNameSql, new { OfficeCode = officeCode }).FirstOrDefault();

            string contractNumber = $"select ContractNumber from Contract where ContractGuid =  @ContractGuid";
            var getContractNumber = _context.Connection.Query<string>(contractNumber, new { ContractGuid = id }).FirstOrDefault();

            string basicContractSql = @"select
							Contract.IsPrimeContract, 
							Contract.IsIDIQContract, 
							Contract.ProjectNumber, 
							Contract.ContractNumber, 
							Contract.SubContractNumber, 
							(OrgID.Name + ' ' + OrgID.Title) OrganizationName,
							Contract.ORGID,
							Contract.ContractTitle, 
							Country.CountryId CountryOfPerformance,
							Country.CountryName CountryOfPerformanceSelected,
							--State.StateName PlaceOfPerformanceSelected,
							Contract.PlaceOfPerformance PlaceOfPerformanceSelectedIds,
							Contract.POPStart, 
							Contract.POPEnd, 
							(NAICS.Code +' '+ NAICS.Title) NAICSCodeName, 
							Contract.NAICSCode,
							PSC.CodeDescription PSCCodeName,
							Contract.PSCCode,
							Contract.CPAREligible, 
							Contract.QualityLevelRequirements, 
							QualityLevel.Name QualityLevelName,
							Contract.QualityLevel,
							Contract.Description

							from Contract

							left join
							OrgID on Contract.ORGID = OrgID.OrgIDGuid
							left join
							Country on Contract.CountryOfPerformance = Country.CountryId
							left join
							State on Contract.PlaceOfPerformance = State.StateId
							left join
							NAICS on Contract.NAICSCode = NAICS.NAICSGuid
							left join
							PSC on Contract.PSCCode = PSC.PSCGuid
							left join
							ResourceAttributeValue QualityLevel on Contract.QualityLevel = QualityLevel.Value
				
							WHERE ContractGuid =  @ContractGuid;";
            var basicContractInfo = _context.Connection.QuerySingle<BasicContractInfoModel>(basicContractSql, new { ContractGuid = id });

            string keyPersonnelSql = @"select
							(ProjectManager.Displayname+' (' +ProjectManager.JobTitle +')') ProjectManagerName, 
							Contract.CompanyPresident,
							Contract.RegionalManager,
							Contract.ProjectManager,
							(ProjectControl.Displayname  +' (' +ProjectControl.JobTitle +')')ProjectControlName,
							Contract.ProjectControls,
							(AccountingRepresentative.Displayname  +' (' +AccountingRepresentative.JobTitle +')') AccountingRepresentativeName,
							Contract.AccountingRepresentative,
							(ContractRepresentative.Displayname +' (' +ContractRepresentative.JobTitle +')') ContractRepresentativeName,
							Contract.ContractRepresentative

							from Contract

							left join
							Users ProjectManager on Contract.ProjectManager = ProjectManager.UserGuid
							left join
							Users ProjectControl on Contract.ProjectControls = ProjectControl.UserGuid
							left join
							Users AccountingRepresentative on Contract.AccountingRepresentative = AccountingRepresentative.UserGuid
							left join	
							Users ContractRepresentative on Contract.ContractRepresentative = ContractRepresentative.UserGuid

							WHERE ContractGuid =  @ContractGuid;";
            var keyPersonnel = _context.Connection.QuerySingle<KeyPersonnelModel>(keyPersonnelSql, new { ContractGuid = id });

            string customerInfoSql = @"select
							AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							OfficeContractRepresentative.ContactGuid OfficeContractRepresentative,
							(OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) OfficeContractRepresentativeName,
							OfficeContractTechnicalRepresent.ContactGuid OfficeContractTechnicalRepresent,
							(OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) OfficeContractTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
							FundingOfficeContractRepresentative.ContactGuid FundingOfficeContractRepresentative,
							(FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingOfficeContractRepresentativeName,
							FundingOfficeContractTechnicalRepresent.ContactGuid FundingOfficeContractTechnicalRepresent,
							(FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingOfficeContractTechnicalRepresentName
							
							from Contract

							left join
							Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
							left join
							CustomerContact OfficeContractRepresentative on Contract.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
							left join
							CustomerContact OfficeContractTechnicalRepresent on  Contract.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid

							left join
							Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
							left join
							CustomerContact FundingOfficeContractRepresentative on Contract.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
							left join
							CustomerContact FundingOfficeContractTechnicalRepresent on Contract.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid

							WHERE ContractGuid =  @ContractGuid;";
            var customerInformation = _context.Connection.QuerySingle<CustomerInformationModel>(customerInfoSql, new { ContractGuid = id });

            string financialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformancePercent,
							Contract.SBA,
							Competition.Name CompetitionType,
							Contract.Competition,
							ContractType.Name ContractTypeName,
							Contract.ContractType,
							Contract.OverHead,
							Contract.GAPercent,
							Contract.FeePercent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAwardAmount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							AppWageDetermineDavisBaconAct.Name AppWageDetermineDavisBaconActType,
							Contract.AppWageDetermineDavisBaconAct,
							AppWageDetermineServiceContractAct.Name AppWageDetermineServiceContractActType,
							Contract.AppWageDetermineServiceContractAct,
							Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEACPercent,
							Contract.OHonsite,
							Contract.OHoffsite

							from Contract
					   
							left join
							ResourceAttributeValue SetAside on Contract.SetAside = SetAside.Value
							left join
							ResourceAttributeValue Competition on Contract.Competition = Competition.Value
							left join
							ResourceAttributeValue ContractType on Contract.ContractType = ContractType.Value
							left join
							ResourceAttributeValue Currency on Contract.Currency = Currency.Value
							left join
							ResourceAttributeValue BillingFrequency on Contract.BillingFrequency = BillingFrequency.Value
							left join
							ResourceAttributeValue InvoiceSubmissionMethod on Contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
							left join
							ResourceAttributeValue PaymentTerms on Contract.PaymentTerms = PaymentTerms.Value
							left join
							ResourceAttributeValue AppWageDetermineDavisBaconAct on Contract.AppWageDetermineDavisBaconAct = AppWageDetermineDavisBaconAct.Value
							left join
							ResourceAttributeValue AppWageDetermineServiceContractAct on Contract.AppWageDetermineServiceContractAct = AppWageDetermineServiceContractAct.Value
 
							WHERE ContractGuid =  @ContractGuid;";
            var financialInformation = _context.Connection.QuerySingle<FinancialInformationModel>(financialInfoSql, new { ContractGuid = id });

            string baseModelSql = @"select Contract.IsActive from Contract WHERE ContractGuid =  @ContractGuid;";
            var baseModel = _context.Connection.QuerySingle<BaseModel>(baseModelSql, new { ContractGuid = id });

            // to fetch States name through state id array..
            var stateIdArr = basicContractInfo.PlaceOfPerformanceSelectedIds.Split(',');
            var stateIdArrWithStringCote = stateIdArr.Select(x => string.Format("'" + x + "'"));
            var formatQuery = string.Join(",", stateIdArrWithStringCote);
            var stateQuery = $"select StateName from State where StateId in (@formatQuery)";
            var stateNameArr = _context.Connection.Query<string>(stateQuery, new { formatQuery = formatQuery }); ;
            basicContractInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);


            contractEntity.BasicContractInfo = basicContractInfo;
            contractEntity.BasicContractInfo.CompanyName = companyName;
            contractEntity.BasicContractInfo.RegionName = regionName;
            contractEntity.BasicContractInfo.OfficeName = officeName;
            contractEntity.ContractNumber = getContractNumber;
            contractEntity.KeyPersonnel = keyPersonnel;
            contractEntity.KeyPersonnel.CompanyPresidentName = companyPresident;
            contractEntity.KeyPersonnel.RegionalManagerName = regionalManager;
            contractEntity.CustomerInformation = customerInformation;
            contractEntity.ContractGuid = id;
            contractEntity.FinancialInformation = financialInformation;
            contractEntity.IsActive = new BaseModel().IsActive;
            contractEntity.CreatedOn = new BaseModel().CreatedOn;

            return contractEntity;
        }

        public User GetUsersDataByUserId(Guid id)
        {
            var usersQuery = string.Format($@"
			   select * from Users
				where userGuid = @UserGuid 
				order by Displayname asc");
            var usersData = _context.Connection.QuerySingle<User>(usersQuery, new { UserGuid = id });
            return usersData;
        }

        public CustomerContact GetContactsDataByContactId(Guid id)
        {
            var contactQuery = string.Format($@"
			   select * from CustomerContact
				where ContactGuid = @ContactGuid 
				order by Firstname asc");
            var contactData = _context.Connection.QuerySingle<CustomerContact>(contactQuery, new { ContactGuid = id });
            return contactData;
        }

        public Customer GetCustomersDataByCustomerId(Guid id)
        {
            var customerQuery = string.Format($@"
			   select * from Customer
				where CustomerGuid = @CustomerGuid 
				order by CustomerName asc");
            var customerData = _context.Connection.QuerySingle<Customer>(customerQuery, new { CustomerGuid = id });
            return customerData;
        }

        public Core.Entities.Contract GetInfoById(Guid id)
        {
            Core.Entities.Contract contractEntity = new Core.Entities.Contract();
            string companyCodeSql = $"select SUBSTRING(OrgID.Name,1,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var companyCode = _context.Connection.Query<string>(companyCodeSql, new { ContractGuid = id }).FirstOrDefault();

            string companyGuidSql = $"select Company.President from Company where CompanyCode = @CompanyCode";
            var companyGuid = _context.Connection.Query<Guid>(companyGuidSql, new { CompanyCode = companyCode }).FirstOrDefault();

            string companyPresidentSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = @CompanyGuid";
            var companyPresident = _context.Connection.Query<string>(companyPresidentSql, new { CompanyGuid = companyGuid }).FirstOrDefault();

            string companyNameSql = $"select Company.CompanyName from Company where CompanyCode = @CompanyCode and IsDeleted = 0";
            var companyName = _context.Connection.Query<string>(companyNameSql, new { CompanyCode = companyCode }).FirstOrDefault();

            string regionCodeSql = $@"select SUBSTRING(OrgID.Name,4,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var regionCode = _context.Connection.Query<string>(regionCodeSql, new { ContractGuid = id }).FirstOrDefault();

            string regionGuidSql = $@"select Region.RegionalManager from Region where RegionCode = @RegionCode";
            var regionGuid = _context.Connection.Query<Guid>(regionGuidSql, new { RegionCode = regionCode }).FirstOrDefault();

            string regionalManagerSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = @RegionGuid";
            var regionalManager = _context.Connection.Query<string>(regionalManagerSql, new { RegionGuid = regionGuid }).FirstOrDefault();

            string regionNameSql = $"select Region.RegionName from Region where RegionCode = @RegionCode and IsDeleted = 0";
            var regionName = _context.Connection.Query<string>(regionNameSql, new { RegionCode = regionCode }).FirstOrDefault();

            string officeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = @ContractGuid;";
            var officeCode = _context.Connection.Query<string>(officeCodeSql, new { RegionCode = regionCode, ContractGuid = id }).FirstOrDefault();

            string officeNameSql = $"select Office.OfficeName from Office where OfficeCode = @OfficeCode and IsDeleted = 0";
            var officeName = _context.Connection.Query<string>(officeNameSql, new { OfficeCode = officeCode }).FirstOrDefault();

            string questionary = $" select questionaire.ContractQuestionaireGuid,users.Displayname,questionaire.UpdatedOn from ContractQuestionaire questionaire " +
                   $"left join Users users on users.UserGuid = questionaire.UpdatedBy where questionaire.ContractGuid = @ContractGuid and Isdeleted = 0";
            var contractQuestionaire = _context.Connection.Query<ContractQuestionaire>(questionary, new { ContractGuid = id }).FirstOrDefault();

            string billingrates = $"select * from EmployeeBillingRates where ContractGuid = @ContractGuid and Isdeleted = 0";
            var employeeeBillingRates = _context.Connection.Query<EmployeeBillingRates>(billingrates, new { ContractGuid = id }).FirstOrDefault();

            string contractWBSSql = $"select * from ContractWBS where ContractGuid = @ContractGuid and Isdeleted = 0";
            var contractWBS = _context.Connection.Query<ContractWBS>(contractWBSSql, new { ContractGuid = id }).FirstOrDefault();

            string categoryrates = $"select * from LaborCategoryRates where ContractGuid = @ContractGuid and Isdeleted = 0";
            var laborCategoryRates = _context.Connection.Query<LaborCategoryRates>(categoryrates, new { ContractGuid = id }).FirstOrDefault();

            string revenue = $"select * from RevenueRecognization where ContractGuid = @ContractGuid and Isdeleted = 0";
            var revenueRecognitionModel = _context.Connection.Query<Northwind.Core.Entities.RevenueRecognition>(revenue, new { ContractGuid = id }).FirstOrDefault();

            string requestForm = $"select * from JobRequest where ContractGuid = @ContractGuid and Isdeleted = 0";
            var requestFormModel = _context.Connection.Query<Core.Entities.JobRequest>(requestForm, new { ContractGuid = id }).FirstOrDefault();

            string basicContractSql = @"select
							Contract.IsPrimeContract, 
							Contract.IsIDIQContract, 
							Contract.ProjectNumber, 
							Contract.ContractNumber, 
							Contract.SubContractNumber, 
							(OrgID.Name + ' ' + OrgID.Title) OrganizationName,
							Contract.ORGID,
							Contract.ContractTitle, 
							Country.CountryId CountryOfPerformance,
							Country.CountryName CountryOfPerformanceSelected,
							--State.StateName PlaceOfPerformanceSelected,
							Contract.PlaceOfPerformance PlaceOfPerformanceSelectedIds,
							Contract.POPStart, 
							Contract.POPEnd, 
							(NAICS.Code +' '+ NAICS.Title) NAICSCodeName, 
							Contract.NAICSCode,
							PSC.CodeDescription PSCCodeName,
							Contract.PSCCode,
							Contract.CPAREligible, 
							Contract.QualityLevelRequirements, 
							QualityLevel.Name QualityLevelName,
							Contract.QualityLevel,
							Contract.Description

							from Contract

							left join
							OrgID on Contract.ORGID = OrgID.OrgIDGuid
							left join
							Country on Contract.CountryOfPerformance = Country.CountryId
							left join
							State on Contract.PlaceOfPerformance = State.StateId
							left join
							NAICS on Contract.NAICSCode = NAICS.NAICSGuid
							left join
							PSC on Contract.PSCCode = PSC.PSCGuid
							left join
							ResourceAttributeValue QualityLevel on Contract.QualityLevel = QualityLevel.Value
				
							WHERE ContractGuid =  @ContractGuid;";
            var basicContractInfo = _context.Connection.QuerySingle<BasicContractInfoModel>(basicContractSql, new { ContractGuid = id });

            string keyPersonnelSql = @"select
							(ProjectManager.Displayname+' (' +ProjectManager.JobTitle +')') ProjectManagerName, 
							Contract.CompanyPresident,
							Contract.RegionalManager,
							Contract.ProjectManager,
							(ProjectControl.Displayname  +' (' +ProjectControl.JobTitle +')')ProjectControlName,
							Contract.ProjectControls,
							(AccountingRepresentative.Displayname  +' (' +AccountingRepresentative.JobTitle +')') AccountingRepresentativeName,
							Contract.AccountingRepresentative,
							(ContractRepresentative.Displayname +' (' +ContractRepresentative.JobTitle +')') ContractRepresentativeName,
							Contract.ContractRepresentative

							from Contract

							left join
							Users ProjectManager on Contract.ProjectManager = ProjectManager.UserGuid
							left join
							Users ProjectControl on Contract.ProjectControls = ProjectControl.UserGuid
							left join
							Users AccountingRepresentative on Contract.AccountingRepresentative = AccountingRepresentative.UserGuid
							left join	
							Users ContractRepresentative on Contract.ContractRepresentative = ContractRepresentative.UserGuid

							WHERE ContractGuid =  @ContractGuid;";
            var keyPersonnel = _context.Connection.QuerySingle<KeyPersonnelModel>(keyPersonnelSql, new { ContractGuid = id });

            string customerInfoSql = @"select
							AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							OfficeContractRepresentative.ContactGuid OfficeContractRepresentative,
							(OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) OfficeContractRepresentativeName,
							OfficeContractTechnicalRepresent.ContactGuid OfficeContractTechnicalRepresent,
							(OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) OfficeContractTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
							FundingOfficeContractRepresentative.ContactGuid FundingOfficeContractRepresentative,
							(FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingOfficeContractRepresentativeName,
							FundingOfficeContractTechnicalRepresent.ContactGuid FundingOfficeContractTechnicalRepresent,
							(FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingOfficeContractTechnicalRepresentName
							
							from Contract

							left join
							Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
							left join
							CustomerContact OfficeContractRepresentative on Contract.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
							left join
							CustomerContact OfficeContractTechnicalRepresent on  Contract.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid

							left join
							Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
							left join
							CustomerContact FundingOfficeContractRepresentative on Contract.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
							left join
							CustomerContact FundingOfficeContractTechnicalRepresent on Contract.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid

							WHERE ContractGuid =  @ContractGuid;";
            var customerInformation = _context.Connection.QuerySingle<CustomerInformationModel>(customerInfoSql, new { ContractGuid = id });

            string financialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformancePercent,
							Contract.SBA,
							Competition.Name CompetitionType,
							Contract.Competition,
							ContractType.Name ContractTypeName,
							Contract.ContractType,
							Contract.OverHead,
							Contract.GAPercent,
							Contract.FeePercent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAwardAmount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							AppWageDetermineDavisBaconAct.Name AppWageDetermineDavisBaconActType,
							Contract.AppWageDetermineDavisBaconAct,
							AppWageDetermineServiceContractAct.Name AppWageDetermineServiceContractActType,
							Contract.AppWageDetermineServiceContractAct,
							Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEACPercent,
							Contract.OHonsite,
							Contract.OHoffsite

							from Contract
					   
							left join
							ResourceAttributeValue SetAside on Contract.SetAside = SetAside.Value
							left join
							ResourceAttributeValue Competition on Contract.Competition = Competition.Value
							left join
							ResourceAttributeValue ContractType on Contract.ContractType = ContractType.Value
							left join
							ResourceAttributeValue Currency on Contract.Currency = Currency.Value
							left join
							ResourceAttributeValue BillingFrequency on Contract.BillingFrequency = BillingFrequency.Value
							left join
							ResourceAttributeValue InvoiceSubmissionMethod on Contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
							left join
							ResourceAttributeValue PaymentTerms on Contract.PaymentTerms = PaymentTerms.Value
							left join
							ResourceAttributeValue AppWageDetermineDavisBaconAct on Contract.AppWageDetermineDavisBaconAct = AppWageDetermineDavisBaconAct.Value
							left join
							ResourceAttributeValue AppWageDetermineServiceContractAct on Contract.AppWageDetermineServiceContractAct = AppWageDetermineServiceContractAct.Value
 
							WHERE ContractGuid =  @ContractGuid;";
            var financialInformation = _context.Connection.QuerySingle<FinancialInformationModel>(financialInfoSql, new { ContractGuid = id });

            string baseModelSql = @"select Contract.IsActive from Contract WHERE ContractGuid =  @ContractGuid;";
            var baseModel = _context.Connection.QuerySingle<BaseModel>(baseModelSql, new { ContractGuid = id });

            // to fetch States name through state id array..
            if (!string.IsNullOrEmpty(basicContractInfo.PlaceOfPerformanceSelectedIds))
            {
                var stateIdArr = basicContractInfo.PlaceOfPerformanceSelectedIds.Split(',');
                foreach (var item in stateIdArr)
                {
                    var stateQuery = $"select StateName from State where StateId =@StateId";
                    var stateNameArr = _context.Connection.Query<string>(stateQuery, new { StateId = item });
                    basicContractInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);
                }
            }

            contractEntity.BasicContractInfo = basicContractInfo;
            contractEntity.BasicContractInfo.CompanyName = companyName;
            contractEntity.BasicContractInfo.RegionName = regionName;
            contractEntity.BasicContractInfo.OfficeName = officeName;
            contractEntity.KeyPersonnel = keyPersonnel;
            contractEntity.KeyPersonnel.CompanyPresidentName = companyPresident;
            contractEntity.KeyPersonnel.CompanyPresident = companyGuid;
            contractEntity.KeyPersonnel.RegionalManagerName = regionalManager;
            contractEntity.KeyPersonnel.RegionalManager = regionGuid;
            contractEntity.CustomerInformation = customerInformation;
            contractEntity.ContractGuid = id;
            contractEntity.FinancialInformation = financialInformation;
            contractEntity.ContractQuestionaire = contractQuestionaire;
            contractEntity.EmployeeBillingRates = employeeeBillingRates;
            contractEntity.LaborCategoryRates = laborCategoryRates;
            contractEntity.RevenueRecognition = revenueRecognitionModel;
            contractEntity.ContractWBS = contractWBS;
            contractEntity.JobRequest = requestFormModel;
            contractEntity.IsActive = baseModel.IsActive;

            return contractEntity;
        }

        public bool IsExistContractNumber(string contractNumber, Guid contractGuid)
        {
            string contactNumberQuery = $@"select contractNumber from  Contract 
											  where  IsDeleted   = 0
												 AND contractNumber = @contractNumber 
												 AND ContractGuid <> @contractGuid ";
            var result = _context.Connection.QueryFirstOrDefault<string>(contactNumberQuery, new { contractNumber = contractNumber, contractGuid = contractGuid });

            return !string.IsNullOrEmpty(result) ? true : false;
        }
        public bool IsExistProjectNumber(string projectNumber, Guid contractGuid)
        {
            string projectNumberQuery = $@"select projectNumber from  Contract 
											  where  IsDeleted   = 0
												 AND projectNumber = @projectNumber 
												 AND ContractGuid <> @contractGuid";
            var result = _context.Connection.QueryFirstOrDefault<string>(projectNumberQuery, new { projectNumber = projectNumber, contractGuid = contractGuid });

            return !string.IsNullOrEmpty(result) ? true : false;
        }
        public bool IsContractTitleValid(string contractTitle, Guid contractGuid)
        {
            var sqlQuery = @"SELECT Count(*)
							FROM Contract
							WHERE ContractTitle = @contractTitle 
							AND ContractGuid != @contractGuid
							AND IsDeleted = 0";
            var result = _context.Connection.QueryFirstOrDefault<int>(sqlQuery, new { contractTitle = contractTitle, contractGuid = contractGuid });
            if (result == 0)
                return false;
            return true;
        }

        public Core.Entities.Contract GetContractByContractGuid(Guid contractGuid)
        {
            string sql = $"SELECT * FROM Contract WHERE ContractGuid = @ContractGuid;";
            var result = _context.Connection.QuerySingle<Core.Entities.Contract>(sql, new { ContractGuid = contractGuid });
            return result;
        }

        public Core.Entities.Contract GetInfoByContractGuid(Guid contractGuid)
        {
            string sql = $"SELECT contractnumber,projectnumber,contracttitle FROM Contract WHERE ContractGuid = @ContractGuid;";
            var result = _context.Connection.QuerySingle<Core.Entities.Contract>(sql, new { ContractGuid = contractGuid });
            return result;
        }

        public IEnumerable<ProjectForList> GetProjectByContractGuid(Guid contractGuid)
        {
            var sql = @"
				SELECT P.[ContractGuid]
					  ,P.[ProjectNumber]
					  ,P.[ORGID]
					  ,OrgId.title OrgName
					  ,P.[ContractTitle]
					  ,P.[CompanyPresident]
					  ,(CompanyPresident.Firstname + ' ' + CompanyPresident.Lastname)  CompanyPresidentName
					  ,P.[RegionalManager]
					  ,(RegionalManager.Firstname + ' ' + RegionalManager.Lastname) RegionalManagerName
					  --,(p.Firstname + ' ' + p.Lastname) ContractName
					  ,P.[ProjectControls]
					  ,(ContractControl.Firstname + ' ' + ContractControl.Lastname) ProjectControlsName
					  ,P.[AccountingRepresentative]
					  ,(AccountingRepresentative.Firstname + ' ' + AccountingRepresentative.Lastname) AccountingRepresentativeName
					  ,P.[ContractRepresentative]
					  ,(ContractRepresentative.Firstname + ' ' + ContractRepresentative.Lastname) ContractRepresentativeName
					  ,P.[CountryOfPerformance]
					  ,Country.CountryName CountryOfPerformanceName
					  ,P.[PlaceOfPerformance]
					  ,P.[POPStart]
					  ,P.[POPEnd]
					  ,P.[NAICSCode]
					  ,NAICS.title NAICSCodeName
					  ,P.[PSCCode]
					  ,PSC.CodeDescription PSCCodeName
					  ,P.[QualityLevelRequirements]
					  ,P.[QualityLevel]
					  ,P.[AwardingAgencyOffice]
					  ,P.[OfficeContractRepresentative]
					  ,P.[OfficeContractTechnicalRepresent]
					  ,P.[FundingAgencyOffice]
					  ,P.[SetAside]
					  ,P.[SelfPerformancePercent]
					  ,P.[SBA]
					  ,P.[Competition]
					  ,P.[ContractType]
					  ,P.[OverHead]
					  ,P.[GAPercent]
					  ,P.[FeePercent]
					  ,P.[Currency]
					  ,P.[BlueSkyAwardAmount]
					  ,P.[AwardAmount]
					  ,P.[FundingAmount]
					  ,P.[BillingAddress]
					  ,P.[BillingFrequency]
					  ,P.[InvoiceSubmissionMethod]
					  ,InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName
					  ,P.[PaymentTerms]
					  ,PaymentTerms.Name PaymentTermsName
					  ,P.[AppWageDetermineDavisBaconAct]
					  ,P.[BillingFormula]
					  ,P.[RevenueFormula]
					  ,P.[RevenueRecognitionEACPercent]
					  ,P.[OHonsite]
					  ,P.[OHoffsite]
					  ,P.[CreatedOn]
					  ,P.[UpdatedOn]
					  ,P.[CreatedBy]
					  ,P.[UpdatedBy]
					  ,P.[IsActive]
					  ,P.[IsDeleted]
					  ,P.[Description]
					  ,P.[AppWageDetermineServiceContractAct]
					  ,P.[FundingOfficeContractRepresentative]
					  ,'' FundingOfficeContractRepresentativeName
					  ,P.[FundingOfficeContractTechnicalRepresent]
					  ,'' FundingOfficeContractTechnicalRepresentName
					  ,P.[ProjectCounter]
				  FROM [dbo].[Contract] P
				
				left join ResourceAttributeValue PaymentTerms on P.PaymentTerms = PaymentTerms.Value
				left join ResourceAttributeValue InvoiceSubmissionMethod on P.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
				left join NAICS NAICS on P.NAICSCode = NAICS.NAICSGuid
				left join OrgID OrgID on P.ORGID = ORGID.OrgIDGuid
				left join Users CompanyPresident on P.CompanyPresident = CompanyPresident.UserGuid
				left join Users RegionalManager	on P.RegionalManager = RegionalManager.UserGuid
				left join Users ContractControl on P.ProjectControls = ContractControl.UserGuid
				left join Users AccountingRepresentative on P.AccountingRepresentative = AccountingRepresentative.UserGuid                
				left join Users ContractRepresentative on p.ContractRepresentative = ContractRepresentative.UserGuid
				left join Customer AwardingAgency on P.AwardingAgencyOffice = AwardingAgency.CustomerGuid
				left join Customer FundingAgency on P.FundingAgencyOffice = FundingAgency.CustomerGuid
				left join ResourceAttributeValue Currency on P.Currency = Currency.Value
				left join Country Country on P.CountryOfPerformance = Country.CountryId
				left join State State on P.PlaceOfPerformance = State.StateId
				left join PSC on p.PSCCode = PSC.PSCGuid

				join Contract c 
				on p.ParentContractGuid  = c.ContractGuid and c.IsIDIQContract = 1

				  WHERE P.[IsDeleted] = 0
			";

            sql += $" AND P.ParentContractGuid = @ContractGuid ";

            return _context.Connection.Query<ProjectForList>(sql, new { ContractGuid = contractGuid });
        }

        public IEnumerable<ContractForList> GetContract(string searchValue, int pageSize, int skip, int take, string orderBy, string dir)
        {
            string direction = dir;
            if (!String.IsNullOrEmpty(direction))
            {
                switch (direction.ToUpper())
                {
                    case ("ASC"):
                        dir = "asc";
                        break;
                    case ("DESC"):
                        dir = "desc";
                        break;
                    default:
                        dir = "asc";
                        break;
                }
            }
            var where = "";
            var searchString = "";
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "ContractNumber";
            }
            if (take == 0)
            {
                take = 10;
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (C.ContractNumber LIKE @searchValue Or OrgId.title LIKE @searchValue Or C.[ProjectNumber] LIKE @searchValue OR C.[ContractTitle] LIKE @searchValue)";
            }

            var sql = @"
				  SELECT C.[ContractGuid]
				  ,C.[IsIDIQContract]
				  ,C.[IsPrimeContract]
				  ,C.[ContractNumber]
				  ,C.[SubContractNumber]
				  ,C.[ORGID]
				  ,OrgId.title OrgName
				  ,C.[ProjectNumber]
				  ,C.[ContractTitle]
				  ,C.[CompanyPresident]
				  ,(CompanyPresident.Firstname + ' ' + CompanyPresident.Lastname)  CompanyPresidentName
				  ,C.[RegionalManager]
				  ,(RegionalManager.Firstname + ' ' + RegionalManager.Lastname) RegionalManagerName
				  ,C.[ProjectManager]
				  ,(ProjectManager.Firstname + ' ' + ProjectManager.Lastname) ProjectManagerName
				  ,C.[ProjectControls]
				  ,(ProjectControl.Firstname + ' ' + ProjectControl.Lastname) ProjectControlsName
				  ,C.[AccountingRepresentative]
				  ,(AccountingRepresentative.Firstname + ' ' + AccountingRepresentative.Lastname) AccountingRepresentativeName
				  ,C.[ContractRepresentative]
				  ,(ContractRepresentative.Firstname + ' ' + ContractRepresentative.Lastname) ContractRepresentativeName
				  ,C.[CountryOfPerformance]
				  ,Country.CountryName CountryOfPerformanceName
				  ,C.[PlaceOfPerformance]
				  ,C.[POPStart]
				  ,C.[POPEnd]
				  ,C.[NAICSCode]
				  ,NAICS.title NAICSCodeName
				  ,C.[PSCCode]
				  ,PSC.CodeDescription PSCCodeName
				  ,C.[CPAREligible]
				  ,C.[QualityLevelRequirements]
				  ,C.[QualityLevel]
				  ,C.[AwardingAgencyOffice]
				  ,C.[OfficeContractRepresentative]
				  ,C.[OfficeContractTechnicalRepresent]
				  ,C.[FundingAgencyOffice]
				  ,C.[SetAside]
				  ,C.[SelfPerformancePercent]
				  ,C.[SBA]
				  ,C.[Competition]
				  ,C.[ContractType]
				  ,C.[OverHead]
				  ,C.[GAPercent]
				  ,C.[FeePercent]
				  ,C.[Currency]
				  ,C.[BlueSkyAwardAmount]
				  ,C.[AwardAmount]
				  ,C.[FundingAmount]
				  ,C.[BillingAddress]
				  ,C.[BillingFrequency]
				  ,C.[InvoiceSubmissionMethod]
				  ,C.[PaymentTerms]
				  ,C.[AppWageDetermineDavisBaconAct]
				  ,C.[BillingFormula]
				  ,C.[RevenueFormula]
				  ,C.[RevenueRecognitionEACPercent]
				  ,C.[OHonsite]
				  ,C.[OHoffsite]
				  ,C.[CreatedOn]
				  ,C.[UpdatedOn]
				  ,C.[CreatedBy]
				  ,C.[UpdatedBy]
				  ,C.[IsActive]
				  ,C.[IsDeleted]
				  ,(Case When(C.IsActive = 1) Then 'Active' Else 'Inactive' End) IsActiveStatus
				  ,C.[Description]
				  ,C.[AppWageDetermineServiceContractAct]
				  ,C.[FundingOfficeContractRepresentative]
				  ,C.[FundingOfficeContractTechnicalRepresent]
				  FROM [Contract] C
					left join ResourceAttributeValue PaymentTerms on C.PaymentTerms = PaymentTerms.Value
					left join ResourceAttributeValue ContractType on C.ContractType = ContractType.Value
					left join ResourceAttributeValue InvoiceSubmissionMethod on C.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
					left join NAICS NAICS on C.NAICSCode = NAICS.NAICSGuid
					left join OrgID OrgID on C.ORGID = ORGID.OrgIDGuid
					left join Users CompanyPresident on C.CompanyPresident = CompanyPresident.UserGuid
					left join Users RegionalManager	on C.RegionalManager = RegionalManager.UserGuid
					left join Users ProjectManager on C.ProjectManager = ProjectManager.UserGuid
					left join Users ProjectControl on C.ProjectControls = ProjectControl.UserGuid
					left join Users AccountingRepresentative on C.AccountingRepresentative = AccountingRepresentative.UserGuid
					left join Users ContractRepresentative on C.ContractRepresentative = ContractRepresentative.UserGuid
					left join Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
					left join Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
					left join ResourceAttributeValue Currency on C.Currency = Currency.Value
					left join Country Country on C.CountryOfPerformance = Country.CountryId
					left join State State on C.PlaceOfPerformance = State.StateId
					left join PSC on C.PSCCode = PSC.PSCGuid
					where C.IsDeleted = 0
					AND C.ParentContractGuid is null
				 ";
            sql += $"{ where }";
            sql += $" ORDER BY CAST(@orderBy as nvarchar(200)) {dir} OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
            return _context.Connection.Query<ContractForList>(sql, new { searchValue = searchString, orderBy, skip, take });
        }

        public int GetContractCount(string searchValue)
        {
            string where = "";
            string searchString = "";
            if (searchValue != "")
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (C.ContractNumber LIKE @searchValue Or OrgId.title LIKE @searchValue Or C.[ProjectNumber] LIKE @searchValue OR C.[ContractTitle] LIKE @searchValue)";
            }

            var sql = @"
				  SELECT count(1)
				  FROM [Contract] C
					left join ResourceAttributeValue PaymentTerms on C.PaymentTerms = PaymentTerms.Value
					left join ResourceAttributeValue ContractType on C.ContractType = ContractType.Value
					left join ResourceAttributeValue InvoiceSubmissionMethod on C.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
					left join NAICS NAICS on C.NAICSCode = NAICS.NAICSGuid
					left join OrgID OrgID on C.ORGID = ORGID.OrgIDGuid
					left join Users CompanyPresident on C.CompanyPresident = CompanyPresident.UserGuid
					left join Users RegionalManager	on C.RegionalManager = RegionalManager.UserGuid
					left join Users ProjectManager on C.ProjectManager = ProjectManager.UserGuid
					left join Users ProjectControl on C.ProjectControls = ProjectControl.UserGuid
					left join Users AccountingRepresentative on C.AccountingRepresentative = AccountingRepresentative.UserGuid
					left join Users ContractRepresentative on C.ContractRepresentative = ContractRepresentative.UserGuid
					left join Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
					left join Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
					left join ResourceAttributeValue Currency on C.Currency = Currency.Value
					left join Country Country on C.CountryOfPerformance = Country.CountryId
					left join State State on C.PlaceOfPerformance = State.StateId
					left join PSC on C.PSCCode = PSC.PSCGuid
					where C.IsDeleted = 0
				 ";
            sql += $"{ where }";
            return _context.Connection.ExecuteScalar<int>(sql, new { searchValue = searchString });
        }

        public string GetContractNumberById(Guid id)
        {
            string contractnumbersql = @"select ContractNumber from Contract WHERE ContractGuid = @ContractGuid;";
            var contractnumber = _context.Connection.QueryFirstOrDefault<string>(contractnumbersql, new { ContractGuid = id });
            return contractnumber;
        }

        public Guid? GetContractIdByProjectId(Guid id)
        {
            var contractGuidQuery = string.Format($@"
			   select contractGuid from contract 
						 where Contract.IsDeleted = 0 
						   and Contract.ContractGuid = @ContractGuid");
            var contractGuid = _context.Connection.QueryFirstOrDefault<Guid>(contractGuidQuery, new { ContractGuid = id });
            return contractGuid;
        }

        public int GetTotalCountProjectByContractId(Guid contractGuid)
        {
            var getCount = string.Format($@"    
			   select count(1) from contract 
			   where contract.IsDeleted = 0 and contract.ParentContractGuid = @ContractGuid");
            var totalProjectForContract = _context.Connection.QuerySingle<int>(getCount, new { ContractGuid = contractGuid });
            return totalProjectForContract;
        }
        public Guid? GetPreviousProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
			   select top 1 project.contractGuid 
					from contract project
						 where project.IsDeleted = 0 
						   and project.ParentContractGuid = @ContractGuid 
						   and projectCounter < @CurrentProjectCounter
						   order by projectCounter desc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery, new { ContractGuid = contractGuid, CurrentProjectCounter = currentProjectCounter });
            return getPreviousProjectGuid;
        }
        public Guid? GetNextProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
			   select top 1 project.contractGuid 
					from contract project
						 where project.IsDeleted = 0 
						   and project.ParentContractGuid = @ContractGuid 
						   and projectCounter < @CurrentProjectCounter
						   order by projectCounter asc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery, new { ContractGuid = contractGuid, CurrentProjectCounter = currentProjectCounter });
            return getPreviousProjectGuid;
        }

        public int HasChild(Guid projectGuid)
        {
            var getCount = string.Format($@"
			   select count(1) from projectModification where isDeleted = 0 and projectGuid = @ProjectGuid");
            var totalProjectModificaton = _context.Connection.QuerySingle<int>(getCount, new { ProjectGuid = projectGuid });
            return totalProjectModificaton;
        }
        public IEnumerable<Core.Entities.Contract> GetAllProject(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            var where = "";
            var searchString = "";
            var rowNum = pageSize + skip;

            if (sortField.ToLower().Equals("contracttitle"))
            {
                orderingQuery.Append($"contract.ContractTitle {sortDirection}");
            }
            else
            {
                orderingQuery.Append($"Contract.{sortField} {sortDirection}");
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(Contract.ContractNumber like @searchValue )";
            }

            var sqlQuery = string.Format($@"Select * 
													FROM 
														(SELECT ROW_NUMBER() OVER (ORDER BY @orderingQuery) AS RowNum, 
																						ContractGuid,
																						Contract.ProjectNumber,
																						Contract.IsActive,
																						Contract.ORGID,
																						Contract.Currency,
																						Contract.AwardAmount AwardAmount,
																						Contract.POPStart Periodofperformancestart,
																						Contract.POPEnd Periodofperformanceend,
																						Contract.UpdatedOn,
																						Contract.ContractNumber,
																						Contract.ContractTitle,
																						Contract.FundingAmount FundingAmount
																						from Contract
																						where Contract.IsDeleted = 0
																						{ where } 
																						and contract.ParentContractGuid= @ContractGuid
									  ) AS Paged 
											WHERE   
											RowNum > @skip 
											AND RowNum <= @rowNum  
										ORDER BY RowNum");
            //sqlQuery += ""
            var pagedData = _context.Connection.Query<Core.Entities.Contract>(sqlQuery, new { searchValue = searchString, ContractGuid = contractGuid, orderingQuery = orderingQuery.ToString(), skip = skip, rowNum = rowNum });
            return pagedData;
        }

        public ContractQuestionaire GetQuestionariesFromContract(Guid id)
        {
            string sql = $"SELECT * FROM ContractQuestionaire WHERE ContractGuid = @ContractGuid;";
            var result = _context.Connection.QueryFirstOrDefault<ContractQuestionaire>(sql, new { ContractGuid = id });
            if (result == null)
            {
                ContractQuestionaire contractQuestionaire = new ContractQuestionaire();
                return contractQuestionaire;
            }
            return result;
        }

        public string GetParentContractNumberById(Guid id)
        {
            string parentGuidsql = $"select ParentContractGuid from Contract WHERE ContractGuid = @ContractGuid;";
            var parentContractGuid = _context.Connection.QueryFirstOrDefault<Guid?>(parentGuidsql, new { ContractGuid = id });
            string contractnumbersql = $"select ContractNumber from Contract WHERE ContractGuid = @ParentContractGuid;";
            var contractnumber = _context.Connection.QueryFirstOrDefault<string>(contractnumbersql, new { ParentContractGuid = parentContractGuid });
            return contractnumber;
        }

        public string GetProjectNumberById(Guid id)
        {
            string projectnumbersql = $"select ProjectNumber from Contract WHERE ContractGuid = @ContractGuid;";
            var projectNumber = _context.Connection.QueryFirstOrDefault<string>(projectnumbersql, new { ContractGuid = id });
            return projectNumber;
        }

        public Guid GetFarContractTypeGuidById(Guid id)
        {
            string projectnumbersql = $"select FarContractTypeGuid from Contract WHERE ContractGuid = @ContractGuid;";
            var farContractTypeGuidById = _context.Connection.QuerySingle<Guid?>(projectnumbersql, new { ContractGuid = id });
            return farContractTypeGuidById ?? Guid.Empty;
        }

        //public bool updateContractRevenueRecognitionGuid(Guid contractGuid, Guid revenueRecognitionGuid)
        //{
        //    var updateQuery = @"Update Contract set RevenueRecognitionguid = @revenueRecognitionGuid
        //                         where contractGuid= @contractGuid ";
        //    _context.Connection.Query(updateQuery, new { revenueRecognitionGuid = revenueRecognitionGuid, contractGuid = contractGuid });
        //    return true; throw new NotImplementedException();
        //}
    }
}

