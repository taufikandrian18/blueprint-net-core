using System;
namespace WITNetCoreProject.Utilities {

    public class DateUtil {

        // this function is to make a default template date
        public static DateTime GetCurrentDate() {

            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
        }
    }
}
