using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderEnemy : BaseEntity, IMovable
{
    //Darren

    [Header("Variables")]
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float shootingRange = 15f;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private bool canShoot = true;
    private bool isMeleeAttacking = false;
    private bool canMeleeAttack = true; 


    [Header("References")]
    [SerializeField] private GameObject playerPosition;
    Rigidbody rb;
    public GameObject Goo;
    public Transform bulletSpawn;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Settings")]
    private Vector3 hitboxSize = new Vector3(1f, 1f, 1f); // Size of the hitbox
    private float hitboxDistance = 1f; // Distance in front of the player
    [SerializeField] float knockBackForce = 20f;
    [SerializeField] float knockBackDuration = 0.2f;

    public float movementSpeed { get; private set; }
    private NavMeshAgent navMeshAgent;



    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;
        movementSpeed = 5.0f; 
        damage = 5.0f;
        attackSpeed = 1.5f;
        defense = 1.0f;

        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Mathf.RoundToInt(maxHealth);
        Mathf.RoundToInt(currentHealth);

        Debug.Log(currentHealth);
       
        playerPosition = GameObject.FindGameObjectWithTag("Player");

        if (!HasLineOfSight(playerPosition.transform))
        {
            navMeshAgent.ResetPath();
            return;
        }

        FacePlayer();

        if (!PlayerIsInChaseRange() || IsPlayerInMeleeHitbox())
            navMeshAgent.isStopped = true;

        if (canMeleeAttack && !isMeleeAttacking && IsPlayerInMeleeHitbox())
            StartCoroutine(MeleeAttack());         
              

        if (PlayerIsInShootingRange() && canShoot && !PlayerIsInChaseRange())
            StartCoroutine("ShootGoo");

        if (PlayerIsInChaseRange() && !isShooting && !isMeleeAttacking && !IsPlayerInMeleeHitbox())
        {
            navMeshAgent.isStopped = false;
            Move(playerPosition.transform.position);
        }
    }

    private void FixedUpdate()
    {
       
          
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

        //AudioManager.Instance.PlayMusic(AudioManager.Instance.spiderDeath); 
    }
    public override void TakeDamage(float damage)
    {
        currentHealth -= (damage / defense);
    }


    private bool PlayerIsInChaseRange()
    {
        if (playerPosition == null) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.transform.position);

        return distanceToPlayer <= chaseRange;
        
    }

    private bool PlayerIsInShootingRange()
    {
        if (playerPosition == null) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.transform.position);

        return distanceToPlayer <= shootingRange;
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


    IEnumerator ShootGoo()
    {
        canShoot = false;
        isShooting = true;
        GameObject GooInstance = Instantiate(Goo, bulletSpawn.position, Quaternion.identity);
        GooBall gooScript = GooInstance.GetComponent<GooBall>();

        //AudioManager.Instance.PlayMusic(AudioManager.Instance.spiderShoot);

        if (gooScript != null) 
        {
            gooScript.InitializeDamage(damage);
        }
    
        yield return new WaitForSeconds(attackSpeed);
        canShoot = true;
        isShooting = false;      
    }

    IEnumerator MeleeAttack()
    {
        canMeleeAttack = false;
        isMeleeAttacking = true;

        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(0.4f);

            //Pelle (Collider) Darren(The rest)
            //Sets a position on where the collider should be to hit the player
            Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, hitboxSize / 2);
            foreach (Collider collider in hitColliders)
            {
                
                if (collider.CompareTag("Player"))
                {
                    BaseEntity entity = collider.GetComponent<BaseEntity>();
                    Attack(entity);
                    entity.Death();
                    Debug.Log("Attacked");


                    Rigidbody playerRb = collider.GetComponent<Rigidbody>();
                    PlayerMovement playerController = collider.GetComponent<PlayerMovement>();
                    if (playerRb != null && playerController != null)
                    {

                    yield return StartCoroutine(Knockback(playerRb, playerController, collider.transform.position));
              
                    }

                }
            }

        yield return new WaitForSeconds(0.2f);
        isMeleeAttacking = false;
        navMeshAgent.isStopped = false;

        yield return new WaitForSeconds(attackSpeed);
        canMeleeAttack = true;
    }


    IEnumerator Knockback(Rigidbody playerRb, PlayerMovement playerController, Vector3 playerPosition)
    {


        playerController.isBeingKnockedBack = true;
        float elpasedTime = 0f;
        Vector3 knockbackDirection = new Vector3(playerPosition.x - transform.position.x, 0,0).normalized;


        while (elpasedTime < knockBackDuration)
        {
            playerRb.AddForce(knockbackDirection * knockBackForce, ForceMode.Impulse);

            elpasedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        playerController.isBeingKnockedBack = false;
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
        if (playerPosition != null)
        {
            // Calculate the direction to the player
            Vector3 direction = playerPosition.transform.position - transform.position;

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRange);

        Gizmos.color = Color.magenta;
        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        Gizmos.DrawWireCube(boxCenter, hitboxSize);
    }
}
