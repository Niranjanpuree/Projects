using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Infrastructure.Data.FarClauseRepo
{
    public class FarContractRepository : IFarContractRepository
    {
        private readonly IDatabaseContext _context;

        public FarContractRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void Add(FarContract farContract)
        {
            string countSql = $@"SELECT COUNT(*)
                            FROM FarContract
                            WHERE ContractGuid= @contractGuid 
                            AND FarContractTypeClauseGuid = @farContractTypeClauseGuid AND IsDeleted=0 ";
            var result = _context.Connection.QueryFirstOrDefault<int>(countSql,
                new
                {
                    contractGuid = farContract.ContractGuid,
                    farContractTypeClauseGuid = farContract.FarContractTypeClauseGuid
                });
            if (result == 0)
            {
                var insertSql = $@"INSERT INTO FarContract
                                (
                                    ContractGuid,
                                    FarContractTypeClauseGuid,
                                    CreatedBy,
                                    UpdatedBy,
                                    CreatedOn,
                                    IsDeleted,
                                    UpdatedOn
                                )
                        VALUES  (
                                @ContractGuid,
                                @FarContractTypeClauseGuid,
                                @CreatedBy,
                                @UpdatedBy,
                                @CreatedOn,
                                @IsDeleted,
                                @UpdatedOn 
                                )";
                _context.Connection.Execute(insertSql, farContract);
            }
        }

        public void AddRequiredData(FarContract farContract)
        {
            string sql = $@"select count (1) from FarContract  fc
                            left join
                            FarContractTypeClause fctc
                            on fctc.FarContractTypeClauseGuid = fc.FarContractTypeClauseGuid
                            where ContractGuid = @ContractGuid and fc.IsDeleted =0 and fctc.FarContractTypeGuid = @FarContractTypeGuid";
            var result = _context.Connection.QuerySingle<int>(sql, new { ContractGuid = farContract.ContractGuid, FarContractTypeGuid = farContract.FarContractTypeGuid });

            if (result == 0)
            {
                string sqlSoftDelete = $@"update FarContract set IsDeleted = 1
                                from FarContract Fc 
                                inner join
                                FarContractTypeClause fctc
                                on fctc.FarContractTypeClauseGuid = Fc.FarContractTypeClauseGuid
                                and
                                fctc.IsRequired = 1
                                 and Fc.ContractGuid =@ContractGuid";
                _context.Connection.Execute(sqlSoftDelete, new { ContractGuid = farContract.ContractGuid });

                string selectSql = $"SELECT FarContractTypeClauseGuid FROM FarContractTypeClause WHERE FarContractTypeGuid = @FarContractTypeGuid AND IsRequired = 1";
                var datas = _context.Connection.Query<Guid>(selectSql, new { FarContractTypeGuid = farContract.FarContractTypeGuid });
                foreach (var item in datas)
                {
                    farContract.FarContractTypeClauseGuid = item;
                    var insertSql = $@"INSERT INTO FarContract
                                                (
                                                    ContractGuid,
                                                    FarContractTypeClauseGuid,
                                                    CreatedBy,
                                                    UpdatedBy,
                                                    CreatedOn,
                                                    IsDeleted,
                                                    UpdatedOn
                                               )
                                        VALUES  (
                                                @ContractGuid,
                                                @FarContractTypeClauseGuid,
                                                @CreatedBy,
                                                @UpdatedBy,
                                                @CreatedOn,
                                                @IsDeleted,
                                                @UpdatedOn 
                                                )";
                    _context.Connection.Execute(insertSql, farContract);
                }
            }

        }

        public List<FarContractDetail> GetAvailableAndOptional(Guid contractGuid, Guid farContractTypeGuid)
        {
            // Optional And Applicable Datas
            string selectSqldata = $@"
                                   SELECT 
                                            Case WHEN FARCONTRACT.FarContractTypeClauseGuid Is Null 
                                            THEN FARCONTRACTCLAUSE.FarContractTypeClauseGuid 
                                            Else FARCONTRACT.FarContractTypeClauseGuid END  FarContractTypeClauseGuid,

                                            Case WHEN FARCONTRACT.Number Is Null 
                                            THEN FARCONTRACTCLAUSE.Number 
                                            Else FARCONTRACT.Number END  FarClauseNumber,

                                            Case WHEN FARCONTRACT.Title Is Null 
                                            THEN FARCONTRACTCLAUSE.Title 
                                            Else FARCONTRACT.Title END  FarClauseTitle,

                                            Case WHEN FARCONTRACT.Paragraph Is Null 
                                            THEN FARCONTRACTCLAUSE.Paragraph 
                                            Else FARCONTRACT.Paragraph END  FarClauseParagraph

                                            FROM ((SELECT 
                                            FarContractTypeClause.FarContractTypeClauseGuid,
                                            FarClause.FarClauseGuid,
                                            FarClause.Number,
                                            FarClause.Title,
                                            FarClause.Paragraph 
                                            FROM FarContract
                                            LEFT JOIN FarContractTypeClause ON 
                                            FarContractTypeClause.FarContractTypeClauseGuid = FarContract.FarContractTypeClauseGuid
                                            LEFT JOIN FarClause 
                                            ON FarClause.FarClauseGuid = FarContractTypeClause.FarClauseGuid
                                            WHERE FarContract.ContractGuid = @ContractGuid
                                            AND FarContract.IsDeleted = 0
                                            AND (FarContractTypeClause.IsApplicable =1 or FarContractTypeClause.IsOptional=1)) FARCONTRACT
                                            FULL JOIN
                                            (SELECT 
                                            FarContractTypeClause.FarContractTypeClauseGuid,
                                            FarClause.FarClauseGuid,
                                            FarClause.Number,
                                            FarClause.Title,
                                            FarClause.Paragraph 
                                            FROM FarContractTypeClause
                                            LEFT JOIN 
                                            FarClause
                                            ON FarClause.FarClauseGuid = FarContractTypeClause.FarClauseGuid
                                            WHERE 
                                            FarContractTypeClause.FarContractTypeGuid = @FarContractTypeGuid 
                                            AND
                                            (FarContractTypeClause.IsApplicable = 1 or FarContractTypeClause.IsOptional=1)) FARCONTRACTCLAUSE
                                            ON
                                            FARCONTRACTCLAUSE.FarClauseGuid = FARCONTRACT.FarClauseGuid)
                                            ORDER BY FarClauseNumber ";

            var applicableOptionalDatas = _context.Connection.Query<FarContractDetail>(selectSqldata, new { ContractGuid = contractGuid, FarContractTypeGuid = farContractTypeGuid }).ToList();
            return applicableOptionalDatas;
        }

        public List<FarContractDetail> GetRequiredData(Guid contractGuid, Guid farContractTypeGuid)
        {
            string selectSql = $@"select 
                                FarContractGuid,
                                ContractGuid,
                                FarContractTypeClause.FarContractTypeClauseGuid,
                                FarClause.Title FarClauseTitle,
                                FarClause.Number FarClauseNumber,
                                FarClause.Paragraph FarClauseParagraph,
                                FarContractTypeClause.IsRequired,
                                FarContractTypeClause.IsApplicable,
                                FarContractTypeClause.IsOptional

                                from FarContractTypeClause
                                inner join
                                FarContract
                                on FarContract.FarContractTypeClauseGuid = FarContractTypeClause.FarContractTypeClauseGuid and 
                                FarContract.IsDeleted =0
                                left join
                                FarClause
                                on FarClause.FarClauseGuid = FarContractTypeClause.FarClauseGuid
                                where FarContractTypeGuid = @FarContractTypeGuid and ContractGuid= @ContractGuid and IsRequired = 1  order by FarClause.Number asc ";

            var requiredData = _context.Connection.Query<FarContractDetail>(selectSql, new { ContractGuid = contractGuid, FarContractTypeGuid = farContractTypeGuid }).ToList();
            return requiredData;
        }

        public List<FarContractDetail> GetSelectedData(Guid contractGuid, Guid farContractTypeGuid)
        {
            string selectSql = $@"select    Fc.FarContractTypeClauseGuid,
                                            FClause.Number FarClauseNumber,
                                            FClause.Paragraph FarClauseParagraph,
                                            FClause.Title FarClauseTitle
                                            from FarContract FC
                                            left join FarContractTypeClause FCTC
                                            on FCTC.FarContractTypeClauseGuid = Fc.FarContractTypeClauseGuid
                                            left join
                                            FarClause FClause
                                            on
                                            FClause.FarClauseGuid = FCTC.FarClauseGuid
                                where Fc.ContractGuid= @ContractGuid and IsRequired = 0";

            var requiredData = _context.Connection.Query<FarContractDetail>(selectSql, new { ContractGuid = contractGuid }).ToList();
            return requiredData;
        }

        public void Delete(Guid contractGuid)
        {
            string deleteQuery = @"Delete Fc from  FarContract Fc 
                                                inner join
                                                FarContractTypeClause fctc
                                                on fctc.FarContractTypeClauseGuid = Fc.FarContractTypeClauseGuid
                                                and
                                                fctc.IsRequired = 0
                                            where ContractGuid =@ContractGuid and isRequired = 0 and Fc.isDeleted =1";
            _context.Connection.Execute(deleteQuery, new { ContractGuid = contractGuid });
        }

        public void SoftDelete(Guid contractGuid)
        {
            string sql = $@"update FarContract set IsDeleted = 1
                                from FarContract Fc 
                                inner join
                                FarContractTypeClause fctc
                                on fctc.FarContractTypeClauseGuid = Fc.FarContractTypeClauseGuid
                                and
                                fctc.IsRequired = 0
                                 and Fc.ContractGuid =@ContractGuid";
            _context.Connection.Execute(sql, new { ContractGuid = contractGuid });
        }

        public List<FarContractDetail> GetAvailableAndOptionalList(Guid contractGuid, Guid farContractType, int pageSize, int skip, string text)
        {
            var rowNum = skip + pageSize;
            var where = "";
            var searchString = "";

            if (!string.IsNullOrEmpty(text))
            {
                searchString = "%" + text + "%";
                where += " and FarClause.Number LIKE @searchValue or FarClause.Title like  @searchValue ";
            }
            where += $" ORDER BY FarClause.Number asc  OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            var pagingQuery = string.Format($@"select 
                                                FarContractGuid,
                                                ContractGuid,
                                                FarContractTypeClause.FarContractTypeClauseGuid,
                                                FarClause.Title FarClauseTitle,
                                                FarClause.Number FarClauseNumber,
                                                FarClause.Paragraph FarClauseParagraph,
                                                FarContractTypeClause.IsRequired,
                                                FarContractTypeClause.IsApplicable,
                                                Case When ((ContractGuid is null or ContractGuid = '00000000-0000-0000-0000-000000000000') and IsApplicable = 1) then 0 else 1 end ApplicableStatus,
                                                Case When ((ContractGuid is null or ContractGuid = '00000000-0000-0000-0000-000000000000') and IsOptional = 1) then 0 else 1 end  OptionalStatus,
                                                FarContractTypeClause.IsOptional

                                                from FarContractTypeClause
                                                left join 
                                                FarContract 
                                                on FarContract.FarContractTypeClauseGuid = FarContractTypeClause.FarContractTypeClauseGuid
                                                and
                                                FarContract.ContractGuid = @ContractGuid
                                                left join 
                                                FarClause 
                                                on FarClause.FarClauseGuid = FarContractTypeClause.FarClauseGuid
                                                where FarContractTypeGuid=@FarContractTypeGuid and (IsApplicable =1 or IsOptional=1)
                                                    { where }");


            var pagedData = _context.Connection.Query<FarContractDetail>(pagingQuery, new { ContractGuid = contractGuid, FarContractTypeGuid = farContractType, searchValue = searchString, skip = skip, rowNum = rowNum }).ToList();
            return pagedData;
        }

        public FarContract GetAvailableFarContractByContractGuid(Guid contractGuid)
        {
            var sql = @"SELECT *
                        FROM FarContract fc
                        LEFT JOIN Users u
                        ON u.UserGuid = fc.UpdatedBy
                        LEFT JOIN FarContractTypeClause fcc
                        ON fcc.FarContractTypeClauseGuid = fc.FarContractTypeClauseGuid
                        WHERE fc.IsDeleted = 0
                        AND fcc.IsRequired = 0
                        AND fc.ContractGuid = @contractGuid";
            return _context.Connection.QueryFirstOrDefault<FarContract>(sql, new { contractGuid = contractGuid});
        }
    }
}
