// The MeleeAttack class represents a short-duration, close-range attack.
// The attack's hitbox is enabled temporarily, and the activation and deactivation are managed by the owning Weapon.
using UnityEngine;
using UnityEngine.VFX;

public class MeleeAttack : Attack
{
    public VisualEffect meleeAnimation;

    protected override void HandleHit(Collider other)
    {
        base.HandleHit(other);
        SpecificAttackBehaviour();
    }

    public override void SpecificAttackBehaviour()
    {
        meleeAnimation.Play();
    }
}
