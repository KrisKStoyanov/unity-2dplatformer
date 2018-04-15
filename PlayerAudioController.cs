using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour {

    public static PlayerAudioController Instance;

    private AudioSource mainAudioSource;

    public AudioClip[] runSounds;

    public AudioClip runSoundOne;
    public AudioClip runSoundTwo;
    public AudioClip runSoundThree;
    public AudioClip runSoundFour;
    public AudioClip runSoundFive;

    public AudioClip jumpSound;
    public AudioClip fallSound;
    public AudioClip hitSound;
    public AudioClip pickUpSound;
    public AudioClip craftSound;
    public AudioClip throwSound;
    public AudioClip healSound;
    public AudioClip flyBoostSound;
    public AudioClip trackingSound;

    private void Awake()
    {
        Instance = this;
        mainAudioSource = GetComponent<AudioSource>();
        runSounds = new AudioClip[5];

        runSounds[0] = runSoundOne;
        runSounds[1] = runSoundTwo;
        runSounds[2] = runSoundThree;
        runSounds[3] = runSoundFour;
        runSounds[4] = runSoundFive;
    }

    public void PlayRunSound()
    {
        if(GameMaster.Instance.mutedSound == false)
        {
            mainAudioSource.clip = runSounds[Random.Range(0, runSounds.Length)];
            mainAudioSource.volume = LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol;
            mainAudioSource.Play();
        }        
    }

    public void PlayJumpSound()
    {
        if(GameMaster.Instance.mutedSound == false)
        {
            mainAudioSource.clip = jumpSound;
            mainAudioSource.volume = LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol;
            mainAudioSource.Play();
        }
    }
    public void PlayFallSound()
    {
        if(GameMaster.Instance.mutedSound == false)
        {
            mainAudioSource.clip = fallSound;
            mainAudioSource.volume = LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol;
            mainAudioSource.Play();
        }
    }

    public void PlayHitSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            AudioSource.PlayClipAtPoint(hitSound, gameObject.transform.position, LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol);
        }
    }

    public void PlayPickUpSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            AudioSource.PlayClipAtPoint(pickUpSound, gameObject.transform.position, LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol);
        }
    }
    public void PlayCraftSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            AudioSource.PlayClipAtPoint(craftSound, gameObject.transform.position , LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol);
        }
    }

    public void PlayThrowSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            AudioSource.PlayClipAtPoint(throwSound, gameObject.transform.position , LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol);
        }
    }

    public void PlayHealSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            AudioSource.PlayClipAtPoint(healSound, gameObject.transform.position, LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol);
        }
    }

    public void PlayFlyBoostSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            AudioSource.PlayClipAtPoint(flyBoostSound, gameObject.transform.position, LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol);
        }
    }

    public void PlayBladeTrackingSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            AudioSource.PlayClipAtPoint(trackingSound, gameObject.transform.position, LevelLoader.Instance.sfxVol * LevelLoader.Instance.masterVol);
        }
    }
}
