using System.Data.Entity;

namespace AutoCare.Product.Application.Models.EntityModels
{
    public class PadbContext : DbContext
    {
        public PadbContext() : base("PadbConnection")
        {
        }
    }
}
