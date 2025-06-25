using System;
using ringo.SaveSystem.Attributes;

namespace _Project.SaveSystem._Dev
{
    [Serializable]
    [SaveData("TestBindableData2")]
    public class TestBindableData2
    {
        public string A, B, C;
    }
}