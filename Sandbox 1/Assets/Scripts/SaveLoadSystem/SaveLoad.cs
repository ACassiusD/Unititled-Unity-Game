using SaveLoadSystem;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

//SaveLoadManager. Rename. Manages saving and loading of data. Contains unity actions that can be subscribed to that fire during saving and loading.
public class SaveLoad
{

    public static UnityAction OnSaveGame;
    public static UnityAction<SaveData> OnLoadGame;

    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.sav";

    public static bool SaveGame(SaveData data)
    {
        OnSaveGame?.Invoke(); //Call unity action to know when we are trying to save.

        //Application.persistentDataPath is a unity directory path that works cross platform for storage.
        string dir = Application.persistentDataPath + SaveDirectory;

        GUIUtility.systemCopyBuffer = dir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + FileName, json);

        //Copy path to clipboard to navigate to folder easier for dev work
        GUIUtility.systemCopyBuffer = dir;

        Debug.Log("Saving Game...");

        return true;
    }

    public static SaveData LoadGame()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;
        SaveData data = new SaveData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);

            //Call load game action if anyone is subscribed to it.
            //For example chests will hook onto this load action, grab the chest data contained in data variable, and load themselves.
            OnLoadGame?.Invoke(data);
        }
        else
        {
            Debug.LogError("Save file does not exist!");
        }

        return data;
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}
