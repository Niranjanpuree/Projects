using Northwind.Core.Entities.HomePage;
using Northwind.Core.Interfaces.HomePage;
using Northwind.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Core.Services.HomePage
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<int> Add(Application application)
        {
            return await _applicationRepository.Add(application);
        }

        public async Task<int> Delete(Application application)
        {
            return await _applicationRepository.Delete(application);
        }

        public async Task<Application> GetApplication(int applicationId)
        {
            return await _applicationRepository.GetApplication(applicationId);
        }

        public async Task<IEnumerable<ApplicationCategory>> GetApplicationCategories()
        {
            return await _applicationRepository.GetApplicationCategories();
        }

        public async Task<ApplicationCategory> GetApplicationCategory(int applicationCategoryId)
        {
            return await _applicationRepository.GetApplicationCategory(applicationCategoryId);
        }

        public async Task<IEnumerable<Application>> GetApplications(int applicationCategoryId, SearchSpec searchSpec)
        {
            return await _applicationRepository.GetApplications(applicationCategoryId, searchSpec);
        }

        public async Task<IEnumerable<ApplicationCategory>> GetUserMenuTree(Guid userGuid)
        {
            return await _applicationRepository.GetUserMenuTree(userGuid);
        }

        public async Task<int> Update(Application application)
        {
            return await _applicationRepository.Update(application);
        }
    }
}
