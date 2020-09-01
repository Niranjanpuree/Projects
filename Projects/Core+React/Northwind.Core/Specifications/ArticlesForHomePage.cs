using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Specifications
{
   public class ArticleTypes
    {
       public List<ArticlesCount> Articles { get; set; }
    }

    public class ArticlesCount
    {
        public int Count { get; set; }
        public EnumGlobal.ArticleType ArticleType { get; set; }
    }
}
