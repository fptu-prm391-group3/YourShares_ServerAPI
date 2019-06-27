using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace YourShares.Domain.Util
{
    public class HashingUtils
    {
        private static readonly string[] algorithms = new[] { "SHA256", "SHA1", "SHA512", "MD5", "SHA384" };

        public static DataHashing GetHashData(string text)
        {
            Random rand = new Random();
            int index = rand.Next(algorithms.Length);
            string dataHased = String.Empty;
            if (algorithms[index].Equals("SHA256"))
            {
                dataHased = SHA256(text);
            }
            if (algorithms[index].Equals("SHA1"))
            {
                dataHased = SHA1(text);
            }
            if (algorithms[index].Equals("MD5"))
            {
                dataHased = MD5(text);
            }
            if (algorithms[index].Equals("SHA512"))
            {
                dataHased = SHA512(text);
            }
            if (algorithms[index].Equals("SHA384"))
            {
                dataHased = SHA384(text);
            }


            return new DataHashing
            {
                HashType = algorithms[index],
                DataHashed = dataHased

            };
        }

        public static string HashString(string text, string type)
        {
            string dataHased = "";
            if (type.Equals("SHA256"))
            {
                dataHased = SHA256(text);
            }
            if (type.Equals("SHA1"))
            {
                dataHased = SHA1(text);
            }
            if (type.Equals("MD5"))
            {
                dataHased = MD5(text);
            }
            if (type.Equals("SHA512"))
            {
                dataHased = SHA512(text);
            }
            if (type.Equals("SHA384"))
            {
                dataHased = SHA384(text);
            }
            return dataHased;
        }
        private static string GenerateHashString(HashAlgorithm algo, string text)
        {
            // Compute hash from text parameter
            algo.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Get has value in array of bytes
            var result = algo.Hash;

            // Return as hexadecimal string
            return string.Join(
                string.Empty,
                result.Select(x => x.ToString("x2")));
        }

        private static string MD5(string text)
        {
            var result = default(string);

            using (var algo = new MD5CryptoServiceProvider())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        private static string SHA1(string text)
        {
            var result = default(string);

            using (var algo = new SHA1Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        private static string SHA256(string text)
        {
            var result = default(string);

            using (var algo = new SHA256Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        private static string SHA384(string text)
        {
            var result = default(string);

            using (var algo = new SHA384Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        private static string SHA512(string text)
        {
            var result = default(string);

            using (var algo = new SHA512Managed())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }
    }

    public class DataHashing
    {
        public string HashType { get; set; }

        public string DataHashed { get; set; }
    }
}