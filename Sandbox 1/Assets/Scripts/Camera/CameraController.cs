using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float currentDistance = 0;
    [SerializeField]
    private float desiredCameraDistance = 0;
    [SerializeField]
    private float currentHeight = 0;
    [SerializeField]
    bool isBeingControlled;
    public float cameraZoomSpeed = 5;
    public float maxZDistanceFromTarget = 100;
    public float minZDistanceFromTarget;
    public float maxCameraHeight = 15;
    public float minCameraHeight = 0;
    public Transform target; //What the camera looks at
    public CinemachineFreeLook cinemachineFreeLook;
    protected BetaCharacter characterScript;
    bool playerRunningStatus = false;
    private float lerp = 0f;
    private float lerpDuration = .5f;
    private int sprintingFOV = 100;
    private int normalFov = 85;
    private bool startSprintFOVTransition = false;
    private bool startNormalFOVTransition = false;


    // Use this for initialization
    void Start () {
        characterScript = PlayerManager.Instance.getPlayerScript();
        if (characterScript.movementComponent.isRunning)
        {
            playerRunningStatus = true;
            this.setCameraSprintingFOV();
        }
        else
        {
            playerRunningStatus = false;
            this.setCameraNormalFOV();
        }
        //Set negative to be behind the player
        isBeingControlled = false;
    }
	
	// Update is called once per frame
	void Update () {
        SetFOV();
        UpdateFOV();
        isBeingControlled = false;
    }

    void UpdateFOV()
    {
        if (startNormalFOVTransition)
        {
            setCameraNormalFOV();
        }
        else if(startSprintFOVTransition)
        {
            setCameraSprintingFOV();
        }
    }


    void SetFOV()
    {
        if (characterScript.movementComponent.isRunning != playerRunningStatus)
        {
            if (characterScript.movementComponent.isRunning)
            {
                lerp = 0;
                playerRunningStatus = true;
                startSprintFOVTransition = true;
                startNormalFOVTransition = false;
            }
            else
            {
                lerp = 0;
                playerRunningStatus = false;
                startSprintFOVTransition = false;
                startNormalFOVTransition = true;
            }
        }
    }


    public void setCameraSprintingFOV()
    {
        cinemachineFreeLook.m_CommonLens = true;
        lerp += Time.deltaTime / lerpDuration;
        //Debug.Log(lerp);
        cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(cinemachineFreeLook.m_Lens.FieldOfView, sprintingFOV, lerp);
        if(lerp > 1)
        {
            startSprintFOVTransition = false;
        }

    }

    public void setCameraNormalFOV()
    {
        cinemachineFreeLook.m_CommonLens = true;
        lerp += Time.deltaTime / lerpDuration;
        cinemachineFreeLook.m_Lens.FieldOfView = Mathf.Lerp(cinemachineFreeLook.m_Lens.FieldOfView, normalFov, lerp);
        if (lerp > 1)
        {
            startNormalFOVTransition = false;
        }
    }

}
