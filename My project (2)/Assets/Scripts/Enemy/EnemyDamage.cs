using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDamage : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private TextMeshProUGUI enemyHealthText;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionEffectPrefab;
    //[SerializeField] private GameObject bloodEffectPrefab; // Add this line

    public float currentHealth;

    public float CurrentHealth { get { return currentHealth; } }
    public float MaxHealth { get { return maxHealth; } }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateEnemyHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateEnemyHealthUI();

        // Add these lines to instantiate the blood effect when taking damage
        //if (bloodEffectPrefab != null)
        //{
            //GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
           // Destroy(bloodEffect, 2f);
        //}

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void UpdateEnemyHealthUI()
        {
            enemyHealthText.text = $"Enemy Health: {currentHealth}/{maxHealth}";
        }

    private void Die()
    {
        if(explosionEffectPrefab !=null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosionEffect, 2f); // Adjust the duration based on the explosion effect's duration
        }
        // Handle the death of the enemy, e.g., play a death animation, destroy the object, etc.
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }
}
