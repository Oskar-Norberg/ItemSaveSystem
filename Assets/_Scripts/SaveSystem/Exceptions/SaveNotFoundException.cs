using System;

namespace _Project.SaveSystem.Exceptions
{
    public class SaveNotFoundException : Exception
    {
        public SaveNotFoundException(string message) : base(message)
        {
        }
    }
}