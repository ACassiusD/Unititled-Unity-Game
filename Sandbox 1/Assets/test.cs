using UnityEngine;

public class test : MonoBehaviour
{
    PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Update()
    {
        if (playerControls.Player.MouseButton2.WasPerformedThisFrame())
        {
            Debug.Log("Not working here?");
        }
    }
}
