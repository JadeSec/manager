using Microsoft.IdentityModel.Tokens;
using App.Infra.Integration.Jwt.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App.Infra.Integration.RabbitMq.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void UseJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var option = Option.Parse(configuration);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.SaveToken = false;
                x.RequireHttpsMetadata = false;               
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(option.SecretKey),
                };
            });
        }
    }
}
