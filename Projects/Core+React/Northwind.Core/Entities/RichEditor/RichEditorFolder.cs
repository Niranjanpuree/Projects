using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.RichEditor
{
    public class RichEditorFolder
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }

    public class RichEditorFile
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
