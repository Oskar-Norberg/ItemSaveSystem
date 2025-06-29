namespace ringo.SaveSystem.DataLoading.Common.Merging
{
    // TODO: Consider avdantages and disadvantages of using a static class for merging.
    public class DataMerger : IDataMerger
    {
        /// <summary>
        /// Tries to merge data.
        /// </summary>
        /// <param name="target">Object to merge into.</param>
        /// <param name="source">The object to merge from.</param>
        /// <returns>If merge was successfull.</returns>
        public bool TryMergeData(object target, object source)
        {
            // Objects are not mergeable.
            if (source is not IMergeable mergeableSource || target is not IMergeable mergeableTarget) 
                return false;
            
            mergeableTarget.Merge(source);
            return true;

        }
    }
}