using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Article
{
    public class ArticleViewModel
    {
        public Guid ArticleGuid { get; set; }
        [Required]
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        //[Required]
        [DisplayName("Front Page Section")]
        public string FrontPageSection { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        [Required]
        public bool ShowInArchive { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Excerpt { get; set; }
        public string MediaCaption { get; set; }
        public string Body { get; set; }
        public int ArticletypeId { get; set; }
        public int ArticleId { get; set; }
        public string Action { get; set; }
    }
}
