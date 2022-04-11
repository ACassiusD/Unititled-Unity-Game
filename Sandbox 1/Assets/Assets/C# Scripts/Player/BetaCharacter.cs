using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaCharacter : MonoBehaviour
{
    public CharacterController controller;
    private GameObject[] tamedMounts; //Create MountCollection() Class
    Mount currentMount;
    public PlayerMovementComponent movementComponent;

    private void Awake()
    {
        movementComponent = this.GetComponent<PlayerMovementComponent>();
        Application.targetFrameRate = 60;
        Cursor.visible = false;
    }

    public void onCreate()
    {
        tamedMounts = GameObject.FindGameObjectsWithTag("TamedMount"); //Populate tamed mounts
    }


    public void DisMount(float dismountDistance = 10f)
    {
        movementComponent.activeMount = null;
        this.transform.Translate(dismountDistance, 0, 0);
    }


    //Text function for testing item pick up
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            //inventory.AddItem(other.GetComponent<Item>());
        }
    }
    
    public bool getIsRiding()
    {
        return movementComponent.isRiding;
    }

    public void setIsRiding(bool passedVal)
    {
        movementComponent.isRiding = passedVal;
    }
    public void setActiveMount(Mount mount)
    {
        movementComponent.activeMount = mount;
        movementComponent.isRiding = true;
    }

    public void moveToMountedPosition()
    {
        var activeMount = movementComponent.activeMount;
        //Calculate where the rider needs to be positioned, then transform him to that position and rotation
        Vector3 ridingPositon = activeMount.transform.position;
        ridingPositon.y = ridingPositon.y + activeMount.ridingHeight;
        transform.position = ridingPositon;

        //Rotation
        transform.rotation = activeMount.transform.rotation;
    }
}
