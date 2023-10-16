using UnityEngine;
/// <summary>
/// Base Stats Component class for storing and managing player stats.
/// </summary>
public class StatsComponent : MonoBehaviour
{
    public float currentHealth = 90;
    public float maxHealth = 100;
    public float stamina = 100.0f;

    //public virtual void TakeDamage(float damage)
    //{
    //    currentHealth -= damage;
    //    currentHealth = Mathf.Clamp(currentHealth, 0.0f, 100.0f);
    //}

    //public virtual void ConsumeStamina(float amount)
    //{
    //    stamina -= amount;
    //    stamina = Mathf.Clamp(stamina, 0.0f, 100.0f);
    //}
}
