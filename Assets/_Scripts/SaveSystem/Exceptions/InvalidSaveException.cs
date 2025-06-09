using System;

namespace ringo.SaveSystem.Exceptions
{
    public class InvalidSaveException : Exception
    {
        public InvalidSaveException(string message) : base(message)
        {
        }
    }
}