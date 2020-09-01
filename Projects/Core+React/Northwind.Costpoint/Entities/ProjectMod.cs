using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class ProjectModCP
    {
        /* Matches the select results for the query
         * 
         * If changing the Query you need to change the CSV file and Database Resource Attributes
         * Locations:
         * ~Local Project Location~\ESS-Web\src\DBMigration\flyway-5.2.4\DataSourceCSV\ResourceAttribute.csv
         * sqlD-01
         * Ess_new - table - ResrouceAttribute - filter for PFS-ProjectMod
         * */
        //public long ProjectModId { get; set; }
        public string ProjectModId { get; set; }
        public string ProjectNumber { get; set; }
        public string ModNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AwardDate { get; set; }
        public DateTime POPStartDate { get; set; }
        public DateTime POPEndDate { get; set; }
        public decimal AwardAmount { get; set; }
        public decimal FundedAmount { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Fee { get; set; }

        //ADDED for contract brief..
        public decimal? TotalAmount { get; set; }
    }
}
