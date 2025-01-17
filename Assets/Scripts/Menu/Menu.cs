using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
