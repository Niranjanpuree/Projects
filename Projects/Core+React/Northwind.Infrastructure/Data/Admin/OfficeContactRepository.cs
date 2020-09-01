using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;


namespace Northwind.Infrastructure.Data.Admin
{
    public class OfficeContactRepository : IOfficeContactRepository
    {
        IDatabaseContext _context;
        public OfficeContactRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int Add(OfficeContact officeContact)
        {
            string insertQuery = @"INSERT OfficeContact
                                                                   (
                                                                    contactGuid,
                                                                    FirstName, 
																	MiddleName,
																	LastName,
																	ContactType,
																	PhoneNumber,
																	AltPhoneNumber,
																	EmailAddress,
																	AltEmailAddress,
																	Description,
																	Address,
																	OfficeGuid,
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
																	@ContactType,
																	@PhoneNumber,
																	@AltPhoneNumber,
																	@EmailAddress,
																	@AltEmailAddress,
																	@Description,
																	@Address,
																	@OfficeGuid,
                                                                    @IsActive,
                                                                    @IsDeleted, 
                                                                    @CreatedOn, 
                                                                    @UpdatedOn, 
                                                                    @CreatedBy,
                                                                    @UpdatedBy
                                                                )";
            return _context.Connection.Execute(insertQuery, officeContact);
        }

