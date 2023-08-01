using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Infra.Integration.Hangfire.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblies(this Type type)
            => AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(a => a.GetTypes()
                                          .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfalceType"></param>
        /// <param name="makeType"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetMakeGenericType(this Type interfalceType, Type makeType)
            => AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SelectMany(a => a.GetTypes()
                        .Where(t => t.GetInterfaces().Contains(interfalceType.MakeGenericType(makeType))));

    }
}
