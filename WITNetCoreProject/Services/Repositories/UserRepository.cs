using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Models.Exceptions;
using WITNetCoreProject.Services.Interfaces;

namespace WITNetCoreProject.Services.Repositories {

    // this is transactional command between for user and database, using base command from base repository and also inherited from IUserProfileRepository
    public class UserRepository : BaseRepository<Users>, IUserProfileRepository {

        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext) {
        }

        public void CreateUser(Users user) {

            Create(user);
        }

        public void DeleteUserById(Guid userId) {

            var user = this.RepositoryContext.Users.Find(userId);

            if (user != null) {

                this.RepositoryContext.Users.Remove(user);
                this.RepositoryContext.SaveChanges();
            }
            else {

                throw new NotFoundException(System.Net.HttpStatusCode.NotFound,"User with user id : " +userId+ " could not be found in database");
            }
        }

        public async Task<Users> GetUserById(Guid userId) {

            return await FindByCondition(us => us.UserId.Equals(userId)).Where(us => us.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Users> GetUserByUsername(string username) {

            return await FindByCondition(us => us.Username.Equals(username)).Where(us => us.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Users> GetUserIdByEmail(string email) {

            return await FindByCondition(us => us.Email.Equals(email)).Where(us => us.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Users>> GetUsers() {

            return await FindAll()
                .Where(ow => ow.IsDeleted == false).AsNoTracking().ToListAsync();
        }

        public void UpdateUser(Users user) {

            Update(user);
        }
    }
}
