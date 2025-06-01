using _Project.SaveSystem.DataLoading.Common;

namespace _Project.SaveSystem
{
    public interface ISaveableManager
    {
        public void BindSaveable(Saveable saveable);
        public void UnbindSaveable(Saveable saveable);

        public HeadSaveData GetSaveData();
        public void Load(HeadSaveData saveData);
    }
}