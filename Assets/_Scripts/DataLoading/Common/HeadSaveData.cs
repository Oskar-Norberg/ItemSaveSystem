using System.Collections.Generic;
using _Project.SaveSystem.Interfaces.DataLoading;

namespace _Project.SaveSystem.DataLoading.Common
{
    [System.Serializable]
    public class HeadSaveData : ILoadedData
    {
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
            // TODO: This should just be a dictionary.
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
            // Load all lh sub containers into HashSet.
            HashSet<SerializableGuid> lhGuids = new();
            foreach (var subSaveData in lh._subContainers)
            {
                lhGuids.Add(subSaveData.GUID);
            }
            
            // Add all rh sub containers that are not already in lh.
            foreach (var rhSubContainer in rh._subContainers)
            {
                bool alreadyExists = lhGuids.Contains(rhSubContainer.GUID);
                
                if (!alreadyExists)
                {
                    lh._subContainers.Add(rhSubContainer);
                }
            }

            return lh;
        }
    }
}