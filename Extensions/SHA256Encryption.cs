using System;
using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
	public class SHA256Encryption
	{
        public static string SHA256Hash(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            SHA256 shaM = new SHA256Managed();
            var hashBytes = shaM.ComputeHash(data);
            return Convert.ToBase64String(hashBytes);
        }
    }
}

