using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_V2 : MonoBehaviour
{
public Transform player;
    public GameObject explosionPrefab;
    private EnemyDamage enemyDamage;

    public float detectionRadius = 10f;
    public float stopDistance = 2f;
    //public float fireRate = 1f;
    public float avoidDistance = 8f;
    public float lowHealthThreshold = 30f;
    public float minDistanceFromOtherEnemies = 2f;
    public float explosionRadius = 5f;

    public float raycastDistance = 1f;
    public string objectTag = "Enemy";
    public Transform raycastTransform;
    public Color rayColor = Color.red;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isPerformingAction = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(1, 100);
        agent.stoppingDistance = stopDistance;
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        enemyDamage = GetComponent<EnemyDamage>();
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float currentHealth = enemyDamage.CurrentHealth;

            RaycastHit hit;
            Vector3 rayOrigin = raycastTransform.position;
            Vector3 rayDirection = raycastTransform.forward;

            // Visualize the raycast using Debug.DrawLine
            Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * raycastDistance, rayColor);

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag(objectTag))
                {
                    Debug.Log("Object detected: " + hit.collider.gameObject.name);
                }
            }

            if (distanceToPlayer <= detectionRadius)
            {
                if (currentHealth <= lowHealthThreshold)
                {
                    AvoidPlayer();
                }
                else
                {
                    ChaseAndAttackPlayer(distanceToPlayer);
                }
            }
            else
            {
                agent.ResetPath();
            }

            UpdateAnimator();
        }
    }

    private void ChaseAndAttackPlayer(float distanceToPlayer)
    {
        agent.SetDestination(player.position);

        // Rotate towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        if (agent.remainingDistance <= agent.stoppingDistance && !isPerformingAction)
        {
            StartCoroutine(PerformAction());
        }
    }

    private void AvoidPlayer()
    {
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector3 avoidDestination = transform.position + directionAwayFromPlayer * avoidDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(avoidDestination, out hit, avoidDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private IEnumerator PerformAction()
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
            }
        }

        // Destroy the enemy
        Destroy(gameObject);
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            float speed = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void OnPlayerHit()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > detectionRadius)
            {
                // Temporarily increase the detection radius to engage the player
                detectionRadius *= 2f;
                StartCoroutine(ResetDetectionRadiusAfterDelay(5f));
            }
        }
    }

    private IEnumerator ResetDetectionRadiusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        detectionRadius /= 2f;
    }
}

    


