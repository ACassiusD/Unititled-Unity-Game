using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //Static meaning shared across all instances of this class
    //This will be the one and only instantiaed object of PlayerManager (hence, singleton)
    //Private set, no one can erase the instance
    //public get, so you can only get this instance
    public static PlayerManager Instance { get; private set; }
    GameObject player;
    public BetaCharacter playerScript;
    public InventoryManager playerInventory;

    // Use this for initialization
    void Awake () {
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled or disable in quality manually 
        Application.targetFrameRate = 144;
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        loadPlayerScript();
        loadInventoryManager();
    }

    private void loadInventoryManager()
    {
        if (player.GetComponent<InventoryManager>() != null)
        {
            playerInventory = player.GetComponent<InventoryManager>();

        }else{
            //Debug.LogError("Player Manager - Unable return load inventory script.");
            //Debug.Break();
        }
    }

    private void loadPlayerScript()
    {
        player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            Debug.LogError("Player Manager - Find object with 'Player' Tag.");
            Debug.Break();
        }
        playerScript = player.GetComponent<BetaCharacter>();
        if(playerScript == null)
        {
            Debug.LogError("Player Manager - Unable return load player script.");
            Debug.Break();
        }
    }
    public BetaCharacter getPlayerScript()
    {
        if(playerScript == null)
        {
            Debug.LogError("Unable retrieve player script.");
            Debug.Break();
        }
        return playerScript;
    }

    public string getPlayerHeldItem()
    {
        return playerInventory.heldObject;
    }
}
