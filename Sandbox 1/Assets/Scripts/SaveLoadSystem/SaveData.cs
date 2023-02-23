using UnityEngine;
namespace SaveLoadSystem
{
    [System.Serializable]
    //Class to hold save Data, no functionallity
    public class SaveData
    {
        public int index = 1;
        [SerializeField] private float myFloat = 5.8f;
    }
}