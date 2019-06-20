using System;
using System.Security.Cryptography;
using System.Text;

namespace MazeRetreat.Api.Extensions
{
    public static class StringExtensions
    {
        public static String Base64Encode(this String input)
        {
            try
            {
                Byte[] bytesToEncode = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(bytesToEncode);
            }
            catch
            {
                return input;
            }
        }
        public static String Base64Decode(this String input)
        {
            try
            {
                Byte[] decodedBytes = Convert.FromBase64String(input);
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch
            {
                return input;
            }
        }

        public static String Md5Hash(this String input)
        {
            MD5 md5 = MD5.Create();
            Byte[] bytesToHash = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = md5.ComputeHash(bytesToHash);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}