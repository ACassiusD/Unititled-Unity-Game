using UnityEngine;

namespace SaveLoadSystem
{
    //Class to manage saveing and loading a SaveData object.
    public class SaveGameManager : MonoBehaviour
    {
        public static SaveData data;

        private void Awake()
        {
            data = new SaveData();
            SaveLoad.OnLoadGame += LoadData;
        }

        public void DeleteData()
        {
            SaveLoad.DeleteSaveData();
        }

        public static void SaveData()
        {
            var saveData = data;
            SaveLoad.SaveGame(saveData);
        }

        public static void LoadData(SaveData _data)
        {
            Debug.Log("Loading Game...");
            data = _data;
        }
        
        public static void TryLoadData()
        {
            SaveLoad.LoadGame();
        }
    }

}