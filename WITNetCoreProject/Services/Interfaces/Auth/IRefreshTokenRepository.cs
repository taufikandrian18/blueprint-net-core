using System;
using System.Threading.Tasks;
using WITNetCoreProject.Models.Entities;

namespace WITNetCoreProject.Services.Interfaces.Auth {

    // this interface is to make command for transactional refresh token process
    public interface IRefreshTokenRepository {

        Task<RefreshTokens> GetByToken(string token);

        Task<RefreshTokens> GetByEmployeeId(Guid userId);

        Task Create(RefreshTokens refreshToken);

        Task Delete(Guid id);

        Task DeleteAll(Guid userId);
    }
}
