using UnityEngine;
using System.IO;

namespace SaveLoadSystem
{
    //Class to manage saveing and loading a SaveData object.
    public static class SaveGameManager
    { 
        public static SaveData CurrentSaveData = new SaveData();

        public const string SaveDirectory = "/SaveData/";
        public const string FileName = "SaveGame.sav";

        public static bool Save()
        {
            //Application.persistentDataPath is a unity directory path that works cross platform for storage.
            var dir = Application.persistentDataPath + SaveDirectory;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonUtility.ToJson(CurrentSaveData, true);
            File.WriteAllText(dir + FileName, json);

            //Copy path to clipboard to navigate to folder easier for dev work
            GUIUtility.systemCopyBuffer = dir;

            return true;
        }
    }

}
