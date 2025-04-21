using System.Collections.Generic;
using _Project.SaveSystem;

[System.Serializable]
public struct SubJSONContainer
{
    public SerializableGuid GUID;
    public SaveableType SaveableType;
    public Dictionary<string, SaveData> Data;

    public SubJSONContainer(SerializableGuid guid, SaveableType saveableType, Dictionary<string, SaveData> data)
    {
        GUID = guid;
        Data = data;
        SaveableType = saveableType;
    }
}