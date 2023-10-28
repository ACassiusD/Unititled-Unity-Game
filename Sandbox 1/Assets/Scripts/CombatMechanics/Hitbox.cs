/// <summary>
/// The Hitbox class is responsible for detecting collisions for various attack types.
/// This script should be attached to GameObjects intended to act as hitboxes.
/// </summary>
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent<Collider> OnHitEvent;
    private Collider hitboxCollider;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        // Cache the Collider component for quick access
        hitboxCollider = GetComponent<Collider>();

        // Cache the MeshRenderer component for debugging
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            // Optionally, log a message if the MeshRenderer component is not found
            Debug.LogWarning("MeshRenderer not found on " + gameObject.name);
        }
    }

    public void EnableHitbox()
    {
        // Enable the hitbox collider
        hitboxCollider.enabled = true;

        // Debugging: Enable MeshRenderer to visualize the hitbox
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }
    }

    public void DisableHitbox()
    {
        // Disable the hitbox collider
        hitboxCollider.enabled = false;

        // Debugging: Disable MeshRenderer to hide the hitbox
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHitEvent.Invoke(other);
    }
}
