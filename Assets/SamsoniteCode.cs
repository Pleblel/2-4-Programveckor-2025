using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SamsoniteCode : MonoBehaviour
{
    private List<int> digits = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    private Vector2 mouse;
    private int correctCombination;
    private int playerCombination;

    private void Start()
    {
        // Shuffle the list
        for (int i = 0; i < digits.Count; i++)
        {
            int randomIndex = Random.Range(i, digits.Count);
            (digits[i], digits[randomIndex]) = (digits[randomIndex], digits[i]); // Swap
        }

        // Get the first 4 numbers
        string uniqueSequence = $"{digits[0]}{digits[1]}{digits[2]}{digits[3]}";

        Debug.Log("Generated Unique Sequence: " + uniqueSequence);
    }
}
