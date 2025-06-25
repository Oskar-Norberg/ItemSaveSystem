using System;
using ringo.SaveSystem.Attributes;

namespace _Project.SaveSystem._Dev
{
    [Serializable]
    [SaveData("TestBindableData")]
    public class TestBindableData
    {
        public int X, Y, Z;
    }
}