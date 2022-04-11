using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    // A static property called instance
    //Static methods are shared across a class
    //This will be the one and only instantiaed object of PlayerManager (hence, singleton)
    //Private set, no one can erase the instance
    //public get, so you can only get this instance
    public static PlayerManager Instance { get; private set; }
    GameObject player;
    public BetaCharacter playerScript;
    public InventoryManager playerInventory;

    // Use this for initialization
    void Awake () {
        Debug.Log("Initializing shared player script");
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<BetaCharacter>();
        playerInventory = player.GetComponent<InventoryManager>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public BetaCharacter getPlayerScript()
    {
        return playerScript;
    }
    public string getPlayerHeldItem()
    {
        return playerInventory.heldObject;
    }

}
