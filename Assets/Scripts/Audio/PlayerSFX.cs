using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    //Sofi
    public AudioClip audioClip, walkClip, runClip;
    private float threshold = 0.1f;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {

        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //see if pookie is moving
        if (Input.GetKey(KeyCode.LeftShift) && inputVector.magnitude > threshold)
        {
            //stop current steps

            if (audioManager.SFXSource.isPlaying && audioManager.SFXSource.clip != audioManager.running)
            {
                audioManager.StopSFX();
            }

            //play run footstep
            if(!audioManager.SFXSource.isPlaying || audioManager.SFXSource.clip != audioManager.running)
{
                audioManager.PlaySFX(audioManager.running);
            }


        }//check if walk and move
        else if (inputVector.magnitude > threshold)
        {
            //stop any sound
            if (audioManager.SFXSource.isPlaying && audioManager.SFXSource.clip != audioManager.walking)
            {
                audioManager.StopSFX();
            }


            //play step sound
            if (!audioManager.SFXSource.isPlaying || audioManager.SFXSource.clip != audioManager.walking)
            {
                audioManager.PlaySFX(audioManager.walking);
            }
        }
        else
        {
            if (audioManager.SFXSource.isPlaying)
                audioManager.StopSFX();
               
        }

    }



}
