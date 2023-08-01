using App.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Mvc.Middlewares
{
    public class AuthMiddleware : IMiddleware
    {
        public const string COOKIE_AUTH = "WSID";

        //readonly Access _access;
        readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(
            //Access access,
            ILogger<AuthMiddleware> logger)
        {
            //_access = access;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                //if (context.Request.Cookies.Any(x => x.Key.Equals(COOKIE_AUTH)))
                //{
                //    var token = context.Request.Cookies[COOKIE_AUTH];
                //    if (string.IsNullOrEmpty(token))
                //        throw new UnauthorizedAccessException("Invalid token is empty or null.");

                //    var customerId = _access.CustomerId(token);
                //    if(customerId == default)
                //        throw new UnauthorizedAccessException("Invalid token, client ID not found.");
                    
                //    var identity = new ClaimsIdentity(new List<Claim> {
                //        new Claim(ClaimTypes.Sid, customerId.ToString()),
                //    }, typeof(AuthMiddleware).Name);

                //    context.User = new ClaimsPrincipal(identity);
                //}
            }
            catch (DomainException e)
            {
                _logger.LogWarning(e.Message, e);
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogCritical(e.Message, e);
            }
            catch (InvalidOperationException e) when (e.Message.Contains("authenticationScheme"))
            {
                _logger.LogWarning(e.Message, e);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
            finally
            {
                await next.Invoke(context);
            }
        }
    }
}
