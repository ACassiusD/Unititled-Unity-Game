using UnityEngine;

//Class should only deal with casting rays and interfacing with interactable objects.
public class Interactor : MonoBehaviour
{
    private RaycastHit hitinfo;
    public float distance = 10;
    public float yOffset = -.40f;
    public bool isDebugging = false;
    bool isObjectHit = false;
    public bool isInteracting { get; private set; }
    bool capturedKeyPress = false;

    Transform hitObject;
    [SerializeField] Material highlightMaterial;
    Outline targetOutlineScript;
    PlayerEntity playerScript;
    Color outLineColor;
    PlayerControls playerControls;


    public void Start()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
    }
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Update()
    {
        capturedKeyPress = false;
        capturedKeyPress = playerControls.Player.MouseButton2.WasPerformedThisFrame();
        clearActiveScript();
        isObjectHit = castRay();
        if (isObjectHit)
            HandleHighlightAndInteraction(hitinfo);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    //Runs at the beginning of every new frame, removes highlighting and clears the script from memory
    private void clearActiveScript()
    {
        if (targetOutlineScript != null)
        {
            var updateOutineColor = targetOutlineScript.OutlineColor;
            updateOutineColor.a = 0;
            targetOutlineScript.outlineFillMaterial.SetColor("_OutlineColor", updateOutineColor);
            targetOutlineScript = null;
        }
    }

    //Cast ray and return weather a object was hit
    private bool castRay()
    {
        bool isObjectHit = false;
        hitinfo = new RaycastHit();
        var cam = playerScript.playerMovementComponent.cam;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, distance, 1 << 8))
            isObjectHit = true;
        else
            isObjectHit = false;

        if (isDebugging)
            debugRay();

        return isObjectHit;
    }

    //If an object intercepted with the ray, attempt to find the IInteractable within the object hierarchy and then call interact.
    private void HandleHighlightAndInteraction(RaycastHit hitinfo)
    {
        hitObject = hitinfo.transform;

        //Check for Interactor object.
        if (hitObject.GetComponent<Interactor>())
            return;

        //If object has no active script, try to find it in the class hierarchy
        if (hitObject.GetComponent<Outline>())
        {
            targetOutlineScript = hitObject.GetComponent<Outline>();
            outlineObject();

            IInteractable interactableObject = hitObject.GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                StartInteraction(interactableObject);
                return;
            }
        }
        //Try getting from parent
        else if (hitObject.parent && hitObject.parent.GetComponentInChildren<Outline>())
        {
            targetOutlineScript = hitObject.parent.GetComponentInChildren<Outline>();
            outlineObject();

            hitObject = hitObject.parent;
            IInteractable interactableObject = hitObject.GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                StartInteraction(interactableObject);
                return;
            }
        }
        //Try getting from children
        else if (hitObject.GetComponentInChildren<Outline>() != null)
        {
            targetOutlineScript = hitObject.parent.GetComponentInChildren<Outline>();
            outlineObject();

            foreach (Transform child in transform)
            {
                IInteractable interactableObject = child.GetComponentInChildren<IInteractable>();

                if (interactableObject != null)
                {
                    StartInteraction(interactableObject);
                }
            }
        }
    }

    private void outlineObject()
    {
        if (targetOutlineScript != null)
        {
            outLineColor = targetOutlineScript.OutlineColor;
            outLineColor.a = 255.0f;
            targetOutlineScript.outlineFillMaterial.SetColor("_OutlineColor", outLineColor);
        }
    }

    //Expanded out into its own method so we can assign the isInteracting variable;
    void StartInteraction(IInteractable interactable)
    {
        if (capturedKeyPress)
        {
            interactable.Interact(this, out bool interactSuccessful);
            isInteracting = true;
        }
    }

    void EndInteraction()
    {
        isInteracting = false;
    }

    private void debugRay()
    {
        var cam = playerScript.playerMovementComponent.cam;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * distance, Color.red);
    }

}

