using System.Collections.Generic;

[System.Serializable]
public class HeadJSONContainer
{
    public List<SubJSONContainer> SubContainers = new();
    
    public void AddSubContainer(SubJSONContainer subContainer)
    {
        SubContainers.Add(subContainer);
    }
}