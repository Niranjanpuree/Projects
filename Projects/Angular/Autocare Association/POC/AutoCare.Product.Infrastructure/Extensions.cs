using System;

namespace AutoCare.Product.Infrastructure
{
    public static class Extensions
    {
       
        public static string ToCamelCase(this string stringToConvert, char separator = ' ')
        {
            // If there are 0 or 1 characters, just return the string.
            if (stringToConvert == null || stringToConvert.Length < 2)
                return stringToConvert;

            // Split the string into words.
            string[] words = stringToConvert.Split(
                new[] {separator},
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].Substring(0, 1).ToLower() + words[0].Substring(1);

            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }
        
    }
}
