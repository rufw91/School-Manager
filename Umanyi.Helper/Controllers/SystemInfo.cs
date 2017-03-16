using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
namespace UmanyiSMS.Lib.Controllers
{
    public class SystemInfo
    {
        static SystemInfo()
        {
            string query = "SELECT * FROM Win32_Processor";
            ManagementObjectSearcher m = new ManagementObjectSearcher(query);
            ManagementObjectCollection mc = m.Get();
            foreach (ManagementObject i in mc)
            {
                ProcessorID = i["ProcessorId"].ToString();
            }
            query = "SELECT * FROM Win32_DiskDrive";
            ManagementObjectSearcher m1 = new ManagementObjectSearcher(query);
            ManagementObjectCollection mc1 = m1.Get();
            foreach (ManagementObject i1 in mc1)
            {
                HardDiskSerial = i1["SerialNumber"].ToString();
            }
            query = "SELECT * FROM Win32_BaseBoard";
            ManagementObjectSearcher m2 = new ManagementObjectSearcher(query);
            ManagementObjectCollection mc2 = m2.Get();
            foreach (ManagementObject i2 in mc2)
            {
                MotherBoardSerial = i2["SerialNumber"].ToString();
            }
            query = "SELECT * FROM Win32_BIOS";
            ManagementObjectSearcher m3 = new ManagementObjectSearcher(query);
            ManagementObjectCollection mc3 = m3.Get();
            foreach (ManagementObject i3 in mc3)
            {
                BIOSSerial = i3["SerialNumber"].ToString();
            }

            byte[] data = Encoding.UTF8.GetBytes(MotherBoardSerial + ProcessorID);
            using (SHA1Managed d = new SHA1Managed())
            {
                SystemHash = Convert.ToBase64String(d.ComputeHash(data));
            }
        }
     
        public static string MotherBoardSerial
        {
            get;
            set;
        }
        public static string ProcessorID
        {
            get;
            set;
        }
        public static string HardDiskSerial
        {
            get;
            set;
        }
        public static string BIOSSerial
        {
            get;
            set;
        }

        public static string SystemHash
        {
            get;
            set;
        }
    }
}
