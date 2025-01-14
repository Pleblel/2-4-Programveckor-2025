using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private AudioManager audioManager;

    public bool elctricityOn = true;

    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(gameObject);
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager started");

        //HandleSceneMusic(SceneManager.GetActiveScene());


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        audioManager = FindObjectOfType<AudioManager>();

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded scene: " + scene.name);
        HandleSceneMusic(scene);
    }
    private void HandleSceneMusic(Scene scene)
    {
        Debug.Log("Handling scene with index: " + scene.buildIndex);

        if (scene.name == "MainMenu")
        {
            Debug.Log("MainMenu");

            audioManager.PlayMusic(audioManager.mainMenuMusic);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if (scene.name == "GameScene")
        {
            Debug.Log("Game scene");

            audioManager.PlayMusic(audioManager.calmGameMusic);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}