using System;
using System.Security.Claims;
using App.Infra.Bootstrap.Attributes;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using App.Infra.Integration.Jwt.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace App.Infra.Integration.Jwt
{
    [Transient]
    public class JwtService
    {
        private Option _option;

        public JwtService(IConfiguration configuration)
        {
            _option = Option.Parse(configuration);
        }

        public string Generate(ClaimsIdentity identity, DateTime expiration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = expiration,
                Issuer = _option.Issuer,
                Audience = _option.Audience,
                SigningCredentials = new SigningCredentials(
                   key: new SymmetricSecurityKey(_option.SecretKey),
                   algorithm: SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool Validate(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _option.Issuer,
                    ValidAudience = _option.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        key: _option.SecretKey
                    )
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string GetClaim(string token, string claimType)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                return securityToken.Claims.First(claim => claim.Type == claimType).Value;
            }
            catch
            {
                return default;
            }
        }
    }
}
