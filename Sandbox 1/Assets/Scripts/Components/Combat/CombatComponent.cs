using UnityEngine;

/// <summary>
/// The CombatComponent class represents the combat-related functionality of an entity within an action RPG game.
/// This component interfaces with a StatsComponent to track and modify the health of the entity,
/// and defines behavior for receiving damage, healing, and handling death.
/// </summary>
public class CombatComponent : MonoBehaviour
{
    protected StatsComponent statsComponent; //Required

    // Call this method to initialize the PlayerCombatComponent with a StatsComponent
    public void Initialize(StatsComponent statsComp)
    {
        statsComponent = statsComp;
    }

    public void TakeDamage(float damageAmount)
    {
        statsComponent.ModifyHealth(-damageAmount);
        if (statsComponent.IsDead())
        {
            Feint();
        }
    }

    public void Heal(float healAmount)
    {
        statsComponent.ModifyHealth(healAmount);
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
