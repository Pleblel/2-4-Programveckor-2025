using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    float damage = 10f;
    public float knockBackForce = 1f;
    public float knockBackDuration = 0.3f;

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

            
                player.TakeDamage(damage);
                Debug.Log("Electrified");
                Rigidbody playerRb = collision.collider.GetComponent<Rigidbody>();
                PlayerMovement playerController = collision.collider.GetComponent<PlayerMovement>();

                if (playerRb != null && playerController != null)
                {

                     Debug.Log("Knocked back");
                       
                     if(!playerController.isBeingKnockedBack)
                     StartCoroutine(Knockback(playerRb, playerController, collision.collider.transform.position));
                }
            
               
        }
    }


    IEnumerator Knockback(Rigidbody playerRb, PlayerMovement playerController, Vector3 playerPosition)
    {


        playerController.isBeingKnockedBack = true;
        float elpasedTime = 0f;
        Vector3 knockbackDirection = (playerPosition - transform.position).normalized;


        while (elpasedTime < knockBackDuration)
        {
            playerRb.AddForce(knockbackDirection * knockBackForce, ForceMode.Impulse);

            elpasedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        playerController.isBeingKnockedBack = false;
    }
 }
