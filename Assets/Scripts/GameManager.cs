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
        DontDestroyOnLoad(gameObject);
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            audioManager.PlayMusic(audioManager.mainMenuMusic);

        }
        else if (scene.name == "GameScene")
        {
            audioManager.PlayMusic(audioManager.calmGameMusic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}