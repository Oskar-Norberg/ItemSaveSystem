using _Project.SaveSystem.Subsystem;

namespace _Project.SaveSystem.SaveLoader
{
    public interface ISaveLoader
    {
        public void Save(string fileName, bool overrideSave = false);
        public void Load(string fileName);
        
        public void RegisterSaveSubsystem(ISaveSubsystem saveSubsystem);
        public void UnregisterSaveSubsystem(ISaveSubsystem saveSubsystem);
    }
}