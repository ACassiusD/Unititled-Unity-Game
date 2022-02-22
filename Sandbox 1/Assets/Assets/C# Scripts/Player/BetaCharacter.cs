using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaCharacter : MonoBehaviour
{
    public CharacterController controller;
   // public bool isRiding = false;
    private GameObject[] tamedMounts;
    public Mount activeMount = null;
    Mount activeMountScript;
    //public Inventory inventory;
    PlayerMovementComponent movementComponent;

    private void Awake()
    {
        movementComponent = this.GetComponent<PlayerMovementComponent>();
        Application.targetFrameRate = 60;
        Cursor.visible = false;
    }

    private void Start()
    {
    }

    void Update()
    {
        //getNewPosition();
    }

    public void onCreate()
    {

        activeMount = null;
        tamedMounts = GameObject.FindGameObjectsWithTag("TamedMount"); //Populate tamed mounts
    }


    public void unMount(float dismountDistance)
    {
        this.activeMount = null;
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

}
