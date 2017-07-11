using System;

namespace BashSoft.Exceptions
{
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string command) : base($"The command {command} is invalid")
        {
        }
    }
}
