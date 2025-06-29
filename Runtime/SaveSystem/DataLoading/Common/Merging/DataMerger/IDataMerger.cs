namespace ringo.SaveSystem.DataLoading.Common.Merging
{
    public interface IDataMerger
    {
        /// <summary>
        /// Tries to merge data.
        /// </summary>
        /// <param name="into">Object to merge into.</param>
        /// <param name="from">The object to merge from.</param>
        /// <returns>If merge was successful.</returns>
        // TODO: Same as the SaveMerger, consider if this should return a new instance or mutate the target.
        public bool TryMergeData(object into, object from);
    }
}