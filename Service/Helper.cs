using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HealthyLife.ResponseModel;

namespace HealthyLife.Service
{
    public static class Helper
    {
        public static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }


        public static string GenerateHash(string input, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(input + salt);
            var sHA256ManagedString = new SHA256Managed();
            var hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        
        public static bool AreEqual(string plainTextInput, string hashedInput, string salt)
        {
            var newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }
        
        public static bool Validator<T>(T data)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            var phoneRegex = new Regex(@"\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})");
            foreach (var property in data.GetType().GetProperties())
            {
                switch (property.Name)
                {
                    case "Email":
                    {
                        var match = emailRegex.Match(property.GetValue(data).ToString());
                        if (!match.Success)
                        {
                            return false;
                        }

                        break;
                    }
                    case "Fio":
                    {
                        if (property.GetValue(data).ToString().Length < 3)
                        {
                            return false;
                        }

                        break;
                    }
                    case "Phone":
                    {
                        var match = phoneRegex.Match(property.GetValue(data).ToString());
                        if (!match.Success)
                        {
                            return false;
                        }

                        break;
                    }
                }
            }

            return true;
        }
        
        public static ServiseResponse<T> ConvertToServiceResponse<T>(T data, string message = "", bool completed = true)
        {
            return new ServiseResponse<T>
            {
                Message = message,
                Completed = completed,
                Data = data
            };
        }
    }
}