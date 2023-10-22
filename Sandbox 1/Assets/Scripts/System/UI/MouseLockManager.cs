using UnityEngine;

public class MouseLockManager : MonoBehaviour
{
    public bool lockCursorToScreen = false;
    private PlayerControls playerControls;

    void Start()
    {
        if (lockCursorToScreen) Cursor.visible = false;
        else Cursor.visible = true;
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
        if (playerControls.Player.tilde.WasPressedThisFrame())
        {
            lockCursorToScreen = lockCursorToScreen == true ? false : true;
        }
        Cursor.visible = !lockCursorToScreen;
        Cursor.lockState = lockCursorToScreen ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void OnApplicationFocus(bool ApplicationIsBack)
    {
        if (ApplicationIsBack == true)
        {
            if (lockCursorToScreen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
