using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.DocumentMgmt
{
    public class FolderStructureFolder
    {
        private List<FolderStructureFolder> children = new List<FolderStructureFolder>();
        public Guid FolderStructureFolderGuid { get; set; }
        public Guid FolderStructureMasterGuid { get; set; }
        public Guid ParentGuid { get; set; }
        public string Keys { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        [Ignore]
        public List<FolderStructureFolder> Children { get { return children; } set { children = value; } }
    }
}
