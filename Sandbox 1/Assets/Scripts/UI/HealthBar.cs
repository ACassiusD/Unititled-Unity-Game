using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update

 
    public void setHealth(int currentHealth, int maxHealth)
    {
        if(image != null)
        {
            double rawPercentage = (float)currentHealth / (float)maxHealth;
            double percentage = Math.Round(rawPercentage, 2);
            image.fillAmount = (float)percentage;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
