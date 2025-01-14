using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
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

            if (player != null)
                StartCoroutine(PassiveDamage(player));
        }

    }
}
