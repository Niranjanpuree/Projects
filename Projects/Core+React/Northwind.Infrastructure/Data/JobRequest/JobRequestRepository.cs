using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Northwind.Core.Interfaces;
using Northwind.Core.Entities;
using System.Linq;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces.ContractRefactor;

namespace Northwind.Infrastructure.Data.JobRequest
{
    public class JobRequestRepository : IJobRequestRepository
    {
        IDatabaseContext _context;

        IContractsRepository _contractRepo;

        public JobRequestRepository(IDatabaseContext context, IContractsRepository contractRepo)
        {
            _context = context;
            _contractRepo = contractRepo;
        }

        public Guid Add(Core.Entities.JobRequest jobRequest)
        {
            _context.Connection.Execute(@"INSERT INTO dbo.[Jobrequest]
           ([JobRequestGuid]
           ,[ContractGuid]
           ,[Status]
           ,[IsIntercompanyWorkOrder]
           ,[Companies]
           ,[Notes]
           ,[IsActive]
           ,[IsDeleted]
           ,[CreatedOn]
           ,[UpdatedOn]
           ,[CreatedBy]
           ,[UpdatedBy])
            VALUES
           (@JobRequestGuid
           ,@ContractGuid
           ,@Status
           ,@IsIntercompanyWorkOrder
           ,@Companies
           ,@Notes
           ,@IsActive
           ,@IsDeleted
           ,@CreatedOn
           ,@UpdatedOn 
           ,@CreatedBy
           ,@UpdatedBy)",
           new
           {
               jobRequest.JobRequestGuid,
               jobRequest.ContractGuid,
               jobRequest.Status,
               jobRequest.IsIntercompanyWorkOrder,
               jobRequest.Companies,
               jobRequest.Notes,
               jobRequest.IsActive,
               jobRequest.IsDeleted,
               jobRequest.CreatedOn,
               jobRequest.UpdatedOn,
               jobRequest.CreatedBy,
               jobRequest.UpdatedBy
           });
            return jobRequest.JobRequestGuid;
        }

        public void Edit(Core.Entities.JobRequest jobRequest)
        {
            //Status Is Change By A Step Ahead.
            switch (jobRequest.Status)
            {
                case (int)JobRequestStatus.ContractRepresentative:
                    jobRequest.Status = (int)JobRequestStatus.ProjectControl;
                    break;
                case (int)JobRequestStatus.ProjectControl:
                    jobRequest.Status = (int)JobRequestStatus.ProjectManager;
                    break;
                case (int)JobRequestStatus.ProjectManager:
                    jobRequest.Status = (int)JobRequestStatus.Accounting;
                    break;
                case (int)JobRequestStatus.Accounting:
                    jobRequest.Status = (int)JobRequestStatus.Complete;
                    break;
                default:
                    jobRequest.Status = (int)JobRequestStatus.ContractRepresentative;
                    break;
            }
            _context.Connection.Execute(@"UPDATE dbo.[Jobrequest] SET
            [Status] =  @Status                      
           ,[IsIntercompanyWorkOrder] = @IsIntercompanyWorkOrder
           ,[Companies] = @Companies
           ,[Notes] = @Notes
           ,[IsActive] = @IsActive
           ,[IsDeleted] = @IsDeleted
           ,[UpdatedOn] = @UpdatedOn
           ,[UpdatedBy] = @UpdatedBy
           where ContractGuid = @ContractGuid",
           new
           {
               jobRequest.Status,
               jobRequest.IsIntercompanyWorkOrder,
               jobRequest.Companies,
               jobRequest.Notes,
               jobRequest.IsActive,
               jobRequest.IsDeleted,
               jobRequest.UpdatedOn,
               jobRequest.UpdatedBy,
               jobRequest.ContractGuid
           });
        }

        public ICollection<KeyValuePairModel<Guid, string>> GetCompanyData()
        {
            var model = new List<KeyValuePairModel<Guid, string>>();
            var data = _context.Connection.Query<Company>("select * from Company order by CompanyName asc");
            foreach (var item in data)
            {
                model.Add(new KeyValuePairModel<Guid, string> { Keys = item.CompanyGuid, Values = item.CompanyName });
            }
            return model;
        }

