using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Attack Settings")]
    private Vector3 hitboxSize = new Vector3(1f, 1f, 1f); // Size of the hitbox
    private float hitboxDistance = 1f; // Distance in front of the player
    

    public float movementSpeed { get; private set; }
   
    
    Vector3 movementDirection; 


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
    }

    // Update is called once per frame
    protected override void Update()
    {
        Mathf.RoundToInt(maxHealth);
        Mathf.RoundToInt(currentHealth);

        FacePlayer();
        playerPosition = GameObject.FindGameObjectWithTag("Player");

        if (PlayerIsInChaseRange())
            movementDirection = (playerPosition.transform.position - transform.position).normalized;
        else if (!PlayerIsInChaseRange())
            rb.velocity = Vector3.zero;

        if (canMeleeAttack && !isMeleeAttacking && IsPlayerInMeleeHitbox())
            StartCoroutine(MeleeAttack());
        else if (!IsPlayerInMeleeHitbox())
            isMeleeAttacking = false;
        

        if (PlayerIsInShootingRange() && canShoot && !PlayerIsInChaseRange())
            StartCoroutine("ShootGoo");

        Debug.Log(isMeleeAttacking);
    }

    private void FixedUpdate()
    {   
        if(PlayerIsInChaseRange() && !isShooting && !isMeleeAttacking)
        {
            Move(movementDirection);
        }
          
    }

    public void Move(Vector3 direction)
    {
        rb.velocity = direction * movementSpeed;
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

        if (IsPlayerInMeleeHitbox())
        {
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
                }
            }
        }

        yield return new WaitForSeconds(attackSpeed);
        canMeleeAttack = true;
        isMeleeAttacking = false;
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
