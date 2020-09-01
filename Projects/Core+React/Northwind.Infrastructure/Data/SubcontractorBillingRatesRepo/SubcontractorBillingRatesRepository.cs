using Dapper;
using Northwind.Core.Interfaces;
using System;
using Northwind.Core.Entities;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.SubcontractorBillingRatesRepo
{
    public class SubcontractorBillingRatesRepository : ISubcontractorBillingRatesRepository
    {
        private readonly IDatabaseContext _context;

        public SubcontractorBillingRatesRepository(IDatabaseContext context)
        {
            _context = context;
        }

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
                                                                        @CategoryRateGuid    ,
                                                                        @ContractGuid        ,
                                                                        @UploadFileName      ,
                                                                        @IsCsv               ,
                                                                        @CreatedOn           ,
                                                                        @UpdatedOn           ,
                                                                        @CreatedBy           ,
                                                                        @UpdatedBy           ,
                                                                        @IsDeleted           ,
                                                                        @IsActive               
                                                                )";
            return _context.Connection.Execute(insertQuery, new {
                CategoryRateGuid       =    model.CategoryRateGuid  ,
                ContractGuid           =    model.ContractGuid      ,
                UploadFileName         =    model.UploadFileName    ,
                IsCsv                  =    model.IsCsv             ,
                CreatedOn              =    model.CreatedOn         ,
                UpdatedOn              =    model.UpdatedOn         ,
                CreatedBy              =    model.CreatedBy         ,
                UpdatedBy              =    model.UpdatedBy         ,
                IsDeleted              =    model.IsDeleted         ,
                IsActive               =    model.IsActive         
            });
        }

        public int UpdateLaborCategoryRates(LaborCategoryRates laborCategoryRates)
        {
            string insertQuery = $@"UPDATE LaborCategoryRates set 
                                                    UpdatedOn              =           @UpdatedOn       ,
                                                    UpdatedBy              =           @UpdatedBy       ,
                                                    IsDeleted              =           @IsDeleted       ,
                                                    IsActive               =           @IsActive        
                                                    where CategoryRateGuid =           @CategoryRateGuid ";
            return _context.Connection.Execute(insertQuery, new {
                UpdatedOn               =  laborCategoryRates.UpdatedOn           ,
                UpdatedBy               =  laborCategoryRates.UpdatedBy           ,
                IsDeleted               =  laborCategoryRates.IsDeleted           ,
                IsActive                =  laborCategoryRates.IsActive            ,
                CategoryRateGuid        =  laborCategoryRates.CategoryRateGuid
            });
        }

        public int DeleteLaborCategoryRates(Guid id)
        {
            string deleteQuery = $@"UPDATE LaborCategoryRates set IsDeleted = 1 WHERE ContractGuid = @ContractGuid;";
            return _context.Connection.Execute(deleteQuery, new { ContractGuid = id });
        }

        public LaborCategoryRates GetLaborCategoryRatesById(Guid id)
        {
            string sql = $"SELECT * FROM LaborCategoryRates WHERE ContractGuid = @ContractGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<LaborCategoryRates>(sql, new { ContractGuid = id });
            return result;
        }

        public bool UpdateFileName(Guid subcontractorGuid, string fileName)
        {
            string updateQuery = @"Update LaborCategoryRates set 
                                               UploadFileName   = @fileName
                                               where CategoryRateGuid = @CategoryRateGuid ";
            _context.Connection.Execute(updateQuery,new { fileName= fileName , CategoryRateGuid = subcontractorGuid });
            return true;
        }
    }
}
