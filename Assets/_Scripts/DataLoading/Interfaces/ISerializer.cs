using System.Collections.Generic;
using _Project.SaveSystem.Interfaces.DataLoading;

namespace _Project.SaveSystem.Interfaces
{
    public interface ISerializer
    {
        // TODO: This is now inexplicably linked to the Saveable class. This should be made more generic. This is just a serializer. Not explicitly a part of the save system.
        /**
         * <summary>Serialize and save SaveData to path.</summary>
         * <param name="saveData">The SaveData to serialize.</param>
         * <param name="path">The path to save the data to. File extension is chosen in serializer.</param>
         * <returns>True upon success. False on failure.</returns>
         */
        bool Serialize(List<Saveable> saveables, string path);
        
        /**
         * <summary>Deserialize and load LoadedData from path.</summary>
         * <param name="path">The path to load the data from. File extensions handled by serializer.</param>
         * <returns>The loaded data.</returns>
         */
        ILoadedData Deserialize(string path);
    }
}