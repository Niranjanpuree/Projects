using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.AuditLog.Services;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Service;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.Base;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Interfaces.DocumentMgmt;
using Northwind.Core.Interfaces.HomePage;
using Northwind.Core.Interfaces.RichEditor;
using Northwind.Core.Interfaces.Sync;
using Northwind.Core.Services;
using Northwind.Core.Services.ContractRefactor;
using Northwind.Core.Services.DocumentMgmt;
using Northwind.Core.Services.HomePage;
using Northwind.Core.Services.RichEditor;
using Northwind.Core.Services.SyncServices;
using Northwind.Core.Utilities;
using Northwind.Infrastructure.AuditLog.Data;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Data.Admin;
using Northwind.Infrastructure.Data.Common;
using Northwind.Infrastructure.Data.Contract;
using Northwind.Infrastructure.Data.Contract.ContractRefactor;
using Northwind.Infrastructure.Data.Country;
using Northwind.Infrastructure.Data.CustomerContactType;
using Northwind.Infrastructure.Data.CustomerType;
using Northwind.Infrastructure.Data.DistributionUser;
using Northwind.Infrastructure.Data.DocumentMgmt;
using Northwind.Infrastructure.Data.EmployeeBillingRatesRepo;
using Northwind.Infrastructure.Data.FarClauseRepo;
using Northwind.Infrastructure.Data.HomePage;
using Northwind.Infrastructure.Data.JobRequest;
using Northwind.Infrastructure.Data.Notification;
using Northwind.Infrastructure.Data.Project;
using Northwind.Infrastructure.Data.Questinoaries;
using Northwind.Infrastructure.Data.QustionaireContractClose;
using Northwind.Infrastructure.Data.RecentActivity;
using Northwind.Infrastructure.Data.RevenueRecognitionRepo;
using Northwind.Infrastructure.Data.RichEditor;
using Northwind.Infrastructure.Data.State;
using Northwind.Infrastructure.Data.SubcontractorBillingRatesRepo;
using Northwind.Infrastructure.Data.Sync;
using Northwind.Infrastructure.Data.UsCustomerOffice;
using Northwind.Infrastructure.Data.WorkBreakdownStructure;

namespace Northwind.Infrastructure.DependencyInjection
{
    public class InfrastructureDependency
    {

        public static IServiceCollection RegisterAppCoreMigration(IServiceCollection services)
        {
            RegisterCoreRepositories(services);
            RegisterCoreServices(services);
            RegisterWebRequiredServices(services);
            RegisterUserManagementRepositories(services);
            return services;
        }

        public static void RegisterMemoryService(IServiceCollection services)
        {
        }

        public static void RegisterAppCore(IServiceCollection services)
        {
            RegisterCoreRepositories(services);
            RegisterCoreServices(services);
        }

        public static void RegisterWebRequiredServices(IServiceCollection services)
        {
            RegisterGeneralLookUpRepositories(services);
            RegisterContractRepositories(services);
            RegisterGeneralLookUpServices(services);
            RegisterAdditionalUserServices(services);
            RegisterDistributionListServices(services);
            RegisterContractServices(services);

            //TODO: Remove this registration when contract import command line is built.
            RegisterImportServices(services);
            
                    
        }

        public static void RegisterCoreRepositories(IServiceCollection services)
        {
            //Register Repositories Related to Resources. These repositories are Default 
            //implementations to provide queries to Resource and Associated Tables
            services.AddScoped<IResourceAttributeValueRepository, ResourceAttributeValueRepository>();
            services.AddScoped<IResourceAttributeRepository, ResourceAttributeRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IResourceActionRepository, ResourceActionRepository>();
            services.AddScoped<IQueryOperatorRepository, QueryOperatorRepository>();

            //Register User Management Related Repositories 
            RegisterUserManagementRepositories(services);      
            
            
            //Register Repositories Required For Notification Features
            services.AddScoped<INotificationMessageRepository, NotificationMessageRepository>();
            services.AddScoped<INotificationBatchRepository, NotificationBatchRepository>();
            services.AddScoped<INotificationMessageRepository, NotificationMessageRepository>();
            services.AddScoped<INotificationTemplatesRepository, NotificationTemplatesRepository>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();

            //Register Repositories Required for User Interface Menu
            services.AddScoped<IUserInterfaceMenuRepository, UserInterfaceMenuRepository>();

            //Register Repositories Required for Documentat Management
            services.AddScoped<IDocumentManagementRepository, DocumentManagementRepository>();
            services.AddScoped<IFolderStructureMasterRepository, FolderStructureMasterRepository>();
            services.AddScoped<IFolderStructureFolderRepository, FolderStructureFolderRepository>();
            services.AddScoped<IContractResourceFileRepository , ContractResourceFileRepository>();

            services.AddScoped<IArticleRepository, ArticleRepository>();

            services.AddScoped<IRecentActivityRepository, RecentActivityRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IEventLogRepository, EventLogRepository>();

            services.AddScoped<IRichEditorRepository, RichEditorRepository>();
        }

