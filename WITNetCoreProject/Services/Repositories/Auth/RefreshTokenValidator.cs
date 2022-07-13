using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WITNetCoreProject.Models.Authentications;

namespace WITNetCoreProject.Services.Repositories.Auth {

    public class RefreshTokenValidator {

        // declaration field for every services that you need
        private readonly AuthenticationConfiguration _configuration;

        // this constructor is for implementing what did declare above
        public RefreshTokenValidator(AuthenticationConfiguration configuration) {

            _configuration = configuration;
        }

        // this method is for validate refresh token that gave from user and return whether is valid or not
        public bool Validate(string refreshToken) {

            TokenValidationParameters validationParameters = new TokenValidationParameters() {

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.RefreshTokenSecret)),
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try {

                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception) {

                return false;
            }
        }
    }
}
