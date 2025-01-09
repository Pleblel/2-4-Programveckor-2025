using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : BaseEntity, IMovable
{
    [Header("Variables")]
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float meleeRange = 5f;
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
    public Vector3 hitboxSize = new Vector3(1f, 1f, 1f); // Size of the hitbox
    public float hitboxDistance = 1f; // Distance in front of the player
    

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

        playerPosition = GameObject.FindGameObjectWithTag("Player");

        if (PlayerIsInChaseRange())
            movementDirection = (playerPosition.transform.position - transform.position).normalized;
        else if (!PlayerIsInChaseRange())
            rb.velocity = Vector3.zero;

        if (PlayerIsInMeleeRange() && canMeleeAttack && !isShooting)
            StartCoroutine(MeleeAttack());
            


        if (PlayerIsInShootingRange() && canShoot && !PlayerIsInChaseRange())
            StartCoroutine("ShootGoo");
    }

    private void FixedUpdate()
    {   
        if(PlayerIsInChaseRange() && !isShooting && !isMeleeAttacking)
        {
            Move(movementDirection);
            StopAllCoroutines();
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
        currentHealth -= damage;
    }


    private bool PlayerIsInMeleeRange()
    {
        if (playerPosition == null) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.transform.position);

        return distanceToPlayer <= chaseRange;
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

        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, hitboxSize / 2);
        
        foreach (Collider collider in hitColliders)
        {
            BaseEntity entity = collider.GetComponent<BaseEntity>();
            if (entity != null && entity.isAlive && entity.CompareTag("Player"))
            {
                Attack(entity);
                entity.Death();
            }
        }
        yield return new WaitForSeconds(attackSpeed);
        canMeleeAttack = true;
        isMeleeAttacking = false;
    }

    private void FacePlayer()
    {
       
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        Gizmos.color = Color.magenta;
        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        Gizmos.DrawWireCube(boxCenter, hitboxSize);
    }
}
