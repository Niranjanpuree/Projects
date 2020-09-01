using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Models;

namespace Northwind.Infrastructure.Data.Contract.ContractRefactor
{
    public class ContractsRepository : IContractsRepository
    {
        private readonly IResourceAttributeValueRepository _resoureAttributeValueRepo;

        private string GetOrderByColumn(string sortField)
        {
            if (string.IsNullOrWhiteSpace(sortField))
                return "ContractNumber";

            switch (sortField.ToLower())
            {
                case "contracttype":
                    sortField = "C.contractType";
                    break;
                case "accountingrepresentativename":
                    sortField = "ar.FirstName";
                    break;
                case "organizationname":
                    sortField = "Name";
                    break;
                case "isactivestatus":
                    sortField = "c.IsActive";
                    break;
                case "regionalmanagername":
                    sortField = "rm.FirstName";
                    break;
                case "g_a_percent":
                    sortField = "GAPercent";
                    break;
                case "contractrepresentativename":
                    sortField = "cr.FirstName";
                    break;
                case "psccodename":
                    sortField = "PSCCode";
                    break;
                case "naicscodename":
                    sortField = "NaicsCode";
                    break;
                case "projectcontrolsname":
                    sortField = "pc.FirstName";
                    break;
                case "projectmanagername":
                    sortField = "pm.FirstName";
                    break;
                case "blueskyaward_amount":
                    sortField = "BlueSkyAwardAmount";
                    break;
                case "companypresidentname":
                    sortField = "cp.FirstName";
                    break;
                case "awardingagencyofficename":
                    sortField = "AwardingAgency.CustomerName";
                    break;
                case "fundingagencyofficename":
                    sortField = "FundingAgency.CustomerName";
                    break;
                case "fee_percent":
                    sortField = "FeePercent";
                    break;
                case "applicablewagedetemination":
                    sortField = "ApplicableAageDetemination";
                    break;
                case "updatedon":
                    sortField = "c.UpdatedOn";
                    break;
                case "office_contractrepresentative":
                    sortField = "AwardingAgencyContractRepresentativeName";
                    break;
                case "office_contracttechnicalrepresent":
                    sortField = "AwardingAgencyContractTechnicalRepresentativeName";
                    break;
                case "fundingoffice_contractrepresentativename":
                    sortField = "FundingAgencyContractRepresentativeName";
                    break;
                case "fundingoffice_contracttechnicalrepresentname":
                    sortField = "FundingAgencyContractTechnicalRepresentativeName";
                    break;
                case "idiqcontract":
                    sortField = "isidiqContract";
                    break;
                case "countryofperformancename":
                    sortField = "ContractNumber";
                    break;
                case "createdon":
                    sortField = "c.CreatedOn";
                    break;
                case "status":
                    sortField = "c.Status";
                    break;
                case "companycode":
                    sortField = "com.CompanyName";
                    break;
            }
            return sortField;
        }

        private FinancialInformationModel GetDavisActByApplicableWageDetermination(string applicableWageDetermination)
        {
            var financialInformation = new FinancialInformationModel();
            var wageDeterminaton = new List<string>();
            if (!string.IsNullOrWhiteSpace(applicableWageDetermination))
            {
                wageDeterminaton = applicableWageDetermination.Split(',').ToList();
                foreach (var davis in wageDeterminaton)
                {
                    var wageAct = davis.Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(wageAct))
                    {
                        var resourceValue = _resoureAttributeValueRepo.GetResourceValuesByValue(wageAct);
                        if (wageAct == EnumGlobal.DaviesActType.DavisBaconAct.ToString().ToLower())
                        {
                            financialInformation.AppWageDetermineDavisBaconActType = resourceValue.FirstOrDefault().Name;
                            financialInformation.AppWageDetermineDavisBaconAct = davis.Trim();
                        }
                        if (wageAct == EnumGlobal.DaviesActType.ServiceContractAct.ToString().ToLower())
                        {
                            financialInformation.AppWageDetermineServiceContractActType = resourceValue.FirstOrDefault().Name;
                            financialInformation.AppWageDetermineServiceContractAct = davis.Trim();
                        }
                    }
                }
            }
            return financialInformation;
        }

        private List<AdvancedSearchRequest> GenerateOrgIDSearchRequestList(AdvancedSearchRequest request, OperatorName enumName, int index)
        {
            var requestList = new List<AdvancedSearchRequest>();
            if (request.Value.GetType() == typeof(string))
            {
                var advSearchRequest = new AdvancedSearchRequest();
                advSearchRequest.Operator = new AdvancedSearchOperator
                {
                    OperatorId = "1",
                    OperatorName = (int)enumName,
                    OperatorTitle = "LIKE",
                    OperatorType = (int)enumName
                };
                advSearchRequest.Attribute = new AdvancedSearchAttribute
                {
                    AttributeId = "OrgIDName" + index,
                    AttributeName = "OrgIDName" + index
                };
                advSearchRequest.Value = $"{request.Value}.";
                requestList.Add(advSearchRequest);
            }
            else if (request.Value.GetType() == typeof(JObject))
            {
                var obj = JObject.Parse(request.Value.ToString());
                var advSearchRequest = new AdvancedSearchRequest();
                advSearchRequest.Operator = new AdvancedSearchOperator
                {
                    OperatorId = "1",
                    OperatorName = (int)enumName,
                    OperatorTitle = "LIKE",
                    OperatorType = (int)enumName
                };
                advSearchRequest.Attribute = new AdvancedSearchAttribute
                {
                    AttributeId = "OrgIDName" + index,
                    AttributeName = "OrgIDName" + index
                };
                advSearchRequest.Value = $"{obj["value"]}.";
                requestList.Add(advSearchRequest);
            }
            else if (request.Value.GetType() == typeof(JArray))
            {
                var objectList = ((JArray)request.Value).Select(m => new
                {
                    Value = (string)m["value"]
                }).ToList();
                foreach (var item in objectList)
                {
                    var advSearchRequest = new AdvancedSearchRequest();
                    advSearchRequest.Operator = new AdvancedSearchOperator
                    {
                        OperatorId = "1",
                        OperatorName = (int)enumName,
                        OperatorTitle = "LIKE",
                        OperatorType = (int)enumName
                    };
                    advSearchRequest.Attribute = new AdvancedSearchAttribute
                    {
                        AttributeId = "OrgIDName" + index,
                        AttributeName = "OrgIDName" + index
                    };
                    if (enumName == OperatorName.StringLikeStartWith)
                        advSearchRequest.Value = $"{item.Value}.";
                    else
                        advSearchRequest.Value = $".{item.Value}.";
                    requestList.Add(advSearchRequest);
                    index++;
                }
            }
            return requestList;
        }

        IDatabaseContext _context;
        public ContractsRepository(IDatabaseContext context, IResourceAttributeValueRepository resourceAttributeValueRepo)
        {
            _resoureAttributeValueRepo = resourceAttributeValueRepo;
            _context = context;
        }

        //For mapping 1 to 1 relationship
        private Contracts MapContract(Contracts contract, ContractKeyPersonnel person)
        {
            contract.ProjectManager.DisplayName = person.DisplayName;
            return contract;
        }

        private IEnumerable<Contracts> GetContractListByActivity(string searchValue, string additionalFilter, Guid userGuid, int pageSize, int skip, int take, string orderBy, string dir)
        {
            var where = "";
            var searchString = "";
            var listQuery = string.Empty;
            var recentOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "ContractNumber";

            orderBy = GetOrderByColumn(orderBy);
            var joinRecentActivity = string.Empty;
            var activityOrdering = "desc";

            var userAction = additionalFilter;
            listQuery = $@"SELECT ContractGuid
                        FROM Contract C
                        LEFT JOIN OrgID O
                        ON O.OrgIDGuid = C.ORGID
                        LEFT JOIN RecentActivity RA
                        On RA.EntityGuid = C.ContractGuid AND RA.Entity='Contract' AND RA.IsDeleted = 0 AND RA.UserAction = '{userAction}'
                        WHERE C.IsDeleted = 0
                        AND ParentContractGuid is null {where}";

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (C.ContractNumber LIKE @searchValue Or O.title LIKE @searchValue Or C.[ProjectNumber] LIKE @searchValue OR C.[ContractTitle] LIKE @searchValue)";
                listQuery += $"{ where }";
            }
            var actionType = "";

            if (additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                dir = "desc";
                actionType = EnumGlobal.ActivityType.RecentlyViewed.ToString();
                recentOrderBy = " RA.UpdatedOn";
                orderBy = recentOrderBy;
                where = " AND RA.UserAction = @actionType AND RA.UserGuid=@userGuid";
                listQuery += $" {where} ORDER BY {recentOrderBy} {activityOrdering}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            }
            else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            {
                where = $" AND C.CreatedBy = @userGuid AND RA.UserAction = '{userAction}'";
                listQuery += $" {where} ODER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            }
            else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
            {
                actionType = EnumGlobal.ActivityType.MyFavorite.ToString();
                where = $" AND RA.UserAction = @actionType AND RA.UserGuid=@userGuid AND RA.UserAction = '{userAction}'";
                listQuery += $" {where} ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            }

            //listQuery += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var finalQuery = $@"SELECT * FROM Contract C 
                        LEFT JOIN ContractUserRole cu 
                        ON C.ContractGuid = cu.ContractGuid
                        LEFT JOIN Users u
                        ON u.UserGuid = cu.UserGuid
                        LEFT JOIN ContractResourceFile cf
                        ON cf.ResourceGuid = C.ContractGuid
                        LEFT JOIN OrgID O 
                        ON C.ORGID = O.OrgIDGuid
                        LEFT JOIN Country
                        ON c.CountryOfPerformance = Country.CountryId
                        LEFT JOIN RecentActivity RA
                        On RA.EntityGuid = C.ContractGuid
                        WHERE c.ContractGuid In ({listQuery})
                        ORDER BY {orderBy} {dir}";
            var contractDictionary = new Dictionary<Guid, Contracts>();
            var contracList = _context.Connection.Query<Contracts, ContractKeyPersonnel, ContractResourceFile, Organization, Contracts>(
                finalQuery,
                (contract, keyPerson, files, organisation) =>
                {
                    Contracts contractEntity = contract;
                    if (!contractDictionary.TryGetValue(contract.ContractGuid, out contractEntity))
                    {
                        contractEntity = contract;
                        contractEntity.KeyPersonnel = new List<ContractKeyPersonnel>();
                        contractEntity.ContractResourceFile = new List<ContractResourceFile>();
                        contractDictionary.Add(contractEntity.ContractGuid, contractEntity);
                    }

                    if (keyPerson != null && !contractEntity.KeyPersonnel.Any(x => x.UserRole == keyPerson.UserRole))
                    {
                        contractEntity.KeyPersonnel.Add(keyPerson);
                    }

                    if (files != null && !contractEntity.ContractResourceFile.Any(x => x.Keys == files.Keys))
                        contractEntity.ContractResourceFile.Add(files);
                    if (organisation != null)
                        contractEntity.Organisation = organisation;
                    return contractEntity;
                },
                new { searchValue = searchString, userGuid = userGuid, actionType = actionType },
                splitOn: "ContractUserRoleGuid,ContractResourceFileGuid,OrgIDGuid")
            .Distinct().ToList();


            return contracList;
        }

        private IEnumerable<Contracts> GetContractListByActivity(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {

            var where = "";
            var searchString = "";
            var whereEntity = "";
            var cumulativeAwardAmountQuery = @"((SELECT COALESCE(SUM(awardamount), 0) FROM ContractModification cm1 WHERE cm1.ContractGuid = c.ContractGuid AND cm1.IsDeleted = 0) 
                                                + c.AwardAmount 
                                                + (SELECT COALESCE(SUM(task.awardamount), 0) FROM [Contract] task WHERE task.ParentContractGuid = c.ContractGuid) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid = c.ContractGuid)) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM[ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";

            var cumulativeFundingAmountQuery = @"((SELECT COALESCE(SUM(fundingAmount),0) FROM ContractModification cm1 WHERE cm1.ContractGuid=c.ContractGuid AND cm1.IsDeleted = 0 ) 
                                                + c.fundingAmount 
                                                + (SELECT COALESCE(SUM(task.fundingAmount),0) FROM [Contract] task WHERE task.ParentContractGuid=c.ContractGuid) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid FROM Contract cm2 WHERE cm2.ParentContractGuid=c.ContractGuid)) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";
            List<AdvancedSearchRequest> withoutEntity = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> withEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeName != "CompanyCode" && c.Attribute.AttributeName != "RegionCode").ToList();
            List<AdvancedSearchRequest> companyEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeName == "CompanyCode" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> regionEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeName == "RegionCode" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            AdvancedSearchRequest revenueRequired = postValue.Where(c => c.IsEntity == false && (c.Attribute.AttributeName == "IsRevenueRecRequired")).FirstOrDefault();
            AdvancedSearchRequest revenueCompleted = postValue.Where(c => c.IsEntity == false && (c.Attribute.AttributeName == "IsRevenueRecCompleted")).FirstOrDefault();
            List<AdvancedSearchRequest> keyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> withoutKeyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> companyList = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> regionList = new List<AdvancedSearchRequest>();


            foreach (var entity in withEntity)
            {
                dynamic value = entity.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    if (v.id == ContractUserRole._accountRepresentative || v.id == ContractUserRole._companyPresident || v.id == ContractUserRole._contractRepresentative ||
                    v.id == ContractUserRole._projectControls || v.id == ContractUserRole._projectManager || v.id == ContractUserRole._regionalManager ||
                    v.id == ContractUserRole._subContractAdministrator || v.id == ContractUserRole._purchasingRepresentative || v.id == ContractUserRole._humanResourceRepresentative ||
                    v.id == ContractUserRole._qualityRepresentative || v.id == ContractUserRole._safetyOfficer || v.id == ContractUserRole._operationManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }
                else
                {
                    if (value.id == ContractUserRole._accountRepresentative || value.id == ContractUserRole._companyPresident || value.id == ContractUserRole._contractRepresentative ||
                    value.id == ContractUserRole._projectControls || value.id == ContractUserRole._projectManager || value.id == ContractUserRole._regionalManager ||
                    value.id == ContractUserRole._subContractAdministrator || value.id == ContractUserRole._purchasingRepresentative || value.id == ContractUserRole._humanResourceRepresentative ||
                    value.id == ContractUserRole._qualityRepresentative || value.id == ContractUserRole._safetyOfficer || value.id == ContractUserRole._operationManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }

            }

            var queryBuilder = new AdvancedSearchQueryBuilder(withoutEntity,"c");
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }


