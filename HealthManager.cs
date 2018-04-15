using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour {

    public static HealthManager Instance { get; set; }
    public GameObject thePlayer;
    public GameObject tapToBeginSign;

    private bool willTapToBegin = false;

    public float maxHealth = 100f;
    public float currentHealth;

    public GameObject respawnPoint;
    private float playerSRtransp;

    public ParticleSystem flashCapacitor;

    //Coroutine on reaching 0 health
    private IEnumerator timeBeforeReset;

    private IEnumerator restartCoroutine;
    

    [Header("Unity UI")]
    public Image healthBar;
    public Text healthText;
    //public Image healthUI;
    public Image restartLevel;

    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        restartCoroutine = WaitAndRestart(10f);
    }
    public void TakeDamage(float amount)
    {
        //ShowHealthBar();
        
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / maxHealth;
        StartCoroutine(Flashing());

        if (CraftManager.Instance.capacitors >= 1 && currentHealth > 0f)
        {
            CraftManager.Instance.activCapBtn.gameObject.SetActive(true);
        }
        else if(CraftManager.Instance.capacitors <= 0)
        {
            CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
        }

        if (currentHealth <= 0)
        {
            LevelLoader.Instance.PlayDeathSound();
            thePlayer.gameObject.SetActive(false);
            restartLevel.gameObject.SetActive(true);
            healthText.text = "";
            CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
            //StartCoroutine(restartCoroutine);
        }
        else if(currentHealth >= 1)
        {
            thePlayer.gameObject.SetActive(true);
            restartLevel.gameObject.SetActive(false);
            healthText.text = "" + currentHealth;
        }
    }

    public void TakeSomeDamage()
    {
        //ShowHealthBar();

        currentHealth -= 1;
        healthBar.fillAmount = currentHealth / maxHealth;
        StartCoroutine(Flashing());

        if (CraftManager.Instance.capacitors >= 1 && currentHealth > 0f)
        {
            CraftManager.Instance.activCapBtn.gameObject.SetActive(true);
        }
        else if (CraftManager.Instance.capacitors <= 0)
        {
            CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
        }

        if (currentHealth <= 0)
        {
            thePlayer.gameObject.SetActive(false);
            restartLevel.gameObject.SetActive(true);
            healthText.text = "";
            CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
            //StartCoroutine(restartCoroutine);
        }
        else if (currentHealth >= 1)
        {
            thePlayer.gameObject.SetActive(true);
            restartLevel.gameObject.SetActive(false);
            healthText.text = "" + currentHealth;
        }
    }

    public void TakeContDamage()
    {
        //ShowHealthBar();

        InvokeRepeating("TakeSomeDamage",0.5f,1f);
        healthBar.fillAmount = currentHealth / maxHealth;
        StartCoroutine(Flashing());

        if (CraftManager.Instance.capacitors >= 1 && currentHealth > 0f)
        {
            CraftManager.Instance.activCapBtn.gameObject.SetActive(true);
        }
        else if (CraftManager.Instance.capacitors <= 0)
        {
            CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
        }

        if (currentHealth <= 0)
        {
            thePlayer.gameObject.SetActive(false);
            restartLevel.gameObject.SetActive(true);
            healthText.text = "";
            CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
            //StartCoroutine(restartCoroutine);
        }
        else if (currentHealth >= 1)
        {
            thePlayer.gameObject.SetActive(true);
            restartLevel.gameObject.SetActive(false);
            healthText.text = "" + currentHealth;
        }

    }
    public void StopContDamage()
    {
        CancelInvoke("TakeSomeDamage");
        StopCoroutine(Flashing());
    }

    public void RestartGame()
    {
        GameMaster.Instance.deathCounter += 1;
        if (GameMaster.Instance.deathCounter > 5)
        {
            GameMaster.Instance.deathCounter = 0;
        }
        GameMaster.Instance.Save();
        GameMaster.Instance.Load();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //willTapToBegin = true;
        Time.timeScale = 1f;
    }

    public void GiveHealth(float health)
    {
        if (currentHealth < maxHealth)
        {
            PlayerAudioController.Instance.PlayHealSound();
            currentHealth += health;
            flashCapacitor.Play();
            if (currentHealth + health > maxHealth)
            {
                currentHealth = maxHealth;
                CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
            }

            CraftManager.Instance.capacitors -= 1f;
            CraftManager.Instance.capacitorsText.text = CraftManager.Instance.capacitors + "";
            if (CraftManager.Instance.capacitors <= 0)
            {
                CraftManager.Instance.ultracapacitorsHUD.SetActive(false);
                CraftManager.Instance.activCapBtn.gameObject.SetActive(false);
                CraftManager.Instance.capacitors = 0;
            }

            healthBar.fillAmount = currentHealth / maxHealth;
            healthText.text = "" + currentHealth;
        }
    }
    public void DamageFlash()
    {
        PlayerPlatformerController.Instance.spriteRenderer.color = new Color(PlayerPlatformerController.Instance.spriteRenderer.color.r, PlayerPlatformerController.Instance.spriteRenderer.color.g, PlayerPlatformerController.Instance.spriteRenderer.color.b, 0.196f);
    }
    public void DamageShow()
    {
        PlayerPlatformerController.Instance.spriteRenderer.color = new Color(PlayerPlatformerController.Instance.spriteRenderer.color.r, PlayerPlatformerController.Instance.spriteRenderer.color.g, PlayerPlatformerController.Instance.spriteRenderer.color.b, 1f);
    }

    private IEnumerator WaitAndRestart(float waitTime)
    {
        while (true)
        {

            yield return new WaitForSeconds(4f);

            RestartGame();
        }
    }

    public void TapToBegin()
    {
        willTapToBegin = false;
        Time.timeScale = 1f;
        tapToBeginSign.gameObject.SetActive(false);
    }

    private IEnumerator ShowWorldspaceUI()
    {
        healthBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        healthBar.gameObject.SetActive(true);
    }

    private IEnumerator Flashing()
    {
        DamageFlash();
        yield return new WaitForSeconds(.3f);
        DamageShow();
        yield return new WaitForSeconds(.3f);
        DamageFlash();
        yield return new WaitForSeconds(.3f);
        DamageShow();

    }

    public void ShowUpdatedHealth()
    {
        StartCoroutine(ShowWorldspaceUI());
    }
}
