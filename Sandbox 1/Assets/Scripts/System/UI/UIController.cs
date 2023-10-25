using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image healthSlider;
    public Image staminaSlider;
    public PlayerCombatComponent playerCombatComponent; //Subscribe to its events to update the UI

    public static UIController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); //Must be moved to root of the gameobject.
        }
        else
        {
            Destroy(gameObject);
        }

        //Subscribe to the event.
        if (playerCombatComponent != null){
            playerCombatComponent.OnHealthChanged += SetHealth;
        }
        else{
            throw new System.Exception("CombatComponent not found on " + gameObject.name);
        }
    }

    public void SetHealth(float healthPercentage)
    {
        healthSlider.fillAmount = (float)healthPercentage;
    }

    public void SetStamina(float current, float max)
    {
        double rawPercentage = (float)current / (float)max;
        double percentage = System.Math.Round(rawPercentage, 2);
        staminaSlider.fillAmount = (float)percentage;
    }


}
