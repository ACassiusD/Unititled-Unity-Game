using System;
using UnityEngine;

/// <summary>
/// The CombatComponent class represents the combat-related functionality of an entity within an action RPG game.
/// This component interfaces with a StatsComponent to track and modify the health of the entity,
/// and defines behavior for receiving damage, healing, and handling death.
/// 
/// Declares an event OnHealthChanged, things like floating healthbar UIs, etc will subscribe to this event.
/// </summary>
public class CombatComponent : MonoBehaviour, IDamageable
{
    protected StatsComponent statsComponent; //Required

    // Event that's triggers when health changes.
    public event System.Action<float> OnHealthChanged = delegate { };

    // Call this method to initialize the PlayerCombatComponent with a StatsComponent
    public void Initialize(StatsComponent statsComp)
    {
        if (statsComp == null)
        {
            throw new System.ArgumentNullException("statsComp", "The provided StatsComponent cannot be null.");
        }

        statsComponent = statsComp;
    }

    // Basic implementation of Recieve Damage
    // Raises healthChanged event after taking damage
    public virtual float ReceiveDamage(float damageAmount, int? knockBackForce = null, Vector3? direction = null)
    {
        statsComponent.ModifyHealth(-damageAmount);

        
        OnHealthChanged(statsComponent.GetCurrentHealthPercentage());

        if (statsComponent.IsDead())
        {
            Feint();
        }

        return statsComponent.GetCurrentHealth();
    }

    //Heal entity and raise an event that health has been updated.
    public void Heal(float healAmount)
    {
        statsComponent.ModifyHealth(healAmount);
        OnHealthChanged(statsComponent.GetCurrentHealthPercentage());
    }


    /// <summary>
    /// What happens when this entity dies.
    /// Should be overrided in derrived combat classes.
    /// </summary>
    public virtual void Feint()
    {
        Debug.Log("MUST IMPLEMENT FEINT METHOD");
    }

    public float GetCurrentHealthPercentage()
    {
        return statsComponent.GetCurrentHealthPercentage();
    }
}
