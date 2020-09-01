using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Model
{

    public class ImportFile
    {
        public ImportFiles[] ImportFiles;
    }

    public class ImportFiles
    {
        public string Name { get; set; }
        public object CSVField { get; set; }
        public object Table { get; set; }
    }

    public class Table
    {
        public string Name { get; set; }
        public object Field { get; set; }
        public object Output { get; set; }
    }
}
