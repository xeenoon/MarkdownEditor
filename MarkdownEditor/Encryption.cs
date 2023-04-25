using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownEditor
{
    internal class Encryption
    {
        public static byte[] EncryptFile(string filePath, string key)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] keyBytes = new byte[aes.KeySize / 8];
                byte[] ivBytes = new byte[aes.BlockSize / 8];

                byte[] keyInputBytes = Encoding.UTF8.GetBytes(key);
                int keyLength = Math.Min(keyInputBytes.Length, keyBytes.Length);
                Array.Copy(keyInputBytes, keyBytes, keyLength);

                aes.Key = keyBytes;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileBytes, 0, fileBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] Encrypt(string filecontents, string key)
        {
            byte[] fileBytes = Encoding.ASCII.GetBytes(filecontents);

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] keyBytes = new byte[aes.KeySize / 8];
                byte[] ivBytes = new byte[aes.BlockSize / 8];

                byte[] keyInputBytes = Encoding.UTF8.GetBytes(key);
                int keyLength = Math.Min(keyInputBytes.Length, keyBytes.Length);
                Array.Copy(keyInputBytes, keyBytes, keyLength);

                aes.Key = keyBytes;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileBytes, 0, fileBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        public static string DecryptFile(string filePath, string key)
        {
            byte[] encryptedBytes = File.ReadAllBytes(filePath);

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] keyBytes = new byte[aes.KeySize / 8];
                byte[] ivBytes = new byte[aes.BlockSize / 8];

                byte[] keyInputBytes = Encoding.UTF8.GetBytes(key);
                int keyLength = Math.Min(keyInputBytes.Length, keyBytes.Length);
                Array.Copy(keyInputBytes, keyBytes, keyLength);

                aes.Key = keyBytes;

                byte[] iv = new byte[ivBytes.Length];
                Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}
