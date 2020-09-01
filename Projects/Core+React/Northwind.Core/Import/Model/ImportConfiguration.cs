using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class ImportConfiguration
    {
        public string InputFolderPath { get; set; }
        public string LogOutputPath { get; set; }
        public object CSVToAttributeMapping { get; set; }
    }
}
