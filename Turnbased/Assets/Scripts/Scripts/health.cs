using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth = 100;
    [SerializeField] private Image displayImage;
    


    void healing()
    {
        if (currentHealth < maxHealth && currentHealth > 0)
        {
            currentHealth += 10;
            UpdateUI();
        }
    }


    private void UpdateUI()
    {
        displayImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        
    }
}
