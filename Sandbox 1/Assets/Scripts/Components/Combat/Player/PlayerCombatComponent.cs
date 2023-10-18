using UnityEngine;

public class PlayerCombatComponent : CombatComponent
{
    public float ReceiveDamage(float damageAmount, int knockBackForce, Vector3 direction = new Vector3())
    {
        TakeDamage(damageAmount);
        
        //STUN CODE
        //playerMovementComponent.stunTimer = playerMovementComponent.stunDuration;

        return statsComponent.currentHealth;
    }

    /// <summary>
    /// What happens when this entity dies.
    /// </summary>
    public override void Feint()
    {
        Debug.Log("Player Character Died");
        if (statsComponent.IsDead())
        {
            //Destroy(gameObject);
        }
    }

}
