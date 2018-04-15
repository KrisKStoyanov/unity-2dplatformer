using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : PlayerPlatformerController {
    //Disable the first 2 lines of ComputeVelocity in PPC to enable 
    //Deactivate the PPC script component in the Player GameObject to avoid code overlapping
    public float mobileSpeed;
    public Image resume;
    public Image pause;
    public Image pauseImage;
    public Image tapToBegin;

    public Button backToMenu;

    public Canvas touchControls;

    private bool beginTap = true;

    //Overwrites the requirement for any inherited object of a PhysicsObject script to contain a Rigidbody2D and instead takes the Player's rb2d
    /*private void OnEnable()
    {
        ResetRigidbody2D();
    }

    void ResetRigidbody2D()
    {
        rb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }*/

    public void TouchMove(float directionHandle)
    {
        move.x = mobileSpeed * directionHandle;
    }

    public void TouchJump()
    {
        if (grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
    }
    public void TouchJumpUp()
    {
        if (!grounded)
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }
    }
    public void TouchJumpFly()
    {
        if (grounded == false && CraftManager.Instance.fuelLoaded > 0 && velocity.y < 0)
        {
            flying = true;
            boostActive = true;
            velocity.y += 2.65f;
            CraftManager.Instance.fuelLoaded -= 2f;
            CraftManager.Instance.fuelTank.fillAmount = CraftManager.Instance.fuelLoaded / CraftManager.Instance.maxFuel;
            if (CraftManager.Instance.fuelLoaded <= 0)
            {
                CraftManager.Instance.fuelLoaded = 0f;
                CraftManager.Instance.fuelUI.gameObject.SetActive(false);
                //CraftManager.Instance.jetpackSprite.gameObject.SetActive(true);
            }
        }
    }
    public void TouchJumpFlyBoost()
    {
        if (grounded == false && CraftManager.Instance.fuelLoaded > 0 && velocity.y < 0)
        {
            PlayerAudioController.Instance.PlayFlyBoostSound();
            velocity.y += 26f;
            CraftManager.Instance.fuelLoaded -= 10f;
            CraftManager.Instance.fuelTank.fillAmount = CraftManager.Instance.fuelLoaded / CraftManager.Instance.maxFuel;
            CraftManager.Instance.jetpackSprite.gameObject.SetActive(true);
            flyingParticle.Play();
            if (CraftManager.Instance.fuelLoaded <= 0)
            {
                CraftManager.Instance.fuelLoaded = 0f;
                CraftManager.Instance.fuelUI.gameObject.SetActive(false);
                //CraftManager.Instance.jetpackSprite.gameObject.SetActive(true);
            }
        }
    }
    public void TouchLeft()
    {
        TouchMove(-1);
        
    }
    public void TouchRight()
    {
        TouchMove(1);
    }
    public void UnpressedArrow()
    {
        TouchMove(0);
    }
    public void Pause()
    {
        backToMenu.gameObject.SetActive(true);
        //Speedrunner.Instance.backToMainBtn.interactable = true;
        Time.timeScale = 0f;
        resume.gameObject.SetActive(true);
        pause.gameObject.SetActive(false);
        pauseImage.gameObject.SetActive(true);
        HealthManager.Instance.restartLevel.gameObject.SetActive(true);
        HealthManager.Instance.healthText.text = "";
        //Speedrunner.Instance.backToMainBtn.gameObject.SetActive(true);
        //Speedrunner.Instance.backToMenuAnim.SetTrigger("Activate");

    }
    public void Unpause()
    {
        if (Speedrunner.Instance.timerOn == true)
        {
            backToMenu.gameObject.SetActive(false);
            //Speedrunner.Instance.backToMainBtn.interactable = false;
            //Speedrunner.Instance.backToMenuAnim.SetTrigger("Deactivate");
            Time.timeScale = 1f;
            resume.gameObject.SetActive(false);
            pause.gameObject.SetActive(true);
            pauseImage.gameObject.SetActive(false);
            HealthManager.Instance.restartLevel.gameObject.SetActive(false);
            HealthManager.Instance.healthText.text = "" + HealthManager.Instance.currentHealth;
        }
        if(Speedrunner.Instance.timerOn == false && GameMaster.Instance.speedrunning == true)
        {
            Time.timeScale = 1f;
            resume.gameObject.SetActive(false);
            pause.gameObject.SetActive(true);
            pauseImage.gameObject.SetActive(false);
            HealthManager.Instance.restartLevel.gameObject.SetActive(false);
            HealthManager.Instance.healthText.text = "" + HealthManager.Instance.currentHealth;
        }
        else if(GameMaster.Instance.speedrunning == false)
        {
            backToMenu.gameObject.SetActive(false);
            //Speedrunner.Instance.backToMainBtn.interactable = false;
            //Speedrunner.Instance.backToMenuAnim.SetTrigger("Deactivate");
            Time.timeScale = 1f;
            resume.gameObject.SetActive(false);
            pause.gameObject.SetActive(true);
            pauseImage.gameObject.SetActive(false);
            HealthManager.Instance.restartLevel.gameObject.SetActive(false);
            HealthManager.Instance.healthText.text = "" + HealthManager.Instance.currentHealth;
        }
        /*Time.timeScale = 1f;
        resume.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        pauseImage.gameObject.SetActive(false);
        HealthManager.Instance.restartLevel.gameObject.SetActive(false);
        HealthManager.Instance.healthText.text = "" + HealthManager.Instance.currentHealth;*/
    }
    void TapToBegin()
    {
        beginTap = false;
        touchControls.gameObject.SetActive(true);
    }
    public void TouchThrow()
    {
        ThrowProjectile.throwControl.Throw();
        //ProjectileMovement.Instance.ThrowProjectile();
        PlayerAudioController.Instance.PlayThrowSound();
        CraftManager.Instance.throwables -= 1f;
        CraftManager.Instance.throwablesText.text = CraftManager.Instance.throwables + "";
        if (CraftManager.Instance.throwables <= 0)
        {
            CraftManager.Instance.throwables = 0;
            //CraftManager.Instance.extensionHUD.SetActive(false);
            CraftManager.Instance.throwablesHUD.gameObject.SetActive(false);
            CraftManager.Instance.throwTCbuttton.gameObject.SetActive(false);
        }

    }

    public void TouchGrapple()
    {
        GrappleProjectile.Instance.Grapple();
    }
}
