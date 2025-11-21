using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RMSExternalApi.Commons
{
    public class Aes128Encryption
    {

        private static object lockObj = new object();
        private Aes128Encryption() { }
        private static Aes128Encryption _instance;
        public static Aes128Encryption Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Aes128Encryption(key, iv);
                        }
                    }
                }
                return _instance;
            }
        }

        private AesManaged _aes;
        private byte[] _key;
        private byte[] _iv;
        private static string key = Constant.KEY_AES128;
        private static string iv = Constant.IV_AES128;
        private Aes128Encryption(string key, string iv)
        {
            if (key.Length != 16 || iv.Length != 16)
            {
                throw new ArgumentException("Key and IV must be 16 bytes each.");
            }

            _key = Encoding.UTF8.GetBytes(key);
            _iv = Encoding.UTF8.GetBytes(iv);

            _aes = new AesManaged();
            _aes.KeySize = 128;
            _aes.Mode = CipherMode.CBC;
            _aes.Padding = PaddingMode.PKCS7;
        }

        private byte[] Encrypt(byte[] data)
        {
            using (ICryptoTransform encryptor = _aes.CreateEncryptor(_key, _iv))
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        private byte[] Decrypt(byte[] data)
        {
            using (ICryptoTransform decryptor = _aes.CreateDecryptor(_key, _iv))
            {
                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream msOutput = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                msOutput.Write(buffer, 0, bytesRead);
                            }
                            return msOutput.ToArray();
                        }
                    }
                }
            }
        }

        public string Encrypt(string data)
        {
            try
            {
                byte[] encryptedBytes = Encrypt(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Encryption failed: " + ex.Message);
            }
        }

        public string Decrypt(string data)
        {
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(data);
                byte[] decryptedBytes = Decrypt(encryptedBytes);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Decryption failed: " + ex.Message);
            }
        }
    }

}