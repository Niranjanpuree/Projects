using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;


namespace Northwind.Infrastructure
{
    public class DropDownListHelper
    {
        IDatabaseContext _context;

        public DropDownListHelper(IDatabaseContext context)
        {
            _context = context;
        }

        public ICollection<KeyValuePairModel<Guid, string>> GetCustomerTypeDropDown()
        {
            var model = new List<KeyValuePairModel<Guid, string>>();
            var data = _context.Connection.Query<CustomerType>("SELECT * FROM CustomerType");
            foreach (var item in data)
            {
                model.Add(new KeyValuePairModel<Guid, string> { Keys = item.CustomerTypeId, Values = item.CustomerTypeName });
            }
            return model;
        }
        public ICollection<KeyValuePairModel<Guid, string>> GetCountryDropDown()
        {
            var model = new List<KeyValuePairModel<Guid, string>>();
            var data = _context.Connection.Query<Country>("SELECT * FROM Country");
            foreach (var item in data)
            {
                model.Add(new KeyValuePairModel<Guid, string> { Keys = item.CountryId, Values = item.CountryName });
            }

            return model;
        }
        public ICollection<KeyValuePairModel<Guid, string>> GetStateDropDown()
        {
            var model = new List<KeyValuePairModel<Guid, string>>();
            var data = _context.Connection.Query<States>("SELECT * FROM States");
            foreach (var item in data)
            {
                model.Add(new KeyValuePairModel<Guid, string> { Keys = item.StatesId, Values = item.StatesName });
            }
            return model;
        }
        public ICollection<KeyValuePairModel<bool, string>> GetIsActiveDropDown()
        {
            var model = new List<KeyValuePairModel<bool, string>>();
            model.Add(new KeyValuePairModel<bool, string> { Keys = true, Values = "True" });
            model.Add(new KeyValuePairModel<bool, string> { Keys = false, Values = "False" });
            return model;
        }
    }
}
