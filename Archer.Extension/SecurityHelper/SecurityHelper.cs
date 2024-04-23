using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;

namespace Archer.Extension.SecurityHelper
{
    /// <summary>
    /// keyConn: 加密金鑰, ivConn: 初始向量 </br>
    /// keyData: 加密金鑰, ivData: 初始向量
    /// </summary>
    public class SecurityHelper
    {
        private byte[] _keyConn;
        private byte[] _ivConn;
        private byte[] _keyData;
        private byte[] _ivData;

        /// <summary>
        /// keyConn: 加密金鑰, ivConn: 初始向量 </br>
        /// keyData: 加密金鑰, ivData: 初始向量
        /// </summary>
        /// <param name="keyConn"></param>
        /// <param name="ivConn"></param>
        /// <param name="keyData"></param>
        /// <param name="ivData"></param>
        public SecurityHelper(string keyConn, string ivConn, string keyData, string ivData)
        {
            _keyConn = Encoding.Unicode.GetBytes(keyConn);
            _ivConn = Encoding.Unicode.GetBytes(ivConn);
            _keyData = Encoding.Unicode.GetBytes(keyData);
            _ivData = Encoding.Unicode.GetBytes(ivData);
        }

        public string EncryptConn(string data) => Encrypt(data, _keyConn, _ivConn);
        public string DecryptConn(string data) => Decrypt(data, _keyConn, _ivConn);

        public string EncryptData(string data) => Encrypt(data, _keyData, _ivData);
        public string DecryptData(string data) => Decrypt(data, _keyData, _ivData);

        private static string Encrypt(string data, byte[] key, byte[] iv)
        {
            var inputBytes = Encoding.UTF8.GetBytes(data);
            var engine = new RijndaelEngine(256);
            var blockCipher = new CbcBlockCipher(engine);
            var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
            var keyParam = new KeyParameter(key);
            var keyParamWithIv = new ParametersWithIV(keyParam, iv, 0, 32);
            cipher.Init(true, keyParamWithIv);
            var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            var length = cipher.ProcessBytes(inputBytes, outputBytes, 0);

            //Do the final block
            cipher.DoFinal(outputBytes, length);

            return Convert.ToBase64String(outputBytes);
        }

        /// <summary>
        /// decryt input. If fail, return input data without doing anything
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string Decrypt(string data, byte[] key, byte[] iv)
        {
            try
            {
                var inputBytes = Convert.FromBase64String(data);
                var engine = new RijndaelEngine(256);
                var blockCipher = new CbcBlockCipher(engine);
                CipherUtilities.GetCipher("AES/CTR/NoPadding");
                IBufferedCipher cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
                var keyParam = new KeyParameter(key);
                var keyParamWithIv = new ParametersWithIV(keyParam, iv, 0, 32);
                cipher.Init(false, keyParamWithIv);
                var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
                var length = cipher.ProcessBytes(inputBytes, outputBytes, 0);

                // Do the final block
                cipher.DoFinal(outputBytes, length);

                return Encoding.UTF8.GetString(outputBytes).Split('\0')[0];
            }
            catch
            {
                return data;
            }
        }

        /// <summary>
        /// For password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5 object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            int i;
            for (i = 0; i <= data.Length - 1; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// 將密碼用SHA256進行雜湊
        /// </summary>
        /// <param name="password">密碼明文</param>
        /// <returns></returns>
        public byte[] GetSha256Hash(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            SHA256 sha256 = SHA256.Create();

            return sha256.ComputeHash(bytes);
        }
    }
}