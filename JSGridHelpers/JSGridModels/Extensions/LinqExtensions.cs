using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JSGridModels.Extensions
{
    public static class LinqExtensions
    {
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);

            if (matchedProperty == null)
                throw new ArgumentException("name");

            return matchedProperty;
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo propertyInfo)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, propertyInfo.Name);

            var expr = Expression.Lambda(propAccess, paramExpr);

            return expr;
        }

        private static IEnumerable<EnumerableType> GetEnumerableFromMethod<EnumerableType>(IEnumerable<EnumerableType> objects, string propertyName, string methodName)
        {
            PopulateExpressionAndGenericMethodViaEnumerable<EnumerableType>(propertyName, methodName, out LambdaExpression expression, out MethodInfo genericMethod);
            return (IEnumerable<EnumerableType>)genericMethod.Invoke(null, new object[] { objects, expression.Compile() });
        }

        private static void PopulateExpressionAndGenericMethodViaEnumerable<EnumerableType>(string propertyName, string methodName, out LambdaExpression expression, out MethodInfo genericMethod)
        {
            var propInfo = GetPropertyInfo(typeof(EnumerableType), propertyName);
            expression = GetOrderExpression(typeof(EnumerableType), propInfo);

            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2);
            genericMethod = method.MakeGenericMethod(typeof(EnumerableType), propInfo.PropertyType);
        }

        public static IEnumerable<EnumerableType> OrderBy<EnumerableType>(this IEnumerable<EnumerableType> objects, string propertyName)
        {
            return GetEnumerableFromMethod(objects, propertyName, "OrderBy");
        }

        public static IEnumerable<EnumerableType> OrderByDescending<EnumerableType>(this IEnumerable<EnumerableType> objects, string propertyName)
        {
            return GetEnumerableFromMethod(objects, propertyName, "OrderByDescending");

        }
    }
}