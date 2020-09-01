using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public interface IPersonifyHelper
    {
        Task<AutoCareUser> GetUserAsync(string username, string password);
        Task<string> GetCustomerTokenAsync(string username, string password);
        Task<List<string>> GetRolesAsync(string customerId);
        Task<List<string>> GetCustomersByRoleAsync(string role);
    }
}