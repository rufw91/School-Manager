using System;

namespace Helper
{
    public static class Constants
    {
        public static string LogDirectoryPath
        {
            get
            {
                return Environment.GetFolderPath(
                  Environment.SpecialFolder.CommonApplicationData) + "\\Umanyi MS\\Logs\\";
            }
        }
        public static string BackupDirectoryPath
        {
            get
            {
                return Environment.GetFolderPath(
                  Environment.SpecialFolder.CommonApplicationData) + "\\Umanyi MS\\Backups\\";
            }
        }
    }
}
