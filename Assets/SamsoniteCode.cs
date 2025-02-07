using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SamsoniteCode : MonoBehaviour
{
    public delegate void KeypadActivationHandler();
    public static event KeypadActivationHandler onKeypadCorrect;

    [Header("Keypad Objects")]
    [SerializeField] private TextMeshProUGUI textUIOutput;
    [SerializeField] private Button[] numberButtons = new Button[10];
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button enterButton;

    private List<int> digits = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private string correctCombination;
    private string playerCombination = "";

    public TextMeshProUGUI correctCodeText;
    public GameObject keyPad;

    public GameObject openDoorObject;
    public GameObject closedDoorObject;
    public BoxCollider bC;

    private void Start()
    {
        openDoorObject.SetActive(false);
        closedDoorObject.SetActive(true);


        textUIOutput.text = "";

        for (int i = 0; i < digits.Count; i++)
        {
            int randomIndex = Random.Range(i, digits.Count);
            (digits[i], digits[randomIndex]) = (digits[randomIndex], digits[i]); // Swap
        }

        correctCombination = $"{digits[0]}{digits[1]}{digits[2]}{digits[3]}";

        correctCodeText.text = correctCombination.ToString();

        for (int i = 0; i < numberButtons.Length; i++)
        {
            int index = i;
            numberButtons[i].onClick.AddListener(() => AddNumber(index.ToString()));
        }

        deleteButton.onClick.AddListener(DeleteLastDigit);
        enterButton.onClick.AddListener(CheckCode);
    }

    private void AddNumber(string number)
    {
        if (playerCombination.Length < correctCombination.Length) // Limit to correct code length
        {
            playerCombination += number;
            textUIOutput.text = playerCombination;
        }
    }

    void DeleteLastDigit()
    {
        if (playerCombination.Length > 0)
        {
            playerCombination = playerCombination.Substring(0, playerCombination.Length - 1);
            textUIOutput.text = playerCombination;
        }
    }

    void CheckCode()
    {
        if (playerCombination == correctCombination)
        {
            Debug.Log("Correct Code Entered!");
            textUIOutput.text = "ACCESS GRANTED";
            openDoorObject.SetActive(true);
            closedDoorObject.SetActive(false);
            bC.enabled = false;
            keyPad.SetActive(false);
            enabled = false;
            
        }
        else
        {
            Debug.Log("Wrong Code!");
            textUIOutput.text = "ACCESS DENIED";
            playerCombination = ""; // Reset input
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            keyPad.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            keyPad.SetActive(false);
    }
}
