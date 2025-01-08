using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider chargeBarSlider; // The UI slider element

    float chargeSpeed = 0.75f;  // How fast the bar charges
    float maxCharge = 1f;  // Maximum charge amount
    float currentCharge = 0f;
    public float lastChargeAmount = 0f;
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
            lastChargeAmount = currentCharge;
            currentCharge = 0;
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

    void UpdateBar()
    {
        if (chargeBarSlider != null)
        {
            chargeBarSlider.value = currentCharge;
        }
    }
}