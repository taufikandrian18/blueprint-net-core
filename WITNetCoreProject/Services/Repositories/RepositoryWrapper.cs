using System;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Services.Interfaces;

namespace WITNetCoreProject.Services.Repositories {

    // this function is for wrap all repository pattern while mediates between the query object layer and data mapping layers
    public class RepositoryWrapper : IRepositoryWrapper {

        private RepositoryContext _repoContext;
        private IUserProfileRepository _user;

        public RepositoryWrapper(RepositoryContext repositoryContext) {

            _repoContext = repositoryContext;
        }

        public IUserProfileRepository User {

            get {

                if (_user == null) {

                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public void Save() {

            _repoContext.SaveChanges();
        }
    }
}
