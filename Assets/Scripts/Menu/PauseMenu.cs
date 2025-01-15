using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameManager gm;
    public Button resumeButton;
    public Button menuButton;
    public Button quitButton;

    private void Awake()
    {
  
        
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        resumeButton = transform.Find("Resume").GetComponent<Button>();
        menuButton = transform.Find("Menu").GetComponent<Button>();
        quitButton = transform.Find("Quit").GetComponent<Button>();
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        menuButton.onClick.AddListener(OnMenuButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnQuitButtonClick()
    {
        gm.QuitGame();
    }

    private void OnMenuButtonClicked()
    {
        gm.LoadMenu();
    }


    private void OnResumeButtonClick()
    {
        gm.Resume();
    }
}