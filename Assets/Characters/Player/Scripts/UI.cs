using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Player_Health_System playerHealthSystem;
    Slider healthBarSlider;
    [SerializeField] GameObject PlayerhealthBar;
    void Start()
    {
        
        playerHealthSystem = GetComponent<Player_Health_System>(); 

        // Find the child GameObject named "HealthBar" among all children
        //Transform healthBarTransform = null;

        //foreach (Transform child in GetComponentsInChildren<Transform>())
        //{
        //    if (child.name == "HealthBar")
        //    {
        //        healthBarTransform = child;
        //        break; // Exit the loop once found
        //    }
        //}

        if (PlayerhealthBar != null)
        {
            // Get the Slider component from the HealthBar GameObject
            healthBarSlider = PlayerhealthBar.GetComponent<Slider>();

            if (healthBarSlider != null)
            {
                
                // Initialize health bar
                healthBarSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.maxHealth;
            }
            else
            {
                Debug.LogError("No Slider component found on HealthBar.");
            }
        }
        else
        {
            Debug.LogError("HealthBar not found in children of " + gameObject.name);
        }

        if (playerHealthSystem != null)
        {
            playerHealthSystem.OnPlayerDamage += UpdateHealthBar;
        }
        else
        {
            Debug.LogError("Player_Health_System component not found on this GameObject.");
        }
    }


    private void UpdateHealthBar(float currentHealth)
    {
        if (playerHealthSystem != null)
        {
            healthBarSlider.value = currentHealth / playerHealthSystem.maxHealth;
        }
    }
}
