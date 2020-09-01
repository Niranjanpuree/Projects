using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Northwind.Core.Entities
{
    [Table("ResourceAttribute")]
    public class ResourceAttribute
    {
        [Key]
        public Guid ResourceAttributeGuid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ResourceType { get; set; }
        public ResourceAttributeType AttributeType {get; set;}
        public bool VisibleToGrid { get; set; }
        public bool Exportable { get; set; }
        public bool DefaultSortField { get; set; }
        public int GridFieldOrder { get; set; }
        public string GridColumnCss { get; set; }
        public string GridColumnFormat { get; set; }
        public bool Searchable { get; set; }
        public bool IsEntityLookup { get; set; }
        public string Entity { get; set; }
        public int ColumnWidth { get; set; }
        public int ColumnMinimumWidth { get; set; }
        public string HelpText { get; set; }
        public List<ResourceAttributeValue> ResourceAttributeValues { get; set; }
    }
}
