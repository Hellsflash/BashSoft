using System;

namespace BashSoft.Exceptions
{
    public class InvalidNumberException : Exception
    {
        private const string Message = "{0} is not a valid number.";

        public InvalidNumberException(string notNumber) : base(string.Format(Message, notNumber))
        {
        }
    }
}
