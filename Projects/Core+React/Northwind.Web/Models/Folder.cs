using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class Folder
    {

        public string Name { get; set; }
        public string RelativePath { get; set; }
        private List<Folder> subFolders = new List<Folder>();
        public string Type { get; set; }

        public List<Folder> SubFolders
        {
            get { return subFolders; }
            set { subFolders = value; }
        }

    }
}
