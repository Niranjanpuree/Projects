using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.Sync;
using Northwind.Core.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;
using System.Threading;

namespace Northwind.Infrastructure.Data
{
    public class ActiveDirectoryGroupRepository : IActiveDirectoryGroupRepository
    {
        ActiveDirectoryContext _context;

        public ActiveDirectoryGroupRepository(ActiveDirectoryContext context)
        {
            _context = context;
        }

        public IEnumerable<Group> GetGroups()
        {
            var lstGroups = new List<Group>();
            //var directoryEntry = new DirectoryEntry(_context.LdapPath + _context.LdapDomainController, _context.LdapUsername, _context.LdapPassword);
            //DirectorySearcher dSearch = new DirectorySearcher(directoryEntry);
            //dSearch.Filter = "(&(objectClass=group))";
            //SearchResultCollection results = dSearch.FindAll();

            //for (int i = 0; i < results.Count; i++)
            //{                
            //    DirectoryEntry entry = results[i].GetDirectoryEntry();
            //    var group = new Group();
            //    group.GroupGuid = entry.Guid;
            //    group.ParentGuid = entry.Parent.Guid;
            //    group.GroupName = entry.Name.Replace("CN=","");
            //    group.CN = GetPropertyValue(entry, "CN");
            //    group.Description = GetPropertyValue(entry, "Description");
            //    lstGroups.Add(group);
            //}
            return lstGroups;
        }

        public IEnumerable<Group> GetGroupByCN(string cn)
        {
            var lstGroups = new List<Group>();
            var directoryEntry = new DirectoryEntry(_context.LdapPath + cn, _context.LdapUsername, _context.LdapPassword);
            DirectorySearcher dSearch = new DirectorySearcher(directoryEntry);
            SearchResultCollection results = dSearch.FindAll();

            for (int i = 0; i < results.Count; i++)
            {
                DirectoryEntry entry = results[i].GetDirectoryEntry();
                var group = new Group();
                group.GroupGuid = entry.Guid;
                group.ParentGuid = entry.Parent.Guid;
                group.GroupName = entry.Name.Replace("CN=", "");
                group.CN = GetPropertyValue(entry, "CN");
                group.Description = GetPropertyValue(entry, "Description");
                lstGroups.Add(group);
            }
            return lstGroups;
        }
        
        private string GetPropertyValue(DirectoryEntry entry, string propertyName)
        {
            if (entry.Properties.Contains(propertyName))
            {
                return entry.Properties[propertyName][0].ToString();
            }
            return null;
        }
    }
}
