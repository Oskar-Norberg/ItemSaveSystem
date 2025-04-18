using ringo.EventSystem;

namespace _Project.SaveSystem.Events
{
    public struct LoadGameResponse : IEventResponse
    {
        public LoadedData LoadedData;

        public LoadGameResponse(LoadedData loadedData)
        {
            LoadedData = loadedData;
        }
    }
}