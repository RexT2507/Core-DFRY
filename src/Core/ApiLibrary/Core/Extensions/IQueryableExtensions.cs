using Microsoft.AspNetCore.Http;
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

        private static IQueryable<T> WhereFieldIsLessOrEqual<T>(this IQueryable<T> source, string fieldName, string value, Type type)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);

            DateTime myDate = DateTime.Parse(value);
            var exp = Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(myDate), type));

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }

        private static IQueryable<T> WhereFieldIsGreaterOrEqual<T>(this IQueryable<T> source, string fieldName, string value, Type type)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            BinaryExpression exp;

            if(type == typeof(DateTime))
            {
                DateTime myDate = DateTime.Parse(value);
                exp = Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(myDate), type));
            }
            else
            {
                exp = Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(value), type));
            }


            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }

        private static IQueryable<T> WhereFieldIsBetween<T>(this IQueryable<T> source, string fieldName, string value, Type type)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            BinaryExpression exp;

            var tab = value.Split(',');

            if(type == typeof(DateTime))
            {
                DateTime startDate = DateTime.Parse(tab[0]);
                DateTime endDate = DateTime.Parse(tab[1]);

                exp = Expression.And(
                    Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(startDate), type)),
                    Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(endDate), type))
                );
            }
            else
            {
                var convertedValueStart = Convert.ChangeType(tab[0], type);
                var convertedValueEnd = Convert.ChangeType(tab[1], type);

                exp = Expression.And(
                    Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(convertedValueStart), type)),
                    Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(convertedValueEnd), type))
                );
            }

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }

        private static IQueryable<T> WhereFieldExact<T>(this IQueryable<T> source, string fieldName, string value, Type type)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            BinaryExpression exp;

            if(type == typeof(DateTime))
            {
                DateTime startDate = DateTime.Parse(value);
                DateTime endDate = startDate.AddDays(1);

                exp = Expression.And(
                    Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(startDate), type)),
                    Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(endDate), type))
                );
            }
            else
            {
                var convertedValue = Convert.ChangeType(value, type);
                exp = Expression.Equal(property, Expression.Convert(Expression.Constant(convertedValue), type));
            }

            var lambda = Expression.Lambda<Func<T, bool>>(exp, parameter);

            return source.Where(lambda);
        }

        /**
         * source : Requête d'origine
         * value : Valeur du champ
         * type : Type de l'argument (int, DateTime, etc...)
         * fieldName : Nom du champ à trier
         */
        public static IQueryable<T> WhereFieldIs<T>(this IQueryable<T> source, string value, Type type, string fieldName)
        {
            if (value.StartsWith("[,"))
            {
                value = new string(value.Skip(2).SkipLast(1).ToArray());
                return source.WhereFieldIsLessOrEqual(fieldName, value, type);
            }
            else if (value.EndsWith(",]"))
            {
                value = new string(value.Skip(1).SkipLast(2).ToArray());
                return source.WhereFieldIsGreaterOrEqual(fieldName, value, type);
            }
            else if(value.StartsWith('[') && value.EndsWith(']'))
            {
                value = new string(value.Skip(1).SkipLast(1).ToArray());
                return source.WhereFieldIsBetween(fieldName, value, type);
            }

            return source.WhereFieldExact(fieldName, value, type);

           
            
        }
    }
}
