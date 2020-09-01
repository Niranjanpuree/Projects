using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.HomePage
{
    public class Article
    {
        public int ArticleId { get; set; }
        public int ArticleTypeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Body { get; set; }
        public bool IsLocalMedia { get; set; }
        public string PrimaryMediaPath { get; set; }
        public string PrimaryMediaUrl { get; set; }
        public string MediaCaption { get; set; }
        public ArticleStatus Status { get; set; }
        public bool IsFeatured { get; set; }
        public bool ShowInArchive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public string ArticleTypeName { get; set; }
    }
}
