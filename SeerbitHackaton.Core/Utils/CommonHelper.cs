using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace SeerbitHackaton.Core.Utils
{
    public static class CommonHelper
    {
        public static string GenereateRandonAlphaNumeric()
        {
            return $"{Guid.NewGuid().ToString().Remove(5).ToUpper()}-{Guid.NewGuid().ToString().Remove(5).ToUpper()}";
        }

        public static bool IsValidEmail(this string email)
        {
            var e = new EmailAddressAttribute();
            return (!string.IsNullOrWhiteSpace(email) && e.IsValid(email));
        }

        public static string ToNigeriaMobile(this string str)
        {
            const string naijaPrefix = "234";
            if (string.IsNullOrEmpty(str))
                return str;

            str = str.TrimStart('+');
            var prefix = str.Remove(3);

            if (prefix.Equals(naijaPrefix))
            {
                return str;
            }
            str = str.TrimStart('0');
            str = naijaPrefix + str;
            return str;
        }

        public static string RandomNumber(int length)
        {
            var rand = new Random(0);

            var otRand = string.Empty;

            for (int i = 0; i < length; i++)
            {
                int temp = rand.Next(9);
                otRand += temp;
            }

            return otRand;
        }

        public static string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = string.Concat(s, random.Next(10).ToString());
            return s;
        }

        public static string GenerateTimeStampedFileName(string fileName)
        {
            string ext = fileName.Split(".")[fileName.Split(".").Length - 1];
            string path = $"{fileName.Substring(0, fileName.Length - ext.Length)}_{DateTime.Now.Ticks}.{ext}";
            path = path.Replace(" ", "");
            path = GetSafeFileName(path);
            return path;
        }

        private static string GetSafeFileName(string name, char replace = '_')
        {
            char[] invalids = Path.GetInvalidFileNameChars();
            return new string(name.Select(c => invalids.Contains(c) ? replace : c).ToArray());
        }

        public static T GetEnumFromString<T>(Enum value, string str) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            object result;
            var checkConvert = Enum.TryParse(value.GetType(), str, out result);
            return (T)result;
        }


        public static byte[] GetBase64StringFromImage(this string str)
        {

            if (string.IsNullOrEmpty(str))
                return null;

            var filepath = Path.Combine("Filestore", str.ToString());

            if (File.Exists(filepath))
                 return File.ReadAllBytes(filepath);

            return null;

        }
    }
}