using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterHealth : MonoBehaviour
{
     [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component

    void Start()
    {
        currentHealth = maxHealth;
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Update() {
        UpdateHealthText();
        
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    private void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth.ToString();
    }
}
