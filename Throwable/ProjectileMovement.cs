using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {

    //public static ProjectileMovement Instance;

    //private Rigidbody2D projectileRB2D;
    public SpriteRenderer playerSprite;
    private SpriteRenderer bladeSpriteRenderer;

    public float throwSpeed = .1f;
    public GameObject target; 

    private ParticleSystem spinSmoke;
    private ParticleSystem.EmissionModule smokeEmissionModule;

    public ParticleSystem targetSignal;

    public ParticleSystem[] dreadbladeEffects;

    private void Awake()
    {
        //Instance = this;
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        bladeSpriteRenderer = GetComponent<SpriteRenderer>();
        dreadbladeEffects = GetComponentsInChildren<ParticleSystem>();
        spinSmoke = dreadbladeEffects[0];
        targetSignal = dreadbladeEffects[1];
        //spinSmoke = GetComponentInChildren<ParticleSystem>();
        //targetSignal = GetComponentInChildren<ParticleSystem>();
        smokeEmissionModule = spinSmoke.emission;
    }
    private void Start()
    {
        ThrowProjectile();
    }
    void OnEnable () {
        //projectileRB2D = GetComponent<Rigidbody2D>();
    }

    public void ThrowProjectile()
    {
        if (playerSprite.flipX == false)
        {
            bladeSpriteRenderer.flipX = true;
            StartCoroutine(SpeedUpRight());
            spinSmoke.transform.eulerAngles = new Vector3(-9, -90, 90);
        }
        else if (playerSprite.flipX == true)
        {
            bladeSpriteRenderer.flipX = false;
            StartCoroutine(SpeedUpLeft());
            spinSmoke.transform.eulerAngles = new Vector3(-151, -90, 90);
        }
        //StartCoroutine(SpeedUpProjectile());
    }

    /*public IEnumerator SpeedUpProjectile()
    {
        while (true)
        {
            Rigidbody2D thisRB2D = GetComponent<Rigidbody2D>();
            if (playerSprite.flipX == false && target == null)
            {
                thisRB2D.AddForce(new Vector2(thisRB2D.position.x * throwSpeed, thisRB2D.velocity.y));
            }
            else if(playerSprite.flipX == true && target == null)
            {
                thisRB2D.AddForce(new Vector2(thisRB2D.position.x * -throwSpeed, thisRB2D.velocity.y));
            }
            if(target != null)
            {
                StopCoroutine(SpeedUpProjectile());
            }
            spinSmoke = GetComponentInChildren<ParticleSystem>();
            smokeEmissionModule = spinSmoke.emission;
            yield return null;
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && EnemyController.instance.state == EnemyController.State.PATROL || collision.gameObject.tag == "Enemy" && EnemyController.instance.state == EnemyController.State.CHASE)
        {
            PlayerAudioController.Instance.PlayBladeTrackingSound();
            target = collision.gameObject;

            //target = collision.gameObject;
            
            //StartCoroutine(TrackTarget());
        }
        else
        {
            StopCoroutine(TrackTarget());
        }
        /*else if(collision.gameObject.tag != "Enemy")
        {
            if(bladeSpriteRenderer.flipX == true)
            {
                target = null;
                StopCoroutine(TrackTarget());
                StartCoroutine(SpeedUpRight());
            }
            else if(bladeSpriteRenderer.flipX == false)
            {
                target = null;
                StopCoroutine(TrackTarget());
                StartCoroutine(SpeedUpLeft());
            }

        }*/
    }
    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (bladeSpriteRenderer.flipX == true)
            {
                StopCoroutine(TrackTarget());
                //StartCoroutine(SpeedUpLeft());
            }
            //StopCoroutine(TrackTarget());
            else if (bladeSpriteRenderer.flipX == false)
            {
                StopCoroutine(TrackTarget());
                //StartCoroutine(SpeedUpRight());
            }
        }
    }*/
    private IEnumerator SpeedUpLeft()
    {
        while (true)
        {
            Rigidbody2D thisRB2D = GetComponent<Rigidbody2D>();
            if(target == null)
            {
                thisRB2D.AddForce(new Vector2(thisRB2D.position.x * -throwSpeed, thisRB2D.velocity.y));
            }

            if (target != null)
            {
                StartCoroutine(TrackTarget());
                StopCoroutine(SpeedUpLeft());
            }
            yield return null;
        }
    }

    private IEnumerator SpeedUpRight()
    {
        while (true)
        {
            Rigidbody2D thisRB2D = GetComponent<Rigidbody2D>();
            if (target == null)
            {
                thisRB2D.AddForce(new Vector2(thisRB2D.position.x * throwSpeed, thisRB2D.velocity.y));
            }

            if (target != null)
            {
                StartCoroutine(TrackTarget());
                StopCoroutine(SpeedUpRight());
            }

            yield return null;
        }
    }

    private IEnumerator TrackTarget()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * throwSpeed * 15f);
            smokeEmissionModule.rateOverTime = 20f;
            dreadbladeEffects[1].Play();
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            target = null;
        }
    }
    private void OnDisable()
    {
        target = null;
        StopAllCoroutines();
    }
}
