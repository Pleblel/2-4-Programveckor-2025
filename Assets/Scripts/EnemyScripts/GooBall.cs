using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooBall : Projectile, IDamageAble
{
    GameObject playerPosition;
    public GameObject puddle;

    // Start is called before the first frame update
    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        playerPosition = GameObject.FindGameObjectWithTag("Player");
        travelSpeed = 23f;
        direction = (playerPosition.transform.position - transform.position).normalized;
        rb.velocity = direction * travelSpeed;
    }


    public void InitializeDamage(float spiderDamage)
    {
        damage = spiderDamage;
    }

    public override void Damage(BaseEntity _entity)
    {
        _entity.TakeDamage(damage);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BaseEntity entity = other.GetComponent<BaseEntity>();
            if (entity != null)
                Damage(entity);

            Destroy(gameObject);
            
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
            {
                Instantiate(puddle, hit.point, Quaternion.identity);  // Adjust to exact ground position
            }

            Destroy(gameObject);
        }
       

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
       
    }
}
