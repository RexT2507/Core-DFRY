using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ApiLibrary.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Range<T>(this IQueryable<T> query, int start, int count)
        {
            return query.Skip(start).Take(count);
        }


        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        private static Expression<Func<T, object>> ToLambdaObject<T>(object fields)
        {
            return null;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propName)
        {
            return source.OrderBy(ToLambda<T>(propName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propName)
        {
            return source.OrderByDescending(ToLambda<T>(propName));
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propName)
        {
            return source.ThenBy(ToLambda<T>(propName));
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propName)
        {
            return source.ThenByDescending(ToLambda<T>(propName));
        }

        // SELECT

        public static IEnumerable<dynamic> SelectOnly<T>(this IQueryable<T> source, string fields)
        {
            var tab = fields.Split(',');
            var parameter = Expression.Parameter(typeof(T), "x");
            var bindings = tab.Select(x => Expression.PropertyOrField(parameter, x)).Select(x => Expression.Bind(x.Member, x));
            var exp = Expression.MemberInit(Expression.New(typeof(T)), bindings);

            var lambda = Expression.Lambda<Func<T, object>>(exp, parameter);



            return source.Select(lambda).Select(x => SelectObject(x, fields));
        }

        public static object SelectObject(object value, string fields) // Pour le mapping des données
        {
            dynamic data = new ExpandoObject();
            var dataMap = (IDictionary<string, object>)data;
            var props = value.GetType().GetRuntimeProperties();
            var properties = fields.Split(',');
            foreach (var item in properties)
            {
                dataMap.Add(item, props.Single(x => x.Name.ToLower() == item.ToLower()).GetValue(value));
            }

            return data; // On peut return le ExpandoObject, vu que le Map et le ExpandoObject font référence au même objet en mémoire
        }
    }
}
