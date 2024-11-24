using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float patrolRadius = 10f;
    public float sightRange = 8f;
    public float sightAngle = 45f;
    public float rotationSpeed = 5f;
    public string playerTag = "Player";
    public LayerMask obstaclesLayer;
    public Color patrolColor = Color.blue;
    public Color chaseColor = Color.red;

    private Transform player;
    private Vector3 patrolPoint;
    private bool isChasing = false;
    private Renderer enemyRenderer;
    private ShadowDetection shadowDetection;

    void Start()
    {
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
            Debug.LogError("Player object with tag '" + playerTag + "' not found!");
        }

        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer == null)
        {
            Debug.LogError("No enemy renderer found!");
        }

        SetColor(patrolColor);
        SetRandomPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        if (isChasing)
        {
            ChasePlayer();
            if (!IsPlayerInRange())
            {
                isChasing = false;
                SetColor(patrolColor);
                SetRandomPatrolPoint();
            }
        }
        else
        {
            Patrol();
            if (CanSeePlayer())
            {
                isChasing = true;
                SetColor(chaseColor);
            }
        }
    }

    void Patrol()
    {
        MoveToPoint(patrolPoint);

        if (Vector3.Distance(transform.position, patrolPoint) < 1f)
        {
            SetRandomPatrolPoint();
        }
    }

    void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;
        patrolPoint = randomDirection;
    }

    void ChasePlayer()
    {
        MoveToPoint(player.position);
    }

    void MoveToPoint(Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
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
