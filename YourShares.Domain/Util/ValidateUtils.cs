using System;
using System.Net.Mail;

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
            if (data == null || data == Guid.Empty)
            {
                return true;
            }
            return false;
        }

        public static bool IsNumber(String data)
        {
            long check = 0;
            bool isNumber = long.TryParse(data, out check);
            return isNumber;
        }

        public static bool IsMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
