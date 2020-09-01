using System;

namespace AutoCare.Product.Vcdb.Model
{
    public abstract class EntityBase
    {
        [IdentityProperty]
        public int Id { get; set; }
        [InsertedOnProperty]
        public DateTime? InsertDate { get; set; }
        [UpdatedOnProperty]
        public DateTime? LastUpdateDate { get; set; }
        [DeletedOnProperty]
        public DateTime? DeleteDate { get; set; }
    }
}
