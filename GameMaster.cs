using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster Instance { get; private set; }

    public bool speedrunning;
    public bool disableAds;
    public float speedrunBestTime;
    public bool mutedSound;

    public int deathCounter;


    /*
     * public AudioClip crashSoft;
     * public AudioClip crashHard;
     * source.pitch = Random.Range(lowPitchRange, highPitchRange)
     * float hitVol = coll.relativeVelocity.magnitude * velToVol;
     * if(coll.relativeVelocity.magnitude < velocityClipSplit)
     * source.PlayOneShot(crashSoft, hitVol);
     * else
     * source.PlayOneShot(crashHard,hitVol); */

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        Load();

        /*if(currentScene == 1 && speedrunning == false)
        {
            audioSource.PlayOneShot(lvOneSoundtrack, 1f);
        }
        /*else if(currentScene == 1 && speedrunning == true)
        {
            audioSource.PlayOneShot(lvOneSoundtrackSR, 1f);
        }
        if(currentScene == 2 && speedrunning == false)
        {
            audioSource.PlayOneShot(lvTwoSoundtrack, 1f);
        }
        else if(currentScene == 2 && speedrunning == true)
        {
            audioSource.PlayOneShot(lvTwoSoundtrackSR, 1f);
        }*/
            
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.playerSpeedrunning = speedrunning;
        data.playerDisableAds = disableAds;
        data.playerBestTime = speedrunBestTime;
        data.playerMutedSound = mutedSound;
        data.playerDeaths = deathCounter;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            speedrunning = data.playerSpeedrunning;
            disableAds = data.playerDisableAds;
            speedrunBestTime = data.playerBestTime;
            mutedSound = data.playerMutedSound;
            deathCounter = data.playerDeaths;
        }
    }
    public void Quit()
    {
        GameMaster.Instance.Save();
        Application.Quit();
    }
    public void ActivateSpeedrunning()
    {
        if(speedrunning == false)
        {
            speedrunning = true;
        }
        else if(speedrunning == true)
        {
            speedrunning = false;
        }
        Save();
        Load();
    }
}

[Serializable]
class PlayerData
{
    public bool playerSpeedrunning;
    public bool playerDisableAds;
    public float playerBestTime;
    public bool playerMutedSound;
    public int playerDeaths;
}

