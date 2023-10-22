using UnityEngine;
using UnityEngine.VFX;

public class MeleeAnimation : MonoBehaviour
{
    public VisualEffect anim;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.Player.MeleeKey.WasPerformedThisFrame();)
        {
            anim.Play();
        }
    }
}
