using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class AttachmentModel
    {
        public string NodeID { get; set; }
        public string SourcePath { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

    }
}
