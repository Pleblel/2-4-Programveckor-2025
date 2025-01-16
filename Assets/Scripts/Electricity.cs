using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    float damage = 10f;
    public float knockBackForce = 1f;

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
            {
                // player.TakeDamage(damage);
                Debug.Log("Electrified");
                Rigidbody playerRb = collision.collider.GetComponent<Rigidbody>();
                PlayerMovement playerController = collision.collider.GetComponent<PlayerMovement>();

                if (playerRb != null && playerController != null)
                {

                    playerController.isBeingKnockedBack = true;
                    Vector3 knockbackDirection = new Vector3(collision.collider.transform.position.x - transform.position.x, 0, 0).normalized;


                    if(playerController.isBeingKnockedBack)
                    {
                        playerRb.AddForce(knockbackDirection * knockBackForce, ForceMode.Impulse);
                    }

                    playerController.isBeingKnockedBack = false;

                }
            }
               
        }
    }

}
