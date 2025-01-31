using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Screamer : BaseEntity, IMovable
{
    [Header("MovementVariables")]
    [SerializeField] float walkingRange = 15f;
    
    [Header("Refereneces")]
    public GameObject player;
    public ElectricityHandler gm;
    Rigidbody rb;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("FightingVariables")]
    [SerializeField] float electricityOnDefense = 5;
    float originalDefense;
    private Vector3 hitboxSize = new Vector3(1f, 1f, 1f); // Size of the hitbox
    private float hitboxDistance = 1f; // Distance in front of the player
    private bool isMeleeAttacking = false;
    private bool canMeleeAttack = true;
    [SerializeField] float knockBackDuration = 0.1f;
    [SerializeField] float knockBackForce = 1f;

    public float movementSpeed { get; private set; }
    private NavMeshAgent navMeshAgent;



    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 100.0f;
        currentHealth = maxHealth;
        damage = 25.0f;
        attackSpeed = 6.0f;
        defense = 1.0f;
        movementSpeed = 3.5f;
        originalDefense = defense;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;


        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<ElectricityHandler>();


    }

    // Update is called once per frame
    protected override void Update()
    {
        Mathf.RoundToInt(maxHealth);
        Mathf.RoundToInt(currentHealth);

        player = GameObject.FindGameObjectWithTag("Player");
        gm = FindObjectOfType<ElectricityHandler>();


        Debug.Log(navMeshAgent.isStopped);

        if (!gm.electricityOn) 
        {
            navMeshAgent.isStopped = true;
            return;
        }

        if (!HasLineOfSight(player.transform))
        {
            navMeshAgent.isStopped = true;
            return;
        }



        ChangeDefense();


        FacePlayer();

        bool inWalkRange = IsInWalkRange();
        bool inMeleeHitbox = IsPlayerInMeleeHitbox();

        // Update NavMeshAgent state based on current conditions
        navMeshAgent.isStopped = !inWalkRange || inMeleeHitbox || isMeleeAttacking;

        if (canMeleeAttack && !isMeleeAttacking && inMeleeHitbox)
            StartCoroutine(MeleeAttack());

        // If in walk range and not stopped, update destination
        if (inWalkRange && !navMeshAgent.isStopped)
        {
            Move(player.transform.position);    
        }


    }


    private void FixedUpdate()
    {
           
    }


    private void ChangeDefense()
    {
        if (gm.electricityOn)
            defense = electricityOnDefense;
        else if (!gm.electricityOn)
            defense = originalDefense;
    }

    private bool IsInWalkRange()
    {
        if (player == null) return false;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool inRange = distance <= walkingRange;
        return inRange;
    }


    public void Move(Vector3 direction)
    {
        navMeshAgent.SetDestination(direction);
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


    private bool IsPlayerInMeleeHitbox()
    {
        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, hitboxSize / 2);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


    private void FacePlayer()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 direction = player.transform.position - transform.position;

            // Zero out the y-axis rotation if you want the enemy to rotate only on the horizontal plane
            direction.y = 0;

            // Ensure the direction is normalized to avoid scaling issues
            direction.Normalize();

            // Calculate the rotation needed to face the player
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the player (optional)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }


    IEnumerator MeleeAttack()
    {
        canMeleeAttack = false;
        isMeleeAttacking = true;

        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.4f);

       
            //Pelle
            //Sets a position on where the collider should be to hit the player

            Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, hitboxSize / 2);
            foreach (Collider collider in hitColliders)
            {
                BaseEntity entity = collider.GetComponent<BaseEntity>();
                if (collider.CompareTag("Player"))
                {
                    Attack(entity);
                    entity.Death();

                   

                    Rigidbody playerRb = collider.GetComponent<Rigidbody>();
                    PlayerMovement playerController = collider.GetComponent<PlayerMovement>();
                    if (playerRb != null && playerController != null)
                    {

                       yield return Knockback(playerRb, playerController, collider.transform.position);

                    }
                }
            }
        

        yield return new WaitForSeconds(0.3f);
        isMeleeAttacking = false;
        navMeshAgent.isStopped = false;


        yield return new WaitForSeconds(attackSpeed);
        canMeleeAttack = true;
        
    }

    IEnumerator Knockback(Rigidbody playerRb, PlayerMovement playerController, Vector3 playerPosition)
    {


        playerController.isBeingKnockedBack = true;
        float elpasedTime = 0f;
        Vector3 knockbackDirection = (playerPosition - transform.position).normalized;


        while (elpasedTime < knockBackDuration)
        {
            playerRb.AddForce(knockbackDirection * knockBackForce, ForceMode.Impulse);

            elpasedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        playerController.isBeingKnockedBack = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, walkingRange);


        Gizmos.color = Color.cyan;
        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        Gizmos.DrawWireCube(boxCenter, hitboxSize);
    }
}
