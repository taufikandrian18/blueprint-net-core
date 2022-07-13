using System;
using NLog;

namespace WITNetCoreProject.Services.Logger {

    // this is transactional command for logging process inherited from ILoggerManager
    public class LoggerManagerRepository : ILoggerManager {

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message) {

            logger.Debug(message);
        }

        public void LogError(string message) {

            logger.Error(message);
        }

        public void LogInfo(string message) {

            logger.Info(message);
        }

        public void LogWarn(string message) {

            logger.Warn(message);
        }
    }
}
