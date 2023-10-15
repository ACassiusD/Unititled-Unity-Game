using UnityEngine;
/// <summary>
/// Base Stats Component class for storing and managing player stats.
/// </summary>
public class StatsComponent
{
    public float health = 100.0f;
    public float stamina = 100.0f;

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0.0f, 100.0f);
    }

    public virtual void ConsumeStamina(float amount)
    {
        stamina -= amount;
        stamina = Mathf.Clamp(stamina, 0.0f, 100.0f);
    }
}
