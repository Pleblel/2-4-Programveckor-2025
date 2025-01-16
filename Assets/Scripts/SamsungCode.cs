using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SamsungCode : MonoBehaviour
{
    public GameObject uiPanel; // The UI Panel
    public GameObject buttonPrefab; // Prefab for a button
    public TextMeshProUGUI feedbackText; // Feedback text
    public int gridSize = 3; // Number of rows and columns (n × n grid)
    public int sequenceLength = 4; // Length of the sequence
    public LayerMask playerLayer; // Set to "Player" layer
    public GameManager gameManager; // Reference to the GameManager
    public float buttonSpacing = 120f; // Distance between buttons

    private List<int> correctSequence = new List<int>();
    private List<int> playerSequence = new List<int>();
    private bool isPlayerInside = false;
    private bool isUIActive = false;
    private List<Button> buttons = new List<Button>(); // Track generated buttons

    private void Start()
    {
        uiPanel.SetActive(false); // Initially hide the panel
        CenterUIPanel(); // Center the panel on the screen
        GenerateGrid();
        GenerateSequence();
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (isUIActive)
            {
                CloseUI();
            }
            else
            {
                OpenUI();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
        {
            isPlayerInside = false;
            if (isUIActive)
            {
                CloseUI();
            }
        }
    }

    private void OpenUI()
    {
        isUIActive = true;
        uiPanel.SetActive(true);
        feedbackText.text = "Tap the buttons in the correct order!";
        playerSequence.Clear(); // Reset the sequence
        gameManager.EnableMouse();
    }

    private void CloseUI()
    {
        isUIActive = false;
        uiPanel.SetActive(false);
        gameManager.DisableMouse();
    }

    private void CenterUIPanel()
    {
        RectTransform rectTransform = uiPanel.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("UI Panel is missing a RectTransform component!");
            return;
        }

        // Center the panel on the screen
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void GenerateGrid()
    {
        // Clear old buttons if any
        foreach (Transform child in uiPanel.transform)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        // Total number of buttons in the grid
        int totalButtons = gridSize * gridSize;

        for (int i = 0; i < totalButtons; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, uiPanel.transform);
            Button button = buttonObj.GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("Button prefab must have a Button component!");
                return;
            }

            buttons.Add(button);
            int index = i; // Capture index for closure
            button.onClick.AddListener(() => OnButtonPressed(index));

            // Set the button position
            RectTransform buttonTransform = buttonObj.GetComponent<RectTransform>();

            if (i == 0)
            {
                // Place the first button in the center
                buttonTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                // Offset the remaining buttons around the center
                float angle = (i - 1) * Mathf.PI * 2f / (totalButtons - 1); // Circular layout
                float x = Mathf.Cos(angle) * buttonSpacing;
                float y = Mathf.Sin(angle) * buttonSpacing;
                buttonTransform.anchoredPosition = new Vector2(x, y);
            }
        }
    }

    private void GenerateSequence()
    {
        correctSequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, buttons.Count);
            correctSequence.Add(randomIndex);
        }
        Debug.Log("Generated Sequence: " + string.Join(", ", correctSequence));
    }

    private void OnButtonPressed(int buttonIndex)
    {
        playerSequence.Add(buttonIndex);

        if (!IsSequenceCorrect())
        {
            feedbackText.text = "Wrong! Try again.";
            playerSequence.Clear();
            return;
        }

        if (playerSequence.Count == correctSequence.Count)
        {
            feedbackText.text = "Success! You logged in.";
        }
    }

    private bool IsSequenceCorrect()
    {
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
                return false;
        }
        return true;
    }
}

