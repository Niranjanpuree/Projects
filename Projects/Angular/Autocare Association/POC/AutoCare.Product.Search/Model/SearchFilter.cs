using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoCare.Product.Infrastructure;

namespace AutoCare.Product.Search.Model
{
    public class SearchFilter
    {
        public string ColumnName { get; set; }
        public IList<string> Values { get; set; }
        public FilterBy FilterBy { get; set; }

        public override string ToString()
        {
            switch (FilterBy)
            {
                case FilterBy.Any:
                    return FilterByAll(ColumnName, Values);
                case FilterBy.All:
                    return FilterByAny(ColumnName, Values);
                default:
                    return null;
            }
        }

        private string FilterByAll(string columnName, IList<string> values)
        {
            if (String.IsNullOrWhiteSpace(ColumnName) || values == null || !values.Any())
            {
                return null;
            }

            var filterText = new StringBuilder();
            foreach (var value in values)
            {
                if (String.IsNullOrWhiteSpace(filterText.ToString()))
                {
                    filterText.Append($"{ColumnName.ToCamelCase()} eq {value}");
                }

                filterText.Append($" and {ColumnName.ToCamelCase()} eq {value}");
            }

            return filterText.ToString();
        }

        private string FilterByAny(string columnName, IList<string> values)
        {
            if (String.IsNullOrWhiteSpace(ColumnName) || Values == null || !Values.Any())
            {
                return null;
            }

            var filterText = new StringBuilder();
            foreach (var value in values)
            {
                if (String.IsNullOrWhiteSpace(filterText.ToString()))
                {
                    filterText.Append($"{ColumnName.ToCamelCase()} eq {value}");
                }

                filterText.Append($" or {ColumnName.ToCamelCase()} eq {value}");
            }

            return filterText.ToString();
        }
    }
}