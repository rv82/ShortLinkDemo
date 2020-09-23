using System;

namespace ShortLinkDemo.Utils
{
    public static class StringUtils
    {
        public static string GenRandomString(int length)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";
            string alphabet = letters + letters.ToUpper() + numbers;

            char[] result = new char[length];
            Random random = new Random();
            int position;

            for (int i = 0; i < length; i++)
            {
                position = random.Next(alphabet.Length - 1);
                result[i] = alphabet[position];
            }
            return string.Concat(result);
        }
    }
}