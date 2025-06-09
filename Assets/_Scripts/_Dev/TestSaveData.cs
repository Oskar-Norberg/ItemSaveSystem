using System;
using _Project.SaveSubsystems.Bindable;

namespace _Project.SaveSystem._Dev
{
    [Serializable]
    [Attributes.SaveData("TestSaveData")]
    public class TestSaveData : SaveData
    {
        public int X, Y, Z;
    }
}