            foreach (var item in withoutKeyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }
            var queryBuilderEntity = new AdvancedSearchQueryBuilder(withoutKeyPersonnels);
            var queryEntity = queryBuilderEntity.getQuery();
            var _builderEntity = new SqlBuilder();
            var selectorEntity = _builderEntity.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilderEntity.getQuery())
            {
                _builderEntity.Where(d.sql, d.value);
            }

            if (withoutKeyPersonnels.Count > 0)
            {
                _builder.AddParameters(selectorEntity.Parameters);
                whereEntity = selectorEntity.RawSql.Replace("WHERE", " AND ");
            }

            // Adding key personnel in query
            foreach (var item in keyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }

            var queryBuilder1 = new AdvancedSearchQueryBuilder(keyPersonnels);
            var _builder1 = new SqlBuilder();
            var selector1 = _builder1.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilder1.getQuery())
            {
                _builder1.Where(d.sql, d.value);
            }

            //Subquery to search on Keypersonnel table
            if (keyPersonnels.Count > 0)
            {
                _builder.AddParameters(selector1.Parameters);
            }


            //for search by company...
            var index = 0;
            foreach (var company in companyEntity)
            {
                companyList = GenerateOrgIDSearchRequestList(company, OperatorName.StringLikeStartWith, index);
            }
            var companyQueryBuilder = new AdvancedSearchQueryBuilder(companyList);
            var companyQuery = companyQueryBuilder.getQuery();
            var _builderCompany = new SqlBuilder();
            var selectorCompany = _builderCompany.AddTemplate(" /**where**/");
            var companyParameter = string.Empty;
            foreach (dynamic d in companyQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderCompany.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorCompany.Parameters);

            //for search by region..
            foreach (var region in regionEntity)
            {
                regionList = GenerateOrgIDSearchRequestList(region, OperatorName.StringLike, index);
            }
            var regionQueryBuilder = new AdvancedSearchQueryBuilder(regionList);
            var regionQuery = regionQueryBuilder.getQuery();
            var _builderRegion = new SqlBuilder();
            var selectorRegion = _builderRegion.AddTemplate(" /**where**/");
            foreach (dynamic d in regionQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderRegion.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorRegion.Parameters);

            orderBy = GetOrderByColumn(orderBy);

            if (string.IsNullOrEmpty(orderBy))
            {
                if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
                {
                    orderBy = " RA.UpdatedOn ";
                }
                else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
                {
                    orderBy = " C.UpdatedOn ";
                }
                else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
                {
                    orderBy = " C.UpdatedOn ";
                }
                else
                {
                    orderBy = "C.ContractNumber";
                }
            }
            else if (orderBy == "ContractNumber")
            {
                if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
                {
                    orderBy = " RA.UpdatedOn ";
                    dir = dir == "asc" ? "desc" : "asc";
                }
                else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
                {
                    orderBy = " C.UpdatedOn ";
                }
            }

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                _builder.OrWhere("C.ContractNumber LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("OrgId.title LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ProjectNumber] LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ContractTitle] LIKE @searchValue", new { searchValue = searchString });

            }
            var activitySelect = string.Empty;
            var activityPivotQuery = string.Empty;
            if (selector.RawSql != "")
                where = selector.RawSql.Replace("WHERE", " AND ");
            if (selector1.RawSql != "")
                where += selector1.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorCompany.RawSql))
                where += selectorCompany.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorRegion.RawSql))
                where += selectorRegion.RawSql.Replace("WHERE", " AND ");

            //search by revenue
            if (revenueRequired != null)
            {
                var obj = JObject.Parse(revenueRequired.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND c.RevenueRecognitionGuid IS NOT NULL";
                else
                    where += " AND c.RevenueRecognitionGuid IS NULL";
            }

            //search bby revenue completed
            if (revenueCompleted != null)
            {
                var obj = JObject.Parse(revenueCompleted.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND rr.IsCompleted = 1";
                else
                    where += " AND rr.IsCompleted = 0";
            }



            var recentQuery = string.Empty;
            var actionType = "";
            var userAction = additionalFilter;
            var activitySelectQuery = ", ra.*";
            var activityJoinQuery = $@"INNER JOIN (SELECT Distinct(EntityGuid) As EntityGuid,MAX([MyFavorite]) as MyFavorite,MAX([RecentlyViewed]) as RecentlyViewed,MAX([UpdatedOn]) AS UpdatedOn
							FROM RecentActivity
							PIVOT  
							(  
							MAX(UserGuid)
							FOR UserAction IN  
							([MyFavorite],[RecentlyViewed])) tt
							where Entity = 'Contract' AND IsDeleted = 0
							GROUP BY EntityGuid, MyFavorite, RecentlyViewed,UpdatedOn) ra
							ON ra.EntityGuid = c.ContractGuid";
            if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;

                actionType = EnumGlobal.ActivityType.RecentlyViewed.ToString();
                where += $" AND ra.RecentlyViewed = '{userGuid}'";
                orderBy = " RA.UpdatedOn ";
            }
            else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            {
                where += $@" AND (C.CreatedBy = '{userGuid}' OR keyPersonnel.[account-representative] = '{userGuid}' OR keyPersonnel.[contract-representative] = '{userGuid}' or keyPersonnel.[project-manager] = '{userGuid}' or keyPersonnel.[project-controls] = '{userGuid}' OR keyPersonnel.[regional-manager] = '{userGuid}' OR keyPersonnel.[company-president] = '{userGuid}'
                        OR keyPersonnel.[subcontract-administrator] = '{userGuid}' OR keyPersonnel.[purchasing-representative] = '{userGuid}' OR keyPersonnel.[human-resource-representative] = '{userGuid}' OR keyPersonnel.[quality-representative] = '{userGuid}' OR keyPersonnel.[safety-officer] = '{userGuid}')";
            }
            else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;
                actionType = EnumGlobal.ActivityType.MyFavorite.ToString();
                where += $" AND ra.MyFavorite = '{userGuid}'";
                orderBy = " C.UpdatedOn ";
            }

            var taskOrderSql = string.Empty;
            if (isTaskOrder)
            {
                var taskCondition = where + whereEntity;
                taskCondition = taskCondition.Replace("[IsActive]", "c.[IsActive]");
                taskCondition = taskCondition.Replace("[UpdatedOn]", "c.[UpdatedOn]");
                taskCondition = taskCondition.Replace("[Description]", "c.[Description]");
                taskCondition = taskCondition.Replace("[CreatedOn]", "c.[CreatedOn]");

                taskCondition = taskCondition.Replace("[AwardAmount]", cumulativeAwardAmountQuery);
                taskCondition = taskCondition.Replace("[FundingAmount]", cumulativeFundingAmountQuery);
                taskOrderSql = $@"SELECT c.ParentContractGuid 
                            FROM (SELECT  tbl1.*
                            FROM   (SELECT ( contractguid )               AS ContractGuid, 
                                    Max([regional-manager])        AS [regional-manager], 
                                    Max([project-manager])         AS [project-manager], 
                                    Max([project-controls])        AS [project-controls], 
                                    Max([account-representative])  AS [account-representative], 
                                    Max([contract-representative]) AS [contract-representative], 
                                    Max([company-president])       AS [company-president], 
                                    Max([subcontract-administrator]) AS [subcontract-administrator],
                                    Max([purchasing-representative]) AS [purchasing-representative],
                                    Max([human-resource-representative]) AS [human-resource-representative],
                                    Max([quality-representative]) AS [quality-representative],
                                    Max([safety-officer]) AS [safety-officer],
                                    Max([operation-manager]) AS [operation-manager]
                            FROM   (SELECT contractguid, 
                                    [regional-manager], 
                                    [project-manager], 
                                    [project-controls], 
                                    [account-representative], 
                                    [contract-representative], 
                                    [company-president],
                                    [subcontract-administrator],
                                    [purchasing-representative],
                                    [human-resource-representative],
                                    [quality-representative],
                                    [safety-officer],
                                    [operation-manager]
                            FROM   contractuserrole 
                                    PIVOT ( Max(userguid) 
                                            FOR userrole IN ([regional-manager], 
                                                            [project-manager], 
                                                            [project-controls], 
                                                            [account-representative], 
                                                            [contract-representative], 
                                                            [company-president],
                                                            [subcontract-administrator],
                                                            [purchasing-representative],
                                                            [human-resource-representative],
                                                            [quality-representative],
                                                            [safety-officer],
                                                            [operation-manager]) ) tt) t 
                            GROUP  BY contractguid) tbl1 
                            GROUP  BY tbl1.contractguid, 
                            [regional-manager], 
                            [project-manager], 
                            [project-controls], 
                            [account-representative], 
                            [contract-representative],
                            [company-president],
                            [subcontract-administrator],
                            [purchasing-representative],
                            [human-resource-representative],
                            [quality-representative],
                            [safety-officer],
                            [operation-manager]) keyPersonnel
							LEFT JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
                            LEFT JOIN Users pm
							ON pm.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users pc
							ON pc.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users ar
							ON ar.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users cr
							ON cr.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users rm
                            ON rm.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users cp
                            ON cp.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Users sca
                            ON sca.UserGuid = keyPersonnel.[subcontract-administrator]
                            LEFT JOIN Users pr
                            ON pr.UserGuid = keyPersonnel.[purchasing-representative]
                            LEFT JOIN Users hrr
                            ON hrr.UserGuid = keyPersonnel.[human-resource-representative]
                            LEFT JOIN Users qr
                            ON qr.UserGuid = keyPersonnel.[quality-representative]
                            LEFT JOIN Users so
                            ON so.UserGuid = keyPersonnel.[safety-officer]
                            LEFT JOIN Users om
                            ON om.UserGuid = keyPersonnel.[operation-manager]
                            LEFT JOIN Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                            LEFT JOIN Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
                            LEFT JOIN CustomerContact OfficeContractRepresentative on C.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact OfficeContractTechnicalRepresent on  C.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractRepresentative on C.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractTechnicalRepresent on C.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN ResourceAttributeValue ContractType on c.ContractType = ContractType.Value
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        WHERE c.IsDeleted = 0
						and c.ParentContractGuid is NOT null {taskCondition}";

                //where += $" OR c.ContractGuid IN ({taskOrderSql})";
            }

            var sqlQuery = $@"SELECT c.[ContractGuid]
                                      ,c.[IsIDIQContract]
                                      ,[IsPrimeContract]
                                      ,[ContractNumber]
                                      ,[SubContractNumber]
                                      ,[ORGID]
                                      ,[ProjectNumber]
                                      ,[ContractTitle]
                                      ,[CountryOfPerformance]
                                      ,[PlaceOfPerformance]
                                      ,[POPStart]
                                      ,[POPEnd]
                                      ,[NaicsCode]
                                      ,[PSCCode]
                                      ,[CPAREligible]
                                      ,[QualityLevelRequirements]
                                      ,[QualityLevel]
                                      ,[AwardingAgencyOffice]
                                      ,[OfficeContractRepresentative]
                                      ,[OfficeContractTechnicalRepresent]
                                      ,[FundingAgencyOffice]
                                      ,[SetAside]
                                      ,[SelfPerformancePercent]
                                      ,[SBA]
                                      ,[Competition]
                                      ,c.[ContractType]
                                      ,[OverHead]
                                      ,[GAPercent]
                                      ,[FeePercent]
                                      ,[Currency]
                                      ,[BlueSkyAwardAmount]
                                      ,[BillingAddress]
                                      ,[BillingFrequency]
                                      ,[InvoiceSubmissionMethod]
                                      ,[PaymentTerms]
                                      ,[AppWageDetermineDavisBaconAct]
                                      ,[BillingFormula]
                                      ,[RevenueFormula]
                                      ,[RevenueRecognitionEACPercent]
                                      ,[OHonsite]
                                      ,[OHoffsite]
                                      ,c.[CreatedOn]
                                      ,c.[UpdatedOn]
                                      ,c.[CreatedBy]
                                      ,c.[IsActive]
                                      ,c.[Status]
                                      ,c.[Description]
                                      ,[AppWageDetermineServiceContractAct]
                                      ,[FundingOfficeContractRepresentative]
                                      ,[FundingOfficeContractTechnicalRepresent]
                                      ,[ParentContractGuid]
                                      ,[ProjectCounter]
                                      ,[ApplicableWageDetermination]
                                      ,[RevenueRecognitionGuid],
                                        com.CompanyName,
                                       (SELECT TOP(1)CompanyName FROM Company WHERE CompanyCode = SUBSTRING(OrgID.Name,CHARINDEX('.',OrgID.Name)-2,2)) as CompanyName,
                                       --OrgID.Name as companyName,
                                       --OrgID.Name as regionName,
                                    AwardAmount = {cumulativeAwardAmountQuery},
                                    FundingAmount = {cumulativeFundingAmountQuery},
                                    ContractType.Name As ContractType,AwardingAgency.CustomerName AwardingAgencyName,FundingAgency.CustomerName FundingAgencyName,
                                    (OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) AwardingAgencyContractTechnicalRepresentativeName,
                                    (OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) AwardingAgencyContractRepresentativeName,
                                    (FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingAgencyContractRepresentativeName,
                                    (FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingAgencyContractTechnicalRepresentativeName,
                                    --keyPersonnel.*,
                                    cr.UserGuid as ContractRepresentativeGuid, cr.*,
									pc.UserGuid as ProjectControlGuid,pc.*,
                                    pm.UserGuid AS ProjectManagerGuid, pm.*,
									ar.UserGuid as AccountRepresentativeGuid,ar.*, 
                                    rm.UserGuid AS RegionalManagerGuid,rm.*,
                                    cp.UserGuid AS CompanyPresidentGuid,cp.*,
                                    sca.UserGuid AS SubContractAdministratorGuid,sca.*,
                                    pr.UserGuid AS PurchasingRepresentativeGuid,pr.*,
                                    hrr.UserGuid AS HumanResourceRepresentativeGuid,hrr.*,
                                    qr.UserGuid AS QualityRepresentativeGuid,qr.*,
                                    so.UserGuid AS SafetyOfficerGuid,so.*,
                                    om.UserGuid AS OperationManagerGuid,om.*,
									SUBSTRING(OrgID.Name,CHARINDEX('.',OrgID.Name)-2,2) as regionName,
                                    OrgID.Name as orgidName,OrgID.*  {activitySelect}
                            FROM (SELECT  tbl1.*
                            FROM   (SELECT ( contractguid )               AS ContractGuid, 
                                    Max([regional-manager])        AS [regional-manager], 
                                    Max([project-manager])         AS [project-manager], 
                                    Max([project-controls])        AS [project-controls], 
                                    Max([account-representative])  AS [account-representative], 
                                    Max([contract-representative]) AS [contract-representative], 
                                    Max([company-president])       AS [company-president],
                                    Max([subcontract-administrator]) AS [subcontract-administrator],
                                    Max([purchasing-representative]) AS [purchasing-representative],
                                    Max([human-resource-representative]) AS [human-resource-representative],
                                    Max([quality-representative]) AS [quality-representative],
                                    Max([safety-officer]) AS [safety-officer],
                                    Max([operation-manager]) AS [operation-manager]
                            FROM   (SELECT contractguid, 
                                    [regional-manager], 
                                    [project-manager], 
                                    [project-controls], 
                                    [account-representative], 
                                    [contract-representative], 
                                    [company-president],
                                    [subcontract-administrator],
                                    [purchasing-representative],
                                    [human-resource-representative],
                                    [quality-representative],
                                    [safety-officer],
                                    [operation-manager]
                            FROM   contractuserrole 
                                    PIVOT ( Max(userguid) 
                                            FOR userrole IN ([regional-manager], 
                                                            [project-manager], 
                                                            [project-controls], 
                                                            [account-representative], 
                                                            [contract-representative], 
                                                            [company-president],
                                                            [subcontract-administrator],
                                                            [purchasing-representative],
                                                            [human-resource-representative],
                                                            [quality-representative],
                                                            [safety-officer],
                                                            [operation-manager]) ) tt) t 
                            GROUP  BY contractguid) tbl1 
                            GROUP  BY tbl1.contractguid, 
                            [regional-manager], 
                            [project-manager], 
                            [project-controls], 
                            [account-representative], 
                            [contract-representative], 
                            [company-president],
                            [subcontract-administrator],
                            [purchasing-representative],
                            [human-resource-representative],
                            [quality-representative],
                            [safety-officer],
                            [operation-manager]) keyPersonnel
							LEFT JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
							{activityPivotQuery}
                            LEFT JOIN Users pm
							ON pm.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users pc
							ON pc.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users ar
							ON ar.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users cr
							ON cr.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users rm
                            ON rm.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users cp
                            ON cp.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Users sca
                            ON sca.UserGuid = keyPersonnel.[subcontract-administrator]
                            LEFT JOIN Users pr
                            ON pr.UserGuid = keyPersonnel.[purchasing-representative]
                            LEFT JOIN Users hrr
                            ON hrr.UserGuid = keyPersonnel.[human-resource-representative]
                            LEFT JOIN Users qr
                            ON qr.UserGuid = keyPersonnel.[quality-representative]
                            LEFT JOIN Users so
                            ON so.UserGuid = keyPersonnel.[safety-officer]
                            LEFT JOIN Users om
                            ON om.UserGuid = keyPersonnel.[operation-manager]
                            LEFT JOIN Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                            LEFT JOIN Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
                            LEFT JOIN CustomerContact OfficeContractRepresentative on C.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact OfficeContractTechnicalRepresent on  C.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractRepresentative on C.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractTechnicalRepresent on C.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN ResourceAttributeValue ContractType on c.ContractType = ContractType.Value
                            LEFT JOIN RevenueRecognization rr ON rr.RevenueRecognizationGuid = c.RevenueRecognitionGuid
                        --LEFT JOIN ContractResourceFile cf
                        --ON cf.ResourceGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID ON c.ORGID = ORGID.OrgIDGuid
                        LEFT JOIN Company com ON OrgID.Name LIKE com.CompanyCode + '.%'
                        WHERE c.IsDeleted = 0
                        AND c.ParentContractGuid is null";

            //where = where.Replace("[IsActive]", "c.[IsActive]");
            //where = where.Replace("[UpdatedOn]", "c.[UpdatedOn]");
            //where = where.Replace("[Description]", "c.[Description]");
            //where = where.Replace("[CreatedOn]", "c.[CreatedOn]");

            where = where.Replace("c.[AwardAmount]", cumulativeAwardAmountQuery);
            where = where.Replace("c.[FundingAmount]", cumulativeFundingAmountQuery);
            sqlQuery += $"{ where } {whereEntity} ";
            if (isTaskOrder)
            {
                sqlQuery += $" OR c.ContractGuid IN ({taskOrderSql})";
            }
            sqlQuery += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var contractDictionary = new Dictionary<Guid, Contracts>();

            var objectArray = new[] {
                typeof(Contracts),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Organization)
            };

            try
            {
                var contractList = _context.Connection.Query<Contracts>(sqlQuery, objectArray, m =>
                {
                    var contractEntity = new Contracts();
                    contractEntity = m[0] as Contracts;
                    contractEntity.ContractRepresentative = m[1] as User;
                    contractEntity.ProjectControls = m[2] as User;
                    contractEntity.ProjectManager = m[3] as User;
                    contractEntity.AccountRepresentative = m[4] as User;
                    contractEntity.RegionalManager = m[5] as User;
                    contractEntity.CompanyPresident = m[6] as User;
                    contractEntity.SubContractAdministrator = m[7] as User;
                    contractEntity.PurchasingRepresentative = m[8] as User;
                    contractEntity.HumanResourceRepresentative = m[9] as User;
                    contractEntity.QualityRepresentative = m[10] as User;
                    contractEntity.SafetyOfficer = m[11] as User;
                    contractEntity.OperationManager = m[12] as User;
                    contractEntity.Organisation = m[13] as Organization;
                    return contractEntity;
                }, selector.Parameters,
                splitOn: "ContractRepresentativeGuid,ProjectControlGuid,ProjectManagerGuid,AccountRepresentativeGuid,RegionalManagerGuid,CompanyPresidentGuid,SubContractAdministratorGuid,PurchasingRepresentativeGuid,HumanResourceRepresentativeGuid,QualityRepresentativeGuid,SafetyOfficerGuid,OperationManagerGuid,orgidName", commandTimeout: 120).ToList();
                return contractList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        #region Start of Contract
        /// <summary>
        /// To get contract detail based on contract guid
        /// </summary>
        /// <param name="id">contract Guid</param>
        /// <returns>return contract detail</returns>
        /// 
        public Contracts GetContractEntityByContractId(Guid contractId)
        {
            var sql = $@"select * from contract where contractGuid = @contractGuid";
            return _context.Connection.QueryFirstOrDefault<Contracts>(sql, new { contractGuid = contractId });
        }

        public Contracts GetDetailById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }

            var contractDetail = new Contracts();
            var sqlQuery = @"SELECT * FROM Contract c 
                        LEFT JOIN ContractUserRole cu 
                        ON c.ContractGuid = cu.ContractGuid
                        LEFT JOIN Users u
                        ON u.UserGuid = cu.UserGuid
                        LEFT JOIN ContractResourceFile cf
                        ON cf.ResourceGuid = c.ContractGuid 
                        AND cf.Isfile =1
                        LEFT JOIN OrgID OrgID 
                        ON c.ORGID = ORGID.OrgIDGuid
                        LEFT JOIN JobRequest j
                        ON j.ContractGuid = c.ContractGuid
                        LEFT JOIN RevenueRecognization rr
                        ON rr.ContractGuid = c.ContractGuid
                            AND rr.IsDeleted = 0 and
                                rr.isActive = 1
                        WHERE c.ContractGuid = @contractGuid";
            var contractDictionary = new Dictionary<Guid, Contracts>();

            contractDetail = _context.Connection.Query<Contracts, ContractKeyPersonnel, ContractResourceFile, Organization, Core.Entities.JobRequest, RevenueRecognition, Contracts>(
                sqlQuery,
                (contract, keyPerson, files, organisation, jobRequest, revenue) =>
                {
                    Contracts contractEntity;

                    if (!contractDictionary.TryGetValue(contract.ContractGuid, out contractEntity))
                    {
                        contractEntity = contract;
                        contractDictionary.Add(contract.ContractGuid, contractEntity = contract);
                    }

                    if (contractEntity.KeyPersonnel == null)
                    {
                        contractEntity.KeyPersonnel = new List<ContractKeyPersonnel>();
                    }

                    var representative = new User
                    {
                        UserGuid = keyPerson.UserGuid,
                        Firstname = keyPerson.FirstName,
                        Lastname = keyPerson.LastName,
                        JobTitle = keyPerson.JobTitle,
                        DisplayName = keyPerson.DisplayName,
                        PersonalEmail = keyPerson.PersonalEmail,
                        WorkEmail = keyPerson.WorkEmail,
                        WorkPhone = keyPerson.WorkPhone,
                        MobilePhone = keyPerson.MobilePhone,
                        Department = keyPerson.Department,
                        Company = keyPerson.Company
                    };
                    if (keyPerson.UserRole == ContractUserRole._accountRepresentative)
                        contractEntity.AccountRepresentative = representative;
                    if (keyPerson.UserRole == ContractUserRole._projectManager)
                        contractEntity.ProjectManager = representative;
                    if (keyPerson.UserRole == ContractUserRole._projectControls)
                        contractEntity.ProjectControls = representative;
                    if (keyPerson.UserRole == ContractUserRole._contractRepresentative)
                        contractEntity.ContractRepresentative = representative;
                    if (keyPerson.UserRole == ContractUserRole._companyPresident)
                        contractEntity.CompanyPresident = representative;
                    if (keyPerson.UserRole == ContractUserRole._regionalManager)
                        contractEntity.RegionalManager = representative;
                    if (keyPerson.UserRole == ContractUserRole._deputyregionalManager)
                        contractEntity.DeputyRegionalManager = representative;
                    if (keyPerson.UserRole == ContractUserRole._hsregionalManager)
                        contractEntity.HealthAndSafetyRegionalManager = representative;
                    if (keyPerson.UserRole == ContractUserRole._bdregionalManager)
                        contractEntity.BusinessDevelopmentRegionalManager = representative;
                    if (keyPerson.UserRole == ContractUserRole._subContractAdministrator)
                        contractEntity.SubContractAdministrator = representative;
                    if (keyPerson.UserRole == ContractUserRole._purchasingRepresentative)
                        contractEntity.PurchasingRepresentative = representative;
                    if (keyPerson.UserRole == ContractUserRole._humanResourceRepresentative)
                        contractEntity.HumanResourceRepresentative = representative;
                    if (keyPerson.UserRole == ContractUserRole._qualityRepresentative)
                        contractEntity.QualityRepresentative = representative;
                    if (keyPerson.UserRole == ContractUserRole._safetyOfficer)
                        contractEntity.SafetyOfficer = representative;
                    if (keyPerson.UserRole == ContractUserRole._operationManager)
                        contractEntity.OperationManager = representative;

                    if (contractEntity.ContractResourceFile == null)
                        contractEntity.ContractResourceFile = new List<ContractResourceFile>();
                    if (files != null && !contractEntity.ContractResourceFile.Any(x => x.Keys == files.Keys))
                        contractEntity.ContractResourceFile.Add(files);
                    if (organisation != null)
                        contract.Organisation = organisation;
                    if (jobRequest != null)
                        contract.JobRequest = jobRequest;
                    if (revenue != null)
                        contract.RevenueRecognization = revenue;
                    return contractEntity;
                },
                    new { contractGuid = id },
                    splitOn: "ContractUserRoleGuid,ContractResourceFileGuid,OrgIDGuid,JobRequestGuid,RevenueRecognizationGuid")
                .FirstOrDefault();

            string customerInfoSql = @"select
                                     AwardingOffice.CustomerGuid AwardingAgencyOffice,
                AwardingOffice.CustomerName AwardingAgencyOfficeName,
                AwardingContractType.CustomerTypeName AwardingAgencyCustomerTypeName,
                OfficeContractRepresentative.ContactGuid OfficeContractRepresentative,
                (OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) OfficeContractRepresentativeName,
                OfficeContractTechnicalRepresent.ContactGuid OfficeContractTechnicalRepresent,

                OfficeContractRepresentative.PhoneNumber ContractRepresentativePhoneNumber,
                OfficeContractRepresentative.AltPhoneNumber ContractRepresentativeAltPhoneNumber,
                OfficeContractRepresentative.EmailAddress ContractRepresentativeEmailAddress,
                OfficeContractRepresentative.AltEmailAddress ContractRepresentativeAltEmailAddress,
                OfficeContractTechnicalRepresent.PhoneNumber ContractTechnicalRepresentativePhoneNumber,
                OfficeContractTechnicalRepresent.AltPhoneNumber ContractTechnicalRepresentativeAltPhoneNumber,
                OfficeContractTechnicalRepresent.EmailAddress ContractTechnicalRepresentativeEmailAddress,
                OfficeContractTechnicalRepresent.AltEmailAddress ContractTechnicalRepresentativeAltEmailAddress,

                FundingOfficeContractRepresentative.PhoneNumber OfficeContractRepresentativePhoneNumber,
                FundingOfficeContractRepresentative.AltPhoneNumber OfficeContractRepresentativeAltPhoneNumber,
                FundingOfficeContractRepresentative.EmailAddress OfficeContractRepresentativeEmailAddress,
                FundingOfficeContractRepresentative.AltEmailAddress OfficeContractRepresentativeAltEmailAddress,
                FundingOfficeContractTechnicalRepresent.PhoneNumber OfficeContractTechnicalRepresentativePhoneNumber,
                FundingOfficeContractTechnicalRepresent.AltPhoneNumber OfficeContractTechnicalRepresentativeAltPhoneNumber,
                FundingOfficeContractTechnicalRepresent.EmailAddress OfficeContractTechnicalRepresentativeEmailAddress,
                FundingOfficeContractTechnicalRepresent.AltEmailAddress OfficeContractTechnicalRepresentativeAltEmailAddress,

                    
                (OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) OfficeContractTechnicalRepresentName,

                FundingOffice.CustomerGuid FundingAgencyOffice,
                FundingOffice.CustomerName FundingAgencyOfficeName,
                FundingContractType.CustomerTypeName FundingAgencyCustomerTypeName,
                                     FundingOfficeContractRepresentative.ContactGuid FundingOfficeContractRepresentative,
                                     (FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingOfficeContractRepresentativeName,
                FundingOfficeContractTechnicalRepresent.ContactGuid FundingOfficeContractTechnicalRepresent,
                (FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingOfficeContractTechnicalRepresentName
                                     from Contract
                                     left join
                   Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
                   left join
                   CustomerType AwardingContractType on AwardingOffice.CustomerTypeGuid = AwardingContractType.CustomerTypeGuid
                   left join
                                     CustomerContact OfficeContractRepresentative on Contract.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                   left join
                   CustomerContact OfficeContractTechnicalRepresent on  Contract.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid

                                     left join
                   Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
                   left join
                   CustomerType FundingContractType on FundingOffice.CustomerTypeGuid = FundingContractType.CustomerTypeGuid
                   left join
                   CustomerContact FundingOfficeContractRepresentative on Contract.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                   left join
                   CustomerContact FundingOfficeContractTechnicalRepresent on Contract.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid

                                     WHERE Contract.ContractGuid =  @ContractGuid;";
            var customerInformation = _context.Connection.QueryFirstOrDefault<CustomerInformationModel>(customerInfoSql, new { ContractGuid = id });

            string financialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformancePercent,
							Contract.SBA,
							Competition.Name CompetitionType,
                            Contract.Competition,
							ContractType.Name ContractTypeName,
                            Contract.AddressLine1,
                            Contract.AddressLine2,
                            Contract.AddressLine3,
                            Contract.City,
                            Contract.FarContractTypeGuid,
                            Contract.Province,
                            Contract.County,
                            Contract.PostalCode,
                            Contract.ContractType,
							Contract.OverHead,
							Contract.GAPercent,
							Contract.FeePercent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAwardAmount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							Contract.ApplicableWageDetermination,
							BillingFormulaValue.Name BillingFormulaName,
							RevenueFormulaValue.Name RevenueFormulaName,
                            Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEACPercent,
							Contract.OHonsite,
							Contract.OHoffsite,
                            ((select COALESCE(sum(awardamount),0) from ContractModification cm1 where cm1.ContractGuid=Contract.ContractGuid AND cm1.IsDeleted = 0 ) + Contract.AwardAmount + (select COALESCE(sum(task.awardamount),0) from [Contract] task where task.ParentContractGuid=Contract.ContractGuid) + (select COALESCE(sum(cm1.awardamount),0) from [Contract] cm1 where cm1.ParentContractGuid in (select cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid=Contract.ContractGuid)) + (select COALESCE(sum(cm1.awardamount),0) from [ContractModification] cm1 where cm1.ContractGuid in (select cm.ContractGuid from ContractModification cm where ContractGuid in (select ContractGuid from Contract cc where cc.ParentContractGuid = Contract.ContractGuid)) )) as CumulativeAwardAmount,
                            ((select COALESCE(sum(fundingAmount),0) from ContractModification cm1 where cm1.ContractGuid=Contract.ContractGuid AND cm1.IsDeleted = 0 ) + Contract.fundingAmount + (select COALESCE(sum(task.fundingAmount),0) from [Contract] task where task.ParentContractGuid=Contract.ContractGuid) + (select COALESCE(sum(cm1.fundingAmount),0) from [Contract] cm1 where cm1.ParentContractGuid in (select cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid=Contract.ContractGuid)) + (select COALESCE(sum(cm1.fundingAmount),0) from [ContractModification] cm1 where cm1.ContractGuid in (select cm.ContractGuid from ContractModification cm where ContractGuid in (select ContractGuid from Contract cc where cc.ParentContractGuid = Contract.ContractGuid)) )) as CumulativeFundingAmount
                            from Contract
						    left join
						    ResourceAttributeValue SetAside on Contract.SetAside = SetAside.Value
						    left join
						    ResourceAttributeValue Competition on Contract.Competition = Competition.Value
						    left join
						    ResourceAttributeValue ContractType on Contract.ContractType = ContractType.Value
						    left join
						    ResourceAttributeValue Currency on Contract.Currency = Currency.Value
						    left join
						    ResourceAttributeValue BillingFrequency on Contract.BillingFrequency = BillingFrequency.Value
						    left join
						    ResourceAttributeValue InvoiceSubmissionMethod on Contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
						    left join
						    ResourceAttributeValue PaymentTerms on Contract.PaymentTerms = PaymentTerms.Value
                            LEFT JOIN ResourceAttributeValue BillingFormulaValue ON BillingFormulaValue.Value = Contract.BillingFormula
                            LEFT JOIN ResourceAttributeValue RevenueFormulaValue ON RevenueFormulaValue.Value = Contract.RevenueFormula
                            WHERE ContractGuid =  @ContractGuid;";
            var financialInformation = _context.Connection.QueryFirstOrDefault<FinancialInformationModel>(financialInfoSql, new { ContractGuid = id });

            if (financialInformation != null)
            {
                var daviesAct = GetDavisActByApplicableWageDetermination(financialInformation.ApplicableWageDetermination);
                //for splitting appwage davis bacon and appwage service contract
                financialInformation.AppWageDetermineDavisBaconActType = daviesAct.AppWageDetermineDavisBaconActType;
                financialInformation.AppWageDetermineDavisBaconAct = daviesAct.AppWageDetermineDavisBaconAct;
                financialInformation.AppWageDetermineServiceContractActType = daviesAct.AppWageDetermineServiceContractActType;
                financialInformation.AppWageDetermineServiceContractAct = daviesAct.AppWageDetermineServiceContractAct;
                contractDetail.FinancialInformation = financialInformation;
            }
            contractDetail.CustomerInformation = customerInformation;

            return contractDetail;
        }

        public Contracts GetDetailByContractNumber(string projectNumber)
        {
            if (string.IsNullOrEmpty(projectNumber))
            {
                return null;
            }

            var contractDetail = new Contracts();
            var sqlQuery = @"SELECT * FROM Contract c 
                        LEFT JOIN ContractUserRole cu 
                        ON c.ContractGuid = cu.ContractGuid
                        LEFT JOIN Users u
                        ON u.UserGuid = cu.UserGuid
                        LEFT JOIN ContractResourceFile cf
                        ON cf.ResourceGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID 
                        ON c.ORGID = ORGID.OrgIDGuid
                        LEFT JOIN JobRequest j
                        ON j.ContractGuid = c.ContractGuid
                        LEFT JOIN ContractQuestionaire cq
                        ON cq.ContractGuid = c.COntractGuid
                        LEFT JOIN RevenueRecognization rr
                        ON rr.ContractGuid = c.ContractGuid
                            AND rr.IsDeleted = 0 and
                                rr.isActive = 1
                        WHERE c.projectNumber = @projectNumber";
            var contractDictionary = new Dictionary<Guid, Contracts>();

            contractDetail = _context.Connection.Query<Contracts, ContractKeyPersonnel, ContractResourceFile, Organization, Core.Entities.JobRequest, ContractQuestionaire, RevenueRecognition, Contracts>(
                sqlQuery,
                (contract, keyPerson, files, organisation, jobRequest, questionaire, revenue) =>
                {
                    Contracts contractEntity;

                    if (!contractDictionary.TryGetValue(contract.ContractGuid, out contractEntity))
                    {
                        contractEntity = contract;
                        contractDictionary.Add(contract.ContractGuid, contractEntity = contract);
                    }

                    if (contractEntity.KeyPersonnel == null)
                    {
                        contractEntity.KeyPersonnel = new List<ContractKeyPersonnel>();
                    }

                    //if (keyPerson != null && !contractEntity.KeyPersonnel.Any(x => x.UserGuid == keyPerson.UserGuid && x.UserRole == keyPerson.UserRole))
                    //{
                    //    contractEntity.KeyPersonnel.Add(keyPerson);
                    //}
                    var representative = new User
                    {
                        UserGuid = keyPerson.UserGuid,
                        Firstname = keyPerson.FirstName,
                        Lastname = keyPerson.LastName,
                        JobTitle = keyPerson.JobTitle,
                        DisplayName = keyPerson.DisplayName,
                        PersonalEmail = keyPerson.PersonalEmail,
                        WorkEmail = keyPerson.WorkEmail,
                        WorkPhone = keyPerson.WorkPhone,
                        MobilePhone = keyPerson.MobilePhone,
                        Department = keyPerson.Department,
                        Company = keyPerson.Company
                    };
                    if (keyPerson.UserRole == ContractUserRole._accountRepresentative)
                        contractEntity.AccountRepresentative = representative;
                    if (keyPerson.UserRole == ContractUserRole._projectManager)
                        contractEntity.ProjectManager = representative;
                    if (keyPerson.UserRole == ContractUserRole._projectControls)
                        contractEntity.ProjectControls = representative;
                    if (keyPerson.UserRole == ContractUserRole._contractRepresentative)
                        contractEntity.ContractRepresentative = representative;
                    if (keyPerson.UserRole == ContractUserRole._companyPresident)
                        contractEntity.CompanyPresident = representative;
                    if (keyPerson.UserRole == ContractUserRole._regionalManager)
                        contractEntity.RegionalManager = representative;

                    if (contractEntity.ContractResourceFile == null)
                        contractEntity.ContractResourceFile = new List<ContractResourceFile>();
                    if (files != null && !contractEntity.ContractResourceFile.Any(x => x.Keys == files.Keys))
                        contractEntity.ContractResourceFile.Add(files);
                    if (organisation != null)
                        contract.Organisation = organisation;
                    if (jobRequest != null)
                        contract.JobRequest = jobRequest;
                    if (questionaire != null)
                        contract.ContractQuestionaire = questionaire;
                    if (revenue != null)
                        contract.RevenueRecognization = revenue;
                    return contractEntity;
                },
                    new { projectNumber },
                    splitOn: "ContractUserRoleGuid,ContractResourceFileGuid,OrgIDGuid,JobRequestGuid,ContractQuestionaireGuid,RevenueRecognizationGuid")
                .FirstOrDefault();

            string customerInfoSql = @"select
                                     AwardingOffice.CustomerGuid AwardingAgencyOffice,
                AwardingOffice.CustomerName AwardingAgencyOfficeName,
                AwardingContractType.CustomerTypeName AwardingAgencyCustomerTypeName,
                OfficeContractRepresentative.ContactGuid OfficeContractRepresentative,
                (OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) OfficeContractRepresentativeName,
                OfficeContractTechnicalRepresent.ContactGuid OfficeContractTechnicalRepresent,

                OfficeContractRepresentative.PhoneNumber ContractRepresentativePhoneNumber,
                OfficeContractRepresentative.AltPhoneNumber ContractRepresentativeAltPhoneNumber,
                OfficeContractRepresentative.EmailAddress ContractRepresentativeEmailAddress,
                OfficeContractRepresentative.AltEmailAddress ContractRepresentativeAltEmailAddress,
                OfficeContractTechnicalRepresent.PhoneNumber ContractTechnicalRepresentativePhoneNumber,
                OfficeContractTechnicalRepresent.AltPhoneNumber ContractTechnicalRepresentativeAltPhoneNumber,
                OfficeContractTechnicalRepresent.EmailAddress ContractTechnicalRepresentativeEmailAddress,
                OfficeContractTechnicalRepresent.AltEmailAddress ContractTechnicalRepresentativeAltEmailAddress,

                FundingOfficeContractRepresentative.PhoneNumber OfficeContractRepresentativePhoneNumber,
                FundingOfficeContractRepresentative.AltPhoneNumber OfficeContractRepresentativeAltPhoneNumber,
                FundingOfficeContractRepresentative.EmailAddress OfficeContractRepresentativeEmailAddress,
                FundingOfficeContractRepresentative.AltEmailAddress OfficeContractRepresentativeAltEmailAddress,
                FundingOfficeContractTechnicalRepresent.PhoneNumber OfficeContractTechnicalRepresentativePhoneNumber,
                FundingOfficeContractTechnicalRepresent.AltPhoneNumber OfficeContractTechnicalRepresentativeAltPhoneNumber,
                FundingOfficeContractTechnicalRepresent.EmailAddress OfficeContractTechnicalRepresentativeEmailAddress,
                FundingOfficeContractTechnicalRepresent.AltEmailAddress OfficeContractTechnicalRepresentativeAltEmailAddress,

                    
                (OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) OfficeContractTechnicalRepresentName,

                FundingOffice.CustomerGuid FundingAgencyOffice,
                FundingOffice.CustomerName FundingAgencyOfficeName,
                FundingContractType.CustomerTypeName FundingAgencyCustomerTypeName,
                                     FundingOfficeContractRepresentative.ContactGuid FundingOfficeContractRepresentative,
                                     (FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingOfficeContractRepresentativeName,
                FundingOfficeContractTechnicalRepresent.ContactGuid FundingOfficeContractTechnicalRepresent,
                (FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingOfficeContractTechnicalRepresentName
                                     from Contract
                                     left join
                   Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
                   left join
                   CustomerType AwardingContractType on AwardingOffice.CustomerTypeGuid = AwardingContractType.CustomerTypeGuid
                   left join
                                     CustomerContact OfficeContractRepresentative on Contract.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                   left join
                   CustomerContact OfficeContractTechnicalRepresent on  Contract.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid

                                     left join
                   Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
                   left join
                   CustomerType FundingContractType on FundingOffice.CustomerTypeGuid = FundingContractType.CustomerTypeGuid
                   left join
                   CustomerContact FundingOfficeContractRepresentative on Contract.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                   left join
                   CustomerContact FundingOfficeContractTechnicalRepresent on Contract.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid

                                     WHERE Contract.projectNumber =  @projectNumber;";
            var customerInformation = _context.Connection.QueryFirstOrDefault<CustomerInformationModel>(customerInfoSql, new { projectNumber });

            string financialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformancePercent,
							Contract.SBA,
							Competition.Name CompetitionType,
                            Contract.Competition,
							ContractType.Name ContractTypeName,
                            Contract.ContractType,
							Contract.OverHead,
							Contract.GAPercent,
							Contract.FeePercent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAwardAmount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							Contract.ApplicableWageDetermination,
							BillingFormulaValue.Name BillingFormulaName,
							RevenueFormulaValue.Name RevenueFormulaName,
                            Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEACPercent,
							Contract.OHonsite,
							Contract.OHoffsite,
                            ((select COALESCE(sum(awardamount),0) from ContractModification cm1 where cm1.ContractGuid=Contract.ContractGuid AND cm1.IsDeleted = 0 ) + Contract.AwardAmount + (select COALESCE(sum(task.awardamount),0) from [Contract] task where task.ParentContractGuid=Contract.ContractGuid) + (select COALESCE(sum(cm1.awardamount),0) from [Contract] cm1 where cm1.ParentContractGuid in (select cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid=Contract.ContractGuid)) + (select COALESCE(sum(cm1.awardamount),0) from [ContractModification] cm1 where cm1.ContractGuid in (select cm.ContractGuid from ContractModification cm where ContractGuid in (select ContractGuid from Contract cc where cc.ParentContractGuid = Contract.ContractGuid)) )) as CumulativeAwardAmount,
                            ((select COALESCE(sum(fundingAmount),0) from ContractModification cm1 where cm1.ContractGuid=Contract.ContractGuid AND cm1.IsDeleted = 0 ) + Contract.fundingAmount + (select COALESCE(sum(task.fundingAmount),0) from [Contract] task where task.ParentContractGuid=Contract.ContractGuid) + (select COALESCE(sum(cm1.fundingAmount),0) from [Contract] cm1 where cm1.ParentContractGuid in (select cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid=Contract.ContractGuid)) + (select COALESCE(sum(cm1.fundingAmount),0) from [ContractModification] cm1 where cm1.ContractGuid in (select cm.ContractGuid from ContractModification cm where ContractGuid in (select ContractGuid from Contract cc where cc.ParentContractGuid = Contract.ContractGuid)) )) as CumulativeFundingAmount
                            from Contract
						    left join
						    ResourceAttributeValue SetAside on Contract.SetAside = SetAside.Value
						    left join
						    ResourceAttributeValue Competition on Contract.Competition = Competition.Value
						    left join
						    ResourceAttributeValue ContractType on Contract.ContractType = ContractType.Value
						    left join
						    ResourceAttributeValue Currency on Contract.Currency = Currency.Value
						    left join
						    ResourceAttributeValue BillingFrequency on Contract.BillingFrequency = BillingFrequency.Value
						    left join
						    ResourceAttributeValue InvoiceSubmissionMethod on Contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
						    left join
						    ResourceAttributeValue PaymentTerms on Contract.PaymentTerms = PaymentTerms.Value
                            LEFT JOIN ResourceAttributeValue BillingFormulaValue ON BillingFormulaValue.Value = Contract.BillingFormula
                            LEFT JOIN ResourceAttributeValue RevenueFormulaValue ON RevenueFormulaValue.Value = Contract.RevenueFormula
                            WHERE projectNumber =  @projectNumber;";
            var financialInformation = _context.Connection.QueryFirstOrDefault<FinancialInformationModel>(financialInfoSql, new { projectNumber });

            if (financialInformation != null)
            {
                var daviesAct = GetDavisActByApplicableWageDetermination(financialInformation.ApplicableWageDetermination);
                //for splitting appwage davis bacon and appwage service contract
                financialInformation.AppWageDetermineDavisBaconActType = daviesAct.AppWageDetermineDavisBaconActType;
                financialInformation.AppWageDetermineDavisBaconAct = daviesAct.AppWageDetermineDavisBaconAct;
                financialInformation.AppWageDetermineServiceContractActType = daviesAct.AppWageDetermineServiceContractActType;
                financialInformation.AppWageDetermineServiceContractAct = daviesAct.AppWageDetermineServiceContractAct;
                contractDetail.FinancialInformation = financialInformation;
            }
            contractDetail.CustomerInformation = customerInformation;

            return contractDetail;
        }


        public string GetCompanyCodeByContractId(Guid contractGuid)
        {
            string companyCodeSql = @"SELECT SUBSTRING(OrgID.Name,1,2) 
                                    FROM Contract 
                                    LEFT JOIN OrgID 
                                    ON Contract.ORGID = OrgID.OrgIDGuid 
                                    WHERE ContractGuid = @contractGuid;";
            var companyCode = _context.Connection.Query<string>(companyCodeSql, new { contractGuid = contractGuid }).FirstOrDefault();
            return companyCode;
        }

        public string GetRegionCodeByContractuid(Guid contractGuid)
        {
            string regionCodeSql = $@"SELECT SUBSTRING(OrgID.Name,4,2) 
                                    FROM Contract 
                                    LEFT JOIN OrgID 
                                    ON Contract.ORGID = OrgID.OrgIDGuid 
                                    WHERE ContractGuid = @contractGuid;";
            var regionCode = _context.Connection.Query<string>(regionCodeSql, new { contractGuid = contractGuid }).FirstOrDefault();
            return regionCode;
        }

        public string GetOfficeCodeByContractGuid(Guid contractGuid)
        {
            string officeCodeSql = $@"SELECT SUBSTRING(OrgID.Name,7,2) 
                                    FROM Contract 
                                    LEFT JOIN OrgID 
                                    ON Contract.ORGID = OrgID.OrgIDGuid 
                                    WHERE ContractGuid = @contractGuid;";
            return _context.Connection.Query<string>(officeCodeSql, new { contractGuid = contractGuid }).FirstOrDefault();
        }

        public BasicContractInfoModel GetBasicContractInfoByContractGuid(Guid contractGuid)
        {
            string basicContractSql = @"select
                            Contract.IsPrimeContract, 
                            Contract.IsIDIQContract, 
                            Contract.ProjectNumber, 
                            Contract.ContractNumber, 
                            Contract.SubContractNumber, 
                            (OrgID.Name + ' ' + OrgID.Title) OrganizationName,
                            Contract.ORGID,
                            Contract.ContractTitle, 
                            Country.CountryId CountryOfPerformance,
                            Country.CountryName CountryOfPerformanceSelected,
                            --State.StateName PlaceOfPerformanceSelected,
                            Contract.PlaceOfPerformance PlaceOfPerformanceSelectedIds,
                            Contract.POPStart, 
                            Contract.POPEnd, 
                            (NAICS.Code +' '+ NAICS.Title) NAICSCodeName, 
                            Contract.NAICSCode,
                            PSC.CodeDescription PSCCodeName,
                            Contract.PSCCode,
                            Contract.CPAREligible, 
                            Contract.QualityLevelRequirements, 
                            QualityLevel.Name QualityLevelName,
                            Contract.QualityLevel,
                            Contract.Description
                            from Contract
                            left join
						    OrgID on Contract.ORGID = OrgID.OrgIDGuid
                            left join
						    Country on Contract.CountryOfPerformance = Country.CountryId
						    left join
						    State on Contract.PlaceOfPerformance = State.StateId
						    left join
						    NAICS on Contract.NAICSCode = NAICS.NAICSGuid
						    left join
						    PSC on Contract.PSCCode = PSC.PSCGuid
						    left join
						    ResourceAttributeValue QualityLevel on Contract.QualityLevel = QualityLevel.Value
                
                            WHERE ContractGuid =  @ContractGuid;";
            var basicContractInfo = _context.Connection.QueryFirstOrDefault<BasicContractInfoModel>(basicContractSql, new { ContractGuid = contractGuid });

            // to fetch States name through state id array..
            if (!string.IsNullOrEmpty(basicContractInfo.PlaceOfPerformanceSelectedIds))
            {
                var stateIdArr = basicContractInfo.PlaceOfPerformanceSelectedIds.Split(',');
                var stateIdArrWithStringCote = stateIdArr.Select(x => string.Format("'" + x + "'"));
                var formatQuery = string.Join(",", stateIdArrWithStringCote);
                var stateQuery = $"select StateName from State where StateId in ({formatQuery})";
                var stateNameArr = _context.Connection.Query<string>(stateQuery);
                basicContractInfo.PlaceOfPerformanceSelected = string.Join(" , ", stateNameArr);
            }

            return basicContractInfo;
        }

        /// <summary>
        /// To get list of contract
        /// </summary>
        /// <returns>return list of available contract</returns>
        public IEnumerable<Contracts> GetContractList(string searchValue, string additionalFilter, Guid userGuid, int pageSize, int skip, int take, string orderBy, string dir)
        {
            var where = "";
            var searchString = "";
            var listQuery = string.Empty;
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "ContractNumber";

            orderBy = GetOrderByColumn(orderBy);
            var joinRecentActivity = string.Empty;

            if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower() || additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                return GetContractListByActivity(searchValue, additionalFilter, userGuid, pageSize, skip, take, orderBy, dir);
            }

            listQuery = $@"SELECT ContractGuid
                        FROM Contract C
                        LEFT JOIN OrgID O
                        ON O.OrgIDGuid = C.ORGID
                        WHERE C.IsDeleted = 0
                        AND ParentContractGuid is null";
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (C.ContractNumber LIKE @searchValue Or O.title LIKE @searchValue Or C.[ProjectNumber] LIKE @searchValue OR C.[ContractTitle] LIKE @searchValue)";
                listQuery += $"{ where }";
            }

            if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            {
                where = $" AND C.CreatedBy = @userGuid";
                listQuery += $" {where}";
            }
            listQuery += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var finalQuery = $@"SELECT * FROM Contract C 
                        LEFT JOIN ContractUserRole cu 
                        ON C.ContractGuid = cu.ContractGuid
                        LEFT JOIN Users u
                        ON u.UserGuid = cu.UserGuid
                        LEFT JOIN ContractResourceFile cf
                        ON cf.ResourceGuid = C.ContractGuid
                        LEFT JOIN OrgID O 
                        ON C.ORGID = O.OrgIDGuid
                        LEFT JOIN Country
                        ON c.CountryOfPerformance = Country.CountryId
                        WHERE c.ContractGuid In ({listQuery})
                        ORDER BY {orderBy} {dir}";
            var contractDictionary = new Dictionary<Guid, Contracts>();
            var contracList = _context.Connection.Query<Contracts, ContractKeyPersonnel, ContractResourceFile, Organization, Contracts>(
                finalQuery,
                (contract, keyPerson, files, organisation) =>
                {
                    Contracts contractEntity = contract;
                    if (!contractDictionary.TryGetValue(contract.ContractGuid, out contractEntity))
                    {
                        contractEntity = contract;
                        contractEntity.KeyPersonnel = new List<ContractKeyPersonnel>();
                        contractEntity.ContractResourceFile = new List<ContractResourceFile>();
                        contractDictionary.Add(contractEntity.ContractGuid, contractEntity);
                    }

                    if (keyPerson != null && !contractEntity.KeyPersonnel.Any(x => x.UserRole == keyPerson.UserRole))
                    {
                        contractEntity.KeyPersonnel.Add(keyPerson);
                    }

                    if (files != null && !contractEntity.ContractResourceFile.Any(x => x.Keys == files.Keys))
                        contractEntity.ContractResourceFile.Add(files);
                    if (organisation != null)
                        contractEntity.Organisation = organisation;
                    return contractEntity;
                },
                new { searchValue = searchString, userGuid = userGuid },
                splitOn: "ContractUserRoleGuid,ContractResourceFileGuid,OrgIDGuid")
            .Distinct().ToList();


            return contracList;
        }

        public IEnumerable<Contracts> GetContractList(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            return GetContractListByActivity(searchValue, pageSize, skip, take, orderBy, dir, postValue, userGuid, additionalFilter, isTaskOrder);
        }

        public IEnumerable<Contracts> GetContraBctListCopy(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            return GetContractListByActivity(searchValue, pageSize, skip, take, orderBy, dir, postValue, userGuid, additionalFilter, isTaskOrder);
        }
        //for multiplay query
        public IEnumerable<Contracts> GetContract(string searchValue, int pageSize, int skip, int take, string orderBy, string dir)
        {
            var where = "";
            var searchString = "";
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "ContractNumber";

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (C.ContractNumber LIKE @searchValue Or OrgId.title LIKE @searchValue Or C.[ProjectNumber] LIKE @searchValue OR C.[ContractTitle] LIKE @searchValue)";
            }
            var sqlQuery = @"SELECT * FROM Contract c 
                        LEFT JOIN ContractUserRole cu 
                        ON c.ContractGuid = cu.ContractGuid
                        LEFT JOIN Users u
                        ON u.UserGuid = cu.UserGuid
                        LEFT JOIN ContractResourceFile cf
                        ON cf.ContractGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        WHERE c.IsDeleted = 0
                        AND c.ParentContractGuid is null";

            sqlQuery += $"{ where }";
            sqlQuery += $" ORDER BY {orderBy} {dir}";
            //sqlQuery += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var contractDictionary = new Dictionary<Guid, Contracts>();

            // var cList = _context.Connection.QueryMultiple();

            var contracList = _context.Connection.Query<Contracts, ContractKeyPersonnel, ContractResourceFile, Organization, Contracts>(
                sqlQuery,
                (contract, keyPerson, files, organisation) =>
                {
                    Contracts contractEntity = contract;
                    if (!contractDictionary.TryGetValue(contract.ContractGuid, out contractEntity))
                    {
                        contractEntity = contract;
                        contractEntity.KeyPersonnel = new List<ContractKeyPersonnel>();
                        contractEntity.ContractResourceFile = new List<ContractResourceFile>();
                        contractDictionary.Add(contractEntity.ContractGuid, contractEntity);
                    }

                    if (keyPerson != null && !contractEntity.KeyPersonnel.Any(x => x.UserRole == keyPerson.UserRole))
                    {
                        contractEntity.KeyPersonnel.Add(keyPerson);
                    }

                    if (files != null && !contractEntity.ContractResourceFile.Any(x => x.Keys == files.Keys))
                        contractEntity.ContractResourceFile.Add(files);

                    return contractEntity;
                },
                new { searchValue = searchString },
                splitOn: "ContractUserRoleGuid,ContractResourceFileGuid,OrgIDGuid")
            .Distinct().Skip(skip).Take(take).ToList();
            return contracList;
        }
        public int GetContractCount(string searchValue)
        {
            string where = "";
            string searchString = "";
            if (searchValue != "")
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (C.ContractNumber LIKE @searchValue Or OrgId.title LIKE @searchValue Or C.[ProjectNumber] LIKE @searchValue OR C.[ContractTitle] LIKE @searchValue)";
            }
            var sql = @"
                  SELECT count(Distinct C.ContractGuid)
                  FROM [Contract] C
                    LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                    WHERE C.IsDeleted = 0
                    AND c.ParentContractGuid is null";
            sql += $"{ where }";
            return _context.Connection.ExecuteScalar<int>(sql, new { searchValue = searchString });
        }

        public int GetContractCountByFilter(string searchValue, string additionalFilter, Guid userGuid)
        {
            string where = "";
            string searchString = "";
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (C.ContractNumber LIKE @searchValue Or OrgId.title LIKE @searchValue Or C.[ProjectNumber] LIKE @searchValue OR C.[ContractTitle] LIKE @searchValue)";
            }
            if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
                where += $" AND C.CreatedBy = @userGuid";
            var joinString = string.Empty;
            if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower() || additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                joinString = @" LEFT JOIN RecentActivity RA 
                                ON RA.EntityGuid = C.ContractGuid AND RA.IsDeleted = 0 AND RA.Entity = 'Contract'";
                where += " AND RA.UserAction=@actionType";
            }
            var sql = $@"
                  SELECT count(Distinct C.ContractGuid)
                  FROM [Contract] C
                    LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid {joinString}
                    WHERE C.IsDeleted = 0
                    AND c.ParentContractGuid is null";
            sql += $"{ where }";
            return _context.Connection.ExecuteScalar<int>(sql, new { searchValue = searchString, userGuid = userGuid, actionType = additionalFilter });
        }

        public int GetAdvanceContractSearchCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            var where = "";
            var searchString = "";
            var subQueryKeyPersonnel = "";
            var whereEntity = "";
            var cumulativeAwardAmountQuery = @"((SELECT COALESCE(SUM(awardamount), 0) FROM ContractModification cm1 WHERE cm1.ContractGuid = c.ContractGuid AND cm1.IsDeleted = 0) 
                                                + c.AwardAmount 
                                                + (SELECT COALESCE(SUM(task.awardamount), 0) FROM [Contract] task WHERE task.ParentContractGuid = c.ContractGuid) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid = c.ContractGuid)) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM[ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";

            var cumulativeFundingAmountQuery = @"((SELECT COALESCE(SUM(fundingAmount),0) FROM ContractModification cm1 WHERE cm1.ContractGuid=c.ContractGuid AND cm1.IsDeleted = 0 ) 
                                                + c.fundingAmount 
                                                + (SELECT COALESCE(SUM(task.fundingAmount),0) FROM [Contract] task WHERE task.ParentContractGuid=c.ContractGuid) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid FROM Contract cm2 WHERE cm2.ParentContractGuid=c.ContractGuid)) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";
            List<AdvancedSearchRequest> withoutEntity = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeTitle != "Company" && c.Attribute.AttributeTitle != "Region" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> withEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeTitle != "Company" && c.Attribute.AttributeTitle != "Region" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> companyEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeTitle == "Company" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> regionEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeTitle == "Region" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            AdvancedSearchRequest revenueRequired = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeName == "IsRevenueRecRequired").FirstOrDefault();
            AdvancedSearchRequest revenueCompleted = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeName == "IsRevenueRecCompleted").FirstOrDefault();
            List<AdvancedSearchRequest> keyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> withoutKeyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> companyList = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> regionList = new List<AdvancedSearchRequest>();

            foreach (var entity in withEntity)
            {
                dynamic value = entity.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    if (v.id == ContractUserRole._accountRepresentative || v.id == ContractUserRole._companyPresident || v.id == ContractUserRole._contractRepresentative ||
                    v.id == ContractUserRole._projectControls || v.id == ContractUserRole._projectManager || v.id == ContractUserRole._regionalManager ||
                    v.id == ContractUserRole._subContractAdministrator || v.id == ContractUserRole._purchasingRepresentative || v.id == ContractUserRole._humanResourceRepresentative ||
                    v.id == ContractUserRole._qualityRepresentative || v.id == ContractUserRole._safetyOfficer || v.id == ContractUserRole._operationManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }
                else
                {
                    if (value.id == ContractUserRole._accountRepresentative || value.id == ContractUserRole._companyPresident || value.id == ContractUserRole._contractRepresentative ||
                   value.id == ContractUserRole._projectControls || value.id == ContractUserRole._projectManager || value.id == ContractUserRole._regionalManager ||
                   value.id == ContractUserRole._subContractAdministrator || value.id == ContractUserRole._purchasingRepresentative || value.id == ContractUserRole._humanResourceRepresentative ||
                   value.id == ContractUserRole._qualityRepresentative || value.id == ContractUserRole._safetyOfficer || value.id == ContractUserRole._operationManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }

            }
            var queryBuilder = new AdvancedSearchQueryBuilder(withoutEntity,"c");
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }

            foreach (var item in withoutKeyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }
            var queryBuilderEntity = new AdvancedSearchQueryBuilder(withoutKeyPersonnels);
            var queryEntity = queryBuilderEntity.getQuery();
            var _builderEntity = new SqlBuilder();
            var selectorEntity = _builderEntity.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilderEntity.getQuery())
            {
                _builderEntity.Where(d.sql, d.value);
            }

            if (withoutKeyPersonnels.Count > 0)
            {
                _builder.AddParameters(selectorEntity.Parameters);
                whereEntity = selectorEntity.RawSql.Replace("WHERE", " AND ");
            }

            // Adding key personnel in query
            foreach (var item in keyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }

            var queryBuilder1 = new AdvancedSearchQueryBuilder(keyPersonnels);
            var _builder1 = new SqlBuilder();
            var selector1 = _builder1.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilder1.getQuery())
            {
                _builder1.Where(d.sql, d.value);
            }


            //for search by company...
            var index = 0;
            foreach (var company in companyEntity)
            {
                companyList = GenerateOrgIDSearchRequestList(company, OperatorName.StringLikeStartWith, index);
            }
            var companyQueryBuilder = new AdvancedSearchQueryBuilder(companyList);
            var companyQuery = companyQueryBuilder.getQuery();
            var _builderCompany = new SqlBuilder();
            var selectorCompany = _builderCompany.AddTemplate(" /**where**/");
            var companyParameter = string.Empty;
            foreach (dynamic d in companyQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderCompany.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorCompany.Parameters);

            //for search by region..
            foreach (var region in regionEntity)
            {
                regionList = GenerateOrgIDSearchRequestList(region, OperatorName.StringLike, index);
            }
            var regionQueryBuilder = new AdvancedSearchQueryBuilder(regionList);
            var regionQuery = regionQueryBuilder.getQuery();
            var _builderRegion = new SqlBuilder();
            var selectorRegion = _builderRegion.AddTemplate(" /**where**/");
            foreach (dynamic d in regionQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderRegion.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorRegion.Parameters);

            //Subquery to search on Keypersonnel table
            if (keyPersonnels.Count > 0)
            {
                _builder.AddParameters(selector1.Parameters);
                subQueryKeyPersonnel = @" AND c.ContractGuid in (SELECT contractguid 
                                            FROM   (SELECT ( contractguid )               AS ContractGuid, 
                                                           Max([regional-manager])        AS [regional-manager], 
                                                           Max([project-manager])         AS [project-manager], 
                                                           Max([project-controls])        AS [project-controls], 
                                                           Max([account-representative])  AS [account-representative], 
                                                           Max([contract-representative]) AS [contract-representative], 
                                                           Max([company-president])       AS [company-president],
                                                           Max([subcontract-administrator]) AS [subcontract-administrator],
                                                           Max([purchasing-representative]) AS [purchasing-representative],
                                                           Max([human-resource-representative]) AS [human-resource-representative],
                                                           Max([quality-representative]) AS [quality-representative],
                                                           Max([safety-officer]) AS [safety-officer],
                                                           Max([operation-manager]) AS [operation-manager]
                                                    FROM   (SELECT contractguid, 
                                                                   [regional-manager], 
                                                                   [project-manager], 
                                                                   [project-controls], 
                                                                   [account-representative], 
                                                                   [contract-representative], 
                                                                   [company-president],
                                                                   [subcontract-administrator],
                                                                   [purchasing-representative],
                                                                   [human-resource-representative],
                                                                   [quality-representative],
                                                                   [safety-officer],
                                                                   [operation-manager]
                                                            FROM   contractuserrole 
                                                                   PIVOT ( Max(userguid) 
                                                                         FOR userrole IN ([regional-manager], 
                                                                                          [project-manager], 
                                                                                          [project-controls], 
                                                                                          [account-representative], 
                                                                                          [contract-representative], 
                                                                                          [company-president],
                                                                                          [subcontract-administrator],
                                                                                          [purchasing-representative],
                                                                                          [human-resource-representative],
                                                                                          [quality-representative],
                                                                                          [safety-officer],
                                                                                          [operation-manager]) ) tt) t 
                                                    GROUP  BY contractguid) tbl1 
                                            " + selector1.RawSql + @"
                                            GROUP  BY contractguid, 
                                                      [regional-manager], 
                                                      [project-manager], 
                                                      [project-controls], 
                                                      [account-representative], 
                                                      [contract-representative], 
                                                      [company-president],
                                                      [subcontract-administrator],
                                                      [purchasing-representative],
                                                      [human-resource-representative],
                                                      [quality-representative],
                                                      [safety-officer],
                                                      [operation-manager])";
            }

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                _builder.OrWhere("C.ContractNumber LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("OrgId.title LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ProjectNumber] LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ContractTitle] LIKE @searchValue", new { searchValue = searchString });

            }
            if (selector.RawSql != "")
                where = selector.RawSql.Replace("WHERE", " AND ");
            if (selector1.RawSql != "")
                where += selector1.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorCompany.RawSql))
                where += selectorCompany.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorRegion.RawSql))
                where += selectorRegion.RawSql.Replace("WHERE", " AND ");

            //search by revenue
            if (revenueRequired != null)
            {
                var obj = JObject.Parse(revenueRequired.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND c.RevenueRecognitionGuid IS NOT NULL";
                else
                    where += " AND c.RevenueRecognitionGuid IS NULL";
            }

            //search bby revenue completed
            if (revenueCompleted != null)
            {
                var obj = JObject.Parse(revenueCompleted.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND rr.IsCompleted = 1";
                else
                    where += " AND rr.IsCompleted = 0";
            }

            var actionType = "";
            var userAction = additionalFilter;
            var activitySelectQuery = ", ra.*";
            var activitySelect = string.Empty;
            var activityPivotQuery = string.Empty;
            var activityJoinQuery = $@"INNER JOIN (SELECT Distinct(EntityGuid) As EntityGuid,MAX([MyFavorite]) as MyFavorite,MAX([RecentlyViewed]) as RecentlyViewed,MAX([UpdatedOn]) AS UpdatedOn
							FROM RecentActivity
							PIVOT  
							(  
							MAX(UserGuid)
							FOR UserAction IN  
							([MyFavorite],[RecentlyViewed])) tt
							where Entity = 'Contract' AND IsDeleted = 0
							GROUP BY EntityGuid, MyFavorite, RecentlyViewed,UpdatedOn) ra
							ON ra.EntityGuid = c.ContractGuid";
            if (!string.IsNullOrWhiteSpace(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;
                actionType = EnumGlobal.ActivityType.RecentlyViewed.ToString();
                where += $" AND ra.RecentlyViewed = '{userGuid}'";
            }
            else if (!string.IsNullOrWhiteSpace(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            {
                where += $@" AND (C.CreatedBy = '{userGuid}' OR keyPersonnel.[account-representative] = '{userGuid}' OR keyPersonnel.[contract-representative] = '{userGuid}' or keyPersonnel.[project-manager] = '{userGuid}' or keyPersonnel.[project-controls] = '{userGuid}' OR keyPersonnel.[regional-manager] = '{userGuid}' OR keyPersonnel.[company-president] = '{userGuid}'
                        OR keyPersonnel.[subcontract-administrator] = '{userGuid}' OR keyPersonnel.[purchasing-representative] = '{userGuid}' OR keyPersonnel.[human-resource-representative] = '{userGuid}' OR keyPersonnel.[quality-representative] = '{userGuid}' OR keyPersonnel.[safety-officer] = '{userGuid}')";
            }
            else if (!string.IsNullOrWhiteSpace(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;
                actionType = EnumGlobal.ActivityType.MyFavorite.ToString();
                where += $" AND ra.MyFavorite = '{userGuid}'";
            }

            //if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            //    where += $" AND C.CreatedBy = '{userGuid}'";
            //var joinString = string.Empty;
            //if (!string.IsNullOrWhiteSpace(additionalFilter) && (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower() || additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower()))
            //{
            //    joinString = @" LEFT JOIN RecentActivity RA 
            //                    ON RA.EntityGuid = C.ContractGuid AND RA.IsDeleted = 0 AND RA.Entity = 'Contract'";
            //    where += $" AND RA.UserAction='{userAction}'";
            //}

            var taskOrderSql = string.Empty;
            if (isTaskOrder)
            {
                var taskCondition = where + whereEntity;
                taskCondition = taskCondition.Replace("[IsActive]", "c.[IsActive]");
                taskCondition = taskCondition.Replace("[UpdatedOn]", "c.[UpdatedOn]");
                taskCondition = taskCondition.Replace("[Description]", "c.[Description]");
                taskCondition = taskCondition.Replace("[CreatedOn]", "c.[CreatedOn]");

                taskCondition = taskCondition.Replace("[AwardAmount]", cumulativeAwardAmountQuery);
                taskCondition = taskCondition.Replace("[FundingAmount]", cumulativeFundingAmountQuery);
                taskOrderSql = $@"SELECT c.ParentContractGuid 
                            FROM (SELECT  tbl1.*
                            FROM   (SELECT ( contractguid )               AS ContractGuid, 
                                    Max([regional-manager])        AS [regional-manager], 
                                    Max([project-manager])         AS [project-manager], 
                                    Max([project-controls])        AS [project-controls], 
                                    Max([account-representative])  AS [account-representative], 
                                    Max([contract-representative]) AS [contract-representative], 
                                    Max([company-president])       AS [company-president],
                                    Max([subcontract-administrator]) AS [subcontract-administrator],
                                    Max([purchasing-representative]) AS [purchasing-representative],
                                    Max([human-resource-representative]) AS [human-resource-representative],
                                    Max([quality-representative]) AS [quality-representative],
                                    Max([safety-officer]) AS [safety-officer],
                                    Max([operation-manager]) AS [operation-manager]
                            FROM   (SELECT contractguid, 
                                    [regional-manager], 
                                    [project-manager], 
                                    [project-controls], 
                                    [account-representative], 
                                    [contract-representative], 
                                    [company-president],
                                    [subcontract-administrator],
                                    [purchasing-representative],
                                    [human-resource-representative],
                                    [quality-representative],
                                    [safety-officer],
                                    [operation-manager]
                            FROM   contractuserrole 
                                    PIVOT ( Max(userguid) 
                                            FOR userrole IN ([regional-manager], 
                                                            [project-manager], 
                                                            [project-controls], 
                                                            [account-representative], 
                                                            [contract-representative], 
                                                            [company-president],
                                                            [subcontract-administrator],
                                                            [purchasing-representative],
                                                            [human-resource-representative],
                                                            [quality-representative],
                                                            [safety-officer],
                                                            [operation-manager]) ) tt) t 
                            GROUP  BY contractguid) tbl1 
                            GROUP  BY tbl1.contractguid, 
                            [regional-manager], 
                            [project-manager], 
                            [project-controls], 
                            [account-representative], 
                            [contract-representative], 
                            [company-president],
                            [subcontract-administrator],
                            [purchasing-representative],
                            [human-resource-representative],
                            [quality-representative],
                            [safety-officer],
                            [operation-manager]) keyPersonnel
							LEFT JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
                            LEFT JOIN Users pm
							ON pm.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users pc
							ON pc.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users ar
							ON ar.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users cr
							ON cr.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users rm
                            ON rm.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users cp
                            ON cp.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Users sca
                            ON sca.UserGuid = keyPersonnel.[subcontract-administrator]
                            LEFT JOIN Users pr
                            ON pr.UserGuid = keyPersonnel.[purchasing-representative]
                            LEFT JOIN Users hrr
                            ON hrr.UserGuid = keyPersonnel.[human-resource-representative]
                            LEFT JOIN Users qr
                            ON qr.UserGuid = keyPersonnel.[quality-representative]
                            LEFT JOIN Users so
                            ON so.UserGuid = keyPersonnel.[safety-officer]
                            LEFT JOIN Users om
                            ON om.UserGuid = keyPersonnel.[operation-manager]
                            LEFT JOIN Customer AwardingAgency ON C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                            LEFT JOIN Customer FundingAgency ON C.FundingAgencyOffice = FundingAgency.CustomerGuid
                            LEFT JOIN CustomerContact OfficeContractRepresentative ON C.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact OfficeContractTechnicalRepresent ON  C.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractRepresentative ON C.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractTechnicalRepresent ON C.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN ResourceAttributeValue ContractType ON c.ContractType = ContractType.Value
                            LEFT JOIN RevenueRecognization rr ON rr.RevenueRecognizationGuid = c.RevenueRecognitionGuid
                            LEFT JOIN OrgID OrgID ON c.ORGID = ORGID.OrgIDGuid
                            LEFT JOIN Company com ON OrgID.Name LIKE com.CompanyCode + '.%'
                            WHERE c.IsDeleted = 0
						    AND c.ParentContractGuid IS NOT null {taskCondition}";
            }

            var sqlQuery = $@"SELECT COUNT(*)
                            FROM (SELECT  tbl1.*
                            FROM   (SELECT ( contractguid )               AS ContractGuid, 
                                    Max([regional-manager])        AS [regional-manager], 
                                    Max([project-manager])         AS [project-manager], 
                                    Max([project-controls])        AS [project-controls], 
                                    Max([account-representative])  AS [account-representative], 
                                    Max([contract-representative]) AS [contract-representative], 
                                    Max([company-president])       AS [company-president],
                                    Max([subcontract-administrator]) AS [subcontract-administrator],
                                    Max([purchasing-representative]) AS [purchasing-representative],
                                    Max([human-resource-representative]) AS [human-resource-representative],
                                    Max([quality-representative]) AS [quality-representative],
                                    Max([safety-officer]) AS [safety-officer],
                                    Max([operation-manager]) AS [operation-manager]
                            FROM   (SELECT contractguid, 
                                    [regional-manager], 
                                    [project-manager], 
                                    [project-controls], 
                                    [account-representative], 
                                    [contract-representative], 
                                    [company-president],
                                    [subcontract-administrator],
                                    [purchasing-representative],
                                    [human-resource-representative],
                                    [quality-representative],
                                    [safety-officer],
                                    [operation-manager]
                            FROM   contractuserrole 
                                    PIVOT ( Max(userguid) 
                                            FOR userrole IN ([regional-manager], 
                                                            [project-manager], 
                                                            [project-controls], 
                                                            [account-representative], 
                                                            [contract-representative], 
                                                            [company-president],
                                                            [subcontract-administrator],
                                                            [purchasing-representative],
                                                            [human-resource-representative],
                                                            [quality-representative],
                                                            [safety-officer],
                                                            [operation-manager]) ) tt) t 
                            GROUP  BY contractguid) tbl1 
                            GROUP  BY tbl1.contractguid, 
                            [regional-manager], 
                            [project-manager], 
                            [project-controls], 
                            [account-representative], 
                            [contract-representative], 
                            [company-president],
                            [subcontract-administrator],
                            [purchasing-representative],
                            [human-resource-representative],
                            [quality-representative],
                            [safety-officer],
                            [operation-manager]) keyPersonnel
							INNER JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
							{activityPivotQuery}
                            LEFT JOIN Users projectManager
							ON projectManager.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users projectControls
							ON projectControls.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users accountRep
							ON accountRep.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users contractRep
							ON contractRep.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users rm
                            ON rm.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users cp
                            ON cp.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Users sca
                            ON sca.UserGuid = keyPersonnel.[subcontract-administrator]
                            LEFT JOIN Users pr
                            ON pr.UserGuid = keyPersonnel.[purchasing-representative]
                            LEFT JOIN Users hrr
                            ON hrr.UserGuid = keyPersonnel.[human-resource-representative]
                            LEFT JOIN Users qr
                            ON qr.UserGuid = keyPersonnel.[quality-representative]
                            LEFT JOIN Users so
                            ON so.UserGuid = keyPersonnel.[safety-officer]
                            LEFT JOIN Users om
                            ON om.UserGuid = keyPersonnel.[operation-manager]
                        --LEFT JOIN ContractResourceFile cf
                        --ON cf.ResourceGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        LEFT JOIN RevenueRecognization rr ON rr.RevenueRecognizationGuid = c.RevenueRecognitionGuid
                        WHERE c.IsDeleted = 0
                        AND c.ParentContractGuid is null";
            //where = where.Replace("[IsActive]", "c.[IsActive]");
            //where = where.Replace("[UpdatedOn]", "c.[UpdatedOn]");
            //where = where.Replace("[CreatedOn]", "c.[CreatedOn]");
            //where = where.Replace("[Description]", "c.[Description]");
            where = where.Replace("c.[AwardAmount]", cumulativeAwardAmountQuery);
            where = where.Replace("c.[FundingAmount]", cumulativeFundingAmountQuery);
            sqlQuery += $"{ where } {whereEntity} ";

            if (isTaskOrder)
            {
                sqlQuery += $" OR c.ContractGuid IN ({taskOrderSql})";
            }
            return _context.Connection.ExecuteScalar<int>(sqlQuery, selector.Parameters);
        }

        public bool Insert(Contracts contract)
        {
            string insertQuery = $@"INSERT INTO [dbo].[Contract]
                                                                   (
                                                                        ContractGuid					                            ,
                                                                        ParentContractGuid					                        ,
                                                                        IsIDIQContract						                        ,
                                                                        IsPrimeContract						                        ,
                                                                        ContractNumber				                                ,
                                                                        SubContractNumber			                                ,
                                                                        ORGID						                                ,
                                                                        ProjectNumber				                                ,
                                                                        ContractTitle				                                ,
                                                                        Description				                                    ,
                                                                        CountryOfPerformance			                            ,
                                                                        PlaceOfPerformance			                                ,
                                                                        POPStart						                            ,
                                                                        POPEnd						                                ,
                                                                        NAICSCode					                                ,
                                                                        PSCCode						                                ,
                                                                        CPAREligible					                            ,
                                                                        QualityLevelRequirements		                            ,
                                                                        QualityLevel					                            ,
                                                                                                                                    
                                                                        AwardingAgencyOffice				                        ,
                                                                        OfficeContractRepresentative		                        ,
                                                                        OfficeContractTechnicalRepresent                           ,
                                                                        FundingAgencyOffice				                            ,
                                                                        FundingOfficeContractRepresentative				        ,
                                                                        FundingOfficeContractTechnicalRepresent				    ,
                                                                                                                                    
                                                                        SetAside						                            ,
                                                                        SelfPerformancePercent			                            ,
                                                                        SBA								                            ,
                                                                        Competition						                            ,
                                                                        ContractType					                            ,
                                                                        OverHead						                            ,
                                                                        GAPercent						                            ,
                                                                        FeePercent						                            ,
                                                                        Currency						                            ,
                                                                        BlueSkyAwardAmount				                            ,
                                                                        AwardAmount						                            ,
                                                                        FundingAmount					                            ,
                                                                        BillingAddress					                            ,
                                                                        BillingFrequency				                            ,
                                                                        InvoiceSubmissionMethod			                            ,
                                                                        PaymentTerms					                            ,
                                                                        ApplicableWageDetermination				                ,
                                                                        BillingFormula					                            ,
                                                                        RevenueFormula					                            ,
                                                                        RevenueRecognitionEACPercent	                            ,
                                                                        OHonsite						                            ,
                                                                        OHoffsite						                            ,
                                                                        ProjectCounter						                        ,
                                                                        CreatedOn						                            ,
                                                                        UpdatedOn						                            ,
                                                                        CreatedBy						                            ,
                                                                        UpdatedBy						                            ,
                                                                        IsActive						                            ,
                                                                        Status						                                ,
                                                                        AddressLine1						                        ,
                                                                        AddressLine2						                        ,
                                                                        AddressLine3						                        ,
                                                                        City						                                ,
                                                                        Province						                            ,
                                                                        County						                                ,
                                                                        PostalCode						                            ,
                                                                        FarContractTypeGuid						                    ,

                                                                        IsDeleted,
                                                                        IsImported
                                                                    )
                                  VALUES (
                                                                        @ContractGuid                                       ,
                                                                        @ParentContractGuid                                ,
                                                                        @IsIDIQContract                                     ,
                                                                        @IsPrimeContract                                    ,
                                                                        @ContractNumber                                     ,
                                                                        @SubContractNumber                                  ,
                                                                        @ORGID                                              ,
                                                                        @ProjectNumber                                      ,
                                                                        @ContractTitle                                      ,
                                                                        @Description                                        ,
                                                                        @CountryOfPerformance                               ,
                                                                        @PlaceOfPerformanceSelectedIds                      ,
                                                                        @POPStart                                           ,
                                                                        @POPEnd                                             ,
                                                                        @NAICSCode                                          ,
                                                                        @PSCCode                                            ,
                                                                        @CPAREligible                                       ,
                                                                        @QualityLevelRequirements                           ,
                                                                        @QualityLevel                                       ,
                                                                                                                                                        
                                                                        @AwardingAgencyOffice                             ,	
                                                                        @OfficeContractRepresentative                    ,	
                                                                        @OfficeContractTechnicalRepresent                ,
                                                                        @FundingAgencyOffice                              ,
                                                                        @FundingOfficeContractRepresentative             ,
                                                                        @FundingOfficeContractTechnicalRepresent         ,
                                                                        
                                                                        @setAside                                         ,
                                                                        @SelfPerformancePercent                          ,
                                                                        @SBA                                              ,
                                                                        @Competition                                      ,
                                                                        @ContractType                                     ,
                                                                        @OverHead                                         ,
                                                                        @GAPercent                                        ,
                                                                        @FeePercent                                       ,
                                                                        @Currency                                         ,
                                                                        @BlueSkyAwardAmount                               ,
                                                                        @AwardAmount                                      ,
                                                                        @FundingAmount                                    ,
                                                                        @BillingAddress                                   ,
                                                                        @BillingFrequency                                 ,
                                                                        @InvoiceSubmissionMethod                          ,
                                                                        @PaymentTerms                                     ,
                                                                        @ApplicableWageDetermination                      ,
                                                                        @BillingFormula                                   ,
                                                                        @RevenueFormula                                   ,
                                                                        @RevenueRecognitionEACPercent                     ,
                                                                        @OHonsite                                         ,
                                                                        @OHoffsite                                        ,
                                                                        @ProjectCounter                                   ,
                                                                        @CreatedOn                                        ,
                                                                        @UpdatedOn                                        ,
                                                                        @CreatedBy                                        ,
                                                                        @UpdatedBy                                        ,
                                                                        @IsActive                                         ,
                                                                        @Status                                           ,
                                                                        @AddressLine1						              ,
                                                                        @AddressLine2						              ,
                                                                        @AddressLine3						              ,
                                                                        @City						                      ,
                                                                        @Province						                  ,
                                                                        @County						                      ,
                                                                        @PostalCode						                  ,
                                                                        @FarContractTypeGuid						      ,
                                                                        @IsDeleted,
                                                                        @IsImported
                                                                )";
            _context.Connection.Execute(insertQuery, contract);

            //contract.ContractUserRole.ForEach(m => m.ContractGuid = contract.ContractGuid);
            //var personnalQuery = @"INSERT INTO [dbo].[ContractUserRole]
            //                     (
            //                        ContrractGuid,
            //                        UserGuid,
            //                        ContractUserRole,
            //                     )
            //                     VALUES 
            //                    (
            //                        @ContractGuid.
            //                        @UserGuid,
            //                        @ContractUserRole,
            //                    )";
            //using (var transaction = _context.Connection.BeginTransaction())
            //{
            //    _context.Connection.Query(personnalQuery, contract.ContractUserRole);
            //    transaction.Commit();
            //}

            return true;
        }

        public bool Update(Contracts contract)
        {
            string updateQuery = $@"Update Contract       set 
                                                                      ParentContractGuid						=           @ParentContractGuid                                                 ,
                                                                      IsIDIQContract						    =           @IsIDIQContract                                    ,
                                                                      IsPrimeContract						    =           @IsPrimeContract                                   ,
                                                                      ContractNumber				            =           @ContractNumber                                    ,
                                                                      SubContractNumber			                =           @SubContractNumber                                 ,
                                                                      ORGID						                =           @ORGID                                             ,
                                                                      ProjectNumber				                =           @ProjectNumber                                     ,
                                                                      ContractTitle				                =           @ContractTitle                                     ,
                                                                      Description				                =           @Description                                       ,
                                                                      CountryOfPerformance			            =           @CountryOfPerformance                              ,
                                                                      PlaceOfPerformance			            =           @PlaceOfPerformanceSelectedIds                     ,
                                                                      POPStart						            =           @POPStart                                          ,
                                                                      POPEnd						            =           @POPEnd                                            ,
                                                                      NAICSCode					                =           @NAICSCode                                         ,
                                                                      PSCCode						            =           @PSCCode                                           ,
                                                                      CPAREligible					            =           @CPAREligible                                      ,
                                                                      QualityLevelRequirements		            =           @QualityLevelRequirements                          ,
                                                                      QualityLevel					            =           @QualityLevel                                      ,
                                                                                                                                                                                                           
                                                                      AwardingAgencyOffice				        =           @AwardingAgencyOffice                            ,	
                                                                      OfficeContractRepresentative		        =           @OfficeContractRepresentative                   ,	
                                                                      OfficeContractTechnicalRepresent          =            @OfficeContractTechnicalRepresent               ,
                                                                      FundingAgencyOffice				        =           @FundingAgencyOffice                             ,
                                                                      FundingOfficeContractRepresentative		=	        @FundingOfficeContractRepresentative            ,
                                                                      FundingOfficeContractTechnicalRepresent	=	        @FundingOfficeContractTechnicalRepresent        ,
                                                                                                                            
                                                                      SetAside						            =           @setAside                                       ,
                                                                      SelfPerformancePercent			        =           @SelfPerformancePercent                        ,
                                                                      SBA								        =           @SBA                                            ,
                                                                      Competition						        =           @Competition                                    ,
                                                                      ContractType					            =           @ContractType                                   ,
                                                                      OverHead						            =           @OverHead                                       ,
                                                                      GAPercent						            =           @GAPercent                                    ,
                                                                      FeePercent						        =           @FeePercent                                    ,
                                                                      Currency						            =           @Currency                                       ,
                                                                      BlueSkyAwardAmount				        =           @BlueSkyAwardAmount                            ,
                                                                      AwardAmount						        =           @AwardAmount                                    ,
                                                                      FundingAmount					            =           @FundingAmount                                  ,
                                                                      BillingAddress					        =           @BillingAddress                                 ,
                                                                      BillingFrequency				            =           @BillingFrequency                               ,
                                                                      InvoiceSubmissionMethod			        =           @InvoiceSubmissionMethod                        ,
                                                                      PaymentTerms					            =           @PaymentTerms                                   ,
                                                                      ApplicableWageDetermination			    =	        @ApplicableWageDetermination                 ,
                                                                      BillingFormula					        =           @BillingFormula                                 ,
                                                                      RevenueFormula					        =           @RevenueFormula                                 ,
                                                                      RevenueRecognitionEACPercent	            =           @RevenueRecognitionEACPercent                  ,
                                                                      OHonsite						            =           @OHonsite                                       ,
                                                                      OHoffsite						            =           @OHoffsite                                      ,
                                                                      UpdatedOn						            =           @UpdatedOn                                      ,
                                                                      UpdatedBy						            =           @UpdatedBy                                      ,
                                                                      IsActive						            =           @IsActive                                       ,
                                                                      AddressLine1						        =           @AddressLine1                                   ,
                                                                      AddressLine2						        =           @AddressLine2                                   ,
                                                                      AddressLine3						        =           @AddressLine3                                   ,
                                                                      City						                =           @City                                           ,
                                                                      Province						            =           @Province                                       ,
                                                                      County						            =           @County                                         ,
                                                                      PostalCode						        =           @PostalCode                                     ,
                                                                      FarContractTypeGuid						=           @FarContractTypeGuid                                ,
                                                                      IsDeleted                                 =           @IsDeleted                                          
                                                                      where ContractGuid = @ContractGuid ";
            _context.Connection.Execute(updateQuery, contract);
            return true;
        }

        public bool DeleteByGuid(Guid[] guids)
        {

            foreach (var contractGuid in guids)
            {
                var contract = new
                {
                    ContractGuid = contractGuid
                };
                string disableQuery = @"Update Contract set 
                                               IsDeleted   = 1
                                               where ContractGuid =@ContractGuid ";
                _context.Connection.Execute(disableQuery, contract);
            }
            return true;
        }

        public bool EnableByGuid(Guid[] guids)
        {
            foreach (var contractGuid in guids)
            {
                var contract = new
                {
                    ContractGuid = contractGuid
                };
                string disableQuery = $@"Update Contract 
                                        SET 
                                            IsActive   = 1 , 
                                            status = 'Active'
                                        WHERE ContractGuid =@ContractGuid ";
                _context.Connection.Execute(disableQuery, contract);
            }
            return true;
        }

        public bool DisableByGuid(Guid[] guids)
        {
            foreach (var contractGuid in guids)
            {
                var contract = new
                {
                    ContractGuid = contractGuid
                };
                string disableQuery = @"Update Contract 
                                        SET 
                                            IsActive   = 0 , 
                                            status = 'Inactive'
                                        WHERE ContractGuid =@ContractGuid ";
                _context.Connection.Execute(disableQuery, contract);
            }
            return true;
        }

        public IEnumerable<Contracts> GetTaskByContractGuid(Guid contractGuid)
        {
            var cumulativeAwardAmountQuery = @"((SELECT COALESCE(SUM(awardamount), 0) FROM ContractModification cm1 WHERE cm1.ContractGuid = c.ContractGuid AND cm1.IsDeleted = 0) 
                                                + c.AwardAmount 
                                                + (SELECT COALESCE(SUM(task.awardamount), 0) FROM [Contract] task WHERE task.ParentContractGuid = c.ContractGuid) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid = c.ContractGuid)) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM[ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";

            var cumulativeFundingAmountQuery = @"((SELECT COALESCE(SUM(fundingAmount),0) FROM ContractModification cm1 WHERE cm1.ContractGuid=c.ContractGuid AND cm1.IsDeleted = 0 ) 
                                                + c.fundingAmount 
                                                + (SELECT COALESCE(SUM(task.fundingAmount),0) FROM [Contract] task WHERE task.ParentContractGuid=c.ContractGuid) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid FROM Contract cm2 WHERE cm2.ParentContractGuid=c.ContractGuid)) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";
            var sqlQuery = $@"SELECT c.*,
                                    AwardAmount = {cumulativeAwardAmountQuery},
                                    FundingAmount = {cumulativeFundingAmountQuery},
                                    com.CompanyName,
                                    ContractType.Name AS ContractType,AwardingAgency.CustomerName AwardingAgencyName,FundingAgency.CustomerName FundingAgencyName,
                                    (OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) AwardingAgencyContractRepresentativeName,
                                    (OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) AwardingAgencyContractTechnicalRepresentativeName,
                                    (FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingAgencyContractRepresentativeName,
                                    (FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingAgencyContractTechnicalRepresentativeName,
                                    keyPersonnel.*,
                                    contractRep.UserGuid as ProjectContractGuid, contractRep.*,
									projectControls.UserGuid as ProjectControlGuid,projectControls.*,
                                    projectManager.UserGuid AS ProjectManagerGuid, projectManager.*,
									accountRep.UserGuid as AccountRepresentativeGuid,accountRep.*, 
                                    regionalManager.UserGuid AS RegionalManagerGuid,regionalManager.*,
                                    companyPresident.UserGuid AS CompanyPresidentGuid,companyPresident.*,
                                    OrgID.Name as orgidName,OrgID.*
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
							INNER JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
                            LEFT JOIN Users projectManager
							ON projectManager.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users projectControls
							ON projectControls.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users accountRep
							ON accountRep.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users contractRep
							ON contractRep.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users regionalManager
                            ON regionalManager.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users companyPresident
                            ON companyPresident.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                            LEFT JOIN Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
                            LEFT JOIN CustomerContact OfficeContractRepresentative on C.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact OfficeContractTechnicalRepresent on  C.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractRepresentative on C.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractTechnicalRepresent on C.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN ResourceAttributeValue ContractType on c.ContractType = ContractType.Value
                        --LEFT JOIN ContractResourceFile cf
                        --ON cf.ResourceGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        LEFT JOIN Company com ON OrgID.Name LIKE com.CompanyCode + '.%'
                        WHERE c.IsDeleted = 0
                        AND c.ParentContractGuid = '{contractGuid}'";
            var contractDictionary = new Dictionary<Guid, Contracts>();

            var objectArray = new[] {
                typeof(Contracts),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Organization)
            };

            var contractList = _context.Connection.Query<Contracts>(sqlQuery, objectArray, m =>
            {
                var contractEntity = new Contracts();
                contractEntity = m[0] as Contracts;
                contractEntity.ContractRepresentative = m[1] as User;
                contractEntity.ProjectControls = m[2] as User;
                contractEntity.ProjectManager = m[3] as User;
                contractEntity.AccountRepresentative = m[4] as User;
                contractEntity.RegionalManager = m[5] as User;
                contractEntity.CompanyPresident = m[6] as User;
                contractEntity.Organisation = m[7] as Organization;
                return contractEntity;
            },
                splitOn: "ProjectContractGuid,ProjectControlGuid,ProjectManagerGuid,AccountRepresentativeGuid,RegionalManagerGuid,CompanyPresidentGuid,orgidName").ToList();

            return contractList;
            //var sqlQuery = @"SELECT * FROM Contract c 
            //            LEFT JOIN ContractUserRole cu 
            //            ON c.ContractGuid = cu.ContractGuid
            //            LEFT JOIN Users u
            //            ON u.UserGuid = cu.UserGuid
            //            LEFT JOIN ContractResourceFile cf
            //            ON cf.ResourceGuid = c.ContractGuid
            //            LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
            //            WHERE c.ParentContractGuid = @parentGuid
            //            AND c.IsDeleted = 0";


            //var contractDictionary = new Dictionary<Guid, Contracts>();

            //var contracList = _context.Connection.Query<Contracts, ContractKeyPersonnel, ContractResourceFile, Organization, Contracts>(
            //    sqlQuery,
            //    (contract, keyPerson, files, organisation) =>
            //    {
            //        Contracts contractEntity;
            //        contractEntity = contract;

            //        if (!contractDictionary.TryGetValue(contract.ContractGuid, out contractEntity))
            //        {
            //            contractEntity = contract;
            //            contractEntity.KeyPersonnel = new List<ContractKeyPersonnel>();
            //            contractDictionary.Add(contract.ContractGuid, contractEntity = contract);
            //        }

            //        if (keyPerson != null && !contractEntity.KeyPersonnel.Any(x => x.UserGuid == keyPerson.UserGuid))
            //        {
            //            contractEntity.KeyPersonnel.Add(keyPerson);
            //        }

            //        if (contractEntity.ContractResourceFile == null)
            //            contractEntity.ContractResourceFile = new List<ContractResourceFile>();
            //        if (files != null && !contractEntity.ContractResourceFile.Any(x => x.Keys == files.Keys))
            //            contractEntity.ContractResourceFile.Add(files);
            //        //resource attribute value() get from cache..
            //        return contractEntity;
            //    },
            //    new { parentGuid = contractGuid },
            //    splitOn: "ContractUserRoleGuid,ContractResourceFileGuid,OrgIDGuid")
            //.Distinct().ToList();


            //return contracList;
        }

        public bool IsContractNumberValid(string contractNumber, Guid contractGuid)
        {
            var sqlQuery = @"SELECT Count(*)
                            FROM Contract
                            WHERE ContractNumber = @contractNumber
                            AND ContractGuid != @contractGuid
                            AND IsDeleted = 0";
            var result = _context.Connection.QueryFirstOrDefault<int>(sqlQuery, new { contractNumber = contractNumber, contractGuid = contractGuid });
            if (result > 0)
                return false;
            return true;
        }

        public bool IsExistContractTitle(string contractTitle, Guid contractGuid)
        {
            var sqlQuery = @"SELECT Count(*)
                            FROM Contract
                            WHERE ContractTitle = @contractTitle 
                            AND ContractGuid != @contractGuid
                            AND IsDeleted = 0";
            var result = _context.Connection.QueryFirstOrDefault<int>(sqlQuery, new { contractTitle = contractTitle, contractGuid = contractGuid });
            if (result > 0)
                return true;
            return false;
        }

        public bool IsProjectNumberValid(string projectNumber)
        {
            var sqlQuery = @"SELECT Count(*)
                            FROM Contract
                            WHERE ProjectNumber = @projectNumber";
            var result = _context.Connection.QueryFirstOrDefault<int>(sqlQuery, new { projectNumber = projectNumber });
            if (result > 0)
                return false;
            return true;
        }

        public bool IsProjectNumberDuplicate(string projectNumber, Guid contractGuid)
        {
            var sqlQuery = @"SELECT Count(*)
                            FROM Contract
                            WHERE ProjectNumber = @projectNumber
                            AND ContractGuid != @contractGuid";
            var result = _context.Connection.QueryFirstOrDefault<int>(sqlQuery, new { projectNumber = projectNumber, contractGuid = contractGuid });
            if (result > 0)
                return false;
            return true;
        }

        public bool IsContractGuidValid(Guid contractGuid)
        {
            var sqlQuery = @"SELECT Count(*)
                            FROM Contract
                            WHERE ContractGuid = @contractGuid";
            var result = _context.Connection.QueryFirst(sqlQuery, new { contractGuid = contractGuid });
            if (result > 0)
                return false;
            return true;
        }

        public Contracts GetTaskDetailByParentGuid(Guid guid)
        {

            return new Contracts();
        }

        public Guid? GetPreviousTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
               select top 1 project.contractGuid 
                    from contract project
                         where project.IsDeleted = 0 
                           and project.ParentContractGuid ='{contractGuid}' 
                           and projectCounter < {currentProjectCounter}
                           order by projectCounter desc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery);
            return getPreviousProjectGuid;
        }

        public Guid? GetNextTaskOfContractByCounter(Guid contractGuid, int currentProjectCounter)
        {
            var getPreviousProjectGuidQuery = string.Format($@"
               select top 1 project.contractGuid 
                    from contract project
                         where project.IsDeleted = 0 
                           and project.ParentContractGuid ='{contractGuid}' 
                           and projectCounter > {currentProjectCounter}
                           order by projectCounter asc ");
            var getPreviousProjectGuid = _context.Connection.QueryFirstOrDefault<Guid?>(getPreviousProjectGuidQuery);
            return getPreviousProjectGuid;
        }

        //public string GetContractNumberById(Guid contractGuid)
        //{
        //    string sql = @"SELECT TOP(1) ContractNumber 
        //                    FROM Contract 
        //                    WHERE ContractGuid = @contractGuid;";
        //    var contractNumber = _context.Connection.QueryFirstOrDefault<string>(sql,new { contractGuid = contractGuid});
        //    return contractNumber;
        //}

        public bool IsIDIQContract(Guid contratGuid)
        {
            if (contratGuid == Guid.Empty)
                return false;
            var sql = @"SELECT TOP(1) IsIDIQContract
                FROM Contract
                WHERE ContractGuid = @contractGuid";
            return _context.Connection.QueryFirstOrDefault<bool>(sql, new { contractGuid = contratGuid });
        }

        public string GetContractNumberByGuid(Guid contractGuid)
        {
            var sql = @"SELECT TOP(1) ContractNumber
                        FROM Contract
                        WHERE ContractGuid = @contractGuid";
            var contractNumber = _context.Connection.QueryFirstOrDefault<string>(sql, new { contractGuid = contractGuid });
            return contractNumber;
        }
        public Guid GetContractIdByProjectId(Guid contractGuid)
        {
            var sql = @"SELECT TOP(1) ParentContractGuid
                        FROM Contract
                        WHERE ContractGuid = @contractGuid";
            var contractId = _context.Connection.QueryFirstOrDefault<Guid>(sql, new { contractGuid = contractGuid });
            return contractId;
        }

        public IEnumerable<Contracts> GetAllProject(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            var where = "";
            var searchString = "";
            var rowNum = pageSize + skip;

            if (sortField.ToLower().Equals("contracttitle"))
            {
                orderingQuery.Append($"contract.ContractTitle {sortDirection}");
            }
            else
            {
                orderingQuery.Append($"Contract.{sortField} {sortDirection}");
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(Contract.ContractNumber like @searchValue )";
            }

            var sqlQuery = string.Format($@"Select * 
                                                    FROM 
                                                        (SELECT ROW_NUMBER() OVER (ORDER BY @orderingQuery) AS RowNum, 
                                                                                        ContractGuid,
                                                                                        Contract.ProjectNumber,
                                                                                        Contract.IsActive,
                                                                                        Contract.ORGID,
                                                                                        Contract.Currency,
                                                                                        Contract.AwardAmount AwardAmount,
                                                                                        Contract.POPStart POPStart,
                                                                                        Contract.POPEnd POPEnd,
                                                                                        Contract.UpdatedOn,
                                                                                        Contract.ContractNumber,
                                                                                        Contract.ContractTitle,
                                                                                        Contract.FundingAmount FundingAmount
                                                                                        from Contract
                                                                                        where Contract.IsDeleted = 0
                                                                                        { where } 
                                                                                        and contract.ParentContractGuid= @ContractGuid
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > @skip 
                                            AND RowNum <= @rowNum  
                                        ORDER BY RowNum");
            //sqlQuery += ""
            var pagedData = _context.Connection.Query<Contracts>(sqlQuery, new { searchValue = searchString, ContractGuid = contractGuid, orderingQuery = orderingQuery.ToString(), skip = skip, rowNum = rowNum });
            return pagedData;
        }


        public Guid GetContractGuidByProjectNumber(string projectNumber)
        {
            var sql = @"SELECT TOP(1) ContractGuid
                        FROM Contract
                        WHERE ProjectNumber = @projectNumber";
            var contractGuid = _context.Connection.QueryFirstOrDefault<Guid>(sql, new { projectNumber = projectNumber });
            return contractGuid;
        }

        public Contracts GetContractByContractNumber(string contractNumber)
        {
            var sql = @"SELECT * 
                        FROM Contract
                        WHERE ContractNumber = @contractNumber";
            return _context.Connection.QueryFirstOrDefault<Contracts>(sql, new { contractNumber = contractNumber });
        }

        public Guid GetContractByContractNumberProjectNumberAndTitle(string contractNumber, string projectNumber, string title)
        {
            var sql = @"SELECT ContractGuid
                        FROM Contract
                        WHERE ContractNumber = @contractNumber AND ProjectNumber = @projectNumber AND ContractTitle = contractTitle";
            return _context.Connection.QueryFirstOrDefault<Guid>(sql, new { contractNumber = contractNumber, projectNumber = projectNumber, contractTitle = title });
        }

        public Guid GetParentContractByContractNumber(string contractNumber)
        {
            var sql = @"SELECT ContractGuid 
                        FROM Contract
                        WHERE ParentContractGuid IS NULL
                        AND IsIDIQContract = 1
                        AND ContractNumber = @contractNumber";
            return _context.Connection.QueryFirstOrDefault<Guid>(sql, new { contractNumber = contractNumber });
        }

        public bool InsertContract(Contracts contract)
        {
            string insertQuery = $@"INSERT INTO [dbo].[Contract]
                                                                   (
                                                                        ContractGuid					                            ,
                                                                        ParentContractGuid					                        ,
                                                                        IsIDIQContract						                        ,
                                                                        IsPrimeContract						                        ,
                                                                        ContractNumber				                                ,
                                                                        SubContractNumber			                                ,
                                                                        ORGID						                                ,
                                                                        ProjectNumber				                                ,
                                                                        ContractTitle				                                ,
                                                                        Description				                                    ,
                                                                        CountryOfPerformance			                            ,
                                                                        PlaceOfPerformance			                                ,
                                                                        POPStart						                            ,
                                                                        POPEnd						                                ,
                                                                        NAICSCode					                                ,
                                                                        PSCCode						                                ,
                                                                        CPAREligible					                            ,
                                                                        QualityLevelRequirements		                            ,
                                                                        QualityLevel					                            ,
                                                                                                                                    
                                                                        AwardingAgencyOffice				                        ,
                                                                        OfficeContractRepresentative		                        ,
                                                                        OfficeContractTechnicalRepresent                           ,
                                                                        FundingAgencyOffice				                            ,
                                                                        FundingOfficeContractRepresentative				        ,
                                                                        FundingOfficeContractTechnicalRepresent				    ,
                                                                                                                                    
                                                                        SetAside						                            ,
                                                                        SelfPerformancePercent			                            ,
                                                                        SBA								                            ,
                                                                        Competition						                            ,
                                                                        ContractType					                            ,
                                                                        OverHead						                            ,
                                                                        GAPercent						                            ,
                                                                        FeePercent						                            ,
                                                                        Currency						                            ,
                                                                        BlueSkyAwardAmount				                            ,
                                                                        AwardAmount						                            ,
                                                                        FundingAmount					                            ,
                                                                        BillingAddress					                            ,
                                                                        BillingFrequency				                            ,
                                                                        InvoiceSubmissionMethod			                            ,
                                                                        PaymentTerms					                            ,
                                                                        ApplicableWageDetermination				                ,
                                                                        BillingFormula					                            ,
                                                                        RevenueFormula					                            ,
                                                                        RevenueRecognitionEACPercent	                            ,
                                                                        OHonsite						                            ,
                                                                        OHoffsite						                            ,
                                                                        ProjectCounter						                        ,
                                                                        CreatedOn						                            ,
                                                                        UpdatedOn						                            ,
                                                                        CreatedBy						                            ,
                                                                        UpdatedBy						                            ,
                                                                        IsActive						                            ,
                                                                        Status						                                ,
                                                                        AddressLine1						                        ,
                                                                        AddressLine2						                        ,
                                                                        AddressLine3						                        ,
                                                                        City						                                ,
                                                                        Province						                            ,
                                                                        County						                                ,
                                                                        PostalCode						                            ,
                                                                        FarContractTypeGuid						                    ,

                                                                        IsDeleted,
                                                                        IsImported,
                                                                        MasterTaskNodeID,
                                                                        TaskNodeID
                                                                    )
                                  VALUES (
                                                                        @ContractGuid                                       ,
                                                                        @ParentContractGuid                                ,
                                                                        @IsIDIQContract                                     ,
                                                                        @IsPrimeContract                                    ,
                                                                        @ContractNumber                                     ,
                                                                        @SubContractNumber                                  ,
                                                                        @ORGID                                              ,
                                                                        @ProjectNumber                                      ,
                                                                        @ContractTitle                                      ,
                                                                        @Description                                        ,
                                                                        @CountryOfPerformance                               ,
                                                                        @PlaceOfPerformanceSelectedIds                      ,
                                                                        @POPStart                                           ,
                                                                        @POPEnd                                             ,
                                                                        @NAICSCode                                          ,
                                                                        @PSCCode                                            ,
                                                                        @CPAREligible                                       ,
                                                                        @QualityLevelRequirements                           ,
                                                                        @QualityLevel                                       ,
                                                                                                                                                        
                                                                        @AwardingAgencyOffice                             ,	
                                                                        @OfficeContractRepresentative                    ,	
                                                                        @OfficeContractTechnicalRepresent                ,
                                                                        @FundingAgencyOffice                              ,
                                                                        @FundingOfficeContractRepresentative             ,
                                                                        @FundingOfficeContractTechnicalRepresent         ,
                                                                        
                                                                        @setAside                                         ,
                                                                        @SelfPerformancePercent                          ,
                                                                        @SBA                                              ,
                                                                        @Competition                                      ,
                                                                        @ContractType                                     ,
                                                                        @OverHead                                         ,
                                                                        @GAPercent                                        ,
                                                                        @FeePercent                                       ,
                                                                        @Currency                                         ,
                                                                        @BlueSkyAwardAmount                               ,
                                                                        @AwardAmount                                      ,
                                                                        @FundingAmount                                    ,
                                                                        @BillingAddress                                   ,
                                                                        @BillingFrequency                                 ,
                                                                        @InvoiceSubmissionMethod                          ,
                                                                        @PaymentTerms                                     ,
                                                                        @ApplicableWageDetermination                      ,
                                                                        @BillingFormula                                   ,
                                                                        @RevenueFormula                                   ,
                                                                        @RevenueRecognitionEACPercent                     ,
                                                                        @OHonsite                                         ,
                                                                        @OHoffsite                                        ,
                                                                        @ProjectCounter                                   ,
                                                                        @CreatedOn                                        ,
                                                                        @UpdatedOn                                        ,
                                                                        @CreatedBy                                        ,
                                                                        @UpdatedBy                                        ,
                                                                        @IsActive                                         ,
                                                                        @Status                                           ,
                                                                        @AddressLine1						              ,
                                                                        @AddressLine2						              ,
                                                                        @AddressLine3						              ,
                                                                        @City						                      ,
                                                                        @Province						                  ,
                                                                        @County						                      ,
                                                                        @PostalCode						                  ,
                                                                        @FarContractTypeGuid						      ,
                                                                        @IsDeleted,
                                                                        @IsImported,
                                                                        @MasterTaskNodeID,
                                                                        @TaskNodeID
                                                                )";
            _context.Connection.Execute(insertQuery, contract);
            return true;
        }

        public Contracts GetContractByProjectNumber(string projectNumber)
        {
            var sql = @"SELECT * 
                        FROM Contract
                        WHERE ProjectNumber = @projectNumber";
            return _context.Connection.QueryFirstOrDefault<Contracts>(sql, new { projectNumber = projectNumber });
        }

        public Contracts GetContractByTaskNodeID(int taskNodeID)
        {
            var sql = @"SELECT * 
                        FROM Contract
                        WHERE TaskNodeID = @taskNodeID";
            return _context.Connection.QueryFirstOrDefault<Contracts>(sql, new { taskNodeID = taskNodeID });
        }

        public Guid GetParentContractByMasterTaskNodeID(int masterTaskNodeID)
        {
            var sql = @"SELECT ContractGuid 
                        FROM Contract
                        WHERE ParentContractGuid IS NULL
                        AND IsIDIQContract = 1
                        AND TaskNodeID = @masterTaskNodeID";
            return _context.Connection.QueryFirstOrDefault<Guid>(sql, new { masterTaskNodeID = masterTaskNodeID });
        }

        public bool CheckTaskOdrderByContractGuid(Guid parentContractGuid)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM Contract
                        WHERE ParentContractGuid = @parentContractGuid";
            int taskOrderCount = _context.Connection.QueryFirstOrDefault<int>(sql, new { parentContractGuid = parentContractGuid });
            if (taskOrderCount > 0)
                return true;
            return false;
        }

        public IEnumerable<string> GetTaskOrderByParentContractGuid(Guid parentContractGuid)
        {
            var sql = @"SELECT * 
                        FROM Contract
                        WHERE ParentContractGuid = @parentContractGuid";
            return _context.Connection.Query<string>(sql, new { parentContractGuid = parentContractGuid });
        }

        public bool IsExistContractNumber(string contractNumber, Guid contractGuid)
        {
            string contactNumberQuery = $@"select contractNumber from  Contract 
											  where  IsDeleted   = 0
												 AND contractNumber = @contractNumber 
												 AND ContractGuid <> @contractGuid ";
            var result = _context.Connection.QueryFirstOrDefault<string>(contactNumberQuery, new { contractNumber = contractNumber, contractGuid = contractGuid });

            return !string.IsNullOrEmpty(result) ? true : false;
        }

        public bool IsExistProjectNumber(string projectNumber, Guid contractGuid)
        {
            string projectNumberQuery = $@"select projectNumber from  Contract 
											  where  IsDeleted   = 0
												 AND projectNumber = @projectNumber 
												 AND ContractGuid <> @contractGuid";
            var result = _context.Connection.QueryFirstOrDefault<string>(projectNumberQuery, new { projectNumber = projectNumber, contractGuid = contractGuid });

            return !string.IsNullOrEmpty(result) ? true : false;
        }
        #endregion End of Contract region

        #region Contract User Role
        /// <summary>
        /// For inserting the contract user base on role 
        /// </summary>
        /// <param name="contractUser"></param>
        /// <returns></returns>
        public bool InsertContractUsersList(List<ContractUserRole> contractUser)
        {
            List<ContractUserRole> isUserExistList = new List<ContractUserRole>();
            var contractUserSql = string.Format($@"select * 
                                                                            from ContractUserRole
                                                                            where 
                                                                            ContractGuid = @ContractGuid and
                                                                            UserRole = @UserRole");


            var sqlQuery = @"INSERT INTO [dbo].[ContractUserRole]
                                 (
                                    ContractGuid,
                                    UserGuid,
                                    UserRole
                                 )
                                 VALUES 
                                (
                                    @ContractGuid,
                                    @UserGuid,
                                    @UserRole
                                )";
            foreach (var user in contractUser)
            {
                var getContractUser = _context.Connection.QueryFirstOrDefault<ContractUserRole>(contractUserSql,
                    new
                    {
                        ContractGuid = user.ContractGuid,
                        UserRole = user.UserRole
                    });
                if (getContractUser == null)
                {
                    if (user.ContractGuid != Guid.Empty && user.UserGuid != Guid.Empty)
                        _context.Connection.Query(sqlQuery, user);
                }
                else
                {
                    isUserExistList.Add(user);
                }
            }
            if (isUserExistList.Count() > 0)
                UpdateContractUsersList(isUserExistList);
            return true;
        }
        /// <summary>
        /// For updating contract user based on role
        /// </summary>
        /// <param name="contractUser"></param>
        /// <returns></returns>
        public bool UpdateContractUsersList(List<ContractUserRole> contractUser)
        {
            List<ContractUserRole> newContractUserRole = new List<ContractUserRole>();

            Guid contractGuid = contractUser[0].ContractGuid;
            var contractUserRoleList = contractUser.Where(x => (x.UserGuid != null || x.UserGuid != Guid.Empty));
            if (contractUserRoleList != null)
            {
                foreach (var data in contractUserRoleList)
                {
                    string sql = "SELECT * FROM ContractUserRole WHERE ContractGuid = @ContractGuid and UserRole=@UserRole;";
                    var result = _context.Connection.Query<ContractUserRole>(sql, new { ContractGuid = data.ContractGuid, UserRole = data.UserRole });
                    if (result.Count() > 0)
                    {
                        var updateQuery = @"UPDATE [dbo].[ContractUserRole]
                                SET UserGuid = @UserGuid
                                WHERE ContractGuid = @ContractGuid
                                AND UserRole = @UserRole";
                        if (data.UserGuid != Guid.Empty && data.ContractGuid != Guid.Empty)
                            _context.Connection.Query(updateQuery, data);
                    }
                    else
                    {
                        InsertContractUser(data);
                    }
                }
            }
            var contractUserRoleRemoveList = contractUser.Where(x => (x.UserGuid == null || x.UserGuid == Guid.Empty) && x.ContractGuid != Guid.Empty && x.UserRole != null);
            if (contractUserRoleRemoveList != null)
            {
                foreach (var data in contractUserRoleRemoveList)
                {
                    var deleteQuery = @"Delete from contractUserRole
                                WHERE ContractGuid = @ContractGuid
                                AND UserRole = @UserRole";
                    _context.Connection.Query(deleteQuery, data);
                }
            }
            return true;
        }

        public bool InsertContractUser(ContractUserRole contractUser)
        {
            if (contractUser.ContractGuid == Guid.Empty || contractUser.UserGuid == Guid.Empty)
                return false;
            var sqlQuery = @"INSERT INTO [dbo].[ContractUserRole]
                                 (
                                    ContractGuid,
                                    UserGuid,
                                    UserRole
                                 )
                                 VALUES 
                                (
                                    @ContractGuid,
                                    @UserGuid,
                                    @UserRole
                                )";
            _context.Connection.Query(sqlQuery, contractUser);
            return true;
        }

        public bool UpdateContractUserByGuid(ContractUserRole contractUser)
        {
            if (contractUser.ContractGuid == Guid.Empty || contractUser.UserGuid == Guid.Empty)
                return false;
            var updateQuery = @"UPDATE [dbo].[ContractUserRole]
                                SET UserGuid = @UserGuid,
                                WHERE ContractGuid = @ContractGuid
                                AND UserRole = @UserRole";
            _context.Connection.Query(updateQuery, contractUser);
            return true;
        }

        public bool UpdateProjectNumberByGuid(Guid contractGuid, string projectNumber)
        {
            var updateQuery = @"UPDATE [dbo].[Contract]
                                SET
                                ProjectNumber = @projectNumber
                                WHERE ContractGuid = @contractGuid";
            _context.Connection.Execute(updateQuery, new { contractGuid = contractGuid, projectNumber = projectNumber });
            return true;
        }

        public IEnumerable<ContractUserRole> GetKeyPersonnelByContractGuid(Guid contractGuid)
        {
            var sqlQuery = @"
							SELECT * FROM ContractUserRole c
							LEFT JOIN Users u
							ON c.UserGuid = u.UserGuid
							WHERE c.ContractGuid = @contractGuid";
            var userDictionary = new Dictionary<Guid, ContractUserRole>();
            var keyPersonnelList = _context.Connection.Query<ContractUserRole, User, ContractUserRole>(sqlQuery,
                (userRole, user) =>
                {
                    ContractUserRole keyPerson;
                    if (!userDictionary.TryGetValue(userRole.ContractUserRoleGuid, out keyPerson))
                    {
                        keyPerson = userRole;
                        keyPerson.User = new User();
                        userDictionary.Add(userRole.ContractUserRoleGuid, keyPerson = userRole);
                    }

                    if (user != null)
                        keyPerson.User = user;
                    return keyPerson;
                },
                new { contractGuid = contractGuid },
                    splitOn: "UserGuid");
            return keyPersonnelList;
        }

        #endregion

        #region Start of contract Modification
        public IEnumerable<ContractModification> GetAllContractMod(Guid contractGuid, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();

            if (sortField.Equals("isActiveStatus"))
            {
                orderingQuery.Append($"ContractModification.isActive {sortDirection}");  //Ambiguous if not done.. 
            }
            else
            {
                orderingQuery.Append($"{sortField} {sortDirection}");
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                conditionalQuery.Append($"and ContractModification.ContractReference like '%{searchValue}%'");
            }

            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                       (SELECT ROW_NUMBER() OVER (ORDER BY {orderingQuery}) AS RowNum, 
                                                                                       ContractModification.ContractModificationGuid					                ,
                                                                                       ContractModification.ContractGuid					                            ,
                                                                                       ContractModification.ModificationNumber						                ,
                                                                                       ContractModification.ModificationTitle						                ,
                                                                                       ContractModification.IsAwardAmount						                ,
                                                                                       ContractModification.IsFundingAmount						                ,
                                                                                       ContractModification.IsPop						                ,
                                                                                       ContractModification.FundingAmount						                ,
                                                                                       ContractModification.EnteredDate						                        ,
                                                                                       ContractModification.EffectiveDate				                            ,
                                                                                       ContractModification.AwardAmount			                            ,
                                                                                       ContractModification.POPStart						                            ,
                                                                                       ContractModification.POPEnd						                            ,
                                                                                       ContractModification.Description						                            ,
                                                                                       ContractModification.UpdatedOn						                        ,
                                                                                       ContractModification.IsActive						                            ,
                                                                                       Contract.ContractNumber                           						        ,
                                                                                       Contract.ProjectNumber                           						        ,
                                                                                       Contract.ContractTitle                                                           ,
                                                                                       ContractModification.CreatedOn                                                   
                                                                                        
                                                                                       FROM ContractModification
                                                                                       LEFT JOIN Contract
                                                                                       ON ContractModification.ContractGuid = Contract.ContractGuid
                                                                                       WHERE ContractModification.IsDeleted = 0
                                                                                        {conditionalQuery}
                                                                                       AND   Contract.ContractGuid = '{contractGuid}'
                                                                                       
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > {skip} 
                                            AND RowNum <= {pageSize + skip}  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<ContractModification>(pagingQuery);
            return pagedData;
        }
        public int TotalRecord(Guid contractGuid)
        {
            string sql = $@"SELECT Count(1) 
                            from ContractModification
                            left join Contract
                            on ContractModification.ContractGuid = Contract.ContractGuid
                            where ContractModification.IsDeleted = 0
                            and   Contract.ContractGuid = '{contractGuid}'";
            var result = _context.Connection.QueryFirstOrDefault<int>(sql);
            return result;
        }
        public int InsertMod(ContractModification contractModificationModel)
        {
            string insertQuery = @"INSERT INTO [dbo].[ContractModification]
                                                                   (
                                                                    ContractModificationGuid						                    ,
                                                                    ContractGuid						                                ,
                                                                    ModificationNumber							                    ,
                                                                    AwardAmount				                                        ,
                                                                    EnteredDate				                                    ,
                                                                    EffectiveDate				                                        ,
                                                                    POPStart											            ,
                                                                    POPEnd						                                    ,     
                                                                    Description						                                    ,
                                                                    UploadedFileName						                            ,
                                                                    ModificationTitle						                            ,
                                                                    IsAwardAmount						                            ,
                                                                    IsFundingAmount						                            ,
                                                                    IsPop						                            ,
                                                                    FundingAmount						                            ,
                                                                    
                                                                    CreatedOn						                            ,
                                                                    UpdatedOn						                            ,
                                                                    CreatedBy						                            ,
                                                                    UpdatedBy						                            ,
                                                                    IsActive						                            ,
                                                                    IsDeleted                                                   
                                                                    )
                                  VALUES (
                                                                    @ContractModificationGuid						                    ,
                                                                    @ContractGuid						                                ,
                                                                    @ModificationNumber							                    ,
                                                                    @AwardAmount				                            ,
                                                                    @EnteredDate				                            ,
                                                                    @EffectiveDate				                            ,
                                                                    @POPStart											            ,
                                                                    @POPEnd						                                    ,     
                                                                    @Description						                                    ,
                                                                    @UploadedFileName						                            ,
                                                                    @ModificationTitle						                            ,
                                                                    @IsAwardAmount						                            ,
                                                                    @IsFundingAmount						                            ,
                                                                    @IsPop						                            ,
                                                                    @FundingAmount						                            ,
                                                                    
                                                                    @CreatedOn						                            ,
                                                                    @UpdatedOn						                            ,
                                                                    @CreatedBy						                            ,
                                                                    @UpdatedBy						                            ,
                                                                    @IsActive						                            ,
                                                                    @IsDeleted                                                   
                                                                )";
            return _context.Connection.Execute(insertQuery, contractModificationModel);
        }
        public int UpdateMod(ContractModification contractModificationModel)
        {
            string updateQuery = @"Update ContractModification set 
                                                                    ContractGuid					 = @ContractGuid			    ,
                                                                    ModificationNumber			     = @ModificationNumber			,
                                                                    AwardAmount				         = @AwardAmount				    ,
                                                                    EnteredDate				         = @EnteredDate				    ,
                                                                    EffectiveDate				     = @EffectiveDate				,
                                                                    POPStart						 = @POPStart					,	
                                                                    POPEnd						     = @POPEnd						,
                                                                    Description					     = @Description					,
                                                                    UploadedFileName				 = @UploadedFileName			,	
                                                                    ModificationTitle				 = @ModificationTitle			,	
                                                                    IsAwardAmount				    = @IsAwardAmount			,	
                                                                    IsFundingAmount				    = @IsFundingAmount			,	
                                                                    IsPop				            = @IsPop			,	
                                                                    FundingAmount				    = @FundingAmount			,	
                                                                                                     
                                                                    UpdatedOn						 = @UpdatedOn					,	
                                                                    UpdatedBy						 = @UpdatedBy					,	
                                                                    IsActive						 = @IsActive					,	
                                                                    IsDeleted                        = @IsDeleted                   
                                                                    where ContractModificationGuid    = @ContractModificationGuid";
            return _context.Connection.Execute(updateQuery, contractModificationModel);

        }
        public int DeleteMod(Guid[] ids)
        {
            foreach (var contractModificationGuid in ids)
            {
                var contractModification = new
                {
                    ContractModificationGuid = contractModificationGuid
                };
                string disableQuery = @"Update ContractModification set 
                                               IsDeleted   = 1
                                               where ContractModificationGuid =@ContractModificationGuid ";
                _context.Connection.Execute(disableQuery, contractModification);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int DisableMod(Guid[] ids)
        {
            foreach (var contractModificationGuid in ids)
            {
                var contractModification = new
                {
                    ContractModificationGuid = contractModificationGuid
                };
                string disableQuery = @"Update ContractModification set 
                                            IsActive   = 0
                                            where ContractModificationGuid =@ContractModificationGuid ";
                _context.Connection.Execute(disableQuery, contractModification);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int EnableMod(Guid[] ids)
        {
            foreach (var contractModificationGuid in ids)
            {
                var contractModification = new
                {
                    ContractModificationGuid = contractModificationGuid
                };
                string disableQuery = @"Update ContractModification set 
                                            IsActive   = 1
                                            where ContractModificationGuid =@ContractModificationGuid ";
                _context.Connection.Execute(disableQuery, contractModification);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public ContractModification GetModDetailById(Guid id)
        {
            string sql = @"select distinct 
                            ContractModification.ContractModificationGuid		                                ,
                            ContractModification.ContractGuid									                ,
                            ContractModification.ModificationNumber				                                ,
                            ContractModification.ModificationTitle				                                ,
                            ContractModification.IsAwardAmount				                                ,
                            ContractModification.IsFundingAmount				                                ,
                            ContractModification.IsPop				                                ,
                            ContractModification.FundingAmount				                                ,
                            ContractModification.EnteredDate						                            ,
                            ContractModification.EffectiveDate				    				                ,
                            ContractModification.AwardAmount			                                        ,
                            ContractModification.POPStart							                            ,
                            ContractModification.POPEnd											                ,
                            ContractModification.Description						                            ,
                            ContractModification.UploadedFileName						                            ,
							ContractModification.IsActive                                                       ,
							ContractModification.UpdatedOn                                                      ,
                            Contract.ContractNumber                                                             ,
                            Contract.ProjectNumber                                                              ,
                            Contract.ContractTitle
                            from ContractModification
                            left join Contract
                            on ContractModification.ContractGuid = Contract.ContractGuid
                            WHERE ContractModificationGuid =  @ContractModificationGuid;";
            var contractModificationModel = _context.Connection.QueryFirstOrDefault<ContractModification>(sql, new { ContractModificationGuid = id });
            return contractModificationModel;
        }
        public Contracts GetDetailsForProjectByContractId(Guid id)
        {
            var contractDetail = new Contracts();

            var sqlQuery = @"SELECT * FROM Contract c 
                        LEFT JOIN ContractUserRole cu 
                        ON c.ContractGuid = cu.ContractGuid
                        LEFT JOIN Users u
                        ON u.UserGuid = cu.UserGuid
                        LEFT JOIN ContractResourceFile cf
                        ON cf.ResourceGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        WHERE c.ContractGuid = @contractGuid";
            var contractDictionary = new Dictionary<Guid, Contracts>();

            contractDetail = _context.Connection.Query<Contracts, ContractKeyPersonnel, ContractResourceFile, Organization, Contracts>(
                   sqlQuery,
                   (contract, keyPerson, files, organisation) =>
                   {
                       Contracts contractEntity;

                       contractEntity = contract;

                       if (!contractDictionary.TryGetValue(contract.ContractGuid, out contractEntity))
                       {
                           contractDictionary.Add(contract.ContractGuid, contractEntity = contract);
                       }

                       if (contractEntity.KeyPersonnel == null)
                       {
                           contractEntity.KeyPersonnel = new List<ContractKeyPersonnel>();
                       }

                       if (keyPerson != null && !contractEntity.KeyPersonnel.Any(x => x.UserGuid == keyPerson.UserGuid))
                       {
                           contractEntity.KeyPersonnel.Add(keyPerson);
                       }

                       if (contractEntity.ContractResourceFile == null)
                           contractEntity.ContractResourceFile = new List<ContractResourceFile>();
                       if (files != null && !contractEntity.ContractResourceFile.Any(x => x.Keys == files.Keys))
                           contractEntity.ContractResourceFile.Add(files);
                       if (organisation != null)
                           contract.Organisation = organisation;

                       return contractEntity;
                   },
                   new { contractGuid = id },
                   splitOn: "ContractUserRoleGuid,ContractResourceFileGuid,OrgIDGuid")
               .FirstOrDefault();


            string customerInfoSql = @"select
                            AwardingOffice.CustomerGuid AwardingAgencyOffice,
							AwardingOffice.CustomerName AwardingAgencyOfficeName,
							OfficeContractRepresentative.ContactGuid OfficeContractRepresentative,
							(OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) OfficeContractRepresentativeName,
							OfficeContractTechnicalRepresent.ContactGuid OfficeContractTechnicalRepresent,
							(OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) OfficeContractTechnicalRepresentName,

							FundingOffice.CustomerGuid FundingAgencyOffice,
							FundingOffice.CustomerName FundingAgencyOfficeName,
                            FundingOfficeContractRepresentative.ContactGuid FundingOfficeContractRepresentative,
                            (FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingOfficeContractRepresentativeName,
							FundingOfficeContractTechnicalRepresent.ContactGuid FundingOfficeContractTechnicalRepresent,
							(FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingOfficeContractTechnicalRepresentName
                            
                            from Contract

                            left join
						    Customer AwardingOffice on Contract.AwardingAgencyOffice = AwardingOffice.CustomerGuid
						    left join
                            CustomerContact OfficeContractRepresentative on Contract.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
						    left join
						    CustomerContact OfficeContractTechnicalRepresent on  Contract.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid

                            left join
						    Customer FundingOffice on Contract.FundingAgencyOffice = FundingOffice.CustomerGuid
						    left join
						    CustomerContact FundingOfficeContractRepresentative on Contract.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
						    left join
						    CustomerContact FundingOfficeContractTechnicalRepresent on Contract.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid

                            WHERE ContractGuid =  @ContractGuid;";
            var customerInformation = _context.Connection.QueryFirstOrDefault<CustomerInformationModel>(customerInfoSql, new { ContractGuid = id });

            string financialInfoSql = @"select
							SetAside.Name setAsideName,
							SetAside.Value setAside,
							Contract.SelfPerformancePercent,
							Contract.SBA,
							Competition.Name CompetitionType,
                            Contract.Competition,
							ContractType.Name ContractTypeName,
                            Contract.ContractType,
							Contract.OverHead,
							Contract.GAPercent,
							Contract.FeePercent,
							Currency.Name CurrencyName,
							Contract.Currency,
							Contract.BlueSkyAwardAmount,
							Contract.AwardAmount,
							Contract.FundingAmount,
							Contract.BillingAddress,
							BillingFrequency.Name BillingFrequencyName,
							Contract.BillingFrequency,
							InvoiceSubmissionMethod.Name InvoiceSubmissionMethodName,
							Contract.InvoiceSubmissionMethod,
							PaymentTerms.Name PaymentTermsName,
							Contract.PaymentTerms,
							Contract.ApplicableWageDetermination,
							Contract.BillingFormula,
							Contract.RevenueFormula,
							Contract.RevenueRecognitionEACPercent,
							Contract.OHonsite,
							Contract.OHoffsite
                            from Contract
						    left join
						    ResourceAttributeValue SetAside on Contract.SetAside = SetAside.Value
						    left join
						    ResourceAttributeValue Competition on Contract.Competition = Competition.Value
						    left join
						    ResourceAttributeValue ContractType on Contract.ContractType = ContractType.Value
						    left join
						    ResourceAttributeValue Currency on Contract.Currency = Currency.Value
						    left join
						    ResourceAttributeValue BillingFrequency on Contract.BillingFrequency = BillingFrequency.Value
						    left join
						    ResourceAttributeValue InvoiceSubmissionMethod on Contract.InvoiceSubmissionMethod = InvoiceSubmissionMethod.Value
						    left join
						    ResourceAttributeValue PaymentTerms on Contract.PaymentTerms = PaymentTerms.Value
                            WHERE ContractGuid =  @ContractGuid;";
            var financialInformation = _context.Connection.QueryFirstOrDefault<FinancialInformationModel>(financialInfoSql, new { ContractGuid = id });


            //for splitting appwage davis bacon and appwage service contract
            var daviesAct = GetDavisActByApplicableWageDetermination(financialInformation.ApplicableWageDetermination);
            //for splitting appwage davis bacon and appwage service contract
            financialInformation.AppWageDetermineDavisBaconActType = daviesAct.AppWageDetermineDavisBaconActType;
            financialInformation.AppWageDetermineDavisBaconAct = daviesAct.AppWageDetermineDavisBaconAct;
            financialInformation.AppWageDetermineServiceContractActType = daviesAct.AppWageDetermineServiceContractActType;
            financialInformation.AppWageDetermineServiceContractAct = daviesAct.AppWageDetermineServiceContractAct;

            contractDetail.CustomerInformation = customerInformation;
            contractDetail.ContractGuid = id;
            contractDetail.FinancialInformation = financialInformation;
            //contractDetail.IsActive = new BaseModel().IsActive;
            //contractDetail.CreatedOn = new BaseModel().CreatedOn;

            return contractDetail;
        }

        public bool IsExistModificationNumber(Guid contractGuid, string modificationNumber)
        {
            string modificationNumberQuery = $@"select modificationNumber from  ContractModification  
                                              where  IsDeleted   = 0
                                                 and contractGuid = @contractGuid
                                                 and modificationNumber = @modificationNumber ";
            var result = _context.Connection.QueryFirstOrDefault<string>(modificationNumberQuery, new { contractGuid = contractGuid, modificationNumber = modificationNumber });

            return !string.IsNullOrEmpty(result) ? true : false;
        }


        public IEnumerable<ContractUserRole> GetKeyPersonnels(string keyPersonnelType)
        {
            var sqlQuery = @"
							SELECT * FROM ContractUserRole c
							LEFT JOIN Users u
							ON c.UserGuid = u.UserGuid
							WHERE c.UserRole = @UserRole Order By u.firstName";
            var userDictionary = new Dictionary<Guid, ContractUserRole>();
            var keyPersonnelList = _context.Connection.Query<ContractUserRole, User, ContractUserRole>(sqlQuery,
                (userRole, user) =>
                {
                    ContractUserRole keyPerson;
                    if (!userDictionary.TryGetValue(userRole.ContractUserRoleGuid, out keyPerson))
                    {
                        keyPerson = userRole;
                        keyPerson.User = new User();
                        userDictionary.Add(userRole.ContractUserRoleGuid, keyPerson = userRole);
                    }

                    if (user != null)
                        keyPerson.User = user;
                    return keyPerson;
                },
                new { UserRole = keyPersonnelType },
                    splitOn: "UserGuid");
            return keyPersonnelList;
        }

        public bool UpdateAllUserByRole(Guid userGuid, string userRole)
        {
            string query = @"UPDATE ContractUserRole 
                            SET UserGuid = @userGuid 
                            WHERE UserRole = @userRole";
            var affectedRows = _context.Connection.Execute(query, new { userGuid = userGuid, userRole = userRole});
            return true;
        }
        #endregion End of Contract Modification

        #region Contract file repo
        public IEnumerable<ContractResourceFile> GetFileListByContractGuid(Guid contractGuid)
        {
            var sql = @"SELECT * 
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ResourceGuid] = @contractGuid
                    ORDER BY UploadFileName asc";
            var fileList = _context.Connection.Query<ContractResourceFile>(sql, new { contractGuid = contractGuid });
            return fileList;
        }

        public IEnumerable<ContractResourceFile> GetFileListByContentResourceGuid(Guid contractGuid)
        {
            var sql = @"SELECT * 
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ContentResourceGuid] = @contractGuid
                    AND IsDeleted = 0
                    ORDER BY UploadFileName asc";
            var fileList = _context.Connection.Query<ContractResourceFile>(sql, new { contractGuid = contractGuid });
            return fileList;
        }

        public ContractResourceFile GetFilesByContractFileGuid(Guid contractFileGuid)
        {
            var sql = @"SELECT TOP(1) *
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ContractResourceFileGuid] = @contractFileGuid";
            var contractFile = _context.Connection.QueryFirstOrDefault<ContractResourceFile>(sql, new { contractFileGuid = contractFileGuid });
            return contractFile;
        }

        public IEnumerable<ContractResourceFile> GetFilesByContractResourceFileGuid(Guid contractFileGuid)
        {
            var sql = @"SELECT  *
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ResourceGuid] = @contractFileGuid and ([isfile] is null or [isfile] = 0)";
            var contractFile = _context.Connection.Query<ContractResourceFile>(sql, new { contractFileGuid = contractFileGuid });
            return contractFile;
        }

        public IEnumerable<ContractResourceFile> GetFilesAndFolders(Guid contractFileGuid, string column)
        {
            var sql = @"SELECT  *
                    FROM [dbo].[ContractResourceFile]
                    WHERE " + column + " = @contractFileGuid and ([isfile] is null or [isfile] = 0) and isdeleted = 0";
            var contractFile = _context.Connection.Query<ContractResourceFile>(sql, new { contractFileGuid = contractFileGuid });
            return contractFile;
        }

        public IEnumerable<ContractResourceFile> GetFilesAndFoldersByParentId(Guid parentId)
        {
            var sql = @"SELECT  *
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ParentId] = @contractFileGuid";
            var contractFile = _context.Connection.Query<ContractResourceFile>(sql, new { contractFileGuid = parentId });

            return contractFile;
        }

        public ContractResourceFile GetFilesByContractFileResourceGuid(Guid contractFileGuid)
        {
            var sql = @"SELECT TOP(1) *
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ResourceGuid] = @contractFileGuid";
            var contractFile = _context.Connection.QueryFirstOrDefault<ContractResourceFile>(sql, new { contractFileGuid = contractFileGuid });
            return contractFile;
        }

        public ContractResourceFile GetFilesByContractGuid(Guid contractGuid, string formType)
        {
            var sql = @"SELECT TOP(1) * 
                        FROM ContractResourceFile
                        WHERE ResourceGuid = @contractGuid
                        AND Keys = @formType AND IsActive =1 AND IsDeleted=0";
            return _context.Connection.Query<ContractResourceFile>(sql, new { contractGuid = contractGuid, formType = formType }).FirstOrDefault();
        }

        public ContractResourceFile GetFileByResourceGuid(Guid resourceGuid, string formType)
        {
            var sql = @"SELECT TOP(1) * 
                        FROM ContractResourceFile
                        WHERE ResourceGuid = @ResourceGuid
                        AND Keys = @formType AND ISFILE =1 AND ISACTIVE =1";
            return _context.Connection.Query<ContractResourceFile>(sql, new { ResourceGuid = resourceGuid, formType = formType }).FirstOrDefault();
        }

        public bool CheckAndInsertContractFile(ContractResourceFile file)
        {
            var sql = @"SELECT TOP(1) *
                    FROM [dbo].[ContractResourceFile]
                    WHERE [ContentResourceGuid] = @ContentResourceGuid
                    AND [Keys] = @keys";
            var contractFile = _context.Connection.QueryFirstOrDefault<ContractResourceFile>
                (sql, new { ContentResourceGuid = file.ContentResourceGuid, Keys = file.Keys });
            if (contractFile != null)
            {
                var updateSql = @"UPDATE ContractResourceFile  SET
                        UploadFileName = @UploadFileName,
						FilePath = @FilePath,
						FileSize = @FileSize,
                        UpdatedBy = @UpdatedBy,
                        UpdatedOn = @UpdatedOn,
                        IsCsv = @IsCsv,
                        Keys = @Keys
                        WHERE  ContentResourceGuid = @ContentResourceGuid
                        AND [Keys] = @keys";
                _context.Connection.Execute(updateSql, file);
                return true;
            }
            else
            {
                var insertSql = @"INSERT INTO [dbo].[ContractResourceFile]
                        (  
							ResourceGuid,
							UploadFileName,
							UploadUniqueFileName,
							Keys,
							MimeType,
							IsActive,
							IsDeleted,
							CreatedBy,
							UpdatedBy,
							CreatedOn,
							UpdatedOn,
							IsCsv,
							Description,
							FilePath,
							FileSize,
                            IsFile,
                            IsReadOnly,
                            ParentId,
                            ResourceType,
                            ContentResourceGuid
                        )
                        VALUES
                        (
                            @ResourceGuid,
                            @UploadFileName,
                            @UploadUniqueFileName,
                            @Keys,
                            @MimeType,
                            @IsActive,
                            @IsDeleted,
                            @CreatedBy,
                            @UpdatedBy,
                            @CreatedOn,
                            @UpdatedOn,
                            @IsCsv,
                            @Description,
							@FilePath,
							@FileSize,
                            @IsFile,
                            @IsReadOnly,
                            @ParentId,
                            @ResourceType,
                            @ResourceGuid
                        );";
                _context.Connection.Execute(insertSql, file);
            }
            return true;
        }
        public bool InsertContractFile(ContractResourceFile file)
        {
            var insertSql = @"INSERT INTO [dbo].[ContractResourceFile]
                        (  
							ResourceGuid,
							UploadFileName,
							UploadUniqueFileName,
							Keys,
							MimeType,
							IsActive,
							IsDeleted,
							CreatedBy,
							UpdatedBy,
							CreatedOn,
							UpdatedOn,
							IsCsv,
							Description,
							FilePath,
							FileSize,
                            IsFile,
                            IsReadOnly,
                            ParentId,
                            ResourceType,
                            ContentResourceGuid
                        )
                        VALUES
                        (
                            @ResourceGuid,
                            @UploadFileName,
                            @UploadUniqueFileName,
                            @Keys,
                            @MimeType,
                            @IsActive,
                            @IsDeleted,
                            @CreatedBy,
                            @UpdatedBy,
                            @CreatedOn,
                            @UpdatedOn,
                            @IsCsv,
                            @Description,
							@FilePath,
							@FileSize,
                            @IsFile,
                            @IsReadOnly,
                            @ParentId,
                            @ResourceType,
                            @ContentResourceGuid
                        );";
            _context.Connection.Execute(insertSql, file);
            return true;
        }

        public Guid InsertContractFileAndGetIdBack(ContractResourceFile file)
        {
            var insertSql = @"INSERT INTO [dbo].[ContractResourceFile]
                        (
							ResourceGuid,
							UploadFileName,
							UploadUniqueFileName,
							Keys,
							MimeType,
							IsActive,
							IsDeleted,
							CreatedBy,
							UpdatedBy,
							CreatedOn,
							UpdatedOn,
							IsCsv,
							Description,
							FilePath,
							FileSize,
                            IsFile,
                            IsReadOnly,
                            ParentId 
                        ) 
                        VALUES
                        (
                            @ResourceGuid,
                            @UploadFileName,
                            @UploadUniqueFileName,
                            @Keys,
                            @MimeType,
                            @IsActive,
                            @IsDeleted,
                            @CreatedBy,
                            @UpdatedBy,
                            @CreatedOn,
                            @UpdatedOn,
                            @IsCsv,
                            @Description,
							@FilePath,
							@FileSize,
                            @IsFile,
                            @IsReadOnly,
                            @ParentId
                        );";
            var test = (Guid)_context.Connection.ExecuteScalar(insertSql, file);

            return test;

        }

        public bool UpdateContractFile(ContractResourceFile file)
        {
            var updateSql = @"UPDATE TOP(1) [dbo].[ContractResourceFile]
                        SET
                        ResourceGuid = @ResourceGuid,
                        UploadFileName = @UploadFileName,
                        Keys = @Keys,
                        MimeType = @MimeType,
                        UpdatedBy = @UpdatedBy,
                        UpdatedOn = @UpdatedOn,
                        IsCsv = @IsCsv,
                        FilePath = @FilePath,
                        FileSize = @FileSize
                        WHERE ResourceGuid = @ResourceGuid";
            _context.Connection.Execute(updateSql, file);
            return true;
        }

        public bool UpdateFile(ContractResourceFile file)
        {
            var updateSql = @"UPDATE ContractResourceFile  SET
                        ResourceGuid = @ResourceGuid,
                        UploadFileName = @UploadFileName,
                        Description =@Description,
						FilePath = @FilePath,
						FileSize = @FileSize,
                        Keys = @Keys,
                        MimeType = @MimeType,
                        UpdatedBy = @UpdatedBy,
                        UpdatedOn = @UpdatedOn,
                        IsCsv = @IsCsv
                        WHERE  ContractResourceFileGuid = @ContractResourceFileGuid";
            _context.Connection.Execute(updateSql, file);
            return true;
        }

        public bool DeleteContractFileById(Guid contractResourceFileGuid)
        {
            try
            {
                var deleteSql = @"DELETE TOP(1) 
                                FROM ContractResourceFile
                                WHERE ContractResourceFileGuid = @contractResourceFileGuid";
                _context.Connection.Execute(deleteSql, new { ContractResourceFileGuid = contractResourceFileGuid });
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DeleteContractFileByContractGuid(Guid contractGuid, string formType)
        {
            var deleteSql = @"DELETE TOP(1)
                            FROM ContractResourceFile
                            WHERE ResourceGuid = @contractGuid
                            AND Keys = @formType;";
            _context.Connection.Execute(deleteSql, new { contractGuid = contractGuid, formType = formType });
            return true;
        }

        public bool UpdateFileName(Guid contractFileGuid, string fileName)
        {
            var updateSql = @"UPDATE TOP(1) [dbo].[ContractResourceFile]
                            SET
                            UploadFileName = @fileName
                            WHERE ContractResourceFileGuid = @contractFileGuid";
            _context.Connection.Execute(updateSql, new { contractFileGuid = contractFileGuid, fileName = fileName });
            return true;
        }

        #endregion

        public ICollection<Organization> GetOrganizationData(string searchText)
        {
            if (searchText != "")
            {
                var organizationDataQuery = string.Format($@"
                select * from OrgID
                where Name like '%{@searchText}%' or Description like '%{@searchText}%' Order by Name ");
                var organizationData = _context.Connection.Query<Organization>(organizationDataQuery).ToList();
                return organizationData;
            }
            else
            {
                var organizationDataQuery = string.Format($@"
                select * from OrgID
                Order by Name ");
                var organizationData = _context.Connection.Query<Organization>(organizationDataQuery).ToList();
                return organizationData;
            }

        }

        public Organization GetOrganizationByOrgId(Guid orgId)
        {
            var organizationDataQuery = string.Format($@"
				SELECT * FROM OrgID where orgIdGuid = @orgId");
            var organizationData = _context.Connection.QueryFirstOrDefault<Organization>(organizationDataQuery, new { orgId = orgId });
            return organizationData;
        }

        public string GetContractType(Guid contratGuid)
        {
            string sqllQuery = @"select contractType 
                                    FROM Contract 
                                    WHERE ContractGuid = @contractGuid;";
            var contractType = _context.Connection.QueryFirstOrDefault<string>(sqllQuery, new { contractGuid = contratGuid });
            return contractType;
        }

        public bool InsertRevenueRecognitionGuid(Guid revenueRecognition, Guid contractGuid)
        {
            var updateSql = @"Update Contract set RevenueRecognitionGuid=@RevenueRecognition
                        WHERE ContractGuid = @ContractGuid ";
            _context.Connection.Execute(updateSql, new { RevenueRecognition = revenueRecognition, ContractGuid = contractGuid });
            return true;
        }

        public bool SetNullRevenueRecognitionGuid(Guid contractGuid, decimal? awardAmount, decimal? fundingAmount)
        {
            var updateQuery = @"Update Contract set RevenueRecognitionguid = null
                                 where contractGuid= @contractGuid";
            _context.Connection.Query(updateQuery, new { contractGuid = contractGuid, awardAmount = awardAmount });
            return true;
        }
        public bool UpdateRevenueRecognitionGuidInContract(Guid contractGuid, Guid revenueRecognitionGuid)
        {
            var updateQuery = @"Update Contract set RevenueRecognitionguid = @revenueRecognitionGuid
                                 where contractGuid= @contractGuid";
            _context.Connection.Query(updateQuery, new { contractGuid = contractGuid, revenueRecognitionGuid = revenueRecognitionGuid });
            return true;
        }
        public Contracts GetAmountById(Guid id)
        {
            string modificationNumberQuery = $@"select Awardamount,FundingAmount,RevenueRecognitionGuid from  Contract
                                                 where ContractGuid = @ContractGuid ";
            var result = _context.Connection.QueryFirstOrDefault<Contracts>(modificationNumberQuery, new { ContractGuid = id });
            return result;
        }

        public Contracts.ContractBasicDetails GetOnlyRequiredContractData(Guid id)
        {
            var sqlQuery = string.Format($@" select * from Contract
                where contractGuid = @ContractGuid");
            var result = _context.Connection.QueryFirstOrDefault<Contracts.ContractBasicDetails>(sqlQuery, new { ContractGuid = id });
            return result;
        }

        public bool DeleteContractFolder(Guid fileid)
        {
            var updateSql = @"Update ContractResourceFile set IsDeleted = 1 where ContractResourceFileGuid = @fileid or ParentId = @fileid";
            _context.Connection.Execute(updateSql, new { fileid = fileid });
            return true;
        }

        public bool RenameContractFolder(Guid fileid, string Foldername)
        {
            var updateSql = @"Update ContractResourceFile set UploadFilename = @Foldername,UploadUniqueFileName=@Foldername where ContractResourceFileGuid = @fileid";
            _context.Connection.Execute(updateSql, new { fileid = fileid, Foldername = Foldername });
            return true;
        }

        public List<string> GetContractRoleByUserGuid(Guid contractGuid, Guid userGuid)
        {
            string sqllQuery = @"select UserRole 
                                    FROM ContractUserRole 
                                    WHERE ContractGuid = @ContractGuid and UserGuid= @UserGuid";
            var contractType = _context.Connection.Query<string>(sqllQuery, new { ContractGuid = contractGuid, UserGuid = userGuid }).ToList();
            return contractType;
        }

        public bool CloseContractStatus(Guid contractGuid)
        {
            string closeContractQuery = $@"Update Contract 
                                        SET 
                                            IsActive   = 0 , 
                                            status = 'Closed'
                                        WHERE ContractGuid =@ContractGuid ";
            _context.Connection.Execute(closeContractQuery, new { ContractGuid = contractGuid });
            return true;
        }

        public QuestionaireUserAnswer GetAnswer(Guid contractGuid)
        {
            string getAnswer = $@"select * from QuestionaireUserAnswer
                                        WHERE ContractGuid =@ContractGuid ";
            return _context.Connection.QueryFirstOrDefault<QuestionaireUserAnswer>(getAnswer, new { ContractGuid = contractGuid });

        }


        public Guid? GetParentContractGuidByContractGuid(Guid contractGuid)
        {
            var sql = @"SELECT ParentContractGuid 
                        FROM Contract
                        WHERE ContractGuid = @ContractGuid";
            var parentGuid = _context.Connection.QuerySingle<Guid?>(sql, new { ContractGuid = contractGuid });

            return parentGuid;
        }

        public BasicContractInfoModel GetBasicContractById(Guid id)
        {
            var sql = @"SELECT *   
                        FROM Contract
                        WHERE ContractGuid = @ContractGuid";
            var contract = _context.Connection.QuerySingle<BasicContractInfoModel>(sql, new { ContractGuid = id });
            return contract;
        }

        public string GetProjectNumberById(Guid contractGuid)
        {
            string sql = @"SELECT TOP(1) ProjectNumber 
                            FROM Contract 
                            WHERE ContractGuid = @contractGuid;";
            var contractNumber = _context.Connection.QueryFirstOrDefault<string>(sql, new { contractGuid = contractGuid });
            return contractNumber;
        }

        public AssociateUserList GetCompanyRegionAndOfficeNameByCode(EntityCode entityCode)
        {
            AssociateUserList userList = new AssociateUserList();
            var companyPresidentQuery = string.Format($@"select top 1 Users.Displayname , Users.UserGuid , Users.JobTitle from Company left join Users on Company.President = Users.UserGuid where Company.CompanyCode = @CompanyCode");
            var operationManagerQuery = string.Format($@"select top 1 Users.Displayname , Users.UserGuid , Users.JobTitle from Office left join Users on Office.OperationManagerGuid = Users.UserGuid where Office.OfficeCode = @OfficeCode");
            var regionalManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._regionalManager}'");
            var deputyManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._deputyregionalManager}'");
            var HSManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._hsregionalManager}'");
            var BDManagerQuery = string.Format($@"select top 1 c.Displayname , c.UserGuid , c.JobTitle  from Region a join regionuserrolemapping b on a.regionGuid = b.RegionGuid join Users c on c.userGuid = b.UserGuid where a.RegionCode = @RegionCode and b.roletype= '{ContractUserRole._bdregionalManager}'");
            var companyNameQuery = string.Format($@"select top 1 Company.CompanyName from Company where CompanyCode = @CompanyCode and IsDeleted = 0");
            var regionNameQuery = string.Format($@"select top 1 Region.RegionName from Region where RegionCode = @RegionCode and IsDeleted = 0");
            var officeNameQuery = string.Format($@"select top 1 Office.OfficeName from Office where OfficeCode = @OfficeCode and IsDeleted = 0");

            //QueryFirst changed to QueryFirstOrDefault as data may not be available to match with column
            userList.CompanyPresident = _context.Connection.QueryFirstOrDefault<User>(companyPresidentQuery, new { CompanyCode = entityCode.CompanyCode });
            userList.OperationManager = _context.Connection.QueryFirstOrDefault<User>(operationManagerQuery, new { OfficeCode = entityCode.OfficeCode });
            userList.RegionManager = _context.Connection.QueryFirstOrDefault<User>(regionalManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.DeputyRegionManager = _context.Connection.QueryFirstOrDefault<User>(deputyManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.HealthAndSafetyRegionManager = _context.Connection.QueryFirstOrDefault<User>(HSManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.BusinessDevelopmentRegionManager = _context.Connection.QueryFirstOrDefault<User>(BDManagerQuery, new { RegionCode = entityCode.RegionCode });
            userList.CompanyName = _context.Connection.QueryFirstOrDefault<string>(companyNameQuery, new { CompanyCode = entityCode.CompanyCode }) ?? "";
            userList.RegionName = _context.Connection.QueryFirstOrDefault<string>(regionNameQuery, new { RegionCode = entityCode.RegionCode }) ?? "";
            userList.OfficeName = _context.Connection.QueryFirstOrDefault<string>(officeNameQuery, new { OfficeCode = entityCode.OfficeCode }) ?? "";
            return userList;
        }

        public string GetOrgNameById(Guid id)
        {
            string sql = @"SELECT Name  
                            FROM OrgId 
                            WHERE OrgIdGuid = @OrgIdGuid;";
            var name = _context.Connection.QueryFirstOrDefault<string>(sql, new { OrgIdGuid = id });
            return name;
        }

        #region organization
        #endregion

        #region Project For PFS
        public IEnumerable<Contracts> GetProjectListForPFS(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {

            var where = "";
            var searchString = "";
            var whereEntity = "";
            var cumulativeAwardAmountQuery = @"((SELECT COALESCE(SUM(awardamount), 0) FROM ContractModification cm1 WHERE cm1.ContractGuid = c.ContractGuid AND cm1.IsDeleted = 0) 
                                                + c.AwardAmount 
                                                + (SELECT COALESCE(SUM(task.awardamount), 0) FROM [Contract] task WHERE task.ParentContractGuid = c.ContractGuid) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid = c.ContractGuid)) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM[ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";

            var cumulativeFundingAmountQuery = @"((SELECT COALESCE(SUM(fundingAmount),0) FROM ContractModification cm1 WHERE cm1.ContractGuid=c.ContractGuid AND cm1.IsDeleted = 0 ) 
                                                + c.fundingAmount 
                                                + (SELECT COALESCE(SUM(task.fundingAmount),0) FROM [Contract] task WHERE task.ParentContractGuid=c.ContractGuid) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid FROM Contract cm2 WHERE cm2.ParentContractGuid=c.ContractGuid)) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";
            List<AdvancedSearchRequest> withoutEntity = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> withEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeName != "CompanyCode" && c.Attribute.AttributeName != "RegionCode").ToList();
            List<AdvancedSearchRequest> companyEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeName == "CompanyCode" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> regionEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeName == "RegionCode" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            AdvancedSearchRequest revenueRequired = postValue.Where(c => c.IsEntity == false && (c.Attribute.AttributeName == "IsRevenueRecRequired")).FirstOrDefault();
            AdvancedSearchRequest revenueCompleted = postValue.Where(c => c.IsEntity == false && (c.Attribute.AttributeName == "IsRevenueRecCompleted")).FirstOrDefault();
            List<AdvancedSearchRequest> keyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> withoutKeyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> companyList = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> regionList = new List<AdvancedSearchRequest>();


            foreach (var entity in withEntity)
            {
                dynamic value = entity.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    if (v.id == ContractUserRole._accountRepresentative || v.id == ContractUserRole._companyPresident || v.id == ContractUserRole._contractRepresentative ||
                    v.id == ContractUserRole._projectControls || v.id == ContractUserRole._projectManager || v.id == ContractUserRole._regionalManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }
                else
                {
                    if (value.id == ContractUserRole._accountRepresentative || value.id == ContractUserRole._companyPresident || value.id == ContractUserRole._contractRepresentative ||
                    value.id == ContractUserRole._projectControls || value.id == ContractUserRole._projectManager || value.id == ContractUserRole._regionalManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }

            }

            var queryBuilder = new AdvancedSearchQueryBuilder(withoutEntity);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }


            foreach (var item in withoutKeyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }
            var queryBuilderEntity = new AdvancedSearchQueryBuilder(withoutKeyPersonnels);
            var queryEntity = queryBuilderEntity.getQuery();
            var _builderEntity = new SqlBuilder();
            var selectorEntity = _builderEntity.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilderEntity.getQuery())
            {
                _builderEntity.Where(d.sql, d.value);
            }

            if (withoutKeyPersonnels.Count > 0)
            {
                _builder.AddParameters(selectorEntity.Parameters);
                whereEntity = selectorEntity.RawSql.Replace("WHERE", " AND ");
            }

            // Adding key personnel in query
            foreach (var item in keyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }

            var queryBuilder1 = new AdvancedSearchQueryBuilder(keyPersonnels);
            var _builder1 = new SqlBuilder();
            var selector1 = _builder1.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilder1.getQuery())
            {
                _builder1.Where(d.sql, d.value);
            }

            //Subquery to search on Keypersonnel table
            if (keyPersonnels.Count > 0)
            {
                _builder.AddParameters(selector1.Parameters);
            }


            //for search by company...
            var index = 0;
            foreach (var company in companyEntity)
            {
                companyList = GenerateOrgIDSearchRequestList(company, OperatorName.StringLikeStartWith, index);
            }
            var companyQueryBuilder = new AdvancedSearchQueryBuilder(companyList);
            var companyQuery = companyQueryBuilder.getQuery();
            var _builderCompany = new SqlBuilder();
            var selectorCompany = _builderCompany.AddTemplate(" /**where**/");
            var companyParameter = string.Empty;
            foreach (dynamic d in companyQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderCompany.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorCompany.Parameters);

            //for search by region..
            foreach (var region in regionEntity)
            {
                regionList = GenerateOrgIDSearchRequestList(region, OperatorName.StringLike, index);
            }
            var regionQueryBuilder = new AdvancedSearchQueryBuilder(regionList);
            var regionQuery = regionQueryBuilder.getQuery();
            var _builderRegion = new SqlBuilder();
            var selectorRegion = _builderRegion.AddTemplate(" /**where**/");
            foreach (dynamic d in regionQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderRegion.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorRegion.Parameters);

            orderBy = GetOrderByColumn(orderBy);

            if (string.IsNullOrEmpty(orderBy))
            {
                if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
                {
                    orderBy = " RA.UpdatedOn ";
                }
                else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
                {
                    orderBy = " C.UpdatedOn ";
                }
                else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
                {
                    orderBy = " C.UpdatedOn ";
                }
                else
                {
                    orderBy = "C.ContractNumber";
                }
            }
            else if (orderBy == "ContractNumber")
            {
                if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
                {
                    orderBy = " RA.UpdatedOn ";
                    dir = dir == "asc" ? "desc" : "asc";
                }
                else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
                {
                    orderBy = " C.UpdatedOn ";
                }
            }

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                _builder.OrWhere("C.ContractNumber LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("OrgId.title LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ProjectNumber] LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ContractTitle] LIKE @searchValue", new { searchValue = searchString });

            }
            var activitySelect = string.Empty;
            var activityPivotQuery = string.Empty;
            if (selector.RawSql != "")
                where = selector.RawSql.Replace("WHERE", " AND ");
            if (selector1.RawSql != "")
                where += selector1.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorCompany.RawSql))
                where += selectorCompany.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorRegion.RawSql))
                where += selectorRegion.RawSql.Replace("WHERE", " AND ");

            //search by revenue
            if (revenueRequired != null)
            {
                var obj = JObject.Parse(revenueRequired.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND c.RevenueRecognitionGuid IS NOT NULL";
                else
                    where += " AND c.RevenueRecognitionGuid IS NULL";
            }

            //search bby revenue completed
            if (revenueCompleted != null)
            {
                var obj = JObject.Parse(revenueCompleted.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND rr.IsCompleted = 1";
                else
                    where += " AND rr.IsCompleted = 0";
            }



            var recentQuery = string.Empty;
            var actionType = "";
            var userAction = additionalFilter;
            var activitySelectQuery = ", ra.*";
            var activityJoinQuery = $@"INNER JOIN (SELECT Distinct(EntityGuid) As EntityGuid,MAX([MyFavorite]) as MyFavorite,MAX([RecentlyViewed]) as RecentlyViewed,MAX([UpdatedOn]) AS UpdatedOn
							FROM RecentActivity
							PIVOT  
							(  
							MAX(UserGuid)
							FOR UserAction IN  
							([MyFavorite],[RecentlyViewed])) tt
							where Entity = 'Contract' AND IsDeleted = 0
							GROUP BY EntityGuid, MyFavorite, RecentlyViewed,UpdatedOn) ra
							ON ra.EntityGuid = c.ContractGuid";
            if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;

                actionType = EnumGlobal.ActivityType.RecentlyViewed.ToString();
                where += $" AND ra.RecentlyViewed = '{userGuid}'";
            }
            else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            {
                where += $" AND (C.CreatedBy = '{userGuid}' OR keyPersonnel.[account-representative] = '{userGuid}' OR keyPersonnel.[contract-representative] = '{userGuid}' or keyPersonnel.[project-manager] = '{userGuid}' or keyPersonnel.[project-controls] = '{userGuid}')";
            }
            else if (!string.IsNullOrEmpty(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;
                actionType = EnumGlobal.ActivityType.MyFavorite.ToString();
                where += $" AND ra.MyFavorite = '{userGuid}'";
                orderBy = " C.UpdatedOn ";
            }

            var taskOrderSql = string.Empty;
            if (isTaskOrder)
            {
                var taskCondition = where + whereEntity;
                taskCondition = taskCondition.Replace("[IsActive]", "c.[IsActive]");
                taskCondition = taskCondition.Replace("[UpdatedOn]", "c.[UpdatedOn]");
                taskCondition = taskCondition.Replace("[Description]", "c.[Description]");
                taskCondition = taskCondition.Replace("[CreatedOn]", "c.[CreatedOn]");

                taskCondition = taskCondition.Replace("[AwardAmount]", cumulativeAwardAmountQuery);
                taskCondition = taskCondition.Replace("[FundingAmount]", cumulativeFundingAmountQuery);
                taskOrderSql = $@"SELECT c.ParentContractGuid 
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
							LEFT JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
                            LEFT JOIN Users pm
							ON pm.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users pc
							ON pc.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users ar
							ON ar.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users cr
							ON cr.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users rm
                            ON rm.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users cp
                            ON cp.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                            LEFT JOIN Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
                            LEFT JOIN CustomerContact OfficeContractRepresentative on C.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact OfficeContractTechnicalRepresent on  C.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractRepresentative on C.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractTechnicalRepresent on C.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN ResourceAttributeValue ContractType on c.ContractType = ContractType.Value
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        WHERE c.IsDeleted = 0
						and ISIDIQContract=0 {taskCondition}";

                //where += $" OR c.ContractGuid IN ({taskOrderSql})";
            }

            var sqlQuery = $@"SELECT c.[ContractGuid]
                                      ,c.[IsIDIQContract]
                                      ,[IsPrimeContract]
                                      ,[ContractNumber]
                                      ,[SubContractNumber]
                                      ,[ORGID]
                                      ,[ProjectNumber]
                                      ,[ContractTitle]
                                      ,[CountryOfPerformance]
                                      ,[PlaceOfPerformance]
                                      ,[POPStart]
                                      ,[POPEnd]
                                      ,[NaicsCode]
                                      ,[PSCCode]
                                      ,[CPAREligible]
                                      ,[QualityLevelRequirements]
                                      ,[QualityLevel]
                                      ,[AwardingAgencyOffice]
                                      ,[OfficeContractRepresentative]
                                      ,[OfficeContractTechnicalRepresent]
                                      ,[FundingAgencyOffice]
                                      ,[SetAside]
                                      ,[SelfPerformancePercent]
                                      ,[SBA]
                                      ,[Competition]
                                      ,c.[ContractType]
                                      ,[OverHead]
                                      ,[GAPercent]
                                      ,[FeePercent]
                                      ,[Currency]
                                      ,[BlueSkyAwardAmount]
                                      ,[BillingAddress]
                                      ,[BillingFrequency]
                                      ,[InvoiceSubmissionMethod]
                                      ,[PaymentTerms]
                                      ,[AppWageDetermineDavisBaconAct]
                                      ,[BillingFormula]
                                      ,[RevenueFormula]
                                      ,[RevenueRecognitionEACPercent]
                                      ,[OHonsite]
                                      ,[OHoffsite]
                                      ,c.[CreatedOn]
                                      ,c.[UpdatedOn]
                                      ,c.[CreatedBy]
                                      ,c.[IsActive]
                                      ,c.[Status]
                                      ,c.[Description]
                                      ,[AppWageDetermineServiceContractAct]
                                      ,[FundingOfficeContractRepresentative]
                                      ,[FundingOfficeContractTechnicalRepresent]
                                      ,[ParentContractGuid]
                                      ,[ProjectCounter]
                                      ,[ApplicableWageDetermination]
                                      ,[RevenueRecognitionGuid],
                                       OrgID.Name as companyName,
                                       OrgID.Name as regionName,
                                    AwardAmount = {cumulativeAwardAmountQuery},
                                    FundingAmount = {cumulativeFundingAmountQuery},
                                    ContractType.Name As ContractType,AwardingAgency.CustomerName AwardingAgencyName,FundingAgency.CustomerName FundingAgencyName,
                                    (OfficeContractTechnicalRepresent.FirstName + ' ' + OfficeContractTechnicalRepresent.MiddleName + ' ' + OfficeContractTechnicalRepresent.LastName) AwardingAgencyContractTechnicalRepresentativeName,
                                    (OfficeContractRepresentative.FirstName + ' ' + OfficeContractRepresentative.MiddleName + ' ' + OfficeContractRepresentative.LastName) AwardingAgencyContractRepresentativeName,
                                    (FundingOfficeContractRepresentative.FirstName + ' ' + FundingOfficeContractRepresentative.MiddleName + ' ' + FundingOfficeContractRepresentative.LastName) FundingAgencyContractRepresentativeName,
                                    (FundingOfficeContractTechnicalRepresent.FirstName + ' ' + FundingOfficeContractTechnicalRepresent.MiddleName + ' ' + FundingOfficeContractTechnicalRepresent.LastName) FundingAgencyContractTechnicalRepresentativeName,
                                    --keyPersonnel.*,
                                    cr.UserGuid as ContractRepresentativeGuid, cr.*,
									pc.UserGuid as ProjectControlGuid,pc.*,
                                    pm.UserGuid AS ProjectManagerGuid, pm.*,
									ar.UserGuid as AccountRepresentativeGuid,ar.*, 
                                    rm.UserGuid AS RegionalManagerGuid,rm.*,
                                    cp.UserGuid AS CompanyPresidentGuid,cp.*,
                                    SUBSTRING(OrgID.Name,CHARINDEX('.',OrgID.Name)-2,2) as companyName,
									SUBSTRING(OrgID.Name,CHARINDEX('.',OrgID.Name)-2,2) as regionName,
                                    OrgID.Name as orgidName,OrgID.*  {activitySelect}
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
							LEFT JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
							{activityPivotQuery}
                            LEFT JOIN Users pm
							ON pm.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users pc
							ON pc.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users ar
							ON ar.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users cr
							ON cr.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users rm
                            ON rm.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users cp
                            ON cp.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                            LEFT JOIN Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
                            LEFT JOIN CustomerContact OfficeContractRepresentative on C.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact OfficeContractTechnicalRepresent on  C.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractRepresentative on C.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractTechnicalRepresent on C.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN ResourceAttributeValue ContractType on c.ContractType = ContractType.Value
                            LEFT JOIN RevenueRecognization rr ON rr.RevenueRecognizationGuid = c.RevenueRecognitionGuid
                        --LEFT JOIN ContractResourceFile cf
                        --ON cf.ResourceGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        WHERE c.IsDeleted = 0
                        AND ISIDIQContract=0";

            where = where.Replace("[IsActive]", "c.[IsActive]");
            where = where.Replace("[UpdatedOn]", "c.[UpdatedOn]");
            where = where.Replace("[Description]", "c.[Description]");
            where = where.Replace("[CreatedOn]", "c.[CreatedOn]");

            where = where.Replace("[AwardAmount]", cumulativeAwardAmountQuery);
            where = where.Replace("[FundingAmount]", cumulativeFundingAmountQuery);
            sqlQuery += $"{ where } {whereEntity} ";
            if (isTaskOrder)
            {
                sqlQuery += $" OR c.ContractGuid IN ({taskOrderSql})";
            }
            sqlQuery += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var contractDictionary = new Dictionary<Guid, Contracts>();

            var objectArray = new[] {
                typeof(Contracts),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Core.Entities.User),
                typeof(Organization)
            };

            var contractList = _context.Connection.Query<Contracts>(sqlQuery, objectArray, m =>
            {
                var contractEntity = new Contracts();
                contractEntity = m[0] as Contracts;
                contractEntity.ContractRepresentative = m[1] as User;
                contractEntity.ProjectControls = m[2] as User;
                contractEntity.ProjectManager = m[3] as User;
                contractEntity.AccountRepresentative = m[4] as User;
                contractEntity.RegionalManager = m[5] as User;
                contractEntity.CompanyPresident = m[6] as User;
                contractEntity.Organisation = m[7] as Organization;
                return contractEntity;
            }, selector.Parameters,
                splitOn: "ContractRepresentativeGuid,ProjectControlGuid,ProjectManagerGuid,AccountRepresentativeGuid,RegionalManagerGuid,CompanyPresidentGuid,orgidName", commandTimeout: 120).ToList();
            return contractList;
        }

        public int GetContractCountForPFS(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            var where = "";
            var searchString = "";
            var subQueryKeyPersonnel = "";
            var whereEntity = "";
            var cumulativeAwardAmountQuery = @"((SELECT COALESCE(SUM(awardamount), 0) FROM ContractModification cm1 WHERE cm1.ContractGuid = c.ContractGuid AND cm1.IsDeleted = 0) 
                                                + c.AwardAmount 
                                                + (SELECT COALESCE(SUM(task.awardamount), 0) FROM [Contract] task WHERE task.ParentContractGuid = c.ContractGuid) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid from Contract cm2 where cm2.ParentContractGuid = c.ContractGuid)) 
                                                +(SELECT COALESCE(SUM(cm1.awardamount), 0) FROM[ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";

            var cumulativeFundingAmountQuery = @"((SELECT COALESCE(SUM(fundingAmount),0) FROM ContractModification cm1 WHERE cm1.ContractGuid=c.ContractGuid AND cm1.IsDeleted = 0 ) 
                                                + c.fundingAmount 
                                                + (SELECT COALESCE(SUM(task.fundingAmount),0) FROM [Contract] task WHERE task.ParentContractGuid=c.ContractGuid) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [Contract] cm1 WHERE cm1.ParentContractGuid IN (SELECT cm2.contractGuid FROM Contract cm2 WHERE cm2.ParentContractGuid=c.ContractGuid)) 
                                                + (SELECT COALESCE(SUM(cm1.fundingAmount),0) FROM [ContractModification] cm1 WHERE cm1.ContractGuid IN (SELECT cm.ContractGuid FROM ContractModification cm WHERE ContractGuid IN (SELECT ContractGuid FROM Contract cc WHERE cc.ParentContractGuid = c.ContractGuid)) ))";
            List<AdvancedSearchRequest> withoutEntity = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeTitle != "Company" && c.Attribute.AttributeTitle != "Region" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> withEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeTitle != "Company" && c.Attribute.AttributeTitle != "Region" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> companyEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeTitle == "Company" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            List<AdvancedSearchRequest> regionEntity = postValue.Where(c => c.IsEntity == true && c.Attribute.AttributeTitle == "Region" && c.Attribute.AttributeName != "IsRevenueRecRequired" && c.Attribute.AttributeName != "IsRevenueRecCompleted").ToList();
            AdvancedSearchRequest revenueRequired = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeName == "IsRevenueRecRequired").FirstOrDefault();
            AdvancedSearchRequest revenueCompleted = postValue.Where(c => c.IsEntity == false && c.Attribute.AttributeName == "IsRevenueRecCompleted").FirstOrDefault();
            List<AdvancedSearchRequest> keyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> withoutKeyPersonnels = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> companyList = new List<AdvancedSearchRequest>();
            List<AdvancedSearchRequest> regionList = new List<AdvancedSearchRequest>();

            foreach (var entity in withEntity)
            {
                dynamic value = entity.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    if (v.id == ContractUserRole._accountRepresentative || v.id == ContractUserRole._companyPresident || v.id == ContractUserRole._contractRepresentative ||
                    v.id == ContractUserRole._projectControls || v.id == ContractUserRole._projectManager || v.id == ContractUserRole._regionalManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }
                else
                {
                    if (value.id == ContractUserRole._accountRepresentative || value.id == ContractUserRole._companyPresident || value.id == ContractUserRole._contractRepresentative ||
                    value.id == ContractUserRole._projectControls || value.id == ContractUserRole._projectManager || value.id == ContractUserRole._regionalManager)
                    {

                        keyPersonnels.Add(entity);
                    }
                    else
                    {
                        withoutKeyPersonnels.Add(entity);
                    }
                }

            }
            var queryBuilder = new AdvancedSearchQueryBuilder(withoutEntity);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }

            foreach (var item in withoutKeyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }
            var queryBuilderEntity = new AdvancedSearchQueryBuilder(withoutKeyPersonnels);
            var queryEntity = queryBuilderEntity.getQuery();
            var _builderEntity = new SqlBuilder();
            var selectorEntity = _builderEntity.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilderEntity.getQuery())
            {
                _builderEntity.Where(d.sql, d.value);
            }

            if (withoutKeyPersonnels.Count > 0)
            {
                _builder.AddParameters(selectorEntity.Parameters);
                whereEntity = selectorEntity.RawSql.Replace("WHERE", " AND ");
            }

            // Adding key personnel in query
            foreach (var item in keyPersonnels)
            {
                dynamic value = item.Value;
                if (value.GetType() == typeof(JArray))
                {
                    dynamic v = ((JArray)value)[0];
                    item.Attribute.AttributeName = v.id;
                }
                else
                {
                    item.Attribute.AttributeName = value.id;
                }

            }

            var queryBuilder1 = new AdvancedSearchQueryBuilder(keyPersonnels);
            var _builder1 = new SqlBuilder();
            var selector1 = _builder1.AddTemplate(" /**where**/");
            foreach (dynamic d in queryBuilder1.getQuery())
            {
                _builder1.Where(d.sql, d.value);
            }


            //for search by company...
            var index = 0;
            foreach (var company in companyEntity)
            {
                companyList = GenerateOrgIDSearchRequestList(company, OperatorName.StringLikeStartWith, index);
            }
            var companyQueryBuilder = new AdvancedSearchQueryBuilder(companyList);
            var companyQuery = companyQueryBuilder.getQuery();
            var _builderCompany = new SqlBuilder();
            var selectorCompany = _builderCompany.AddTemplate(" /**where**/");
            var companyParameter = string.Empty;
            foreach (dynamic d in companyQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderCompany.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorCompany.Parameters);

            //for search by region..
            foreach (var region in regionEntity)
            {
                regionList = GenerateOrgIDSearchRequestList(region, OperatorName.StringLike, index);
            }
            var regionQueryBuilder = new AdvancedSearchQueryBuilder(regionList);
            var regionQuery = regionQueryBuilder.getQuery();
            var _builderRegion = new SqlBuilder();
            var selectorRegion = _builderRegion.AddTemplate(" /**where**/");
            foreach (dynamic d in regionQuery)
            {
                var stringAttr = (string)d.sql;
                var attr = stringAttr.Replace("[OrgIDName" + index + "]", "OrgID.Name");
                _builderRegion.OrWhere(attr, d.value);
                index++;
            }
            _builder.AddParameters(selectorRegion.Parameters);

            //Subquery to search on Keypersonnel table
            if (keyPersonnels.Count > 0)
            {
                _builder.AddParameters(selector1.Parameters);
                subQueryKeyPersonnel = @" AND c.ContractGuid in (SELECT contractguid 
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
                                            " + selector1.RawSql + @"
                                            GROUP  BY contractguid, 
                                                      [regional-manager], 
                                                      [project-manager], 
                                                      [project-controls], 
                                                      [account-representative], 
                                                      [contract-representative], 
                                                      [company-president])";
            }

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                _builder.OrWhere("C.ContractNumber LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("OrgId.title LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ProjectNumber] LIKE @searchValue", new { searchValue = searchString });
                _builder.OrWhere("C.[ContractTitle] LIKE @searchValue", new { searchValue = searchString });

            }
            if (selector.RawSql != "")
                where = selector.RawSql.Replace("WHERE", " AND ");
            if (selector1.RawSql != "")
                where += selector1.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorCompany.RawSql))
                where += selectorCompany.RawSql.Replace("WHERE", " AND ");
            if (!string.IsNullOrWhiteSpace(selectorRegion.RawSql))
                where += selectorRegion.RawSql.Replace("WHERE", " AND ");

            //search by revenue
            if (revenueRequired != null)
            {
                var obj = JObject.Parse(revenueRequired.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND c.RevenueRecognitionGuid IS NOT NULL";
                else
                    where += " AND c.RevenueRecognitionGuid IS NULL";
            }

            //search bby revenue completed
            if (revenueCompleted != null)
            {
                var obj = JObject.Parse(revenueCompleted.Value.ToString());
                var value = (string)obj["value"];
                if (value == "1")
                    where += $" AND rr.IsCompleted = 1";
                else
                    where += " AND rr.IsCompleted = 0";
            }

            var actionType = "";
            var userAction = additionalFilter;
            var activitySelectQuery = ", ra.*";
            var activitySelect = string.Empty;
            var activityPivotQuery = string.Empty;
            var activityJoinQuery = $@"INNER JOIN (SELECT Distinct(EntityGuid) As EntityGuid,MAX([MyFavorite]) as MyFavorite,MAX([RecentlyViewed]) as RecentlyViewed,MAX([UpdatedOn]) AS UpdatedOn
							FROM RecentActivity
							PIVOT  
							(  
							MAX(UserGuid)
							FOR UserAction IN  
							([MyFavorite],[RecentlyViewed])) tt
							where Entity = 'Contract' AND IsDeleted = 0
							GROUP BY EntityGuid, MyFavorite, RecentlyViewed,UpdatedOn) ra
							ON ra.EntityGuid = c.ContractGuid";
            if (!string.IsNullOrWhiteSpace(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;
                actionType = EnumGlobal.ActivityType.RecentlyViewed.ToString();
                where += $" AND ra.RecentlyViewed = '{userGuid}'";
            }
            else if (!string.IsNullOrWhiteSpace(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            {
                where += $" AND (C.CreatedBy = '{userGuid}' OR keyPersonnel.[account-representative] = '{userGuid}' OR keyPersonnel.[contract-representative] = '{userGuid}' or keyPersonnel.[project-manager] = '{userGuid}' or keyPersonnel.[project-controls] = '{userGuid}')";
            }
            else if (!string.IsNullOrWhiteSpace(additionalFilter) && additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
            {
                activitySelect = activitySelectQuery;
                activityPivotQuery = activityJoinQuery;
                actionType = EnumGlobal.ActivityType.MyFavorite.ToString();
                where += $" AND ra.MyFavorite = '{userGuid}'";
            }

            //if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            //    where += $" AND C.CreatedBy = '{userGuid}'";
            //var joinString = string.Empty;
            //if (!string.IsNullOrWhiteSpace(additionalFilter) && (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower() || additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower()))
            //{
            //    joinString = @" LEFT JOIN RecentActivity RA 
            //                    ON RA.EntityGuid = C.ContractGuid AND RA.IsDeleted = 0 AND RA.Entity = 'Contract'";
            //    where += $" AND RA.UserAction='{userAction}'";
            //}

            var taskOrderSql = string.Empty;
            if (isTaskOrder)
            {
                var taskCondition = where + whereEntity;
                taskCondition = taskCondition.Replace("[IsActive]", "c.[IsActive]");
                taskCondition = taskCondition.Replace("[UpdatedOn]", "c.[UpdatedOn]");
                taskCondition = taskCondition.Replace("[Description]", "c.[Description]");
                taskCondition = taskCondition.Replace("[CreatedOn]", "c.[CreatedOn]");

                taskCondition = taskCondition.Replace("[AwardAmount]", cumulativeAwardAmountQuery);
                taskCondition = taskCondition.Replace("[FundingAmount]", cumulativeFundingAmountQuery);
                taskOrderSql = $@"SELECT c.ParentContractGuid 
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
							LEFT JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
                            LEFT JOIN Users pm
							ON pm.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users pc
							ON pc.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users ar
							ON ar.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users cr
							ON cr.UserGuid = keyPersonnel.[contract-representative]
                            LEFT JOIN Users rm
                            ON rm.UserGuid =  keyPersonnel.[regional-manager]
                            LEFT JOIN Users cp
                            ON cp.UserGuid = keyPersonnel.[company-president]
                            LEFT JOIN Customer AwardingAgency on C.AwardingAgencyOffice = AwardingAgency.CustomerGuid
                            LEFT JOIN Customer FundingAgency on C.FundingAgencyOffice = FundingAgency.CustomerGuid
                            LEFT JOIN CustomerContact OfficeContractRepresentative on C.OfficeContractRepresentative = OfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact OfficeContractTechnicalRepresent on  C.OfficeContractTechnicalRepresent = OfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractRepresentative on C.FundingOfficeContractRepresentative = FundingOfficeContractRepresentative.ContactGuid
                            LEFT JOIN CustomerContact FundingOfficeContractTechnicalRepresent on C.FundingOfficeContractTechnicalRepresent = FundingOfficeContractTechnicalRepresent.ContactGuid
                            LEFT JOIN ResourceAttributeValue ContractType on c.ContractType = ContractType.Value
                            
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        WHERE c.IsDeleted = 0
						and IsIDIQContract=0 {taskCondition}";
            }

            var sqlQuery = $@"SELECT COUNT(*)
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
							INNER JOIN Contract c on c.ContractGuid = keyPersonnel.ContractGuid
							{activityPivotQuery}
                            LEFT JOIN Users projectManager
							ON projectManager.UserGuid =  keyPersonnel.[project-manager]
							LEFT JOIN Users projectControls
							ON projectControls.UserGuid = keyPersonnel.[project-controls]
							LEFT JOIN Users accountRep
							ON accountRep.UserGuid = keyPersonnel.[account-representative]
							LEFT JOIN Users contractRep
							ON contractRep.UserGuid = keyPersonnel.[contract-representative]
                        --LEFT JOIN ContractResourceFile cf
                        --ON cf.ResourceGuid = c.ContractGuid
                        LEFT JOIN OrgID OrgID on c.ORGID = ORGID.OrgIDGuid
                        LEFT JOIN RevenueRecognization rr ON rr.RevenueRecognizationGuid = c.RevenueRecognitionGuid
                        WHERE c.IsDeleted = 0
                       AND IsIDIQContract = 0";
            where = where.Replace("[IsActive]", "c.[IsActive]");
            where = where.Replace("[UpdatedOn]", "c.[UpdatedOn]");
            where = where.Replace("[Description]", "c.[Description]");
            where = where.Replace("[AwardAmount]", cumulativeAwardAmountQuery);
            where = where.Replace("[FundingAmount]", cumulativeFundingAmountQuery);
            sqlQuery += $"{ where } {whereEntity} ";

            if (isTaskOrder)
            {
                sqlQuery += $" OR c.ContractGuid IN ({taskOrderSql})";
            }
            return _context.Connection.ExecuteScalar<int>(sqlQuery, selector.Parameters);
        }
        #endregion

        public Guid GetFarContractTypeGuidById(Guid id)
        {
            string projectnumbersql = $"SELECT FarContractTypeGuid FROM Contract WHERE ContractGuid = @ContractGuid;";
            var farContractTypeGuidById = _context.Connection.QuerySingle<Guid?>(projectnumbersql, new { ContractGuid = id });
            return farContractTypeGuidById ?? Guid.Empty;
        }

        public int GetTotalCountProjectByContractId(Guid contractGuid)
        {
            var getCount = string.Format($@"    
			   select count(1) from contract 
			   where contract.IsDeleted = 0 and contract.ParentContractGuid = @ContractGuid");
            var totalProjectForContract = _context.Connection.QuerySingle<int>(getCount, new { ContractGuid = contractGuid });
            return totalProjectForContract;
        }

        public int HasChild(Guid projectGuid)
        {
            var getCount = string.Format($@"
			   select count(1) from projectModification where isDeleted = 0 and projectGuid = @ProjectGuid");
            var totalProjectModificaton = _context.Connection.QuerySingle<int>(getCount, new { ProjectGuid = projectGuid });
            return totalProjectModificaton;
        }

        public Contracts GetInfoByContractGuid(Guid contractGuid)
        {
            string sql = $"SELECT contractnumber,projectnumber,contracttitle FROM Contract WHERE ContractGuid = @ContractGuid;";
            var result = _context.Connection.QuerySingle<Contracts>(sql, new { ContractGuid = contractGuid });
            return result;
        }
    }
}
