using System;
using UnityEngine;
/// <summary>
/// Base Stats Component class for storing and managing player stats.
/// </summary>
public class StatsComponent : MonoBehaviour
{
    public float currentHealth = 90;
    public float maxHealth = 100;
    public float stamina = 100.0f;

    public float ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        return currentHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public float GetCurrentHealthPercentage()
    {
        if (maxHealth == 0) return 0; // To prevent division by zero

        float rawPercentage = currentHealth / maxHealth;
        return (float)Math.Round(rawPercentage, 2);
    }
}
