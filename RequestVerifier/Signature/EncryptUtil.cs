using System;
using System.Security.Cryptography;

namespace RequestVerifier.Signature
{
    /// <summary>
    /// </summary>
    internal static class EncryptUtil
    {
        /// <summary>
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static string ToMd5(this byte[] inputValue)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(inputValue);
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}