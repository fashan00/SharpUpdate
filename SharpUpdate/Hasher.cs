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
            switch (algo)
            {
                case HashType.MD5:
                    return MD5.Create().ComputeHash(new FileStream(filePath, FileMode.Open)).ToHex(false);
                case HashType.SHA1:
                    return SHA1.Create().ComputeHash(new FileStream(filePath, FileMode.Open)).ToHex(false);
                case HashType.SHA512:
                    return SHA512.Create().ComputeHash(new FileStream(filePath, FileMode.Open)).ToHex(false);

                default:
                    return "";
            }

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
