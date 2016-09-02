namespace TokenGenerator
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Utility used to encrypt/decrypt tokens. Provides encoding and decoding services, if provided with a password and salt.
    /// </summary>
    public class CipherUtility
    {
        /// <summary>
        /// Encrypts the given value using the provided symmetric algorithm, password and salt.
        /// </summary>
        /// <param name="value">The original value.</param>
        /// <returns>The encrypted value.</returns>
        public string Encrypt(string value)
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(Settings.Default.WrKey, Encoding.Unicode.GetBytes(Settings.Default.WrSalt));

            SymmetricAlgorithm algorithm = new RijndaelManaged();

            var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);

            var transform = algorithm.CreateEncryptor(rgbKey, rgbIv);

            using (var buffer = new MemoryStream())
            {
                using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }
    }
}
