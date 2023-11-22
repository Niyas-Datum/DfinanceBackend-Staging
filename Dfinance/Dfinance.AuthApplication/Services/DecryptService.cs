using Dfinance.AuthApplication.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.AuthApplication.Services
{
    public class DecryptService : IDecryptService
    {
        public  string Decrypt(string TextToBeDecrypted)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            string text = "parva@123*";
            try
            {
                byte[] array = Convert.FromBase64String(TextToBeDecrypted);
                byte[] bytes = Encoding.ASCII.GetBytes(text.Length.ToString());
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(text, bytes);
                ICryptoTransform transform = rijndaelManaged.CreateDecryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(array);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
                byte[] array2 = new byte[array.Length];
                int count = cryptoStream.Read(array2, 0, array2.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.Unicode.GetString(array2, 0, count);
            }
            catch
            {
                return TextToBeDecrypted;
            }
        }
    }
}
