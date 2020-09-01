using AutoCare.Product.Infrastructure.Command;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.VcdbSearch.Indexing.Command
{
    public class ApplyMakeNameChange : ICommand
    {
        public Make Make { get; set; }
    }
}
