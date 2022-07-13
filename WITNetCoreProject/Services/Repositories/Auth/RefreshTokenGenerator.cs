using System;
using WITNetCoreProject.Models.Authentications;

namespace WITNetCoreProject.Services.Repositories.Auth {

    public class RefreshTokenGenerator {

        // declaration field for every services that you need
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        // this constructor is for implementing what did declare above
        public RefreshTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator) {

            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        // this method is for generate refresh token that gave from appsetting and return new token
        public string GenerateToken() {

            DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpirationMinutes);

            return _tokenGenerator.GenerateToken(
                _configuration.RefreshTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime);
        }
    }
}
