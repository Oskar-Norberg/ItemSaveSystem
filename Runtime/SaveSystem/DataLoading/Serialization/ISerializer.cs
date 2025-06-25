namespace ringo.SaveSystem.DataLoading.Serialization
{
    public interface ISerializer
    {
        /**
         * <summary>Serialize object and return as string.</summary>
         * <param name="objectToSerialize">The object to serialize.</param>
         * <returns>String representing serialized object.</returns>
         */
        string Serialize<T>(T objectToSerialize);
        
        /**
         * <summary>Deserialize object.</summary>
         * <typeparam name="T">Type to deserialize object to.</typeparam>
         * <param name="serializedObject">String representing object to deserialize.</param>
         * <returns>The loaded data.</returns>
         */
        T Deserialize<T>(string serializedObject);
    }
}