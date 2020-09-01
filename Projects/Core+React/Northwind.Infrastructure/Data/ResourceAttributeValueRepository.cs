using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data
{
    public class ResourceAttributeValueRepository : IResourceAttributeValueRepository
    {
        IDatabaseContext _context;
        public ResourceAttributeValueRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<ResourceAttributeValue> GetResourceAttributeOptionsByResourceAttributeGuid(Guid resourceAttributeGuid)
        {
            var sql = "SELECT * FROM ResourceAttributeValue WHERE ResourceAttributeGuid = @value order by name asc";
            return _context.Connection.Query<ResourceAttributeValue>(sql, new { value = resourceAttributeGuid });
        }

        public ResourceAttributeValue GetResourceAttributeValueByValue(string value)
        {
            var sql = "SELECT * FROM ResourceAttributeValue WHERE Value = @value";
            return _context.Connection.QueryFirstOrDefault<ResourceAttributeValue>(sql, new { value = value });
        }

        public ResourceAttributeValue GetResourceAttributeValueByName(string name)
        {
            var sql = "SELECT * FROM ResourceAttributeValue WHERE Name = @name";
            return _context.Connection.QueryFirstOrDefault<ResourceAttributeValue>(sql, new { name = name });
        }

        public IEnumerable<ResourceAttributeValue> GetResourceValuesByResourceType(string resourceType, string name)
        {
            var sql = "SELECT value.* FROM ResourceAttributeValue value INNER JOIN ResourceAttribute attr on attr.ResourceAttributeGuid = value.ResourceAttributeGuid WHERE attr.ResourceType = @ResourceType and attr.Name = @Name order by name asc";
            return _context.Connection.Query<ResourceAttributeValue>(sql, new { ResourceType = resourceType, Name = name });
        }


        public IEnumerable<ResourceAttributeValue> GetResourceValuesByValue(string value)
        {
            var sql = "SELECT * FROM ResourceAttributeValue WHERE Value = @value order by name asc";
            return _context.Connection.Query<ResourceAttributeValue>(sql, new { value = value });
        }

        public ResourceAttributeValue GetResourceAttributeValueByResourceTypeNameValue(string resourceType, string attributeName, string nameValue)
        {
            var query = @"SELECT value.* FROM ResourceAttributeValue value 
                        INNER JOIN ResourceAttribute attr 
                        ON attr.ResourceAttributeGuid = value.ResourceAttributeGuid 
                        WHERE attr.ResourceType = @resourceType 
                        AND attr.Name = @attributeName 
                        AND value.Name = @nameValue";
            return _context.Connection.QueryFirstOrDefault<ResourceAttributeValue>(query, new { resourceType = resourceType, attributeName = attributeName, nameValue = nameValue });
        }
    }
}
