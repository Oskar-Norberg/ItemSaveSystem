namespace _Project.SaveSystem.Interfaces
{
    public interface ISerializer
    {
        // TODO: This is now inexplicably linked to the Saveable class. This should be made more generic. This is just a serializer. Not explicitly a part of the save system.
        // TODO: this should return a string rather than saving it to a file.
        /**
         * <summary>Serialize and save object to path.</summary>
         * <param name="objectToSerialize">The object to serialize.</param>
         * <param name="path">The path to save the data to. File extension is chosen in serializer.</param>
         * <returns>True upon success. False on failure.</returns>
         */
        bool Serialize(object objectToSerialize, string path);
        
        /**
         * <summary>Deserialize and load LoadedData from path.</summary>
         * <param name="path">The path to load the data from. File extensions handled by serializer.</param>
         * <returns>The loaded data.</returns>
         */
        T Deserialize<T>(string path);
    }
}