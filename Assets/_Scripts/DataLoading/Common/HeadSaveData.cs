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
        
        // TODO: Consider making an extension class to hold this merge.
        /**
         * <summary>
         * Merge two HeadSaveData objects using the lh as a base.
         * This means lh will be used as the most recent save data.
         * rh will be merged into lh.
         * </summary>
         * <param name="lh">The head save data to merge into.</param>
         * <param name="rh">The child save data to take missing fields from.</param>
         */
        public static HeadSaveData Merge(HeadSaveData lh, HeadSaveData rh)
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
    }
}