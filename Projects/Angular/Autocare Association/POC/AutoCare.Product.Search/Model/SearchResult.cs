using System.Collections.Generic;

namespace AutoCare.Product.Search.Model
{
    public class SearchResult<T> where T: class
    {
        public long? TotalCount { get; set; }
        public IList<Facet> Facets { get; set; }
        public IList<T> Documents { get; set; }
    }
}
