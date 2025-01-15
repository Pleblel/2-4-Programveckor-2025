using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    float damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            BaseEntity player = collision.collider.GetComponent<BaseEntity>();

            if (player != null)
                player.TakeDamage(damage);
        }
    }
}
