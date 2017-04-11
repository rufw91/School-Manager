using System;

namespace UmanyiSMS.Lib
{
    public static class Constants
    {
        public static string LogDirectoryPath
        {
            get
            {
                return Environment.GetFolderPath(
                  Environment.SpecialFolder.CommonApplicationData) + "\\Raphael Muindi\\UmanyiSMS\\Logs\\";
            }
        }
        public static string BackupDirectoryPath
        {
            get
            {
                return Environment.GetFolderPath(
                  Environment.SpecialFolder.CommonApplicationData) + "\\Raphael Muindi\\UmanyiSMS\\Backups\\";
            }
        }
    }
}
