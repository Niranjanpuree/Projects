using System;
using AutoMapper;
using Northwind.Core.Entities;
using Northwind.Web.Models.ViewModels;
using Contract = Northwind.Core.Entities.Contract;
using Northwind.Web.Models.ViewModels.Contract;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Models.ViewModels.FarClause;
using Northwind.Core.Entities.HomePage;
using Northwind.Web.Models.ViewModels.Article;

//using Northwind.Core.Entities;

namespace Northwind.Web.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasicContractInfoModel, Models.ViewModels.Contract.BasicContractInfoViewModel>();
            CreateMap<Contract, Models.ViewModels.Contract.ContractViewModel>().ReverseMap();
            CreateMap<ContractViewModel, Contract>();
            CreateMap<ContractWBS, Models.ViewModels.Contract.ContractWBSViewModel>().ReverseMap();
            CreateMap<ContractModification, Models.ViewModels.Contract.ContractModificationViewModel>().ReverseMap();
            CreateMap<ContractNotice, Models.ViewModels.Contract.ContractNoticeViewModel>().ReverseMap();
            CreateMap<CustomerAttributeValuesModel, Models.ViewModels.Contract.CustomerAttributeValuesViewModel>().ReverseMap();
            CreateMap<CustomerInformationModel, Models.ViewModels.Contract.CustomerInformationViewModel>().ReverseMap();
            CreateMap<CustomerContact, Models.ViewModels.CustomerContactViewModel>().ReverseMap();
            CreateMap<DistributionListViewModel, DistributionList>().ReverseMap();
            CreateMap<DistributionListViewModel, DistributionUser>().ReverseMap();
            CreateMap<DistributionList, DistributionListViewModel>().ReverseMap();
            CreateMap<DistributionUser, DistributionListViewModel>().ReverseMap();
            CreateMap<EmployeeBillingRates, Models.ViewModels.EmployeeBillingRatesViewModel>().ReverseMap();
            CreateMap<FinancialInformationModel, Models.ViewModels.Contract.FinancialInformationViewModel>().ReverseMap();
            CreateMap<Models.ViewModels.JobRequestViewModel, JobRequest>().ReverseMap();
            CreateMap<KeyPersonnelModel, Models.ViewModels.Contract.KeyPersonnelViewModel>().ReverseMap();
            CreateMap<Core.Entities.KeyValuePairModel<Guid, string>, Models.ViewModels.KeyValuePairModel<Guid, string>>();
            CreateMap<Core.Entities.KeyValuePairWithDescriptionModel<Guid, string,bool>, Models.ViewModels.KeyValuePairWithDescriptionModel<Guid, string,bool>>();
            CreateMap<LaborCategoryRates, Models.ViewModels.LaborCategoryRatesViewModel>().ReverseMap();
            CreateMap<ProjectModificationModel, Models.ViewModels.Project.ProjectModificationViewModel>().ReverseMap();
            CreateMap<RevenueRecognition, Models.ViewModels.RevenueRecognition.RevenueRecognitionViewModel>().ReverseMap();
            CreateMap<RevenueContractExtension, Models.ViewModels.RevenueRecognition.RevenueContractExtensionViewModel>().ReverseMap();
            CreateMap<RevenuePerformanceObligation, Models.ViewModels.RevenueRecognition.RevenuePerformanceObligationViewModel>().ReverseMap();
            CreateMap<ContractQuestionaire, Models.ViewModels.Contract.ContractQuestionaireViewModel>().ReverseMap();
            CreateMap<Contracts, Models.ViewModels.Contract.ContractAndProjectView>().ReverseMap();
            CreateMap<Contracts, Models.ViewModels.Contract.ContractListViewModel>();
            CreateMap<ContractResourceFile, Models.ViewModels.Contract.ContractFileViewModel>().ReverseMap();
            CreateMap<ContractResourceFile, Models.ViewModels.Contract.ContractWBSViewModel>().ReverseMap();
            CreateMap<ContractResourceFile, Models.ViewModels.EmployeeBillingRatesViewModel>().ReverseMap();
            CreateMap<ContractResourceFile, Models.ViewModels.LaborCategoryRatesViewModel>().ReverseMap();
            CreateMap<User, ContractUserViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<FarContractTypeClause, FarContractTypeClauseViewModel>();
            CreateMap<Article,ArticleViewModel>().ReverseMap();
        }
    }
}
