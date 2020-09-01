using System.Collections.Generic;

namespace AutoCare.Product.Search.Model
{
    public class SearchOptions
    {
        /// <summary>
        /// Count to retrieve number of records
        /// </summary>
        public int RecordCount { get; set; }
        public int PageNumber { get; set; }
        public bool ReturnTotalCount { get; set; }
        public List<string> OrderBy { get; set; }
        public List<string> FacetsToInclude { get; set; }
        public SearchMode SearchMode { get; set; }
        public List<string> SearchFields { get; set; }
    }
}