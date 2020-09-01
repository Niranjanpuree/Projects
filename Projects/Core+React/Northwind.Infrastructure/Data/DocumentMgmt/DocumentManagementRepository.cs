using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using Northwind.Core.Entities.DocumentMgmt;

namespace Northwind.Infrastructure.Data.DocumentMgmt
{
    public class DocumentManagementRepository : IDocumentManagementRepository
    {
        IDatabaseSingletonContext _context;

        public DocumentManagementRepository(IDatabaseSingletonContext context)
        {
            _context = context;
        }

        public void CreateFileOrFolder(IDocumentEntity entity)
        {
            var sql = @"INSERT INTO [ContractResourceFile]
                       ([ContractResourceFileGuid]
                       ,[MasterStructureGuid]
                       ,[MasterFolderGuid]
                       ,[ParentId]
                       ,[ResourceGuid]
                       ,[UploadFileName]
                       ,[ResourceType]
                       ,[Keys]
                       ,[MimeType]
                       ,[IsActive]
                       ,[IsDeleted]
                       ,[CreatedBy]
                       ,[UpdatedBy]
                       ,[CreatedOn]
                       ,[UpdatedOn]
                       ,[IsCsv]
                       ,[Description]
                       ,[FilePath]
                       ,[FileSize]
                       ,[UploadUniqueFileName]
                       ,[IsFile]
                       ,[IsReadOnly])
                VALUES
                       (@ContractResourceFileGuid
                       ,@MasterStructureGuid
                       ,@MasterFolderGuid
                       ,@ParentId
                       ,@ResourceGuid
                       ,@UploadFileName
                       ,@ResourceType
                       ,@Keys
                       ,@MimeType
                       ,@IsActive
                       ,@IsDeleted
                       ,@CreatedBy
                       ,@UpdatedBy
                       ,@CreatedOn
                       ,@UpdatedOn
                       ,@IsCsv
                       ,@Description
                       ,@FilePath
                       ,@FileSize
                       ,@UploadUniqueFileName
                       ,@IsFile
                       ,@IsReadOnly)";

            _context.Connection.Execute(sql, entity);
        }

