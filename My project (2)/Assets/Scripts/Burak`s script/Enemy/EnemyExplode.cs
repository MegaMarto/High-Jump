using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : EnemyAI
{
    [Header("Enemy Explosion Elements")]
    public GameObject explosionPrefab;
    public float explosionRadius = 5f;
    public int damage=30;
    private Coroutine increaseSpeedCoroutine;  



     protected new void Start()
    {
        base.Start();

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

   
    protected override IEnumerator PerformAction()
    {
        isPerformingAction = true;

        Explode();

        yield return new WaitForSeconds(1f / 1f);

        isPerformingAction = false;
    }
    protected  new void Update()
    {
        base.Update(); // Call the base class Update method
        
        
        
            // If the enemy is moving, play the walking animation
            if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                 animator.SetBool("IsRunning", true);
                
                
            }
            else
            {
                animator.SetBool("IsRunning", false);
                
            }
        
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
            CharacterHealth character = collider.gameObject.GetComponent<CharacterHealth>();
            // Check if the collider is the player
            if (collider.transform == player)
            {
                // Damage the player
                // You'll need to replace this with your own logic for damaging the player(reminder)
                if (character != null)
                {
                    character.TakeDamage(damage);
                    Destroy(gameObject); // Destroy the bullet after it hits the character
                }
                Debug.Log("Player hit by explosion");
                Destroy(gameObject);

            }
        }

    }
    

    
}
