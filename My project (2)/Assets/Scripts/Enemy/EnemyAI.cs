using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public EnemyData enemyData;
    private EnemyDamage enemyDamage;

    
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    


    
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
        agent.avoidancePriority = Random.Range(1, 100); // Add this line
        agent.stoppingDistance = enemyData.stopDistance;
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
                // Perform your desired action with the detected object
            }
        }
    

            if (distanceToPlayer <= enemyData.detectionRadius)
            {
                if (currentHealth <= enemyData.lowHealthThreshold)
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
        Vector3 avoidDestination = transform.position + directionAwayFromPlayer * enemyData.avoidDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(avoidDestination, out hit, enemyData.avoidDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private IEnumerator PerformAction()
    {
        isPerformingAction = true;

        ShootBullet();

        yield return new WaitForSeconds(1f / fireRate);

        isPerformingAction = false;
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
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
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRadius);
    }

    public void OnPlayerHit()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > enemyData.detectionRadius)
            {
                // Temporarily increase the detection radius to engage the player
                enemyData.detectionRadius *= 2f;
                StartCoroutine(ResetDetectionRadiusAfterDelay(5f));
            }
        }
    }

    private IEnumerator ResetDetectionRadiusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyData.detectionRadius /= 2f;
    }

    
}