        private static void RegisterUserManagementRepositories(IServiceCollection services)
        {
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IUserRepository, ESSUserRepository>();
            services.AddScoped<IManagerUserRepository, ManagerUserRepository>();
            services.AddScoped<IGroupRepository, ESSGroupRepository>();
            services.AddScoped<IGroupUserRepository, GroupUserRepository>();
            services.AddSingleton<IGroupPermissionRepository, GroupPermissionRepository>();
            services.AddSingleton<IPolicyRepository, PolicyRepository>();
        }

        private static void RegisterGeneralLookUpRepositories(IServiceCollection services)
        {
            //Register General Look Up Repositories
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerContactRepository, CustomerContactRepository>();
            services.AddScoped<ICustomerContactTypeRepository, CustomerContactTypeRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<INaicsRepository, NaicsRepository>();
            services.AddScoped<IPscRepository, PscRepository>();
            //Distribution List Related Repositories
            services.AddScoped<IDistributionUserRepository, DistributionUserRepository>();
            services.AddScoped<IDistributionListRepository, DistributionListRepository>();
            services.AddScoped<ISyncBatchRepository, SyncBatchRepository>();
            services.AddScoped<ISyncStatusRepository, SyncStatusRepository>();

        }

        private static void RegisterContractRepositories(IServiceCollection services)
        {
            //TODO: There are duplicate repository for Contract. Re-factor and make this one.
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IContractsRepository, ContractsRepository>();
            services.AddScoped<IProjectModificationRepository, ProjectModificationRepository>();
            services.AddScoped<IContractModificationRepository, ContractModificationRepository>();
            services.AddScoped<IRevenueRecognitionRepository, RevenueRecognitionRepository>();
            services.AddScoped<IJobRequestRepository, JobRequestRepository>();
            services.AddScoped<IUsCustomerOfficeListRepository, UsCustomerOfficeListRepository>();
            services.AddScoped<IContractWBSRepository, ContractWBSRepository>();
            services.AddScoped<IEmployeeBillingRatesRepository, EmployeeBillingRatesRepository>();
            services.AddScoped<ISubcontractorBillingRatesRepository, SubcontractorBillingRatesRepository>();
            services.AddScoped<IContractQuestionariesRepository, ContractQuestionariesRepository>();
            services.AddScoped<IFarContractTypeRepository, FarContractTypeRepository>();
            services.AddScoped<IFarClauseRepository, FarClauseRepository>();
            services.AddScoped<IFarContractTypeClauseRepository, FarContractTypeClauseRepository>();
            services.AddScoped<IFarContractTypeClauseRepository, FarContractTypeClauseRepository>();
            services.AddScoped<IContractNoticeRepository, ContractNoticeRepository>();
            services.AddScoped<IQuestionaireMasterRepository, QuestionaireMasterRepository>();
            services.AddScoped<IQuestionaireUserAnswerRepository, QuestionaireUserAnswerRepository>();
            services.AddScoped<IQuestionaireManagerTypeRepository, QuestionaireManagerTypeRepository>();
            services.AddScoped<IContractCloseApprovalRepository, ContractCloseApprovalRepository>();
            services.AddScoped<IFarContractRepository, FarContractRepository>();
        }

        private static void RegisterCoreServices(IServiceCollection services)
        {

            //TODO: Combine these three services into one service.
            //Resource Services
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IResourceAttributeService, ResourceAttributeService>();
            services.AddScoped<IResourceAttributeValueService, ResourceAttributeValueService>();

            RegisterCoreUserManagementServices(services);
                      
           
            services.AddScoped<IMenuService, MenuService>();

            services.AddScoped<IApplicationService, ApplicationService>();

            //TODO: Combine into one
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationMessageService, NotificationMessageService>();
            services.AddScoped<INotificationBatchService, NotificationBatchService>();
            services.AddScoped<IGenericNotificationService, GenericNotificationService>();
            services.AddScoped<INotificationTemplatesService, NotificationTemplateService>();
            services.AddScoped<INotificationTypeService, NotificationTypeService>();

            //TODO: Combine into one
            services.AddScoped<IFolderStructureMasterService, FolderStructureMasterService>();
            services.AddScoped<IFolderStructureFolderService, FolderStructureFolderService>();
            services.AddScoped<IDocumentManagementService, DocumentManagementService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<IContractResourceFileService, ContractResourceFileService>();

            services.AddScoped<IRecentActivityService, RecentActivityService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IEventLogService, EventLogService>();

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IRichEditorService, RichEditorService>();

        }

