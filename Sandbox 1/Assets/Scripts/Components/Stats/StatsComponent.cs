using UnityEngine;
/// <summary>
/// Base Stats Component class for storing and managing player stats.
/// </summary>
public class StatsComponent : MonoBehaviour
{
    public float currentHealth = 90;
    public float maxHealth = 100;
    public float stamina = 100.0f;

    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
