using _Project.SaveSubsystems.Bindable;
using _Project.SaveSystem._Dev;
using UnityEngine;

public class TMP_SaveableCreator : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var go = new GameObject();
            
            go.AddComponent<Saveable>();
            go.AddComponent<TestSaver>();
        }
    }
}
