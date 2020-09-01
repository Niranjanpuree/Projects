using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;

namespace Northwind.Infrastructure.Data.Admin
{
    public class CustomerContactRepository : ICustomerContactRepository
    {
        IDatabaseContext _context;
        public CustomerContactRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int Add(CustomerContact customerContact)
        {
            string insertQuery = @"INSERT INTO [dbo].customercontact
                                                                   (
                                                                    contactGuid,
                                                                    FirstName, 
																	MiddleName,
																	LastName,
																	ContactTypeGuid,
																	PhoneNumber,
																	AltPhoneNumber,
																	EmailAddress,
																	AltEmailAddress,
																	Notes,
																	CustomerGuid,
                                                                    IsActive,
                                                                    IsDeleted, 
                                                                    CreatedOn, 
                                                                    UpdatedOn, 
                                                                    CreatedBy,
                                                                    UpdatedBy
                                                                    )
                                  VALUES (
                                                                    @contactGuid,
                                                                    @FirstName, 
																	@MiddleName,
																	@LastName,
																	@ContactTypeGuid,
																	@PhoneNumber,
																	@AltPhoneNumber,
																	@EmailAddress,
																	@AltEmailAddress,
																	@Notes,
																	@CustomerGuid,
                                                                    @IsActive,
                                                                    @IsDeleted, 
                                                                    @CreatedOn, 
                                                                    @UpdatedOn, 
                                                                    @CreatedBy,
                                                                    @UpdatedBy
                                                                )";
            return _context.Connection.Execute(insertQuery, customerContact);
        }

