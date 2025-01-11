using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloEnemy : BaseEntity, IMovable
{


    [Header("MovementVariables")]
    [SerializeField] float rollingSpeed = 12f;
    [SerializeField] float rollingRange = 10f;
    [SerializeField] float walkingRange = 15f;
    [SerializeField] float chargeUpRoll = 3.0f;
    Vector3 movementDirection;
    bool canRoll = true;
    bool isRolling = false;
    float originalMovementSpeed;

    [Header("Refereneces")]
    public GameObject player;
    Rigidbody rb;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask playerLayer;


    public float movementSpeed { get; private set; }

  

    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 100.0f;
        currentHealth = maxHealth;
        damage = 10.0f;
        attackSpeed = 5.0f;
        defense = 1000.0f;
        movementSpeed = 2.0f;
        originalMovementSpeed = movementSpeed;
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        Mathf.RoundToInt(maxHealth);
        Mathf.RoundToInt(currentHealth);

        player = GameObject.FindGameObjectWithTag("Player");


        if (!HasLineOfSight(player.transform))
        {
            rb.velocity = Vector3.zero;
            return;       
        }

        if (currentHealth <= 1)
        {
            currentHealth = maxHealth;
        }

        if (IsInWalkRange() || IsInRollRange())
            movementDirection = (player.transform.position - transform.position).normalized;
    }


    private void FixedUpdate()
    {

        if (!HasLineOfSight(player.transform)) return;

        if (IsInWalkRange() && !isRolling)
        Move(movementDirection);

        if(IsInRollRange() && canRoll && !isRolling)
        {
            StartCoroutine(Roll());
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


    public void Move(Vector3 direction)
    {
        rb.velocity = direction * movementSpeed;
    }

    IEnumerator Roll()
    {
        if (!canRoll) yield break;
        canRoll = false;
        isRolling = true;

        Vector3 rollDirection = movementDirection;

        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(chargeUpRoll);
        rb.velocity = rollDirection * rollingSpeed;

        while (isRolling)
        {
            yield return null;
        }

        rb.velocity = Vector3.zero;
        isRolling = false;

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



    private void OnCollisionEnter(Collision collision)
    {
        if (isRolling)
        {
            // Check if the collision is with the player or a wall
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
            {
                if (collision.gameObject.CompareTag("Player") && isRolling)
                {
                    BaseEntity playerEntity = collision.gameObject.GetComponent<BaseEntity>();

                    if(playerEntity != null)
                    playerEntity.TakeDamage(damage);
                      
                }

                isRolling = false; // Stop rolling
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
