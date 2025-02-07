using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Animator anim;

    public CanvasGroup mainMenuGroup; // Assign in Inspector
    public CanvasGroup optionsGroup;  // Assign in Inspector

    void Start()
    {
        anim = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("HasPlayedStartAnimation", 0) == 0)
        {
            anim.Play("MainMenuStart");
            PlayerPrefs.SetInt("HasPlayedStartAnimation", 1);
        }
        else
        {
            anim.Play("Idle");
        }

        ShowMainMenu(); // Ensure the main menu is visible at start
    }

    public void OpenOptions()
    {
        Debug.Log("Options Button Clicked");  // Debug check
        anim.SetBool("ShowOptions", true);
        anim.SetBool("ReturnToMenu", false);
        StartCoroutine(FadeCanvas(mainMenuGroup, 0));  // Fade out main menu
        StartCoroutine(FadeCanvas(optionsGroup, 1));   // Fade in options menu
    }

    public void BackToMainMenu()
    {
        Debug.Log("Back Button Clicked");  // Debug check
        anim.SetBool("ShowOptions", false);
        anim.SetBool("ReturnToMenu", true);
        StartCoroutine(FadeCanvas(optionsGroup, 0));  // Fade out options menu
        StartCoroutine(FadeCanvas(mainMenuGroup, 1)); // Fade in main menu
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float targetAlpha)
    {
        float duration = 0.5f;
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        canvasGroup.interactable = (targetAlpha == 1);
        canvasGroup.blocksRaycasts = (targetAlpha == 1);
    }

    private void ShowMainMenu()
    {
        // Main Menu is visible
        mainMenuGroup.alpha = 1;
        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;

        // Options Menu is completely hidden
        optionsGroup.alpha = 0;
        optionsGroup.interactable = false;
        optionsGroup.blocksRaycasts = false;

        // Ensure the animator is at the correct state
        anim.Play("Idle");
    }
}
