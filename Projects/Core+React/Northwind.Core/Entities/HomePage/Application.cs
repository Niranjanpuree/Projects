using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities.HomePage
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public int ApplicationCategoryId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        private string IconPath { get; set; }
        public string Url { get; set; }
        public bool IsInternalApplication { get; set; }
        public bool ShowAppForAllUsers { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
    }
}
