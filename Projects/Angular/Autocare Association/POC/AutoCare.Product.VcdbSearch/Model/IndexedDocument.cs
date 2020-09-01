using System;
using System.Linq.Expressions;

namespace AutoCare.Product.VcdbSearchService.Model
{
    public class IndexedDocument<T>
    {
        public static string QueryToExactMatch<TProp, TMatch>(Expression<Func<T, TProp>> propertyExpression,
            TMatch valueToMatch)
        {
            var memberEx = propertyExpression.Body as MemberExpression;
            if (memberEx == null)
            {
                return null;
            }

            return $"{memberEx.Member.Name} eq {valueToMatch}";
        }
    }
}
