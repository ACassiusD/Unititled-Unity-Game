using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    public UnityEvent<Collider> OnHitEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnHitEvent.Invoke(other);
    }
}