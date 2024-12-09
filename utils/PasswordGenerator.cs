using System;
using System.Linq;
using System.Text;

namespace sap_hana_user_export.utils
{
    internal class PasswordGenerator
    {
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";
        private const string SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        public string GeneratePassword(int length, int specialCharCount)
        {
            if (length < 1)
                throw new ArgumentException("Password length must be at least 1.", nameof(length));

            if (specialCharCount < 0)
                throw new ArgumentException("Number of special characters cannot be negative.", nameof(specialCharCount));

            if (specialCharCount > length)
                throw new ArgumentException("Number of special characters cannot be greater than the password length.", nameof(specialCharCount));

            Random random = new Random();

            // Generate special characters
            var specialChars = new char[specialCharCount];
            for (int i = 0; i < specialCharCount; i++)
            {
                specialChars[i] = SpecialChars[random.Next(SpecialChars.Length)];
            }

            // Generate the remaining characters
            var remainingCharCount = length - specialCharCount;
            var remainingChars = new char[remainingCharCount];
            var allChars = LowercaseChars + UppercaseChars + Digits;
            for (int i = 0; i < remainingCharCount; i++)
            {
                remainingChars[i] = allChars[random.Next(allChars.Length)];
            }

            // Combine and shuffle the characters
            var passwordChars = specialChars.Concat(remainingChars).ToArray();
            passwordChars = passwordChars.OrderBy(c => random.Next()).ToArray();

            // Convert to string and return
            return new string(passwordChars);
        }
    }
}
