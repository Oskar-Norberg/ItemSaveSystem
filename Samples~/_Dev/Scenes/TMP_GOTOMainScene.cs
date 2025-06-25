using ringo.SceneSystem;
using UnityEngine;

public class TMP_GOTOMainScene : MonoBehaviour
{
    [SerializeField] private SceneGroupSO mainSceneGroup;
    
    void Start()
    {
        SceneManager.LoadSceneGroup(mainSceneGroup);
    }
}
