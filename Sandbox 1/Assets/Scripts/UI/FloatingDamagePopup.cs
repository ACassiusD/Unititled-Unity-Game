using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class FloatingDamagePopup : MonoBehaviour
{
    public GameObject floatingDmgText;
    public CombatComponent combatComponent;
    public float floatingHealthDamageOffset = 15f;

    // Duration to display text before destroying it
    // In the future we can destory them with an event based on the text animation ending
    // But for now we'll just use a timer
    private float textDisplayDuration = .35f;

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

    // Display floating combat text above the entity 
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
            
            // Start the coroutine to destroy the text after the specified duration
            StartCoroutine(DestroyTextAfterDelay(floatingDamageText, textDisplayDuration));
        }
    }
    private IEnumerator DestroyTextAfterDelay(GameObject textObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(textObject);
    }
}