using Dapper;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Northwind.Infrastructure.Data
{
    public class AdvancedSearchQueryBuilder
    {
        
        List<AdvancedSearchRequest> _postValue = null;
        List<ComplexAdvancedSearchRequest> _additional = null;
        string _prefix = "";
        public AdvancedSearchQueryBuilder(List<AdvancedSearchRequest> postValue, string prefix="")
        {
            _postValue = postValue;
            if(prefix != "")
                _prefix = prefix + ".";
        }

        public void SetComplexSearchOption(List<ComplexAdvancedSearchRequest> additional)
        {
            _additional = additional;
        }

        public List<dynamic> getQuery()
        {
            var lst = new List<dynamic>();
            foreach(var item in _postValue)
            {
                lst.Add(getFieldQuery(item));
            }
            return lst;
        }

        public List<dynamic> getComplexQuery()
        {
            var lst = new List<dynamic>();
            var index = 0;
            if (_additional != null)
            {
                foreach (var more in _additional)
                {
                    lst.Add(getComplexFieldQuery(more, index));
                    index++;
                }
            }
            return lst;
        }

        private dynamic getComplexFieldQuery(ComplexAdvancedSearchRequest item, int index)
        {

            OperatorName op = (OperatorName)item.SearchRequest.Operator.OperatorName;
            switch (op)
            {
                case OperatorName.ComboEquals:
                    return ComboEqualsOperator(item, index);
                case OperatorName.ComboIn:
                    return ComboInOperator(item, index);
                //case OperatorName.DateTimeBetween:
                //    return DateTimeBetweenOperator(item);
                //case OperatorName.DateTimeEquals:
                //    return DateTimeEqualsOperator(item);
                //case OperatorName.DateTimeGreaterThan:
                //    return DateTimeGreaterThanOperator(item);
                //case OperatorName.DateTimeGreaterThanOrEquals:
                //    return DateTimeGreaterThanOrEqualsOperator(item);
                //case OperatorName.DateTimeIn:
                //    return DateTimeInOperator(item);
                //case OperatorName.DateTimeLessThan:
                //    return DateTimeLessThanOperator(item);
                //case OperatorName.DateTimeLessThanOrEquals:
                //    return DateTimeLessThanOrEqualsOperator(item);
                //case OperatorName.DateTimeNotEquals:
                //    return DateTimeNotEqualsOperator(item);
                //case OperatorName.DecimalBetween:
                //    return DecimalBetweenOperator(item);
                //case OperatorName.DecimalEquals:
                //    return DecimalEqualsOperator(item);
                //case OperatorName.DecimalGreaterThan:
                //    return DecimalGreaterThanOperator(item);
                //case OperatorName.DecimalGreaterThanOrEquals:
                //    return DecimalGreaterThanOrEqualsOperator(item);
                //case OperatorName.DecimalIn:
                //    return DecimalInOperator(item);
                //case OperatorName.DecimalLessThan:
                //    return DecimalLessThanOperator(item);
                //case OperatorName.DecimalLessThanOrEquals:
                //    return DecimalLessThanOrEqualsOperator(item);
                //case OperatorName.DecimalNotEquals:
                //    return DecimalNotEqualsOperator(item);
                //case OperatorName.IntegerBetween:
                //    return IntegerBetweenOperator(item);
                //case OperatorName.IntegerEquals:
                //    return IntegerEqualsOperator(item);
                //case OperatorName.IntegerGreaterThan:
                //    return IntegerGreaterThanOperator(item);
                //case OperatorName.IntegerGreaterThanOrEquals:
                //    return IntegerGreaterThanOrEqualsOperator(item);
                //case OperatorName.IntegerIn:
                //    return IntegerInOperator(item);
                //case OperatorName.IntegerLessThan:
                //    return IntegerLessThanOperator(item);
                //case OperatorName.IntegerLessThanorEquals:
                //    return IntegerLessThanorEqualsOperator(item);
                //case OperatorName.IntegerNotEquals:
                //    return IntegerNotEqualsOperator(item);
                //case OperatorName.MultiCheckboxEquals:
                //    return MultiCheckboxEqualsOperator(item);
                //case OperatorName.MultiCheckboxIn:
                //    return MultiCheckboxInOperator(item);
                //case OperatorName.StringEquals:
                //    return StringEqualsOperator(item);
                //case OperatorName.StringGreaterThan:
                //    return StringGreaterThanOperator(item);
                //case OperatorName.StringIn:
                //    return StringInOperator(item);
                //case OperatorName.StringLessThan:
                //    return StringLessThanOperator(item);
                //case OperatorName.StringLike:
                //    return StringLikeOperator(item);
                //case OperatorName.StringNotEquals:
                //    return StringNotEqualsOperator(item);
                //case OperatorName.RadioEquals:
                //    return RadioEqualsOperator(item);
                default:
                    return "";
            }
        }

        

        #region "complex query"
        private dynamic ComboEqualsOperator(ComplexAdvancedSearchRequest item, int index)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.SearchRequest.Operator.OperatorType;
            eo.TryAdd<string, object>($"{item.SearchRequest.Attribute.AttributeName}{index}", ((JObject)item.SearchRequest.Value).Value<string>("value"));
            string extraCondition = "";
            if (item.Prefix != "")
                item.Prefix += ".";

            foreach (var cond in item.AdditionalCondition)
            {
                extraCondition += $" AND {item.Prefix}{cond.Key}=@{cond.Key}{index}";
                eo.TryAdd<string, object>($"{cond.Key}{index}", cond.Value);
            }

            return new
            {
                sql = $"({item.Prefix}{item.SearchRequest.Attribute.AttributeName}=@{item.SearchRequest.Attribute.AttributeName}{index} {extraCondition})",
                value = eo
            };
        }

        private dynamic ComboInOperator(ComplexAdvancedSearchRequest item, int index)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.SearchRequest.Operator.OperatorType;
            if (item.Prefix != "")
                item.Prefix += ".";

            var lst = new List<string>();
            try
            {
                JArray array = (JArray)item.SearchRequest.Value;
                foreach (var ar in array)
                {
                    lst.Add(ar.Value<string>("value"));
                }
            }
            catch (Exception)
            {
                var obj = (JObject)item.SearchRequest.Value;
                lst.Add(obj.Value<string>("value"));
            }


            eo.TryAdd<string, object>($"{item.SearchRequest.Attribute.AttributeName}{index}", lst);

            string extraCondition = "";
            foreach (var cond in item.AdditionalCondition)
            {
                extraCondition += $" AND {item.Prefix}{cond.Key}=@{cond.Key}{index}";
                eo.TryAdd<string, object>($"{cond.Key}{index}", cond.Value);
            }

            return new
            {
                sql = $"({item.Prefix}{item.SearchRequest.Attribute.AttributeName} in @{item.SearchRequest.Attribute.AttributeName}{index} {extraCondition})",
                value = eo
            };
        }
        #endregion

        #region "simple query"

        private dynamic getFieldQuery(AdvancedSearchRequest item)
        {

            OperatorName op = (OperatorName)item.Operator.OperatorName;
            switch (op)
            {
                case OperatorName.ComboEquals:
                    return ComboEqualsOperator(item);
                case OperatorName.ComboIn:
                    return ComboInOperator(item);
                case OperatorName.DateTimeBetween:
                    return DateTimeBetweenOperator(item);
                case OperatorName.DateTimeEquals:
                    return DateTimeEqualsOperator(item);
                case OperatorName.DateTimeGreaterThan:
                    return DateTimeGreaterThanOperator(item);
                case OperatorName.DateTimeGreaterThanOrEquals:
                    return DateTimeGreaterThanOrEqualsOperator(item);
                case OperatorName.DateTimeIn:
                    return DateTimeInOperator(item);
                case OperatorName.DateTimeLessThan:
                    return DateTimeLessThanOperator(item);
                case OperatorName.DateTimeLessThanOrEquals:
                    return DateTimeLessThanOrEqualsOperator(item);
                case OperatorName.DateTimeNotEquals:
                    return DateTimeNotEqualsOperator(item);
                case OperatorName.DecimalBetween:
                    return DecimalBetweenOperator(item);
                case OperatorName.DecimalEquals:
                    return DecimalEqualsOperator(item);
                case OperatorName.DecimalGreaterThan:
                    return DecimalGreaterThanOperator(item);
                case OperatorName.DecimalGreaterThanOrEquals:
                    return DecimalGreaterThanOrEqualsOperator(item);
                case OperatorName.DecimalIn:
                    return DecimalInOperator(item);
                case OperatorName.DecimalLessThan:
                    return DecimalLessThanOperator(item);
                case OperatorName.DecimalLessThanOrEquals:
                    return DecimalLessThanOrEqualsOperator(item);
                case OperatorName.DecimalNotEquals:
                    return DecimalNotEqualsOperator(item);
                case OperatorName.IntegerBetween:
                    return IntegerBetweenOperator(item);
                case OperatorName.IntegerEquals:
                    return IntegerEqualsOperator(item);
                case OperatorName.IntegerGreaterThan:
                    return IntegerGreaterThanOperator(item);
                case OperatorName.IntegerGreaterThanOrEquals:
                    return IntegerGreaterThanOrEqualsOperator(item);
                case OperatorName.IntegerIn:
                    return IntegerInOperator(item);
                case OperatorName.IntegerLessThan:
                    return IntegerLessThanOperator(item);
                case OperatorName.IntegerLessThanorEquals:
                    return IntegerLessThanorEqualsOperator(item);
                case OperatorName.IntegerNotEquals:
                    return IntegerNotEqualsOperator(item);
                case OperatorName.MultiCheckboxEquals:
                    return MultiCheckboxEqualsOperator(item);
                case OperatorName.MultiCheckboxIn:
                    return MultiCheckboxInOperator(item);
                case OperatorName.StringEquals:
                    return StringEqualsOperator(item);
                case OperatorName.StringGreaterThan:
                    return StringGreaterThanOperator(item);
                case OperatorName.StringIn:
                    return StringInOperator(item);
                case OperatorName.StringLessThan:
                    return StringLessThanOperator(item);
                case OperatorName.StringLike:
                    return StringLikeOperator(item);
                case OperatorName.StringNotEquals:
                    return StringNotEqualsOperator(item);
                case OperatorName.RadioEquals:
                    return RadioEqualsOperator(item);
                case OperatorName.StringLikeStartWith:
                    return StringLikeOperatorStartWith(item);
                case OperatorName.IsNull:
                    return StringIsNull(item);
                case OperatorName.IsNotNull:
                    return StringIsNotNull(item);
                default:
                    return "";
            }
        }

        private dynamic RadioEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            var data = (JObject)item.Value;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", data.Value<string>("value"));
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] = @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringNotEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] <> @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringLikeOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", $"%{item.Value}%");
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] LIKE @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringIsNull(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", $"{item.Value}");
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringIsNotNull(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", $"{item.Value}");
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringLikeOperatorStartWith(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", $"{item.Value}%");
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] LIKE @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringLessThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] < @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringInOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] In @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringGreaterThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] > @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic StringEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] = @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic MultiCheckboxInOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] = @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic MultiCheckboxEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            if(item.Value.GetType() == typeof(JObject))
            {
                var jObj = (JObject)item.Value;
                eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", jObj.Value<string>("value"));
            }
            else
            {
                eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            }
            
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] = @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerNotEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] <> @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerLessThanorEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] <= @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerLessThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] < @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerInOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] In @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerGreaterThanOrEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] >= @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerGreaterThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] > @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] = @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic IntegerBetweenOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}Value1", item.Value);
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}Value2", item.Value2);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] between @{formatQueryParameter(item.Attribute.AttributeName)}Value1 and @{formatQueryParameter(item.Attribute.AttributeName)}Value2",
                value = eo
            };
        }

        private dynamic DecimalNotEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] <> @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DecimalLessThanOrEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] <= @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DecimalLessThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] < @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DecimalInOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] in @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DecimalGreaterThanOrEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}]>=@{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DecimalGreaterThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] > @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DecimalEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}]=@{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DecimalBetweenOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}Value1", item.Value);
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}Value2", item.Value2);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] between @{formatQueryParameter(item.Attribute.AttributeName)}Value1 and @{formatQueryParameter(item.Attribute.AttributeName)}Value2",
                value = eo
            };
        }


        private dynamic ComboEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", ((JObject)item.Value).Value<string>("value"));
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}]=@{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic ComboInOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            var lst = new List<string>();
            try
            {
                JArray array = (JArray)item.Value;
                foreach (var ar in array)
                {
                    lst.Add(ar.Value<string>("value"));
                }
            }
            catch (Exception)
            {
                var obj = (JObject)item.Value;
                lst.Add(obj.Value<string>("value"));
            }
            
            
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", lst);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] in @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DateTimeBetweenOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}Value1", item.Value);
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}Value2", item.Value2);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] between @{formatQueryParameter(item.Attribute.AttributeName)}Value1 and @{formatQueryParameter(item.Attribute.AttributeName)}Value2",
                value = eo
            };
        }

        private dynamic DateTimeEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"CONVERT(DATE,{_prefix}[{item.Attribute.AttributeName}],101) = CONVERT(DATE,@{formatQueryParameter(item.Attribute.AttributeName)},101)",
                value = eo
            };
        }

        private dynamic DateTimeGreaterThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] > @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DateTimeGreaterThanOrEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"CONVERT(DATE,{_prefix}[{item.Attribute.AttributeName}],101) >= CONVERT(DATE,@{formatQueryParameter(item.Attribute.AttributeName)},101)",
                value = eo
            };
        }

        private dynamic DateTimeInOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] in @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DateTimeLessThanOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] < @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }

        private dynamic DateTimeLessThanOrEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"CONVERT(DATE,{_prefix}[{item.Attribute.AttributeName}],101) <= CONVERT(DATE,@{formatQueryParameter(item.Attribute.AttributeName)},101)",
                value = eo
            };
        }

        private dynamic DateTimeNotEqualsOperator(AdvancedSearchRequest item)
        {
            var eo = new ExpandoObject();
            OperatorName op = (OperatorName)item.Operator.OperatorType;
            eo.TryAdd<string, object>($"{formatQueryParameter(item.Attribute.AttributeName)}", item.Value);
            return new
            {
                sql = $"{_prefix}[{item.Attribute.AttributeName}] <> @{formatQueryParameter(item.Attribute.AttributeName)}",
                value = eo
            };
        }
        #endregion

        private string formatQueryParameter(string name)
        {
            return name.Replace(".", "").Replace("-", "");
        }
    }

    public class ComplexAdvancedSearchRequest
    {
    
        public ComplexAdvancedSearchRequest(AdvancedSearchRequest searchRequest, string prefix, Dictionary<string, object> additional)
        {
            this._searchRequest = searchRequest;
            this._prefix = prefix;
            this._AdditionalCondition = additional;
        }

        private AdvancedSearchRequest _searchRequest;
        private string _prefix;
        private Dictionary<string, object> _AdditionalCondition;

        public AdvancedSearchRequest SearchRequest
        {
            get { return _searchRequest; }
            set { _searchRequest = value; }
        }
        
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        public Dictionary<string, object> AdditionalCondition
        {
            get { return _AdditionalCondition; }
            set { _AdditionalCondition = value; }
        }


    }
}
