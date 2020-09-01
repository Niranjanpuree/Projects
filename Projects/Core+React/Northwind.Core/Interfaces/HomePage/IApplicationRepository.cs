using Northwind.Core.Entities.HomePage;
using Northwind.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces.HomePage
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<ApplicationCategory>> GetApplicationCategories();
        Task<ApplicationCategory> GetApplicationCategory(int applicationCategoryId);
        Task<IEnumerable<Application>> GetApplications(int applicationCategoryId, SearchSpec searchSpec);
        Task<IEnumerable<ApplicationCategory>> GetUserMenuTree(Guid userGuid);
        Task<Application> GetApplication(int applicationId);
        Task<int> Add(Application application);
        Task<int> Update(Application application);
        Task<int> Delete(Application application);
    }
}
