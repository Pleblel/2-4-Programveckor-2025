using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : BaseEntity, IMovable
{
    [Header("Variables")]
    [SerializeField] private float meleeRange = 10f;
    [SerializeField] private float shootingRange = 15f;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private bool canShoot = true;


    [Header("References")]
    [SerializeField] private GameObject playerPosition;
    Rigidbody rb;
    public GameObject Goo;
    public Transform bulletSpawn; 
   
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
      
        if(PlayerIsInMeleeRange())
        movementDirection = (playerPosition.transform.position  - transform.position).normalized;

        if (PlayerIsInShootingRange() && canShoot && !PlayerIsInMeleeRange())
            StartCoroutine("ShootGoo");
    }

    private void FixedUpdate()
    {   
        if(PlayerIsInMeleeRange() && !isShooting)
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

        return distanceToPlayer <= meleeRange;
        
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleeRange);


        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
