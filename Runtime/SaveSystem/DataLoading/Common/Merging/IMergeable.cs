namespace ringo.SaveSystem.DataLoading.Common.Merging
{
    // TODO: Look into if this can somehow be made strongly-typed.
    public interface IMergeable
    {
        public void Merge(object data);
    }
}