using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public static PlayerPlatformerController Instance;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    public SpriteRenderer spriteRenderer;
    private Animator animator;

    static readonly int anim_Grounded = Animator.StringToHash("grounded");
    static readonly int anim_VelocityX = Animator.StringToHash("velocityX");
    static readonly int anim_VelocityY = Animator.StringToHash("velocityY");

    [Header("Throwing Mechanism")]
    private int currentThrowables;
    private int maxThrowables = 10;

    [Header("DustParticle")]
    public ParticleSystem dustParticle;
    private bool facingRight;
    private Quaternion dustRotation;
    public ParticleSystem airKnock;

    [Header("FlyingParticle")]
    public ParticleSystem flyingParticle;
    public bool flying;
    public bool boostActive;

    // Use this for initialization
    void Awake () {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        //Enable or disable the next 2 lines to make Touch Controls compatible
        //TouchControls should be disabled when these are active

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");
#endif
        if (knockbackCount > 0)
        {
            if (knockFromRight)
            {
                //move = new Vector2(0, 0);
                //rb2d.velocity = new Vector2(-knockback, knockback);
                move = new Vector2(-knockback, knockback);
            }
            if (!knockFromRight)
            {
                //move = new Vector2(0, 0);
                //rb2d.velocity = new Vector2(knockback, knockback);
                move = new Vector2(knockback, knockback);
            }
            knockbackCount -= Time.deltaTime;

        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;

        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }

        }
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetButton("Jump") && grounded == false && CraftManager.Instance.fuelLoaded > 0 && velocity.y < 0)
        {
            flying = true;
            boostActive = true;
            velocity.y += 0.65f;
            CraftManager.Instance.fuelLoaded -= .5f;
            CraftManager.Instance.fuelTank.fillAmount = CraftManager.Instance.fuelLoaded / CraftManager.Instance.maxFuel;
            if (CraftManager.Instance.fuelLoaded <= 0)
            {
                CraftManager.Instance.fuelLoaded = 0f;
                CraftManager.Instance.fuelUI.gameObject.SetActive(false);
                CraftManager.Instance.jetpackSprite.gameObject.SetActive(true);
            }

        }
        if (Input.GetButtonUp("Jump") && grounded == false && CraftManager.Instance.fuelLoaded > 0 && velocity.y < 0)
        {
            flying = false;
        }

        if (Input.GetButtonDown("Jump") && grounded == false && CraftManager.Instance.fuelLoaded > 0 && velocity.y < 0 && boostActive == true && !airKnock.isPlaying)
        {
            if (CraftManager.Instance.fuelLoaded >= 20f)
            {
                velocity.y += 30f;
                CraftManager.Instance.fuelLoaded -= 20f;
                CraftManager.Instance.fuelTank.fillAmount = CraftManager.Instance.fuelLoaded / CraftManager.Instance.maxFuel;
                if (CraftManager.Instance.fuelLoaded <= 0)
                {
                    CraftManager.Instance.fuelLoaded = 0f;
                    CraftManager.Instance.fuelUI.gameObject.SetActive(true);
                    CraftManager.Instance.jetpackSprite.gameObject.SetActive(true);
                }
            }
        }
        if (Input.GetButtonDown("Throw") && CraftManager.Instance.throwables >= 1)
        {
            ThrowProjectile.throwControl.Throw();
            CraftManager.Instance.throwables -= 1f;
            CraftManager.Instance.throwablesText.text = CraftManager.Instance.throwables + "";
            if (CraftManager.Instance.throwables <= 0)
            {
                //CraftManager.Instance.extensionHUD.SetActive(false);
                CraftManager.Instance.throwablesHUD.gameObject.SetActive(false);
                CraftManager.Instance.throwTCbuttton.gameObject.SetActive(false);
            }
        }
        if (Input.GetButtonDown("ActivateCapacitor") && CraftManager.Instance.capacitors >= 1)
        {
            HealthManager.Instance.GiveHealth(35);
        }

#endif

        /*if (grounded == false)
        {
            if (spriteRenderer.flipX == true)
            {
                CraftManager.Instance.jetpackSprite.transform.position = new Vector3(1, -1.1f, 0);
            }
            else if (spriteRenderer.flipX == false)
            {
                CraftManager.Instance.jetpackSprite.transform.position = new Vector3(-1, -1.1f, 0);
            }
        }*/

        if (grounded == true || CraftManager.Instance.fuelLoaded <= 0)
        {
            CraftManager.Instance.jetpackSprite.gameObject.SetActive(false);
            flying = false;
            boostActive = false;
        }

        if (flying == true)
        {
            flyingParticle.Emit(1);
        }
        else if (flying == false)
        {
            flyingParticle.Emit(0);
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;

        }
        animator.SetBool(anim_Grounded, grounded);
        animator.SetFloat(anim_VelocityX, Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat(anim_VelocityY, velocity.y);
        //animator.SetFloat("health", HealthManager.Instance.currentHealth);

        targetVelocity = move * maxSpeed;

        if ((move.x > 0.5) && grounded && !dustParticle.isPlaying || (move.x < -0.5) && grounded && !dustParticle.isPlaying)
        {
            dustParticle.Play();
        }

        if (move.x == 0 || !grounded)
        {
            dustParticle.Stop();
        }
        if (velocity.y > 0.01f && !airKnock.isPlaying && grounded)
        {
            airKnock.Play();
        }
        if (grounded && velocity.y == 0)
        {
            airKnock.Stop();
        }

        if (spriteRenderer.flipX == true)
        {
            CraftManager.Instance.jetpackSprite.transform.localPosition = new Vector3(1f, 0.5f, 0f);
            if (grounded == true)
            {
                dustParticle.transform.eulerAngles = new Vector3(-151, -90, 90);
            }
        }
        else if (spriteRenderer.flipX == false)
        {
            CraftManager.Instance.jetpackSprite.transform.localPosition = new Vector3(-1f, 0.5f, 0f);
            if (grounded == true)
            {
                dustParticle.transform.eulerAngles = new Vector3(-9, -90, 90);
            }
        }
    }

           
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            gameObject.transform.parent = collision.transform;
        }
        if(collision.gameObject.tag == "Enemy")
        {
            PlayerAudioController.Instance.PlayHitSound();
            HealthManager.Instance.TakeDamage(10);
            print("damaged");
        }
        if(collision.gameObject.tag == "Crusher" && grounded == true)
        {
            HealthManager.Instance.TakeDamage(100f);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            gameObject.transform.parent = null;
        }
        /*if(collision.gameObject.tag == "Crusher")
        {
            HealthManager.Instance.StopContDamage();
        }*/
    }

    //Gives multiple value because TouchControls is attached to this controller's gameobject and is inheriting function & event
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PickUp")
        {
            CraftManager.Instance.GivePoints(12.5f);
            collision.gameObject.SetActive(false);
            PlayerAudioController.Instance.PlayPickUpSound();
        }

        if(collision.gameObject.tag == "CraftingHub" && move.x == 0)
        {
            CraftManager.Instance.craftingInterface.transform.position = collision.transform.position;
            CraftManager.Instance.Craft();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "CraftingHub")
        {
            CraftManager.Instance.StopCraft();
        }
    }
}
