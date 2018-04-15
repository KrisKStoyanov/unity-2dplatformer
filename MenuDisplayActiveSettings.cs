using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplayActiveSettings : MonoBehaviour 
{
    public static MenuDisplayActiveSettings Instance;

    public GameObject spdrunSet;
    public GameObject muteSet;
    public GameObject disableAdsSet;

    public GameObject disableAdsBtn;

    public GameObject bestTimeDisplay;
    public Text bestTimeTextDisplay;
    public Image gameLogo;

    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        masterVolSlider.value = LevelLoader.Instance.masterVol;
        musicVolSlider.value = LevelLoader.Instance.musicVol;
        sfxVolSlider.value = LevelLoader.Instance.sfxVol;
        if (GameMaster.Instance.mutedSound == true)
        {
            muteSet.gameObject.SetActive(true);
        }
        else if (GameMaster.Instance.mutedSound == false)
        {
            muteSet.gameObject.SetActive(false);
        }
        if(GameMaster.Instance.speedrunning == true)
        {
            spdrunSet.gameObject.SetActive(true);
            if (GameMaster.Instance.speedrunBestTime > 0)
            {
                bestTimeDisplay.gameObject.SetActive(true);
                gameLogo.gameObject.SetActive(true);
                bestTimeTextDisplay.text = "Best Time: " + GameMaster.Instance.speedrunBestTime;
                bestTimeTextDisplay.color = new Color(bestTimeTextDisplay.color.r, bestTimeTextDisplay.color.g, bestTimeTextDisplay.color.b, bestTimeTextDisplay.color.a) + new Color(0, 0, 0, 0.5f);
            }

            else if (GameMaster.Instance.speedrunBestTime <= 0)
            {
                bestTimeDisplay.gameObject.SetActive(false);
                gameLogo.gameObject.SetActive(false);
            }
        }
        else if(GameMaster.Instance.speedrunning == false)
        {
            spdrunSet.gameObject.SetActive(false);
        }
        if(GameMaster.Instance.disableAds == true)
        {
            disableAdsSet.gameObject.SetActive(true);
            disableAdsBtn.GetComponent<Button>().interactable = false;
        }
        if (GameMaster.Instance.disableAds == false)
        {
            disableAdsSet.gameObject.SetActive(false);
            disableAdsBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void LoadFirstLevel()
    {
        LevelLoader.Instance.LoadLevel(1);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void SwitchSpeedrunning()
    {
        GameMaster.Instance.ActivateSpeedrunning();
        if (GameMaster.Instance.speedrunning == true)
        {
            spdrunSet.SetActive(true);
            if (GameMaster.Instance.speedrunBestTime > 0)
            {
                bestTimeDisplay.gameObject.SetActive(true);
                gameLogo.gameObject.SetActive(true);
                bestTimeTextDisplay.text = "Best Time: " + GameMaster.Instance.speedrunBestTime;
                bestTimeTextDisplay.color = new Color(bestTimeTextDisplay.color.r, bestTimeTextDisplay.color.g, bestTimeTextDisplay.color.b, bestTimeTextDisplay.color.a) + new Color(0, 0, 0, 0.5f);
            }

            else if (GameMaster.Instance.speedrunBestTime <= 0)
            {
                bestTimeDisplay.gameObject.SetActive(false);
                gameLogo.gameObject.SetActive(false);
            }
        }
        else if (GameMaster.Instance.speedrunning == false)
        {
            spdrunSet.SetActive(false);
            bestTimeDisplay.gameObject.SetActive(false);
            gameLogo.gameObject.SetActive(false);
        }
        GameMaster.Instance.Save();
        GameMaster.Instance.Load();
    }
    public void ToggleMute()
    {
        if(LevelLoader.Instance.soundtrackSource.mute == false)
        {
            //LevelLoader.Instance.audioSource.volume = 0;
            LevelLoader.Instance.soundtrackSource.mute = true;
            GameMaster.Instance.mutedSound = true;
            muteSet.gameObject.SetActive(true);
        }
        else if(LevelLoader.Instance.soundtrackSource.mute == true)
        {
            //LevelLoader.Instance.audioSource.volume = 1;
            LevelLoader.Instance.soundtrackSource.mute = false;
            muteSet.gameObject.SetActive(false);
            GameMaster.Instance.mutedSound = false;
        }
        GameMaster.Instance.Save();
        GameMaster.Instance.Load();

    }

    public void ToggleAds()
    {
        if (GameMaster.Instance.disableAds == true)
        {
            disableAdsSet.SetActive(true);
            disableAdsBtn.GetComponent<Button>().interactable = false;
        }
        else if (GameMaster.Instance.disableAds == false)
        {
            disableAdsSet.SetActive(false);
            disableAdsBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void ChangeMasterVol()
    {
        LevelLoader.Instance.masterVol = masterVolSlider.value;
        LevelLoader.Instance.soundtrackSource.volume = LevelLoader.Instance.musicVol * LevelLoader.Instance.masterVol;
    }
    public void ChangeMusicVol()
    {
        LevelLoader.Instance.musicVol = musicVolSlider.value;
        LevelLoader.Instance.soundtrackSource.volume = LevelLoader.Instance.musicVol * LevelLoader.Instance.masterVol;
    }
    public void ChangeSfxVol()
    {
        LevelLoader.Instance.sfxVol = sfxVolSlider.value; 
    }
}
