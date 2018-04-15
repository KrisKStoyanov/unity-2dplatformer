using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    private float enemyMaxHealth = 100f;
    public float enemyCurrentHealth;

    private SpriteRenderer enemySR;
    private float enemySRred = 1f;

    [Header("Unity UI")]
    public Image enemyHealthBar;
    public Image enemyHealthUI;

    // Use this for initialization
    private void OnEnable()
    {
        enemyCurrentHealth = enemyMaxHealth;
        enemySR = GetComponent<SpriteRenderer>();
        enemySRred = enemySR.color.r;
    } 


    public void EnemyTakeDamage(float amount)
    {
        StartCoroutine(ShowWorldspaceUI());
        StartCoroutine(Flashing());

        enemyCurrentHealth -= amount;
        enemyHealthBar.fillAmount = enemyCurrentHealth / enemyMaxHealth;

        if(enemyCurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void EnemyDamageFlash()
    {
        enemySR.color = new Color(enemySR.color.r, enemySR.color.g, enemySR.color.b, 0.1f);
    }
    public void EnemyDamageShow()
    {
        enemySR.color = new Color(enemySR.color.r, enemySR.color.g, enemySR.color.b);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            EnemyTakeDamage(25f);
            enemyHealthBar.fillAmount = enemyCurrentHealth / enemyMaxHealth;
        }
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<TouchControls>();
            player.knockbackCount = player.knockbackLength;

            if (collision.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
        }

    }

    private IEnumerator ShowWorldspaceUI()
    {
        enemyHealthUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        enemyHealthUI.gameObject.SetActive(false);
    }

    private IEnumerator Flashing()
    {
        EnemyDamageFlash();
        yield return new WaitForSeconds(0.3f);
        EnemyDamageShow();
        yield return new WaitForSeconds(0.3f);
        EnemyDamageFlash();
        yield return new WaitForSeconds(0.3f);
        EnemyDamageShow();
    }
}
