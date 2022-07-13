using System;
namespace WITNetCoreProject.Services.Logger {

    // this interface is to make command for logging process and stored to this folder with logfile.txt
    public interface ILoggerManager {

        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