        public IEnumerable<Core.Entities.JobRequest> GetAll(string searchValue, string filterBy, int pageSize, int skip, int take, string orderBy, string dir, Guid userGuid)
        {
            string sortOrder = String.Empty;
            if (!string.IsNullOrEmpty(dir))
            {
                switch (dir.ToLower())
                {
                    case "asc":
                        dir = "asc";
                        break;
                    case "desc":
                        dir = "desc";
                        break;
                    default:
                        dir = "asc";
                        break;
                }
            }
            var where = "";
            var searchString = "";
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "ProjectNumber";
            }
            if (take == 0)
            {
                take = 10;
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (ProjectNumber LIKE @searchValue OR C.ContractTitle like @searchValue OR C.ContractNumber like @searchValue)";
            }

            if (!string.IsNullOrWhiteSpace(filterBy) && filterBy.ToLower() == "pending")
            {
                var status = (int)EnumGlobal.JobRequestStatus.Complete;
                where = $" AND Status != '{status}'";
            }

            if (filterBy.ToLower() == EnumGlobal.JobRequestFilterBy.MyPending.ToString().ToLower() && userGuid != Guid.Empty)
                where += $" AND (keyPersonnel.[account-representative] = '{userGuid}' OR keyPersonnel.[contract-representative] = '{userGuid}' or keyPersonnel.[project-manager] = '{userGuid}' or keyPersonnel.[project-controls] = '{userGuid}')";

            var sqlQuery = @"
                            SELECT  JobRequest.JobRequestGuid,                                         
                                    JobRequest.ContractGuid,		                            		
                                    JobRequest.Status,				                            	    
                                    c.ProjectNumber,				                            
                                    JobRequest.IsIntercompanyWorkOrder,
                                    JobRequest.Companies,
                                    JobRequest.Notes,
                                    JobRequest.IsActive,
                                    JobRequest.IsDeleted,
                                    JobRequest.CreatedBy,
                                    JobRequest.UpdatedBy,
                                    JobRequest.CreatedOn,
                                    JobRequest.UpdatedOn,
                                    TaskOrderNumber = c.ContractNumber,
                                    case when c.ParentContractGuid is null then 'Contract'
            	                    else 'Task Order' end ContractOrTaskOrder,
                                    UserCreator.Displayname AS InitiatedBy,
									CASE WHEN c.ParentContractGuid IS NULL then c.ContractNumber
									ELSE (SELECT TOP(1)ContractNumber From Contract where ContractGuid = c.ParentContractGuid and IsIDIQContract = 1 and IsDeleted = 0) END ContractNumber,
                                    --c.ContractNumber,
                                    c.ParentContractGuid,
                                    c.ContractTitle,
									keyPersonnel.*,
                                    contractRep.UserGuid as ProjectContractGuid, contractRep.*,
									projectControls.UserGuid as ProjectControlGuid,projectControls.*,
                                    projectManager.UserGuid AS ProjectManagerGuid, projectManager.*,
									accountRep.UserGuid as AccountRepresentativeGuid,accountRep.*
                                    
