using AutoCare.Product.Infrastructure.Command;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.VcdbSearch.Indexing.Command
{
    public class ApplyChangeRequestChanges : ICommand
    {
        public long ChangeRequestId { get; set; }
        public ChangeType ChangeType { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Payload { get; set; }
    }
}
