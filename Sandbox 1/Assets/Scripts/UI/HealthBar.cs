using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls a 'horizontal filled' image element on the entitys canvis visually display a health bar.
/// Update the fill percentage of the image based on the passed current health and max health
/// Update the Health bar display in the UI
/// </summary>
public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    public void setHealth(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            double rawPercentage = (float)currentHealth / (float)maxHealth;
            double percentage = Math.Round(rawPercentage, 2);
            healthBar.fillAmount = (float)percentage;
        }
    }
}
