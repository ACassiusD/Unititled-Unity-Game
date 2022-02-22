//Extendable class that contains core logic for playable characters such as collisions and movement.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Playable character may not be the best description for this class
//This class defines any "living" creature and the most basic of functions like movement, health system etc.
public class Entity : MonoBehaviour, InteractableInterface
{
    public CharacterController controller = new CharacterController();
    private static GameObject canvas;
    BetaCharacter playerScript;
    HealthBar healthBarScript;
    private RectTransform rectTransform;
    //public float speed = 30;
	//protected Vector3 moveDirection;
	//public float jumpForce = 50f;
	//public float gravityScale = .25f;
	//public int numOfJumps = 0;
	//public int maxJumps = 2;
    //public int rotationSpeed = 3;
    public bool isControllable = false;
    public bool isBeingControlled = false;
    //public bool isTurning = false;
    //public bool isMoving = false;
    //public float turnLength = 0.0f;
   // public float walkLength = 0.0f;
    //public int turnDirection = 0;
    public float aiRotationSpeed = 1.5f;
    public int aiMoveSpeed = 30;
    public bool canAttack = true;
    public int currentHealth = 50;
    public int maxHealth = 100;
    public bool inHitStun = false;
    protected static GameObject playerReference;
    public bool isWandering = false;


    protected virtual void Start () {
        //if (!playerReference)
        //{
        //    playerReference = GameObject.FindGameObjectWithTag("Player");
        //}
        //gameObject.layer = 8; //Set to interactive layer automatically
        //playerScript = PlayerManager.Instance.getPlayerScript();
        //healthBarScript = this.GetComponentInChildren<HealthBar>();
        //var test = this.gameObject.name;
        //onCreate();
        //controller = GetComponent<CharacterController>();
        //rectTransform = GetComponent<RectTransform>();
        //canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        //updateHealthBar();
    }

    //Update the float UI healthbar above the player in world space
    protected void updateHealthBar()
    {
       // healthBarScript.setHealth(currentHealth, maxHealth);
    }

    //Overloadable function that is called when a mount is initialized 
    public virtual void onCreate()
    {
        Debug.Log("base");
    }

    //Update is called once per frame
    protected virtual void Update()
    {
        //MoveCharacterController();
    }

	//Control player with character controller
	//protected void MoveCharacterController(){
 //       isMoving = checkIfMoving();

 //       if (isBeingControlled && !inHitStun)
 //       {
 //           getPlayerInput();
 //       }
 //       else //If in hitstun, keep moveDirection from Knockback function.
 //       {
 //           if (controller.isGrounded)
 //           {
 //               moveDirection.y = 0;
 //           }
 //           //Zero out all movement except verticle momentum
 //           moveDirection = new Vector3(0, moveDirection.y, 0);
 //       }

 //       //Calculate gravity if not grounded 
 //       if (!controller.isGrounded)
 //       {
 //           moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);
 //       }

 //       //Transform objects direction in global space
 //       moveDirection = transform.TransformDirection(moveDirection);

 //       //Apply vector to object
 //       controller.Move(moveDirection * Time.deltaTime);
 //   }


    //public bool checkIfMoving()
    //{
    //    var input = Input.GetAxis("Vertical");
    //    if ( input > 0 || input < 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //         return false;
    //    }
        
    //}
    //Apply the input to the players movement
    //void getPlayerInput()
    //{

    //        //Check if the player is grounded and reset the jumps
    //        if (controller.isGrounded)
    //        {
    //            numOfJumps = 0;
    //        }

    //        //Apply y velocity from last frame and apply the forward/backward movement
    //        moveDirection = new Vector3(0, moveDirection.y, Input.GetAxis("Vertical") * speed);

    //        //If jump is pressed, or in the air and we have a jump, add a force to the verticle axis
    //        if (controller.isGrounded || (!controller.isGrounded && numOfJumps < maxJumps))
    //        {
    //            if (Input.GetButtonDown("Jump"))
    //            {
    //                numOfJumps = numOfJumps + 1;
    //                moveDirection.y = jumpForce;
    //            }
    //            else
    //            {
    //                if (controller.isGrounded)
    //                {
    //                    moveDirection.y = 0;
    //                }
    //            }
    //        }

    //        //Apply player rotation 
    //        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

    //    //Check if attacking
    //    if (canAttack) {
    //        if (Input.GetMouseButtonDown(1)){
    //            this.attack();
    //        }
    //    }
    //}
    public virtual void attack()
    {
        Debug.Log(this.name + "Attacks");
    }

    public int receiveDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        updateHealthBar();
        if(currentHealth <= 0)
        {
            feint();
            return 0;
        }
        Knockback(300);
        return currentHealth;
    }

    public void Knockback(float force)
    {
        inHitStun = true;
        
        //Add Impact
        Vector3 direction = this.transform.forward * -1; //Need to make this direction
        Vector3 up = this.transform.up;
        up.Normalize();
        direction.Normalize();
        direction.y = up.y;
        var impact = Vector3.zero;
        impact += direction.normalized * force;

        //Apply vector to object
        controller.Move(impact * Time.deltaTime);
    }

    public void interact()
    {
        throw new System.NotImplementedException();
    }

    //Kill/death command, despawn enemy and drop any loot.
    public void feint()
    {
        Destroy(gameObject);
    }
}
