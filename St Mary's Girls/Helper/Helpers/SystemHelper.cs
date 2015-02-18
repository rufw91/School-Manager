using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class SystemHelper
    {
        public SystemHelper()
        {
            SystemInfo = GetSystemInfo();
        }
        public void SerializeObject()
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SystemInfo));
                System.IO.FileStream fs = new System.IO.FileStream(Constants.SysInfoFilePath,
                  System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

                serializer.Serialize(fs, SystemInfo);
                fs.Flush();

                fs.Close();
            }
            catch { }
        }
        public void SerializeObject(string filePath)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(SystemInfo));
                System.IO.FileStream fs = new System.IO.FileStream(filePath,
                  System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

                serializer.Serialize(fs, SystemInfo);
                fs.Flush();

                fs.Close();
            }
            catch { }
        }
        private SystemInfo GetSystemInfo()
        {
            SystemInfo s = new SystemInfo();
            return s;
        }
        public SystemInfo SystemInfo
        {
            get;
            set;
        }
        public static SystemInfo DeSerializeObject(string filename)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SystemInfo));

                serializer.UnknownNode += delegate(object sender, System.Xml.Serialization.XmlNodeEventArgs e)
                {
                    throw new ArgumentException("Unknown Node:" + e.Name + "\t" + e.Text);
                };

                serializer.UnknownAttribute += delegate(object sender, System.Xml.Serialization.XmlAttributeEventArgs e)
                {
                    throw new ArgumentException("Unknown attribute " +
            e.Attr.Name + "='" + e.Attr.Value + "'");
                };

                System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open);

                SystemInfo po = (SystemInfo)serializer.Deserialize(fs);
                return po;
            }
            catch { return null; }
        }
  
    }
}
