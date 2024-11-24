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

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object with tag '" + playerTag + "' not found!");
        }

        
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer == null)
        {
            Debug.LogError("No Renderer found on enemy!");
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

    // Patrol randomly within the radius
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

    // Chase the player
    void ChasePlayer()
    {
        MoveToPoint(player.position);

        if (!CanSeePlayer())
        {
            isChasing = false; // Stop chasing if the player is out of sight
            SetColor(patrolColor); 
        }
    }

    void MoveToPoint(Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position).normalized;

        // Rotate towards the target point
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move forward in the direction the enemy is facing
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if within sight range and angle
        if (distanceToPlayer <= sightRange)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle <= sightAngle)
            {
                // Check for obstacles between enemy and player
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstaclesLayer))
                {
                    //// Check the player's light level
                    //PlayerLightDetection lightDetection = player.GetComponent<PlayerLightDetection>();
                    //if (lightDetection != null && !lightDetection.IsInShadow())
                    //{
                    return true; // Player is visible
                    //}
                }
            }
        }

        return false;
    }


    void SetColor(Color newColor)
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = newColor;
        }
    }

    // Debugging: Draw the field of view in the scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Vector3 forward = transform.forward * sightRange;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -sightAngle, 0) * forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, sightAngle, 0) * forward);
    }
}
