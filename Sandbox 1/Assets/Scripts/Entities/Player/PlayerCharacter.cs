using UnityEngine;

public class PlayerCharacter : Entity {

    public bool isRiding = false;
    private GameObject[] tamedMounts;
    public Mount activeMount = null;
    Mount activeMountScript;
    public Inventory inventory;
    CharacterController characterController;

    private void Awake() { 
        Application.targetFrameRate = 60;
        characterController = GetComponent<CharacterController>();
    }

    public override void onCreate()
    {
        activeMount = null;
        tamedMounts = GameObject.FindGameObjectsWithTag("TamedMount"); //Populate tamed mounts
    }

    //Flips weather the player is in a riding state or not
    void flipRidingState()
    {
        if (isRiding == true) //Player is already mounted
        {
            isRiding = false;
            Physics.IgnoreCollision(controller, activeMount.GetComponent<CharacterController>(), false); //Turn on collisions
        }
        else //Player is not yet mounted
        {
            activeMountScript = activeMount.GetComponent<Mount>();

            //Only flip state if there is a mount to be mounted
            if (activeMount)
            {
                isRiding = true;
                Physics.IgnoreCollision(controller, activeMount.GetComponent<CharacterController>(), true);
            }
        }
    }

    public void unMount()
    {
        flipRidingState();
        activeMount = null;

    }

    //Text function for testing item pick up
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            //inventory.AddItem(other.GetComponent<Item>());
        }
    }
}
