using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Infrastructure.Data.UsCustomerOffice
{
    public class UsCustomerOfficeListRepository : IUsCustomerOfficeListRepository
    {
        private readonly IDatabaseContext _context;

        public UsCustomerOfficeListRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<Core.Entities.UsCustomerOfficeList> GetDistinctCustomerName()
        {
            var data = _context.Connection.Query<Core.Entities.UsCustomerOfficeList>("select distinct(CUSTOMERNAME) from UsCustomerOfficeList order by CUSTOMERNAME");
            return data;
        }

        public IEnumerable<UsCustomerOfficeList> GetDistinctCustomerNameByDepartment(string department)
        {
            var data = _context.Connection.Query<UsCustomerOfficeList>(@"SELECT distinct(CUSTOMERNAME) from UsCustomerOfficeList
                                                                        WHERE DEPARTMENTNAME = @DEPARTMENTNAME",
                                                                     new { DEPARTMENTNAME = department });
            return data;
        }

        public IEnumerable<Core.Entities.UsCustomerOfficeList> GetUsCustomerOfficeDepartmentList()
        {
            var data = _context.Connection.Query<Core.Entities.UsCustomerOfficeList>("SELECT distinct(DEPARTMENTNAME) FROM UsCustomerOfficeList order by DEPARTMENTNAME");
            return data;
        }

        public IEnumerable<UsCustomerOfficeList> GetUsCustomerOfficeDepartmentListBySixDigitCode(string sixDigitCode)
        {
            var data = _context.Connection.Query<Core.Entities.UsCustomerOfficeList>("SELECT * FROM UsCustomerOfficeList where ContractingOfficeCode = @sixDigitCode order by DEPARTMENTNAME",new { sixDigitCode= sixDigitCode });
            return data;
        }

        public UsCustomerOfficeList GetCustomerOfficeByContractingOfficeName(string contractingOfficeName)
        {
            var query = @"SELECT * FROM UsCustomerOfficeList
                            WHERE ContractingOfficeName = @officeName";
            return _context.Connection.QueryFirstOrDefault<UsCustomerOfficeList>(query, new { officeName = contractingOfficeName });
        }
    }
}
