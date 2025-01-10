using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoisonPuddle : MonoBehaviour, IDamageAbleOvertime
{

    float damage = 1f;
    float aliveTimer = 3f;
    bool canDealDamage = true;
   
    public float damageTimer { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        damageTimer = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTimer -= Time.deltaTime;

        Die();
    }

    void Die()
    {
        if(aliveTimer <= 0)
        {
            Destroy(gameObject);
        }
    }


    public IEnumerator PassiveDamage(BaseEntity entity)
    {
        canDealDamage = false;
        entity.TakeDamage(damage);
        yield return new WaitForSeconds(damageTimer);
        canDealDamage = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canDealDamage)
        {
            BaseEntity player = other.GetComponent<BaseEntity>();

            if(player != null)
            StartCoroutine(PassiveDamage(player));
        }
             
    }
}
