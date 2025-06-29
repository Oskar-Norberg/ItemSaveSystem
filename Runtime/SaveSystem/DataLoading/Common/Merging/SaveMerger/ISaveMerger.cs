namespace ringo.SaveSystem.DataLoading.Common.Merging
{
    public interface ISaveMerger
    {
        /// <summary>
        /// Tries to merge HeadSaveData.
        /// </summary>
        /// <param name="into">SaveData to merge into.</param>
        /// <param name="from">The SaveData to merge from.</param>
        /// <returns>If merge was successful.</returns>
        // TODO: Consider if this should return a new instance of HeadSaveData or keep modifying the target. The former is slightly less C# like.
        bool TryMergeSaves(HeadSaveData into, HeadSaveData from);
    }
}