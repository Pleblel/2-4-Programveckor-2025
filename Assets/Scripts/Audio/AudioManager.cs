using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("------------- Audio Source -------------")]

    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("------------- Audio Clip -------------")]
    [Header("CHARACTER")]
    public AudioClip lowHealthBeat;
    public AudioClip death;
    public AudioClip walking;
    public AudioClip running;
    public AudioClip hitSpider;
    public AudioClip hitArmadillo;
    public AudioClip hitMan;

    [Header("MUSIC")]
    public AudioClip mainMenuMusic;
    public AudioClip settingMusic;
    public AudioClip calmGameMusic;
    public AudioClip panicGameMusic;

    [Header("INTERACTIONS")]
    public AudioClip drag;
    public AudioClip button;
    public AudioClip lever;
    public AudioClip keycard;
    public AudioClip door;
    public AudioClip lightSwitchOn;
    public AudioClip lightSwitchOff;
    public AudioClip obtainItem;

    [Header("MOBS")]

    public AudioClip manCrying;
    public AudioClip manHitting;
    public AudioClip manScreaming;
    public AudioClip spiderShoot;
    public AudioClip spiderDeath;
    public AudioClip armadilloWalk;
    public AudioClip armadilloDash;
    public AudioClip armadilloWallHit;


    [Header("UI")]

    public AudioClip startButton;
    public AudioClip menuButtons;

    [Header("OTHER")]
    public AudioClip electricity;

    public static AudioManager Intance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void PlayMusic(AudioClip newClip)
    {
        Debug.Log("Attempting to play music: " + newClip.name);
        if (musicSource.clip == newClip && musicSource.isPlaying)
            return; //avoids restarting track

        musicSource.clip = newClip;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.Play();
    }
    
    public void StopSFX()
    {
        SFXSource.Stop();
    }
}
