using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float sightRange = 8f;
    public float sightAngle = 45f;
    public string playerTag = "Player";
    public LayerMask obstaclesLayer;
    public Color patrolColor = Color.blue;
    public Color chaseColor = Color.red;
    public float patrolInterval = 5f; // Time in seconds between choosing new random points
    private AudioSource audioSource;
    public AudioClip ScreamSFX;
    private AudioClip currentSFX;

    private Transform player;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private Renderer enemyRenderer;
    private ShadowDetection shadowDetection;
    private Light spotlight;
    private float patrolTimer;
    private Animator animator;

    void Start()
    {
        // Initialize NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentSFX = audioSource.clip;

        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent component found on this GameObject!");
            return;
        }

        // Find player
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
            shadowDetection = playerObject.GetComponent<ShadowDetection>();
            if (shadowDetection == null)
            {
                Debug.LogError("ShadowDetection script not found");
            }
        }
        else
        {
            Debug.LogError($"Player object with tag '{playerTag}' not found!");
        }

        // Initialize Renderer
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer == null)
        {
            Debug.LogError("No Renderer found!");
        }

        spotlight = GetComponentInChildren<Light>();
        if (spotlight == null)
        {
            Debug.LogError("No spotlight found as a child of the enemy!");
        }

        SetColor(patrolColor);
        MoveToRandomNavMeshPoint();
    }

    void Update()
    {
        if (player == null) return;

        patrolTimer += Time.deltaTime;

        if (isChasing)
        {
            ChasePlayer();
            if (!CanSeePlayer())
            {
                isChasing = false;
                audioSource.volume = 0.5f;
                audioSource.clip = currentSFX;
                SetColor(patrolColor);
                animator.SetBool("PlayerSpotted", false);
                MoveToRandomNavMeshPoint();
            }
        }
        else
        {
            if (patrolTimer >= patrolInterval)
            {
                MoveToRandomNavMeshPoint();
                patrolTimer = 0f;
            }

            if (CanSeePlayer())
            {
                audioSource.volume = 1f;
                //audioSource.PlayOneShot(ScreamSFX);
                audioSource.Stop();
                audioSource.clip = ScreamSFX;
                audioSource.Play();
                isChasing = true;
                SetColor(chaseColor);
                animator.SetBool("PlayerSpotted", true);
            }
        }
    }

    /// <summary>
    /// Moves to a random point on the NavMesh.
    /// </summary>
    void MoveToRandomNavMeshPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 100f; // Large search area
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 100f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    bool CanSeePlayer()
    {
        if (player == null || shadowDetection == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle <= sightAngle)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstaclesLayer))
                {
                    if (!shadowDetection.isInShadow)
                    {
                        if (distanceToPlayer <= sightRange && distanceToPlayer <= 3.5f)
                        {
                            animator.SetBool("CloseToPlayer", true);
                        }
                        else if (distanceToPlayer <= sightRange)
                            animator.SetBool("CloseToPlayer", false);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    bool IsPlayerInRange()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer <= sightRange && distanceToPlayer <= 5f)
        {
            animator.SetBool("CloseToPlayer", true);
        } else if (distanceToPlayer <= sightRange)
            animator.SetBool("CloseToPlayer", false);
        return distanceToPlayer <= sightRange;
    }

    void SetColor(Color newColor)
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = newColor;
        }
    }
}
