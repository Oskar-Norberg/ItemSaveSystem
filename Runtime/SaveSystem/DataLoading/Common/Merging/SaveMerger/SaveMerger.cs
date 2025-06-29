namespace ringo.SaveSystem.DataLoading.Common.Merging
{
    public class SaveMerger : ISaveMerger
    {
        private readonly IDataMerger _dataMerger;

        public SaveMerger(IDataMerger dataMerger)
        {
            _dataMerger = dataMerger;
        }

        public bool TryMergeSaves(HeadSaveData into, HeadSaveData from)
        {
            // No data to merge.
            if (into == null || from == null)
            {
                return false;
            }

            // Copy data from 'from' into 'into' skipping values that already exist in 'into'.
            foreach (var subData in from._saveDatas)
            {
                var intoHasSubsystemData = into.TryGetSubsystemData(subData.Key, out var existingSubData);
                if (intoHasSubsystemData)
                {
                    // If data exists, merge it.
                    _dataMerger.TryMergeData(existingSubData, subData.Value);
                }
            }

            return true;
        }
    }
}