using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterHealth : MonoBehaviour,IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI playerHealthText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdatePlayerHealthUI();
    }
    private void Update()
{
    if (Input.GetKeyDown(KeyCode.U))
    {
        float damage = 10f; // Set the damage value you want to apply when pressing 'U'
        TakeDamage(damage);
    }
}

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        UpdatePlayerHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdatePlayerHealthUI()
    {
        playerHealthText.text = $"Player Health: {currentHealth}/{maxHealth}";
    }

    private void Die()
    {
        // Handle the death of the player, e.g., play a death animation, restart the level, etc.
        Debug.Log("Player died");
    }
}