                                           FROM (SELECT  tbl1.*
                                            FROM   (SELECT ( contractguid )               AS ContractGuid, 
                                                            Max([regional-manager])        AS [regional-manager], 
                                                            Max([project-manager])         AS [project-manager], 
                                                            Max([project-controls])        AS [project-controls], 
                                                            Max([account-representative])  AS [account-representative], 
                                                            Max([contract-representative]) AS [contract-representative], 
                                                            Max([company-president])       AS [company-president] 
                                                    FROM   (SELECT contractguid, 
                                                                    [regional-manager], 
                                                                    [project-manager], 
                                                                    [project-controls], 
                                                                    [account-representative], 
                                                                    [contract-representative], 
                                                                    [company-president] 
                                                            FROM   contractuserrole 
                                                                    PIVOT ( Max(userguid) 
                                                                            FOR userrole IN ([regional-manager], 
                                                                                            [project-manager], 
                                                                                            [project-controls], 
                                                                                            [account-representative], 
                                                                                            [contract-representative], 
                                                                                            [company-president]) ) tt) t 
                                                    GROUP  BY contractguid) tbl1 
                                                    GROUP  BY tbl1.contractguid, 
                                                                [regional-manager], 
                                                                [project-manager], 
                                                                [project-controls], 
                                                                [account-representative], 
                                                                [contract-representative], 
                                                                [company-president]) keyPersonnel
							inner join Contract c on c.ContractGuid = keyPersonnel.ContractGuid
							LEFT JOIN JobRequest
                            ON c.ContractGuid = JobRequest.ContractGuid
                            LEFT JOIN Users UserCreator
                            ON JobRequest.CreatedBy = UserCreator.UserGuid
                            INNER JOIN Users projectManager
							ON projectManager.UserGuid =  keyPersonnel.[project-manager]
							INNER JOIN Users projectControls
							ON projectControls.UserGuid = keyPersonnel.[project-controls]
							INNER JOIN Users accountRep
							ON accountRep.UserGuid = keyPersonnel.[account-representative]
							INNER JOIN Users contractRep
							ON contractRep.UserGuid = keyPersonnel.[contract-representative]
                            WHERE JobRequest.IsDeleted = 0
                            AND JobRequest.JobRequestGuid IN (SELECT JobRequestGuid 
                            FROM JobRequest
                            WHERE IsDeleted = 0";
            sqlQuery += $"{ where }";
            sqlQuery += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY)";

            var jobDictionary = new Dictionary<Guid, Core.Entities.JobRequest>();
            var jobList = _context.Connection.Query<Core.Entities.JobRequest, Core.Entities.User, Core.Entities.User, Core.Entities.User, Core.Entities.User, Core.Entities.JobRequest>(
                sqlQuery,
                (job, contractRepresentative, projectControl, projectManager, accountRepresentative) =>
                {
                    Core.Entities.JobRequest jobEntity = job;
                    if (!jobDictionary.TryGetValue(job.JobRequestGuid, out jobEntity))
                    {
                        jobEntity = job;
                        jobEntity.ContractNumber = job.ContractNumber;
                        jobEntity.KeyPersonnelList = new List<ContractKeyPersonnel>();
                        jobDictionary.Add(job.JobRequestGuid, jobEntity);
                    }
                    //if (keyPerson != null && !jobEntity.KeyPersonnelList.Any(x => x.UserRole == keyPerson.DisplayName))
                    //{
                    //    jobEntity.KeyPersonnelList.Add(keyPerson);
                    //}

                    jobEntity.ContractRepresentative = contractRepresentative;
                    jobEntity.ProjectControls = projectControl;
                    jobEntity.ProjectManager = projectManager;
                    jobEntity.AccountRepresentative = accountRepresentative;

                    //if (job.ParentContractGuid != Guid.Empty)
                    //{
                    //    jobEntity.TaskOrderNumber = job.ContractNumber;
                    //}
                    //if (job.ParentContractGuid == Guid.Empty)
                    //    jobEntity.ContractNumber = job.ContractNumber;
                    return jobEntity;
                },
                new { searchValue = searchString, orderBy = orderBy, dir = dir, skip = skip, take = take },
                splitOn: "ProjectContractGuid,ProjectControlGuid,ProjectManagerGuid,AccountRepresentativeGuid")
            .Distinct().ToList();
            return jobList;
        }

        public int TotalRecord(string searchValue, string filterBy, Guid userGuid)
        {
            var where = string.Empty;
            var searchString = string.Empty;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (ProjectNumber LIKE @searchValue OR C.ContractTitle like @searchValue OR C.ContractNumber like @searchValue)";
            }
            if (!string.IsNullOrWhiteSpace(filterBy) && filterBy.ToLower() == EnumGlobal.JobRequestFilterBy.Pending.ToString().ToLower() || filterBy.ToLower() == EnumGlobal.JobRequestFilterBy.MyPending.ToString().ToLower())
            {
                var statusInt = (int)EnumGlobal.JobRequestStatus.Complete;
                where = $" AND JobRequest.Status != '{statusInt}'";
            }

