using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    public bool electricityOn = false;
    private Room currentRoom; // Tracks the player's current room

    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject canvas;
    public CanvasGroup CanvasGroup;
    Transform pauseMenuTransform;

    private AudioSettings audioSettings; 

    private AudioClip currentMusic;

    public GameObject MainMenuUI = GameObject.Find("MainMenuUI");
    public GameObject OptionMenuUI = GameObject.Find("OptionMenuUI");

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

     
        OptionMenuUI.SetActive(false);

    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager started");

        EnableMouse();
        
        audioSettings = FindObjectOfType<AudioSettings>();
        if(audioSettings == null)
        {
            audioSettings.LoadVolume();
        }

       
    }
    void Update()
    {
    


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


    public void SetCurrentRoom(Room room)
    {
        if (room != null)
        {
            currentRoom = room;
            electricityOn = room.hasElectricity;
            Debug.Log("Electricity is now: " + electricityOn);
        }
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

            pauseMenuUI.SetActive(false);
        }

        if (scene.name == "MainMenu")
        {
            //checks if main menu is there so option menu can be deactivated
            if(MainMenuUI == isActiveAndEnabled)
            {
                Debug.Log("menu is on ");
                if (OptionMenuUI == isActiveAndEnabled)
                {
                    Debug.Log("menu is on and I am deactivated");
                    OptionMenuUI.SetActive(false);
                }  
            }
            
            
        }

        AudioSettings audioSettings = FindObjectOfType<AudioSettings>();
        if(audioSettings != null)
        {
            audioSettings.LoadVolume();
        }
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


}