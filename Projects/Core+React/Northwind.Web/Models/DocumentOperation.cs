using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class DocumentOperation
    {
        public string Path { get; set; }
        [Display(Name = "Folder Name")]
        [Required(ErrorMessage = "Folder name is required.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Operation { get; set; }
        public string Source { get; set; }
        public string ResourceType { get; set; }
        public Guid ResourceId { get; set; }
        public Guid ParentId { get; set; }
        public Guid FileId { get; set; }
        public Guid DestinationGuid { get; set; }
        public Guid DestinationParentGuid { get; set; }
    }
}
