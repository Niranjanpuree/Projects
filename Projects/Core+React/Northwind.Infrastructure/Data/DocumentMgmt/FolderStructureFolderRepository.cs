using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Northwind.Core.Utilities;
using System.Linq;

namespace Northwind.Infrastructure.Data.DocumentMgmt
{
    public class FolderStructureFolderRepository : IFolderStructureFolderRepository
    {
        IDatabaseSingletonContext _context;

        public FolderStructureFolderRepository(IDatabaseSingletonContext context)
        {
            _context = context;
        }

        public IEnumerable<FolderStructureFolder> GetFolders(Guid? ParentGuid)
        {
            var sql = "select * from FolderStructureFolder Where ParentGuid=@ParentGuid";
            return _context.Connection.Query<FolderStructureFolder>(sql, new { ParentGuid });
        }

        public FolderStructureFolder GetFolderTree(Guid FolderStructureMasterGuid)
        {
            var sql = "select * from FolderStructureFolder Where FolderStructureMasterGuid=@FolderStructureMasterGuid";
            var folders = _context.Connection.Query<FolderStructureFolder>(sql, new { FolderStructureMasterGuid });
            FolderStructureFolder rootFolder = new FolderStructureFolder();
            var root = folders.Where(c => c.ParentGuid == Guid.Empty).ToList();
            rootFolder.Name = "Root";
            GetChild(rootFolder.FolderStructureFolderGuid, rootFolder, folders);
            return rootFolder;
        }

        public FolderStructureFolder GetFolderTree(string Module)
        {
            var sql = "select * from FolderStructureFolder Where Module=@Module and status=1";
            var folders = _context.Connection.Query<FolderStructureFolder>(sql, new { Module });
            FolderStructureFolder rootFolder = new FolderStructureFolder();
            var root = folders.Where(c => c.ParentGuid == null).ToList();
            if (root.Count > 0)
            {
                rootFolder = root[0];
                return GetChild(null, rootFolder, folders);
            }
            else
            {
                return rootFolder;
            }

        }

        public FolderStructureFolder GetParentFolder(Guid folderStructureFolderGuid)
        {
            var sql = "select * from FolderStructureFolder Where FolderStructureFolderGuid=@folderStructureFolderGuid";
            return _context.Connection.QueryFirst<FolderStructureFolder>(sql, new { folderStructureFolderGuid });
        }

        private FolderStructureFolder GetChild(Guid? ParentGuid, FolderStructureFolder parent, IEnumerable<FolderStructureFolder> lst)
        {
            var children = lst.Where(c => c.ParentGuid == parent.FolderStructureFolderGuid).ToList();
            foreach (var f in children)
            {
                parent.Children.Add(f);
                GetChild(f.ParentGuid, f, lst);
            }
            return parent;
        }
    }
}
