using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharpUpdate
{
    internal enum HashType
    {
        MD5,
        SHA1,
        SHA512
    }

    internal static class Hasher
    {
        internal static string HashFile(string filePath, HashType algo)
        {
            string hexStr;

            FileStream fstream = new FileStream(filePath, FileMode.Open);

            switch (algo)
            {
                case HashType.MD5:
                    hexStr = MD5.Create().ComputeHash(fstream).ToHex(true);
                    break;
                case HashType.SHA1:
                    hexStr = SHA1.Create().ComputeHash(fstream).ToHex(true);
                    break;
                case HashType.SHA512:
                    hexStr = SHA512.Create().ComputeHash(fstream).ToHex(true);
                    break;

                default:
                    hexStr = "";
                    break;
            }

            fstream.Close();

            return hexStr;
        }

        private static string ToHex(this byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
                result.Append(b.ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}
