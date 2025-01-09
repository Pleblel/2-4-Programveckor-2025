using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------------- Audio Source -------------")]

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------- Audio Clip -------------")]
    [Header("CHARACTER")]
    public AudioClip lowHealthBeat;
    public AudioClip death;
    public AudioClip walking;
    public AudioClip running;

    [Header("MUSIC")]
    public AudioClip mainMenuMusic;
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
        musicSource.clip = mainMenuMusic;
        musicSource.Play();
    }
}
