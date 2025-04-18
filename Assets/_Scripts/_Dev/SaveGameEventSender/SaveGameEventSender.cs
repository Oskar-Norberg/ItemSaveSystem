using _Project.SaveSystem.Events;
using ringo.EventSystem;
using UnityEngine;

namespace _Project.SaveSystem.SaveGameEventSender
{
    public class SaveGameEventSender : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                EventBus.Publish(new SaveGameRequest());
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                EventBus.Publish(new LoadGameRequest());
            }
        }
    }
}