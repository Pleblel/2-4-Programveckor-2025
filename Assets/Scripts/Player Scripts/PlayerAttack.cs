using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider chargeBarSlider; // The UI slider element

    float chargeSpeed = 0.5f;  // How fast the bar charges
    float maxCharge = 1f;  // Maximum charge amount
    float currentCharge = 0f;
    float lastChargeAmount = 0f;
    bool isCharging = false;

    [Header("Attack Settings")]
    public Vector3 hitboxSize = new Vector3(1f, 1f, 1f); // Size of the hitbox
    public float hitboxDistance = 1f; // Distance in front of the player

    void Start()
    {
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
            SaveChargeAmount();
            DealDamage();
            ResetCharge();
        }

        // Charge while mouse button is held
        if (isCharging)
        {
            Charge();
        }

        // Update UI
        UpdateBar();
    }

    void Charge()
    {
        currentCharge += chargeSpeed * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0f, maxCharge);
    }

    void ResetCharge()
    {
        currentCharge = 0f;
    }

    void SaveChargeAmount()
    {
        lastChargeAmount = currentCharge;
    }

    void UpdateBar()
    {
        if (chargeBarSlider != null)
        {
            chargeBarSlider.value = currentCharge;
        }
    }   

    void DealDamage()
    {
        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, hitboxSize / 2);
        foreach (Collider collider in hitColliders)
        {
            BaseEntity entity = collider.GetComponent<BaseEntity>();
            if (entity != null && entity.isAlive)
            {
                entity.TakeDamage(lastChargeAmount * 100);
                entity.Death();
            }
        }
    }

    // Draw the hitbox for visualization in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position + transform.forward * hitboxDistance;
        Gizmos.DrawWireCube(boxCenter, hitboxSize);
    }
}

