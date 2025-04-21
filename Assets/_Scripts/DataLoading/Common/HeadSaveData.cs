using System.Collections.Generic;
using _Project.SaveSystem.Interfaces.DataLoading;

namespace _Project.SaveSystem.DataLoading.Common
{
    [System.Serializable]
    public class HeadSaveData : ILoadedData
    {
        // TODO: Will this be serialized if private?
        public List<SubSaveData> _subContainers;
        
        public HeadSaveData(List<SubSaveData> saveDatas)
        {
            _subContainers = saveDatas;
        }

        public HeadSaveData() : this(new List<SubSaveData>())
        {
        }
    
        public void AddSubContainer(SubSaveData subContainer)
        {
            _subContainers.Add(subContainer);
        }

        public bool TryGetDataByGUID(SerializableGuid guid, out Dictionary<string, SaveData> data)
        {
            // O(N) for each lookup, really really slow.
            foreach (var container in _subContainers)
            {
                if (Equals(container.GUID, guid))
                {
                    data = container.Data;
                    return true;
                }
            }

            data = null;
            return false;
        }
    }
}