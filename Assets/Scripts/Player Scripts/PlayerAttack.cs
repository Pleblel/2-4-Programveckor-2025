using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{

    // Pelle
    [Header("UI Elements")]
    [SerializeField] Slider chargeBarSlider; // The UI slider element

    float chargeSpeed = 0.5f;  // How fast the bar charges
    float maxCharge = 1f;  // Maximum charge amount
    float currentCharge = 0f;
    float lastChargeAmount = 0f;
   [SerializeField] float boxHeight = 1.2f;
    [SerializeField] float boxForward = 1.2f;
    bool isCharging = false;

  

    [Header("Attack Settings")]
    [SerializeField] Vector3 hitboxSize = new Vector3(1f, 1f, 1f); // Size of the hitbox
    [SerializeField] float hitboxDistance = 1f; // Distance in front of the player
    float pDamage;
    [SerializeField] float knockBackForce = 1f;
    [SerializeField] float knockBackDuration = 0.05f;
    float maxDamage;
    float minDamage;
    PlayerEntity PE;

    void Start()
    {
        PE = GetComponent<PlayerEntity>();
        pDamage = PE.playerDamage;
        maxDamage = pDamage;
        minDamage = maxDamage / 2.5f;


        // Ensure the slider starts at 0
        if (chargeBarSlider != null)
        {
            chargeBarSlider.minValue = 0;
            chargeBarSlider.maxValue = maxCharge;
            chargeBarSlider.value = lastChargeAmount;
        }
    }

    void Update()
    {
       


        // Detect mouse button press
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
        }

        // Detect mouse button release
        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            lastChargeAmount = currentCharge;
            DealDamage();
            currentCharge = 0f;
        }

        // Charge while mouse button is held
        if (isCharging)
        {
            Charge();
        }

        // Update UI
        if (chargeBarSlider != null)
        {
            chargeBarSlider.value = currentCharge;
        }
    }

    void Charge()
    {
        currentCharge += chargeSpeed * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0f, maxCharge);
    }

    // Damage calculation
    float CalculateDamage(float chargeTime)
    {
        float chargePercentage = chargeTime / maxCharge;
        return Mathf.Lerp(minDamage, maxDamage, chargePercentage);
    }

    void DealDamage()
    {
        // Calls damage calculatin.
        float _damage = CalculateDamage(currentCharge);
        Debug.Log(_damage);

        // Creates collider for the attack
        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        this.GetComponent<Animator>().Play("Attack");
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, hitboxSize / 2);
        foreach (Collider collider in hitColliders)
        {
            BaseEntity entity = collider.GetComponent<BaseEntity>();
            Rigidbody enemyRb = collider.GetComponent<Rigidbody>();
            NavMeshAgent enemyNMA = collider.GetComponent<NavMeshAgent>();
            //checks if there is an enemy
            if (entity != null && entity.isAlive && collider.CompareTag("Enemy"))
            {
                string enemyName = collider.gameObject.name;//check name
                GetComponent<PlayerHitSFX>().PlayHitSound(enemyName); //hit sound of enemy
                Debug.Log("bap");

                // Knocks back, deals damage and does death check
                StartCoroutine(Knockback(enemyRb, collider.transform.position, enemyNMA));
                entity.TakeDamage(_damage);
                entity.Death();
            }
        }
    }

    // Darren
    IEnumerator Knockback(Rigidbody enemyRb, Vector3 enemyPosition, NavMeshAgent NMA)
    {
        NMA.isStopped = true;
        float elpasedTime = 0f;
        Vector3 knockbackDirection = (enemyPosition - transform.position).normalized;


        while (elpasedTime < knockBackDuration)
        {
            enemyRb.AddForce(knockbackDirection * knockBackForce, ForceMode.Impulse);

            elpasedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        NMA.isStopped = false;

    }

    // Draw the hitbox for visualization in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = new Vector3(transform.position.x, transform.position.y * boxHeight, transform.position.z) + (transform.forward * boxForward) * hitboxDistance;
        Gizmos.DrawWireCube(boxCenter, hitboxSize);
    }
}

