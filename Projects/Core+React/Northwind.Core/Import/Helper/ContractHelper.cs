using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Northwind.Core.Import.Helper
{
    public class ContractHelper
    {
        private static string[] trueBooleanArray = { "1", "yes", "true","y" };
        private string[] falseBooleanArray = { "0", "no", "false","n" };
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

        public static Dictionary<string, string> CreateObjectWithValueFromObject(object dataObject,Dictionary<string,string> header)
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

        public static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                object newA = Convert.ChangeType(item.Value, newT);
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;
        }

        public static Contracts MapObjectToContracts(object contract, Contracts contractEntity)
        {
            foreach (PropertyInfo property in contract.GetType().GetProperties())
            {
                if (property.PropertyType != typeof(bool))
                {
                    var value = property.GetValue(contract);
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        PropertyInfo propertyInfo = contractEntity.GetType().GetProperty(property.Name);
                        if (propertyInfo != null)
                        {
                            if (propertyInfo.PropertyType == typeof(Nullable<decimal>))
                            {
                                var decimalValue = Decimal.Parse(value.ToString());
                                propertyInfo.SetValue(contractEntity, decimalValue, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(Nullable<DateTime>))
                            {
                                var dateTimeValue = DateTime.Parse(value.ToString());
                                propertyInfo.SetValue(contractEntity, dateTimeValue, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(bool))
                            {
                                var boolValue = false;
                                if (trueBooleanArray.Contains(value.ToString().Trim().ToLower()))
                                    boolValue = true;
                                propertyInfo.SetValue(contractEntity, boolValue, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(Guid))
                            {
                                var guid = Guid.Parse(value.ToString());
                                if (guid != Guid.Empty)
                                    propertyInfo.SetValue(contractEntity, guid, null);
                            }
                            //else if (propertyInfo.PropertyType == typeof(List<ContractUserRole>))
                            //{
                            //    var roleList = List<ContractUserRole>()property.GetValue(contract);
                            //}
                            else
                            {
                                propertyInfo.SetValue(contractEntity, value, null);
                            }


                        }
                    }
                }
            }
            return contractEntity;
        }

        //public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        //{
        //    //Take use of the IDictionary implementation
        //    var expandoDict = expando as IDictionary<string,object>;
        //    if (expandoDict.ContainsKey(propertyName))
        //        expandoDict[propertyName] = propertyValue;
        //    else
        //        expandoDict.Add(propertyName, propertyValue);
        //}

        public static ExpandoObject CreateExpandoFromObject(object source)
        {
            var obj = new ExpandoObject();
            IDictionary<string, object> dictionary = obj;
            foreach (var property in source
                .GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.GetMethod.IsPublic))
            {
                var value = property.GetValue(source, null);
                if(value != null)
                    dictionary[property.Name] = property.GetValue(source, null);
            }
            return obj;
        }
    }
}
