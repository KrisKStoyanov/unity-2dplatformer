using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public static LevelLoader Instance;

    private float activeScene;

    public GameObject loadingScreen;
    public Image loadingBar;
    public Image loadingCog;
    public Canvas loadingUI;
    //public Text loadingText;

    //public Text progressText;
    //public GameObject mainTitle;
    //public GameObject subTitle;

    private bool loading = false;

    //Audio
    public AudioSource soundtrackSource;

    public AudioClip deathSound;

    public AudioClip mainSoundtrack;
    public AudioClip lvOneSoundtrack;
    public AudioClip lvOneSoundtrackSR;
    //public AudioClip lvTwoSoundtrack;
    //public AudioClip lvTwoSoundtrackSR;

    private bool replaySoundtrack;
    private float vollowRange = .5f;
    private float volHighRange = 1.0f;

    private float lowPitchRange = .75f;
    private float highPitchRange = 1.5f;
    private float velToVol = .2f;
    private float velocityClipSplit = 10f;

    public float masterVol;
    public float musicVol;
    public float sfxVol;

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
        activeScene = SceneManager.GetActiveScene().buildIndex;

    }
    private void Start()
    {
        soundtrackSource = GetComponent<AudioSource>();
        if (GameMaster.Instance.mutedSound == false)
        {
            soundtrackSource.PlayOneShot(mainSoundtrack, musicVol * masterVol);
        }
    }
    public void PlayDeathSound()
    {
        if (GameMaster.Instance.mutedSound == false)
        {
            //Player is disabled so sound is not played (NEEDS FIX)
            AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        //loading = true;
        StartCoroutine(LoadAsynchronously(sceneIndex)); 
    }
    
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        activeScene = sceneIndex;
        if(sceneIndex == 0 && GameMaster.Instance.mutedSound == false)
        {
            soundtrackSource.Stop();
            soundtrackSource.PlayOneShot(mainSoundtrack, musicVol * masterVol);
        }
        if(sceneIndex == 1 && GameMaster.Instance.speedrunning == false && GameMaster.Instance.mutedSound == false)
        {
            soundtrackSource.Stop();
            soundtrackSource.PlayOneShot(lvOneSoundtrack, musicVol * masterVol);
        }
        if(sceneIndex == 1 && GameMaster.Instance.speedrunning == true && GameMaster.Instance.mutedSound == false)
        {
            soundtrackSource.Stop();
            soundtrackSource.PlayOneShot(lvOneSoundtrackSR, musicVol * masterVol);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            loadingUI.gameObject.SetActive(true);
            loadingCog.gameObject.SetActive(true);
            loadingBar.gameObject.SetActive(true);
            
            //mainTitle.SetActive(false);
            //subTitle.SetActive(false);
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.fillAmount = progress;

            yield return null;
        }
        if (operation.isDone)
        {
            loadingUI.gameObject.SetActive(false);
        }
    } 

}
