using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Extensions
{
    internal static class ExpressionExtensions
    {
        public static PropertyInfo GetPropertyInfo<TType, TReturn>(this Expression<Func<TType, TReturn>> property)
        {
            LambdaExpression lambda = property;
            MemberExpression? memberExpression;
            if (lambda.Body is UnaryExpression expression)
            {
                memberExpression = (MemberExpression)expression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return (PropertyInfo)memberExpression.Member;
        }

        public static string GetPropertyName<TType, TReturn>(this Expression<Func<TType, TReturn>> property)
        {
            return property.GetPropertyInfo().Name;
        }
    }
}
