using System;
using System.Collections.Generic;
using System.Security.Claims;
using WITNetCoreProject.Models.Authentications;
using WITNetCoreProject.Models.Dtos;

namespace WITNetCoreProject.Services.Repositories.Auth {

    public class TokenProjectServices {

        // declaration field for every services that you need
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        // this constructor is for implementing what did declare above
        public TokenProjectServices(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator) {

            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        // this function is for authenticate access token for valid user and also create the access token in the same time then return as AccessToken object
        public AccessToken Authenticate(UserDto user) {

            try {

                List<Claim> claims = new List<Claim>() {

                    new Claim("id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpirationMinutes);
                string value = _tokenGenerator.GenerateToken(
                    _configuration.AccessTokenSecret,
                    _configuration.Issuer,
                    _configuration.Audience,
                    expirationTime,
                    claims);

                return new AccessToken() {

                    Value = value,
                    ExpirationTime = expirationTime
                };
            }
            catch (Exception ex) {

                throw ex;
            }
        }
    }
}
