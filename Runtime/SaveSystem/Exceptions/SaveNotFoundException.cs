using System;

namespace ringo.SaveSystem.Exceptions
{
    public class SaveNotFoundException : Exception
    {
        public SaveNotFoundException(string message) : base(message)
        {
        }
    }
}