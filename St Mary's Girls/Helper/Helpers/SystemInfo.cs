using System.Management;
namespace Helper
{
    public class SystemInfo
    {
        public SystemInfo()
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
        }
        public string MotherBoardSerial
        {
            get;
            set;
        }
        public string ProcessorID
        {
            get;
            set;
        }        
        public string HardDiskSerial
        {
            get;
            set;
        }
        public string BIOSSerial
        {
            get;
            set;
        }
    }
}
