using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;

namespace Northwind.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository
    {
        
        public string BuildSql(BaseSearchSpec spec, out Dictionary<string, object> o)
        {
            var sql = string.Empty;
            var listOfAttributes = string.Join(",", spec.AttributesToReturn);
            var stringBuilder = new StringBuilder();

            Dictionary<string, object> dict = new Dictionary<string, object>();
            var entityName = typeof(T).Name;

            stringBuilder.AppendLine($"SELECT {listOfAttributes} FROM dbo.{entityName} ");
            if (spec.Criteria.Count > 0)
            {
                stringBuilder.AppendLine(" WHERE 1 = 1");
            }
            foreach (var criteria in spec.Criteria)
            {
                var criteriaBuilder = new List<string>();
                string newAppendedValue = string.Empty;
                List<string> newAppendedValueList = new List<string>();
                int i;
                stringBuilder.AppendLine(" and (");

                switch (criteria.Operator)
                {
                    case OperatorName.StringEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name}=@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", value.ToString());
                            ++i;
                        }
                        break;
                    case OperatorName.StringLike:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} like '%@{criteria.Attribute.Name}{i}%'");
                            dict.Add($"@{criteria.Attribute.Name}{i}", value.ToString());
                            ++i;
                        }
                        break;
                    case OperatorName.StringNotEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} not like '%@{criteria.Attribute.Name}{i}%'");
                            dict.Add($"@{criteria.Attribute.Name}{i}", value.ToString());
                            ++i;
                        }
                        break;

                    case OperatorName.StringIn:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            newAppendedValueList.Add("@" + criteria.Attribute.Name + i);
                            dict.Add($"@{criteria.Attribute.Name}{i}", value.ToString());
                            i++;
                        }
                        newAppendedValue = string.Join(",", newAppendedValueList);
                        criteriaBuilder.Add($"{criteria.Attribute.Name} in ({newAppendedValue})");
                        break;

                    case OperatorName.DateTimeBetween:
                        criteriaBuilder.Add($"{criteria.Attribute.Name}  between @{criteria.Attribute.Name}From  and  @{criteria.Attribute.Name}To ");
                        dict.Add($"@{criteria.Attribute.Name}From", DataConversion.ParseDate(Convert.ToString(criteria.Value.First()), DateTime.Now));
                        dict.Add($"@{criteria.Attribute.Name}To", DataConversion.ParseDate(Convert.ToString(criteria.ValueToCompare), DateTime.Now));
                        break;
                    case OperatorName.DateTimeEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name}=@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDate(Convert.ToString(value), DateTime.Now));
                            ++i;
                        }
                        break;
                    case OperatorName.DateTimeGreaterThan:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} >@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDate(Convert.ToString(value), DateTime.Now));
                            ++i;
                        }
                        break;
                    case OperatorName.DateTimeGreaterThanOrEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} >=@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDate(Convert.ToString(value), DateTime.Now));
                            ++i;
                        }
                        break;
                    case OperatorName.DateTimeLessThan:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} <@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDate(Convert.ToString(value), DateTime.Now));
                            ++i;
                        }
                        break;
                    case OperatorName.DateTimeLessThanOrEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} <=@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDate(Convert.ToString(value), DateTime.Now));
                            ++i;
                        }
                        break;
                    case OperatorName.DateTimeNotEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} <> @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDate(Convert.ToString(value), DateTime.Now));
                            ++i;
                        }
                        break;
                    case OperatorName.DateTimeIn:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            newAppendedValueList.Add("@" + criteria.Attribute.Name + i);
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDate(Convert.ToString(value), DateTime.Now));
                            i++;
                        }
                        newAppendedValue = string.Join(",", newAppendedValueList);
                        criteriaBuilder.Add($"{criteria.Attribute.Name} in ({newAppendedValue})");
                        break;
                    case OperatorName.IntegerEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name}=@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseInt(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.IntegerGreaterThan:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} > @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseInt(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.IntegerGreaterThanOrEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} >= @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseInt(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.IntegerLessThan:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} < @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseInt(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.IntegerLessThanorEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} <= @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseInt(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.IntegerNotEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} <> @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseInt(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.IntegerIn:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            newAppendedValueList.Add("@" + criteria.Attribute.Name + i);
                            dict.Add($"{criteria.Attribute.Name}{i}", DataConversion.ParseInt(value, 0));
                            i++;
                        }
                        newAppendedValue = string.Join(",", newAppendedValueList);
                        criteriaBuilder.Add($"@{criteria.Attribute.Name} in ({newAppendedValue})");
                        break;
                    case OperatorName.DecimalEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name}=@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDec(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.DecimalGreaterThan:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} > @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDec(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.DecimalGreaterThanOrEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} >= @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDec(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.DecimalLessThan:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} < @{criteria.Attribute.Name}{i}");
                            dict.Add($"{criteria.Attribute.Name}{i}", value.ToString());
                            ++i;
                        }
                        break;
                    case OperatorName.DecimalLessThanOrEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} <= @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDec(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.DecimalNotEquals:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name} <> @{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDec(value, 0));
                            ++i;
                        }
                        break;
                    case OperatorName.DecimalIn:
                        i = 0;
                        foreach (var value in criteria.Value)
                        {
                            newAppendedValueList.Add("@" + criteria.Attribute.Name + i);
                            dict.Add($"@{criteria.Attribute.Name}{i}", DataConversion.ParseDec(value, 0));
                            i++;
                        }
                        newAppendedValue = string.Join(",", newAppendedValueList);
                        criteriaBuilder.Add($"{criteria.Attribute.Name} in ({newAppendedValue})");
                        break;
                    default:
                        break;
                }
                stringBuilder.Append(string.Join(" OR ", criteriaBuilder));
                stringBuilder.AppendLine(")");
            }
            o = dict;
            return stringBuilder.ToString();
        }

    }
}
