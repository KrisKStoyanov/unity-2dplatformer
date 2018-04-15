using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour {

    public static EnemyController instance;

    public ParticleSystem hornSmoke;
    public Transform smokeStartPosition;
    public Transform smokeFlipXposition;

    private Animator animator;
    static readonly int anim_Patrol = Animator.StringToHash("patrolling");
    static readonly int anim_Chase = Animator.StringToHash("chasing");

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    private float jumpWait;
    private float jumpSet;

    private float switchWait;
    private float switchSet;

    public GameObject targetPlayer;
    private Transform originTransform;

    private bool hasJumped = false;
    private bool hasSwitched = false;

    private bool stopJumper;

    private GameObject enemyObject;
    public BoxCollider2D[] colliders;

    private CircleCollider2D defaultTriggerZone;
    //private BoxCollider2D defaultTriggerZone;
    private float triggerZoneOffsetX;
    private float triggerZoneSizeX;
    private float triggerZoneSizeY;

    /*private BoxCollider2D vulnerableZone;
    private float vulnerableZoneOffsetX;
    private float vulnerableZoneSizeX;
    private float vulnerableZoneSizeY;*/

    //private BoxCollider2D idleTriggerZone;
    //private BoxCollider2D patrolTriggerZone;
    //private BoxCollider2D chaseTriggerZone;

    private BoxCollider2D mainBodyCollider;
    private BoxCollider2D balanceCheck;
    private BoxCollider2D groundCheck;
    private BoxCollider2D distToGroundMeasure;

    private Collider2D detectedVertCollider;
    private Collider2D detectedHorCollider;
    private Collider2D detectedLowerGround;
    //private BoxCollider2D dynamicMoveCtrl;

    private Collider2D balanceHolder;

    /*private Collider2D[] effectiveColliders;
    private Collision2D[] effectiveCollisions;
    private ContactPoint2D[] contactPoints;*/

    //private bool standingOnTile = false;

    public enum State
    {
        IDLE,
        PATROL,
        CHASE
    }

    public State state;
    public bool alive;

    public bool switchDirection;
    private bool grounded;

    // Idle data
    private float idleTime;

    // Patrol data
    private bool patrolling;
    private float patrolTime;


    // Chase data
    private bool chasing;
    private float chaseTime;

    private void Awake()
    {
        instance = this;
    }

    void Start () {

        enemyObject = this.gameObject;
        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        colliders = GetComponents<BoxCollider2D>();

        defaultTriggerZone = GetComponent<CircleCollider2D>();

        mainBodyCollider = colliders[0];

        balanceCheck = colliders[1];
        groundCheck = colliders[2];
        distToGroundMeasure = colliders[3];
        triggerZoneOffsetX = defaultTriggerZone.offset.x;

        state = State.IDLE;
        alive = true;
        grounded = true;

        idleTime = 4f;
        patrolTime = 10f;
        chaseTime = 20f;

        jumpWait = 1.5f;
        jumpSet = 1.5f;

        switchWait = 1.5f;
        switchSet = 1.5f;

        //Initialize Finite State Machine
        StartCoroutine("FSM");
		
	}

    IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.IDLE:
                    Idle();
                    break;
                case State.PATROL:
                    Patrol();
                    break;
                case State.CHASE:
                    Chase();
                    break;
            }


            yield return null;
        }

    }
    void Idle()
    {
        patrolling = false;
        chasing = false;
        animator.SetBool(anim_Patrol, patrolling);
        animator.SetBool(anim_Chase, chasing);
        patrolTime = 10;
        chaseTime = 20;

        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            rb2d.velocity = new Vector2(0, 0);
            state = EnemyController.State.PATROL;
        }

        if (stopJumper == true)
        {
            StopCoroutine(Jumper());
            stopJumper = false;
        }

        if (hasJumped == true)
        {
            jumpWait -= Time.deltaTime;
        }
        if (jumpWait <= 0f)
        {
            jumpWait = jumpSet;
            hasJumped = false;
            switchDirection = !switchDirection;
        }

        if (spriteRenderer.flipX == true)
        {
            triggerZoneOffsetX = 4f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);

            //vulnerableZoneOffsetX = -4.5f;
            //vulnerableZone.offset = new Vector2(vulnerableZoneOffsetX, vulnerableZone.offset.y);
        }
        else if (spriteRenderer.flipX == false)
        {
            triggerZoneOffsetX = -5f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);
            
            //vulnerableZoneOffsetX = 4f;
            //vulnerableZone.offset = new Vector2(vulnerableZoneOffsetX, vulnerableZone.offset.y);
        }

    }
    void Patrol()
    {
        patrolling = true;
        chasing = false;
        animator.SetBool(anim_Patrol, patrolling);
        animator.SetBool(anim_Chase, chasing);
        idleTime = 4;
        chaseTime = 20;

        patrolTime -= Time.deltaTime;
        if (patrolTime < 0 && grounded == true && rb2d.velocity.y == 0)
        {
            rb2d.velocity = new Vector2(0, 0);
            state = EnemyController.State.IDLE;
        }

        if (stopJumper == true)
        {
            StopCoroutine(Jumper());
            stopJumper = false;
        }

        if (hasJumped == true)
        {
            jumpWait -= Time.deltaTime;
        }
        if (jumpWait <= 0f)
        {
            jumpWait = jumpSet;
            hasJumped = false;
            stopJumper = false;
            //switchDirection = !switchDirection;
        }

        if (hasSwitched == true)
        {
            switchWait -= Time.deltaTime;
        }
        if (switchWait <= 0f)
        {
            switchWait = switchSet;
            hasSwitched = false;
        }

        /*if(rb2d.velocity.y <= 0)
        {
            jumpStackPrevent = false;

        }
        else if(rb2d.velocity.y > 0)
        {
            jumpStackPrevent = true;
        }*/

        if (balanceCheck.IsTouching(detectedVertCollider) && hasSwitched == false)
        {
            switchDirection = !switchDirection;
            hasSwitched = true;
        }




        if (!groundCheck.IsTouching(detectedHorCollider) && hasJumped == false && hasSwitched == false)
        {
            switchDirection = !switchDirection;
            hasSwitched = true;
            StartCoroutine(Jumper());
        }

        /*if(!distToGroundMeasure.IsTouching(detectedLowerGround) && hasJumped == false)
        {
            switchDirection = !switchDirection;
            StartCoroutine(Jumper());
        }*/
        if (defaultTriggerZone.radius < 12.5f)
        {
            defaultTriggerZone.radius = 12.5f;
        }
        if (defaultTriggerZone.radius > 17.5)
        {
            defaultTriggerZone.radius = 17.5f;
        }

        if (switchDirection == false)
        {
            spriteRenderer.flipX = true;
            rb2d.AddForce(new Vector2(rb2d.velocity.x * Time.deltaTime + 25f, rb2d.velocity.y - 3f * Time.deltaTime));
            triggerZoneOffsetX = 3.5f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);

            hornSmoke.transform.position = smokeFlipXposition.transform.position;
        }
        else if(switchDirection == true)
        {
            spriteRenderer.flipX = false;
            triggerZoneOffsetX = -4f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);
            rb2d.AddForce(new Vector2(rb2d.velocity.x * Time.deltaTime - 25f, rb2d.velocity.y - 3f * Time.deltaTime));

            hornSmoke.transform.position = smokeStartPosition.transform.position;
        }
        /*if(distToGroundMeasure.IsTouching(detectedLowerGround) == false)
        {
            switchDirection = !switchDirection;
        }*/

        /*if (spriteRenderer.flipX == true)
        {
            triggerZoneOffsetX = 3.5f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);

            //vulnerableZoneOffsetX = -4.5f;
            //vulnerableZone.offset = new Vector2(vulnerableZoneOffsetX, vulnerableZone.offset.y);
        }
        else if (spriteRenderer.flipX == false)
        {
            triggerZoneOffsetX = -4f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);

            //vulnerableZoneOffsetX = 4f;
            //vulnerableZone.offset = new Vector2(vulnerableZoneOffsetX, vulnerableZone.offset.y);
        }
        */
    }
    void Chase()
    {
        chasing = true;
        patrolling = false;
        
        animator.SetBool(anim_Chase, chasing);
        animator.SetBool(anim_Patrol, patrolling);

        idleTime = 3;
        patrolTime = 10;

        chaseTime -= Time.deltaTime;

        if (chaseTime < 0)
        {
            rb2d.velocity = new Vector2(0, 0);
            state = EnemyController.State.PATROL;
        }

        if (groundCheck.IsTouching(detectedHorCollider) && hasJumped == true)
        {
            jumpWait -= Time.deltaTime;
        }
        if (jumpWait <= 0f)
        {
            jumpWait = jumpSet;
            hasJumped = false;
            stopJumper = false;
            //switchDirection = !switchDirection;
        }
        if(stopJumper == true)
        {
            StopCoroutine(Jumper());
            stopJumper = false;
        }

        if (hasSwitched == true)
        {
            switchWait -= Time.deltaTime;
        }
        if (switchWait <= 0f)
        {
            switchWait = switchSet;
            hasSwitched = false;
        }
        if (balanceCheck.IsTouching(detectedVertCollider) && hasJumped == false)
        {
            if (stopJumper == false)
            {
                StartCoroutine(Jumper());
                if (!balanceCheck.IsTouching(detectedVertCollider))
                {
                    StopCoroutine(Jumper());
                }
            }
        }
        if(defaultTriggerZone.radius < 12.5f)
        {
            defaultTriggerZone.radius = 12.5f;
        }
        if(defaultTriggerZone.radius > 17.5)
        {
            defaultTriggerZone.radius = 17.5f;
        }

        if (!balanceCheck.IsTouching(detectedVertCollider))
        {
            detectedVertCollider = null;
        }

        /*if (balanceCheck.IsTouching(detectedVertCollider) && hasJumped == true && hasSwitched == false)
        {
            StopCoroutine(Jumper());
            stopJumper = false;
            switchDirection = !switchDirection;
            hasSwitched = true;
            state = State.PATROL;
        }*/

        if (spriteRenderer.flipX == true)
        {
            triggerZoneOffsetX = 4f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);
        }
        else if (spriteRenderer.flipX == false)
        {
            triggerZoneOffsetX = -5f;
            defaultTriggerZone.offset = new Vector2(triggerZoneOffsetX, defaultTriggerZone.offset.y);
        }

        if (targetPlayer.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
            rb2d.AddForce(new Vector2(rb2d.velocity.x * Time.deltaTime + 30f, rb2d.velocity.y * -1));
            hornSmoke.transform.position = smokeFlipXposition.transform.position;
        }
        else if(targetPlayer.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
            rb2d.AddForce(new Vector2(rb2d.velocity.x * Time.deltaTime - 30f, rb2d.velocity.y * -1));
            hornSmoke.transform.position = smokeStartPosition.transform.position;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        /*if (groundCheck.IsTouching(collision))
        {
            detectedHorCollider = collision;
            print(groundCheck.size);
        }
        if (balanceCheck.IsTouching(collision))
        {
            detectedVertCollider = collision;
            print(balanceCheck.size);
            print("ACQUIRED NEW VERT COL");
        }
        /*if (collision.gameObject.tag != "Player")
        {
            if(balanceCheck.IsTouching(collision) && hasJumped == false)
            {
                Jump();
            }
        }  */ 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (distToGroundMeasure.IsTouching(collision))
        {
            detectedLowerGround = collision;
        }
        if (collision.gameObject.tag == "Player")
        {
            //rb2d.velocity = new Vector2(0, 0);
            TransitionToChase();
        }
        /*if (collision.gameObject.tag == "Prop" && chasing == true && hasJumped == false)
        {
            Jump();
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb2d.velocity = new Vector2(0, 0);
            switchDirection = !switchDirection;
            TransitionToPatrol();
        }
        /*if(collision.gameObject.tag == "Terrain" && hasJumped == false)
        {
            switchDirection = !switchDirection;
            StartCoroutine(Jumper());
        }*/
    }
    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (balanceCheck.IsTouching(collision.collider))
        {
            switchDirection = !switchDirection;
            hasSwitched = true;
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundCheck.IsTouching(collision.collider))
        {
            detectedHorCollider = collision.collider;
        }
        if (balanceCheck.IsTouching(collision.collider))
        {
            detectedVertCollider = collision.collider;
        }

        if(patrolling == true && collision.gameObject.tag == "Prop")
        {
            hasSwitched = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(chasing == true && hasJumped == true && hasSwitched == false && chaseTime < 12f)
        {
            switchDirection = !switchDirection;
            TransitionToPatrol();
        }
        if(!balanceCheck.IsTouching(detectedVertCollider) && detectedVertCollider == null)
        {
            detectedVertCollider = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            grounded = false;
        }
    }

    private void Jump()
    {
        hasJumped = true;
        //jumpStackPrevent = true;
        //detectedHorCollider = null;
        //detectedVertCollider = null;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 5f);
        //rb2d.AddForce(new Vector2(rb2d.velocity.x, rb2d.velocity.y + 10));
        //grounded = true;
        print("jumped");
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.GetComponent<PlayerPlatformerController>();
            player.knockbackCount = player.knockbackLength;

            if (collision.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }

            //rb2d.velocity = new Vector2(0, 0);
        }
        if(collision.gameObject.tag == "Projectile")
        {
            defaultTriggerZone.radius *= 2;
            
        }
    }

    private IEnumerator Jumper()
    {
        rb2d.velocity = new Vector2(0, 0);
        hasJumped = true;
        rb2d.velocity = new Vector2(0, 40f);
        print("Jumped AI");
        stopJumper = true;
        yield return null;
    }
    
    void TransitionToPatrol()
    {
        state = State.PATROL;
        defaultTriggerZone.radius -= 12.5f;
    }

    void TransitionToChase()
    {
        state = State.CHASE;
        defaultTriggerZone.radius = 17.5f;
    }

}
