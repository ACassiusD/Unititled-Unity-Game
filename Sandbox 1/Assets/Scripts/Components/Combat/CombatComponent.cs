using UnityEngine;

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
