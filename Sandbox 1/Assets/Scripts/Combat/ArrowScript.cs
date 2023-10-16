using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int power = 30;
    public Transform projectileOrigin;
    PlayerEntity betaCharacter;
    Transform cam;
    public float forwardVel = 50f;
    public float upwardVel = 50f;
    public float mass = 5f;
    public bool useGravity = true;
    public int rayRange = 2000;
    private PlayerControls playerControls;

    void Start()
    {
        betaCharacter = gameObject.GetComponent<PlayerEntity>();
        cam = PlayerManager.Instance.getPlayerScript().playerMovementComponent.cam.transform;
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Update()
    {
        bool capturedKeyPress = playerControls.Player.MouseButton1.WasPerformedThisFrame();
        if (capturedKeyPress)
        {
            //Rotate the character to face firing direction
            transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

            //Instantiate and postion new arrow
            GameObject newArrow = Instantiate(arrowPrefab);
            newArrow.transform.position = projectileOrigin.transform.position;
            Rigidbody rb = newArrow.GetComponent<Rigidbody>();

            //Trace ray for aim target position and rotate accordingly
            bool isObjectHit = false;
            RaycastHit hitinfo = new RaycastHit();

            //Layermask to ignore player and friendly units 
            int layerMask1 = 1 << 12;
            int layerMask2 = 1 << 11;
            int layerMask3 = 1 << 10;
            var finalmask = layerMask1 | layerMask2;
            finalmask = finalmask | layerMask3;
            finalmask = ~finalmask;

            if (Physics.Raycast(cam.position, cam.transform.forward, out hitinfo, rayRange, finalmask))
            {
                newArrow.transform.LookAt(hitinfo.point);
            }
            else
            {
                newArrow.transform.rotation = Quaternion.Euler(cam.localEulerAngles.x, betaCharacter.transform.eulerAngles.y, 0);
            }

            //Add Force
            rb.AddForce(newArrow.transform.forward * forwardVel);
            rb.AddForce(newArrow.transform.up * upwardVel);
            rb.useGravity = useGravity;
            rb.mass = mass;
        }
    }
}
