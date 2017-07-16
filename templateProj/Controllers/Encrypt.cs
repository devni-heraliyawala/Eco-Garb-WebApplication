using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace templateProj.Controllers
{
    public class Encrypt
    {
        //Encrypting the password
        public string HashPassword(string pw, string salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(pw + salt);
            System.Security.Cryptography.SHA256Managed hashstring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            
            return ByteArrayToHexString(hash);
        }
        
        public string CreateSalt()
        {
            int size = 10;//new Random().Next(10, 15);
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);
            string HexAlphabet = "0123456789ABCDEF";

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }

            return Result.ToString();
        }
    }
}