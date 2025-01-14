using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArmadilloEnemy : BaseEntity, IMovable
{
    [Header("MovementVariables")]
    [SerializeField] float rollingSpeed = 12f;
    [SerializeField] float rollingRange = 10f;
    [SerializeField] float walkingRange = 15f;
    [SerializeField] float chargeUpRoll = 3.0f;
    Vector3 movementDirection;
    Vector3 rollDirection;
    bool canRoll = true;
    bool isRolling = false;
    bool bouncing = false;
    float originalMovementSpeed;
    Vector3 bounceDirection;

    [Header("References")]
    public GameObject player;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask playerLayer;
    Button buttonScript;

    public float movementSpeed { get; private set; }
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 100.0f;
        currentHealth = maxHealth;
        damage = 10.0f;
        attackSpeed = 5.0f;
        defense = 1000.0f;
        movementSpeed = 3.5f;
        originalMovementSpeed = movementSpeed;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        Mathf.RoundToInt(maxHealth);
        Mathf.RoundToInt(currentHealth);

        if (!HasLineOfSight(player.transform))
        {
            navMeshAgent.ResetPath();
            return;
        }

        if(!isRolling)
        FacePlayer();

        Bounce();

        if (currentHealth <= 1)
        {
            currentHealth = maxHealth;
        }

        if (IsInWalkRange() || IsInRollRange())
        {
            movementDirection = (player.transform.position - transform.position).normalized;
        }

        if (IsInRollRange() && canRoll && !isRolling && !bouncing)
        {
            StartCoroutine(Roll());
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            if (!HasLineOfSight(player.transform))
            {
                return;
            }
        }

        if (IsInWalkRange() && !isRolling && !bouncing)
        {
            Move(player.transform.position);
        }

        
    }

    private bool IsInWalkRange()
    {
        if (player == null) return false;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= walkingRange;
    }

    private bool IsInRollRange()
    {
        if (player == null) return false;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= rollingRange;
    }

    public void Move(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }


    private void Bounce()
    {
        if(bouncing)
            transform.position += bounceDirection * rollingSpeed * Time.deltaTime;
    }

    IEnumerator Roll()
    {
        if (!canRoll) yield break;

        canRoll = false;
        isRolling = true;

        navMeshAgent.isStopped = true; // Stop NavMeshAgent during roll
        rollDirection = movementDirection;
        yield return new WaitForSeconds(chargeUpRoll);

      
        while (isRolling)
        {
            transform.position += rollDirection * rollingSpeed * Time.deltaTime;
            yield return null;
        }

        isRolling = false;
        navMeshAgent.isStopped = false; // Resume NavMeshAgent
        yield return new WaitForSeconds(attackSpeed);
        canRoll = true;
    }

    public override void Attack(ILivingEntity entity)
    {
        entity.TakeDamage(damage);
    }

    public override void Death()
    {
        if (!isAlive)
            Destroy(gameObject);
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= (damage / defense);
    }

    private bool HasLineOfSight(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, distanceToTarget, obstacleLayer | playerLayer))
        {
            return hit.transform == target;
        }

        return false;
    }

    private void FacePlayer()
    {
        if (player != null && !isRolling)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isRolling || bouncing)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {

                isRolling = false;

                Vector3 collisionNormal = collision.contacts[0].normal;

                bounceDirection = Vector3.Reflect(rollDirection.normalized, collisionNormal);

                // Optionally, add a slight vertical offset to prevent sticking to the surface
                bounceDirection.y = 0;

                bouncing = true;

                Debug.Log("Wall hit");
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                BaseEntity playerEntity = collision.gameObject.GetComponent<BaseEntity>();
                if (playerEntity != null)
                {
                    Attack(playerEntity);
                }

                // Stop rolling after hitting the player
                isRolling = false;
                bouncing = false;
            }
            else if (collision.gameObject.CompareTag("Button"))
            {
                buttonScript = collision.gameObject.GetComponent<Button>();
                buttonScript.buttonPressed = true;

                // Stop rolling after pressing a button
                isRolling = false;
                bouncing = false;
            }


        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkingRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, rollingRange);
    }
}
