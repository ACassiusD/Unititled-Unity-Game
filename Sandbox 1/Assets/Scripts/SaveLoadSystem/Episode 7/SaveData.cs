using UnityEngine;
namespace SaveLoadSystem
{
    //Class to hold save Data, no functionallity
    public class SaveData
    {
        public SerializableDictionary<string, ChestSaveData> chestDictionary;
       // public PlayerData PlayerData = new PlayerData();

        //Constructor
        public SaveData()
        {
            chestDictionary = new SerializableDictionary<string, ChestSaveData> (); 
        }
    }
}