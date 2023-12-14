using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NLog;

namespace HealthyLife.Utility
{
    public class EncryptionService : IEncryptionService, IJsonEncryptionService
    {
        private const string ENCRYPTION_KEY = "BHTEU/EEHcA8RBbWI0r/7zgPZsn6xtHqRj7X8qkeosA=";
        private const string INITIALIZATION_VECTOR = "UF6avPUNcDxnVBYg+MbSPw==";

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public string Encrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                this._logger.Error($"{nameof(data)} is null or empty");
                return null;
            }

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.IV = Convert.FromBase64String(INITIALIZATION_VECTOR);
                    aes.Key = Convert.FromBase64String(ENCRYPTION_KEY);
                    ICryptoTransform encryptor = aes.CreateEncryptor();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(cryptoStream, Encoding.Unicode))
                            {
                                writer.Write(data);
                            }
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (CryptographicException ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return null;
            }
        }

        public string Decrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                this._logger.Error($"{nameof(data)} is null or empty");
                return null;
            }

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.IV = Convert.FromBase64String(INITIALIZATION_VECTOR);
                    aes.Key = Convert.FromBase64String(ENCRYPTION_KEY);
                    ICryptoTransform decryptor = aes.CreateDecryptor();

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data)))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cryptoStream, Encoding.Unicode))
                            {
                                return reader.ReadLine();
                            }
                        }
                    }
                }
            }
            catch (CryptographicException ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);

                return null;
            }
        }

        public string Hash(string data)
        {
            if (data == null)
            {
                this._logger.Error($"Input data is null");

                return null;
            }

            using (SHA256 sha256Hash = SHA256.Create("SHA-256"))
            {
                byte[] sourceArray = Encoding.UTF8.GetBytes(data);

                byte[] hash = sha256Hash.ComputeHash(sourceArray);

                return Encoding.UTF8.GetString(hash);
            }
        }

        public bool CompareWithHash(string data, string hash)
        {
            return this.Hash(data) == hash;
        }

        public TEntity Decrypt<TEntity>(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            string decryptedData = this.Decrypt(data);

            return JsonHelper.Deserialize<TEntity>(decryptedData);
        }

        public string Encrypt<TEntity>(TEntity entity)
        {
            string rawEntityData = JsonHelper.Serialize(entity);

            string encryptedData = this.Encrypt(rawEntityData);

            return encryptedData;
        }
    }
}
