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
using Attribute = Northwind.Core.Entities.Attribute;
using Northwind.Core.Entities.ContractRefactor;
using static Northwind.Core.Entities.RevenueRecognition;

namespace Northwind.Infrastructure.Data.RevenueRecognitionRepo
{
    public class RevenueRecognitionRepository : IRevenueRecognitionRepository
    {
        private readonly IDatabaseContext _context;

        public RevenueRecognitionRepository(IDatabaseContext context)
        {
            _context = context;
        }


        private bool CheckIsExistsContractExtension(List<RevenueContractExtension> contractExtensions, DateTime? ExtensionDate)
        {
            var extensionsdate = ExtensionDate.Value.ToShortDateString();
            var contractExtensionlist = contractExtensions.Where(x => x.ExtensionDate.HasValue && x.IsDeleted == false);
            var isExists = contractExtensionlist.Count(x => x.ExtensionDate.Value.ToShortDateString() == extensionsdate);
            if (isExists > 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckIsExistsObligationPeriod(List<RevenuePerformanceObligation> performanceObligationsList, RevenuePerformanceObligation revenuePerformance)
        {
            var isExists = performanceObligationsList.Count(x => x.RevenueStreamIdentifier == revenuePerformance.RevenueStreamIdentifier && x.IsDeleted == false);
            if (isExists > 0)
            {
                return true;
            }
            return false;
        }

        private RevenueRecognition validateFilledData(Core.Entities.RevenueRecognition RevenueRecognition)
        {
            RevenueRecognition.IdentifyTerminationClause = null;
            RevenueRecognition.WarrantyTerms = null;
            RevenueRecognition.EstimateWarrantyExposure = null;
            RevenueRecognition.PricingExplanation = null;
            RevenueRecognition.Approach = null;
            RevenueRecognition.EachMultipleObligation = null;
            RevenueRecognition.IsDiscountPurchase = false;
            RevenueRecognition.HasMultipleContractObligations = null;
            RevenueRecognition.Step1Note = null;
            RevenueRecognition.IdentityContract = null;
            RevenueRecognition.IsTerminationClauseGovernmentStandard = null;
            RevenueRecognition.IsContractTermExpansion = false;
            RevenueRecognition.Step2Note = null;
            RevenueRecognition.IdentityPerformanceObligation = null;
            RevenueRecognition.IsMultiRevenueStream = false;
            RevenueRecognition.IsRepetativeService = false;
            RevenueRecognition.HasOptionToPurchageAdditionalGoods = false;
            RevenueRecognition.IsNonRefundableAdvancePayment = false;
            RevenueRecognition.HasDiscountProvision = false;
            RevenueRecognition.HasWarrenty = false;
            RevenueRecognition.Step3Note = null;
            RevenueRecognition.ContractType = null;
            RevenueRecognition.IsPricingVariation = false;
            RevenueRecognition.BaseContractPrice = decimal.Zero;
            RevenueRecognition.AdditionalPeriodOption = null;
            RevenueRecognition.Step4Note = null;
            RevenueRecognition.HasLicensingOrIntellectualProperty = false;
            RevenueRecognition.Step5Note = null;
            disableRevenueExtensionAndObligationForValidation(RevenueRecognition.RevenueRecognizationGuid);
            return RevenueRecognition;
        }


        public IEnumerable<Core.Entities.RevenueRecognition> GetAll(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();
            var searchdata = "";
            if (sortField.Equals("isActiveStatus"))
            {
                orderingQuery.Append($"RevenueRecognition.isActive {sortDirection}");  //Ambiguous if not done.. 
            }
            else
            {
                orderingQuery.Append($"{sortField} {sortDirection}");
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchdata = "%" + searchValue + "%";
                conditionalQuery.Append($"and RevenueRecognition.ContractReference like @searchdata");
            }

            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                       (SELECT ROW_NUMBER() OVER (ORDER BY RevenueRecognition.ModificationNumber asc) AS RowNum, 
                                                                                       RevenueRecognition.RevenueRecognitionGuid					                ,
                                                                                       RevenueRecognition.ContractGuid					                            ,
                                                                                       RevenueRecognition.ModificationNumber						                ,
                                                                                       RevenueRecognition.EnteredDate						                        ,
                                                                                       RevenueRecognition.EffectiveDate				                                ,
                                                                                       RevenueRecognition.ChangeInFundedAmount			                            ,
                                                                                       RevenueRecognition.POPStart						                            ,
                                                                                       RevenueRecognition.POPEnd						                            ,
                                                                                       RevenueRecognition.Notes						                                ,
                                                                                       RevenueRecognition.UpdatedOn						                            ,
                                                                                       RevenueRecognition.IsActive						                            ,
                                                                                       Contract.ContractNumber                           						    ,
                                                                                       Contract.ContractTitle						                                        
                                                                                        
                                                                                       from RevenueRecognition
                                                                                       left join Contract
                                                                                       on RevenueRecognition.ContractGuid = Contract.ContractGuid
                                                                                       where RevenueRecognition.IsDeleted = 0
                                                                                        {conditionalQuery}
                                                                                       and   Contract.ContractGuid = @ContractGuid
                                                                                       
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > {skip} 
                                            AND RowNum <= {pageSize + skip}  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<Core.Entities.RevenueRecognition>(pagingQuery, new { ContractGuid = contractGuid, searchdata = searchdata });
            return pagedData;
        }

        public int TotalRecord(Guid contractGuid)
        {
            string sql = $@"SELECT Count(1) 
                            from RevenueRecognition
                            left join Contract
                            on RevenueRecognition.ContractGuid = Contract.ContractGuid
                            where RevenueRecognition.IsDeleted = 0
                            and   Contract.ContractGuid = @ContractGuid";
            var result = _context.Connection.QuerySingle<int>(sql, new { ContractGuid = contractGuid });
            return result;
        }




        public Core.Entities.RevenueRecognition GetDetailsById(Guid id)
        {
            Core.Entities.RevenueRecognition revenueRecognition = new Core.Entities.RevenueRecognition();
            string sql = @"SELECT * 
                            FROM RevenueRecognization r
                            LEFT JOIN ContractResourceFile c
                            ON c.ResourceGuid = r.ContractGuid and c.Isfile = 1
                            WHERE RevenueRecognizationGuid = @RevenueRecognitionGuid;";
            var revenueDictionary = new Dictionary<Guid, Core.Entities.RevenueRecognition>();
            var revenueObj = _context.Connection.Query<Core.Entities.RevenueRecognition, ContractResourceFile, Core.Entities.RevenueRecognition>(sql,
                (revenue, contractFile) =>
                {
                    Core.Entities.RevenueRecognition revenueEntity;
                    if (!revenueDictionary.TryGetValue(revenue.RevenueRecognizationGuid, out revenueEntity))
                    {
                        revenueEntity = revenue;
                        revenueEntity.ContractFileList = new List<ContractResourceFile>();
                        revenueDictionary.Add(revenue.RevenueRecognizationGuid, revenueEntity = revenue);
                    }

                    if (contractFile != null && !revenueEntity.ContractFileList.Any(x => x.Keys == contractFile.Keys))
                        revenueEntity.ContractFileList.Add(contractFile);
                    return revenueEntity;
                }, new { RevenueRecognitionGuid = id },
                splitOn: "ContractResourceFileGuid").FirstOrDefault();

            revenueRecognition = revenueObj;
            return revenueRecognition;
        }

        public List<RevenueContractExtension> GetContractExtension(Guid id)
        {
            string sql = @"select *
                            from RevenueContractExtension
                            WHERE RevenueGuid =  @RevenueGuid and IsDeleted=0;";
            var ContractExtension = _context.Connection.Query<Core.Entities.RevenueContractExtension>(sql, new { RevenueGuid = id });
            if (ContractExtension.Count() <= 0)
                ContractExtension = new List<RevenueContractExtension> { new RevenueContractExtension { } };
            return ContractExtension.ToList();
        }

        public List<RevenuePerformanceObligation> GetPerformanceObligation(Guid id)
        {
            string sql = @"select *
                            from RevenuePerformanceObligation
                            WHERE RevenueGuid = @RevenueGuid and IsDeleted = 0;";
            var RevenuePerformanceObligation = _context.Connection.Query<Core.Entities.RevenuePerformanceObligation>(sql, new { RevenueGuid = id });
            if (RevenuePerformanceObligation.Count() <= 0)
                RevenuePerformanceObligation = new List<RevenuePerformanceObligation> { new RevenuePerformanceObligation { } };
            return RevenuePerformanceObligation.ToList();
        }



        public bool AddRevenueWithResourceGuid(RevenueRecognition revenueRecognition)
        {
            string disableQuery = @"Update RevenueRecognization set 
                                               IsActive   = 0
                                               where contractGuid = @contractGuid ";
            _context.Connection.Execute(disableQuery, new { contractGuid = revenueRecognition.ContractGuid });
            string RevenueContractExtension = $@"INSERT INTO RevenueRecognization
                                                                   (
	                                                                        RevenueRecognizationGuid                                     ,
	                                                                        ResourceGuid                                   ,
	                                                                        ContractGuid                                   ,
	                                                                        IsActive                                        ,
	                                                                        CreatedOn                                       ,
	                                                                        UpdatedOn                                       ,
	                                                                        CreatedBy                                       ,
	                                                                        IsRevenueCreated                                ,
	                                                                        IsDeleted                                ,
	                                                                        IsCompleted                                ,
	                                                                        UpdatedBy                                       
                                                                    )
                                                             VALUES (
                                                                        @RevenueRecognizationGuid                    ,
	                                                                    @ResourceGuid                  ,
	                                                                    @ContractGuid                  ,
	                                                                    @IsActive                       ,
	                                                                    @CreatedOn                      ,
	                                                                    @UpdatedOn                      ,
	                                                                    @CreatedBy                      ,
	                                                                    @IsRevenueCreated                      ,
	                                                                    @IsDeleted                      ,
	                                                                    @IsCompleted                      ,
	                                                                    @UpdatedBy                      
                                                                )";
            _context.Connection.Execute(RevenueContractExtension, new
            {
                RevenueRecognizationGuid = revenueRecognition.RevenueRecognizationGuid,
                ResourceGuid = revenueRecognition.ResourceGuid,
                ContractGuid = revenueRecognition.ContractGuid,
                IsActive = true,
                IsCompleted = false,
                IsDeleted = false,
                IsRevenueCreated = false,
                CreatedOn = revenueRecognition.CreatedOn,
                UpdatedOn = revenueRecognition.UpdatedOn,
                CreatedBy = revenueRecognition.CreatedBy,
                UpdatedBy = revenueRecognition.UpdatedBy
            });
            return true;
        }

        public bool AddContractExtension(List<RevenueContractExtension> contractExtensions)
        {
            Guid idRevenue = contractExtensions.FirstOrDefault(x => x.IsDeleted == false).RevenueGuid;
            var contractExtensionlist = GetContractExtension(idRevenue);
            foreach (var item in contractExtensions)
            {
                if (contractExtensionlist != null)
                {
                    if (!CheckIsExistsContractExtension(contractExtensionlist, item.ExtensionDate))
                    {
                        string RevenueContractExtension = $@"INSERT INTO RevenueContractExtension
                                                                   (
	                                                                        RevenueGuid                                     ,
	                                                                        ExtensionDate                                   ,
	                                                                        IsActive                                        ,
	                                                                        CreatedOn                                       ,
	                                                                        UpdatedOn                                       ,
	                                                                        CreatedBy                                       ,
	                                                                        IsDeleted                                       ,
	                                                                        UpdatedBy                                       
                                                                    )
                                                             VALUES (
                                                                        @RevenueGuid                    ,
	                                                                    @ExtensionDate                  ,
	                                                                    @IsActive                       ,
	                                                                    @CreatedOn                      ,
	                                                                    @UpdatedOn                      ,
	                                                                    @CreatedBy                      ,
	                                                                    @IsDeleted                      ,
	                                                                    @UpdatedBy                      
                                                                )";
                        _context.Connection.Execute(RevenueContractExtension, new
                        {
                            RevenueGuid = item.RevenueGuid,
                            ExtensionDate = item.ExtensionDate,
                            IsActive = true,
                            CreatedOn = item.CreatedOn,
                            UpdatedOn = item.UpdatedOn,
                            CreatedBy = item.CreatedBy,
                            IsDeleted = false,
                            UpdatedBy = item.UpdatedBy
                        });
                    }
                }
            }
            return true;
        }

        public bool AddPerformanceObligation(List<RevenuePerformanceObligation> performanceObligations)
        {
            Guid idRevenue = performanceObligations.FirstOrDefault(x => x.RevenueStreamIdentifier != null).RevenueGuid;
            foreach (var item in performanceObligations.Where(x => x.RevenueStreamIdentifier != null))
            {
                var ObligationList = GetPerformanceObligation(idRevenue);
                if (ObligationList != null)
                {
                    if (!CheckIsExistsObligationPeriod(ObligationList, item))
                    {
                        string RevenuePerformanceObligation = $@"INSERT INTO RevenuePerformanceObligation
                                                                   (
	                                                                        RevenueGuid                                     ,
	                                                                        RevenueStreamIdentifier                         ,
	                                                                        RightToPayment                                  ,
	                                                                        RoutineService                                  ,
	                                                                        RevenueOverTimePointInTime                      ,
	                                                                        SatisfiedOverTime                               ,
	                                                                        IsActive                                        ,
	                                                                        IsDeleted                                        ,
	                                                                        CreatedOn                                       ,
	                                                                        UpdatedOn                                       ,
	                                                                        CreatedBy                                       ,
	                                                                        UpdatedBy                                       
                                                                    )
                                                             VALUES (
                                                                        @RevenueGuid                    ,
	                                                                    @RevenueStreamIdentifier        ,
	                                                                    @RightToPayment                 ,
	                                                                    @RoutineService                 ,
	                                                                    @RevenueOverTimePointInTime     ,
	                                                                    @SatisfiedOverTime              ,
	                                                                    @IsActive                       ,
	                                                                    @IsDeleted                       ,
	                                                                    @CreatedOn                      ,
	                                                                    @UpdatedOn                      ,
	                                                                    @CreatedBy                      ,
	                                                                    @UpdatedBy                      
                                                                )";
                        _context.Connection.Execute(RevenuePerformanceObligation, new
                        {
                            RevenueGuid = item.RevenueGuid,
                            RevenueStreamIdentifier = item.RevenueStreamIdentifier,
                            RightToPayment = item.RightToPayment,
                            RoutineService = item.RoutineService,
                            RevenueOverTimePointInTime = item.RevenueOverTimePointInTime,
                            SatisfiedOverTime = item.SatisfiedOverTime,
                            IsActive = item.IsActive,
                            IsDeleted = item.IsDeleted,
                            CreatedOn = item.CreatedOn,
                            UpdatedOn = item.UpdatedOn,
                            CreatedBy = item.CreatedBy,
                            UpdatedBy = item.UpdatedBy
                        });
                    }
                }
            }
            return true;
        }



        public int UpdateRevenueRecognition(Core.Entities.RevenueRecognition RevenueRecognition)
        {
            if (!RevenueRecognition.IsCurrentFiscalYearOfNorthWind)
            { validateFilledData(RevenueRecognition); RevenueRecognition.IsCompleted = true; }
            string updateQuery = $@"Update RevenueRecognization      set 
                                                                     IsModAdministrative                        = @IsModAdministrative                          ,
                                                                     IsASC606                                   = @IsASC606                                     ,
                                                                     IsCurrentFiscalYearOfNorthWind             = @IsCurrentFiscalYearOfNorthWind               ,
                                                                                                                                                                                  
                                                                     DoesScopeContractChange                    = @DoesScopeContractChange                      ,
                                                                     IdentifyTerminationClause                  = @IdentifyTerminationClause                    ,
                                                                     WarrantyTerms                              = @WarrantyTerms                                ,
                                                                     EstimateWarrantyExposure                   = @EstimateWarrantyExposure                     ,
                                                                     PricingExplanation                         = @PricingExplanation                           ,
                                                                     Approach                                   = @Approach                                     ,
                                                                     EachMultipleObligation                     = @EachMultipleObligation                       ,
                                                                     IsDiscountPurchase                         = @IsDiscountPurchase                           ,
                                                                     HasMultipleContractObligations             = @HasMultipleContractObligations               ,
                                                                                                                  
                                                                     Step1Note                                  = @Step1Note                                    ,
                                                                     IdentityContract                           = @IdentityContract                             ,
                                                                     IsTerminationClauseGovernmentStandard      = @IsTerminationClauseGovernmentStandard        ,
                                                                     IsContractTermExpansion                    = @IsContractTermExpansion                      ,
                                                                     Step2Note                                  = @Step2Note                                    ,
                                                                     IdentityPerformanceObligation              = @IdentityPerformanceObligation                ,
                                                                     IsMultiRevenueStream                       = @IsMultiRevenueStream                         ,
                                                                     IsRepetativeService                        = @IsRepetativeService                          ,
                                                                     HasOptionToPurchageAdditionalGoods         = @HasOptionToPurchageAdditionalGoods           ,
                                                                     IsNonRefundableAdvancePayment              = @IsNonRefundableAdvancePayment                ,
                                                                     HasDiscountProvision                       = @HasDiscountProvision                         ,
                                                                     HasWarrenty                                = @HasWarrenty                                  ,
                                                                     Step3Note                                  = @Step3Note                                    ,
                                                                     ContractType                               = @ContractType                                 ,
                                                                     IsPricingVariation                         = @IsPricingVariation                           ,
                                                                     BaseContractPrice                          = @BaseContractPrice                            ,
                                                                     AdditionalPeriodOption                     = @AdditionalPeriodOption                       ,
                                                                     Step4Note                                  = @Step4Note                                    ,
                                                                     HasLicensingOrIntellectualProperty         = @HasLicensingOrIntellectualProperty           ,
                                                                     Step5Note                                  = @Step5Note                                    ,
                                                                     OverviewNotes                              = @OverviewNotes                                ,
                                                                     IsCompleted                                = @IsCompleted                                  ,
                                                                     IsRevenueCreated                           = @IsRevenueCreated                             ,  
                                                                                                                  
                                                                     UpdatedOn						            = @UpdatedOn                                    ,
                                                                     CreatedOn						            = @CreatedOn                                    ,
                                                                     CreatedBy						            = @CreatedBy                                    ,
                                                                     UpdatedBy						            = @UpdatedBy                                    
                                                                     where RevenueRecognizationGuid             = @RevenueRecognizationGuid                      ";
            _context.Connection.Execute(updateQuery, new
            {
                IsModAdministrative = RevenueRecognition.IsModAdministrative,
                IsASC606 = RevenueRecognition.IsASC606,
                IsCurrentFiscalYearOfNorthWind = RevenueRecognition.IsCurrentFiscalYearOfNorthWind,

                DoesScopeContractChange = RevenueRecognition.DoesScopeContractChange,
                IdentifyTerminationClause = RevenueRecognition.IdentifyTerminationClause,
                WarrantyTerms = RevenueRecognition.WarrantyTerms,
                EstimateWarrantyExposure = RevenueRecognition.EstimateWarrantyExposure,
                PricingExplanation = RevenueRecognition.PricingExplanation,
                Approach = RevenueRecognition.Approach,
                EachMultipleObligation = RevenueRecognition.EachMultipleObligation,
                IsDiscountPurchase = RevenueRecognition.IsDiscountPurchase,
                HasMultipleContractObligations = RevenueRecognition.HasMultipleContractObligations,

                Step1Note = RevenueRecognition.Step1Note,
                IdentityContract = RevenueRecognition.IdentityContract,
                IsTerminationClauseGovernmentStandard = RevenueRecognition.IsTerminationClauseGovernmentStandard,
                IsContractTermExpansion = RevenueRecognition.IsContractTermExpansion,
                Step2Note = RevenueRecognition.Step2Note,
                IdentityPerformanceObligation = RevenueRecognition.IdentityPerformanceObligation,
                IsMultiRevenueStream = RevenueRecognition.IsMultiRevenueStream,
                IsRepetativeService = RevenueRecognition.IsRepetativeService,
                HasOptionToPurchageAdditionalGoods = RevenueRecognition.HasOptionToPurchageAdditionalGoods,
                IsNonRefundableAdvancePayment = RevenueRecognition.IsNonRefundableAdvancePayment,
                HasDiscountProvision = RevenueRecognition.HasDiscountProvision,
                HasWarrenty = RevenueRecognition.HasWarrenty,
                Step3Note = RevenueRecognition.Step3Note,
                ContractType = RevenueRecognition.ContractType,
                IsPricingVariation = RevenueRecognition.IsPricingVariation,
                BaseContractPrice = RevenueRecognition.BaseContractPrice,
                AdditionalPeriodOption = RevenueRecognition.AdditionalPeriodOption,
                Step4Note = RevenueRecognition.Step4Note,
                HasLicensingOrIntellectualProperty = RevenueRecognition.HasLicensingOrIntellectualProperty,
                Step5Note = RevenueRecognition.Step5Note,
                OverviewNotes = RevenueRecognition.OverviewNotes,
                IsNotify = RevenueRecognition.IsNotify,
                IsCompleted = RevenueRecognition.IsCompleted,
                IsRevenueCreated = true,
                UpdatedOn = RevenueRecognition.UpdatedOn,
                CreatedOn = RevenueRecognition.CreatedOn,
                CreatedBy = RevenueRecognition.CreatedBy,
                UpdatedBy = RevenueRecognition.UpdatedBy,
                RevenueRecognizationGuid = RevenueRecognition.RevenueRecognizationGuid
            });
            return 1;
        }

        public int UpdateContractExtension(List<RevenueContractExtension> contractExtensions)
        {
            foreach (var item in contractExtensions)
            {
                if (item.ContractExtensionGuid != Guid.Empty)
                {
                    string ContractExtension = @"Update RevenueContractExtension      set 
                                                                     ExtensionDate                              = @ExtensionDate          ,
                                                                     UpdatedOn						            = @UpdatedOn              ,
                                                                     UpdatedBy						            = @UpdatedBy              ,
                                                                     IsDeleted						            = 0
                                                                     where ContractExtensionGuid                = @ContractExtensionGuid   ";
                    _context.Connection.Execute(ContractExtension, new
                    {
                        ExtensionDate = item.ExtensionDate,
                        UpdatedOn = item.UpdatedOn,
                        UpdatedBy = item.UpdatedBy,
                        ContractExtensionGuid = item.ContractExtensionGuid
                    });
                }
                else
                {
                    AddContractExtension(contractExtensions);
                }
            }
            return 1;
        }

        public int UpdatePerformanceObligation(List<RevenuePerformanceObligation> performanceObligations)
        {
            foreach (var item in performanceObligations)
            {
                if (item.PerformanceObligationGuid != Guid.Empty)
                {
                    string RevenuePerformanceObligation = $@"Update  RevenuePerformanceObligation      set 
                                                                     RevenueStreamIdentifier                    = @RevenueStreamIdentifier             ,
                                                                     RightToPayment                             = @RightToPayment                      ,
                                                                     RoutineService                             = @RoutineService                      ,
                                                                     RevenueOverTimePointInTime                 = @RevenueOverTimePointInTime          ,
                                                                     SatisfiedOverTime                          = @SatisfiedOverTime                   ,
                                                                     UpdatedOn						            = @UpdatedOn                           ,
                                                                     UpdatedBy						            = @UpdatedBy                           
                                                                     where PerformanceObligationGuid            = @PerformanceObligationGuid";
                    _context.Connection.Execute(RevenuePerformanceObligation, new
                    {
                        RevenueStreamIdentifier = item.RevenueStreamIdentifier,
                        RightToPayment = item.RightToPayment,
                        RoutineService = item.RoutineService,
                        RevenueOverTimePointInTime = item.RevenueOverTimePointInTime,
                        SatisfiedOverTime = item.SatisfiedOverTime,
                        UpdatedOn = item.UpdatedOn,
                        UpdatedBy = item.UpdatedBy,
                        PerformanceObligationGuid = item.PerformanceObligationGuid
                    });
                }
                else
                {
                    AddPerformanceObligation(performanceObligations);
                }
            }
            return 1;
        }


        public int EnableRevenueRecognition(Guid[] ContractGuidIds)
        {
            foreach (var RevenueRecognitionGuid in ContractGuidIds)
            {
                var Contract = new
                {
                    RevenueRecognitionGuid = RevenueRecognitionGuid
                };
                string disableQuery = @"Update RevenueRecognition set 
                                            IsActive   = 1
                                            where RevenueRecognitionGuid = @RevenueRecognitionGuid ";
                _context.Connection.Execute(disableQuery, Contract);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }


        public int DeleteRevenueRecognition(Guid ContractGuidIds)
        {
            string disableQuery = @"Update RevenueRecognization set 
                                               IsDeleted   = 1
                                               where RevenueRecognizationGuid = @ContractGuidIds ";
            _context.Connection.Execute(disableQuery, new { ContractGuidIds = ContractGuidIds });
            string RevenuePerformanceObligation = @"Update RevenuePerformanceObligation set 
                                               IsDeleted   = 1
                                               where RevenueGuid = @ContractGuidIds ";
            _context.Connection.Execute(RevenuePerformanceObligation, new { ContractGuidIds = ContractGuidIds });
            string RevenueContractExtension = @"Update RevenueContractExtension set 
                                               IsDeleted   = 1
                                               where RevenueGuid = @ContractGuidIds ";
            _context.Connection.Execute(RevenueContractExtension, new { ContractGuidIds = ContractGuidIds });
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int DisableRevenueRecognition(Guid[] ContractGuidIds)
        {
            foreach (var RevenueRecognitionGuid in ContractGuidIds)
            {
                var Contract = new
                {
                    RevenueRecognitionGuid = RevenueRecognitionGuid
                };
                string disableQuery = @"Update RevenueRecognition set 
                                            IsActive   = 0
                                            where RevenueRecognitionGuid = @RevenueRecognitionGuid ";
                _context.Connection.Execute(disableQuery, Contract);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }


        public int DisableRevenueExtensionPeriod(Guid[] id)
        {
            foreach (var ContractExtensionGuid in id)
            {
                var ContractExtension = new
                {
                    ContractExtensionGuid = ContractExtensionGuid
                };
                string disableQuery = @"Update RevenueContractExtension set 
                                               IsDeleted   = 1
                                               where ContractExtensionGuid = @ContractExtensionGuid ";
                _context.Connection.Execute(disableQuery, ContractExtension);
            }
            return 1;
        }

        public int DisableRevenueObligation(Guid[] id)
        {
            foreach (var PerformanceObligationGuid in id)
            {
                var PerformanceObligation = new
                {
                    PerformanceObligationGuid = PerformanceObligationGuid
                };
                string disableQuery = @"Update RevenuePerformanceObligation set 
                                               IsDeleted   = 1
                                               where PerformanceObligationGuid = @PerformanceObligationGuid ";
                _context.Connection.Execute(disableQuery, PerformanceObligation);
            }
            return 1;
        }


        private bool disableRevenueExtensionAndObligationForValidation(Guid id)
        {
            string disableQuery = @"Update RevenueContractExtension set 
                                               IsDeleted   = 1
                                               where revenueGuid = @revenueGuid ";
            _context.Connection.Execute(disableQuery, new { revenueGuid = id });

            string disableQuery2 = @"Update RevenuePerformanceObligation set 
                                               IsDeleted   = 1
                                               where revenueGuid = @revenueGuid ";
            _context.Connection.Execute(disableQuery2, new { revenueGuid = id });
            return true;
        }

        public Guid GetAccountRepresentive(Guid revenueGuid)
        {
            string sql = $@"SELECT c.UserGuid 
                            FROM ContractUserRole c 
                            LEFT JOIN RevenueRecognization r
                            ON c.ContractGuid= r.contractGuid
                            WHERE r.RevenueRecognizationGuid = @revenueGuid
                            AND c.UserRole = @userRole";
            var result = _context.Connection.QueryFirstOrDefault<Guid>(sql, new { revenueGuid = revenueGuid, userRole = ContractUserRole._accountRepresentative });
            return result;
        }



        public RevenueTriggeredDetailModel GetAwardAmountDetail(Guid? contractGuid)
        {
            var sqlQuery = string.Format($@"select  a.ContractType,
                                                    sum(a.CAwardamount+a.mAwardamount) AwardAmount, 
													sum(a.CFundingAmount+a.mFundingAmount) FundingAmount 
                                                    from 	(select 
                                                    c.ContractType,
												    case when c.RevenueRecognitionGuid is null then isnull(c.AwardAmount,0) else 0 end CAwardAmount,
												    case when c.RevenueRecognitionGuid is null then isnull(c.FundingAmount,0) else 0 end CFundingAmount,
												    isnull (sum(m.AwardAmount),0) mAwardAmount,
												    isnull(sum(m.FundingAmount),0)  mFundingAmount
                                                    from Contract c
                                                    left join 
													(select * from ContractModification m 
													where m.RevenueRecognitionGuid is null 
													and contractGuid=@contractGuid)m
                                                    on m.ContractGuid = c.ContractGuid
                                                    where 
                                                    c.ContractGuid=@contractGuid
                                                    group by 
                                                    c.ContractType,c.RevenueRecognitionGuid,m.RevenueRecognitionGuid,c.AwardAmount,c.FundingAmount)a
												      group by 
                                                    a.ContractType,a.CAwardamount,a.CFundingAmount,a.mAwardamount,a.mFundingAmount");
            var data = _context.Connection.QueryFirstOrDefault<RevenueTriggeredDetailModel>(sqlQuery, new { ContractGuid = contractGuid });
            return data;
        }


        public RevenueRecognition GetInfoForDetailPage(Guid revenueRecognizationGuid)
        {
            string sql = @"SELECT * 
                            FROM RevenueRecognization
                            WHERE RevenueRecognizationGuid = @revenueRecognizationGuid order by  UpdatedOn";
            var list = _context.Connection.QueryFirstOrDefault<RevenueRecognition>(sql, new { revenueRecognizationGuid = revenueRecognizationGuid });
            return list;
        }

        public List<RevenueRecognition> DetailList(Guid id, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var where = " ";
            var searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where += " Where ((title LIKE @searchValue or ResourceNumber like @searchvalue) AND isDeleted = 0)";
            }

            if (take == 0)
            {
                take = 10;
            }
            var orderBy = GetOrderByColumn(sortField, dir);

            string sql = @"select * from (select r.*,
                                        case when r.IsCompleted = 1 then 'Completed' else 'Not Completed' end Status,
										case when c.ContractNumber is null then 0 else 1  end IsContractNumber,
                                        case when c.ContractNumber is null then m.ModificationNumber else c.ContractNumber end ResourceNumber,
                                        case when c.ContractTitle is null then m.ModificationTitle else c.ContractTitle end Title,
                                        u.Displayname UpdatedByName
                                        from RevenueRecognization r
                                        left join Contract c
                                        on c.ContractGuid = r.ResourceGuid
                                        left join ContractModification m
                                        on m.ContractModificationGuid = r.ResourceGuid
                                        left join Users u
                                        on u.UserGuid = r.UpdatedBy where r.contractGuid = @contractGuid and r.isActive=0) a";
            sql += $"{ where }";
            sql += $" ORDER BY {orderBy} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            var list = _context.Connection.Query<RevenueRecognition>(sql, new { contractGuid = id, searchValue = searchString }).ToList();
            return list;
        }

        private string GetOrderByColumn(string sortField, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortDirection))
                sortDirection = " asc ";
            switch (sortDirection.ToUpper())
            {
                case "DESC":
                    sortDirection = " Desc";
                    break;
                default:
                    sortDirection = " Asc";
                    break;
            }
            var sortBy = "";
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToLower())
                {
                    case "title":
                        sortBy = "Title" + sortDirection;
                        break;
                    case "updateddate":
                        sortBy = "Updatedon" + sortDirection;
                        break;
                    case "updatedbyname":
                        sortBy = "Updatedby" + sortDirection;
                        break;
                    case "status":
                        sortBy = "Status" + sortDirection;
                        break;
                    default:
                        sortBy = "ResourceNumber" + sortDirection;
                        break;
                }
            }
            else
            {
                sortBy = "ResourceNumber" + sortDirection;
            }
            return sortBy;
        }

        public int DetailListCount(Guid id, string searchValue)
        {
            var where = " ";
            var searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where += " Where ((title LIKE @searchValue or ResourceNumber like @searchvalue) AND isDeleted = 0)";
            }
            string sql = @"select count(*) from (select r.*,
                                        case when r.IsCompleted = 1 then 'Completed' else 'Not Completed' end Status,
										case when c.ContractNumber is null then 0 else 1  end IsContractNumber,
                                        case when c.ContractNumber is null then m.ModificationNumber else c.ContractNumber end ResourceNumber,
                                        case when c.ContractTitle is null then m.ModificationTitle else c.ContractTitle end Title,
                                        u.Displayname UpdatedByName
                                        from RevenueRecognization r
                                        left join Contract c
                                        on c.ContractGuid = r.ResourceGuid
                                        left join ContractModification m
                                        on m.ContractModificationGuid = r.ResourceGuid
                                        left join Users u
                                        on u.UserGuid = r.UpdatedBy where r.contractGuid = @contractGuid and r.isActive=0) a";
            sql += $"{ where }";
            var result = _context.Connection.QuerySingle<int>(sql, new { contractGuid = id, searchValue = searchString });
            return result;
        }

        public int CountRevenueByContractGuid(Guid id)
        {
            string sql = $@"SELECT count(*)
                            FROM RevenueRecognization 
                            WHERE ContractGuid = @ContractGuid and isActive=0";
            var result = _context.Connection.QueryFirstOrDefault<int>(sql, new { ContractGuid = id });
            return result;
        }

        public int UpdateIsNotify(Guid id)
        {
            string notifyQuery = @"Update RevenueRecognization set 
                                               IsNotify   = 1 WHERE
                                              RevenueRecognizationGuid   = @RevenueRecognizationGuid ";
            return _context.Connection.Execute(notifyQuery, new { RevenueRecognizationGuid = id });
        }
    }
}

