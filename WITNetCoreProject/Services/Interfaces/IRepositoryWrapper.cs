using System;
namespace WITNetCoreProject.Services.Interfaces {

    // this function is for wrap all repository interfaces which already created
    public interface IRepositoryWrapper {

        IUserProfileRepository User { get; }
        void Save();
    }
}
