using System.Collections.Generic;

namespace SaveLoadSystem
{
    //Class to hold save Data, no functionallity
    public class SaveData
    {
        public List<string> collectedItems;
        public SerializableDictionary<string, ItemPickUpSaveData> activeItems;
        public SerializableDictionary<string, ChestSaveData> chestDictionary;

        //Constructor
        public SaveData()
        {
            collectedItems = new List<string>();
            activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
            chestDictionary = new SerializableDictionary<string, ChestSaveData>();
        }
    }
}