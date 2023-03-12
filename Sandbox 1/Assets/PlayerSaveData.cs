using SaveLoadSystem;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour
{
    private PlayerControls playerControls;
    public int currentHealth = 10;
    private PlayerData MyData = new PlayerData();

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    //Saving player data every frame currently
    void Update()
    {
        var myTransform = transform;
        MyData.PlayerPosition = myTransform.position;
        MyData.PlayerRotation = myTransform.rotation;
        MyData.CurrentHealth = currentHealth;

/*        if (playerControls.Player.KeyNum1.WasPerformedThisFrame())
        {
            //SaveGameManager.CurrentSaveData.PlayerData = MyData;
            //SaveGameManager.SaveGame();
        }


        if (playerControls.Player.KeyNum2.WasPerformedThisFrame())
        {
            //SaveGameManager.LoadGame();
            //MyData = SaveGameManager.CurrentSaveData.PlayerData;
            myTransform.position = MyData.PlayerPosition;
            myTransform.rotation = MyData.PlayerRotation;
            currentHealth = MyData.CurrentHealth;
        }*/
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}


[System.Serializable]
public struct PlayerData
{
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;
    public int CurrentHealth;
}