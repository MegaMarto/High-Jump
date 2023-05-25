using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
private Rigidbody bulletRigidbody;

    public float damageAmount = 10f; // the amount of damage each bullet does

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        float speed = 20f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // assuming "Enemy" is the tag for your enemy objects
        {
            EnemyAI enemyDamage = other.gameObject.GetComponent<EnemyAI>();
            if (enemyDamage != null)
            {
                enemyDamage.TakeDamage(damageAmount); // reduce the health of the enemy
            }
        }
        Destroy(gameObject);
    }
}
