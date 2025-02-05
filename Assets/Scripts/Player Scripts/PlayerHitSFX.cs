using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitSFX : MonoBehaviour
{
    public AudioClip spiderHit, armadilloHit, manHit;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayHitSound(string enemyName) //method that plays different sfx on different enemy
    {

        AudioClip selectedClip = null;

        switch (enemyName)
        {
        case "Spider":
            selectedClip = spiderHit;
            break;
        case "Armadillo":
            selectedClip = armadilloHit;
            break;
        case "Man":
            selectedClip = manHit;
            break;
        }
            
        if(selectedClip != null)
        {
            if(audioManager.SFXSource.isPlaying && audioManager.SFXSource.clip != selectedClip)
            {
                audioManager.StopSFX();
            }

            audioManager.PlaySFX(selectedClip);
        }
    }

}
