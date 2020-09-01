using System;
using System.Linq;
using System.Linq.Expressions;

namespace AutoCare.Product.Infrastructure
{
    public static class AzureExtensions
    {
        private static Type _azureDocumentType = null;
        public static string ToAzureSearchFilter(this LambdaExpression expression)
        {
            _azureDocumentType = null;
            if (expression == null)
            {
                return null;
            }

            if (!expression.Parameters.Any() || expression.Parameters.Count > 1)
            {
                throw new ArgumentException("We support lambda expression with only one parameter");
            }

            _azureDocumentType = expression.Parameters[0].Type;

            var binaryExpression = expression.Body as BinaryExpression;
            if (binaryExpression == null)
            {
                throw new ArgumentException("Lambda Expression body must be binary expressions");
            }

            return ParseExpression(binaryExpression);
        }

        private static string ParseExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    return ParseExpressionAndBuildQuery((BinaryExpression)expression, "eq");
                case ExpressionType.NotEqual:
                    return ParseExpressionAndBuildQuery((BinaryExpression)expression, "ne");
                case ExpressionType.GreaterThan:
                    return ParseExpressionAndBuildQuery((BinaryExpression)expression, "gt");
                case ExpressionType.GreaterThanOrEqual:
                    return ParseExpressionAndBuildQuery((BinaryExpression)expression, "ge");
                case ExpressionType.LessThan:
                    return ParseExpressionAndBuildQuery((BinaryExpression)expression, "lt");
                case ExpressionType.LessThanOrEqual:
                    return ParseExpressionAndBuildQuery((BinaryExpression)expression, "le");
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    return ParseExpressionAndBuildQueryWithClosedParenthesis((BinaryExpression)expression, "and");
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return ParseExpressionAndBuildQueryWithClosedParenthesis((BinaryExpression)expression, "or");
                case ExpressionType.Constant:
                    return ParseConstantExpression((ConstantExpression)expression);
                case ExpressionType.MemberAccess:
                    return ParseMemberExpression((MemberExpression)expression);
                case ExpressionType.Call:
                    return ParseMethodCallExpression((MethodCallExpression)expression);
                case ExpressionType.Convert:
                    return ParseExpressionAndBuildQuery((UnaryExpression)expression);
                default:
                    return null;
            }
        }

        private static string ParseExpressionAndBuildQuery(UnaryExpression expression)
        {
            return $"{ParseExpression(expression.Operand)}";
        }

        private static string ParseMethodCallExpression(MethodCallExpression expression)
        {
            if (expression?.Arguments == null || !expression.Arguments.Any())
            {
                return null;
            }

            var parameterExpression = expression.Arguments[0];
            return ParseExpression(parameterExpression);
        }

        private static string ParseExpressionAndBuildQuery(BinaryExpression expression, string operatorKeyword)
        {
            return $"{ParseExpression(expression.Left)} {operatorKeyword} {ParseExpression(expression.Right)}";
        }

        private static string ParseExpressionAndBuildQueryWithClosedParenthesis(BinaryExpression expression,
            string operatorKeyword)
        {
            return
                $"({ParseExpression(expression.Left)} {operatorKeyword} {ParseExpression(expression.Right)})";
        }

        private static string ParseMemberExpression(MemberExpression memberEx)
        {
            if (memberEx.Member.DeclaringType == _azureDocumentType)
            {
                return memberEx?.Member.Name.ToCamelCase();
            }

            var objectMember = Expression.Convert(memberEx, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();

            if (memberEx.Type.AssemblyQualifiedName != null && memberEx.Type.AssemblyQualifiedName.IndexOf("DateTime", StringComparison.Ordinal) > 0)
            {
                return $"{Convert.ToDateTime(getter()).ToString("yyyy-MM-dd")}";
            }

            return memberEx.Type.IsPrimitive ? $"{getter()}" : $"'{getter()}'";
        }

        private static string ParseConstantExpression(ConstantExpression constEx)
        {
            if (constEx == null)
            {
                return null;
            }

            if (constEx.Type == typeof(string) || constEx.Type == typeof(String))
            {
                return $"'{constEx.Value}'";
            }

            return (string)constEx.Value;
        }
    }
}
