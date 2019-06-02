using System;

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
    }
}
