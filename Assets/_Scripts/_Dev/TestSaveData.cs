using System;

namespace _Project.SaveSystem._Dev
{
    [Serializable]
    [Attributes.SaveData("TestSaveData")]
    public class TestSaveData : SaveData
    {
        public int X, Y, Z;
    }
}