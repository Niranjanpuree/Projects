using System.Collections.Generic;

namespace AutoCare.Product.Search.Model
{
    public class Facet
    {
        public string Name { get; set; }
        public IList<FacetValue> Value { get; set; }
    }
}
