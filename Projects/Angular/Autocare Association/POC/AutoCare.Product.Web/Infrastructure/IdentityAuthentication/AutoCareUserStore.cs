using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class AutoCareUserStore : IUserStore<AutoCareUser>, IUserStore<AutoCareUser, string>, 
        IUserRoleStore<AutoCareUser>, IUserRoleStore<AutoCareUser, string>
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task CreateAsync(AutoCareUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AutoCareUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(AutoCareUser user)
        {
            throw new NotImplementedException();
        }

        public Task<AutoCareUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<AutoCareUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(AutoCareUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(AutoCareUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(AutoCareUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsInRoleAsync(AutoCareUser user, string roleName)
        {
            throw new NotImplementedException();
        }
    }
}