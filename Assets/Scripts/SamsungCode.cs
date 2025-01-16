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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseUI()
    {
        isUIActive = false;
        uiPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        GridLayoutGroup gridLayout = uiPanel.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = uiPanel.AddComponent<GridLayoutGroup>();
        }

        // Configure GridLayoutGroup
        gridLayout.cellSize = new Vector2(100, 100);
        gridLayout.spacing = new Vector2(10, 10);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gridSize;

        // Clear old buttons if any
        foreach (Transform child in uiPanel.transform)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        // Create the grid of buttons
        for (int i = 0; i < gridSize * gridSize; i++)
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

