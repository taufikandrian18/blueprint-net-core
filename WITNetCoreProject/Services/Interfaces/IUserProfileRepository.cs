using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WITNetCoreProject.Models.Entities;

namespace WITNetCoreProject.Services.Interfaces {

    // this interface is to make command for transactional user object process
    public interface IUserProfileRepository : IBaseProfileRepository<Users> {

        Task<IEnumerable<Users>> GetUsers();

        Task<Users> GetUserById(Guid userId);

        Task<Users> GetUserByUsername(string username);

        Task<Users> GetUserIdByEmail(string email);

        void DeleteUserById(Guid userId);

        void CreateUser(Users user);

        void UpdateUser(Users user);
    }
}
