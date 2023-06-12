using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : EnemyAI
{
    [Header("Enemy Shooting Elements")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float fireTimer = 0f;
    
    
    [SerializeField]private float bulletSpeed=5f;
    
    

    protected new void Start()
    {
        base.Start();

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    

    protected override IEnumerator PerformAction()
    {
        isPerformingAction = true;
        
        ShootBullet();

        yield return new WaitForSeconds(1f / fireRate);

        isPerformingAction = false;
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
    }

     protected new void Update()
    {
        base.Update(); // Call the base class Update method

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Update the fire timer and shoot if it is time AND player is within detection radius
        fireTimer += Time.deltaTime;
        if (fireTimer >= 1f / fireRate && distanceToPlayer <= detectionRadius)
    {
        // Only shoot if enemy isn't running (agent.pathPending or remainingDistance > stoppingDistance indicates running)
        if (!isPerformingAction && !(agent.pathPending || agent.remainingDistance > agent.stoppingDistance))
        {
            fireTimer = 0f;
            StartCoroutine(PerformAction());
        }
    }

        // If the enemy isn't doing anything, play the idle animation
        
            // If the enemy is moving, play the walking animation
            if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                 animator.SetBool("IsRunning", true);
                // Start the coroutine to increase speed over time
                
            }
            else
            {
                animator.SetBool("IsRunning", false);
                //animator.SetFloat("Speed", 1f); // Shooting/Performing action animation

                
            }
        
    }


  
        
        
    
    
}
