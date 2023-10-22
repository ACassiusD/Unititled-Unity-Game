using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image healthSlider;
    public Image staminaSlider;

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
    }

    void test()
    {
        Debug.Log("Test");
    }

    public void setHealth(float currentHealth, float maxHealth)
    {
        double rawPercentage = (float)currentHealth / (float)maxHealth;
        double percentage = System.Math.Round(rawPercentage, 2);
        healthSlider.fillAmount = (float)percentage;
    }

    public void setStamina(float current, float max)
    {
        double rawPercentage = (float)current / (float)max;
        double percentage = System.Math.Round(rawPercentage, 2);
        staminaSlider.fillAmount = (float)percentage;
    }


}
