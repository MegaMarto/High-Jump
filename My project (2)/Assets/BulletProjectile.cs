using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        float speed = 40f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other){
            BulletTarget target = other.GetComponent<BulletTarget>();
    if (target != null)
    {
        // Hit target
        Instantiate(vfxHitGreen, transform.position, Quaternion.identity);

        // Reduce the enemy's health
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            float damage = 10f; // Set this to the amount of damage you want the bullet to do
            enemy.TakeDamage(damage);
        }
    }
    else
    {
        // Hit something else
        Instantiate(vfxHitRed, transform.position, Quaternion.identity);
    }
    Destroy(gameObject);
    }
}
