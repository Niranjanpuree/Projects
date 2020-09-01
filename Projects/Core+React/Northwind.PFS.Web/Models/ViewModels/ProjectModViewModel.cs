using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Northwind.PFS.Web.Models.ViewModels
{
    public class ProjectModViewModel
    {
        
        public string ProjectModId { get; set; }
        public string ProjectNumber { get; set; }
        [JsonProperty("proj_mod_id")]
        public string ModNumber { get; set; }
        [JsonProperty("proj_mod_desc")]
        public string Title { get; set; }     
        public string Description { get; set; }
        [JsonProperty("effect_dt")]
        public DateTime AwardDate { get; set; }
        [JsonProperty("proj_start_dt")]
        public DateTime POPStartDate { get; set; }
        [JsonProperty("proj_end_dt")]
        public DateTime POPEndDate { get; set; }
        [JsonProperty("proj_v_cst_amt")]
        public decimal AwardAmount { get; set; }
        [JsonProperty("proj_f_cst_amt")]
        public decimal FundedAmount { get; set; }

        public string Currency { get; set; }
        public int Id { get; set; }
        private List<ProjectModAttachments> attachments = new List<ProjectModAttachments>();

        public List<ProjectModAttachments> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

    }

    public class ProjectModAttachments
    {
        public string Title { get; set; }
        public string DownloadLink { get; set; }
    }
}
