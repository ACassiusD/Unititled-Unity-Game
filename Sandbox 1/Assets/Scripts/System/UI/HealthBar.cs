using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls a 'horizontal filled' image element on the entitys canvis visually display a health bar.
/// Update the fill percentage of the image based on the passed current health and max health
/// Update the Health bar display in the UI
/// 
/// Must have a reference to the combat component in order to entitys health updated event.
/// </summary>
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public CombatComponent combatComponent; // Reference to the CombatComponent of the entity

    private void Start()
    {
        // Get the CombatComponent from the entity. Assumes both scripts are on the same GameObject.
        combatComponent = GetComponent<CombatComponent>();

        if (combatComponent != null)
        {
            combatComponent.OnHealthChanged += UpdateHealthBar;
        }
        else
        {
            Debug.LogWarning("CombatComponent not found on " + gameObject.name);
        }
    }

    private void UpdateHealthBar(float healthPercentage)
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = healthPercentage;
        }
    }

    private void OnDestroy()
    {
        // Ensure to unsubscribe from the event when this component is destroyed to prevent potential memory leaks
        if (combatComponent != null)
        {
            combatComponent.OnHealthChanged -= UpdateHealthBar;
        }
    }

}
