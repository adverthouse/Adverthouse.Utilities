﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Adverthouse.Common.Data
{
    public static class Queryable
    {
        public static IQueryable<TSource> NotNullWhere<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, string exp)
        {
            if (!String.IsNullOrWhiteSpace(exp)) return source.Where(predicate);

            return source;
        }

        public static IQueryable<TSource> NotNullWhere<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, int? exp)
        {
            if (exp.HasValue) return source.Where(predicate);

            return source;
        }

        public static IQueryable<TSource> NotNullWhere<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, bool? exp)
        {
            if (exp.HasValue) return source.Where(predicate);

            return source;
        }
        public static IQueryable<TSource> NotNullWhere<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, double? exp)
        {
            if (exp.HasValue) return source.Where(predicate);

            return source;
        }
    }
}