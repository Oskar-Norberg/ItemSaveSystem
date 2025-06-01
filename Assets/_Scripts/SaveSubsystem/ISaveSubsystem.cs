namespace _Project.SaveSystem.SaveSubsystem
{
    public interface ISaveSubsystem
    {
        SaveData GetSaveData();
        void Load(SaveData saveData);
    }
}