using System.Threading.Tasks;
using ringo.SaveSystem.Subsystem;

namespace ringo.SaveSystem.Managers
{
    public interface ISaveLoader
    {
        public void Save(string fileName, bool overrideSave = false);
        
        // TODO: Offer implementation of both synchronous and asynchronous save/load methods.
        public Task Load(string fileName);
        
        public void RegisterSaveSubsystem(ISaveSubsystem saveSubsystem);
        public void UnregisterSaveSubsystem(ISaveSubsystem saveSubsystem);
    }
}