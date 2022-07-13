using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Services.Interfaces.Auth;

namespace WITNetCoreProject.Services.Repositories.Auth {

    // this is transactional command between for refresh token object and refresh token in database, implementation command from IRefreshTokenRepository
    public class DatabaseRefreshTokenRepository : IRefreshTokenRepository {

        private readonly RepositoryContext _context;

        public DatabaseRefreshTokenRepository(RepositoryContext repositoryContext) {

            _context = repositoryContext;
        }

        public async Task Create(RefreshTokens refreshToken) {

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id) {

            RefreshTokens refreshToken = await _context.RefreshTokens.FindAsync(id);
            if (refreshToken != null) {

                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(Guid userId) {

            IEnumerable<RefreshTokens> refreshTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshTokens> GetByEmployeeId(Guid userId) {

            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<RefreshTokens> GetByToken(string token) {

            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }
    }
}
