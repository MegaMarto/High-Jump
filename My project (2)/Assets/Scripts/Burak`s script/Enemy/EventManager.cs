using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Delegate for the enemy health changed event
    public delegate void EnemyHealthChanged(float currentHealth, float maxHealth);
    // The actual enemy health changed event
    public static event EnemyHealthChanged OnEnemyHealthChanged;
    
    public static void TriggerEnemyHealthChanged(float currentHealth, float maxHealth)
    {
        OnEnemyHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
