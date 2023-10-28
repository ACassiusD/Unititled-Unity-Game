// The MeleeAttack class represents a short-duration, close-range attack.
// The attack's hitbox is enabled temporarily, and the activation and deactivation are managed by the owning Weapon.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MeleeAttack : Attack
{
    private Dictionary<GameObject, float> hitRegistry;
    private float attackDuration = 0.5f; // or however long the attack lasts
    public VisualEffect meleeAnimation;

    private void OnEnable()
    {
        hitRegistry = new Dictionary<GameObject, float>();
    }

    protected override void HandleHit(Collider other)
    {
        CombatComponent enemyCombatComponent = other.GetComponentInParent<CombatComponent>();
        if (enemyCombatComponent == null) return;  // Hit something that isn't an enemy

        GameObject enemyRoot = enemyCombatComponent.gameObject;

        if (!hitRegistry.ContainsKey(enemyRoot))
        {
            // This is a new enemy that we've not hit before during this attack
            base.HandleHit(other);
            SpecificAttackBehaviour();
            hitRegistry.Add(enemyRoot, Time.time);
        }
    }

    public override void SpecificAttackBehaviour()
    {
        meleeAnimation.Play();
    }
}
