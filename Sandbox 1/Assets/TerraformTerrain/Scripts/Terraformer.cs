using System.Drawing;
using UnityEngine;

public class Terraformer : MonoBehaviour
{
    public event System.Action onTerrainModified;
    public LayerMask terrainMask;
    public float terraformRadius = 5;
    public float terraformSpeedNear = 0.1f;
    public float terraformSpeedFar = 0.25f;
    public PlayerControls playerControls;
    public Transform cam;
    GenTest genTest;
    bool hasHit;
    Vector3 hitPoint;
    bool isTerraforming;
    Vector3 lastTerraformPointLocal;

    void Start()
    {
        if (playerControls == null) playerControls = new PlayerControls();
        genTest = FindObjectOfType<GenTest>();
    }

    void Update()
    {
        RaycastHit hit;
        hasHit = false;

        bool wasTerraformingLastFrame = isTerraforming;
        isTerraforming = false;

        int numIterations = 5;
        bool rayHitTerrain = false;

        
        for (int i = 0; i < numIterations; i++)
        {
            float rayRadius = terraformRadius * Mathf.Lerp(0.01f, 1, i / (numIterations - 1f));

            //Draw the sphere that is cast to try and find a point on the terrain to terraform.
            DebugDrawSphereCast(cam.position, rayRadius, cam.forward, 1000);

            //Casts a sphere forward from the camera.
            if (Physics.SphereCast(cam.position, rayRadius, cam.forward, out hit, 1000, terrainMask))
            {
                lastTerraformPointLocal = MathUtility.WorldToLocalVector(cam.rotation, hit.point);
                Terraform(hit.point);
                rayHitTerrain = true;
                break;
            }
        }


        if (!rayHitTerrain && wasTerraformingLastFrame)
        {
            Vector3 terraformPoint = MathUtility.LocalToWorldVector(cam.rotation, lastTerraformPointLocal);
            Terraform(terraformPoint);
        }

    }

    // Helper method to draw the SphereCast
    void DebugDrawSphereCast(Vector3 origin, float radius, Vector3 direction, float distance)
    {
        Vector3 endPoint = origin + direction.normalized * distance;

        // Draw the line from camera to hit point
        Debug.DrawLine(origin, endPoint, UnityEngine.Color.red);

        // Draw the sphere at the end point
        Debug.DrawRay(endPoint, Vector3.up * radius, UnityEngine.Color.green);
        Debug.DrawRay(endPoint, Vector3.down * radius, UnityEngine.Color.green);
        Debug.DrawRay(endPoint, Vector3.right * radius, UnityEngine.Color.green);
        Debug.DrawRay(endPoint, Vector3.left * radius, UnityEngine.Color.green);
        Debug.DrawRay(endPoint, Vector3.forward * radius, UnityEngine.Color.green);
        Debug.DrawRay(endPoint, Vector3.back * radius, UnityEngine.Color.green);
    }

    void Terraform(Vector3 terraformPoint)
    {

        hasHit = true;
        hitPoint = terraformPoint;

        //Draw a line from the camera to the hit point
        Debug.DrawLine(cam.position, terraformPoint, UnityEngine.Color.green);

        const float dstNear = 10;
        const float dstFar = 60;

        float dstFromCam = (terraformPoint - cam.position).magnitude;
        float weight01 = Mathf.InverseLerp(dstNear, dstFar, dstFromCam);
        float weight = Mathf.Lerp(terraformSpeedNear, terraformSpeedFar, weight01);

        bool mb1 = playerControls.Player.MouseButton1.IsPressed();
        bool mb2 = playerControls.Player.MouseButton2.IsPressed();


        //Check if mouse button is being held down


        Debug.Log(mb1);



        // Add terrain
        if (mb1)
        {
            isTerraforming = true;
            genTest.Terraform(terraformPoint, -weight, terraformRadius);
        }
        // Subtract terrain
        else if (mb2)
        {
            isTerraforming = true;
            genTest.Terraform(terraformPoint, weight, terraformRadius);
        }

        if (isTerraforming)
        {
            onTerrainModified?.Invoke();
        }
    }

    void OnDrawGizmos()
    {
        if (hasHit)
        {
            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawSphere(hitPoint, 0.25f);
        }
    }

    private void OnEnable()
    {
        //OnEnable gets called before Awake so this is needed.
        if (playerControls == null) playerControls = new PlayerControls();
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
