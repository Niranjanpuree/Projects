using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.ViewModels;
using System.Linq;
using System.Text;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Infrastructure.Data.Project
{
    public class ProjectRepository : IProjectRepository
    {
        IDatabaseContext _context;
        public ProjectRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<ProjectViewModel> GetAll(Guid ContractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();

            if (sortField.ToLower().Equals("contracttitle"))
            {
                orderingQuery.Append($"contract.ContractTitle {sortDirection}");
            }
            else
            {
                orderingQuery.Append($"project.{sortField} {sortDirection}");
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                conditionalQuery.Append($"and Project.ProjectNumber like '%{searchValue}%'");
            }

            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                        (SELECT ROW_NUMBER() OVER (ORDER BY {orderingQuery}) AS RowNum, 
                                                                                        ProjectGuid,
                                                                                        Project.ProjectNumber ProjectNumber,
                                                                                        ProjectTitle ProjectTitle,
                                                                                        Project.IsActive,
                                                                                        Project.ORGID,
                                                                                        Project.AwardAmount AwardAmount,
                                                                                        Project.POPStart POPStart,
                                                                                        Project.POPEnd POPEnd,
                                                                                        Project.UpdatedOn,
                                                                                        Contract.ContractNumber,
                                                                                        Contract.ContractTitle
                                                                                        from Project
                                                                                        left outer join
                                                                                         Contract
                                                                                        on Project.ContractGuid = Contract.ContractGuid
                                                                                        where Project.IsDeleted = 0
                                                                                        {conditionalQuery} 
                                                                                        and contract.ContractGuid= '{ContractGuid}'
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > {skip} 
                                            AND RowNum <= {pageSize + skip}  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<ProjectViewModel>(pagingQuery);
            return pagedData;
        }

        public int TotalRecord(Guid ContractGuid)
        {
            string sql = $@"SELECT Count(1) 
                            from Project
                               left join
                                 Contract
                                on Project.ContractGuid = Contract.ContractGuid
                                where Project.IsDeleted = 0
                                and contract.ContractGuid= '{ContractGuid}'";
            var result = _context.Connection.QuerySingle<int>(sql);
            return result;
        }
        public int AddProject(Core.Entities.Project ProjectModel)
        {
            string insertQuery = $@"INSERT INTO [dbo].[Project]
                                                                   (
                                                                        ProjectGuid					                                ,
                                                                        ContractGuid					                            ,
                                                                        ORGID						                                ,
                                                                        ProjectNumber				                                ,
                                                                        ProjectTitle				                                ,
                                                                        Description				                                    ,
                                                                        CountryOfPerformance			                            ,
                                                                        PlaceOfPerformance			                                ,
                                                                        POPStart						                            ,
                                                                        POPEnd						                                ,
                                                                        NAICSCode					                                ,
                                                                        PSCCode						                                ,
                                                                        QualityLevelRequirements		                            ,
                                                                        QualityLevel					                            ,
                                                                                                                                    
                                                                        CompanyPresident					                        ,
                                                                        RegionalManager					                            ,
                                                                        ProjectManager					                            ,
                                                                        ProjectControls					                            ,
                                                                        AccountingRepresentative			                        ,
                                                                        ProjectRepresentative			                            ,
                                                                                                                                    
                                                                        AwardingAgencyOffice				                        ,
                                                                        Office_ProjectRepresentative		                        ,
                                                                        Office_ProjectTechnicalRepresent                           ,
                                                                        FundingAgencyOffice				                            ,
                                                                        FundingOffice_ProjectRepresentative				        ,
                                                                        FundingOffice_ProjectTechnicalRepresent				    ,
                                                                                                                                    
                                                                        SetAside						                            ,
                                                                        SelfPerformance_Percent			                            ,
                                                                        SBA								                            ,
                                                                        Competition						                            ,
                                                                        ProjectType					                            ,
                                                                        OverHead						                            ,
                                                                        G_A_Percent						                            ,
                                                                        Fee_Percent						                            ,
                                                                        Currency						                            ,
                                                                        BlueSkyAward_Amount				                            ,
                                                                        AwardAmount						                            ,
                                                                        FundingAmount					                            ,
                                                                        BillingAddress					                            ,
                                                                        BillingFrequency				                            ,
                                                                        InvoiceSubmissionMethod			                            ,
                                                                        PaymentTerms					                            ,
                                                                        AppWageDetermine_DavisBaconAct				                ,
                                                                        AppWageDetermine_ServiceProjectAct				            ,
                                                                        BillingFormula					                            ,
                                                                        RevenueFormula					                            ,
                                                                        RevenueRecognitionEAC_Percent	                            ,
                                                                        OHonsite						                            ,
                                                                        OHoffsite						                            ,

                                                                        ProjectCounter						                        ,
                                                                        CreatedOn						                            ,
                                                                        UpdatedOn						                            ,
                                                                        CreatedBy						                            ,
                                                                        UpdatedBy						                            ,
                                                                        IsActive						                            ,
                                                                        IsDeleted                                                   
                                                                    )
                                  VALUES (
                                                                        '{ProjectModel.ProjectGuid                                                       }'      ,
                                                                        '{ProjectModel.ContractGuid                                                      }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.ORGID                                            }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.ProjectNumber                                    }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.ProjectTitle                                     }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.Description                                      }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.CountryOfPerformance                             }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.PlaceOfPerformanceSelectedIds                    }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.POPStart                                         }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.POPEnd                                           }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.NAICSCode                                        }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.PSCCode                                          }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.QualityLevelRequirements                         }'      ,
                                                                        '{ProjectModel.BasicProjectInfo.QualityLevel                                     }'      ,
                                                                                                                                                           
                                                                        '{ProjectModel.KeyPersonnel.CompanyPresident                                      }'      ,
                                                                        '{ProjectModel.KeyPersonnel.RegionalManager                                       }'      ,
                                                                        '{ProjectModel.KeyPersonnel.ProjectManager                                        }'      ,
                                                                        '{ProjectModel.KeyPersonnel.ProjectControls                                       }'      ,
                                                                        '{ProjectModel.KeyPersonnel.AccountingRepresentative                              }'      ,
                                                                        '{ProjectModel.KeyPersonnel.ProjectRepresentative                                 }'      ,
                                                                                                                                                           
                                                                        '{ProjectModel.CustomerInformation.AwardingAgencyOffice                           }'      ,	
                                                                        '{ProjectModel.CustomerInformation.Office_ProjectRepresentative                   }'      ,	
                                                                        '{ProjectModel.CustomerInformation.Office_ProjectTechnicalRepresent               }'      ,
                                                                        '{ProjectModel.CustomerInformation.FundingAgencyOffice                            }'      ,
                                                                        '{ProjectModel.CustomerInformation.FundingOffice_ProjectRepresentative            }'      ,
                                                                        '{ProjectModel.CustomerInformation.FundingOffice_ProjectTechnicalRepresent        }'      ,

                                                                        '{ProjectModel.FinancialInformation.setAside                                       }'      ,
                                                                        '{ProjectModel.FinancialInformation.SelfPerformance_Percent                        }'      ,
                                                                        '{ProjectModel.FinancialInformation.SBA                                            }'      ,
                                                                        '{ProjectModel.FinancialInformation.Competition                                    }'      ,
                                                                        '{ProjectModel.FinancialInformation.ProjectType                                    }'      ,
                                                                        '{ProjectModel.FinancialInformation.OverHead                                       }'      ,
                                                                        '{ProjectModel.FinancialInformation.G_A_Percent                                    }'      ,
                                                                        '{ProjectModel.FinancialInformation.Fee_Percent                                    }'      ,
                                                                        '{ProjectModel.FinancialInformation.Currency                                       }'      ,
                                                                        '{ProjectModel.FinancialInformation.BlueSkyAward_Amount                            }'      ,
                                                                        '{ProjectModel.FinancialInformation.AwardAmount                                    }'      ,
                                                                        '{ProjectModel.FinancialInformation.FundingAmount                                  }'      ,
                                                                        '{ProjectModel.FinancialInformation.BillingAddress                                 }'      ,
                                                                        '{ProjectModel.FinancialInformation.BillingFrequency                               }'      ,
                                                                        '{ProjectModel.FinancialInformation.InvoiceSubmissionMethod                        }'      ,
                                                                        '{ProjectModel.FinancialInformation.PaymentTerms                                   }'      ,
                                                                        '{ProjectModel.FinancialInformation.AppWageDetermine_DavisBaconAct                 }'      ,
                                                                        '{ProjectModel.FinancialInformation.AppWageDetermine_ServiceProjectAct             }'      ,
                                                                        '{ProjectModel.FinancialInformation.BillingFormula                                 }'      ,
                                                                        '{ProjectModel.FinancialInformation.RevenueFormula                                 }'      ,
                                                                        '{ProjectModel.FinancialInformation.RevenueRecognitionEAC_Percent                  }'      ,
                                                                        '{ProjectModel.FinancialInformation.OHonsite                                       }'      ,
                                                                        '{ProjectModel.FinancialInformation.OHoffsite                                      }'      ,

                                                                        '{ProjectModel.ProjectCounter                                                     }'      ,
                                                                        '{ProjectModel.CreatedOn                                                          }'      ,
                                                                        '{ProjectModel.UpdatedOn                                                          }'      ,
                                                                        '{ProjectModel.CreatedBy                                                          }'      ,
                                                                        '{ProjectModel.UpdatedBy                                                          }'      ,
                                                                        '{ProjectModel.IsActive                                                           }'      ,
                                                                        '{ProjectModel.IsDeleted                                                          }'
                                                                )";
            return _context.Connection.Execute(insertQuery);
        }

        public int UpdateProject(Core.Entities.Project ProjectModel)
        {
            string updateQuery = $@"Update Project       set 
                                                                      ProjectNumber				                =           '{ProjectModel.BasicProjectInfo.ProjectNumber                                     }'      ,
                                                                      ORGID						                =           '{ProjectModel.BasicProjectInfo.ORGID                                             }'      ,
                                                                      ProjectTitle				                =           '{ProjectModel.BasicProjectInfo.ProjectTitle                                      }'      ,
                                                                      Description				                =           '{ProjectModel.BasicProjectInfo.Description                                       }'      ,
                                                                      CountryOfPerformance			            =           '{ProjectModel.BasicProjectInfo.CountryOfPerformance                              }'      ,
                                                                      PlaceOfPerformance			            =           '{ProjectModel.BasicProjectInfo.PlaceOfPerformanceSelectedIds                     }'      ,
                                                                      POPStart						            =           '{ProjectModel.BasicProjectInfo.POPStart                                          }'      ,
                                                                      POPEnd						            =           '{ProjectModel.BasicProjectInfo.POPEnd                                            }'      ,
                                                                      NAICSCode					                =           '{ProjectModel.BasicProjectInfo.NAICSCode                                         }'      ,
                                                                      PSCCode						            =           '{ProjectModel.BasicProjectInfo.PSCCode                                           }'      ,
                                                                      QualityLevelRequirements		            =           '{ProjectModel.BasicProjectInfo.QualityLevelRequirements                          }'      ,
                                                                      QualityLevel					            =           '{ProjectModel.BasicProjectInfo.QualityLevel                                      }'      ,
                                                                                                                                                                                                             
                                                                      CompanyPresident					        =           '{ProjectModel.KeyPersonnel.CompanyPresident                                       }'      ,
                                                                      RegionalManager					        =           '{ProjectModel.KeyPersonnel.RegionalManager                                        }'      ,
                                                                      ProjectManager					        =           '{ProjectModel.KeyPersonnel.ProjectManager                                         }'      ,
                                                                      ProjectControls					        =           '{ProjectModel.KeyPersonnel.ProjectControls                                        }'      ,
                                                                      AccountingRepresentative			        =           '{ProjectModel.KeyPersonnel.AccountingRepresentative                               }'      ,
                                                                      ProjectRepresentative			            =           '{ProjectModel.KeyPersonnel.ProjectRepresentative                                      }'      ,
                                                                                                                                                                                                             
                                                                      AwardingAgencyOffice				        =           '{ProjectModel.CustomerInformation.AwardingAgencyOffice                            }'      ,	
                                                                      Office_ProjectRepresentative		        =           '{ProjectModel.CustomerInformation.Office_ProjectRepresentative                    }'      ,	
                                                                      Office_ProjectTechnicalRepresent          =           '{ProjectModel.CustomerInformation.Office_ProjectTechnicalRepresent                 }'      ,
                                                                      FundingAgencyOffice				        =           '{ProjectModel.CustomerInformation.FundingAgencyOffice                             }'      ,
                                                                      FundingOffice_ProjectRepresentative		=	        '{ProjectModel.CustomerInformation.FundingOffice_ProjectRepresentative             }'      ,
                                                                      FundingOffice_ProjectTechnicalRepresent	=	        '{ProjectModel.CustomerInformation.FundingOffice_ProjectTechnicalRepresent         }'      ,
                                                                                                                          
                                                                      SetAside						            =           '{ProjectModel.FinancialInformation.setAside                                       }'      ,
                                                                      SelfPerformance_Percent			        =           '{ProjectModel.FinancialInformation.SelfPerformance_Percent                        }'      ,
                                                                      SBA								        =           '{ProjectModel.FinancialInformation.SBA                                            }'      ,
                                                                      Competition						        =           '{ProjectModel.FinancialInformation.Competition                                    }'      ,
                                                                      ProjectType					            =           '{ProjectModel.FinancialInformation.ProjectType                                    }'      ,
                                                                      OverHead						            =           '{ProjectModel.FinancialInformation.OverHead                                       }'      ,
                                                                      G_A_Percent						        =           '{ProjectModel.FinancialInformation.G_A_Percent                                    }'      ,
                                                                      Fee_Percent						        =           '{ProjectModel.FinancialInformation.Fee_Percent                                    }'      ,
                                                                      Currency						            =           '{ProjectModel.FinancialInformation.Currency                                       }'      ,
                                                                      BlueSkyAward_Amount				        =           '{ProjectModel.FinancialInformation.BlueSkyAward_Amount                            }'      ,
                                                                      AwardAmount						        =           '{ProjectModel.FinancialInformation.AwardAmount                                    }'      ,
                                                                      FundingAmount					            =           '{ProjectModel.FinancialInformation.FundingAmount                                  }'      ,
                                                                      BillingAddress					        =           '{ProjectModel.FinancialInformation.BillingAddress                                 }'      ,
                                                                      BillingFrequency				            =           '{ProjectModel.FinancialInformation.BillingFrequency                               }'      ,
                                                                      InvoiceSubmissionMethod			        =           '{ProjectModel.FinancialInformation.InvoiceSubmissionMethod                        }'      ,
                                                                      PaymentTerms					            =           '{ProjectModel.FinancialInformation.PaymentTerms                                   }'      ,
                                                                      AppWageDetermine_DavisBaconAct			=	        '{ProjectModel.FinancialInformation.AppWageDetermine_DavisBaconAct                 }'      ,
                                                                      AppWageDetermine_ServiceProjectAct		=		    '{ProjectModel.FinancialInformation.AppWageDetermine_ServiceProjectAct             }'      ,
                                                                      BillingFormula					        =           '{ProjectModel.FinancialInformation.BillingFormula                                 }'      ,
                                                                      RevenueFormula					        =           '{ProjectModel.FinancialInformation.RevenueFormula                                 }'      ,
                                                                      RevenueRecognitionEAC_Percent	            =           '{ProjectModel.FinancialInformation.RevenueRecognitionEAC_Percent                  }'      ,
                                                                      OHonsite						            =           '{ProjectModel.FinancialInformation.OHonsite                                       }'      ,
                                                                      OHoffsite						            =           '{ProjectModel.FinancialInformation.OHoffsite                                      }'      ,

                                                                      UpdatedOn						            =           '{ProjectModel.UpdatedOn                                                           }'      ,
                                                                      UpdatedBy						            =           '{ProjectModel.UpdatedBy                                                           }'      ,
                                                                      IsActive						            =           '{ProjectModel.IsActive                                                            }'      ,
                                                                      IsDeleted                                 =           '{ProjectModel.IsDeleted                                                           }'
                                                                      where ProjectGuid = '{ProjectModel.ProjectGuid}' ";
            return _context.Connection.Execute(updateQuery);
        }

        public int DeleteProject(Guid[] ProjectGuidIds)
        {
            foreach (var ProjectGuid in ProjectGuidIds)
            {
                var Project = new
                {
                    ProjectGuid = ProjectGuid
                };
                string disableQuery = @"Update Project set 
                                               IsDeleted   = 1
                                               where ProjectGuid =@ProjectGuid ";
                _context.Connection.Execute(disableQuery, Project);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int DisableProject(Guid[] ProjectGuidIds)
        {
            foreach (var ProjectGuid in ProjectGuidIds)
            {
                var Project = new
                {
                    ProjectGuid = ProjectGuid
                };
                string disableQuery = @"Update Project set 
                                            IsActive   = 0
                                            where ProjectGuid =@ProjectGuid ";
                _context.Connection.Execute(disableQuery, Project);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int EnableProject(Guid[] ProjectGuidIds)
        {
            foreach (var ProjectGuid in ProjectGuidIds)
            {
                var Project = new
                {
                    ProjectGuid = ProjectGuid
                };
                string disableQuery = @"Update Project set 
                                            IsActive   = 1
                                            where ProjectGuid =@ProjectGuid ";
                _context.Connection.Execute(disableQuery, Project);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }

        public ICollection<Organization> GetOrganizationData(string searchText)
        {
            var organizationDataQuery = string.Format($@"
            select * from OrgID
                where Name like '%{@searchText}%' or Description like '%{@searchText}%' ");
            var organizationData = _context.Connection.Query<Organization>(organizationDataQuery).ToList();
            return organizationData;
        }

        public ICollection<Customer> GetAwardingAgencyOfficeData(string searchText)
        {
            string sql = $@"select 
                customer.CustomerGuid,
                customerType.CustomerTypeName,
                customer.Department,
                customer.Agency,
                customer.CustomerName, 
                customer.CustomerCode,
                customer.Address,
                customer.AddressLine1,
                customer.City,
                states.StateName,
                country.CountryName,
                customer.ZipCode,
                customer.PrimaryPhone,
                customer.PrimaryEmail,
                customer.Url,
                customer.Tags,
                customer.Abbreviations,
                customer.CustomerDescription,
                customer.Department
                 from Customer customer
                left join CustomerType customertype on customerType.CustomerTypeGuid = customer.CustomerTypeGuid
                left join State states on states.StateId = customer.StateId
                left join Country country on country.CountryId = customer.CountryId 
                where CustomerCode like  '%{searchText}%' or CustomerName like '%{searchText}%' 
                order by CustomerName asc";

            var data = _context.Connection.Query<Customer>(sql).ToList();
            return data;
        }

        public ICollection<Customer> GetFundingAgencyOfficeData(string searchText)
        {
            string sql = $@"select 
                customer.CustomerGuid,
                customerType.CustomerTypeName,
                customer.Department,
                customer.Agency,
                customer.CustomerName, 
                customer.CustomerCode,
                customer.Address,
                customer.AddressLine1,
                customer.City,
                states.StateName,
                country.CountryName,
                customer.ZipCode,
                customer.PrimaryPhone,
                customer.PrimaryEmail,
                customer.Url,
                customer.Tags,
                customer.Abbreviations,
                customer.CustomerDescription,
                customer.Department
                 from Customer customer
                left join CustomerType customertype on customerType.CustomerTypeGuid = customer.CustomerTypeGuid
                left join State states on states.StateId = customer.StateId
                left join Country country on country.CountryId = customer.CountryId 
                where CustomerCode like  '%{searchText}%' or CustomerName like '%{searchText}%' 
                order by CustomerName asc";

            var data = _context.Connection.Query<Customer>(sql).ToList();
            return data;
        }

        public ICollection<Naics> GetNAICSCodeData(string searchText)
        {
            var nAICSCodeQuery = string.Format($@"
            select * from NAICS
                where code like '%{@searchText}%' or title like '%{@searchText}%' ");
            var nAICSCode = _context.Connection.Query<Naics>(nAICSCodeQuery).ToList();
            return nAICSCode;
        }

        public ICollection<Psc> GetPSCCodeData(string searchText)
        {
            var pSCCodeQuery = string.Format($@"
            select * from PSC
                where code like '%{@searchText}%' or CodeDescription like '%{@searchText}%' ");
            var pSCSCode = _context.Connection.Query<Psc>(pSCCodeQuery).ToList();
            return pSCSCode;
        }

        public AssociateUserList GetCompanyRegionOfficeNameByCode(EntityCode entityCode)
        {
            AssociateUserList userList = new AssociateUserList();
            var companyPresidentQuery = string.Format($@"select Users.Displayname , Users.UserGuid , Users.JobTitle from Company left join Users on Company.President = Users.UserGuid where Company.CompanyCode = '{entityCode.CompanyCode}'");
            var regionalManagerQuery = string.Format($@"select Users.Displayname , Users.UserGuid , Users.JobTitle  from Region left join Users on Region.RegionalManager = Users.UserGuid where Region.RegionCode = '{entityCode.RegionCode}'");
            var companyNameQuery = string.Format($@"select Company.CompanyName from Company where CompanyCode = '{entityCode.CompanyCode}' and IsDeleted = 0");
            var regionNameQuery = string.Format($@"select Region.RegionName from Region where RegionCode = '{entityCode.RegionCode}' and IsDeleted = 0");
            var officeNameQuery = string.Format($@"select Office.OfficeName from Office where OfficeCode = '{entityCode.OfficeCode}' and IsDeleted = 0");
            userList.CompanyPresident = _context.Connection.QueryFirst<User>(companyPresidentQuery);
            userList.RegionManager = _context.Connection.QueryFirst<User>(regionalManagerQuery);
            userList.CompanyName = _context.Connection.QueryFirst<string>(companyNameQuery);
            userList.RegionName = _context.Connection.QueryFirst<string>(regionNameQuery);
            userList.OfficeName = _context.Connection.QueryFirst<string>(officeNameQuery);
            return userList;
        }

        public IEnumerable<User> GetUsersData(string searchText)
        {
            var usersQuery = string.Format($@"
               select * from Users
                where Firstname like '%{@searchText}%' or Lastname like '%{@searchText}%' 
                order by Displayname asc");
            var usersData = _context.Connection.Query<User>(usersQuery).ToList();
            return usersData;
        }

        public ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType)
        {
            var model = new List<KeyValuePairWithDescriptionModel<Guid, string, string>>();
            var sql =
                $@"SELECT CustomerContact.* FROM CustomerContact 
                join CustomerContactType on CustomerContact.ContactType = CustomerContactType.ContactTypeGuid
                where CustomerContactType.ContactType = '{contactType}' and CustomerGuid ='{customerId}' order by FirstName asc";
            var data = _context.Connection.Query<CustomerContact>(sql);
            foreach (var item in data)
            {
                model.Add(new KeyValuePairWithDescriptionModel<Guid, string, string> { Keys = item.ContactGuid, Values = item.FirstName + " " + item.MiddleName + " " + item.LastName, Descriptions = item.PhoneNumber });
            }
            return model;
        }

        public ProjectViewModel GetDetailsById(Guid id)
        {
            ProjectViewModel ProjectViewModel = new ProjectViewModel();
            string CompanyCodeSql = $"select SUBSTRING(OrgID.Name,1,2) from Project left join OrgID on Project.ORGID = OrgID.OrgIDGuid WHERE ProjectGuid = '{id}';";
            var CompanyCode = _context.Connection.Query<string>(CompanyCodeSql).FirstOrDefault();

            string CompanyGuidSql = $"select Company.President from Company where CompanyCode = '{CompanyCode}'";
            var CompanyGuid = _context.Connection.Query<Guid>(CompanyGuidSql).FirstOrDefault();

            string CompanyPresidentSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname  from Users where Users.UserGuid = '{CompanyGuid}'";
            var CompanyPresident = _context.Connection.Query<string>(CompanyPresidentSql).FirstOrDefault();

            string CompanyNameSql = $"select Company.CompanyName from Company where CompanyCode = '{CompanyCode}' and IsDeleted = 0";
            var CompanyName = _context.Connection.Query<String>(CompanyNameSql).FirstOrDefault();

            string RegionCodeSql = $@"select SUBSTRING(OrgID.Name,4,2) from Project left join OrgID on Project.ORGID = OrgID.OrgIDGuid WHERE ProjectGuid = '{id}';";
            var RegionCode = _context.Connection.Query<string>(RegionCodeSql).FirstOrDefault();

            string RegionGuidSql = $@"select Region.RegionalManager from Region where RegionCode = '{RegionCode}'";
            var RegionGuid = _context.Connection.Query<Guid>(RegionGuidSql).FirstOrDefault();

            string RegionalManagerSql = $@"select Users.Displayname +' (' + Users.JobTitle +')' Displayname from Users where Users.UserGuid = '{RegionGuid}'";
            var RegionalManager = _context.Connection.Query<string>(RegionalManagerSql).FirstOrDefault();

            string RegionNameSql = $"select Region.RegionName from Region where RegionCode = '{RegionCode}' and IsDeleted = 0";
            var RegionName = _context.Connection.Query<String>(RegionNameSql).FirstOrDefault();

            string OfficeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) from Project left join OrgID on Project.ORGID = OrgID.OrgIDGuid WHERE ProjectGuid = '{id}';";
            var OfficeCode = _context.Connection.Query<string>(OfficeCodeSql).FirstOrDefault();

            string OfficeNameSql = $"select Office.OfficeName from Office where OfficeCode = '{OfficeCode}' and IsDeleted = 0";
            var OfficeName = _context.Connection.Query<String>(OfficeNameSql).FirstOrDefault();

            string questionary = $" select questionaire.ContractQuestionaireGuid,users.Displayname,questionaire.UpdatedOn from ContractQuestionaire questionaire " +
                   $"left join Users users on users.UserGuid = questionaire.UpdatedBy where questionaire.ProjectGuid = '{id}' and Isdeleted = 0";

            var ContractQuestionaire = _context.Connection.Query<ContractQuestionaire>(questionary).FirstOrDefault();

            string billingrates = $"select * from EmployeeBillingRates where ProjectGuid = '{id}' and Isdeleted = 0";
            var employeeeBillingRates = _context.Connection.Query<EmployeeBillingRates>(billingrates).FirstOrDefault();

            string contractWBSSql = $"select * from ContractWBS where ProjectGuid = '{id}' and Isdeleted = 0";
            var contractWBS = _context.Connection.Query<ContractWBS>(contractWBSSql).FirstOrDefault();

            string categoryrates = $"select * from LaborCategoryRates where ProjectGuid = '{id}' and Isdeleted = 0";
            var laborCategoryRates = _context.Connection.Query<LaborCategoryRates>(categoryrates).FirstOrDefault();

            string revenue = $"select * from RevenueRecognization where ProjectGuid = '{id}' and Isdeleted = 0";
            var RevenueRecognitionModel = _context.Connection.Query<Northwind.Core.Entities.RevenueRecognition>(revenue).FirstOrDefault();

            string requestForm = $"select * from JobRequest where ProjectGuid = '{id}' and Isdeleted = 0";
            var requestFormModel = _context.Connection.Query<Core.Entities.JobRequest>(requestForm).FirstOrDefault();

            string sql = @"select distinct 
                            Project.ProjectGuid,
                            Project.ContractGuid,
                            Project.Description, 
                            Project.ProjectNumber, 
                            (OrgID.Name + ' ' + OrgID.Title) OrganizationName,
                            Project.ORGID,
							Project.ProjectNumber, 
                            Project.ProjectTitle, 
                            Country.CountryId CountryOfPerformance,
                            Country.CountryName CountryOfPerformanceSelected,
                            State.StateName PlaceOfPerformanceSelected,
                            Project.PlaceOfPerformance PlaceOfPerformanceSelectedIds,
                            Project.POPStart, 
                            Project.POPEnd, 
                            (NAICS.Code +' '+ NAICS.Title) NAICSCodeName, 
                            Project.NAICSCode,
                            PSC.CodeDescription PSCCodeName,
                            Project.PSCCode,
                            Project.QualityLevelRequirements, 
                            QualityLevel.Name QualityLevelName,
                            Project.QualityLevel,


                            ----------Key personal  info.........
                            (ProjectManager.Displayname +' (' +ProjectManager.JobTitle +')') ProjectManagerName, 
							Project.ProjectManager,
                            Project.CompanyPresident,
                            Project.RegionalManager,
							Project.ProjectManager,
							(ProjectControl.Displayname  +' (' +ProjectControl.JobTitle +')')ProjectControlName,
							Project.ProjectControls,
							(AccountingRepresentative.Displayname  +' (' +AccountingRepresentative.JobTitle +')') AccountingRepresentativeName,
							Project.AccountingRepresentative,
                            Project.ProjectRepresentative,
							(ProjectRepresentative.Displayname +' (' +ProjectRepresentative.JobTitle +')') ProjectRepresentativeName,
							
                            ----------Customer info..............
                            AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							Office_ProjectRepresentative.ContactGuid Office_ProjectRepresentative,
							(Office_ProjectRepresentative.FirstName + ' ' + Office_ProjectRepresentative.MiddleName + ' ' + Office_ProjectRepresentative.LastName) Office_ProjectRepresentativeName,
							Office_ProjectTechnicalRepresent.ContactGuid Office_ProjectTechnicalRepresent,
							(Office_ProjectTechnicalRepresent.FirstName + ' ' + Office_ProjectTechnicalRepresent.MiddleName + ' ' + Office_ProjectTechnicalRepresent.LastName) Office_ProjectTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
                            FundingOffice_ProjectRepresentative.ContactGuid FundingOffice_ProjectRepresentative,
                            (FundingOffice_ProjectRepresentative.FirstName + ' ' + FundingOffice_ProjectRepresentative.MiddleName + ' ' + FundingOffice_ProjectRepresentative.LastName) FundingOffice_ProjectRepresentativeName,
							FundingOffice_ProjectTechnicalRepresent.ContactGuid FundingOffice_ProjectTechnicalRepresent,
							(FundingOffice_ProjectTechnicalRepresent.FirstName + ' ' + FundingOffice_ProjectTechnicalRepresent.MiddleName + ' ' + FundingOffice_ProjectTechnicalRepresent.LastName) FundingOffice_ProjectTechnicalRepresentName,
							
                             --------------Financial info.............
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Project.SelfPerformance_Percent,
							Project.SBA,
							Competition.Name CompetitionType,
                            Project.Competition,
							ProjectType.Name ProjectTypeName,
                            Project.ProjectType,
							Project.OverHead,
							Project.G_A_Percent,
							Project.Fee_Percent,
							Currency.Name CurrencyName,
							Project.Currency,
							Project.BlueSkyAward_Amount,
							Project.AwardAmount,
							Project.FundingAmount,
							Project.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Project.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Project.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Project.PaymentTerms,
							AppWageDetermine_DavisBaconAct.Name AppWageDetermine_DavisBaconActType,
							Project.AppWageDetermine_DavisBaconAct,
							AppWageDetermine_ServiceProjectAct.Name AppWageDetermine_ServiceProjectActType,
							Project.AppWageDetermine_ServiceProjectAct,
							Project.BillingFormula,
							Project.RevenueFormula,
							Project.RevenueRecognitionEAC_Percent,
							Project.OHonsite,
							Project.OHoffsite,
							Project.IsActive,
                            Project.ProjectCounter

                        from Project

						left join
						OrgID on Project.ORGID = OrgID.OrgIDGuid
                        left join
						Country on Project.CountryOfPerformance = Country.CountryId
						left join
						State on Project.PlaceOfPerformance = State.StateId
						left join
						NAICS on Project.NAICSCode = NAICS.NAICSGuid
						left join
						PSC on Project.PSCCode = PSC.PSCGuid
						left join
						ResourceAttributeValue QualityLevel on Project.QualityLevel = QualityLevel.Value
                        

                        left join
						Users ProjectManager on Project.ProjectManager = ProjectManager.UserGuid
                        left join
						Users ProjectControl on Project.ProjectControls = ProjectControl.UserGuid
						left join
						Users AccountingRepresentative on Project.AccountingRepresentative = AccountingRepresentative.UserGuid
						left join	
						Users ProjectRepresentative on Project.ProjectRepresentative = ProjectRepresentative.UserGuid
						

                        left join
						Customer AwardingOffice on Project.AwardingAgencyOffice = AwardingOffice.CustomerGuid
						left join
                        CustomerContact Office_ProjectRepresentative on Project.Office_ProjectRepresentative = Office_ProjectRepresentative.ContactGuid
						left join
						CustomerContact Office_ProjectTechnicalRepresent on  Project.Office_ProjectTechnicalRepresent = Office_ProjectTechnicalRepresent.ContactGuid

                        left join
						Customer FundingOffice on Project.FundingAgencyOffice = FundingOffice.CustomerGuid
						left join
						CustomerContact FundingOffice_ProjectRepresentative on Project.FundingOffice_ProjectRepresentative = FundingOffice_ProjectRepresentative.ContactGuid
						left join
						CustomerContact FundingOffice_ProjectTechnicalRepresent on Project.FundingOffice_ProjectTechnicalRepresent = FundingOffice_ProjectTechnicalRepresent.ContactGuid

                       
						left join
						ResourceAttributeValue SetAside on Project.SetAside = SetAside.Value
						left join
						ResourceAttributeValue Competition on Project.Competition = Competition.Value
						left join
						ResourceAttributeValue ProjectType on Project.ProjectType = ProjectType.Value
						left join
						ResourceAttributeValue Currency on Project.Currency = Currency.Value
						left join
						ResourceAttributeValue BillingFrequency on Project.BillingFrequency = BillingFrequency.Value
						left join
						ResourceAttributeValue InvoiceSubmissionMethod on Project.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
						left join
						ResourceAttributeValue PaymentTerms on Project.PaymentTerms = PaymentTerms.Value
						left join
						ResourceAttributeValue AppWageDetermine_DavisBaconAct on Project.AppWageDetermine_DavisBaconAct = AppWageDetermine_DavisBaconAct.Value
						left join
						ResourceAttributeValue AppWageDetermine_ServiceProjectAct on Project.AppWageDetermine_ServiceProjectAct = AppWageDetermine_ServiceProjectAct.Value
 
                        WHERE ProjectGuid =  @ProjectGuid;";
            var BasicProjectInfo = _context.Connection.QuerySingle<BasicProjectInfoViewModel>(sql, new { ProjectGuid = id });
            var KeyPersonnel = _context.Connection.QuerySingle<KeyPersonnelViewModel_Project>(sql, new { ProjectGuid = id });
            var CustomerInformation = _context.Connection.QuerySingle<CustomerInformationViewModel_Project>(sql, new { ProjectGuid = id });
            var FinancialInformation = _context.Connection.QuerySingle<FinancialInformationViewModel_Project>(sql, new { ProjectGuid = id });
            var BaseModel = _context.Connection.QuerySingle<BaseModel>(sql, new { ProjectGuid = id });

            // to fetch States name through state id array..
            var stateIdArr = BasicProjectInfo.PlaceOfPerformanceSelectedIds.Split(',');
            var stateIdArrWithStringCote = stateIdArr.Select(x => string.Format("'" + x + "'"));
            var formatQuery = string.Join(",", stateIdArrWithStringCote);
            var stateQuery = $"select StateName from State where StateId in ({formatQuery})";
            var stateNameArr = _context.Connection.Query<string>(stateQuery); ;
            BasicProjectInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);

            var contractGuidQuery = $"select ContractGuid from Project  WHERE ProjectGuid =  '{id}';";
            ProjectViewModel.ContractGuid = _context.Connection.Query<Guid>(contractGuidQuery).First();

            var projectCounterQuery = $"select ProjectCounter from Project  WHERE ProjectGuid =  '{id}';";
            ProjectViewModel.ProjectCounter = _context.Connection.Query<int>(projectCounterQuery).First();

            var contractdetails = $"select ContractNumber from Contract  WHERE ContractGuid =  '{ProjectViewModel.ContractGuid}';";
            ProjectViewModel.ContractNumber = _context.Connection.Query<string>(contractdetails).First();

            ProjectViewModel.BasicProjectInfo = BasicProjectInfo;
            ProjectViewModel.BasicProjectInfo.CompanyName = CompanyName;
            ProjectViewModel.BasicProjectInfo.RegionName = RegionName;
            ProjectViewModel.BasicProjectInfo.OfficeName = OfficeName;
            ProjectViewModel.KeyPersonnel = KeyPersonnel;
            ProjectViewModel.KeyPersonnel.CompanyPresidentName = CompanyPresident;
            ProjectViewModel.KeyPersonnel.RegionalManagerName = RegionalManager;
            ProjectViewModel.CustomerInformation = CustomerInformation;
            ProjectViewModel.ProjectGuid = id;
            ProjectViewModel.FinancialInformation = FinancialInformation;
            ProjectViewModel.ContractQuestionaire = ContractQuestionaire;
            ProjectViewModel.employeeBillingRatesViewModel = employeeeBillingRates;
            ProjectViewModel.LaborCategoryRates = laborCategoryRates;
            ProjectViewModel.revenueRecognitionModel = RevenueRecognitionModel;
            ProjectViewModel.ContractWBS = contractWBS;
            ProjectViewModel.JobRequest = requestFormModel;
            ProjectViewModel.IsActive = BaseModel.IsActive;
            return ProjectViewModel;
        }

        public int GetTotalCountProjectByContractId(Guid contractGuid)
        {
            var getCount = string.Format($@"
               select count(1) from contract join project  on contract.contractGuid = project.contractGuid 
               where project.IsDeleted = 0 and contract.contractGuid ='{contractGuid}'");
            var totalProjectForContract = _context.Connection.QuerySingle<int>(getCount);
            return totalProjectForContract;
        }
        public Guid? GetPreviousProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
               select top 1 project.projectGuid 
                    from contract join project  
                       on contract.contractGuid = project.contractGuid 
                         where project.IsDeleted = 0 
                           and contract.contractGuid ='{contractGuid}' 
                           and projectCounter < {currentProjectCounter}
                           order by projectCounter desc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery);
            return getPreviousProjectGuid;
        }
        public Guid? GetNextProjectOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
               select top 1 project.projectGuid 
                    from contract join project  
                       on contract.contractGuid = project.contractGuid 
                         where project.IsDeleted = 0 
                           and contract.contractGuid ='{contractGuid}' 
                           and projectCounter > {currentProjectCounter}
                           order by projectCounter asc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery);
            return getPreviousProjectGuid;
        }
        public Guid? GetContractIdByProjectId(Guid projectGuid)
        {
            var contractGuidQuery = string.Format($@"
               select contractGuid from project 
                         where project.IsDeleted = 0 
                           and project.projectGuid ='{projectGuid}'");
            var contractGuid = _context.Connection.QueryFirstOrDefault<Guid>(contractGuidQuery);
            return contractGuid;
        }

        public int HasChild(Guid projectGuid)
        {
            var getCount = string.Format($@"
               select count(1) from projectModification where isDeleted = 0 and projectGuid ='{projectGuid}'");
            var totalProjectModificaton = _context.Connection.QuerySingle<int>(getCount);
            return totalProjectModificaton;
        }
        public bool IsExistProjectNumber(string projectNumber)
        {
            string projectNumberQuery = $@"select projectNumber from  Project 
                                              where  IsDeleted   = 0
                                                 and projectNumber = @projectNumber ";
            var result = _context.Connection.QueryFirstOrDefault<string>(projectNumberQuery, new { projectNumber = projectNumber });

            return !string.IsNullOrEmpty(result) ? true : false;
        }

        public User GetUsersDataByUserId(Guid id)
        {
            var usersQuery = string.Format($@"
               select * from Users
                where userGuid = '{id}' 
                order by Displayname asc");
            var usersData = _context.Connection.QuerySingle<User>(usersQuery);
            return usersData;
        }

        public CustomerContact GetContactsDataByContactId(Guid id)
        {
            var contactQuery = string.Format($@"
               select * from CustomerContact
                where ContactGuid = '{id}' 
                order by Firstname asc");
            var contactData = _context.Connection.QuerySingle<CustomerContact>(contactQuery);
            return contactData;
        }

        public Customer GetCustomersDataByCustomerId(Guid id)
        {
            var customerQuery = string.Format($@"
               select * from Customer
                where CustomerGuid = '{id}' 
                order by CustomerName asc");
            var customerData = _context.Connection.QuerySingle<Customer>(customerQuery);
            return customerData;
        }

        //ContractQuestionaires
        public int AddContractQuestionaires(ContractQuestionaire model)
        {
            string insertQuery = $@"INSERT INTO [dbo].[ContractQuestionaire]
                                                                   (
                                                                        ContractQuestionaireGuid	   ,
                                                                        ContractGuid				   ,
                                                                        ProjectGuid				       ,
                                                                        IsFARclause					   ,
                                                                        IsReportExecCompensation	   ,
                                                                        Report_LastReportDate		   ,
                                                                        Report_NextReportDate		   ,
                                                                        IsGSAschedulesale			   ,
                                                                        GSA_LastReportDate			   ,
                                                                        GSA_NextReportDate			   ,
                                                                        IsSBsubcontract			       ,
                                                                        SB_LastReportDate			   ,
                                                                        SB_NextReportDate			   ,
                                                                        IsGovtFurnishedProperty		   ,
                                                                        IsServiceContractReport		   ,
                                                                        IsGQAC					       ,
                                                                        GQAC_LastReportDate		       ,
                                                                        GQAC_NextReportDate			   ,
                                                                        IsCPARS					       ,
                                                                        CPARS_LastReportDate		   ,
                                                                        CPARS_NextReportDate		   ,
                                                                        IsWarranties				   ,
                                                                        IsStandardIndustryProvision	   ,
                                                                        WarrantyProvisionDescription   ,
                                                                        CreatedOn				       ,
                                                                        UpdatedOn		               ,
                                                                        CreatedBy                      ,
                                                                        UpdatedBy				       ,
                                                                        IsDeleted				       ,
                                                                        IsActive				       
                                                                    )
                                  VALUES (
                                                                        '{model.ContractQuestionaireGuid                                    }'          ,
                                                                        '{model.ContractGuid                                                }'	        ,
                                                                        '{model.ProjectGuid                                                 }'	        ,
                                                                        '{model.IsFARclause                                                 }'          ,
                                                                        '{model.IsReportExecCompensation                                    }'          ,
                                                                         NULLIF('{model.Report_LastReportDate                               }' , '')    ,
                                                                         NULLIF('{model.Report_NextReportDate                               }' , '')    ,
                                                                        '{model.IsGSAschedulesale                                           }'          ,
                                                                         NULLIF('{model.GSA_LastReportDate                                  }' , '')    ,
                                                                         NULLIF('{model.GSA_NextReportDate                                  }' , '')    ,
                                                                        '{model.IsSBsubcontract                                             }'          ,
                                                                         NULLIF('{model.SB_LastReportDate                                   }' , '')    ,
                                                                         NULLIF('{model.SB_NextReportDate                                   }' , '')    ,
                                                                        '{model.IsGovtFurnishedProperty                                     }'          ,
                                                                        '{model.IsServiceContractReport                                     }'          ,
                                                                        '{model.IsGQAC                                                      }'          ,
                                                                         NULLIF('{model.GQAC_LastReportDate                                 }' , '')    ,
                                                                         NULLIF('{model.GQAC_NextReportDate                                 }' , '')    ,
                                                                        '{model.IsCPARS                                                     }'          ,
                                                                         NULLIF('{model.CPARS_LastReportDate                                }' , '')    ,
                                                                         NULLIF('{model.CPARS_NextReportDate                                }' , '')   ,
                                                                        '{model.IsWarranties                                                }'          ,
                                                                        '{model.IsStandardIndustryProvision                                 }'          ,
                                                                        '{model.WarrantyProvisionDescription                                }'          ,
                                                                        '{model.CreatedOn                                                   }'          ,
                                                                        '{model.UpdatedOn                                                   }'          ,
                                                                        '{model.CreatedBy                                                   }'          ,
                                                                        '{model.UpdatedBy                                                   }'          ,
                                                                        '{model.IsDeleted                                                   }'          ,
                                                                        '{model.IsActive                                                    }'   
                                                                )";
            return _context.Connection.Execute(insertQuery);
        }

        public ContractQuestionaire GetContractQuestionaireById(Guid id)
        {
            string sql = $"SELECT * FROM ContractQuestionaire WHERE ContractQuestionaireGuid = @ContractQuestionaireGuid;";
            var result = _context.Connection.QuerySingle<ContractQuestionaire>(sql, new { ContractQuestionaireGuid = id });
            return result;
        }

        public int UpdateContractQuestionairesById(ContractQuestionaire ContractQuestionaire)
        {
            string updateQuery = $@"Update ContractQuestionaire       set 
                                                                    IsFARclause					 =          '{ContractQuestionaire.IsFARclause                           }'      ,
                                                                    IsReportExecCompensation	 =          '{ContractQuestionaire.IsReportExecCompensation              }'      ,
                                                                    Report_LastReportDate		 =           NULLIF('{ContractQuestionaire.Report_LastReportDate         }' , '')    ,
                                                                    Report_NextReportDate		 =           NULLIF('{ContractQuestionaire.Report_NextReportDate                               }' , '')    ,
                                                                    IsGSAschedulesale			 =          '{ContractQuestionaire.IsGSAschedulesale                     }'      ,
                                                                    GSA_LastReportDate			 =             NULLIF('{ContractQuestionaire.GSA_LastReportDate                                  }' , '')    ,
                                                                    GSA_NextReportDate			 =           NULLIF('{ContractQuestionaire.GSA_NextReportDate                                  }' , '')    ,
                                                                    IsSBsubcontract			     =          '{ContractQuestionaire.IsSBsubcontract                       }'      ,
                                                                    SB_LastReportDate			 =          NULLIF('{ContractQuestionaire.SB_LastReportDate                                   }' , '')    ,
                                                                    SB_NextReportDate			 =          NULLIF('{ContractQuestionaire.SB_NextReportDate                                   }' , '')    ,
                                                                    IsGovtFurnishedProperty		 =          '{ContractQuestionaire.IsGovtFurnishedProperty               }'      ,
                                                                    IsServiceContractReport		 =          '{ContractQuestionaire.IsServiceContractReport               }'      ,
                                                                    IsGQAC					     =          '{ContractQuestionaire.IsGQAC                                }'      ,
                                                                    GQAC_LastReportDate		     =           NULLIF('{ContractQuestionaire.GQAC_LastReportDate                                 }' , '')    ,
                                                                    GQAC_NextReportDate			 =          NULLIF('{ContractQuestionaire.GQAC_NextReportDate                                 }' , '')    ,
                                                                    IsCPARS					     =          '{ContractQuestionaire.IsCPARS                               }'      ,
                                                                    CPARS_LastReportDate		 =           NULLIF('{ContractQuestionaire.CPARS_LastReportDate                                }' , '')    ,                                                                   
                                                                    CPARS_NextReportDate		 =           NULLIF('{ContractQuestionaire.CPARS_NextReportDate                                }' , '')   ,
                                                                    IsWarranties				 =          '{ContractQuestionaire.IsWarranties                      }'      ,
                                                                    IsStandardIndustryProvision	 =          '{ContractQuestionaire.IsStandardIndustryProvision           }'      ,
                                                                    WarrantyProvisionDescription =          '{ContractQuestionaire.WarrantyProvisionDescription         }'      ,
                                                                    UpdatedOn		             =          '{ContractQuestionaire.UpdatedOn                             }'      ,
                                                                    UpdatedBy				     =          '{ContractQuestionaire.UpdatedBy                             }'      
                                                                      where ContractQuestionaireGuid = '{ContractQuestionaire.ContractQuestionaireGuid}' ";
            return _context.Connection.Execute(updateQuery);
        }

        //ContractWBS
        public int AddContractWBS(ContractWBS model)
        {
            string insertQuery = $@"INSERT INTO ContractWBS (
                                                    ContractWBSGuid,
                                                    ContractGuid,
                                                    ProjectGuid,
                                                    UploadFileName,
                                                    IsCsv,
                                                    CreatedOn,
                                                    UpdatedOn,
                                                    CreatedBy,
                                                    UpdatedBy,
                                                    IsDeleted,
                                                    IsActive
                                                            )
                                    VALUES (
                                                    '{model.ContractWBSGuid}',
                                                    '{model.ContractGuid}',
                                                    '{model.ProjectGuid}',
                                                    '{model.UploadFileName}',
                                                    '{model.IsCsv}',
                                                    '{model.CreatedOn}',
                                                    '{model.UpdatedOn}',
                                                    '{model.CreatedBy}',
                                                    '{model.UpdatedBy}',
                                                    '{model.IsDeleted}',
                                                    '{model.IsActive}'
                                                            )";
            return _context.Connection.Execute(insertQuery);
        }

        public int UpdateContractWBS(ContractWBS contractWBS)
        {
            string insertQuery = $@"UPDATE ContractWBS set 
                                                    UpdatedOn              =           '{contractWBS.UpdatedOn}',
                                                    UpdatedBy              =           '{contractWBS.UpdatedBy}',
                                                    IsDeleted              =           '{contractWBS.IsDeleted}',
                                                    IsActive               =           '{contractWBS.IsActive}'
                                                     where ContractWBSGuid = '{contractWBS.ContractWBSGuid}' ";
            return _context.Connection.Execute(insertQuery);
        }

        public int DeleteContractWBS(Guid id)
        {
            string deleteQuery = $@"UPDATE ContractWBS set IsDeleted = 1 WHERE ProjectGuid = '{id}' ";
            return _context.Connection.Execute(deleteQuery);
        }

        public ContractWBS GetContractWBSById(Guid id)
        {
            string sql = $"SELECT * FROM ContractWBS WHERE ProjectGuid = @ProjectGuid AND IsDeleted = 0;";
            var result = _context.Connection.QueryFirstOrDefault<ContractWBS>(sql, new { ProjectGuid = id });
            return result;
        }

        public IEnumerable<ContractWBS> GetWBSList(string path)
        {
            List<ContractWBS> data = new List<ContractWBS>();
            var lines = System.IO.File.ReadAllLines(path);
            int sn = 0;
            foreach (string item in lines.Skip(1))
            {
                var values = item.Split(',');
                sn = sn + 1;
                var wbscode = values[0].Replace('"', ' ').Trim();
                var description = values[1].Replace('"', ' ').Trim();
                var value = values[2].Replace('"', ' ').Trim();
                var contracttype = values[3].Replace('"', ' ').Trim();
                var invoiceatthislevel = values[4].Replace('"', ' ').Trim();
                data.Add(new ContractWBS { WBSCode = wbscode, Description = description, Value = value, ContractType = contracttype, InvoiceAtThisLevel = invoiceatthislevel, sn = sn });
            }
            return data;
        }

        //EmployeeBillingRates
        public int AddEmployeeBillingRates(EmployeeBillingRates model)
        {
            string insertQuery = $@"INSERT INTO [dbo].[EmployeeBillingRates]
                                                                   (
                                                                        BillingRateGuid	                ,
                                                                        ContractGuid				    ,
                                                                        ProjectGuid			    	    ,
                                                                        UploadFileName					,
                                                                        IsCsv                           ,
                                                                        CreatedOn				        ,
                                                                        UpdatedOn		                ,
                                                                        CreatedBy                       ,
                                                                        UpdatedBy				        ,
                                                                        IsDeleted				        ,
                                                                        IsActive				       
                                                                    )
                                  VALUES (
                                                                        '{model.BillingRateGuid     }',
                                                                        '{model.ContractGuid        }',
                                                                        '{model.ProjectGuid         }',
                                                                        '{model.UploadFileName      }',
                                                                        '{model.IsCsv               }',
                                                                        '{model.CreatedOn           }',
                                                                        '{model.UpdatedOn           }',
                                                                        '{model.CreatedBy           }',
                                                                        '{model.UpdatedBy           }',
                                                                        '{model.IsDeleted           }',
                                                                        '{model.IsActive            }'   
                                                                )";
            return _context.Connection.Execute(insertQuery);
        }

        public int UpdateEmployeeBillingRates(EmployeeBillingRates employeeBillingRates)
        {
            string insertQuery = $@"UPDATE EmployeeBillingRates set 
                                                    UpdatedOn              =           '{employeeBillingRates.UpdatedOn}',
                                                    UpdatedBy              =           '{employeeBillingRates.UpdatedBy}',
                                                    IsDeleted              =           '{employeeBillingRates.IsDeleted}',
                                                    IsActive               =           '{employeeBillingRates.IsActive}'
                                                     where BillingRateGuid = '{employeeBillingRates.BillingRateGuid}' ";
            return _context.Connection.Execute(insertQuery);
        }

        public int DeleteEmployeeBillingRates(Guid id)
        {
            string deleteQuery = $@"UPDATE EmployeeBillingRates set IsDeleted = 1 WHERE ProjectGuid = '{id}' ";
            return _context.Connection.Execute(deleteQuery);
        }

        public EmployeeBillingRates GetEmployeeBillingRatesById(Guid id)
        {
            string sql = $"SELECT * FROM EmployeeBillingRates WHERE ProjectGuid = @ProjectGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<EmployeeBillingRates>(sql, new { ProjectGuid = id });
            return result;
        }

        public IEnumerable<EmployeeBillingRates> GetBillingRates(string path)
        {
            List<EmployeeBillingRates> data = new List<EmployeeBillingRates>();
            var lines = System.IO.File.ReadAllLines(path);
            int sn = 0;
            foreach (string item in lines.Skip(1))
            {
                var values = item.Split(',');
                sn = sn + 1;
                var laborcode = values[0].Replace('"', ' ').Trim();
                var employeename = values[1].Replace('"', ' ').Trim();
                var rate = values[2].Replace('"', ' ').Trim();
                decimal ratedecimal = decimal.Parse(rate);
                var startdate = values[3].Replace('"', ' ').Trim();
                var enddate = values[4].Replace('"', ' ').Trim();
                data.Add(new EmployeeBillingRates { LaborCode = laborcode, EmployeeName = employeename, Rate = ratedecimal, StartDate = startdate, EndDate = enddate, sn = sn });
            }
            return data;
        }

        //LaborCategoryRates
        public int AddLaborCategoryRates(LaborCategoryRates model)
        {
            string insertQuery = $@"INSERT INTO [dbo].[LaborCategoryRates]
                                                                   (
                                                                        CategoryRateGuid	            ,
                                                                        ContractGuid				    ,
                                                                        ProjectGuid		    		    ,
                                                                        UploadFileName					,
                                                                        IsCsv				            ,
                                                                        CreatedOn				        ,
                                                                        UpdatedOn		                ,
                                                                        CreatedBy                       ,
                                                                        UpdatedBy				        ,
                                                                        IsDeleted				        ,
                                                                        IsActive				       
                                                                    )
                                  VALUES (
                                                                        '{model.CategoryRateGuid    }',
                                                                        '{model.ContractGuid        }',
                                                                        '{model.ProjectGuid         }',
                                                                        '{model.UploadFileName      }',
                                                                        '{model.IsCsv               }',
                                                                        '{model.CreatedOn           }',
                                                                        '{model.UpdatedOn           }',
                                                                        '{model.CreatedBy           }',
                                                                        '{model.UpdatedBy           }',
                                                                        '{model.IsDeleted           }',
                                                                        '{model.IsActive            }'   
                                                                )";
            return _context.Connection.Execute(insertQuery);
        }

        public int UpdateLaborCategoryRates(LaborCategoryRates laborCategoryRates)
        {
            string insertQuery = $@"UPDATE LaborCategoryRates set 
                                                    UpdatedOn              =           '{laborCategoryRates.UpdatedOn}',
                                                    UpdatedBy              =           '{laborCategoryRates.UpdatedBy}',
                                                    IsDeleted              =           '{laborCategoryRates.IsDeleted}',
                                                    IsActive               =           '{laborCategoryRates.IsActive}'
                                                     where CategoryRateGuid = '{laborCategoryRates.CategoryRateGuid}' ";
            return _context.Connection.Execute(insertQuery);
        }

        public int DeleteLaborCategoryRates(Guid id)
        {
            string deleteQuery = $@"UPDATE LaborCategoryRates set IsDeleted = 1 WHERE ProjectGuid = '{id}' ";
            return _context.Connection.Execute(deleteQuery);
        }

        public LaborCategoryRates GetLaborCategoryRatesById(Guid id)
        {
            string sql = $"SELECT * FROM LaborCategoryRates WHERE ProjectGuid = @ProjectGuid AND IsDeleted = 0;";
            var result = _context.Connection.QuerySingle<LaborCategoryRates>(sql, new { ProjectGuid = id });
            return result;
        }

        public IEnumerable<LaborCategoryRates> GetCategoryRates(string path)
        {
            List<LaborCategoryRates> data = new List<LaborCategoryRates>();
            var lines = System.IO.File.ReadAllLines(path);
            int sn = 0;
            foreach (string item in lines.Skip(1))
            {
                var values = item.Split(',');
                sn = sn + 1;
                var subcontractor = values[0].Replace('"', ' ').Trim();
                var laborcode = values[1].Replace('"', ' ').Trim();
                var employeename = values[2].Replace('"', ' ').Trim();
                var rate = values[3].Replace('"', ' ').Trim();
                decimal ratedecimal = decimal.Parse(rate);
                var startdate = values[4].Replace('"', ' ').Trim();
                var enddate = values[5].Replace('"', ' ').Trim();
                data.Add(new LaborCategoryRates { SubContractor = subcontractor, LaborCode = laborcode, EmployeeName = employeename, Rate = ratedecimal, StartDate = startdate, EndDate = enddate, sn = sn });
            }
            return data;
        }

        public string GetProjectNumberById(Guid id)
        {
            string Contractnumbersql = $"select ProjectNumber from Project WHERE ProjectGuid = '{id}';";
            var Contractnumber = _context.Connection.Query<string>(Contractnumbersql).FirstOrDefault();
            return Contractnumber;
        }

        public IEnumerable<ProjectForList> GetProjectByContractGuid(Guid contractGuid)
        {
            var sql = @"
                SELECT P.[ProjectGuid]
                      ,P.[ContractGuid]
                      ,P.[ProjectNumber]
                      ,P.[ORGID]
                      ,OrgId.title OrgName
                      ,P.[ProjectTitle]
                      ,P.[CompanyPresident]
                      ,(CompanyPresident.Firstname + ' ' + CompanyPresident.Lastname)  CompanyPresidentName
                      ,P.[RegionalManager]
                      ,(RegionalManager.Firstname + ' ' + RegionalManager.Lastname) RegionalManagerName
                      ,P.[ProjectManager]
                      ,(ProjectManager.Firstname + ' ' + ProjectManager.Lastname) ProjectManagerName
                      ,P.[ProjectControls]
                      ,(ProjectControl.Firstname + ' ' + ProjectControl.Lastname) ProjectControlsName
                      ,P.[AccountingRepresentative]
                      ,(AccountingRepresentative.Firstname + ' ' + AccountingRepresentative.Lastname) AccountingRepresentativeName
                      ,P.[ProjectRepresentative]
                      ,(ContractRepresentative.Firstname + ' ' + ContractRepresentative.Lastname) ContractRepresentativeName
                      ,P.[CountryOfPerformance]
                      ,Country.CountryName CountryOfPerformanceName
                      ,P.[PlaceOfPerformance]
                      ,P.[POPStart]
                      ,P.[POPEnd]
                      ,P.[NAICSCode]
                      ,NAICS.title NAICSCodeName
                      ,P.[PSCCode]
                      ,PSC.CodeDescription PSCCodeName
                      ,P.[QualityLevelRequirements]
                      ,P.[QualityLevel]
                      ,P.[AwardingAgencyOffice]
                      ,P.[Office_ProjectRepresentative]
                      ,P.[Office_ProjectTechnicalRepresent]
                      ,P.[FundingAgencyOffice]
                      ,P.[SetAside]
                      ,P.[SelfPerformance_Percent]
                      ,P.[SBA]
                      ,P.[Competition]
                      ,P.[ProjectType]
                      ,P.[OverHead]
                      ,P.[G_A_Percent]
                      ,P.[Fee_Percent]
                      ,P.[Currency]
                      ,P.[BlueSkyAward_Amount]
                      ,P.[AwardAmount]
                      ,P.[FundingAmount]
                      ,P.[BillingAddress]
                      ,P.[BillingFrequency]
                      ,P.[InvoiceSubmissionMethod]
                      ,InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName
                      ,P.[PaymentTerms]
                      ,PaymentTerms.Name PaymentTermsName
                      ,P.[AppWageDetermine_DavisBaconAct]
                      ,P.[BillingFormula]
                      ,P.[RevenueFormula]
                      ,P.[RevenueRecognitionEAC_Percent]
                      ,P.[OHonsite]
                      ,P.[OHoffsite]
                      ,P.[CreatedOn]
                      ,P.[UpdatedOn]
                      ,P.[CreatedBy]
                      ,P.[UpdatedBy]
                      ,P.[IsActive]
                      ,P.[IsDeleted]
                      ,P.[Description]
                      ,P.[AppWageDetermine_ServiceProjectAct]
                      ,P.[FundingOffice_ProjectRepresentative]
                      ,'' FundingOffice_ProjectRepresentativeName
                      ,P.[FundingOffice_ProjectTechnicalRepresent]
                      ,'' FundingOffice_ProjectTechnicalRepresentName
                      ,P.[ProjectCounter]
                  FROM [dbo].[Project] P
                
                inner join Contract on Contract.ContractGuid = P.ContractGuid
                left join ResourceAttributeValue PaymentTerms on P.PaymentTerms = PaymentTerms.Value
				left join ResourceAttributeValue InvoiceSubmissionMethod on P.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
                left join NAICS NAICS on P.NAICSCode = NAICS.NAICSGuid
                left join OrgID OrgID on P.ORGID = ORGID.OrgIDGuid
                left join Users CompanyPresident on P.CompanyPresident = CompanyPresident.UserGuid
                left join Users RegionalManager	on P.RegionalManager = RegionalManager.UserGuid
                left join Users ProjectManager on P.ProjectManager = ProjectManager.UserGuid
                left join Users ProjectControl on P.ProjectControls = ProjectControl.UserGuid
                left join Users AccountingRepresentative on P.AccountingRepresentative = AccountingRepresentative.UserGuid                
                left join Users ContractRepresentative on Contract.ContractRepresentative = ContractRepresentative.UserGuid
                left join Customer AwardingAgency on P.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                left join Customer FundingAgency on P.FundingAgencyOffice = FundingAgency.CustomerGuid
				left join ResourceAttributeValue Currency on P.Currency = Currency.Value
                left join Country Country on P.CountryOfPerformance = Country.CountryId
				left join State State on P.PlaceOfPerformance = State.StateId
                left join PSC on Contract.PSCCode = PSC.PSCGuid

                  WHERE P.[IsDeleted] = 0 
            ";

            sql += $" AND P.ContractGuid = @ContractGuid";
          
            return _context.Connection.Query<ProjectForList>(sql, new { ContractGuid = contractGuid });
        }

        public ContractQuestionaire GetQuestionariesFromContract(Guid id)
        {
            string sql = $"SELECT * FROM ContractQuestionaire WHERE ContractGuid = @ContractGuid;";
            var result = _context.Connection.QueryFirstOrDefault<ContractQuestionaire>(sql, new { ContractGuid = id });
            if (result == null)
            {
                ContractQuestionaire contractQuestionaire = new ContractQuestionaire();
                return contractQuestionaire;
            }
            return result;
        }

    }
}