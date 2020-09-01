using Dapper;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using Northwind.Core.Entities;
using System.Text;

namespace Northwind.Infrastructure.Data.WorkBreakdownStructure
{
    public class ContractWBSRepository : IContractWBSRepository
    {
        private readonly IDatabaseContext _context;

        public ContractWBSRepository(IDatabaseContext context)
        {
            _context = context;
        }

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
                                                    @ContractWBSGuid,
                                                    @ContractGuid,
                                                    @UploadFileName,
                                                    @IsCsv,
                                                    @CreatedOn,
                                                    @UpdatedOn,
                                                    @CreatedBy,
                                                    @UpdatedBy,
                                                    @IsDeleted,
                                                    @IsActive
                                                            )";
            return _context.Connection.Execute(insertQuery, new {
                ContractWBSGuid         =  model.ContractWBSGuid,
                ContractGuid            =  model.ContractGuid,
                UploadFileName          =  model.UploadFileName,
                IsCsv                   =  model.IsCsv,
                CreatedOn               =  model.CreatedOn,
                UpdatedOn               =  model.UpdatedOn,
                CreatedBy               =  model.CreatedBy,
                UpdatedBy               =  model.UpdatedBy,
                IsDeleted               =  model.IsDeleted,
                IsActive                =  model.IsActive
            });
        }

        public int UpdateContractWBS(ContractWBS contractWBS)
        {
            string insertQuery = $@"UPDATE ContractWBS set 
                                                    UpdatedOn              =           @UpdatedOn,
                                                    UpdatedBy              =           @UpdatedBy,
                                                    IsDeleted              =           @IsDeleted,
                                                    IsActive               =           @IsActive
                                                     where ContractWBSGuid =           @ContractWBSGuid ";
            return _context.Connection.Execute(insertQuery, new {
                UpdatedOn           = contractWBS.UpdatedOn,
                UpdatedBy           = contractWBS.UpdatedBy,
                IsDeleted           = contractWBS.IsDeleted,
                IsActive            = contractWBS.IsActive,
                ContractWBSGuid     = contractWBS.ContractWBSGuid
            });
        }

        public int DeleteContractWBS(Guid id)
        {
            string deleteQuery = $@"UPDATE ContractWBS SET IsDeleted = 1 WHERE ContractGuid = @ContractGuid;";
            return _context.Connection.Execute(deleteQuery, new { ContractGuid = id});
        }

        public ContractWBS GetContractWBSById(Guid id)
        {
            string sql = $"SELECT * FROM ContractWBS WHERE ContractGuid = @ContractGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<ContractWBS>(sql, new { ContractGuid = id });
            return result;
        }
        
        public bool updateFileName(Guid wbsGuid,string fileName)
        {
            string updateQuery = @"Update ContractWBs set 
                                               UploadFileName   = @fileName
                                               where ContractWBSGuid =@ContractWBSGuid ";
            _context.Connection.Execute(updateQuery,new { fileName= fileName , ContractWBSGuid = wbsGuid });
            return true;
        }
    }
}
