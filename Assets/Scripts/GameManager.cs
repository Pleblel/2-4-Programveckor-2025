using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    public bool elctricityOn = true;

    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject canvas;
    Transform pauseMenuTransform;

    private AudioSettings audioSettings; 

    private AudioClip currentMusic;

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

        DisableMouse();
        /*
        audioSettings = FindObjectOfType<AudioSettings>();
        if(audioSettings = null)
        {
            audioSettings.LoadVolume();
        }*/
    }
    void Update()
    {
        try
        {
            canvas = GameObject.Find("Canvas");
            pauseMenuTransform = canvas.transform.Find("PauseMenu");
        }
        catch
        {
            Debug.Log("No canvas found");
        }
        
        
        if (pauseMenuTransform != null)
        {
            pauseMenuUI = pauseMenuTransform.gameObject;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("esc");
            Debug.Log("ispaused: " + isGamePaused);
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        audioManager.PlayMusic(currentMusic);

        DisableMouse();
    }

    void Pause()
    {
        print("inne i pause");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        audioManager.PlayMusic(audioManager.settingMusic);

        EnableMouse();

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded scene: " + scene.name);
        HandleSceneMusic(scene);
        /*
        if(scene.name != "MainMenu")
        {
            GameObject MainMenuUI = GameObject.Find("MainMenuUI");
            GameObject OptionMenuUI = GameObject.Find("OptionMenuUI");
            if(MainMenuUI != null)
            {
                Destroy(MainMenuUI); //destory ui in other scenes

            }
            if(OptionMenuUI != null)
            {
                Destroy(OptionMenuUI); 
            }
        }

        AudioSettings audioSettings = FindObjectOfType<AudioSettings>();
        if(audioSettings != null)
        {
            audioSettings.LoadVolume();
        }*/
    }

    public void DisableMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HandleSceneMusic(Scene scene)
    {
        Debug.Log("Handling scene with index: " + scene.buildIndex);

        if (scene.name == "MainMenu")
        {
            Debug.Log("MainMenu");

            //audioManager.PlayMusic(audioManager.mainMenuMusic);
            currentMusic = audioManager.mainMenuMusic;
            audioManager.PlayMusic(currentMusic);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if (scene.name == "GameScene")
        {
            Debug.Log("Game scene");

            //audioManager.PlayMusic(audioManager.calmGameMusic);

            currentMusic = audioManager.calmGameMusic;
            audioManager.PlayMusic(currentMusic);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
<<<<<<< Updated upstream
=======
<<<<<<< HEAD

    // Update is called once per frame
    void Update()
    {
        
    }
=======
>>>>>>> b3af3e4ca9275c95bbd68f02cbd0d3749f725d86
>>>>>>> Stashed changes
}