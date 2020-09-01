using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities.HomePage
{
    public class ApplicationCategory
    {
        [Key]
        public int ApplicationCategoryId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }
        public List<Application> Applications { get; set; }
    }
}
