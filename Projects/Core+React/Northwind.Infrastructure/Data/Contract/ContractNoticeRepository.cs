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
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Infrastructure.Data.Contract
{
    public class ContractNoticeRepository : IContractNoticeRepository
    {
        IDatabaseContext _context;
        public ContractNoticeRepository(IDatabaseContext context)
        {
            _context = context;
        }
     
        public int Add(ContractNotice contractNoticeModel)
        {
            string insertQuery = @"INSERT INTO [dbo].[ContractNotice]
                                                                   (
                                                                    ContractNoticeGuid						                                ,
                                                                    NoticeType						                                ,
                                                                    IssuedDate							                                ,
                                                                    LastUpdatedDate				                                        ,
                                                                    ResourceGuid				                                    ,
                                                                    Resolution
                                                                   ,
                                                                    NoticeDescription,
                                                                    UpdatedBy
                                                                    
                                                                    )
                                  VALUES (
                                                                    @ContractNoticeGuid						                                ,
                                                                    @NoticeType						                                ,
                                                                    @IssuedDate							                                ,
                                                                    @LastUpdatedDate				                                ,
                                                                    @ResourceGuid				                                ,
                                                                    @Resolution	
                                                               ,
                                                                    @NoticeDescription,
                                                                    @UpdatedBy
                                                                                                                     
                                                                )";
            return _context.Connection.Execute(insertQuery, contractNoticeModel);
        }
        public ContractResourceFile GetParentId(string key,Guid ResourceId)
        {
            string selectQuery = @"select contractResourcefileGuid,filepath from ContractResourceFile b join FolderStructureFolder a on a.FolderStructureFolderGuid = b.masterfolderGuid where  a.keys = @key and b.resourceGuid = @ResourceId";
            try
            {
                return _context.Connection.QueryFirst<ContractResourceFile>(selectQuery, new { key = key, ResourceId = ResourceId });
            }
            catch
            {
                return null;
            }
           
        }

        public ContractNotice GetDetailById(Guid contractNoticeGuid)
        {
            string selectQuery = @"select *,convert(date,Issueddate) as dateissued from ContractNotice where ContractNoticeGuid = @contractNoticeGuid";
            return _context.Connection.QueryFirst<ContractNotice>(selectQuery, new { contractNoticeGuid = contractNoticeGuid });
        }
      

        public IEnumerable<ContractNotice> GetDetailByNoticeType(string NoticeType, Guid resourceId, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var where = "";
            var searchString = "";
            var sql = string.Empty;
            var additionalSql = string.Empty;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = $@"where (NoticeType LIKE @searchValue or  Resolution LIKE @searchValue or NoticeDescription LIKE @searchValue or Convert(VARCHAR(10),LastUpdatedDate,103) LIKE @searchValue)";
            }
            additionalSql = $@"{sortField}";
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "LastUpdatedDate";
                dir = "desc";
                additionalSql = $@"{sortField}";
            }
            else if (sortField.ToLower().Equals("attachment"))
            {
                additionalSql = $@"LastUpdatedDate";
            }
            else if (sortField.ToLower().Equals("lastupdateddate"))
            {
                additionalSql = $@"LastUpdatedDate";
            }
            string sqlQuery = @"select *,case when (select count(*) from contractresourcefile where contentresourceguid = a.contractNoticeGuid) = 0 then '' else (select concat('<button class=""btn btn-link p-0"" onclick=ShowAttachmentDialog(''',a.contractNoticeGuid,''')>View Attachments (',(select count(*) from contractresourcefile where contentresourceguid = a.contractNoticeGuid),')', '</button>')) end as Attachment from ContractNotice a where NoticeType = @NoticeType and ResourceGuid = @resourceId";
            sql = $@"
                   
                      select 
                            *
                  from ({sqlQuery}) A
                  {where}
                  ORDER BY {additionalSql} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            var data = _context.Connection.Query<ContractNotice>(sql, new { resourceId = resourceId, NoticeType = NoticeType });
            return data;
        }

        public IEnumerable<ContractNotice> GetContractNoticeByContractId(Guid id, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {

            var where = "";
            var searchString = "";
            var sql = string.Empty;
            var additionalSql = string.Empty;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = $@"where (NoticeType LIKE @searchValue or  Resolution LIKE @searchValue or NoticeDescription LIKE @searchValue or Convert(VARCHAR(10),LastUpdatedDate,103) LIKE @searchValue)";
            }
            additionalSql = $@"{sortField}";
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "LastUpdatedDate";
                dir = "desc";
                additionalSql = $@"{sortField}";
            }
            else if (sortField.ToLower().Equals("attachment"))
            {
                additionalSql = $@"LastUpdatedDate";
            }
            else if (sortField.ToLower().Equals("lastupdateddate"))
            {
                additionalSql = $@"LastUpdatedDate";
            }

            var sqlQuery = @" SELECT *,case when (select count(*) from contractresourcefile where contentresourceguid = x.contractNoticeGuid) = 0 then '' else (select concat('<button class=""btn btn-link p-0"" onclick=ShowAttachmentDialog(''',x.contractNoticeGuid,''')>View Attachments (',(select count(*) from contractresourcefile where contentresourceguid = x.contractNoticeGuid),')', '</button>')) end as Attachment FROM (SELECT  *, ROW_NUMBER() OVER (PARTITION BY NoticeType  ORDER BY LastUpdateddate DESC) rn FROM  ContractNotice where resourceguid = @id) x  WHERE   x.rn = 1";
            sql = $@"
                   
                      select 
                            *
                  from ({sqlQuery}) A
                  {where}
                  ORDER BY {additionalSql} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            var contractNotice = _context.Connection.Query<ContractNotice>(sql, new { id = id });

            return contractNotice;
        }


        public IEnumerable<ContractResourceFile> GetAttachmentsByResourceGuid(Guid id)
        {
            var sqlQuery = @" Select a.*,b.displayname as UpdatedByName from contractResourcefile a join users b on a.updatedby = b.userguid   where contentresourceguid = '" + id+"'";
            var attachments = _context.Connection.Query<ContractResourceFile>(sqlQuery, new { ContractGuid = id });
            return attachments;
        }

        public int GetNoticeCount(Guid contractNoticeGuid)
        {
          
                var sqlQuery = @" SELECT count(*) as DataCount FROM (SELECT  *, ROW_NUMBER() OVER (PARTITION BY NoticeType  ORDER BY LastUpdateddate DESC) rn FROM  ContractNotice where resourceguid = @contractNoticeGuid) x  WHERE   x.rn = 1";
               return  _context.Connection.ExecuteScalar<int>(sqlQuery, new { contractNoticeGuid = contractNoticeGuid });
             
        }

        public int GetNoticeDetailsCount(string NoticeType, Guid resourceId)
        {

            var sqlQuery = @" SELECT count(*) as DataCount FROM ContractNotice WHERE  NoticeType = @NoticeType and ResourceGuid=@resourceId";
            return _context.Connection.ExecuteScalar<int>(sqlQuery, new { NoticeType = NoticeType, resourceId= resourceId });

        }


    }
}

