using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Northwind.Core.Import.Helper
{
    public class MapperHelper
    {
        private static string[] trueBooleanArray = { "1", "yes", "true", "y" };
        private string[] falseBooleanArray = { "0", "no", "false", "n" };
        public static object MapObjectToEntity(object contract, object entity)
        {
            foreach (PropertyInfo property in contract.GetType().GetProperties())
            {
                if (property.PropertyType != typeof(bool))
                {
                    var value = property.GetValue(contract);
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        PropertyInfo propertyInfo = entity.GetType().GetProperty(property.Name);
                        if (propertyInfo != null)
                        {
                            if (propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(Nullable<decimal>))
                            {
                                var decimalValue = Decimal.Parse(value.ToString());
                                propertyInfo.SetValue(entity, decimalValue, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(Nullable<int>))
                            {
                                var intValue = Int32.Parse(value.ToString());
                                propertyInfo.SetValue(entity, intValue, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(Nullable<DateTime>))
                            {
                                var dateTimeValue = DateTime.Parse(value.ToString());
                                propertyInfo.SetValue(entity, dateTimeValue, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(bool))
                            {
                                var boolValue = false;
                                if (trueBooleanArray.Contains(value.ToString().Trim().ToLower()))
                                    boolValue = true;
                                propertyInfo.SetValue(entity, boolValue, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(Guid) || propertyInfo.PropertyType == typeof(Nullable<Guid>))
                            {
                                var guid = Guid.Parse(value.ToString());
                                if (guid != Guid.Empty)
                                    propertyInfo.SetValue(entity, guid, null);
                            }
                            else if (value is IList && value.GetType().IsGenericType)
                            {
                                propertyInfo.SetValue(entity, value, null);
                            }
                            else if (value is Core.Entities.ContractRefactor.Contracts)
                            {
                                continue;
                            }
                            else
                            {
                                propertyInfo.SetValue(entity, value.ToString().Trim(), null);
                            }
                        }
                    }
                }
            }
            return entity;
        }

        public static Dictionary<string, string> CreateDataObjectFromObject(object dataObject)
        {
            var dynamicObject = new Dictionary<string, string>();
            foreach (PropertyInfo property in dataObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dataObject);
                    if (!string.IsNullOrEmpty(value))
                    {
                        dynamicObject.Add(property.Name, value);
                    }
                }
            }
            return dynamicObject;
        }

        public static Dictionary<string, string> CreateObjectWithValueFromObject(object dataObject, Dictionary<string, string> header)
        {
            var _header = new Dictionary<string, string>();

            foreach (var item in header)
            {
                _header.Add(item.Key, item.Value);
            }
            var dynamicObject = new Dictionary<string, string>();
            foreach (PropertyInfo property in dataObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dataObject);
                    //if (!string.IsNullOrEmpty(value))
                    //{
                    //    dynamicObject.Add(property.Name, value);
                    //}
                    if (header.ContainsKey(property.Name))
                    {
                        dynamicObject.Add(property.Name, value);
                        _header[property.Name] = value;
                    }
                }
            }
            return _header;
        }

        public static List<object> GetExportList<T>(IList<T> dataList, Dictionary<string, string> header)
        {
            var exportList = new List<object>();
            dynamic objectList = new List<dynamic>();
            foreach (var contract in dataList)
            {
                var prop = new ExpandoObject() as IDictionary<string, Object>;
                var contractObject = CreateObjectWithValueFromObject(contract, header);

                IDictionary<string, string> dictionary = new Dictionary<string, string>();
                objectList.Add(new ExpandoObject());
                foreach (var item in contractObject)
                {
                    prop.TryAdd<string, Object>(item.Key, item.Value);
                }
                exportList.Add(prop);
            }
            return exportList;
        }

        public static object SetDefaultValueToNullProperty(object dataObject)
        {
            foreach (PropertyInfo property in dataObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = (string)property.GetValue(dataObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        property.SetValue(dataObject, "");
                    }
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    string value = (string)property.GetValue(dataObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        property.SetValue(dataObject, DateTime.UtcNow);
                    }
                }
            }
            return dataObject;
        }
    }
}
