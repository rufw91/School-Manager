using System;

namespace Helper
{
    public static class Constants
    {
        private static string GetSchemaFileName()
        {
            string filepath = Constants.DataDirectoryPath + "\\DbSchema." +
                            new Random().Next(1000, 100000).ToString() + ".xml";
            try
            {
                filepath = Constants.DataDirectoryPath + "\\DbSchema." +
                        new Random().Next(1000, 100000).ToString() + ".xml";
                while (System.IO.File.Exists(filepath))
                {
                    filepath = Constants.DataDirectoryPath + "\\DbSchema." +
                        new Random().Next(1000, 100000).ToString() + ".xml";
                }
            }
            catch
            {

            }
            return filepath;
        }
        public static string SchemaFilePath
        {
            get
            {
                return GetSchemaFileName();
            }
        }
        public static string DefaultSchemaFilePath
        {
            get
            {
                string filepath = Constants.DataDirectoryPath + "\\DbSchema.xml";
                return filepath;
            }
        }
        
        private static string GetSysInfoFileName()
        {
            string filepath = Constants.DataDirectoryPath + "\\SysInfo.xml";
            if (System.IO.File.Exists(filepath))
            {
                try
                {
                    System.IO.File.Delete(filepath);
                }
                catch
                {
                    filepath = Constants.DataDirectoryPath + "\\SysInfo." +
                            new Random().Next(1000, 100000).ToString() + ".xml";
                    while (System.IO.File.Exists(filepath))
                    {
                        filepath = Constants.DataDirectoryPath + "\\SysInfo." +
                            new Random().Next(1000, 100000).ToString() + ".xml";
                    }
                }
            }
            return filepath;
        }
        public static string MySysInfoFilePath
        {

            get { return Constants.DataDirectoryPath + "\\MySysInfo.xml"; }

        }
        public static string SysInfoFilePath
        {
            get
            {
                return GetSysInfoFileName();
            }
        }
        public static string DataFilePath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\Starehe MS\\Data\\Starehe.mdf";
            }
        }

        public static string DataDirectoryPath
        {
            get
            {
                return Environment.GetFolderPath(
                  Environment.SpecialFolder.CommonApplicationData) + "\\Starehe MS\\Data";
            }
        }
        public static string BackupDirectoryPath
        {
            get
            {
                return Environment.GetFolderPath(
                  Environment.SpecialFolder.CommonApplicationData) + "\\Starehe MS\\Backup\\";
            }
        }
    }
}
