using UnityEngine;

/// <summary>
/// The CombatComponent class represents the combat-related functionality of an entity within an action RPG game.
/// This component interfaces with a StatsComponent to track and modify the health of the entity,
/// and defines behavior for receiving damage, healing, and handling death.
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

    //Basic implementation of Recieve Damage
    public float ReceiveDamage(float damageAmount, int? knockBackForce = null, Vector3? direction = null)
    {
        statsComponent.ModifyHealth(-damageAmount);

        // Raise the event after taking damage with the current health percentage
        OnHealthChanged(statsComponent.GetCurrentHealthPercentage());

        if (statsComponent.IsDead())
        {
            Feint();
        }

        return statsComponent.GetCurrentHealth();
    }

    public void Heal(float healAmount)
    {
        statsComponent.ModifyHealth(healAmount);
        // Raise the event after healing with the current health percentage
        //OnHealthChanged(statsComponent.GetCurrentHealthPercentage());
    }


    /// <summary>
    /// What happens when this entity dies.
    /// Should be overrided in derrived combat classes.
    /// </summary>
    public virtual void Feint()
    {
        Debug.Log("MUST IMPLEMNT FEINT METHOD");
    }
}
