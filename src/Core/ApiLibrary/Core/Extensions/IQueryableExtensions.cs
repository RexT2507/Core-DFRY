using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        // WHERE

        public static IQueryable<T> WhereDateIsLessOrEqual<T>(this IQueryable<T> source, string field, string date)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);

            DateTime myDate = DateTime.Parse(date);
            var exp = Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(myDate), typeof(DateTime)));

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }

        public static IQueryable<T> WhereDateIsGreaterOrEqual<T>(this IQueryable<T> source, string field, string date)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);

            DateTime myDate = DateTime.Parse(date);
            var exp = Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(myDate), typeof(DateTime)));

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }

        public static IQueryable<T> WhereDateIsBetween<T>(this IQueryable<T> source, string field, string date)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, field);

            DateTime myDate = DateTime.Parse(date);
            var exp = Expression.And(
                Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(myDate), typeof(DateTime))), 
                Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(myDate), typeof(DateTime))));

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }

        public static IQueryable<T> WhereDateIs<T>(this IQueryable<T> source, string date)
        {
            if (date.StartsWith("[,"))
            {
                date = new string(date.Skip(2).SkipLast(1).ToArray());
                return source.WhereDateIsLessOrEqual("createdAt", date);

            }
            else if (date.EndsWith(",]"))
            {
                date = new string(date.Skip(2).SkipLast(1).ToArray());
                return source.WhereDateIsGreaterOrEqual("createdAt", date);
            }

            date = new string(date.Skip(1).SkipLast(1).ToArray());
            var tab = date.Split(',');
            return source.WhereDateIsBetween("createdAt", date);
            
        }
    }
}
