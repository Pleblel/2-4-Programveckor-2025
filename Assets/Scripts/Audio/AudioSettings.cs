using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] Image masterOnIcon;
    [SerializeField] Image masterOffIcon;
    [SerializeField] Image musicOnIcon;
    [SerializeField] Image musicOffIcon;
    [SerializeField] Image SFXOnIcon;
    [SerializeField] Image SFXOffIcon;

    private bool masterMuted = false;
    private bool musicMuted = false;
    private bool SFXMuted = false;

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
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
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

    private void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");


        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void ToggleMasterMute()
    {
        masterMuted = !masterMuted;
        audioMixer.SetFloat("master", masterMuted ? -80f : Mathf.Log10(masterSlider.value) * 20);
        PlayerPrefs.SetInt("masterMuted", masterMuted ? 1 : 0);
        UpdateButtonIcon();
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
        audioMixer.SetFloat("´SFX", SFXMuted ? -80f : Mathf.Log10(masterSlider.value) * 20);
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

    private void UpdateButtonIcon()
    {


        masterOnIcon.enabled = !masterMuted;
        masterOffIcon.enabled = masterMuted;

        musicOnIcon.enabled = !musicMuted;
        musicOffIcon.enabled = musicMuted;

        SFXOnIcon.enabled = !SFXMuted;
        SFXOffIcon.enabled = SFXMuted;

    }

 
}
