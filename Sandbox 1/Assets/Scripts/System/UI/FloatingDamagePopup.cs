using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class FloatingDamagePopup : MonoBehaviour
{
    public GameObject floatingDmgText;
    public CombatComponent combatComponent;
    public float floatingHealthDamageOffset = 15f;

    public void Awake()
    {
        // Fetch the CombatComponent from the parent GameObject
        combatComponent = GetComponentInParent<CombatComponent>();

        //Make sure the required components are attached
        Assert.IsNotNull(combatComponent, "CombatComponent is not set on " + combatComponent.name + "'s " + this.name);
        Assert.IsNotNull(floatingDmgText, "FloatingDmgText is not set on " + combatComponent.name + "'s " + this.name);

        // Subscribe to the OnAttacked event
        combatComponent.OnAttacked += DisplayFloatingCombatText;
    }

    // Display floating combat text
    public void DisplayFloatingCombatText(int? knockBackForce, Vector3? direction)
    {
        Vector3 popupPosition = combatComponent.transform.position;

        // Raise the y-coordinate to make the text appear higher
        popupPosition.y += floatingHealthDamageOffset;

        if (floatingDmgText)
        {
            // Instantiate the text relative to the CombatComponent's GameObject
            var floatingDamageText = Instantiate(floatingDmgText, popupPosition, combatComponent.transform.rotation, combatComponent.transform);
            Debug.Log(combatComponent.transform.rotation);

            // Update the text to display the current health
            floatingDamageText.GetComponent<TextMeshPro>().text = combatComponent.statsComponent.currentHealth.ToString();
        }
    }
}