        public int Delete(Guid[] ids)
        {
            foreach (var contactGuid in ids)
            {
                var customerContact = new
                {
                    ContactGuid = contactGuid
                };
                string deleteQuery = @"Update customercontact set 
                                                                    IsDeleted   = 1
                                                                   
                                            where contactGuid =@contactGuid ";
                _context.Connection.Execute(deleteQuery, customerContact);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Disable(Guid[] customerContactGuids)
        {
            foreach (var customerContactGuid in customerContactGuids)
            {
                var customerContact = new
                {
                    CustomerContactGuid = customerContactGuid
                };
                string deleteQuery = @"Update CustomerContact set 
                                                                    IsActive   = 0
                                                                   
                                            where ContactGuid =@CustomerContactGuid ";
                _context.Connection.Execute(deleteQuery, customerContact);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Enable(Guid[] customerContactGuids)
        {
            foreach (var customerContactGuid in customerContactGuids)
            {
                var customerContact = new
                {
                    CustomerContactGuid = customerContactGuid
                };
                string deleteQuery = @"Update CustomerContact set 
                                                                    IsActive   = 1
                                                                   
                                            where ContactGuid =@CustomerContactGuid ";
                _context.Connection.Execute(deleteQuery, customerContact);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public CustomerContact GetbyId(Guid id)
        {
            string sql = "SELECT * FROM CustomerContact WHERE ContactGuid = @CustomerContactGuid;";
            var result = _context.Connection.QuerySingle<CustomerContact>(sql, new { CustomerContactGuid = id });
            return result;
        }

        public CustomerContact GetDetailsById(Guid contactGuid)
        {
            string sql = @"select   CustomerContact.ContactGuid,
                                    CustomerContact.FirstName,
									CustomerContact.MiddleName,
									CustomerContact.LastName,
									CustomerContact.Gender,
									contacttype.ContactTypeName,
									CustomerContact.JobTitle,
									CustomerContact.PhoneNumber,
									CustomerContact.AltPhoneNumber,
									CustomerContact.EmailAddress,
									CustomerContact.AltEmailAddress,
									CustomerContact.Notes,
									CustomerContact.CustomerGuid,
									Customer.CustomerName,
									CustomerContact.ContactTypeGuid,
                                    CustomerContact.IsActive 
                                    from CustomerContact customerContact 
                                    left join
                                    CustomerContactType contacttype on
                                    contacttype.ContactTypeGuid = customerContact.ContactTypeGuid 
									left join
									Customer customer on
									customer.CustomerGuid = customerContact.CustomerGuid 
                            WHERE ContactGuid = @ContactGuid;";
            var result = _context.Connection.QuerySingle<CustomerContact>(sql, new { ContactGuid = contactGuid });
            return result;
        }

        public ICollection<KeyValuePairModel<Guid, string>> GetUserList()
        {
            var model = new List<KeyValuePairModel<Guid, string>>();
            var data = _context.Connection.Query<User>("SELECT * FROM users");
            foreach (var item in data)
            {
                model.Add(new KeyValuePairModel<Guid, string> { Keys = item.UserGuid, Values = item.DisplayName });
            }
            return model;
        }

        public int TotalRecord(Guid customerGuid)
        {
            var customerGuids = new
            {
                CustomerGuid = customerGuid
            };
            string sql = "SELECT Count(1) FROM CustomerContact WHERE IsDeleted = 0 and CustomerGuid = @CustomerGuid";
            //var result = _context.Connection.Execute(sql, CustomerGuids);
            var result = _context.Connection.QuerySingle<int>(sql, customerGuids);
            return result;
        }

        public int Edit(CustomerContact customerContact)
        {
            //var contactGuid = CustomerContact.ContactGuid;
            string updateQuery = @"update CustomerContact set 
                                                                    contactGuid     = @contactGuid     ,
                                                                    FirstName       = @FirstName       ,
																	MiddleName      = @MiddleName      ,
																	LastName        = @LastName        ,
																	ContactTypeGuid = @ContactTypeGuid ,
																	PhoneNumber     = @PhoneNumber     ,
																	AltPhoneNumber = @AltPhoneNumber ,
																	EmailAddress    = @EmailAddress    ,
																	AltEmailAddress= @AltEmailAddress,
																	Notes           = @Notes           ,
																	CustomerGuid    = @CustomerGuid    ,
                                                                    UpdatedOn       = @UpdatedOn       ,
                                                                    UpdatedBy       = @UpdatedBy       
                                                                where ContactGuid = @contactGuid";
            return _context.Connection.Execute(updateQuery, customerContact);
        }

        public IEnumerable<CustomerContact> GetAll(string searchValue, Guid customerGuid, int pageSize, int skip, string sortField, string sortDirection)
        {
            var orderingQuery = GetOrderByColumn(sortField,sortDirection);
            var where = "";
            var searchString = "";
            var rowNum = skip + pageSize;
            var orderBy = $@" ORDER BY {orderingQuery} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue.Trim() + "%";
                where = " AND ";
                where += "(contact.FirstName like @searchValue OR contact.LastName like @searchValue OR contact.LastName like @searchValue)";
            }
            var pagingQuery = string.Format($@"Select contact.contactGuid,
                                                contact.FirstName+' '+ ISNULL(contact.MiddleName,'')+' '+ contact.LastName FullName,
                                                contact.PhoneNumber,
                                                contact.PhoneNumber ContactNumber,
                                                contact.AltPhoneNumber,
                                                contact.AltEmailAddress,
                                                contact.EmailAddress,
												contact.Notes,
												contactType.ContactTypeName,
                                                contact.IsActive,
                                                contact.CreatedOn,
                                                contact.CreatedBy,
                                                contact.UpdatedOn,
                                                contact.UpdatedBy 
                                                from CustomerContact contact 
                                                left join
                                                CustomerContactType contactType on contactType.ContactTypeGuid = contact.ContactTypeGuid
                                                where contact.IsDeleted = 0 and contact.CustomerGuid = @CustomerGuid
                                                { where } {orderBy}");

            var pagedData = _context.Connection.Query<CustomerContact>(pagingQuery, new { searchValue = searchString, CustomerGuid = customerGuid, skip = skip, rowNum = rowNum});
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
                        sortBy = "contact.isActive" + sortDirection;
                        break;
                    case "CONTACTTYPENAME":
                        sortBy = "CONTACTTYPENAME" + sortDirection;
                        break;
                    case "PHONENUMBER":
                        sortBy = "PHONENUMBER" + sortDirection;
                        break;
                    case "EMAIL":
                        sortBy = "EMAILADDRESS" + sortDirection;
                        break;
                    case "UPDATEDON":
                        sortBy = "UpdatedOn" + sortDirection;
                        break;
                    default:
                        sortBy = "FirstName" + sortDirection;
                        break;
                }
            }
            return sortBy;
        }

        public IEnumerable<CustomerContact> GetCustomerContactList()
        {
            var sql = @"SELECT *
                          FROM [dbo].[CustomerContact] where IsDeleted = 0 Order By FirstName asc
                      ";
            return _context.Connection.Query<CustomerContact>(sql);
        }

        public CustomerContact GetCustomerContactByName(string firstName,string lastName, Guid customerGuid)
        {
            var query = @"SELECT *
                        FROM [dbo].[CustomerContact]
                        WHERE FirstName = @firstName
                        AND LastName = @lastName
                        AND CustomerGuid = @customerGuid";
            return _context.Connection.QueryFirstOrDefault<CustomerContact>(query,new { firstName = firstName, lastName = lastName, customerGuid = customerGuid});
        }

        public CustomerContact GetCustomerContactByEmail(string email,Guid customerGuid)
        {
            var query = @"SELECT *
                        FROM [dbo].[CustomerContact]
                        WHERE EmailAddress = @emailAddress
                        AND CustomerGuid = @customerGuid";
            return _context.Connection.QueryFirstOrDefault<CustomerContact>(query, new { emailAddress = email, customerGuid = customerGuid });
        }

        public ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType)
        {
            var model = new List<KeyValuePairWithDescriptionModel<Guid, string, string>>();
            var sql =
                $@"SELECT CustomerContact.* FROM CustomerContact 
				join CustomerContactType on CustomerContact.ContactTypeGuid = CustomerContactType.ContactTypeGuid
				where CustomerContactType.ContactType = @ContactType and CustomerGuid = @CustomerId order by FirstName asc";
            var data = _context.Connection.Query<CustomerContact>(sql, new { ContactType = contactType, CustomerId = customerId });
            foreach (var item in data)
            {
                var fullName = FormatHelper.FormatFullName(item.FirstName, item.MiddleName, item.LastName);
                var values = FormatHelper.FormatAutoCompleteData(fullName, item.PhoneNumber);
                model.Add(new KeyValuePairWithDescriptionModel<Guid, string, string> { Keys = item.ContactGuid, Values = values, Descriptions = "" });
            }
            return model;
        }

    }
}
