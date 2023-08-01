using System;
using System.ComponentModel;
using System.Security.Claims;

namespace App.Infra.CrossCutting.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string Sid(this ClaimsPrincipal principal)
            => principal.FindFirst(ClaimTypes.Sid).Value;

        public static T Sid<T>(this ClaimsPrincipal principal)
            where T : IConvertible
            => (T)TypeDescriptor.GetConverter(typeof(T))
                                .ConvertFromString(principal.Sid());
    }
}
