using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider chargeBarSlider; // The UI slider element

    [Header("Hitbox Settings")]
    public GameObject hitboxPrefab; // Hitbox prefab to spawn
    public float hitboxDuration = 0.5f; // Duration of hitbox

    float chargeSpeed = 0.5f;  // How fast the bar charges
    float maxCharge = 1f;  // Maximum charge amount
    float currentCharge = 0f;
    float lastChargeAmount = 0f;
    bool isCharging = false;

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
            SpawnHitbox();
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

    void SpawnHitbox()
    {
        if (hitboxPrefab != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward;
            GameObject hitbox = Instantiate(hitboxPrefab, spawnPosition, Quaternion.identity);

            Destroy(hitbox, hitboxDuration);
        }
    }
}
