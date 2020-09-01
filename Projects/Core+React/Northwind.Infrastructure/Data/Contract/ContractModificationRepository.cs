using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System.Linq;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Http;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Infrastructure.Data.Contract
{
    public class ContractModificationRepository : IContractModificationRepository
    {
        IDatabaseContext _context;
        public ContractModificationRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<ContractModification> GetAll(Guid contractGuid, bool isTaskModification, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();

            if (sortField.Equals("isActiveStatus"))
                orderingQuery.Append($"ContractModification.isActive {sortDirection}");  //Ambiguous if not done.. 
            else
                orderingQuery.Append($"{sortField} {sortDirection}");

            if (!string.IsNullOrEmpty(searchValue))
                conditionalQuery.Append($"and ContractModification.ContractReference like '%{searchValue}%'");

            var taskModificationCondition = "";
            if (isTaskModification)
                taskModificationCondition = $"AND ContractModification.IsTaskModification= 1";
            else
                taskModificationCondition = "AND ContractModification.IsTaskModification = 0";
            var sqlQuery = string.Format($@"SELECT 
                                        ContractModification.ContractModificationGuid,
                                        ContractModification.ContractGuid,
                                        ContractModification.ModificationNumber,
                                        rav.Name AS ModificationType,
                                        ContractModification.ModificationTitle,
                                        ContractModification.IsAwardAmount,
                                        ContractModification.IsFundingAmount,
                                        ContractModification.IsPop,
                                        ContractModification.FundingAmount,
                                        ContractModification.EnteredDate,
                                        ContractModification.EffectiveDate,
                                        ContractModification.AwardAmount,
                                        ContractModification.POPStart,
                                        ContractModification.POPEnd,
                                        ContractModification.Description,
                                        ContractModification.UpdatedOn,
                                        ContractModification.IsActive,
                                        Contract.ContractNumber,
                                        Contract.ProjectNumber,
                                        Contract.ContractTitle,
                                        ContractModification.CreatedOn                                                   
                                        FROM ContractModification
                                        LEFT JOIN Contract
                                        ON ContractModification.ContractGuid = Contract.ContractGuid
                                        LEFT JOIN ResourceAttributeValue rav
                                        ON rav.Value = ContractModification.ModificationType
                                        WHERE ContractModification.IsDeleted = 0
                                        {conditionalQuery}
                                         and   Contract.ContractGuid = '{contractGuid}'");
            sqlQuery += taskModificationCondition;
            sqlQuery += $" ORDER BY {sortField} {sortDirection}  OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            var pagedData = _context.Connection.Query<ContractModification>(sqlQuery);
            return pagedData;
        }

        public int TotalRecord(Guid contractGuid, bool isTaskModification)
        {

            string sql = $@"SELECT Count(1) 
                            from ContractModification
                            left join Contract
                            on ContractModification.ContractGuid = Contract.ContractGuid
                            where ContractModification.IsDeleted = 0
                            and   Contract.ContractGuid = '{contractGuid}'";
            var isTaskCondition = "";
            if (isTaskModification)
                isTaskCondition = " AND ContractModification.IsTaskModification = 1";
            else
                isTaskCondition = " AND ContractModification.IsTaskModification = 0";
            sql += isTaskCondition;
            var result = _context.Connection.QuerySingle<int>(sql);
            return result;
        }
        public int Add(ContractModification contractModificationModel)
        {
            string insertQuery = @"INSERT INTO [dbo].[ContractModification]
                                                                   (
                                                                    ContractModificationGuid						                    ,
                                                                    ContractGuid						                                ,
                                                                    ModificationNumber							                    ,
                                                                    ModificationType							                    ,
                                                                    AwardAmount				                                        ,
                                                                    EnteredDate				                                    ,
                                                                    EffectiveDate				                                        ,
                                                                    POPStart											            ,
                                                                    POPEnd						                                    ,     
                                                                    Description						                                    ,
                                                                    UploadedFileName						                            ,
                                                                    ModificationTitle						                            ,
                                                                    IsAwardAmount						                            ,
                                                                    IsFundingAmount						                            ,
                                                                    IsPop						                            ,
                                                                    FundingAmount						                            ,
                                                                    IsTaskModification,
                                                                    CreatedOn						                            ,
                                                                    UpdatedOn						                            ,
                                                                    CreatedBy						                            ,
                                                                    UpdatedBy						                            ,
                                                                    IsActive						                            ,
                                                                    IsDeleted                                                   
                                                                    )
                                  VALUES (
                                                                    @ContractModificationGuid						                    ,
                                                                    @ContractGuid						                                ,
                                                                    @ModificationNumber							                    ,
                                                                    @ModificationType							                    ,
                                                                    @AwardAmount				                            ,
                                                                    @EnteredDate				                            ,
                                                                    @EffectiveDate				                            ,
                                                                    @POPStart											            ,
                                                                    @POPEnd						                                    ,     
                                                                    @Description						                                    ,
                                                                    @UploadedFileName						                            ,
                                                                    @ModificationTitle						                            ,
                                                                    @IsAwardAmount						                            ,
                                                                    @IsFundingAmount						                            ,
                                                                    @IsPop						                            ,
                                                                    @FundingAmount						                            ,
                                                                    @IsTaskModification,
                                                                    @CreatedOn						                            ,
                                                                    @UpdatedOn						                            ,
                                                                    @CreatedBy						                            ,
                                                                    @UpdatedBy						                            ,
                                                                    @IsActive						                            ,
                                                                    @IsDeleted                                                   
                                                                )";
            return _context.Connection.Execute(insertQuery, contractModificationModel);
        }
        public int Edit(ContractModification contractModificationModel)
        {
            string updateQuery = @"Update ContractModification set 
                                                                    ContractGuid					 = @ContractGuid			    ,
                                                                    ModificationNumber			     = @ModificationNumber			,
                                                                    ModificationType			     = @ModificationType			,
                                                                    AwardAmount				         = @AwardAmount				    ,
                                                                    EnteredDate				         = @EnteredDate				    ,
                                                                    EffectiveDate				     = @EffectiveDate				,
                                                                    POPStart						 = @POPStart					,	
                                                                    POPEnd						     = @POPEnd						,
                                                                    Description					     = @Description					,
                                                                    UploadedFileName				 = @UploadedFileName			,	
                                                                    ModificationTitle				 = @ModificationTitle			,	
                                                                    IsAwardAmount				    = @IsAwardAmount			,	
                                                                    IsFundingAmount				    = @IsFundingAmount			,	
                                                                    IsPop				            = @IsPop			,	
                                                                    FundingAmount				    = @FundingAmount			,	
                                                                    IsTaskModification               =@IsTaskModification,                 
                                                                    UpdatedOn						 = @UpdatedOn					,	
                                                                    UpdatedBy						 = @UpdatedBy					,	
                                                                    IsActive						 = @IsActive					,	
                                                                    IsDeleted                        = @IsDeleted                   
                                                                    where ContractModificationGuid    = @ContractModificationGuid";
            return _context.Connection.Execute(updateQuery, contractModificationModel);

        }
        public int Delete(Guid[] ids)
        {
            foreach (var contractModificationGuid in ids)
            {
                var contractModification = new
                {
                    ContractModificationGuid = contractModificationGuid
                };
                string disableQuery = @"Update ContractModification set 
                                               IsDeleted   = 1
                                               where ContractModificationGuid =@ContractModificationGuid ";
                _context.Connection.Execute(disableQuery, contractModification);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int Disable(Guid[] ids)
        {
            foreach (var contractModificationGuid in ids)
            {
                var contractModification = new
                {
                    ContractModificationGuid = contractModificationGuid
                };
                string disableQuery = @"Update ContractModification set 
                                            IsActive   = 0
                                            where ContractModificationGuid =@ContractModificationGuid ";
                _context.Connection.Execute(disableQuery, contractModification);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int Enable(Guid[] ids)
        {
            foreach (var contractModificationGuid in ids)
            {
                var contractModification = new
                {
                    ContractModificationGuid = contractModificationGuid
                };
                string disableQuery = @"Update ContractModification set 
                                            IsActive   = 1
                                            where ContractModificationGuid =@ContractModificationGuid ";
                _context.Connection.Execute(disableQuery, contractModification);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public ContractModification GetDetailById(Guid id)
        {
            string sql = @"select distinct 
                            ContractModification.ContractModificationGuid		                                ,
                            ContractModification.ContractGuid									                ,
                            ContractModification.ModificationNumber				                                ,
                            ContractModification.ModificationType				                                ,
                            ContractModification.ModificationTitle				                                ,
                            ContractModification.IsAwardAmount				                                ,
                            ContractModification.IsFundingAmount				                                ,
                            ContractModification.IsPop				                                ,
                            ContractModification.FundingAmount				                                ,
                            ContractModification.EnteredDate						                            ,
                            ContractModification.EffectiveDate				    				                ,
                            ContractModification.AwardAmount			                                        ,
                            ContractModification.POPStart							                            ,
                            ContractModification.POPEnd											                ,
                            ContractModification.Description						                            ,
                            ContractModification.UploadedFileName						                            ,
							ContractModification.IsActive                                                       ,
							ContractModification.UpdatedOn                                                      ,
							ContractModification.CreatedBy                                                      ,
                            Contract.ContractNumber                                                             ,
                            Contract.Currency                                                             ,
                            Contract.ProjectNumber                                                              ,
                            Contract.ContractTitle
                            from ContractModification
                            left join Contract
                            on ContractModification.ContractGuid = Contract.ContractGuid
                            WHERE ContractModificationGuid =  @ContractModificationGuid;";
            var contractModificationModel = _context.Connection.QuerySingleOrDefault<ContractModification>(sql, new { ContractModificationGuid = id });
            return contractModificationModel;
        }

        public bool IsExistModificationNumber(Guid contractGuid,Guid contractModificationGuid, string modificationNumber)
        {
            string modificationNumberQuery = $@"select Count(modificationNumber) from  ContractModification  
                                              where  IsDeleted   = 0
                                                and contractGuid = @contractGuid
                                                and ContractModificationGuid != @contractModificationGuid
                                                and modificationNumber = @modificationNumber ";
            var result = _context.Connection.QueryFirstOrDefault<int>(modificationNumberQuery, new { contractGuid = contractGuid, contractModificationGuid = contractModificationGuid, modificationNumber = modificationNumber });

            return result > 0 ? true : false;
        }

        public bool InsertRevenueRecognitionGuid(Guid revenueRecognition, Guid contractGuid)
        {
            var updateSql = @"Update ContractModification set RevenueRecognitionGuid=@RevenueRecognitionGuid
                        WHERE contractGuid = @contractGuid and revenuerecognitionGuid is null";
            _context.Connection.Execute(updateSql, new { RevenueRecognitionGuid = revenueRecognition, contractGuid = contractGuid });
            return true;
        }
        public bool UpdateRevenueRecognitionGuid(Guid modGuid, decimal? awardAmount, decimal? fundingAmount)
        {
            var updateQuery = @"Update ContractModification set RevenueRecognitionguid = null
                                 where ContractModificationGuid= @modGuid ";
            _context.Connection.Query(updateQuery, new { modGuid = modGuid });
            return true;
        }

        public ContractModification getAwardAndFundingAmountbyId(Guid id)
        {
            string modificationNumberQuery = $@"select * from  ContractModification  
            where ContractModificationGuid = @ContractModificationGuid ";
            var result = _context.Connection.QueryFirstOrDefault<ContractModification>(modificationNumberQuery, new { ContractModificationGuid = id });
            return result;
        }

        public ContractModification GetTotalAwardAmount(Guid id)
        {
            string modificationNumberQuery = $@"select sum(awardamount) AwardAmount from  ContractModification  
            where contractGuid = @contractGuid and revenueRecognitionGuid is null";
            var result = _context.Connection.QueryFirstOrDefault<ContractModification>(modificationNumberQuery, new { contractGuid = id });
            return result;
        }

        public bool IsExistModificationTitle(Guid contractGuid, Guid modificationGuid, string modificationTitle)
        {
            string modificationNumberQuery = $@"select Count(*) from  ContractModification  
                                              where  IsDeleted   = 0
                                                 and contractGuid = @contractGuid
                                                 and ContractModificationGuid != @contractModificationGuid
                                                 and modificationTitle = @modificationTitle ";
            var result = _context.Connection.QueryFirstOrDefault<int>(modificationNumberQuery, new { contractGuid = contractGuid, contractModificationGuid = modificationGuid, modificationTitle = modificationTitle });
            //return !string.IsNullOrEmpty(result) ? true : false;
            return  result > 0 ? true: false;
        }

        public ContractModification GetModByContractGuidAndModNumber(Guid contractGuid, string modNumber)
        {
            var query = @"SELECT *
                        FROM ContractModification
                        WHERE ContractGuid = @contractGuid
                        AND ModificationNumber = @modNumber";
            return _context.Connection.QueryFirstOrDefault<ContractModification>(query, new { contractGuid = contractGuid, modNumber = modNumber });
        }
    }
}

