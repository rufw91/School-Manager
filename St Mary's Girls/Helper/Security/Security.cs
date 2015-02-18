using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Helper.Security
{
    public sealed class DataProtection
    {
        private DataProtection()
        { }
        public static byte[] GetHash(byte[] data)
        {
            SHA512 shaM = new SHA512Managed();
            byte[] result = shaM.ComputeHash(data); ;
            return result;
        }

        public static bool CompareHash(byte[] object1, byte[] object2)
        {
            byte[] a1 = GetHash(object1);
            byte[] a2 = GetHash(object2);
            bool result = ArraysAreEqual(a1, a2);
            return result;
        }

        public static Task<bool> CompareHashAsync(byte[] object1, byte[] object2)
        {
            return Task.Run<bool>(()=>CompareHash(object1,object2));
        }

        private static bool ArraysAreEqual(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length) return false;
            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i]) return false;
            return true;
        }
    }
}
