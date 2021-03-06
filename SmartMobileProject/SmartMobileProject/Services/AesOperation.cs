using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SmartMobileProject.Services
{
    class AesOperation
    {
        public static string EncryptString(string key, string plainText)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    byte[] key2 =
                    {
                        13,45,163,95,158,94,26,129,59,6,97,120,93,18,11,138,141,
                        0,171,79,231,159,143,25,137,76,239,159,36,200,55,251
                    };  
                    aes.Key = key2;
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }
                Console.WriteLine("The file was encrypted.");
                return Convert.ToBase64String(array);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
                return string.Empty;
            }
            
        }
        public static string DecryptString(string key, string cipherText)  
        {  
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    byte[] key2 =
                   {
                        13,45,163,95,158,94,26,129,59,6,97,120,93,18,11,138,141,
                        0,171,79,231,159,143,25,137,76,239,159,36,200,55,251
                    };
                    aes.Key = key2;
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                Console.WriteLine("The decrypted original message");
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The decryption failed. {ex}");
                return string.Empty;
            }
             
        }  
    }
}
