using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.DocumentMgmt
{
    public class FolderStructureMaster
    {
        public Guid FolderStructureMasterGuid { get; set; }
        public string Module { get; set; }
        public string ResourceType { get; set; }
        public bool Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
