using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.ViewModels;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;

namespace Northwind.Infrastructure.Data.Contract
{
    public class ContractRepository : IContractRepository
    {
        IDatabaseContext _context;
        public ContractRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<Northwind.Core.Entities.Contract> GetAll(string searchValue, bool IsIDIQContract, int FilterList, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();
            string pagingQuery = string.Empty;
            if (sortField.Equals("isActiveStatus"))
            {
                orderingQuery.Append($"contract.isActive {sortDirection}");  //Ambiguous if not done.. 
            }
            else
            {
                orderingQuery.Append($"{sortField} {sortDirection}");
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var splittedVal = searchValue.Split(' ');
                foreach (var val in splittedVal)
                {
                    conditionalQuery.Append($@"and (contract.ContractNumber like '%{val}%' 
                          or contract.ContractTitle like '%{val}%'
                          or contract.ProjectNumber like '%{val}%')");
                }

            }

            pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                       (SELECT ROW_NUMBER() OVER (ORDER BY contractNumber asc) AS RowNum, 
                                                                                       'Yes' IsContract,
                                                                                        ContractGuid,
                                                                                        ContractNumber ContractNumber,
                                                                                        ProjectNumber ,
                                                                                        ContractTitle ContractTitle,
                                                                                        contract.IsActive,
                                                                                        contract.IsIDIQContract,
                                                                                        contract.AwardAmount AwardAmount,
                                                                                        contract.POPStart Periodofperformancestart,
                                                                                        contract.POPEnd Periodofperformanceend,
                                                                                        PaymentTerms.Name PaymentTerms,
                                                                                        ContractType.Name ContractTypeName,
																						InvoiceSubmissionMethod.Name InvoiceSubmissionMethod,
                                                                                        contract.UpdatedOn,
                                                                                        contract.Description,
                                                                                        contract.SubContractNumber,
                                                                                        NAICS.Title NAICSCodeName,
                                                                                        ORGID.Name OrganizationName,
                                                                                        Country.CountryName CountryOfPerformance,
																						State.StateName PlaceOfPerformance,

                                                                                        CompanyPresident.Displayname CompanyPresidentName,
																						RegionalManager.Displayname RegionalManagerName,
																						ProjectControl.Displayname ProjectControlName,
																						ProjectControl.Displayname ProjectManagerName,
																						AccountingRepresentative.Displayname AccountingRepresentativeName,
																						ContractRepresentative.Displayname ContractRepresentativeName,

                                                                                        AwardingAgency.CustomerName AwardingAgencyOfficeName,
                                                                                        FundingAgency.CustomerName FundingAgencyOfficeName,


                                                                                        contract.OverHead,
                                                                                        contract.G_A_Percent,
                                                                                        contract.Fee_Percent,
                                                                                        Currency.Name,
                                                                                        contract.BlueSkyAward_Amount


                                                                                        from contract  
                                                                                        left join
                                                                                        ResourceAttributeValue PaymentTerms
                                                                                      on contract.PaymentTerms = PaymentTerms.Value
																					    left join
																						ResourceAttributeValue ContractType
																					  on contract.ContractType = ContractType.Value
																					    left join
																						ResourceAttributeValue InvoiceSubmissionMethod
																					  on contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
                                                                                        left join
																						NAICS NAICS
																					  on contract.NAICSCode = NAICS.NAICSGuid
                                                                                        left join
																						OrgID OrgID
																					  on contract.ORGID = ORGID.OrgIDGuid
                                                                                       left join
																						Users CompanyPresident
																					  on contract.CompanyPresident = CompanyPresident.UserGuid
                                                                                        left join
																						Users RegionalManager
																					  on contract.RegionalManager = RegionalManager.UserGuid
                                                                                        left join
																						Users ProjectManager
																					  on contract.ProjectManager = ProjectManager.UserGuid
                                                                                        left join
																						Users ProjectControl
																					  on contract.ProjectControls = ProjectControl.UserGuid
                                                                                        left join
																						Users AccountingRepresentative
																					  on contract.AccountingRepresentative = AccountingRepresentative.UserGuid
                                                                                      left join
																						Users ContractRepresentative
																					  on contract.ContractRepresentative = ContractRepresentative.UserGuid
                                                                                        left join
																						Customer AwardingAgency
																					  on contract.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                                                                                        left join
																						Customer FundingAgency
																					  on contract.FundingAgencyOffice = FundingAgency.CustomerGuid
																						 left join
																						ResourceAttributeValue Currency
																					  on contract.Currency = Currency.Value
                                                                                      left join
																						Country Country
																					  on contract.CountryOfPerformance = Country.CountryId
																					   left join
																						State State
																					  on contract.PlaceOfPerformance = State.StateId
                                                                                        where contract.IsDeleted = 0 
                                                                                             and contract.parent_contractGuid is null 
 
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > {skip} 
                                            AND RowNum <= {pageSize + skip}  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<Northwind.Core.Entities.Contract>(pagingQuery);
            return pagedData;
        }

        public IEnumerable<Northwind.Core.Entities.Contract> GetAllWithTaskOrder(string searchValue, bool IsIDIQContract, int FilterList, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();
            string pagingQuery = string.Empty;
            if (sortField.Equals("isActiveStatus"))
            {
                orderingQuery.Append($"contract.isActive {sortDirection}");  //Ambiguous if not done.. 
            }
            else
            {
                orderingQuery.Append($"{sortField} {sortDirection}");
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var splittedVal = searchValue.Split(' ');
                foreach (var val in splittedVal)
                {
                    conditionalQuery.Append($@" and (a.ContractNumber like '%{val}%' 
                          or a.ContractTitle like '%{val}%'
                          or a.ProjectNumber like '%{val}%' )");
                }

            }

            pagingQuery = string.Format($@" select * from 
															                ( select    contract.ContractGuid,
																						contract.ContractGuid ContractId,
																						'Yes' IsContract,
                                                                                        '(C) ' + ContractNumber ContractNumber,
                                                                                        ProjectNumber ,
                                                                                        ContractTitle ContractTitle,
                                                                                        contract.IsActive,
                                                                                        contract.IsIDIQContract,
                                                                                        contract.AwardAmount AwardAmount,
                                                                                        contract.POPStart Periodofperformancestart,
                                                                                        contract.POPEnd Periodofperformanceend,
                                                                                        PaymentTerms.Name PaymentTerms,
                                                                                        ContractType.Name ContractTypeName,
																						InvoiceSubmissionMethod.Name InvoiceSubmissionMethod,
                                                                                        contract.UpdatedOn,
                                                                                        contract.Description,
                                                                                        contract.SubContractNumber,
                                                                                        NAICS.Title NAICSCodeName,
                                                                                        ORGID.Name OrganizationName,
																						Country.CountryName CountryOfPerformance,
																						State.StateName PlaceOfPerformance,

                                                                                        CompanyPresident.Displayname CompanyPresidentName,
																						RegionalManager.Displayname RegionalManagerName,
																						ProjectControl.Displayname ProjectControlName,
																						ProjectControl.Displayname ProjectManagerName,
																						AccountingRepresentative.Displayname AccountingRepresentativeName,
																						ContractRepresentative.Displayname ContractRepresentativeName,

                                                                                        AwardingAgency.CustomerName AwardingAgencyOfficeName,
                                                                                        FundingAgency.CustomerName FundingAgencyOfficeName,


                                                                                        contract.OverHead,
                                                                                        contract.G_A_Percent,
                                                                                        contract.Fee_Percent,
                                                                                        Currency.Name,
                                                                                        contract.BlueSkyAward_Amount


                                                                                        from contract  
                                                                                        left join
                                                                                        ResourceAttributeValue PaymentTerms
                                                                                      on contract.PaymentTerms = PaymentTerms.Value
																					    left join
																						ResourceAttributeValue ContractType
																					  on contract.ContractType = ContractType.Value
																					    left join
																						ResourceAttributeValue InvoiceSubmissionMethod
																					  on contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
                                                                                        left join
																						NAICS NAICS
																					  on contract.NAICSCode = NAICS.NAICSGuid
                                                                                        left join
																						OrgID OrgID
																					  on contract.ORGID = ORGID.OrgIDGuid
                                                                                       left join
																						Users CompanyPresident
																					  on contract.CompanyPresident = CompanyPresident.UserGuid
                                                                                        left join
																						Users RegionalManager
																					  on contract.RegionalManager = RegionalManager.UserGuid
                                                                                        left join
																						Users ProjectManager
																					  on contract.ProjectManager = ProjectManager.UserGuid
                                                                                        left join
																						Users ProjectControl
																					  on contract.ProjectControls = ProjectControl.UserGuid
                                                                                        left join
																						Users AccountingRepresentative
																					  on contract.AccountingRepresentative = AccountingRepresentative.UserGuid
                                                                                      left join
																						Users ContractRepresentative
																					  on contract.ContractRepresentative = ContractRepresentative.UserGuid
                                                                                        left join
																						Customer AwardingAgency
																					  on contract.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                                                                                        left join
																						Customer FundingAgency
																					  on contract.FundingAgencyOffice = FundingAgency.CustomerGuid
																						 left join
																						ResourceAttributeValue Currency
																					  on contract.Currency = Currency.Value
																					  left join
																						Country Country
																					  on contract.CountryOfPerformance = Country.CountryId
																					   left join
																						State State
																					  on contract.PlaceOfPerformance = State.StateId
                                                                                        where contract.IsDeleted = 0
                                                                                          --    and contract.IsIDIQContract = 1

																					   union 

																					   select
																					    project.ProjectGuid,
																					    project.ContractGuid ContractId,
																						'No' IsContract,
                                                                                        '(P) ' + Contract.ContractNumber ContractNumber,
                                                                                        project.ProjectNumber ,
                                                                                        projectTitle ,
                                                                                        project.IsActive,
                                                                                        0,
                                                                                        project.AwardAmount AwardAmount,
                                                                                        project.POPStart Periodofperformancestart,
                                                                                        project.POPEnd Periodofperformanceend,
                                                                                        PaymentTerms.Name PaymentTerms,
                                                                                        projectType.Name projectType,
																						InvoiceSubmissionMethod.Name InvoiceSubmissionMethod,
                                                                                        project.UpdatedOn,
                                                                                        project.Description,
                                                                                        '',
                                                                                        NAICS.Title NAICSCodeName,
                                                                                        ORGID.Name OrganizationName,
																						Country.CountryName CountryOfPerformance,
																						State.StateName PlaceOfPerformance,

                                                                                        CompanyPresident.Displayname CompanyPresidentName,
																						RegionalManager.Displayname RegionalManagerName,
																						ProjectControl.Displayname ProjectControlName,
																						ProjectControl.Displayname ProjectManagerName,
																						AccountingRepresentative.Displayname AccountingRepresentativeName,
																						ProjectRepresentative.Displayname ContractRepresentativeName,

                                                                                        AwardingAgency.CustomerName AwardingAgencyOfficeName,
                                                                                        FundingAgency.CustomerName FundingAgencyOfficeName,

                                                                                        project.OverHead,
                                                                                        project.G_A_Percent,
                                                                                        project.Fee_Percent,
                                                                                        Currency.Name,
                                                                                        project.BlueSkyAward_Amount


                                                                                        from project  
                                                                                        left join
                                                                                        ResourceAttributeValue PaymentTerms
                                                                                      on project.PaymentTerms = PaymentTerms.Value
																					    left join
																						ResourceAttributeValue projectType
																					  on project.projectType = projectType.Value
																					    left join
																						ResourceAttributeValue InvoiceSubmissionMethod
																					  on project.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
                                                                                        left join
																						NAICS NAICS
																					  on project.NAICSCode = NAICS.NAICSGuid
                                                                                        left join
																						OrgID OrgID
																					  on project.ORGID = ORGID.OrgIDGuid
                                                                                       left join
																						Users CompanyPresident
																					  on project.CompanyPresident = CompanyPresident.UserGuid
                                                                                        left join
																						Users RegionalManager
																					  on project.RegionalManager = RegionalManager.UserGuid
                                                                                        left join
																						Users ProjectManager
																					  on project.ProjectManager = ProjectManager.UserGuid
                                                                                        left join
																						Users ProjectControl
																					  on project.ProjectControls = ProjectControl.UserGuid
                                                                                        left join
																						Users AccountingRepresentative
																					  on project.AccountingRepresentative = AccountingRepresentative.UserGuid
																					   left join
																						Users ProjectRepresentative
																					  on project.ProjectRepresentative = ProjectRepresentative.UserGuid
                                                                                        left join
																						Customer AwardingAgency
																					  on project.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                                                                                        left join
																						Customer FundingAgency
																					  on project.FundingAgencyOffice = FundingAgency.CustomerGuid
																						 left join
																						ResourceAttributeValue Currency
																					  on project.Currency = Currency.Value
																					  left join
																						Country Country
																					  on project.CountryOfPerformance = Country.CountryId
																					   left join
																						State State
																					  on project.PlaceOfPerformance = State.StateId
																					   join Contract 
																					   on Project.ContractGuid = Contract.ContractGuid
																					   where Project.IsDeleted = 0
																					         and Contract.IsDeleted = 0
                                                                                             and contract.IsIDIQContract = 1
                                                                                    ) a 
                                                                                        where 1=1
                                                                                        {conditionalQuery}
                                                                                    order by a.ContractId, IsContract desc,a.UpdatedOn desc
                                                                                ");

            var pagedData = _context.Connection.Query<Northwind.Core.Entities.Contract>(pagingQuery);
            return pagedData;
        }

        public int TotalRecord()
        {
            string sql = "SELECT Count(1) FROM contract WHERE IsDeleted = 0";
            var result = _context.Connection.QuerySingle<int>(sql);
            return result;
        }

        public int AddContract(Core.Entities.Contract ContractModel)
        {
            string insertQuery = $@"INSERT INTO [dbo].[Contract]
                                                                   (
                                                                        ContractGuid					                            ,
                                                                        Parent_ContractGuid					                        ,
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
                                                                        Office_ContractRepresentative		                        ,
                                                                        Office_ContractTechnicalRepresent                           ,
                                                                        FundingAgencyOffice				                            ,
                                                                        FundingOffice_ContractRepresentative				        ,
                                                                        FundingOffice_ContractTechnicalRepresent				    ,
                                                                                                                                    
                                                                        SetAside						                            ,
                                                                        SelfPerformance_Percent			                            ,
                                                                        SBA								                            ,
                                                                        Competition						                            ,
                                                                        ContractType					                            ,
                                                                        OverHead						                            ,
                                                                        G_A_Percent						                            ,
                                                                        Fee_Percent						                            ,
                                                                        Currency						                            ,
                                                                        BlueSkyAward_Amount				                            ,
                                                                        AwardAmount						                            ,
                                                                        FundingAmount					                            ,
                                                                        BillingAddress					                            ,
                                                                        BillingFrequency				                            ,
                                                                        InvoiceSubmissionMethod			                            ,
                                                                        PaymentTerms					                            ,
                                                                        AppWageDetermine_DavisBaconAct				                ,
                                                                        AppWageDetermine_ServiceContractAct				            ,
                                                                        BillingFormula					                            ,
                                                                        RevenueFormula					                            ,
                                                                        RevenueRecognitionEAC_Percent	                            ,
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
                                                                        @Parent_ContractGuid                                ,
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
                                                                        @Office_ContractRepresentative                    ,	
                                                                        @Office_ContractTechnicalRepresent                ,
                                                                        @FundingAgencyOffice                              ,
                                                                        @FundingOffice_ContractRepresentative             ,
                                                                        @FundingOffice_ContractTechnicalRepresent         ,
                                                                        
                                                                        @setAside                                         ,
                                                                        @SelfPerformance_Percent                          ,
                                                                        @SBA                                              ,
                                                                        @Competition                                      ,
                                                                        @ContractType                                     ,
                                                                        @OverHead                                         ,
                                                                        @G_A_Percent                                      ,
                                                                        @Fee_Percent                                      ,
                                                                        @Currency                                         ,
                                                                        @BlueSkyAward_Amount                              ,
                                                                        @AwardAmount                                      ,
                                                                        @FundingAmount                                    ,
                                                                        @BillingAddress                                   ,
                                                                        @BillingFrequency                                 ,
                                                                        @InvoiceSubmissionMethod                          ,
                                                                        @PaymentTerms                                     ,
                                                                        @AppWageDetermine_DavisBaconAct                   ,
                                                                        @AppWageDetermine_ServiceContractAct              ,
                                                                        @BillingFormula                                   ,
                                                                        @RevenueFormula                                   ,
                                                                        @RevenueRecognitionEAC_Percent                    ,
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
            return _context.Connection.Execute(insertQuery, ContractModel);
        }

        public int UpdateContract(Core.Entities.Contract ContractModel)
        {
            string updateQuery = $@"Update Contract       set 
                                                                      Parent_ContractGuid						=           @Parent_ContractGuid                                                 ,
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
                                                                      Office_ContractRepresentative		        =           @Office_ContractRepresentative                   ,	
                                                                      Office_ContractTechnicalRepresent         =           @Office_ContractTechnicalRepresent               ,
                                                                      FundingAgencyOffice				        =           @FundingAgencyOffice                             ,
                                                                      FundingOffice_ContractRepresentative		=	        @FundingOffice_ContractRepresentative            ,
                                                                      FundingOffice_ContractTechnicalRepresent	=	        @FundingOffice_ContractTechnicalRepresent        ,
                                                                                                                            
                                                                      SetAside						            =           @setAside                                       ,
                                                                      SelfPerformance_Percent			        =           @SelfPerformance_Percent                        ,
                                                                      SBA								        =           @SBA                                            ,
                                                                      Competition						        =           @Competition                                    ,
                                                                      ContractType					            =           @ContractType                                   ,
                                                                      OverHead						            =           @OverHead                                       ,
                                                                      G_A_Percent						        =           @G_A_Percent                                    ,
                                                                      Fee_Percent						        =           @Fee_Percent                                    ,
                                                                      Currency						            =           @Currency                                       ,
                                                                      BlueSkyAward_Amount				        =           @BlueSkyAward_Amount                            ,
                                                                      AwardAmount						        =           @AwardAmount                                    ,
                                                                      FundingAmount					            =           @FundingAmount                                  ,
                                                                      BillingAddress					        =           @BillingAddress                                 ,
                                                                      BillingFrequency				            =           @BillingFrequency                               ,
                                                                      InvoiceSubmissionMethod			        =           @InvoiceSubmissionMethod                        ,
                                                                      PaymentTerms					            =           @PaymentTerms                                   ,
                                                                      AppWageDetermine_DavisBaconAct			=	        @AppWageDetermine_DavisBaconAct                 ,
                                                                      AppWageDetermine_ServiceContractAct		=		    @AppWageDetermine_ServiceContractAct            ,
                                                                      BillingFormula					        =           @BillingFormula                                 ,
                                                                      RevenueFormula					        =           @RevenueFormula                                 ,
                                                                      RevenueRecognitionEAC_Percent	            =           @RevenueRecognitionEAC_Percent                  ,
                                                                      OHonsite						            =           @OHonsite                                       ,
                                                                      OHoffsite						            =           @OHoffsite                                      ,
                                                                      UpdatedOn						            =           @UpdatedOn                                                           ,
                                                                      UpdatedBy						            =           @UpdatedBy                                                           ,
                                                                      IsActive						            =           @IsActive                                                            ,
                                                                      IsDeleted                                 =           @IsDeleted                                                           
                                                                      where ContractGuid = @ContractGuid ";
            return _context.Connection.Execute(updateQuery, ContractModel);
        }

        public int DeleteContract(Guid[] ContractGuidIds)
        {
            foreach (var ContractGuid in ContractGuidIds)
            {
                var Contract = new
                {
                    ContractGuid = ContractGuid
                };
                string disableQuery = @"Update Contract set 
                                               IsDeleted   = 1
                                               where ContractGuid =@ContractGuid ";
                _context.Connection.Execute(disableQuery, Contract);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int DisableContract(Guid[] ContractGuidIds)
        {
            foreach (var ContractGuid in ContractGuidIds)
            {
                var Contract = new
                {
                    ContractGuid = ContractGuid
                };
                string disableQuery = @"Update Contract set 
                                            IsActive   = 0
                                            where ContractGuid =@ContractGuid ";
                _context.Connection.Execute(disableQuery, Contract);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int EnableContract(Guid[] ContractGuidIds)
        {
            foreach (var ContractGuid in ContractGuidIds)
            {
                var Contract = new
                {
                    ContractGuid = ContractGuid
                };
                string disableQuery = @"Update Contract set 
                                            IsActive   = 1
                                            where ContractGuid =@ContractGuid ";
                _context.Connection.Execute(disableQuery, Contract);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }

        public ICollection<Organization> GetOrganizationData(string searchText)
        {
            var organizationDataQuery = string.Format($@"
            select * from OrgID
                where Name like '%{@searchText}%' or Description like '%{@searchText}%' ");
            var organizationData = _context.Connection.Query<Organization>(organizationDataQuery).ToList();
            return organizationData;
        }

        public ICollection<Customer> GetAwardingAgencyOfficeData(string searchText)
        {
            string sql = $@"select 
                customer.CustomerGuid,
                customerType.CustomerTypeName,
                customer.Department,
                customer.Agency,
                customer.CustomerName, 
                customer.CustomerCode,
                customer.Address,
                customer.AddressLine1,
                customer.City,
                states.StateName,
                country.CountryName,
                customer.ZipCode,
                customer.PrimaryPhone,
                customer.PrimaryEmail,
                customer.Url,
                customer.Tags,
                customer.Abbreviations,
                customer.CustomerDescription,
                customer.Department
                 from Customer customer
                left join CustomerType customertype on customerType.CustomerTypeGuid = customer.CustomerTypeGuid
                left join State states on states.StateId = customer.StateId
                left join Country country on country.CountryId = customer.CountryId 
                where CustomerCode like  '%{searchText}%' or CustomerName like '%{searchText}%' 
                and customer.IsDeleted = 0
                order by CustomerName asc";

            var data = _context.Connection.Query<Customer>(sql).ToList();
            return data;
        }

        public ICollection<Customer> GetFundingAgencyOfficeData(string searchText)
        {
            string sql = $@"select 
                customer.CustomerGuid,
                customerType.CustomerTypeName,
                customer.Department,
                customer.Agency,
                customer.CustomerName, 
                customer.CustomerCode,
                customer.Address,
                customer.AddressLine1,
                customer.City,
                states.StateName,
                country.CountryName,
                customer.ZipCode,
                customer.PrimaryPhone,
                customer.PrimaryEmail,
                customer.Url,
                customer.Tags,
                customer.Abbreviations,
                customer.CustomerDescription,
                customer.Department
                 from Customer customer
                left join CustomerType customertype on customerType.CustomerTypeGuid = customer.CustomerTypeGuid
                left join State states on states.StateId = customer.StateId
                left join Country country on country.CountryId = customer.CountryId 
                where CustomerCode like  '%{searchText}%' or CustomerName like '%{searchText}%' 
                and customer.IsDeleted = 0
                order by CustomerName asc";

            var data = _context.Connection.Query<Customer>(sql).ToList();
            return data;
        }

        public ICollection<Naics> GetNAICSCodeData(string searchText)
        {
            var nAICSCodeQuery = string.Format($@"
            select * from NAICS
                where code like '%{@searchText}%' or title like '%{@searchText}%' ");
            var nAICSCode = _context.Connection.Query<Naics>(nAICSCodeQuery).ToList();
            return nAICSCode;
        }

        public ICollection<Psc> GetPSCCodeData(string searchText)
        {
            var pSCCodeQuery = string.Format($@"
            select * from PSC
                where code like '%{@searchText}%' or CodeDescription like '%{@searchText}%' ");
            var pSCSCode = _context.Connection.Query<Psc>(pSCCodeQuery).ToList();
            return pSCSCode;
        }

        public AssociateUserList GetCompanyRegionOfficeNameByCode(EntityCode entityCode)
        {
            AssociateUserList userList = new AssociateUserList();
            var companyPresidentQuery = string.Format($@"select Users.Displayname , Users.UserGuid , Users.JobTitle from Company left join Users on Company.President = Users.UserGuid where Company.CompanyCode = '{entityCode.CompanyCode}'");
            var regionalManagerQuery = string.Format($@"select Users.Displayname , Users.UserGuid , Users.JobTitle  from Region left join Users on Region.RegionalManager = Users.UserGuid where Region.RegionCode = '{entityCode.RegionCode}'");
            var companyNameQuery = string.Format($@"select Company.CompanyName from Company where CompanyCode = '{entityCode.CompanyCode}' and IsDeleted = 0");
            var regionNameQuery = string.Format($@"select Region.RegionName from Region where RegionCode = '{entityCode.RegionCode}' and IsDeleted = 0");
            var officeNameQuery = string.Format($@"select Office.OfficeName from Office where OfficeCode = '{entityCode.OfficeCode}' and IsDeleted = 0");
            userList.CompanyPresident = _context.Connection.QueryFirst<User>(companyPresidentQuery);
            userList.RegionManager = _context.Connection.QueryFirst<User>(regionalManagerQuery);
            userList.CompanyName = _context.Connection.QueryFirst<string>(companyNameQuery);
            userList.RegionName = _context.Connection.QueryFirst<string>(regionNameQuery);
            userList.OfficeName = _context.Connection.QueryFirst<string>(officeNameQuery);
            return userList;
        }

        public IEnumerable<User> GetUsersData(string searchText)
        {
            var usersQuery = string.Format($@"
               select * from Users
                where Firstname like '%{@searchText}%' or Lastname like '%{@searchText}%' 
                order by Firstname asc");
            var usersData = _context.Connection.Query<User>(usersQuery).ToList();
            return usersData;
        }

        public ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType)
        {
            var model = new List<KeyValuePairWithDescriptionModel<Guid, string, string>>();
            var sql =
                $@"SELECT CustomerContact.* FROM CustomerContact 
                join CustomerContactType on CustomerContact.ContactType = CustomerContactType.ContactTypeGuid
                where CustomerContactType.ContactType = '{contactType}' and CustomerGuid ='{customerId}' order by FirstName asc";
            var data = _context.Connection.Query<CustomerContact>(sql);
            foreach (var item in data)
            {
                var fullName = FormatHelper.FormatFullName(item.FirstName, item.MiddleName, item.LastName);
                //                model.Add(new KeyValuePairWithDescriptionModel<Guid, string, string> { Keys = item.ContactGuid, Values = item.FirstName + " " + item.MiddleName + " " + item.LastName, Descriptions = item.PhoneNumber });
                //                model.Add(new KeyValuePairWithDescriptionModel<Guid, string, string> { Keys = item.ContactGuid, Values = item.FirstName + " " + item.MiddleName + " " + item.LastName + "<i> " + item.PhoneNumber + "</i>", Descriptions = "" });
                model.Add(new KeyValuePairWithDescriptionModel<Guid, string, string> { Keys = item.ContactGuid, Values = fullName + " - " + item.PhoneNumber, Descriptions = "" });
            }
            return model;
        }

        public ContractViewModel GetDetailsById(Guid id)
        {
            ContractViewModel contractViewModel = new ContractViewModel();
            string CompanyCodeSql = $"select SUBSTRING(OrgID.Name,1,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var CompanyCode = _context.Connection.Query<string>(CompanyCodeSql).FirstOrDefault();

            string CompanyGuidSql = $"select Company.President from Company where CompanyCode = '{CompanyCode}'";
            var CompanyGuid = _context.Connection.Query<Guid>(CompanyGuidSql).FirstOrDefault();

            string CompanyPresidentSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = '{CompanyGuid}'";
            var CompanyPresident = _context.Connection.Query<string>(CompanyPresidentSql).FirstOrDefault();

            string CompanyNameSql = $"select Company.CompanyName from Company where CompanyCode = '{CompanyCode}' and IsDeleted = 0";
            var CompanyName = _context.Connection.Query<String>(CompanyNameSql).FirstOrDefault();

            string RegionCodeSql = $@"select SUBSTRING(OrgID.Name,4,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var RegionCode = _context.Connection.Query<string>(RegionCodeSql).FirstOrDefault();

            string RegionGuidSql = $@"select Region.RegionalManager from Region where RegionCode = '{RegionCode}'";
            var RegionGuid = _context.Connection.Query<Guid>(RegionGuidSql).FirstOrDefault();

            string RegionalManagerSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = '{RegionGuid}'";
            var RegionalManager = _context.Connection.Query<string>(RegionalManagerSql).FirstOrDefault();

            string RegionNameSql = $"select Region.RegionName from Region where RegionCode = '{RegionCode}' and IsDeleted = 0";
            var RegionName = _context.Connection.Query<String>(RegionNameSql).FirstOrDefault();

            string OfficeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var OfficeCode = _context.Connection.Query<string>(RegionCodeSql).FirstOrDefault();

            string OfficeNameSql = $"select Office.OfficeName from Office where OfficeCode = '{OfficeCode}' and IsDeleted = 0";
            var OfficeName = _context.Connection.Query<String>(OfficeNameSql).FirstOrDefault();

            string questionary = $" select questionaire.ContractQuestionaireGuid,users.Displayname,questionaire.UpdatedOn from ContractQuestionaire questionaire " +
                   $"left join Users users on users.UserGuid = questionaire.UpdatedBy where questionaire.ContractGuid = '{id}' and Isdeleted = 0";
            var ContractQuestionaire = _context.Connection.Query<ContractQuestionaire>(questionary).FirstOrDefault();

            string billingrates = $"select * from EmployeeBillingRates where ContractGuid = '{id}' and Isdeleted = 0";
            var employeeeBillingRates = _context.Connection.Query<EmployeeBillingRates>(billingrates).FirstOrDefault();

            string contractWBSSql = $"select * from ContractWBS where ContractGuid = '{id}' and Isdeleted = 0";
            var contractWBS = _context.Connection.Query<ContractWBS>(contractWBSSql).FirstOrDefault();

            string categoryrates = $"select * from LaborCategoryRates where ContractGuid = '{id}' and Isdeleted = 0";
            var laborCategoryRates = _context.Connection.Query<LaborCategoryRates>(categoryrates).FirstOrDefault();

            string revenue = $"select * from RevenueRecognization where ContractGuid = '{id}' and Isdeleted = 0";
            var RevenueRecognitionModel = _context.Connection.Query<Northwind.Core.Entities.RevenueRecognition>(revenue).FirstOrDefault();

            string requestForm = $"select * from JobRequest where ContractGuid = '{id}' and Isdeleted = 0";
            var requestFormModel = _context.Connection.Query<Core.Entities.JobRequest>(requestForm).FirstOrDefault();

            string BasicContractSql = @"select
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
            var BasicContractInfo = _context.Connection.QuerySingle<BasicContractInfoViewModel>(BasicContractSql, new { ContractGuid = id });

            string KeyPersonnelSql = @"select
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
            var KeyPersonnel = _context.Connection.QuerySingle<KeyPersonnelViewModel>(KeyPersonnelSql, new { ContractGuid = id });

            string CustomerInfoSql = @"select
                            AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							Office_ContractRepresentative.ContactGuid Office_ContractRepresentative,
							(Office_ContractRepresentative.FirstName + ' ' + Office_ContractRepresentative.MiddleName + ' ' + Office_ContractRepresentative.LastName) Office_ContractRepresentativeName,
							Office_ContractTechnicalRepresent.ContactGuid Office_ContractTechnicalRepresent,
							(Office_ContractTechnicalRepresent.FirstName + ' ' + Office_ContractTechnicalRepresent.MiddleName + ' ' + Office_ContractTechnicalRepresent.LastName) Office_ContractTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
                            FundingOffice_ContractRepresentative.ContactGuid FundingOffice_ContractRepresentative,
                            (FundingOffice_ContractRepresentative.FirstName + ' ' + FundingOffice_ContractRepresentative.MiddleName + ' ' + FundingOffice_ContractRepresentative.LastName) FundingOffice_ContractRepresentativeName,
							FundingOffice_ContractTechnicalRepresent.ContactGuid FundingOffice_ContractTechnicalRepresent,
							(FundingOffice_ContractTechnicalRepresent.FirstName + ' ' + FundingOffice_ContractTechnicalRepresent.MiddleName + ' ' + FundingOffice_ContractTechnicalRepresent.LastName) FundingOffice_ContractTechnicalRepresentName
                            
                            from Contract

                            left join
						    Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
						    left join
                            CustomerContact Office_ContractRepresentative on Contract.Office_ContractRepresentative = Office_ContractRepresentative.ContactGuid
						    left join
						    CustomerContact Office_ContractTechnicalRepresent on  Contract.Office_ContractTechnicalRepresent = Office_ContractTechnicalRepresent.ContactGuid

                            left join
						    Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
						    left join
						    CustomerContact FundingOffice_ContractRepresentative on Contract.FundingOffice_ContractRepresentative = FundingOffice_ContractRepresentative.ContactGuid
						    left join
						    CustomerContact FundingOffice_ContractTechnicalRepresent on Contract.FundingOffice_ContractTechnicalRepresent = FundingOffice_ContractTechnicalRepresent.ContactGuid

                            WHERE ContractGuid =  @ContractGuid;";
            var CustomerInformation = _context.Connection.QuerySingle<CustomerInformationViewModel>(CustomerInfoSql, new { ContractGuid = id });

            string FinancialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformance_Percent,
							Contract.SBA,
							Competition.Name CompetitionType,
                            Contract.Competition,
							ContractType.Name ContractTypeName,
                            Contract.ContractType,
							Contract.OverHead,
							Contract.G_A_Percent,
							Contract.Fee_Percent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAward_Amount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							AppWageDetermine_DavisBaconAct.Name AppWageDetermine_DavisBaconActType,
							Contract.AppWageDetermine_DavisBaconAct,
							AppWageDetermine_ServiceContractAct.Name AppWageDetermine_ServiceContractActType,
							Contract.AppWageDetermine_ServiceContractAct,
							Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEAC_Percent,
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
						    ResourceAttributeValue AppWageDetermine_DavisBaconAct on Contract.AppWageDetermine_DavisBaconAct = AppWageDetermine_DavisBaconAct.Value
						    left join
						    ResourceAttributeValue AppWageDetermine_ServiceContractAct on Contract.AppWageDetermine_ServiceContractAct = AppWageDetermine_ServiceContractAct.Value
 
                            WHERE ContractGuid =  @ContractGuid;";
            var FinancialInformation = _context.Connection.QuerySingle<FinancialInformationViewModel>(FinancialInfoSql, new { ContractGuid = id });

            string BaseModelSql = @"select Contract.IsActive from Contract WHERE ContractGuid =  @ContractGuid;";
            var BaseModel = _context.Connection.QuerySingle<BaseModel>(BaseModelSql, new { ContractGuid = id });

            // to fetch States name through state id array..
            if (!string.IsNullOrEmpty(BasicContractInfo.PlaceOfPerformanceSelectedIds))
            {
                var stateIdArr = BasicContractInfo.PlaceOfPerformanceSelectedIds.Split(',');
                var stateIdArrWithStringCote = stateIdArr.Select(x => string.Format("'" + x + "'"));
                var formatQuery = string.Join(",", stateIdArrWithStringCote);
                var stateQuery = $"select StateName from State where StateId in ({formatQuery})";
                var stateNameArr = _context.Connection.Query<string>(stateQuery);
                BasicContractInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);
            }

            string parent_ContractGuidQuery = $"select parent_ContractGuid from contract where contractGuid = '{id}'";
            contractViewModel.Parent_ContractGuid = _context.Connection.QuerySingle<Guid?>(parent_ContractGuidQuery);

            contractViewModel.BasicContractInfo = BasicContractInfo;
            contractViewModel.BasicContractInfo.CompanyName = CompanyName;
            contractViewModel.BasicContractInfo.RegionName = RegionName;
            contractViewModel.BasicContractInfo.OfficeName = OfficeName;
            contractViewModel.KeyPersonnel = KeyPersonnel;
            contractViewModel.KeyPersonnel.CompanyPresidentName = CompanyPresident;
            contractViewModel.KeyPersonnel.RegionalManagerName = RegionalManager;
            contractViewModel.CustomerInformation = CustomerInformation;
            contractViewModel.ContractGuid = id;
            contractViewModel.FinancialInformation = FinancialInformation;
            contractViewModel.IsActive = BaseModel.IsActive;
            contractViewModel.ContractQuestionaire = ContractQuestionaire;
            contractViewModel.employeeBillingRatesViewModel = employeeeBillingRates;
            contractViewModel.ContractWBS = contractWBS;
            contractViewModel.LaborCategoryRates = laborCategoryRates;
            contractViewModel.JobRequest = requestFormModel;
            contractViewModel.revenueRecognitionModel = RevenueRecognitionModel;

            return contractViewModel;
        }

        public ContractViewModel GetDetailsForProjectByContractId(Guid id)
        {
            ContractViewModel contractViewModel = new ContractViewModel();
            string CompanyCodeSql = $"select SUBSTRING(OrgID.Name,1,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var CompanyCode = _context.Connection.Query<string>(CompanyCodeSql).FirstOrDefault();

            string CompanyGuidSql = $"select Company.President from Company where CompanyCode = '{CompanyCode}'";
            var CompanyGuid = _context.Connection.Query<Guid>(CompanyGuidSql).FirstOrDefault();

            string CompanyPresidentSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname  from Users where Users.UserGuid = '{CompanyGuid}'";
            var CompanyPresident = _context.Connection.Query<string>(CompanyPresidentSql).FirstOrDefault();

            string CompanyNameSql = $"select Company.CompanyName from Company where CompanyCode = '{CompanyCode}' and IsDeleted = 0";
            var CompanyName = _context.Connection.Query<String>(CompanyNameSql).FirstOrDefault();

            string RegionCodeSql = $@"select SUBSTRING(OrgID.Name,4,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var RegionCode = _context.Connection.Query<string>(RegionCodeSql).FirstOrDefault();

            string RegionGuidSql = $@"select Region.RegionalManager from Region where RegionCode = '{RegionCode}'";
            var RegionGuid = _context.Connection.Query<Guid>(RegionGuidSql).FirstOrDefault();

            string RegionalManagerSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = '{RegionGuid}'";
            var RegionalManager = _context.Connection.Query<string>(RegionalManagerSql).FirstOrDefault();

            string RegionNameSql = $"select Region.RegionName from Region where RegionCode = '{RegionCode}' and IsDeleted = 0";
            var RegionName = _context.Connection.Query<String>(RegionNameSql).FirstOrDefault();

            string OfficeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var OfficeCode = _context.Connection.Query<string>(RegionCodeSql).FirstOrDefault();

            string OfficeNameSql = $"select Office.OfficeName from Office where OfficeCode = '{OfficeCode}' and IsDeleted = 0";
            var OfficeName = _context.Connection.Query<String>(OfficeNameSql).FirstOrDefault();

            string ContractNumber = $"select ContractNumber from Contract where ContractGuid =  '{id}'";
            var getContractNumber = _context.Connection.Query<string>(ContractNumber).FirstOrDefault();

            string BasicContractSql = @"select
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
            var BasicContractInfo = _context.Connection.QuerySingle<BasicContractInfoViewModel>(BasicContractSql, new { ContractGuid = id });

            string KeyPersonnelSql = @"select
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
            var KeyPersonnel = _context.Connection.QuerySingle<KeyPersonnelViewModel>(KeyPersonnelSql, new { ContractGuid = id });

            string CustomerInfoSql = @"select
                            AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							Office_ContractRepresentative.ContactGuid Office_ContractRepresentative,
							(Office_ContractRepresentative.FirstName + ' ' + Office_ContractRepresentative.MiddleName + ' ' + Office_ContractRepresentative.LastName) Office_ContractRepresentativeName,
							Office_ContractTechnicalRepresent.ContactGuid Office_ContractTechnicalRepresent,
							(Office_ContractTechnicalRepresent.FirstName + ' ' + Office_ContractTechnicalRepresent.MiddleName + ' ' + Office_ContractTechnicalRepresent.LastName) Office_ContractTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
                            FundingOffice_ContractRepresentative.ContactGuid FundingOffice_ContractRepresentative,
                            (FundingOffice_ContractRepresentative.FirstName + ' ' + FundingOffice_ContractRepresentative.MiddleName + ' ' + FundingOffice_ContractRepresentative.LastName) FundingOffice_ContractRepresentativeName,
							FundingOffice_ContractTechnicalRepresent.ContactGuid FundingOffice_ContractTechnicalRepresent,
							(FundingOffice_ContractTechnicalRepresent.FirstName + ' ' + FundingOffice_ContractTechnicalRepresent.MiddleName + ' ' + FundingOffice_ContractTechnicalRepresent.LastName) FundingOffice_ContractTechnicalRepresentName
                            
                            from Contract

                            left join
						    Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
						    left join
                            CustomerContact Office_ContractRepresentative on Contract.Office_ContractRepresentative = Office_ContractRepresentative.ContactGuid
						    left join
						    CustomerContact Office_ContractTechnicalRepresent on  Contract.Office_ContractTechnicalRepresent = Office_ContractTechnicalRepresent.ContactGuid

                            left join
						    Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
						    left join
						    CustomerContact FundingOffice_ContractRepresentative on Contract.FundingOffice_ContractRepresentative = FundingOffice_ContractRepresentative.ContactGuid
						    left join
						    CustomerContact FundingOffice_ContractTechnicalRepresent on Contract.FundingOffice_ContractTechnicalRepresent = FundingOffice_ContractTechnicalRepresent.ContactGuid

                            WHERE ContractGuid =  @ContractGuid;";
            var CustomerInformation = _context.Connection.QuerySingle<CustomerInformationViewModel>(CustomerInfoSql, new { ContractGuid = id });

            string FinancialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformance_Percent,
							Contract.SBA,
							Competition.Name CompetitionType,
                            Contract.Competition,
							ContractType.Name ContractTypeName,
                            Contract.ContractType,
							Contract.OverHead,
							Contract.G_A_Percent,
							Contract.Fee_Percent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAward_Amount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							AppWageDetermine_DavisBaconAct.Name AppWageDetermine_DavisBaconActType,
							Contract.AppWageDetermine_DavisBaconAct,
							AppWageDetermine_ServiceContractAct.Name AppWageDetermine_ServiceContractActType,
							Contract.AppWageDetermine_ServiceContractAct,
							Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEAC_Percent,
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
						    ResourceAttributeValue AppWageDetermine_DavisBaconAct on Contract.AppWageDetermine_DavisBaconAct = AppWageDetermine_DavisBaconAct.Value
						    left join
						    ResourceAttributeValue AppWageDetermine_ServiceContractAct on Contract.AppWageDetermine_ServiceContractAct = AppWageDetermine_ServiceContractAct.Value
 
                            WHERE ContractGuid =  @ContractGuid;";
            var FinancialInformation = _context.Connection.QuerySingle<FinancialInformationViewModel>(FinancialInfoSql, new { ContractGuid = id });

            string BaseModelSql = @"select Contract.IsActive from Contract WHERE ContractGuid =  @ContractGuid;";
            var BaseModel = _context.Connection.QuerySingle<BaseModel>(BaseModelSql, new { ContractGuid = id });

            // to fetch States name through state id array..
            var stateIdArr = BasicContractInfo.PlaceOfPerformanceSelectedIds.Split(',');
            var stateIdArrWithStringCote = stateIdArr.Select(x => string.Format("'" + x + "'"));
            var formatQuery = string.Join(",", stateIdArrWithStringCote);
            var stateQuery = $"select StateName from State where StateId in ({formatQuery})";
            var stateNameArr = _context.Connection.Query<string>(stateQuery); ;
            BasicContractInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);


            contractViewModel.BasicContractInfo = BasicContractInfo;
            contractViewModel.BasicContractInfo.CompanyName = CompanyName;
            contractViewModel.BasicContractInfo.RegionName = RegionName;
            contractViewModel.BasicContractInfo.OfficeName = OfficeName;
            contractViewModel.ContractNumber = getContractNumber;
            contractViewModel.KeyPersonnel = KeyPersonnel;
            contractViewModel.KeyPersonnel.CompanyPresidentName = CompanyPresident;
            contractViewModel.KeyPersonnel.RegionalManagerName = RegionalManager;
            contractViewModel.CustomerInformation = CustomerInformation;
            contractViewModel.ContractGuid = id;
            contractViewModel.FinancialInformation = FinancialInformation;
            contractViewModel.IsActive = BaseModel.IsActive;
            contractViewModel.CreatedOn = BaseModel.CreatedOn;

            return contractViewModel;
        }

        public User GetUsersDataByUserId(Guid id)
        {
            var usersQuery = string.Format($@"
               select * from Users
                where userGuid = '{id}' 
                order by Displayname asc");
            var usersData = _context.Connection.QuerySingle<User>(usersQuery);
            return usersData;
        }

        public CustomerContact GetContactsDataByContactId(Guid id)
        {
            var contactQuery = string.Format($@"
               select * from CustomerContact
                where ContactGuid = '{id}' 
                order by Firstname asc");
            var contactData = _context.Connection.QuerySingle<CustomerContact>(contactQuery);
            return contactData;
        }

        public Customer GetCustomersDataByCustomerId(Guid id)
        {
            var customerQuery = string.Format($@"
               select * from Customer
                where CustomerGuid = '{id}' 
                order by CustomerName asc");
            var customerData = _context.Connection.QuerySingle<Customer>(customerQuery);
            return customerData;
        }

        //ContractQuestionaires
        public int AddContractQuestionaires(ContractQuestionaire model)
        {
            string insertQuery = $@"INSERT INTO [dbo].[ContractQuestionaire]
                                                                   (
                                                                        ContractQuestionaireGuid	   ,
                                                                        ContractGuid				   ,
                                                                        IsFARclause					   ,
                                                                        IsReportExecCompensation	   ,
                                                                        Report_LastReportDate		   ,
                                                                        Report_NextReportDate		   ,
                                                                        IsGSAschedulesale			   ,
                                                                        GSA_LastReportDate			   ,
                                                                        GSA_NextReportDate			   ,
                                                                        IsSBsubcontract			       ,
                                                                        SB_LastReportDate			   ,
                                                                        SB_NextReportDate			   ,
                                                                        IsGovtFurnishedProperty		   ,
                                                                        IsServiceContractReport		   ,
                                                                        IsGQAC					       ,
                                                                        GQAC_LastReportDate		       ,
                                                                        GQAC_NextReportDate			   ,
                                                                        IsCPARS					       ,
                                                                        CPARS_LastReportDate		   ,
                                                                        CPARS_NextReportDate		   ,
                                                                        IsWarranties				   ,
                                                                        IsStandardIndustryProvision	   ,
                                                                        WarrantyProvisionDescription   ,
                                                                        CreatedOn				       ,
                                                                        UpdatedOn		               ,
                                                                        CreatedBy                      ,
                                                                        UpdatedBy				       ,
                                                                        IsDeleted				       ,
                                                                        IsActive				       
                                                                    )
                                  VALUES (
                                                                        '{model.ContractQuestionaireGuid                                    }'          ,
                                                                        '{model.ContractGuid                                                }'	        ,
                                                                        '{model.IsFARclause                                                 }'          ,
                                                                        '{model.IsReportExecCompensation                                    }'          ,
                                                                         NULLIF('{model.Report_LastReportDate                               }' , '')    ,
                                                                         NULLIF('{model.Report_NextReportDate                               }' , '')    ,
                                                                        '{model.IsGSAschedulesale                                           }'          ,
                                                                         NULLIF('{model.GSA_LastReportDate                                  }' , '')    ,
                                                                         NULLIF('{model.GSA_NextReportDate                                  }' , '')    ,
                                                                        '{model.IsSBsubcontract                                             }'          ,
                                                                         NULLIF('{model.SB_LastReportDate                                   }' , '')    ,
                                                                         NULLIF('{model.SB_NextReportDate                                   }' , '')    ,
                                                                        '{model.IsGovtFurnishedProperty                                     }'          ,
                                                                        '{model.IsServiceContractReport                                     }'          ,
                                                                        '{model.IsGQAC                                                      }'          ,
                                                                         NULLIF('{model.GQAC_LastReportDate                                 }' , '')    ,
                                                                         NULLIF('{model.GQAC_NextReportDate                                 }' , '')    ,
                                                                        '{model.IsCPARS                                                     }'          ,
                                                                         NULLIF('{model.CPARS_LastReportDate                                }' , '')    ,
                                                                         NULLIF('{model.CPARS_NextReportDate                                }' , '')   ,
                                                                        '{model.IsWarranties                                                }'          ,
                                                                        '{model.IsStandardIndustryProvision                                 }'          ,
                                                                        '{model.WarrantyProvisionDescription                                }'          ,
                                                                        '{model.CreatedOn                                                   }'          ,
                                                                        '{model.UpdatedOn                                                   }'          ,
                                                                        '{model.CreatedBy                                                   }'          ,
                                                                        '{model.UpdatedBy                                                   }'          ,
                                                                        '{model.IsDeleted                                                   }'          ,
                                                                        '{model.IsActive                                                    }'   
                                                                )";
            return _context.Connection.Execute(insertQuery);
        }

        public ContractQuestionaire GetContractQuestionaireById(Guid id)
        {
            string sql = $"SELECT * FROM ContractQuestionaire WHERE ContractQuestionaireGuid = @ContractQuestionaireGuid;";
            var result = _context.Connection.QuerySingle<ContractQuestionaire>(sql, new { ContractQuestionaireGuid = id });
            return result;
        }

        public int UpdateContractQuestionairesById(ContractQuestionaire ContractQuestionaire)
        {
            string updateQuery = $@"Update ContractQuestionaire       set 
                                                                    IsFARclause					 =          '{ContractQuestionaire.IsFARclause                           }'      ,
                                                                    IsReportExecCompensation	 =          '{ContractQuestionaire.IsReportExecCompensation              }'      ,
                                                                    Report_LastReportDate		 =           NULLIF('{ContractQuestionaire.Report_LastReportDate         }' , '')    ,
                                                                    Report_NextReportDate		 =           NULLIF('{ContractQuestionaire.Report_NextReportDate                               }' , '')    ,
                                                                    IsGSAschedulesale			 =          '{ContractQuestionaire.IsGSAschedulesale                     }'      ,
                                                                    GSA_LastReportDate			 =             NULLIF('{ContractQuestionaire.GSA_LastReportDate                                  }' , '')    ,
                                                                    GSA_NextReportDate			 =           NULLIF('{ContractQuestionaire.GSA_NextReportDate                                  }' , '')    ,
                                                                    IsSBsubcontract			     =          '{ContractQuestionaire.IsSBsubcontract                       }'      ,
                                                                    SB_LastReportDate			 =          NULLIF('{ContractQuestionaire.SB_LastReportDate                                   }' , '')    ,
                                                                    SB_NextReportDate			 =          NULLIF('{ContractQuestionaire.SB_NextReportDate                                   }' , '')    ,
                                                                    IsGovtFurnishedProperty		 =          '{ContractQuestionaire.IsGovtFurnishedProperty               }'      ,
                                                                    IsServiceContractReport		 =          '{ContractQuestionaire.IsServiceContractReport               }'      ,
                                                                    IsGQAC					     =          '{ContractQuestionaire.IsGQAC                                }'      ,
                                                                    GQAC_LastReportDate		     =           NULLIF('{ContractQuestionaire.GQAC_LastReportDate                                 }' , '')    ,
                                                                    GQAC_NextReportDate			 =          NULLIF('{ContractQuestionaire.GQAC_NextReportDate                                 }' , '')    ,
                                                                    IsCPARS					     =          '{ContractQuestionaire.IsCPARS                               }'      ,
                                                                    CPARS_LastReportDate		 =           NULLIF('{ContractQuestionaire.CPARS_LastReportDate                                }' , '')    ,                                                                   
                                                                    CPARS_NextReportDate		 =           NULLIF('{ContractQuestionaire.CPARS_NextReportDate                                }' , '')   ,
                                                                    IsWarranties				 =          '{ContractQuestionaire.IsWarranties                      }'      ,
                                                                    IsStandardIndustryProvision	 =          '{ContractQuestionaire.IsStandardIndustryProvision           }'      ,
                                                                    WarrantyProvisionDescription =          '{ContractQuestionaire.WarrantyProvisionDescription         }'      ,
                                                                    UpdatedOn		             =          '{ContractQuestionaire.UpdatedOn                             }'      ,
                                                                    UpdatedBy				     =          '{ContractQuestionaire.UpdatedBy                             }'      
                                                                      where ContractQuestionaireGuid = '{ContractQuestionaire.ContractQuestionaireGuid}' ";
            return _context.Connection.Execute(updateQuery);
        }

        //ContractWBS
        public int AddContractWBS(ContractWBS model)
        {
            string insertQuery = $@"INSERT INTO ContractWBS (
                                                    ContractWBSGuid,
                                                    ContractGuid,
                                                    UploadFileName,
                                                    IsCsv,
                                                    CreatedOn,
                                                    UpdatedOn,
                                                    CreatedBy,
                                                    UpdatedBy,
                                                    IsDeleted,
                                                    IsActive
                                                            )
                                    VALUES (
                                                    '{model.ContractWBSGuid}',
                                                    '{model.ContractGuid}',
                                                    '{model.UploadFileName}',
                                                    '{model.IsCsv}',
                                                    '{model.CreatedOn}',
                                                    '{model.UpdatedOn}',
                                                    '{model.CreatedBy}',
                                                    '{model.UpdatedBy}',
                                                    '{model.IsDeleted}',
                                                    '{model.IsActive}'
                                                            )";
            return _context.Connection.Execute(insertQuery);
        }

        public int UpdateContractWBS(ContractWBS contractWBS)
        {
            string insertQuery = $@"UPDATE ContractWBS set 
                                                    UpdatedOn              =           '{contractWBS.UpdatedOn}',
                                                    UpdatedBy              =           '{contractWBS.UpdatedBy}',
                                                    IsDeleted              =           '{contractWBS.IsDeleted}',
                                                    IsActive               =           '{contractWBS.IsActive}'
                                                     where ContractWBSGuid = '{contractWBS.ContractWBSGuid}' ";
            return _context.Connection.Execute(insertQuery);
        }

        public int DeleteContractWBS(Guid id)
        {
            string deleteQuery = $@"UPDATE ContractWBS SET IsDeleted = 1 WHERE ContractGuid = '{id}';";
            return _context.Connection.Execute(deleteQuery);
        }

        public ContractWBS GetContractWBSById(Guid id)
        {
            string sql = $"SELECT * FROM ContractWBS WHERE ContractGuid = @ContractGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<ContractWBS>(sql, new { ContractGuid = id });
            return result;
        }

        public IEnumerable<ContractWBS> GetWBSList(string path)
        {
            List<ContractWBS> data = new List<ContractWBS>();
            var lines = System.IO.File.ReadAllLines(path);
            int sn = 0;
            foreach (string item in lines.Skip(1))
            {
                var values = item.Split(',');
                sn = sn + 1;
                var wbscode = values[0].Replace('"', ' ').Trim();
                var description = values[1].Replace('"', ' ').Trim();
                var value = values[2].Replace('"', ' ').Trim();
                var contracttype = values[3].Replace('"', ' ').Trim();
                var invoiceatthislevel = values[4].Replace('"', ' ').Trim();
                data.Add(new ContractWBS { WBSCode = wbscode, Description = description, Value = value, ContractType = contracttype, InvoiceAtThisLevel = invoiceatthislevel, sn = sn });
            }
            return data;
        }

        //EmployeeBillingRates
        public int AddEmployeeBillingRates(EmployeeBillingRates model)
        {
            string insertQuery = $@"INSERT INTO [dbo].[EmployeeBillingRates]
                                                                   (
                                                                        BillingRateGuid	                ,
                                                                        ContractGuid				    ,
                                                                        UploadFileName					,
                                                                        IsCsv                           ,
                                                                        CreatedOn				        ,
                                                                        UpdatedOn		                ,
                                                                        CreatedBy                       ,
                                                                        UpdatedBy				        ,
                                                                        IsDeleted				        ,
                                                                        IsActive				       
                                                                    )
                                  VALUES (
                                                                        '{model.BillingRateGuid     }',
                                                                        '{model.ContractGuid        }',
                                                                        '{model.UploadFileName      }',
                                                                        '{model.IsCsv               }',
                                                                        '{model.CreatedOn           }',
                                                                        '{model.UpdatedOn           }',
                                                                        '{model.CreatedBy           }',
                                                                        '{model.UpdatedBy           }',
                                                                        '{model.IsDeleted           }',
                                                                        '{model.IsActive            }'   
                                                                )";
            return _context.Connection.Execute(insertQuery);
        }

        public int UpdateEmployeeBillingRates(EmployeeBillingRates employeeBillingRates)
        {
            string insertQuery = $@"UPDATE EmployeeBillingRates set 
                                                    UpdatedOn              =           '{employeeBillingRates.UpdatedOn}',
                                                    UpdatedBy              =           '{employeeBillingRates.UpdatedBy}',
                                                    IsDeleted              =           '{employeeBillingRates.IsDeleted}',
                                                    IsActive               =           '{employeeBillingRates.IsActive}'
                                                     where BillingRateGuid = '{employeeBillingRates.BillingRateGuid}' ";
            return _context.Connection.Execute(insertQuery);
        }

        public int DeleteEmployeeBillingRates(Guid id)
        {
            string deleteQuery = $@"UPDATE EmployeeBillingRates set IsDeleted = 1 WHERE ContractGuid = '{id}';";
            return _context.Connection.Execute(deleteQuery);
        }

        public EmployeeBillingRates GetEmployeeBillingRatesById(Guid id)
        {
            string sql = $"SELECT * FROM EmployeeBillingRates WHERE ContractGuid = @ContractGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<EmployeeBillingRates>(sql, new { ContractGuid = id });
            return result;
        }

        public IEnumerable<EmployeeBillingRates> GetBillingRates(string path)
        {
            List<EmployeeBillingRates> data = new List<EmployeeBillingRates>();
            var lines = System.IO.File.ReadAllLines(path);
            int sn = 0;
            foreach (string item in lines.Skip(1))
            {
                var values = item.Split(',');
                sn = sn + 1;
                var laborcode = values[0].Replace('"', ' ').Trim();
                var employeename = values[1].Replace('"', ' ').Trim();
                var rate = values[2].Replace('"', ' ').Trim();
                decimal ratedecimal = decimal.Parse(rate);
                var startdate = values[3].Replace('"', ' ').Trim();
                var enddate = values[4].Replace('"', ' ').Trim();
                data.Add(new EmployeeBillingRates { LaborCode = laborcode, EmployeeName = employeename, Rate = ratedecimal, StartDate = startdate, EndDate = enddate, sn = sn });
            }
            return data;
        }

        //LaborCategoryRates
        public int AddLaborCategoryRates(LaborCategoryRates model)
        {
            string insertQuery = $@"INSERT INTO [dbo].[LaborCategoryRates]
                                                                   (
                                                                        CategoryRateGuid	            ,
                                                                        ContractGuid				    ,
                                                                        UploadFileName					,
                                                                        IsCsv				            ,
                                                                        CreatedOn				        ,
                                                                        UpdatedOn		                ,
                                                                        CreatedBy                       ,
                                                                        UpdatedBy				        ,
                                                                        IsDeleted				        ,
                                                                        IsActive				       
                                                                    )
                                  VALUES (
                                                                        '{model.CategoryRateGuid    }',
                                                                        '{model.ContractGuid        }',
                                                                        '{model.UploadFileName      }',
                                                                        '{model.IsCsv               }',
                                                                        '{model.CreatedOn           }',
                                                                        '{model.UpdatedOn           }',
                                                                        '{model.CreatedBy           }',
                                                                        '{model.UpdatedBy           }',
                                                                        '{model.IsDeleted           }',
                                                                        '{model.IsActive            }'   
                                                                )";
            return _context.Connection.Execute(insertQuery);
        }

        public int UpdateLaborCategoryRates(LaborCategoryRates laborCategoryRates)
        {
            string insertQuery = $@"UPDATE LaborCategoryRates set 
                                                    UpdatedOn              =           '{laborCategoryRates.UpdatedOn}',
                                                    UpdatedBy              =           '{laborCategoryRates.UpdatedBy}',
                                                    IsDeleted              =           '{laborCategoryRates.IsDeleted}',
                                                    IsActive               =           '{laborCategoryRates.IsActive}'
                                                     where CategoryRateGuid = '{laborCategoryRates.CategoryRateGuid}' ";
            return _context.Connection.Execute(insertQuery);
        }

        public int DeleteLaborCategoryRates(Guid id)
        {
            string deleteQuery = $@"UPDATE LaborCategoryRates set IsDeleted = 1 WHERE ContractGuid = '{id}';";
            return _context.Connection.Execute(deleteQuery);
        }

        public LaborCategoryRates GetLaborCategoryRatesById(Guid id)
        {
            string sql = $"SELECT * FROM LaborCategoryRates WHERE ContractGuid = @ContractGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<LaborCategoryRates>(sql, new { ContractGuid = id });
            return result;
        }

        public IEnumerable<LaborCategoryRates> GetCategoryRates(string path)
        {
            List<LaborCategoryRates> data = new List<LaborCategoryRates>();
            var lines = System.IO.File.ReadAllLines(path);
            int sn = 0;
            foreach (string item in lines.Skip(1))
            {
                var values = item.Split(',');
                sn = sn + 1;
                var subcontractor = values[0].Replace('"', ' ').Trim();
                var laborcode = values[1].Replace('"', ' ').Trim();
                var employeename = values[2].Replace('"', ' ').Trim();
                var rate = values[3].Replace('"', ' ').Trim();
                decimal ratedecimal = decimal.Parse(rate);
                var startdate = values[4].Replace('"', ' ').Trim();
                var enddate = values[5].Replace('"', ' ').Trim();
                data.Add(new LaborCategoryRates { SubContractor = subcontractor, LaborCode = laborcode, EmployeeName = employeename, Rate = ratedecimal, StartDate = startdate, EndDate = enddate, sn = sn });

            }
            return data;
        }

        public ContractDetailsViewModel GetInfoById(Guid id)
        {
            ContractDetailsViewModel contractdetailsViewModel = new ContractDetailsViewModel();
            string CompanyCodeSql = $"select SUBSTRING(OrgID.Name,1,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var CompanyCode = _context.Connection.Query<string>(CompanyCodeSql).FirstOrDefault();

            string CompanyGuidSql = $"select Company.President from Company where CompanyCode = '{CompanyCode}' and IsDeleted = 0";
            var CompanyGuid = _context.Connection.Query<Guid>(CompanyGuidSql).FirstOrDefault();

            string CompanyPresidentSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = '{CompanyGuid}'";
            var CompanyPresident = _context.Connection.Query<string>(CompanyPresidentSql).FirstOrDefault();

            string CompanyNameSql = $"select Company.CompanyName from Company where CompanyCode = '{CompanyCode}' and IsDeleted = 0";
            var CompanyName = _context.Connection.Query<String>(CompanyNameSql).FirstOrDefault();

            string RegionCodeSql = $@"select SUBSTRING(OrgID.Name,4,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var RegionCode = _context.Connection.Query<string>(RegionCodeSql).FirstOrDefault();

            string RegionGuidSql = $@"select Region.RegionalManager from Region where RegionCode = '{RegionCode}' and IsDeleted = 0";
            var RegionGuid = _context.Connection.Query<Guid>(RegionGuidSql).FirstOrDefault();

            string RegionalManagerSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = '{RegionGuid}'";
            var RegionalManager = _context.Connection.Query<string>(RegionalManagerSql).FirstOrDefault();

            string RegionNameSql = $"select Region.RegionName from Region where RegionCode = '{RegionCode}' and IsDeleted = 0";
            var RegionName = _context.Connection.Query<String>(RegionNameSql).FirstOrDefault();

            string OfficeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) from Contract left join OrgID on Contract.ORGID = OrgID.OrgIDGuid WHERE ContractGuid = '{id}';";
            var OfficeCode = _context.Connection.Query<string>(RegionCodeSql).FirstOrDefault();

            string OfficeNameSql = $"select Office.OfficeName from Office where OfficeCode = '{OfficeCode}' and IsDeleted = 0";
            var OfficeName = _context.Connection.Query<String>(OfficeNameSql).FirstOrDefault();

            string questionary = $" select questionaire.ContractQuestionaireGuid,users.Displayname,questionaire.UpdatedOn from ContractQuestionaire questionaire " +
                               $"left join Users users on users.UserGuid = questionaire.UpdatedBy where questionaire.ContractGuid = '{id}' and Isdeleted = 0";
            var ContractQuestionaire = _context.Connection.Query<ContractQuestionaire>(questionary).FirstOrDefault();

            string billingrates = $"select * from EmployeeBillingRates where ContractGuid = '{id}' and Isdeleted = 0";
            var employeeeBillingRates = _context.Connection.Query<EmployeeBillingRates>(billingrates).FirstOrDefault();

            string contractWBSSql = $"select * from ContractWBS where ContractGuid = '{id}' and Isdeleted = 0";
            var contractWBS = _context.Connection.Query<ContractWBS>(contractWBSSql).FirstOrDefault();

            string categoryrates = $"select * from LaborCategoryRates where ContractGuid = '{id}' and Isdeleted = 0";
            var laborCategoryRates = _context.Connection.Query<LaborCategoryRates>(categoryrates).FirstOrDefault();

            string revenue = $"select * from RevenueRecognization where ContractGuid = '{id}' and Isdeleted = 0";
            var RevenueRecognitionModel = _context.Connection.Query<Northwind.Core.Entities.RevenueRecognition>(revenue).FirstOrDefault();

            string requestForm = $"select * from JobRequest where ContractGuid = '{id}' and Isdeleted = 0";
            var requestFormModel = _context.Connection.Query<Core.Entities.JobRequest>(requestForm).FirstOrDefault();

            string BasicContractSql = @"select
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
            var BasicContractInfo = _context.Connection.QuerySingle<BasicContractInfoViewModel>(BasicContractSql, new { ContractGuid = id });

            string KeyPersonnelSql = @"select
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
            var KeyPersonnel = _context.Connection.QuerySingle<KeyPersonnelViewModel>(KeyPersonnelSql, new { ContractGuid = id });

            string CustomerInfoSql = @"select
                            AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							Office_ContractRepresentative.ContactGuid Office_ContractRepresentative,
							(Office_ContractRepresentative.FirstName + ' ' + Office_ContractRepresentative.MiddleName + ' ' + Office_ContractRepresentative.LastName) Office_ContractRepresentativeName,
							Office_ContractTechnicalRepresent.ContactGuid Office_ContractTechnicalRepresent,
							(Office_ContractTechnicalRepresent.FirstName + ' ' + Office_ContractTechnicalRepresent.MiddleName + ' ' + Office_ContractTechnicalRepresent.LastName) Office_ContractTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
                            FundingOffice_ContractRepresentative.ContactGuid FundingOffice_ContractRepresentative,
                            (FundingOffice_ContractRepresentative.FirstName + ' ' + FundingOffice_ContractRepresentative.MiddleName + ' ' + FundingOffice_ContractRepresentative.LastName) FundingOffice_ContractRepresentativeName,
							FundingOffice_ContractTechnicalRepresent.ContactGuid FundingOffice_ContractTechnicalRepresent,
							(FundingOffice_ContractTechnicalRepresent.FirstName + ' ' + FundingOffice_ContractTechnicalRepresent.MiddleName + ' ' + FundingOffice_ContractTechnicalRepresent.LastName) FundingOffice_ContractTechnicalRepresentName
                            
                            from Contract

                            left join
						    Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
						    left join
                            CustomerContact Office_ContractRepresentative on Contract.Office_ContractRepresentative = Office_ContractRepresentative.ContactGuid
						    left join
						    CustomerContact Office_ContractTechnicalRepresent on  Contract.Office_ContractTechnicalRepresent = Office_ContractTechnicalRepresent.ContactGuid

                            left join
						    Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
						    left join
						    CustomerContact FundingOffice_ContractRepresentative on Contract.FundingOffice_ContractRepresentative = FundingOffice_ContractRepresentative.ContactGuid
						    left join
						    CustomerContact FundingOffice_ContractTechnicalRepresent on Contract.FundingOffice_ContractTechnicalRepresent = FundingOffice_ContractTechnicalRepresent.ContactGuid

                            WHERE ContractGuid =  @ContractGuid;";
            var CustomerInformation = _context.Connection.QuerySingle<CustomerInformationViewModel>(CustomerInfoSql, new { ContractGuid = id });

            string FinancialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformance_Percent,
							Contract.SBA,
							Competition.Name CompetitionType,
                            Contract.Competition,
							ContractType.Name ContractTypeName,
                            Contract.ContractType,
							Contract.OverHead,
							Contract.G_A_Percent,
							Contract.Fee_Percent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAward_Amount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							AppWageDetermine_DavisBaconAct.Name AppWageDetermine_DavisBaconActType,
							Contract.AppWageDetermine_DavisBaconAct,
							AppWageDetermine_ServiceContractAct.Name AppWageDetermine_ServiceContractActType,
							Contract.AppWageDetermine_ServiceContractAct,
							Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEAC_Percent,
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
						    ResourceAttributeValue AppWageDetermine_DavisBaconAct on Contract.AppWageDetermine_DavisBaconAct = AppWageDetermine_DavisBaconAct.Value
						    left join
						    ResourceAttributeValue AppWageDetermine_ServiceContractAct on Contract.AppWageDetermine_ServiceContractAct = AppWageDetermine_ServiceContractAct.Value
 
                            WHERE ContractGuid =  @ContractGuid;";
            var FinancialInformation = _context.Connection.QuerySingle<FinancialInformationViewModel>(FinancialInfoSql, new { ContractGuid = id });

            string BaseModelSql = @"select Contract.IsActive from Contract WHERE ContractGuid =  @ContractGuid;";
            var BaseModel = _context.Connection.QuerySingle<BaseModel>(BaseModelSql, new { ContractGuid = id });

            // to fetch States name through state id array..
            if (!string.IsNullOrEmpty(BasicContractInfo.PlaceOfPerformanceSelectedIds))
            {
                var stateIdArr = BasicContractInfo.PlaceOfPerformanceSelectedIds.Split(',');
                var stateIdArrWithStringCote = stateIdArr.Select(x => string.Format("'" + x + "'"));
                var formatQuery = string.Join(",", stateIdArrWithStringCote);
                var stateQuery = $"select StateName from State where StateId in ({formatQuery})";
                var stateNameArr = _context.Connection.Query<string>(stateQuery); ;
                BasicContractInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);
            }

            contractdetailsViewModel.BasicContractInfo = BasicContractInfo;
            contractdetailsViewModel.BasicContractInfo.CompanyName = CompanyName;
            contractdetailsViewModel.BasicContractInfo.RegionName = RegionName;
            contractdetailsViewModel.BasicContractInfo.OfficeName = OfficeName;
            contractdetailsViewModel.KeyPersonnel = KeyPersonnel;
            contractdetailsViewModel.KeyPersonnel.CompanyPresidentName = CompanyPresident;
            contractdetailsViewModel.KeyPersonnel.RegionalManagerName = RegionalManager;
            contractdetailsViewModel.CustomerInformation = CustomerInformation;
            contractdetailsViewModel.ContractGuid = id;
            contractdetailsViewModel.FinancialInformation = FinancialInformation;
            contractdetailsViewModel.ContractQuestionaire = ContractQuestionaire;
            contractdetailsViewModel.employeeBillingRatesViewModel = employeeeBillingRates;
            contractdetailsViewModel.LaborCategoryRates = laborCategoryRates;
            contractdetailsViewModel.revenueRecognitionModel = RevenueRecognitionModel;
            contractdetailsViewModel.ContractWBS = contractWBS;
            contractdetailsViewModel.JobRequest = requestFormModel;
            contractdetailsViewModel.IsActive = BaseModel.IsActive;

            return contractdetailsViewModel;
        }

        public bool IsExistContractNumber(string contractNumber)
        {
            string contactNumberQuery = $@"select contractNumber from  Contract 
                                              where  IsDeleted   = 0
                                                 and contractNumber = @contractNumber ";
            var result = _context.Connection.QueryFirstOrDefault<string>(contactNumberQuery, new { contractNumber = contractNumber });

            return !string.IsNullOrEmpty(result) ? true : false;
        }
        public bool IsExistProjectNumber(string projectNumber)
        {
            string projectNumberQuery = $@"select projectNumber from  Contract 
                                              where  IsDeleted   = 0
                                                 and projectNumber = @projectNumber ";
            var result = _context.Connection.QueryFirstOrDefault<string>(projectNumberQuery, new { projectNumber = projectNumber });

            return !string.IsNullOrEmpty(result) ? true : false;
        }

        public Core.Entities.Contract GetContractByContractGuid(Guid contractGuid)
        {
            string sql = $"SELECT * FROM Contract WHERE ContractGuid = @ContractGuid;";
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
                      ,P.[Office_ContractRepresentative]
                      ,P.[Office_ContractTechnicalRepresent]
                      ,P.[FundingAgencyOffice]
                      ,P.[SetAside]
                      ,P.[SelfPerformance_Percent]
                      ,P.[SBA]
                      ,P.[Competition]
                      ,P.[ContractType]
                      ,P.[OverHead]
                      ,P.[G_A_Percent]
                      ,P.[Fee_Percent]
                      ,P.[Currency]
                      ,P.[BlueSkyAward_Amount]
                      ,P.[AwardAmount]
                      ,P.[FundingAmount]
                      ,P.[BillingAddress]
                      ,P.[BillingFrequency]
                      ,P.[InvoiceSubmissionMethod]
                      ,InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName
                      ,P.[PaymentTerms]
                      ,PaymentTerms.Name PaymentTermsName
                      ,P.[AppWageDetermine_DavisBaconAct]
                      ,P.[BillingFormula]
                      ,P.[RevenueFormula]
                      ,P.[RevenueRecognitionEAC_Percent]
                      ,P.[OHonsite]
                      ,P.[OHoffsite]
                      ,P.[CreatedOn]
                      ,P.[UpdatedOn]
                      ,P.[CreatedBy]
                      ,P.[UpdatedBy]
                      ,P.[IsActive]
                      ,P.[IsDeleted]
                      ,P.[Description]
                      ,P.[AppWageDetermine_ServiceContractAct]
                      ,P.[FundingOffice_ContractRepresentative]
                      ,'' FundingOffice_ContractRepresentativeName
                      ,P.[FundingOffice_ContractTechnicalRepresent]
                      ,'' FundingOffice_ContractTechnicalRepresentName
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
				on p.Parent_ContractGuid  = c.ContractGuid and c.IsIDIQContract = 1

                  WHERE P.[IsDeleted] = 0
            ";

            sql += $" AND P.Parent_ContractGuid = @ContractGuid ";

            return _context.Connection.Query<ProjectForList>(sql, new { ContractGuid = contractGuid });
        }

        public IEnumerable<ContractForList> GetContract(string searchValue, int pageSize, int skip, int take, string orderBy, string dir)
        {
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
            if (searchValue != "")
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
                  ,C.[Office_ContractRepresentative]
                  ,C.[Office_ContractTechnicalRepresent]
                  ,C.[FundingAgencyOffice]
                  ,C.[SetAside]
                  ,C.[SelfPerformance_Percent]
                  ,C.[SBA]
                  ,C.[Competition]
                  ,C.[ContractType]
                  ,C.[OverHead]
                  ,C.[G_A_Percent]
                  ,C.[Fee_Percent]
                  ,C.[Currency]
                  ,C.[BlueSkyAward_Amount]
                  ,C.[AwardAmount]
                  ,C.[FundingAmount]
                  ,C.[BillingAddress]
                  ,C.[BillingFrequency]
                  ,C.[InvoiceSubmissionMethod]
                  ,C.[PaymentTerms]
                  ,C.[AppWageDetermine_DavisBaconAct]
                  ,C.[BillingFormula]
                  ,C.[RevenueFormula]
                  ,C.[RevenueRecognitionEAC_Percent]
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
                  ,C.[AppWageDetermine_ServiceContractAct]
                  ,C.[FundingOffice_ContractRepresentative]
                  ,C.[FundingOffice_ContractTechnicalRepresent]
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
                    AND C.Parent_ContractGuid is null
                 ";
            sql += $"{ where }";
            sql += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<ContractForList>(sql, new { searchValue = searchString });
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
            string Contractnumbersql = $"select ContractNumber from Contract WHERE ContractGuid = '{id}';";
            var Contractnumber = _context.Connection.QueryFirstOrDefault<string>(Contractnumbersql);
            return Contractnumber;
        }

        public Guid? GetContractIdByProjectId(Guid projectGuid)
        {
            var contractGuidQuery = string.Format($@"
               select contractGuid from contract 
                         where Contract.IsDeleted = 0 
                           and Contract.ContractGuid ='{projectGuid}'");
            var contractGuid = _context.Connection.QueryFirstOrDefault<Guid>(contractGuidQuery);
            return contractGuid;
        }

        public int GetTotalCountProjectByContractId(Guid contractGuid)
        {
            var getCount = string.Format($@"    
               select count(1) from contract 
               where contract.IsDeleted = 0 and contract.Parent_ContractGuid ='{contractGuid}'");
            var totalProjectForContract = _context.Connection.QuerySingle<int>(getCount);
            return totalProjectForContract;
        }
        public Guid? GetPreviousProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
               select top 1 project.contractGuid 
                    from contract project
                         where project.IsDeleted = 0 
                           and project.Parent_ContractGuid ='{contractGuid}' 
                           and projectCounter < {currentProjectCounter}
                           order by projectCounter desc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery);
            return getPreviousProjectGuid;
        }
        public Guid? GetNextProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
               select top 1 project.contractGuid 
                    from contract project
                         where project.IsDeleted = 0 
                           and project.Parent_ContractGuid ='{contractGuid}' 
                           and projectCounter > {currentProjectCounter}
                           order by projectCounter asc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery);
            return getPreviousProjectGuid;
        }

        public int HasChild(Guid projectGuid)
        {
            var getCount = string.Format($@"
               select count(1) from projectModification where isDeleted = 0 and projectGuid ='{projectGuid}'");
            var totalProjectModificaton = _context.Connection.QuerySingle<int>(getCount);
            return totalProjectModificaton;
        }
        public IEnumerable<Northwind.Core.Entities.Contract> GetAllProject(Guid ContractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();

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
                conditionalQuery.Append($"and Contract.ContractNumber like '%{searchValue}%'");
            }

            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                        (SELECT ROW_NUMBER() OVER (ORDER BY {orderingQuery}) AS RowNum, 
                                                                                        ContractGuid,
                                                                                        Contract.ProjectNumber,
                                                                                        Contract.IsActive,
                                                                                        Contract.ORGID,
                                                                                        Contract.AwardAmount AwardAmount,
                                                                                        Contract.POPStart Periodofperformancestart,
                                                                                        Contract.POPEnd Periodofperformanceend,
                                                                                        Contract.UpdatedOn,
                                                                                        Contract.ContractNumber,
                                                                                        Contract.ContractTitle
                                                                                        from Contract
                                                                                        where Contract.IsDeleted = 0
                                                                                        {conditionalQuery} 
                                                                                        and contract.Parent_ContractGuid= '{ContractGuid}'
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > {skip} 
                                            AND RowNum <= {pageSize + skip}  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<Northwind.Core.Entities.Contract>(pagingQuery);
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
            string parentGuidsql = $"select Parent_ContractGuid from Contract WHERE ContractGuid = '{id}';";
            var ParentContractGuid = _context.Connection.QueryFirstOrDefault<Guid?>(parentGuidsql);
            string Contractnumbersql = $"select ContractNumber from Contract WHERE ContractGuid = '{ParentContractGuid}';";
            var Contractnumber = _context.Connection.QueryFirstOrDefault<string>(Contractnumbersql);
            return Contractnumber;
        }

        public string GetProjectNumberById(Guid id)
        {
            string projectnumbersql = $"select ProjectNumber from Contract WHERE ContractGuid = '{id}';";
            var projectNumber = _context.Connection.QueryFirstOrDefault<string>(projectnumbersql);
            return projectNumber;
        }
    }
}

