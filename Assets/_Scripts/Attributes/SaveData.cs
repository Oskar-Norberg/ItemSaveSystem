using System;

namespace _Project.SaveSystem.Attributes
{
    // TODO: Add alias support for attributes.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SaveData : Attribute
    {
        public string Name
        {
            get; private set;
        }

        public SaveData(string name)
        {
            Name = name;
        }
    }
}