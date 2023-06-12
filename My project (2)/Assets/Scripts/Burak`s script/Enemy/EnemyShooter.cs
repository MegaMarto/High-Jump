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
    private float speedDuration = 5f;
    private Coroutine increaseSpeedCoroutine;
    [SerializeField]private float bulletSpeed=5f;
    

    

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

        // Update the fire timer and shoot if it is time
        fireTimer += Time.deltaTime;
        if (fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            if (!isPerformingAction)
            {
                StartCoroutine(PerformAction());
            }
        }

        // If the enemy isn't doing anything, play the idle animation
        if (!isPerformingAction)
        {
            animator.SetFloat("Speed", 0f); // Idle animation

            // Stop the speed increasing coroutine if it's running
            if (increaseSpeedCoroutine != null)
            {
                StopCoroutine(increaseSpeedCoroutine);
                increaseSpeedCoroutine = null;
            }
        }
        else
        {
            // If the enemy is moving, play the walking animation
            if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                // Start the coroutine to increase speed over time
                if (increaseSpeedCoroutine == null)
                {
                    increaseSpeedCoroutine = StartCoroutine(IncreaseSpeedOverTime());
                }
            }
            else
            {
                animator.SetFloat("Speed", 1f); // Shooting/Performing action animation

                // Stop the speed increasing coroutine if it's running
                if (increaseSpeedCoroutine != null)
                {
                    StopCoroutine(increaseSpeedCoroutine);
                    increaseSpeedCoroutine = null;
                }
            }
        }
    }


  
        
        
    
    private IEnumerator IncreaseSpeedOverTime()
    {
        float elapsedTime = 0f; // Time that has passed
        float startSpeed = 0f; // Starting speed
        float endSpeed = 5f; // Final speed

        while (elapsedTime < speedDuration)
        {
            elapsedTime += Time.deltaTime;
            float newSpeed = Mathf.Lerp(startSpeed, endSpeed, (elapsedTime / speedDuration));
            animator.SetFloat("Speed", newSpeed);
            yield return null;
        }

        // Ensure speed is set to endSpeed after the loop
        animator.SetFloat("Speed", endSpeed);

        // Reset the coroutine reference so it can be started again if needed
        increaseSpeedCoroutine = null;
    }
}
