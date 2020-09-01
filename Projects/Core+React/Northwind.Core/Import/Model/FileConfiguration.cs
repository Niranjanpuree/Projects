using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class FileConfiguration
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string DeleteFileAfterProcessing { get; set; }
    }
}
