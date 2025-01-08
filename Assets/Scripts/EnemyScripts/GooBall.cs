using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooBall : Projectile
{
    GameObject playerPosition; 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPosition = GameObject.FindGameObjectWithTag("Player");
        travelSpeed = 15f;
      
        direction = (playerPosition.transform.position - transform.position).normalized;
        rb.velocity = direction * travelSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
       
    }
}
