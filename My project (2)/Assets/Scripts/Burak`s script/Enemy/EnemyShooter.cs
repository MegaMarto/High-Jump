using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyAI
{
    [Header("Enemy Shooting Elements")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float fireTimer = 0f;

    protected override IEnumerator PerformAction()
    {
        isPerformingAction = true;

        Shoot();

        yield return new WaitForSeconds(1f / fireRate);

        isPerformingAction = false;
    }

    private void Shoot()
    {
        if(firePoint != null && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
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
    }
}
