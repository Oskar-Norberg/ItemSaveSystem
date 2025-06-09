using System;

namespace _Project.SaveSystem._Dev
{
    [Serializable]
    [ringo.SaveSystem.Attributes.SaveData("TestSaveData")]
    public class TestSaveData : ringo.SaveModules.Subsystems.Bindable.SaveData
    {
        public int X, Y, Z;
    }
}