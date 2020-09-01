using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMErrorLog
    {
        public string File { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
