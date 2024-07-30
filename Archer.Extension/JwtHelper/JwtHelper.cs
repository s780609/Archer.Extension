using Archer.Extension.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Archer.Extension.JwtHelper
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(TokenModel tokenModel, int expireMinutes = 480)
        {
            if (tokenModel == null)
            {
                throw new ArgumentNullException(nameof(tokenModel));
            }
            else if (string.IsNullOrWhiteSpace(tokenModel.EmployeeNo))
            {
                throw new ArgumentException(nameof(tokenModel.EmployeeNo));
            }
            else if (string.IsNullOrWhiteSpace(tokenModel.EmployeeName))
            {
                throw new ArgumentException(nameof(tokenModel.EmployeeName));
            }
            else if (tokenModel.Roles.Count() == decimal.Zero)
            {
                throw new ArgumentException(nameof(tokenModel.Roles));
            }

            var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
            var signKey = _configuration.GetValue<string>("JwtSettings:SignKey");

            // Configuring "Claims" to your JWT Token
            var claims = new List<Claim>
                         {
                             new Claim(JwtRegisteredClaimNames.Iss, issuer),
                             new Claim(JwtRegisteredClaimNames.Sub, "login"), // User.Identity.Name
                             new Claim("employeeName", tokenModel.EmployeeName),
                             new Claim("employeeNo", tokenModel.EmployeeNo),
                         };

            for (int i = 0; i < tokenModel.Roles.Length; i++)
            {
                claims.Add(new Claim("roles", tokenModel.Roles[i]));
            }

            var userClaimsIdentity = new ClaimsIdentity(claims);

            // Create a SymmetricSecurityKey for JWT Token signatures
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

            // HmacSha256 MUST be larger than 128 bits, so the key can't be too short. At least 16 and more characters.
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create SecurityTokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = userClaimsIdentity,

                // Default is DateTime.Now
                NotBefore = tokenModel.NotValidBefore,

                // Default is DateTime.Now
                IssuedAt = tokenModel.IssuedAt,
                Expires = tokenModel.ExpirationTime.AddMinutes(expireMinutes),
                SigningCredentials = signingCredentials
            };

            // Generate a JWT securityToken, than get the serialized Token result (string)
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }

        public bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;

            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                // Because there is no expiration in the generated token
                ValidateLifetime = true,

                // Because there is no audiance in the generated token
                ValidateAudience = false,

                // Because there is no issuer in the generated token
                ValidateIssuer = true,
                ValidIssuer = _configuration.GetValue<string>("JwtSettings:Issuer"),
                ValidAudience = null,

                // The same key as the one that generate the token
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSettings:SignKey")))
            };
        }
    }
}
