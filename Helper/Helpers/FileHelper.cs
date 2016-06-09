using Helper.Converters;
using Microsoft.Win32;
using OpenXmlPackaging;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Helper
{
    public class FileHelper
    {
        public static string SaveFileAsBak()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.ValidateNames = true;
            saveDialog.Title = "Select location to save backup file";
            saveDialog.Filter = "Backup Files|*.bak";
            saveDialog.OverwritePrompt = true;
            if ((saveDialog.ShowDialog() == true) &&
                (new System.IO.FileInfo(saveDialog.FileName).Extension.ToLower() == ".bak"))
            {
                return saveDialog.FileName;
            }
            else return null;
        }

        public static bool CheckFiles()
        {
            try
            {
                if (!Directory.Exists(Constants.LogDirectoryPath))
                    Directory.CreateDirectory(Constants.LogDirectoryPath);
                if (!Directory.Exists(Constants.BackupDirectoryPath))
                    Directory.CreateDirectory(Constants.BackupDirectoryPath);
                return true;
            }
            catch { return false; }
        }

        public static bool Decompress(string fileToDecompress, string destNewFile)
        {

            try
            {
                if (File.Exists(destNewFile))
                {
                    File.Delete(destNewFile);
                }
                FileInfo fi = new FileInfo(fileToDecompress);
                using (FileStream inFile = fi.OpenRead())
                {
                    string curFile = fi.FullName;
                    using (FileStream outFile = File.Create(destNewFile))
                    {
                        using (System.IO.Compression.GZipStream Decompress = new System.IO.Compression.GZipStream(inFile,
                                System.IO.Compression.CompressionMode.Decompress))
                        {
                            Decompress.CopyTo(outFile);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Compress(string fileToCompress, string destNewFile)
        {
            bool succ = false;
            try
            {
                FileInfo fi = new FileInfo(fileToCompress);
                using (FileStream inFile = fi.OpenRead())
                {

                    if (File.Exists(destNewFile))

                        return false;


                    using (FileStream outFile =
                                File.Create(destNewFile))
                    {
                        using (System.IO.Compression.GZipStream Compress =
                            new System.IO.Compression.GZipStream(outFile,
                            System.IO.Compression.CompressionMode.Compress))
                        {
                            inFile.CopyTo(Compress);
                        }
                    }

                }
                succ = true;
            }
            catch
            {
                succ = false;
            }
            return succ;
        }

        public static string GetNewDefBackupPath()
        {
            string newpath = Constants.BackupDirectoryPath +
                (new Random().Next(1000) *
                new Random().Next(2000)).ToString() + ".BAK";
            while (File.Exists(newpath))
            {
                newpath = Constants.BackupDirectoryPath + (new Random().Next(1000) *
                   new Random().Next(2000)).ToString() + ".BAK";
            }
            return newpath;
        }

      
        public static bool DeleteFile(string path)
        {
            bool succ = false;
            try
            {
                File.Delete(path);
                succ = true;
            }
            catch { succ = false; }
            return succ;
        }

        
        
        public static byte[] BrowseImageAsByteArray()
        {
            try
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "All Picture Files|*.bmp;*.jpeg;*.jpg;*.png" + "|Bitmap Files (*.bmp)|*.bmp" +
                "|JPEG Files (*.jpeg*)|*.jpeg" + "|JPG Files (*.jpg*)|*.jpg" +
                "|PNG files (*.png)|*.png";

                if ((myDialog.ShowDialog() == true) && (myDialog.CheckFileExists))
                {
                    try
                    {
                    }
                    catch
                    {
                        MessageBox.Show("Invalid file.");                        
                        return null;
                    }
                    using (FileStream mems = new FileStream(myDialog.FileName, FileMode.Open))
                    {
                        MemoryStream mem = new MemoryStream(1000);
                        mems.CopyTo(mem);
                        mem.Seek(0, System.IO.SeekOrigin.Begin);

                        byte[] buff = mem.ToArray();
                        mems.Dispose();
                        mem.Dispose();
                        return buff;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Invalid file.");
                }

            }
            catch { MessageBox.Show("An Error occured while loading the file."); }
            return null;

        }

        public static string BrowseExcelFileAsString()
        {
            try
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "Excel Files|*.xlsx";

                if ((myDialog.ShowDialog() == true) && (myDialog.CheckFileExists))
                {
                    if (!SpreadsheetDocument.Test(myDialog.FileName))
                    {
                        MessageBox.Show("Invalid file.");
                        return null;
                    }
                    return myDialog.FileName;
                }
            }
            catch { MessageBox.Show("An Error occured while loading the file."); }
            return null;
        }

        public async static Task<string> BrowseBAKFileAsString()
        {
            try
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "Backup Files|*.bak";

                if ((myDialog.ShowDialog() == true) && (myDialog.CheckFileExists))
                {
                    if (!await DataAccessHelper.TestBackupFile(myDialog.FileName))
                    {
                        MessageBox.Show("Invalid file.");
                        return null;
                    }
                    return myDialog.FileName;
                }
            }
            catch { MessageBox.Show("An Error occured while loading the file."); }
            return null;
        }

        internal static string GetNewNetworkServiceTempFilePath(string pref)
        {
            //C:\Windows\ServiceProfiles\NetworkService
           return Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\ServiceProfiles\\NetworkService\\AppData\\Local\\Temp\\" + "UmanyiSMS_"+pref + DateTime.Now.ToString("dd.MM.yyyy.hh.mm.ss");
            
        }

        public static Uri BrowseImageAsUri()
        {
            try
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "All Picture Files|*.bmp;*.jpeg;*.jpg;*.png" + "|Bitmap Files (*.bmp)|*.bmp" +
                "|JPEG Files (*.jpeg*)|*.jpeg" + "|JPG Files (*.jpg*)|*.jpg" +
                "|PNG files (*.png)|*.png";

                if ((myDialog.ShowDialog() == true) && (myDialog.CheckFileExists))
                {
                    try
                    {
                        new BitmapImage(new Uri(myDialog.FileName));
                    }
                    catch
                    {
                        MessageBox.Show("Invalid file.");
                        return null;
                    }
                    return new Uri(myDialog.FileName);
                }
            }
            catch { MessageBox.Show("An Error occured while loading the file."); }
            return null;
        }

        public static Uri BrowseImageorVideoAsUri()
        {
            try
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "All Supported Files|*.bmp;*.jpeg;*.jpg;*.png;*.3gp;*.3gpp;*.avi;*.divx;*.flv;"+
                    "*.mkv;*.mov;*.mp4;*.mpeg;*.mpg;*.ogg;*.vob;*.webm;*.wmv;*.flac;*.m4a;*.mid;*.mp3;*.oga;*.wav;*.wma;*.cda"; 

                if ((myDialog.ShowDialog() == true) && (myDialog.CheckFileExists))
                {                    
                    return new Uri(myDialog.FileName);
                }
            }
            catch { MessageBox.Show("An Error occured while loading the file."); }
            return null;
        }

        public static string BrowseMediaAsString()
        {
            try
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "All Supported Files|*.bmp;*.jpeg;*.jpg;*.png;*.3gp;*.3gpp;*.avi;" +
                    "*.mov;*.mp4;*.mpeg;*.mpg;*.wmv;*.m4a;*.mp3;*.wav;*.wma;*.cda";

                if ((myDialog.ShowDialog() == true) && (myDialog.CheckFileExists))
                {
                    return myDialog.FileName;
                }
            }
            catch { MessageBox.Show("An Error occured while loading the file."); }
            return null;
        }

        public static BitmapImage BrowseImageAsBitmap()
        {
            try
            {
                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "All Picture Files|*.bmp;*.jpeg;*.jpg;*.png" + "|Bitmap Files (*.bmp)|*.bmp" +
                "|JPEG Files (*.jpeg*)|*.jpeg" + "|JPG Files (*.jpg*)|*.jpg" +
                "|PNG files (*.png)|*.png";

                if ((myDialog.ShowDialog() == true) && (myDialog.CheckFileExists))
                {
                    try
                    {
                        new BitmapImage(new Uri(myDialog.FileName));
                    }
                    catch
                    {
                        MessageBox.Show("Invalid file.");
                        return null;
                    }
                    return (BitmapImage)new UriToImageSourceConverter().Convert(new Uri(myDialog.FileName), null, null, null);
                }
            }
            catch { MessageBox.Show("An Error occured while loading the file."); }
            return null;
        }

        public static string SaveFileAsGz()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.ValidateNames = true;
            saveDialog.Title = "Select location to save backup file";
            saveDialog.Filter = "Backup Files|*.gz";
            if ((saveDialog.ShowDialog() == true) &&
                (new System.IO.FileInfo(saveDialog.FileName).Extension.ToLower() == ".gz"))
            {
                return saveDialog.FileName;
            }
            else return null;
        }

        public static bool Exists(string pathToFile)
        {
            return File.Exists(pathToFile);
        }
        
    }
}
