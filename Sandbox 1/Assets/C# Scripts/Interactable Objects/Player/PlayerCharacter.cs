using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : PlayableCharacters {

    public bool isRiding = false;
    public float ridingHeight = 4f;
    private GameObject[] tamedMounts;
    private Mount activeMount = null;
    Mount activeMountScript;


    private void Awake() { 
        Application.targetFrameRate = 60;
    }

    public override void onCreate()
    {
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
            base.moveCharacterController();
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
        ridingPositon.y = ridingPositon.y + ridingHeight;
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
}
