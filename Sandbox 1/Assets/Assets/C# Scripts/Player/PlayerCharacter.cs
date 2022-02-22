using System.Collections;
using System.Collections.Generic;
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
        isControllable = true;
        isBeingControlled = true;
        activeMount = null;
        tamedMounts = GameObject.FindGameObjectsWithTag("TamedMount"); //Populate tamed mounts
    }

    public void PlayerMovement()
    {
        ////Returns 0, -1 or 1 for corrosponding direction
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        ////Calcuate the Vector3 direction, and normalize it to a lenght of 1 unit (just get the direction we want to wak in p much)
        //Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        ////If we picked up a movement input
        //if (direction.magnitude >= 0.1f)
        //{
        //    //Mathf.Atan2(direction.x, direction.z) - Gives us the angle in radians our player needs to turn
        //    //Mathf.Rad2Deg Update the angle to degrees
        //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //    //Smooth the angle to not immediatly point towards it.
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);
        //    //Rotate our player on the Y axis by the angle 
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //    //reference for more information - https://www.youtube.com/watch?v=4HpC--2iowE
        //    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //    characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        //}
    }


    void Update()
    {
        //getNewPosition();
    }

    //Check if riding state changes before calling Basic movement
    //void getNewPosition()
    //{
    //    if (!isRiding)
    //    {
    //        base.MoveCharacterController();
    //    }
    //    else
    //    {
    //        moveToMountedPosition();
    //    }
    //}


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
