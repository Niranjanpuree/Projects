using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities.HomePage
{
    public class ArticleType
    {
        [Key]
        public int ArticleTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
