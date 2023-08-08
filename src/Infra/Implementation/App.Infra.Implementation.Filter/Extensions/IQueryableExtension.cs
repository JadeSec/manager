using System;
using System.Linq;

namespace App.Infra.Implementation.Filter.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<TSource> FilterEqual<TSource, TValue>(this IQueryable<TSource> source, Filter filter, string attr, Func<IQueryable<TSource>, TValue, IQueryable<TSource>> exec)
        {
            var expression = filter.Expressions.Where(x => x.Name.Equals(attr) && (x.Operator.Equals(string.Empty) || x.Operator.Equals("=")))
                                               .FirstOrDefault();

            if (expression != null)
            {
                var value = expression.Value.GetValue<TValue>();
                if (value != null)
                    source = exec(source, value);
            }

            return source;
        }
    }
}