using Dfinance.Application.Services.Interface;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dfinance.Application.Services
{
    public class EncryptService : IEncryptService
    {
        public string Encrypt(string textToBeEncrypted)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            string text = "parva@123*";
            byte[] bytes = Encoding.Unicode.GetBytes(textToBeEncrypted);
            byte[] bytes2 = Encoding.ASCII.GetBytes(text.Length.ToString());
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(text, bytes2);
            ICryptoTransform transform = rijndaelManaged.CreateEncryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] inArray = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(inArray);
        }
    }
}
