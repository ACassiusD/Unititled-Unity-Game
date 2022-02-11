//Extendable class that contains core logic for playable characters such as collisions and movement.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Playable character may not be the best description for this class
//This class defines any "living" creature and the most basic of functions like movement, health system etc.
public class PlayableCharacters : MonoBehaviour {
    public CharacterController controller = new CharacterController();
    private static GameObject canvas;
    private RectTransform rectTransform;
    public float speed = 30;
	private Vector3 moveDirection;
	public float jumpForce = 50f;
	public float gravityScale = .25f;
	public int numOfJumps = 0;
	public int maxJumps = 2;
    public int rotationSpeed = 3;
    public bool isBeingControlled = false;
    public bool isTurning = false;
    public bool isMoving = false;
    public float turnLength = 0.0f;
    public float walkLength = 0.0f;
    public int turnDirection = 0;
    float halfSecondLength = 0.5f;
    public float aiRotationSpeed = 1.5f;
    public int aiMoveSpeed = 30;
    public bool canAttack = true;
    public int health = 50;

    protected virtual void Start () {
        var test = this.gameObject.name;
        onCreate();
        controller = GetComponent<CharacterController>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        createHealthBar();
    }

    protected void createHealthBar()
    {
       // var spiderHP = GameObject.Find("SpiderHP");
       // RectTransform rectTransform = spiderHP.GetComponent<RectTransform>();
       // float offsetPosY = this.transform.position.y + 1.5f;

        //// Final position of marker above GO in world space
      //  Vector3 offsetPos = new Vector3(this.transform.position.x, offsetPosY, this.transform.position.z);

       // rectTransform.position = Camera.main.WorldToScreenPoint(this.transform.position + offsetPos);
       // var test = canvas.transform.GetChild(0);
        //var tst = 1;

        ////Create new gameobject to attach to canvas gameobject
        //GameObject newGO = new GameObject(this.name + "-HealthBar");

        ////Create text to attach to new game object
        //Text text = newGO.AddComponent<Text>();

        ////Attach gameobject to canvas
        //newGO.transform.SetParent(canvas.transform);

        //text.text = this.name + "-HealthBar";
        //Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        //text.font = ArialFont;
        //text.material = ArialFont.material;

        ////calculate position 
        //// Offset position above object bbox (in world space)
        //float offsetPosY = this.transform.position.y + 1.5f;

        //// Final position of marker above GO in world space
        //Vector3 offsetPos = new Vector3(this.transform.position.x, offsetPosY, this.transform.position.z);

        //// Calculate *screen* position (note, not a canvas/recttransform position)
        //Vector2 canvasPos;
        //Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        //RectTransform canvasRect = newGO.GetComponent<RectTransform>();

        //// Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

        // Set
        //markerRtra.localPosition = canvasPos;
    }

    //Overloadable function that is called when a mount is initialized 
    public virtual void onCreate()
    {
        Debug.Log("base");
    }

    //Update is called once per frame
    protected virtual void Update()
    {
        MoveCharacterController();
    }

	//Control player with character controller
	protected void MoveCharacterController(){
        isMoving = checkIfMoving();

        if (isBeingControlled)
        {
            getPlayerInput();
        }
        else
        {
            //Zero out all movement except verticle momentum
            moveDirection = new Vector3(0, moveDirection.y, 0);
        }

        //Calculate gravity
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);

        //Transform objects direction in global space
        moveDirection = transform.TransformDirection(moveDirection);

        //Apply vector to object
        controller.Move(moveDirection * Time.deltaTime);
    }

    public bool checkIfMoving()
    {
        var input = Input.GetAxis("Vertical");
        if ( input > 0 || input < 0)
        {
            return true;
        }
        else
        {
             return false;
        }
        
    }
    //Apply the input to the players movement
    void getPlayerInput()
    {

            //Check if the player is grounded and reset the jumps
            if (controller.isGrounded)
            {
                numOfJumps = 0;
            }

            //Apply y velocity from last frame and apply the forward/backward movement
            moveDirection = new Vector3(0, moveDirection.y, Input.GetAxis("Vertical") * speed);

            //If jump is pressed, add a force to the verticle axis
            if (controller.isGrounded || (!controller.isGrounded && numOfJumps < maxJumps))
            {
                if (Input.GetButtonDown("Jump"))
                {
                    numOfJumps = numOfJumps + 1;
                    moveDirection.y = jumpForce;
                }
                else
                {
                    if (controller.isGrounded)
                    {
                        moveDirection.y = 0;
                    }
                }
            }

            //Apply player rotation 
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        //Check if attacking
        if (canAttack) {
            if (Input.GetMouseButtonDown(1)){
                this.attack();
            }
        }
    }
    public virtual void attack()
    {
        Debug.Log(this.name + "Attacks");
    }
}
