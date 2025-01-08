using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : BaseEntity, IMovable
{
    [Header("Variables")]
    [SerializeField] private float meleeRange = 10f;
    [SerializeField] private float shootingRange = 15f;
    [SerializeField] private float shootingCooldown = 1.0f;
    [SerializeField] private float timeInBetweenShots = 0.1f;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private bool canShoot = true;


    [Header("References")]
    [SerializeField] private GameObject playerPosition;
    Rigidbody rb;
    GameObject Goo;
   
    public float movementSpeed { get; private set; }
   
    
    Vector3 movementDirection; 


    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;
        movementSpeed = 5.0f; 
        damage = 5.0f;
        attackSpeed = 1.0f;
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

        if (PlayerIsInShootingRange())
            StartCoroutine("ShootGoo");
    }

    private void FixedUpdate()
    {   
        if(PlayerIsInMeleeRange() && !isShooting)
        Move(movementDirection);   
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
        Instantiate(Goo, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(shootingCooldown);
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
