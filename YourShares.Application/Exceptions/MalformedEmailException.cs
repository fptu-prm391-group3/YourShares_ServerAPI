using System;

namespace YourShares.Application.Exceptions
{
    public class MalformedEmailException : Exception
    {
        public MalformedEmailException()
        {
        }

        public MalformedEmailException(string message) : base(message)
        {
        }
    }
}