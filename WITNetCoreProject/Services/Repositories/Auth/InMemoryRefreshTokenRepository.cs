using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Services.Interfaces.Auth;

namespace WITNetCoreProject.Services.Repositories.Auth {

    // this is transactional command between for refresh token object and refresh token in memory, implementation command from IRefreshTokenRepository
    public class InMemoryRefreshTokenRepository : IRefreshTokenRepository {

        private readonly List<RefreshTokens> _refreshTokens = new List<RefreshTokens>();

        public InMemoryRefreshTokenRepository() {
        }

        public Task Create(RefreshTokens refreshToken) {

            refreshToken.Id = Guid.NewGuid();

            _refreshTokens.Add(refreshToken);

            return Task.CompletedTask;
        }

        public Task Delete(Guid id) {

            _refreshTokens.RemoveAll(r => r.Id == id);

            return Task.CompletedTask;
        }

        public Task DeleteAll(Guid userId) {

            _refreshTokens.RemoveAll(r => r.UserId == userId);

            return Task.CompletedTask;
        }

        public Task<RefreshTokens> GetByEmployeeId(Guid userId) {

            RefreshTokens refreshToken = _refreshTokens.FirstOrDefault(r => r.UserId == userId);

            return Task.FromResult(refreshToken);
        }

        public Task<RefreshTokens> GetByToken(string token) {

            RefreshTokens refreshToken = _refreshTokens.FirstOrDefault(r => r.Token == token);

            return Task.FromResult(refreshToken);
        }
    }
}