        public IEnumerable<IDocumentEntity> GetFolders(string resourceType, Guid resourceGuid, Guid parentGuid, string keys = "")
        {
            var sql = "";
            if (string.IsNullOrEmpty(sql))
            {
                sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ResourceGuid=@ResourceGuid and ParentId=@ParentId AND IsDeleted=0 order by UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ResourceGuid = resourceGuid, ParentId = parentGuid });
            }
            else
            {
                sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ResourceGuid=@ResourceGuid and Keys=@Keys and ParentId=@ParentId AND IsDeleted=0 order by UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ResourceGuid = resourceGuid, ParentId = parentGuid, Keys = keys });
            }
        }

        private IEnumerable<IDocumentEntity> GetFolders(string resourceType, Guid resourceGuid, string keys = "")
        {
            if (string.IsNullOrEmpty(keys))
            {
                var sql = "SELECT *,(select count(*) from ContractResourceFile b where b.resourceGuid=@ResourceGuid and b.isfile=1 and IsDeleted=0 and Replace(b.FilePath,'\\','/') like CONCAT(Replace(IIF(RIGHT(a.FilePath,1) = '\\' OR RIGHT(a.FilePath,1) = '/', a.FilePath, a.FilePath + '/'),'\\','/'),'%')) as folderfilecounts FROM ContractResourceFile a WHERE ResourceType=@ResourceType AND ResourceGuid=@ResourceGuid AND IsDeleted=0 and IsFile=0  order by UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ResourceGuid = resourceGuid });
            }
            else
            {
                var sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ResourceGuid=@ResourceGuid AND IsDeleted=0 AND Keys=@Keys order by UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ResourceGuid = resourceGuid, Keys = keys });
            }
        }

        public IDocumentEntity GetFolderTree(string resourceType, Guid resourceGuid)
        {
            var folders = GetFolders(resourceType, resourceGuid);
            var entity = folders.Where(c => (c.ParentId == null || c.ParentId == Guid.Empty) && c.IsFile==false).ToList();
            var parentData = entity.FirstOrDefault();
             parentData.UploadFileName = parentData.UploadFileName + " <small style='color:red'>(" + parentData.folderfilecounts + ")</small>";
            if (entity.Count > 0)
            {
                var entity1 = ManageFolderTree(entity[0], folders);
                return entity1;
            }
            else
            {
                return null;
            }
            
        }
        private int CountChildern(IDocumentEntity entity, IEnumerable<IDocumentEntity> entities)
        {

            //var xt = (from x in entities where x.IsFile == true && x.ParentId == entity.ContractResourceFileGuid select x).Count() + entity.Subfolders.Sum(ch => CountChildern(ch, entities));
            //return xt;
            return entity.folderfilecounts;

        }
        public IDocumentEntity GetFilesAndFolderTree(string resourceType, Guid resourceGuid)
        {
            var folders = GetFolders(resourceType, resourceGuid);
            var entity = folders.Where(c => c.ParentId == null || c.ParentId == Guid.Empty).ToList();
            if (entity.Count > 0)
            {
                var entity1 = ManageFilesAndFolderTree(entity[0], folders);
                return entity1;
            }
            else
            {
                return null;
            }

        }

        private IDocumentEntity ManageFilesAndFolderTree(IDocumentEntity entity, IEnumerable<IDocumentEntity> entities)
        {
            var folders = entities.Where(c => c.ParentId == entity.ContractResourceFileGuid).ToList();
            foreach (var f in folders)
            {
                var f1 = ManageFilesAndFolderTree(f, entities);
                f1.FilePath = f1.FilePath.Replace("\\", "/");
                entity.Subfolders.Add(f1);
            }
            return entity;
        }

        private IDocumentEntity ManageFolderTree(IDocumentEntity entity, IEnumerable<IDocumentEntity> entities)
        {
            var folders = entities.Where(c => c.ParentId == entity.ContractResourceFileGuid && c.IsFile == false).ToList();
            foreach(var f in folders)
            {
                var f1 = ManageFolderTree(f, entities);
                f1.FilePath = f1.FilePath.Replace("\\", "/");
                //f1.folderfilecounts = CountChildern(f, entities);
                entity.Subfolders.Add(f1);
                f1.UploadFileName = f1.UploadFileName + " <small style='color:red'>(" + f1.folderfilecounts + ")</small>";
                
            }
            return entity;
        }

        public bool HasDefaultStructure(string resourceType, Guid resourceGuid)
        {
            var sql = "SELECT Count(1) FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ResourceGuid=@ResourceGuid AND MasterStructureGuid IS NOT NULL AND IsDeleted=0";
            return _context.Connection.ExecuteScalar<int>(sql, new { ResourceType = resourceType, ResourceGuid = resourceGuid }) > 0;
        }

        public IEnumerable<IDocumentEntity> GetFilesAndFoldersByParentId(string resourceType, Guid parentId, string keys = "")
        {
            if (string.IsNullOrEmpty(keys))
            {
                var sql = "SELECT cf.*, u.displayName as UpdatedByName FROM ContractResourceFile cf left join Users u on u.UserGuid=cf.UpdatedBy WHERE cf.ResourceType=@ResourceType AND cf.ParentId=@ParentId AND cf.IsDeleted=0 order by cf.UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ParentId = parentId });
            }
            else
            {
                var sql = "SELECT cf.*, u.displayName as UpdatedByName FROM ContractResourceFile cf left join Users u on u.UserGuid=cf.UpdatedBy WHERE Cf.ResourceType=@ResourceType AND Cf.ParentId=@ParentId AND Cf.IsDeleted=0 AND Cf.Keys=@Keys order by Cf.UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ParentId = parentId, Keys = keys });
            }

        }

        public IEnumerable<IDocumentEntity> GetFilesAndFoldersByGuid(string resourceType, Guid fileGuid, string keys = "")
        {
            if (string.IsNullOrEmpty(keys))
            {
                var sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ContractResourceFileGuid=@fileGuid AND IsDeleted=0 order by UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, fileGuid });
            }
            else
            {
                var sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ContractResourceFileGuid=@fileGuid AND IsDeleted=0 AND Keys=@Keys order by UploadFileName";
                return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, fileGuid, Keys = keys });
            }            
        }

        public IDocumentEntity GetFileOrFolder(Guid fileGuid)
        {
            var sql = "SELECT * FROM ContractResourceFile WHERE ContractResourceFileGuid=@ContractResourceFileGuid AND IsDeleted=0 order by UploadFileName";
            var result = _context.Connection.Query<DocumentEntity>(sql, new { ContractResourceFileGuid = fileGuid });
            if (result.Count() > 0)
            {
                return result.SingleOrDefault();
            }
            return null;
        }

        public void UpdateFileOrFolder(IDocumentEntity entity)
        {
            var sql = @"UPDATE [ContractResourceFile]
                           SET [ParentId] = @ParentId
                              ,[UploadFileName] = @UploadFileName
                              ,[MimeType] = @MimeType
                              ,[IsActive] = @IsActive
                              ,[IsDeleted] = @IsDeleted
                              ,[UpdatedBy] = @UpdatedBy
                              ,[UpdatedOn] = @UpdatedOn
                              ,[IsCsv] = @IsCsv
                              ,[Description] = @Description
                              ,[FilePath] = @FilePath
                              ,[FileSize] = @FileSize
                              ,[UploadUniqueFileName] = @UploadUniqueFileName
                              ,[IsFile] = @IsFile
                         WHERE 
                            ContractResourceFileGuid=@ContractResourceFileGuid
                        ";
            _context.Connection.Execute(sql, entity);
        }

        public void DeleteFilesAndFoldersByGuid(string resourceType, Guid fileGuid)
        {
            var sql = "Update [ContractResourceFile] set IsActive=0, IsDeleted=1, UploadFileName=UploadFileName+'_'+convert(nvarchar(50), ContractResourceFileGuid) WHERE ContractResourceFileGuid=@ContractResourceFileGuid AND ResourceType=@ResourceType";
            _context.Connection.Execute(sql, new { ContractResourceFileGuid = fileGuid, ResourceType = resourceType });
            DeleteAllChildFileOrFolder(resourceType, fileGuid);
        }

        private void DeleteAllChildFileOrFolder(string resourceType, Guid FileOrFolderGuid)
        {
            var sql = "SELECT * FROM [ContractResourceFile] WHERE ResourceType=@ResourceType AND ParentId=@ParentId";
            var lst = _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ParentId = FileOrFolderGuid });
            foreach(var f in lst)
            {
                DeleteFilesAndFoldersByGuid(resourceType, f.ContractResourceFileGuid);
            }
        }

        public IDocumentEntity GetFolderByMasterFolderGuid(Guid masterFolderGuid, string resourceType, Guid resourceGuid)
        {
            var sql = "SELECT * FROM ContractResourceFile WHERE Keys=@ResourceType AND ResourceGuid=@ResourceGuid AND MasterFolderGuid=@MasterFolderGuid order by UploadFileName";
            return _context.Connection.QuerySingle<DocumentEntity>(sql, new { MasterFolderGuid = masterFolderGuid, ResourceGuid = resourceGuid, ResourceType = resourceType });            
        }

        public IEnumerable<IDocumentEntity> GetKeyFolder(string resourceType, Guid resourceGuid, string keys = "")
        {
            var sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ResourceGuid=@ResourceGuid AND IsDeleted=0 AND Keys=@Keys order by UploadFileName";
            return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ResourceGuid = resourceGuid, Keys = keys });
        }

        public IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, Guid parentId, string searchString)
        {
            searchString = $"%{searchString}%";
            var sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND ParentId=@ParentId AND IsDeleted=0 AND (UploadFileName like @SearchString or Description like @SearchString) order by UploadFileName";
            return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, ParentId= parentId, SearchString = searchString });
        }

        public IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, string inRoute, string searchString)
        {
            searchString = $"%{searchString}%";
            inRoute = $"{inRoute}%";
            var sql = "SELECT * FROM ContractResourceFile WHERE ResourceType=@ResourceType AND FilePath LIKE @FilePath AND IsDeleted=0 AND (UploadFileName like @SearchString or Description like @SearchString) order by UploadFileName";
            return _context.Connection.Query<DocumentEntity>(sql, new { ResourceType = resourceType, FilePath = inRoute, SearchString = searchString });
        }

        public IDocumentEntity GetFolderByKey(string resourceType, Guid resourceId, string key)
        {
            var sql = @"SELECT *
                      FROM ContractResourceFile
                      where ResourceType = @resourceType
                      and Keys = @key
                      and ResourceGuid = @resourceId
                      and IsFile = 0";
            return _context.Connection.QueryFirstOrDefault<DocumentEntity>(sql, new { resourceType = resourceType, key = key, resourceId = resourceId });
        }
    }
}
