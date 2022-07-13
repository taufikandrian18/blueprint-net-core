using System;
using System.Threading.Tasks;
using WITNetCoreProject.Models.Authentications;
using WITNetCoreProject.Models.Dtos;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Services.Interfaces.Auth;

namespace WITNetCoreProject.Services.Repositories.Auth {

    public class Authenticator {

        // declaration field for every services that you need
        private readonly TokenProjectServices _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        // this constructor is for implementing what did declare above
        public Authenticator(TokenProjectServices accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository) {

            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        // this function is for authenticate refresh token for valid user and also create the refresh token in the same time then return as AuthenticatedUserResponse object
        public async Task<AuthenticatedUserResponse> Authenticate(UserDto user) {

            AccessToken accessToken = _accessTokenGenerator.Authenticate(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            RefreshTokens refreshTokenDTO = new RefreshTokens() {

                Token = refreshToken,
                UserId = user.UserId
            };

            await _refreshTokenRepository.Create(refreshTokenDTO);

            return new AuthenticatedUserResponse() {

                AccessToken = accessToken.Value,
                AccessTokenExpirationTime = accessToken.ExpirationTime,
                RefreshToken = refreshToken
            };
        }
    }
}
