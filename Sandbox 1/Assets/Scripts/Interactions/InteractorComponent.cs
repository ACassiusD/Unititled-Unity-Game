using UnityEngine;
using System;


//Class should only deal with casting rays and interfacing with interactable objects.
public class InteractorComponent : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 direction;
    private RaycastHit hitinfo;
    public float distance = 10;
    public float yOffset = -.40f;
    public bool isDebugging = false;
    Transform hitObject;
    [SerializeField] Material highlightMaterial;
    bool isObjectHit = false;
    int LayerMask; //Layer to ignore
    Outline activeScript;
    float outlineWidth = 0.5f;
    BetaCharacter playerScript;
    Outline outlineScript;
    Color outLineColor;
    PlayerControls playerControls;

    public void Start()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
    }

    private void Update()
    {
        bool capturedKeyPress = playerControls.Player.MouseButton2.WasPerformedThisFrame();
        clearActiveScript();
        isObjectHit = castRay();
        if (isObjectHit)
        {

            CallInteraction(hitinfo, capturedKeyPress);
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    //Runs at the beginning of every new frame, removes highlighting and clears the script from memory
    private void clearActiveScript()
    {
        if (activeScript != null)
        {
            var updateOutineColor = activeScript.OutlineColor;
            updateOutineColor.a = 0;
            activeScript.outlineFillMaterial.SetColor("_OutlineColor", updateOutineColor);
            //active.eraseRenderer = true;
            activeScript = null;
        }
    }

    //Cast ray and return weather a object was hit
    private bool castRay()
    {
        bool isObjectHit = false;
        hitinfo = new RaycastHit();
        var cam = playerScript.movementComponent.cam;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, distance, 1 << 8))
        {
            isObjectHit = true;
        }
        else
        {
            isObjectHit = false;
        }

        //Debugging
        if (isDebugging)
        {
            debugRay();
        }

        return isObjectHit;
    }

    //If an object intercected with the ray, attempt to call interaction with the hit object
    private void CallInteraction(RaycastHit hitinfo, bool capturedKeyPress)
    {
        hitObject = hitinfo.transform;
        bool hasInteractFunction = false;
        //Debug.Log(hitObject);

        //Check for interactive object.
        if (hitObject.GetComponent<InteractorComponent>())
        {
            return;
        }

        //If object has no active script, try to find it in the class hierarchy
        if (hitObject.GetComponent<Outline>())
        {
            activeScript = hitObject.GetComponent<Outline>();

            InteractableInterface InteractableInterface = hitObject.GetComponent<InteractableInterface>();
            if (InteractableInterface != null)
            {
                hasInteractFunction = true;
            }
        }
        else if (hitObject.parent && hitObject.parent.GetComponentInChildren<Outline>()) //Try getting from parent
        {
            activeScript = hitObject.parent.GetComponentInChildren<Outline>();
            hitObject = hitObject.parent;

            InteractableInterface InteractableInterface = hitObject.GetComponent<InteractableInterface>();
            if (InteractableInterface != null)
            {
                hasInteractFunction = true;
            }
        }
        else if (hitObject.GetComponentInChildren<Outline>() != null) //Try getting from children
        {
            activeScript = hitObject.GetComponentInChildren<Outline>();

            foreach (Transform child in transform)
            {
                InteractableInterface InteractableInterface = child.GetComponentInChildren<InteractableInterface>();

                if (InteractableInterface != null)
                {
                    hasInteractFunction = true;
                }
            }

        }

        //Try to call interact
        if (activeScript != null)
        {
            outLineColor = activeScript.OutlineColor;
            outLineColor.a = 255.0f;
            activeScript.outlineFillMaterial.SetColor("_OutlineColor", outLineColor);
            if (capturedKeyPress)
            {

                if (hasInteractFunction)
                {
                    hitObject.SendMessage("interact");
                }
                else
                {
                    Debug.LogWarning("Object has no interact function, one should be added.");
                }
            }
        }
    }

    private void debugRay()
    {
        var cam = playerScript.movementComponent.cam;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * distance, Color.red);
    }

}