        private static void RegisterCoreUserManagementServices(IServiceCollection services)
        {
            //TODO: Combine into one
            services.AddSingleton<IPolicyService, PolicyService>();
            services.AddSingleton<IGroupPermissionService, GroupPermissionService>();
            services.AddScoped<IUserService, UserService>();
        }

        private static void RegisterAdditionalUserServices(IServiceCollection services)
        {
            //TODO: Possibly Comine Into One
            services.AddScoped<IManagerUserService, ManagerUserService>();
            services.AddScoped<IUserSyncService, UserSyncService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IADGroupSyncService, ADGroupSyncService>();
            services.AddScoped<IGroupUserService, GroupUserService>();
            services.AddScoped<ISyncBatchService, SyncBatchService>();
            services.AddScoped<ISyncStatusService, SyncStatusService>();

        }             

        private static void RegisterGeneralLookUpServices(IServiceCollection services)
        {
            //TODO: Combine into 1
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IPscService, PscService>();
            services.AddScoped<INaicsService, NaicsService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IOfficeContactService, OfficeContactService>();
            //TODO: Combine into 1
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerContactTypeService, CustomerContactTypeService>();
            services.AddScoped<IUsCustomerOfficeListService, UsCustomerOfficeListService>();
            services.AddScoped<ICustomerTypeService, CustomerTypeService>();
            //TODO: Combine into 1
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICountryService, CountryService>();
        }        

        private static void RegisterDistributionListServices(IServiceCollection services)
        {
                       
            //TODO: Combine into 1
            services.AddScoped<IDistributionUserService, DistributionUserService>();
            services.AddScoped<IDistributionListService, DistributionListService>();      
           
        }
               
        private static void RegisterContractServices(IServiceCollection services)
        {
            //services.AddScoped<IContractService, ContractService>();
            services.AddScoped<ICustomerContactService, CustomerContactService>();
            services.AddScoped<IProjectModificationService, ProjectModificationService>();
            services.AddScoped<IContractModificationService, ContractModificationService>();
            services.AddScoped<IRevenueRecognitionService, RevenueRecognitionService>();
            services.AddScoped<IJobRequestService, JobRequestService>();
            services.AddScoped<IContractsService, ContractsService>();
            services.AddScoped<IValidationHelper, ValidationHelper>();
            //TODO: Combine into one
            services.AddScoped<IFarClauseService, FarClauseService>();
            services.AddScoped<IFarContractTypeClauseService, FarContractTypeClauseService>();
            services.AddScoped<IFarContractService, FarContractService>();
            services.AddScoped<IFarContractTypeService, FarContractTypeService>();

            //TODO: Combine into one
            services.AddScoped<IContractWBSService, ContractWBSService>();
            services.AddScoped<IEmployeeBillingRatesService, EmployeeBillingRatesService>();
            services.AddScoped<ISubcontractorBillingRatesService, SubcontractorBillingRatesService>();
            services.AddScoped<IContractNoticeService, ContractNoticeService>();

            //TODO: Combine into one
            services.AddScoped<IContractQuestionariesService, ContractQuestionariesService>();
            services.AddScoped<IQuestionaireMasterService, QuestionaireMasterService>();
            services.AddScoped<IQuestionaireUserAnswerService, QuestionaireUserAnswerService>();
            services.AddScoped<IQuestionaireManagerTypeService, QuestionaireManagerTypeService>();
            services.AddScoped<IContractCloseApprovalService, ContractCloseApprovalService>();
        }

        public static IServiceCollection RegisterImportServices(IServiceCollection services)
        {
            #region Import data
            services.AddScoped<Core.Import.Interface.IImportFileService, ImportFileService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IRegionImportService, RegionImportService>();
            services.AddScoped<IExportCSVService, ExportCSVService>();
            services.AddScoped<ICompanyImportService, CompanyImportService>();
            services.AddScoped<ICustomerContactImportService, CustomerContactImportService>();
            services.AddScoped<ICustomerImportService, CustomerImportService>();
            services.AddScoped<IOfficeImportService, OfficeImportService>();
            services.AddScoped<IModImportService, ModImportService>();
            services.AddScoped<IContractImportService, ContractImportService>();
            services.AddScoped<ICommonImportService, CommonImportService>();
            services.AddScoped<IAttachmentImportService, AttachmentImportService>();
            services.AddScoped<IFarClauseImportService, FarClauseImportService>();
            return services;
            #endregion import data
        }


    }
}
