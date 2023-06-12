using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Movement Elements")]
    public Transform player;
    public float detectionRadius = 10f;
    public float stopDistance = 2f;
    public float avoidDistance = 8f;
    public float lowHealthThreshold = 30f;
    public float minDistanceFromOtherEnemies = 2f;

    public float raycastDistance = 1f;
    public string objectTag = "Enemy";
    public Transform raycastTransform;
    public Color rayColor = Color.red;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected bool isPerformingAction = false;
    

    [Header("Enemy Taken Damage Elements")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject explosionEffectPrefab;
   

     [Header("Enemy Health Elements")]
    public float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }
    public float MaxHealth { get { return maxHealth; } }
    private Color originalColor;
    private Renderer enemyRenderer;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(1, 100);
        agent.stoppingDistance = stopDistance;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        EventManager.TriggerEnemyHealthChanged(currentHealth, maxHealth);
        enemyRenderer = GetComponentInChildren<Renderer>();
        originalColor = enemyRenderer.material.color;

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        
        
    }

    public void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float currentHealth = CurrentHealth;

            RaycastHit hit;
            Vector3 rayOrigin = raycastTransform.position;
            Vector3 rayDirection = raycastTransform.forward;

            
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

    protected virtual IEnumerator PerformAction()
    {
        isPerformingAction = true;
        // This will be overridden in derived classes.
        yield return new WaitForSeconds(1f / 1f);
        isPerformingAction = false;
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

    


     public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        EventManager.TriggerEnemyHealthChanged(currentHealth, maxHealth);

         StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosionEffect, 2f);
        }
        
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }
     private IEnumerator FlashRed()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        enemyRenderer.material.color = originalColor;
    }
}




