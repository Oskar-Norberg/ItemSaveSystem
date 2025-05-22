using System;

namespace _Project.SaveSystem.Exceptions
{
    public class InvalidSaveException : Exception
    {
        public InvalidSaveException(string message) : base(message)
        {
        }
    }
}