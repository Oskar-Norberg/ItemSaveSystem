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

        public static HeadSaveData operator +(HeadSaveData lh, HeadSaveData rh)
        {
            HeadSaveData newHeadSaveData = new HeadSaveData(rh._subContainers);
            
            return AddNonPresentSaveData(lh, rh);
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
                    if (container.Data == null)
                        break;
                    
                    if (container.Data.Count == 0)
                        break;
                    
                    data = container.Data;
                    return true;
                }
            }

            data = null;
            return false;
        }

        private static HeadSaveData AddNonPresentSaveData(HeadSaveData lh, HeadSaveData rh)
        {
            // TODO: O(N^2)
            foreach (var rhSubContainer in rh._subContainers)
            {
                bool found = lh._subContainers.Exists(subContainer => Equals(subContainer.GUID, rhSubContainer.GUID));
                
                if (!found)
                {
                    lh._subContainers.Add(rhSubContainer);
                }
            }

            return lh;
        }
    }
}