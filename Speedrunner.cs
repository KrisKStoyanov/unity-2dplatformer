using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Speedrunner : UnityEngine.MonoBehaviour
{
    public static Speedrunner Instance;

    public GameObject speedrunUI;
    public GameObject bestTimeImage;

    //public Animator speedUIanim;
    //public Animator backToMenuAnim;

    public bool timerOn;
    public bool resetTimer;

    public Text timeText;
    public Text achievedTimeText;
    public Text bestTimeText;

    public float gameTime;
    public float timePerSecond = 1f;
    public float achievedTime;
    public float bestTime;

    private void Awake()
    {
        Instance = this;
        GameMaster.Instance.Load();
        //speedUIanim = GetComponentInChildren<Animator>();
        //speedUIanim = speedrunHolder.gameObject.GetComponentInChildren<Animator>();
        //backToMenuAnim = backToMainBtn.gameObject.GetComponent<Animator>();

        bestTime = GameMaster.Instance.speedrunBestTime;

        if (GameMaster.Instance.speedrunning == true)
        {
            ActivateSpeedrunUI();
        }

        if(GameMaster.Instance.speedrunning == false)
        {
            speedrunUI.SetActive(false);
        }
    }
    private void ActivateSpeedrunUI()
    {
        timerOn = true;
        speedrunUI.SetActive(true); 
        //speedUIanim.SetTrigger("Activate");
        timeText.text = ("" + gameTime);

        bestTimeText.text = ("Best Time: " + GameMaster.Instance.speedrunBestTime);
        if(GameMaster.Instance.speedrunBestTime == 0)
        {
            bestTimeText.text = ("Best Time: ");
        }
    }

    private void Update()
    {
        
        if (GameMaster.Instance.speedrunning == true && HealthManager.Instance.currentHealth <= 0)
        {
            timerOn = false;
            resetTimer = true;
        }
        if(resetTimer == true)
        {
            gameTime = 0 + Time.deltaTime;
            resetTimer = false;
        }

        if(timerOn == true)
        {
            gameTime = gameTime + Time.deltaTime;
            timeText.text = ("" + gameTime);
            achievedTimeText.gameObject.SetActive(false);
            bestTimeText.gameObject.SetActive(false);
        }


        if(timerOn == false && HealthManager.Instance.currentHealth > 0 && GameMaster.Instance.speedrunning == true)
        {
            //speedUIanim.SetTrigger("Expand");
            timeText.gameObject.SetActive(false);
            bestTimeImage.gameObject.SetActive(true);
            timeText.text = "Level Complete:";
            achievedTime = gameTime;
            achievedTimeText.gameObject.SetActive(true);
            bestTimeText.gameObject.SetActive(true);
            achievedTimeText.text = "Achieved Time: " + achievedTime;
            bestTimeText.text = "Best Time: " + GameMaster.Instance.speedrunBestTime;
            //backToMainBtn.interactable = true;
            //backToMenuAnim.SetTrigger("Activate");
            if (achievedTimeText.color.a < 1f && bestTimeText.color.a < 1f)
            {
                achievedTimeText.color = new Color(achievedTimeText.color.r, achievedTimeText.color.g, achievedTimeText.color.b, achievedTimeText.color.a) + new Color(0, 0, 0, 0.01f);
                bestTimeText.color = new Color(bestTimeText.color.r, bestTimeText.color.g, bestTimeText.color.b, bestTimeText.color.a) + new Color(0, 0, 0, 0.01f);
            }
            //achievedTimeText.color = new Color(achievedTimeText.color.r, achievedTimeText.color.g, achievedTimeText.color.b, achievedTimeText.color.a) + new Color(0, 0, 0, 0.0025f);
            //bestTimeText.color = new Color(bestTimeText.color.r, bestTimeText.color.g, bestTimeText.color.b, bestTimeText.color.a) + new Color(0, 0, 0, 0.0025f);
            if (achievedTime < bestTime || GameMaster.Instance.speedrunBestTime == 0)
            {
                bestTimeText.text = "Best Time: " + achievedTime;
                GameMaster.Instance.speedrunBestTime = achievedTime;
                GameMaster.Instance.Save();
                GameMaster.Instance.Load();
            }
        }
        /*if(backToMainBtn.interactable == true)
        {
            backToMainText.color = new Color(backToMainText.color.r, backToMainText.color.g, backToMainText.color.b, backToMainText.color.a) + new Color(0, 0, 0, 0.075f);
        }
        else if(backToMainBtn.interactable == false)
        {
            backToMainText.color = new Color(backToMainText.color.r, backToMainText.color.g, backToMainText.color.b, backToMainText.color.a) - new Color(0, 0, 0, 0.075f);
        }

        else if(Time.timeScale == 1 && backToMainBtn.interactable == false)
        {
            if (Speedrunner.Instance.backToMainText.color.a < 1f)
            {
                Speedrunner.Instance.backToMainText.color = new Color(Speedrunner.Instance.backToMainText.color.r, Speedrunner.Instance.backToMainText.color.g, Speedrunner.Instance.backToMainText.color.b, Speedrunner.Instance.backToMainText.color.a) - new Color(0, 0, 0, 0.0025f);
            }
        }*/
    }
    public void LoadMainMenu()
    {
        LevelLoader.Instance.LoadLevel(0);
        Time.timeScale = 1f;
    }
    public void FadeIn()
    {
        achievedTimeText.color = new Color(achievedTimeText.color.r, achievedTimeText.color.g, achievedTimeText.color.b, achievedTimeText.color.a) + new Color(0, 0, 0, 0.01f);
        bestTimeText.color = new Color(bestTimeText.color.r, bestTimeText.color.g, bestTimeText.color.b, bestTimeText.color.a) + new Color(0, 0, 0, 0.01f);
    }

}