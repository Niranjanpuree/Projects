using System;

namespace AutoCare.Product.Vcdb.Model
{
    public interface IDomainEntity
    {
        int Id { get; set; }
        DateTime? InsertDate { get; set; }
        DateTime? LastUpdateDate { get; set; }
        DateTime? DeleteDate { get; set; }
        long? ChangeRequestId { get; set; }
    }
}
