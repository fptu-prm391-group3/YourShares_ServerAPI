using System;

namespace YourShares.Application.Exceptions
{
    public class UnauthorizedUser : Exception
    {
        public UnauthorizedUser()
        {
        }

        public UnauthorizedUser(string message) : base(message)
        {
        }
    }
}