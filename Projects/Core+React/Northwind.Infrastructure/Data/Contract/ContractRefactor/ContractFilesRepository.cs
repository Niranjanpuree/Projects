using Dapper;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.Contract.ContractRefactor
{
    public class ContractFilesRepository : IContractFileRepository
    {
        IDatabaseContext _context;

        public ContractFilesRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<ContractResourceFile> GetFileListByContractGuid(Guid contractGuid)
        {
            var sql = @"SELECT * 
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ResourceGuid] = @resourceGuid";
            var fileList = _context.Connection.Query<ContractResourceFile>(sql, new { resourceGuid = contractGuid });
            return fileList;
        }

        public ContractResourceFile GetFilesByContractGuid(Guid contractGuid)
        {
            var sql = @"SELECT TOP(1) *
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ResourceGuid] = @contractGuid";
            var contractFile = _context.Connection.QueryFirstOrDefault<ContractResourceFile>(sql, new { contractGuid = contractGuid });
            return contractFile;
        }

        public bool InsertContractFile(ContractResourceFile file)
        {
            var insertSql = @"INSERT INTO [dbo].[ContractResourceFile]
                        VALUES
                        (
                            @ResourceGuid,
                            @Keys,
                            @UploadFileName,
                            @MimeType,
                            @IsActive,
                            @IsDeleted,
                            @CreatedBy,
                            @UpdatedBy,
                            @CreatedOn,
                            @IsCsv,
                        )";
            _context.Connection.Execute(insertSql, file);
            return true;
        }

        public bool UpdateContractFile(ContractResourceFile file)
        {
            var insertSql = @"UPDATTE [dbo].[ContractResourceFile]
                        SET
                        ResourceGuid = @ResourceGuid,
                        UploadFileName = @UploadFileName,
                        Keys = @Keys,
                        MimeType = @MimeType,
                        IsActive = @IsActive,
                        IsDeleted = @IsDeleted,
                        CreatedBy = @CreatedBy,
                        UpdatedBy = @UpdatedBy,
                        CreatedOn = @CreatedOn,
                        IsCsv =@IsCsv
                        WHERE ContractResourceFileGuid = @ContractResourceFileGuid
                        )";
            _context.Connection.Execute(insertSql, file);
            return true;
        }
    }
}