            if (filterBy.ToLower() == EnumGlobal.JobRequestFilterBy.MyPending.ToString().ToLower() && userGuid != Guid.Empty)
                where += $" AND (keyPersonnel.[account-representative] = '{userGuid}' OR keyPersonnel.[contract-representative] = '{userGuid}' or keyPersonnel.[project-manager] = '{userGuid}' or keyPersonnel.[project-controls] = '{userGuid}')";

            string sqlQuery = @"
                            SELECT  Count(1)
                                    
                                           FROM (SELECT  tbl1.*
                                            FROM   (SELECT ( contractguid )               AS ContractGuid, 
                                                            Max([regional-manager])        AS [regional-manager], 
                                                            Max([project-manager])         AS [project-manager], 
                                                            Max([project-controls])        AS [project-controls], 
                                                            Max([account-representative])  AS [account-representative], 
                                                            Max([contract-representative]) AS [contract-representative], 
                                                            Max([company-president])       AS [company-president] 
                                                    FROM   (SELECT contractguid, 
                                                                    [regional-manager], 
                                                                    [project-manager], 
                                                                    [project-controls], 
                                                                    [account-representative], 
                                                                    [contract-representative], 
                                                                    [company-president] 
                                                            FROM   contractuserrole 
                                                                    PIVOT ( Max(userguid) 
                                                                            FOR userrole IN ([regional-manager], 
                                                                                            [project-manager], 
                                                                                            [project-controls], 
                                                                                            [account-representative], 
                                                                                            [contract-representative], 
                                                                                            [company-president]) ) tt) t 
                                                    GROUP  BY contractguid) tbl1 
                                                    GROUP  BY tbl1.contractguid, 
                                                                [regional-manager], 
                                                                [project-manager], 
                                                                [project-controls], 
                                                                [account-representative], 
                                                                [contract-representative], 
                                                                [company-president]) keyPersonnel
							inner join Contract c on c.ContractGuid = keyPersonnel.ContractGuid
							LEFT JOIN JobRequest
                            ON c.ContractGuid = JobRequest.ContractGuid
                            LEFT JOIN Users UserCreator
                            ON JobRequest.CreatedBy = UserCreator.UserGuid
                            INNER JOIN Users projectManager
							ON projectManager.UserGuid =  keyPersonnel.[project-manager]
							INNER JOIN Users projectControls
							ON projectControls.UserGuid = keyPersonnel.[project-controls]
							INNER JOIN Users accountRep
							ON accountRep.UserGuid = keyPersonnel.[account-representative]
							INNER JOIN Users contractRep
							ON contractRep.UserGuid = keyPersonnel.[contract-representative]
                            WHERE JobRequest.IsDeleted = 0
                            AND JobRequest.JobRequestGuid IN (SELECT JobRequestGuid 
                            FROM JobRequest
                            WHERE IsDeleted = 0)";
            sqlQuery += $"{ where }"; ;
            var result = _context.Connection.QuerySingle<int>(sqlQuery, new { searchValue = searchString });
            return result;
        }

        public Core.Entities.JobRequest GetDetailForJobRequestById(Guid id)
        {
            var jobRequestEntity = new Core.Entities.JobRequest();
            var jobSql = @"SELECT * 
                        FROM JobRequest 
                        WHERE ContractGuid = @contractGuid";
            jobRequestEntity = _context.Connection.Query<Core.Entities.JobRequest>(jobSql, new { contractGuid = id }).FirstOrDefault();
            return jobRequestEntity;
        }

        public int? GetJobRequestStatusByUserId(Guid loginUserGuid)
        {
            string sql = @"select JobRequest.Status 
                                                        from UserNotification usernotification
														join JobRequest 
														on JobRequest.JobRequestGuid = usernotification.ModuleGuid
														join 
                                                        UserNotificationMessage usernotificationmsg 
                                                        on usernotificationmsg.UserNotificationGuid = usernotification.NotificationGuid
                                                        --where usernotification.ModuleGuid = '914e21b5-d16f-4bf2-abf1-cc90d11b25bf'
														
                                                        and usernotificationmsg.Status != 0
                                                        and usernotificationmsg.UserGuid = @loginUserGuid;";
            var status = _context.Connection.QueryFirstOrDefault<int?>(sql, new { loginUserGuid = loginUserGuid });
            return status;
        }

        public void InsertContractBasicInfo(BasicContractInfoModel jobRequest, Guid contractGuid)
        {
            _context.Connection.Execute(@"UPDATE [Contract]
                    SET 
                         [ProjectNumber] = @ProjectNumber
                    WHERE ContractGuid = @ContractGuid",
                new
                {
                    ProjectNumber = jobRequest.ProjectNumber,
                    ContractGuid = contractGuid
                });
        }

        public void InserContractKeyPersonel(KeyPersonnelModel jobRequest, Guid contractGuid)
        {
            _context.Connection.Execute(@"UPDATE [Contract]
                    SET 
                         [CompanyPresident] = @CompanyPresident,
                         [RegionalManager] = @RegionalManager,
                         [ContractRepresentative] = @ContractRepresentative,
                         [ProjectManager] = @ProjectManager,
                         [ProjectControls] = @ProjectControls,
                         [AccountingRepresentative] = @AccountingRepresentative
                    WHERE ContractGuid = @ContractGuid",
                 new
                 {
                     ContractGuid = contractGuid,
                     CompanyPresident = jobRequest.CompanyPresident,
                     RegionalManager = jobRequest.RegionalManager,
                     ContractRepresentative = jobRequest.ContractRepresentative,
                     ProjectManager = jobRequest.ProjectManager,
                     ProjectControls = jobRequest.ProjectControls,
                     AccountingRepresentative = jobRequest.AccountingRepresentative,
                 });
        }

        public int Delete(Guid[] ids)
        {
            foreach (var jobRequestGuid in ids)
            {
                var jobRequest = new
                {
                    JobRequestGuid = jobRequestGuid
                };
                string disableQuery = @"Update JobRequest set 
                                               IsDeleted   = 1
                                               where JobRequestGuid = @JobRequestGuid ";
                _context.Connection.Execute(disableQuery, jobRequest);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Disable(Guid[] ids)
        {
            foreach (var jobRequestGuid in ids)
            {
                var jobRequest = new
                {
                    JobRequestGuid = jobRequestGuid
                };
                string disableQuery = @"Update JobRequest set 
                                            IsActive   = 0
                                            where JobRequestGuid = @JobRequestGuid ";
                _context.Connection.Execute(disableQuery, jobRequest);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Enable(Guid[] ids)
        {
            foreach (var jobRequestGuid in ids)
            {
                var jobRequest = new
                {
                    JobRequestGuid = jobRequestGuid
                };
                string disableQuery = @"Update JobRequest set 
                                            IsActive   = 1
                                            where JobRequestGuid = @JobRequestGuid ";
                _context.Connection.Execute(disableQuery, jobRequest);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }

        public string GetCompanyName(string id)
        {
            string getCompany = @"select CompanyName from company where CompanyGuid = @CompanyGuid";
            return _context.Connection.QueryFirstOrDefault<string>(getCompany, new { CompanyGuid = id });
        }

        public Core.Entities.JobRequest GetJobRequestEntityByJobRequestId(Guid id)
        {
            var sql = $@"select * from jobRequest where jobRequestGuid = @jobRequestGuid";
            return _context.Connection.QueryFirstOrDefault<Core.Entities.JobRequest>(sql, new { jobRequestGuid = id });
        }

        public int GetCurrentStatusByGuid(Guid id)
        {
            var jobSql = @"SELECT TOP(1) Status 
                        FROM JobRequest 
                        WHERE ContractGuid = @contractGuid";
            var status = _context.Connection.QueryFirstOrDefault<int>(jobSql, new { contractGuid = id });
            return status;
        }
    }
}
