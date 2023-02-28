using UnityEngine;
namespace SaveLoadSystem
{
    //Class to hold save Data, no functionallity
    [System.Serializable]
    public class SaveData
    {
        public PlayerData PlayerData = new PlayerData();
    }
}