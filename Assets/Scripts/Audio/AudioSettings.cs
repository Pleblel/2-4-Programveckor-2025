using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("-------- Audio Mixer --------")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("-------- Sliders --------")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [Header("-------- Mute buttons --------")]
    [SerializeField] Sprite masterOnIcon;
    [SerializeField] Sprite masterOffIcon;
    [SerializeField] Sprite musicOnIcon;
    [SerializeField] Sprite musicOffIcon;
    [SerializeField] Sprite SFXOnIcon;
    [SerializeField] Sprite SFXOffIcon;
    [SerializeField] Button masterMute;
    [SerializeField] Button musicMute;
    [SerializeField] Button SFXMute;

    private bool masterMuted = false;
    private bool musicMuted = false;
    private bool SFXMuted = false;

    public int Length { get; private set; }

    private void Awake()
    {
        if(FindObjectOfType<AudioSettings>().Length > 1)
        {
            DontDestroyOnLoad(gameObject);
            return;
        }


        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
        }

        LoadMuteStates();
        UpdateButtonIcon();

    }

    private void Update()
    {
        FindSlidersInNewScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("loading new scene");
       
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void FindSlidersInNewScene()
    {

        masterSlider = GameObject.Find("Master Volume")?.GetComponent<Slider>();
        musicSlider = GameObject.Find("Music Volume")?.GetComponent<Slider>();
        SFXSlider = GameObject.Find("Sound Effects")?.GetComponent<Slider>();


        // If sliders are found, attach listeners and initialize them
        if (masterSlider != null)
        {
            masterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f); 
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f); 
        }

        if (SFXSlider != null)
        {
            SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f); 
        }
        
        LoadVolume();
    }
    public void SetMasterVolume()
    {
        Debug.Log("master vol saved");
        float volume = masterSlider.value;// Apply to the AudioMixer
        audioMixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume); // Save value to PlayerPrefs
    }       
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }    
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void LoadVolume()
    {

        if (masterSlider != null)
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);

        if (musicSlider != null)
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);

        if (SFXSlider != null)
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();

    }

    public void ToggleMasterMute()
    {
        masterMuted = !masterMuted;
        float volume = masterSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void ToggleMusicMute()
    {
        musicMuted = !musicMuted;
        audioMixer.SetFloat("music", musicMuted ? -80f : Mathf.Log10(masterSlider.value) * 20);
        PlayerPrefs.SetInt("musicMuted", musicMuted ? 1 : 0);
        UpdateButtonIcon();
    }
    public void ToggleSFXMute()
    {
        SFXMuted = !SFXMuted;
        audioMixer.SetFloat("SFX", SFXMuted ? -80f : Mathf.Log10(masterSlider.value) * 20);
        PlayerPrefs.SetInt("SFXMuted", SFXMuted ? 1 : 0);
        UpdateButtonIcon();
    }

    private void LoadMuteStates()
    {
        masterMuted = PlayerPrefs.GetInt("masterMuted", 0) == 1;
        musicMuted = PlayerPrefs.GetInt("musicMuted", 0) == 1;
        SFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        audioMixer.SetFloat("master", masterMuted ? -80f : Mathf.Log10(masterSlider.value) * 20);
        audioMixer.SetFloat("music", musicMuted ? -80f : Mathf.Log10(musicSlider.value) * 20);
        audioMixer.SetFloat("SFX", SFXMuted ? -80f : Mathf.Log10(SFXSlider.value) * 20);
    }

    public void UpdateButtonIcon()
    {
        if (masterMute != null)
            masterMute.image.sprite = masterMuted ? masterOffIcon : masterOnIcon;

        if (masterMuted)
        {
            masterMute.image.sprite = masterOffIcon;
        }
        else
        {
            masterMute.image.sprite = masterOnIcon;
        }

    }

 
}
