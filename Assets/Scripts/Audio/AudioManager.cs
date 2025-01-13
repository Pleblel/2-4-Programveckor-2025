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

    [Header("MUSIC")]
    public AudioClip mainMenuMusic;
    public AudioClip settingMusic;
    public AudioClip calmGameMusic;
    public AudioClip panicGameMusic;

    [Header("INTERACTIONS")]
    public AudioClip drag;
    public AudioClip button;
    public AudioClip lever;
    public AudioClip hit;
    public AudioClip obtainItem;

    [Header("MOBS")]

    public AudioClip manCrying;
    public AudioClip manScreaming;
    public AudioClip ratNoise;
    public AudioClip ratDeath;
    public AudioClip spiderShoot;
    public AudioClip spiderDeath;
    public AudioClip armadilloDash;
    public AudioClip armadilloWallHit;


    [Header("OTHER")]
    public AudioClip jumpScareSound;


    private void Start()
    {

    }

    public void PlayMusic(AudioClip newClip)
    {
        if(musicSource == newClip && musicSource.isPlaying)
            return; //avoids restarting track

        musicSource.clip = newClip;
        musicSource.Play();
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
