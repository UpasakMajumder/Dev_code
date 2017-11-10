using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Kadena2.MicroserviceClients.Helpers
{
    static class Utils
    {
        private const string ValidUrlCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public static string UrlEncode(string data)
        {
            var encoded = new StringBuilder();
            foreach (char symbol in Encoding.UTF8.GetBytes(data))
            {
                if (ValidUrlCharacters.IndexOf(symbol) != -1)
                {
                    encoded.Append(symbol);
                }
                else
                {
                    encoded.Append("%").Append(string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)symbol));
                }
            }
            return encoded.ToString();
        }

        public static byte[] Hash(string value)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        public static byte[] GetKeyedHash(string key, string value)
        {
            return GetKeyedHash(Encoding.UTF8.GetBytes(key), value);
        }

        public static byte[] GetKeyedHash(byte[] key, string value)
        {
            KeyedHashAlgorithm mac = new HMACSHA256(key);
            return mac.ComputeHash(Encoding.UTF8.GetBytes(value));
        }

        public static string ToHex(byte[] data)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }
    }
}
