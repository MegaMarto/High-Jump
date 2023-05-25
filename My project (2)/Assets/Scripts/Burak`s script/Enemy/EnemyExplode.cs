using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : EnemyAI
{
    [Header("Enemy Explosion Elements")]
    public GameObject explosionPrefab;
    public float explosionRadius = 5f;
    public Color[] damageColors;  // Array to hold the colors for different damage states

   
    protected override IEnumerator PerformAction()
    {
        isPerformingAction = true;

        Explode();

        yield return new WaitForSeconds(1f / 1f);

        isPerformingAction = false;
    }

    private void Explode()
    {
        // Instantiate the explosion prefab at the enemy's position
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Get all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // Iterate over each collider
        foreach (Collider collider in colliders)
        {
            // Check if the collider is the player
            if (collider.transform == player)
            {
                // Damage the player
                // You'll need to replace this with your own logic for damaging the player(reminder)
                Debug.Log("Player hit by explosion");
                Destroy(gameObject);

            }
        }

    }
     public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);  // Call the base class TakeDamage method

        // Change color of the object according to its health.
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            float healthPercentage = currentHealth / MaxHealth;
            int colorIndex = Mathf.Clamp(Mathf.FloorToInt((1 - Mathf.Pow(healthPercentage, 2)) * damageColors.Length), 0, damageColors.Length - 1);
            renderer.material.color = damageColors[colorIndex];

            // Check if the enemy is at the last color
            if (colorIndex >= damageColors.Length - 1)
            {
                // If it is, then trigger the explosion
                Explode();
                Destroy(gameObject);
            }
        }
        // Trigger the event
        EventManager.TriggerEnemyHealthChanged(currentHealth, MaxHealth);

    
    }
}
