using Dapper;
using Northwind.Core.Interfaces;
using System;
using Northwind.Core.Entities;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.EmployeeBillingRatesRepo
{
    public class EmployeeBillingRatesRepository : IEmployeeBillingRatesRepository
    {
        private readonly IDatabaseContext _context;

        public EmployeeBillingRatesRepository(IDatabaseContext context)
        {
            _context = context;
        }

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
                                                                        @BillingRateGuid     ,
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
                BillingRateGuid     =   model.BillingRateGuid,
                ContractGuid        =   model.ContractGuid,
                UploadFileName      =   model.UploadFileName,
                IsCsv               =   model.IsCsv,
                CreatedOn           =   model.CreatedOn,
                UpdatedOn           =   model.UpdatedOn,
                CreatedBy           =   model.CreatedBy,
                UpdatedBy           =   model.UpdatedBy,
                IsDeleted           =   model.IsDeleted,
                IsActive            = model.IsActive
            });
        }

        public int UpdateEmployeeBillingRates(EmployeeBillingRates employeeBillingRates)
        {
            string insertQuery = $@"UPDATE EmployeeBillingRates set 
                                                    UpdatedOn              =           @UpdatedOn,
                                                    UpdatedBy              =           @UpdatedBy,
                                                    IsDeleted              =           @IsDeleted,
                                                    IsActive               =           @IsActive
                                                     where BillingRateGuid =           @BillingRateGuid ";
            return _context.Connection.Execute(insertQuery, new {
                UpdatedOn           =           employeeBillingRates.UpdatedOn,
                UpdatedBy           =           employeeBillingRates.UpdatedBy,
                IsDeleted           =           employeeBillingRates.IsDeleted,
                IsActive            =           employeeBillingRates.IsActive,
                BillingRateGuid     =           employeeBillingRates.BillingRateGuid
            });
        }

        public int DeleteEmployeeBillingRates(Guid id)
        {
            string deleteQuery = $@"UPDATE EmployeeBillingRates set IsDeleted = 1 WHERE ContractGuid = @ContractGuid;";
            return _context.Connection.Execute(deleteQuery, new { ContractGuid = id});
        }

        public EmployeeBillingRates GetEmployeeBillingRatesById(Guid id)
        {
            string sql = $"SELECT * FROM EmployeeBillingRates WHERE ContractGuid = @ContractGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<EmployeeBillingRates>(sql, new { ContractGuid = id });
            return result;
        }

        public bool UpdateFileName(Guid employeeGuid, string fileName)
        {
            string updateQuery = @"Update EmployeeBillingRates set 
                                               UploadFileName   = @fileName
                                               where BillingRateGuid =@BillingRateGuid ";
            _context.Connection.Execute(updateQuery,new { fileName= fileName , BillingRateGuid = employeeGuid });
            return true;
        }
    }
}
