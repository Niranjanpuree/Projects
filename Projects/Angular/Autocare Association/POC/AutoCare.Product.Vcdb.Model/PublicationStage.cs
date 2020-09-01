using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class PublicationStage: EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
