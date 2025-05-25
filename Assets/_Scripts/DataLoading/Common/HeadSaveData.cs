using System.Collections.Generic;
using _Project.SaveSystem.Interfaces.DataLoading;

namespace _Project.SaveSystem.DataLoading.Common
{
    [System.Serializable]
    public class HeadSaveData : ILoadedData
    {
        // TODO: Any way I can have this private and still serialize it?
        public Dictionary<SerializableGuid, SubSaveData> _subContainers = new();
        
        public HeadSaveData(IEnumerable<KeyValuePair<SerializableGuid, SubSaveData>> saveDatas)
        {
            AddEntries(saveDatas);
        }

        public HeadSaveData()
        {
        }
        
        public void AddEntries(IEnumerable<KeyValuePair<SerializableGuid, SubSaveData>> saveDatas)
        {
            foreach (var saveData in saveDatas)
            {
                _subContainers.Add(saveData.Key, saveData.Value);
            }
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
                lhGuids.Add(subSaveData.Key);
            }
            
            // Add all rh sub containers that are not already in lh.
            foreach (var rhSubContainer in rh._subContainers)
            {
                bool alreadyExists = lhGuids.Contains(rhSubContainer.Key);
                
                if (!alreadyExists)
                {
                    lh._subContainers.Add(rhSubContainer.Key, rhSubContainer.Value);
                }
            }

            return lh;
        }
    
        public void AddSubContainer(SerializableGuid guid, SubSaveData subContainer)
        {
            _subContainers.Add(guid, subContainer);
        }

        public bool TryGetDataByGUID(SerializableGuid guid, out Dictionary<string, SaveData> data)
        {
            bool subContainerExists = _subContainers.TryGetValue(guid, out var subContainer);
            
            if (subContainerExists)
            {
                data = subContainer.Data;
                return true;
            }

            data = null;
            return false;
        }
    }
}