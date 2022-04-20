using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int power = 30;
    public Transform arrowPos;
    BetaCharacter betaCharacter;
    Transform cam;
    public float forwardVel = 50f;
    public float upwardVel = 50f;
    public float mass = 5f;
    public bool useGravity = true;
    public int rayRange = 2000;

    void Start()
    {
        betaCharacter = gameObject.GetComponent<BetaCharacter>();
        cam = PlayerManager.Instance.getPlayerScript().movementComponent.cam;
    }

    void Update()
    {
        bool capturedKeyPress = Input.GetMouseButtonDown(0);
        if (capturedKeyPress)
        {
            //Rotate the character to face firing direction
            transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

            //Instantiate and postion new arrow
            GameObject newArrow = Instantiate(arrowPrefab);
            newArrow.transform.position = arrowPos.transform.position;
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
                Debug.Log(hitinfo.collider.name);
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
