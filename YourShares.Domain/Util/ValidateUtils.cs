using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace YourShares.Domain.Util
{
    public static class ValidateUtils
    {
        public static bool IsNullOrEmpty(string data)
        {
            return string.IsNullOrEmpty(data);
        }

        public static bool IsNullOrEmpty(Guid data)
        {
            return data == Guid.Empty;
        }

        public static bool IsNumber(string data)
        {
            long check = 0;
            var isNumber = long.TryParse(data, out check);
            return isNumber;
        }

        public static bool IsMail(string emailAddress)
        {
            try
            {
                var m = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsPhone(string phone)
        {
            var regex = new Regex(@"^(\+)?[0-9]{8,12}$");
            return regex.IsMatch(phone);
        }
    }
}