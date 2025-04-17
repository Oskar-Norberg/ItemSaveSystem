using System.Collections.Generic;
using ringo.ServiceLocator;
using UnityEngine;

namespace _Project.SaveSystem
{   
    public class SaveManager : MonoBehaviour
    {
        private List<Saveable> _saveables = new ();
        
        private void Awake()
        {
            ServiceLocator.Instance.Register<SaveManager>(this);
        }
        
        public void BindSaveable(Saveable saveable)
        {
            _saveables.Add(saveable);
        }

        public void OnApplicationQuit()
        {
            SaveGame();
        }

        public void SaveGame()
        {
            print("Saving!");
            
            foreach (var saveable in _saveables)
            {
                var saveDatas = saveable.GetSaveData();
                foreach (var data in saveDatas)
                {
                    print(data.GetData());
                }
            }
        }
    }
}
