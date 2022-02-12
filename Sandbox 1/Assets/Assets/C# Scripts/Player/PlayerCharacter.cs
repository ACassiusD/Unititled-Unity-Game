using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity {

    public bool isRiding = false;
    private GameObject[] tamedMounts;
    public Mount activeMount = null;
    Mount activeMountScript;
    public Inventory inventory;

    private void Awake() { 
        Application.targetFrameRate = 60;
    }

    public override void onCreate()
    {
        isControllable = true;
        isBeingControlled = true;
        activeMount = null;
        tamedMounts = GameObject.FindGameObjectsWithTag("TamedMount"); //Populate tamed mounts
    }

 
    void Update()
    {
        getNewPosition();
    }

    //Check if riding state changes before calling Basic movement
    void getNewPosition()
    {
        if (!isRiding)
        {
            base.MoveCharacterController();
        }
        else
        {
            moveToMountedPosition();
        }
    }

    public void setActiveMount(Mount mount)    
    {
        this.activeMount = mount;
        flipRidingState();
    }
    //Moves player to mounted position
    public void moveToMountedPosition()
    {
        //Calculate where the rider needs to be positioned, then transform him to that position and rotation
        Vector3 ridingPositon = activeMount.transform.position;
        ridingPositon.y = ridingPositon.y + this.activeMount.ridingHeight;
        transform.position = ridingPositon;

        //Rotation
        transform.rotation = activeMount.transform.rotation;
    }


    //Flips weather the player is in a riding state or not
    void flipRidingState()
    {
        if (isRiding == true) //Player is already mounted
        {
            isBeingControlled = true; //Bring back control to player
            isRiding = false;
            Physics.IgnoreCollision(controller, activeMount.GetComponent<CharacterController>(), false); //Turn on collisions
        }
        else //Player is not yet mounted
        {
            activeMountScript = activeMount.GetComponent<Mount>();

            //Only flip state if there is a mount to be mounted
            if (activeMount)
            {
                isBeingControlled = false;
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
