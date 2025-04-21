using ringo.SceneSystem;
using UnityEngine;

public class TMP_GOTOMainScene : MonoBehaviour
{
    [SerializeField] private SceneGroup mainSceneGroup;
    
    void Start()
    {
        SceneManager.LoadSceneGroup(mainSceneGroup);
    }
}