        public int Delete(Guid[] ids)
        {
            foreach (var officeContactGuid in ids)
            {
                var officeContact = new
                {
                    OfficeContactGuid = officeContactGuid
                };
                string deleteQuery = @"Update OfficeContact set 
                                              IsDeleted   = 1
                                            where contactGuid =@contactGuid ";
                _context.Connection.Execute(deleteQuery, officeContact);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Disable(Guid[] ids)
        {
            foreach (var officeContactGuid in ids)
            {
                var officeContact = new
                {
                    OfficeContactGuid = officeContactGuid
                };
                string deleteQuery = @"Update OfficeContact set 
                                            IsActive   = 0
                                            where ContactGuid =@OfficeContactGuid ";
                _context.Connection.Execute(deleteQuery, officeContact);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Enable(Guid[] ids)
        {
            foreach (var officeContactGuid in ids)
            {
                var officeContact = new
                {
                    OfficeContactGuid = officeContactGuid
                };
                string deleteQuery = @"Update OfficeContact set 
                                            IsActive   = 1
                                            where ContactGuid =@OfficeContactGuid ";
                _context.Connection.Execute(deleteQuery, officeContact);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }
        public OfficeContact GetById(Guid id)
        {
            string sql = "SELECT * FROM OfficeContact WHERE ContactGuid = @OfficeContactGuid;";
            var result = _context.Connection.QuerySingle<OfficeContact>(sql, new { OfficeContactGuid = id });
            return result;
        }

        public OfficeContact GetDetailById(Guid contactGuid)
        {
            string sql = @"select   OfficeContact.ContactGuid,
                                    OfficeContact.FirstName,
									OfficeContact.MiddleName,
									OfficeContact.LastName,
									ContactType.ContactTypeName,
									OfficeContact.PhoneNumber,
									OfficeContact.AltPhoneNumber,
									OfficeContact.EmailAddress,
									OfficeContact.AltEmailAddress,
									OfficeContact.Description,
									OfficeContact.Address,
									OfficeContact.OfficeGuid,
									Office.OfficeName,
									OfficeContact.ContactType,
                                    OfficeContact.IsActive 
                                    from OfficeContact OfficeContact 
                                    left join
                                    CustomerContactType ContactType on
                                    ContactType.ContactTypeGuid = OfficeContact.ContactType 
									left join
									Office Office on
									Office.OfficeGuid = OfficeContact.OfficeGuid 
                            WHERE ContactGuid = @ContactGuid;";
            var result = _context.Connection.QuerySingle<OfficeContact>(sql, new { ContactGuid = contactGuid });
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

        public int TotalRecord(Guid officeGuid)
        {
            var officeGuids = new
            {
                OfficeGuid = officeGuid
            };
            string sql = "SELECT Count(1) FROM OfficeContact WHERE IsDeleted = 0 and OfficeGuid = @OfficeGuid";
            //var result = _context.Connection.Execute(sql, OfficeGuids);
            var result = _context.Connection.QuerySingle<int>(sql, officeGuids);
            return result;
        }

        public int Edit(OfficeContact officeContact)
        {
            //var contactGuid = OfficeContact.ContactGuid;
            string updateQuery = @"update OfficeContact set 
                                                                    contactGuid     = @contactGuid     ,
                                                                    FirstName       = @FirstName       ,
																	MiddleName      = @MiddleName      ,
																	LastName        = @LastName        ,
																	ContactType     = @ContactType     ,
																	PhoneNumber     = @PhoneNumber     ,
																	AltPhoneNumber  = @AltPhoneNumber ,
																	EmailAddress    = @EmailAddress    ,
																	AltEmailAddress = @AltEmailAddress,
																	Description     = @Description           ,
																	Address         = @Address           ,
																	OfficeGuid      = @OfficeGuid    ,
                                                                    UpdatedOn       = @UpdatedOn       ,
                                                                    UpdatedBy       = @UpdatedBy       
                                                                where ContactGuid = @contactGuid";
            return _context.Connection.Execute(updateQuery, officeContact);
        }

        public IEnumerable<OfficeContact> GetAll(string searchValue, Guid officeGuid, int pageSize, int skip, string sortField, string sortDirection)
        {
            StringBuilder orderingQuery = new StringBuilder();
            StringBuilder conditionalQuery = new StringBuilder();

            if (sortField.Equals("isActiveStatus"))
            {
                orderingQuery.Append($"OfficeContact.isActive {sortDirection}");  //Ambiguous if not done.. 
            }
            else
            {
                orderingQuery.Append($"{sortField} {sortDirection}");
            }

            if (!string.IsNullOrEmpty(searchValue) && searchValue != "undefined")
            {
                conditionalQuery.Append($"and FirstName like '%{searchValue.Trim()}%'");
                //conditionalQuery.Append($"and FirstName like '%{searchValue.Trim()}%' or middleName like '%{searchValue.Trim()}%' or LastName like '%{searchValue.Trim()}%'");
            }

            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                         (SELECT ROW_NUMBER() OVER (ORDER BY {orderingQuery}) AS RowNum, 
                                                                                        officeContact.contactGuid,
                                                                                        officeContact.FirstName,
                                                                                        officeContact.MiddleName,
                                                                                        officeContact.LastName,
                                                                                        officeContact.PhoneNumber,
                                                                                        officeContact.PhoneNumber ContactNumber,
                                                                                        officeContact.AltPhoneNumber,
                                                                                        officeContact.AltEmailAddress,
                                                                                        officeContact.EmailAddress,
																						officeContact.Description,
																						officeContact.Address,
																						contactType.ContactTypeName,
                                                                                        officeContact.IsActive,
                                                                                        officeContact.CreatedOn,
                                                                                        officeContact.CreatedBy,
                                                                                        officeContact.UpdatedOn,
                                                                                        officeContact.UpdatedBy 
                                                                                        from OfficeContact officeContact 
                                                                                        left join
                                                                                        CustomerContactType contactType on contactType.ContactTypeGuid = officeContact.ContactType
                                                                                        where officeContact.IsDeleted = 0 and officeContact.OfficeGuid = @OfficeGuid
                                                                                        {conditionalQuery}
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > {skip} 
                                            AND RowNum <= {pageSize + skip}  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<OfficeContact>(pagingQuery, new { OfficeGuid = officeGuid });
            return pagedData;
        }

        public IDictionary<Guid, string> GetContactType()
        {
            IDictionary<Guid, string> model = new Dictionary<Guid, string>();
            var data = _context.Connection.Query<Northwind.Core.Entities.CustomerContactType>("SELECT * FROM CustomerContactType Where isActive =1");
            foreach (var item in data)
            {
                model.Add(new KeyValuePair<Guid, string>(item.ContactTypeGuid, item.ContactTypeName));
            }
            return model;
        }
    }
}
