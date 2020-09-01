using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System.Linq;
using System.Text;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Infrastructure.Data.Admin
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDatabaseContext _context;
        public CustomerRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int Add(Customer customer)
        {
            string insertQuery = @"INSERT INTO [dbo].[Customer]
                                                                   (
                                                                    CustomerGuid,
                                                                    CustomerName, 
                                                                    IsActive,
                                                                    IsDeleted, 
                                                                    CreatedOn, 
                                                                    UpdatedOn, 
                                                                    CreatedBy,
                                                                    UpdatedBy,
                                                                    Address,
                                                                    AddressLine1,
                                                                    City,
                                                                    StateId,
                                                                    ZipCode,
                                                                    CountryId,
                                                                    CustomerTypeGuid,
                                                                    CustomerDescription,
                                                                    PrimaryPhone,
                                                                    PrimaryEmail,
                                                                    Abbreviations,
                                                                    Tags,
                                                                    CustomerCode,
                                                                    Agency,
                                                                    Url,
                                                                    Department
                                                                    )
                                  VALUES (
                                                                    @CustomerGuid,
                                                                    @CustomerName, 
                                                                    @IsActive,
                                                                    @IsDeleted, 
                                                                    @CreatedOn, 
                                                                    @UpdatedOn, 
                                                                    @CreatedBy,
                                                                    @UpdatedBy,
                                                                    @Address,
                                                                    @AddressLine1,
                                                                    @City,
                                                                    @StateId,
                                                                    @ZipCode,
                                                                    @CountryId,
                                                                    @CustomerTypeGuid,
                                                                    @CustomerDescription,
                                                                    @PrimaryPhone,
                                                                    @PrimaryEmail,
                                                                    @Abbreviations,
                                                                    @Tags,
                                                                    @CustomerCode,
                                                                    @Agency,
                                                                    @Url,
                                                                    @Department
                                                                )";
            return _context.Connection.Execute(insertQuery, customer);
        }

        public int Edit(Customer customer)
        {
            string updateQuery = @"Update Customer set 
                                                                    CustomerName  =@CustomerName      , 
                                                                    UpdatedOn   =@UpdatedOn            ,
                                                                    UpdatedBy   =@UpdatedBy            ,
                                                                    Address     =@Address,
                                                                    AddressLine1=@AddressLine1         ,
                                                                    City        =@City,
                                                                    StateId     =@StateId        ,
                                                                    ZipCode     =@ZipCode      ,
                                                                    CountryId   =@CountryId,
                                                                    CustomerTypeGuid=@CustomerTypeGuid,
                                                                    CustomerDescription=@CustomerDescription,
                                                                    PrimaryPhone=@PrimaryPhone,
                                                                    PrimaryEmail= @PrimaryEmail,
                                                                    Abbreviations = @Abbreviations,
                                                                    Tags=@Tags,
                                                                    CustomerCode=@CustomerCode,
                                                                    Agency=@Agency,
                                                                    Url=@Url,
                                                                    Department=@Department
                                            where CustomerGuid =@CustomerGuid ";
            return _context.Connection.Execute(updateQuery, customer);
        }

        public int Delete(Guid[] customerGuidIds)
        {
            foreach (var customerGuid in customerGuidIds)
            {
                var customer = new
                {
                    CustomerGuid = customerGuid
                };
                string disableQuery = @"Update Customer set 
                                                                    IsDeleted   = 1
                                                                   
                                            where CustomerGuid =@CustomerGuid ";
                _context.Connection.Execute(disableQuery, customer);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int TotalRecord(string searchValue)
        {
            var searchString = string.Empty;
            var where = string.Empty;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(CustomerName like @searchValue )";
            }
            string sql = $"SELECT Count(1) FROM Customer WHERE IsDeleted = 0 {where}";
            var result = _context.Connection.QuerySingle<int>(sql, new { searchValue = searchString });
            return result;
        }

        public Customer GetCustomerById(Guid CustomerGuid)
        {

            string sql = @"SELECT * 
                            FROM Customer c
                            LEFT JOIN CustomerType ct
                            ON ct.CustomerTypeGuid = c.CustomerTypeGuid
                            WHERE CustomerGuid = @CustomerGuid;";
            var result = _context.Connection.QuerySingle<Customer>(sql, new { CustomerGuid = CustomerGuid });
            return result;
        }

        public int DisableCustomer(Guid[] customerGuidIds)
        {
            foreach (var customerGuid in customerGuidIds)
            {
                var customer = new
                {
                    CustomerGuid = customerGuid
                };
                string disableQuery = @"Update Customer set 
                                            IsActive   = 0
                                            where CustomerGuid =@CustomerGuid ";
                _context.Connection.Execute(disableQuery, customer);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int EnableCustomer(Guid[] customerGuidIds)
        {
            foreach (var customerGuid in customerGuidIds)
            {
                var customer = new
                {
                    CustomerGuid = customerGuid
                };
                string disableQuery = @"Update Customer set 
                                            IsActive   = 1
                                            where CustomerGuid =@CustomerGuid ";
                _context.Connection.Execute(disableQuery, customer);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }

        public Customer GetCustomerDetailsById(Guid id)
        {
            string sql = @"select customerType.CustomerTypeName,
                customer.Department,
                customer.Agency,
                customer.CustomerName, 
                customer.CustomerCode,
                customer.Address,
                customer.AddressLine1,
                customer.City,
                states.StateName StatesName,
                country.CountryName CountryName,
                customer.ZipCode,
                customer.PrimaryPhone,
                customer.PrimaryEmail,
                customer.Url,
                customer.Tags,
                customer.Abbreviations,
                customer.CustomerDescription
                 from Customer customer
                left join CustomerType customertype on customerType.CustomerTypeGuid = customer.CustomerTypeGuid
                left join State states on states.StateId = customer.StateId
                left join Country country on country.CountryId = customer.CountryId where customer.CustomerGuid = @id;";
            var result = _context.Connection.QuerySingle<Customer>(sql, new { id = id });
            return result;
        }

        public IEnumerable<Customer> Find(CustomerSearchSpec spec)
        {
            var o = new Dictionary<string, object>();
            var sql = BuildSql(spec, out o);
            if (sql == string.Empty) throw new Exception("Empty Sql");
            var result = _context.Connection.Query<Customer>(sql, new DynamicParameters(o));
            return _context.Connection.Query<Customer>(sql, new DynamicParameters(o));
            //            return _context.Connection.Query<Customer>(sql);
        }

        public string BuildSql(BaseSearchSpec spec, out Dictionary<string, object> o)
        {
            IBaseRepository baseRepository = new BaseRepository<Customer>();
            var sql = baseRepository.BuildSql(spec, out o);
            return sql;
        }

        public IEnumerable<Attribute> GetAttributeNameListByResource(string resourceName)
        {
            var result = _context.Connection.Query<Attribute>($@"select * from Attributes where Resource = @Resource", new { Resource = resourceName });
            return result;
        }

        public IEnumerable<Customer> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            var orderingQuery = GetOrderByColumn(sortField, sortDirection);
            var where = "";
            var searchString = "";
            var rowNum = skip + pageSize;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(CustomerName like @searchValue )";
            }
            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                            (SELECT ROW_NUMBER() OVER (ORDER BY {orderingQuery}) AS RowNum, 
                                                                                        Customer.CustomerGuid,
                                                                                        customer.CustomerName DisplayName,
                                                                                        Customer.PrimaryEmail,
                                                                                        Customer.Agency,
                                                                                        Customer.Department,
                                                                                        Customer.CustomerCode,
                                                                                        case when isnull(Customer.Department,'') = '' then Customer.CustomerName else Customer.CustomerName
																						 +' , '+Customer.Agency + ' , '+Customer.Department end CustomerName,
                                                                                        Customer.IsActive,
																						Customer.AddressLine1,
																						Customer.City,
																						states.StateName State,
																						country.CountryName Country,
            																			Customer.ZipCode ZipCode,
            																			Customer.UpdatedOn,
                                                                                        Customer.PrimaryPhone PrimaryPhone,
                                                                                        Customer.Abbreviations Abbreviations,
                                                                                        Customer.Tags Tags,
																						Customer.Address,
                                                                                        CustomerType.CustomerTypeName customerTypeName
                                                                                        FROM Customer Customer
                                                                                        LEFT JOIN Country country
                                                                                        ON country.CountryId = Customer.CountryId
                                                                                        LEFT JOIN State states
                                                                                        ON states.StateId = Customer.StateId
                                                                                        LEFT JOIN CustomerType Customertype
                                                                                        ON Customertype.CustomerTypeGuid = Customer.CustomerTypeGuid
                                                                                        where Customer.IsDeleted = 0
                                                                                        { where }
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > @skip 
                                            AND RowNum <= @rowNum  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<Customer>(pagingQuery, new { searchValue = searchString, skip = skip, rowNum = rowNum });
            return pagedData;
        }

        public string GetOrderByColumn(string sortField, string sortDirection)
        {
            switch (sortDirection.ToUpper())
            {
                case "DESC":
                    sortDirection = " Desc";
                    break;
                default:
                    sortDirection = " Asc";
                    break;
            }
            var sortBy = "";
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToUpper())
                {
                    case "ISACTIVESTATUS":
                        sortBy = "Customer.isActive" + sortDirection;
                        break;
                    case "CUSTOMERNAME":
                        sortBy = "CustomerName" + sortDirection;
                        break;
                    case "CUSTOMERTYPENAME":
                        sortBy = "customerTypeName" + sortDirection;
                        break;
                    case "ADDRESS":
                        sortBy = "ADDRESS" + sortDirection;
                        break;
                    case "UPDATEDON":
                        sortBy = "Customer.UpdatedOn" + sortDirection;
                        break;
                    default:
                        sortBy = "CUSTOMERCODE" + sortDirection;
                        break;
                }
            }
            return sortBy;
        }

        public int CheckDuplicates(Customer customer)
        {
            string sql = $@"SELECT Count(1) 
                            FROM Customer 
                            WHERE (CustomerCode = @CustomerCode or CustomerName = @CustomerName) 
                            AND CustomerGuid != @CustomerGuid 
                            AND IsDeleted != 1 ";
            var result = _context.Connection.QuerySingle<int>(sql, new { CustomerCode = customer.CustomerCode, CustomerName = customer.CustomerName, CustomerGuid = customer.CustomerGuid });
            return result;
        }

        public ICollection<Customer> GetOfficeData(string searchText)
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
                and customer.IsDeleted = 0
                order by CustomerName asc";
            var data = _context.Connection.Query<Customer>(sql).ToList();
            return data;
        }

        public IEnumerable<Customer> GetCustomerList()
        {
            var sql = @"SELECT *
                          FROM [dbo].[Customer] where IsDeleted = 0 Order By CustomerName asc
                      ";
            return _context.Connection.Query<Customer>(sql);
        }

        public int CheckDuplicateForImport(string name, Guid customerGuid)
        {
            string sql = $@"SELECT Count(1) 
                            FROM Customer 
                            WHERE CustomerName = @CustomerName 
                            AND CustomerGuid != @CustomerGuid 
                            AND IsDeleted =0 ";
            var result = _context.Connection.QuerySingle<int>(sql, new { CustomerName = name, CustomerGuid = customerGuid });
            return result;
        }

        public Customer GetCustomerByCodeOrName(string name, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = string.Empty;
            string sql = @"SELECT * 
                                FROM Customer
                                WHERE (CustomerCode = @CustomerCode
                                OR CustomerName = @CustomerName)";
            return _context.Connection.QueryFirstOrDefault<Customer>(sql, new { CustomerCode = code, CustomerName = name });
        }
        public Customer GetCustomerByName(string customerName)
        {
            var query = @"SELECT *
                        FROM Customer
                        WHERE CustomerName = @customerName";
            return _context.Connection.QueryFirstOrDefault<Customer>(query, new { customerName = customerName });
        }

        public int DeleteById(Guid id)
        {
            string disableQuery = @"Update Customer set 
                                                                    IsDeleted   = 1
                                                                   
                                            where CustomerGuid =@CustomerGuid ";
           return _context.Connection.Execute(disableQuery, new { CustomerGuid = id });
        }
    }
}
