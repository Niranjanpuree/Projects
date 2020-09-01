using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Source :EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
