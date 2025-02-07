using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Animator anim;


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

    }

    public void OpenOptions()
    {
        anim.SetBool("ShowOptions", true);
        anim.SetBool("ReturnToMenu", false);
    }

    public void BackToMainMenu()
    {
        anim.SetBool("ShowOptions", false);
        anim.SetBool("ReturnToMenu", true);
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

}
