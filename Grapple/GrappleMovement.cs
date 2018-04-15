using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleMovement : MonoBehaviour {

    private GameObject player;
    public SpriteRenderer playerSprite;
    private float grappleSpeed = 10;
    private SpriteRenderer grappleRenderer;

    private float hangTime = 250f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSprite = player.GetComponent<SpriteRenderer>();
        grappleRenderer = GetComponent<SpriteRenderer>();
        //Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
    }

    private void Start()
    {
        ShootGrapple();
    }

    public void ShootGrapple()
    {
        StartCoroutine(ResetObject());
        if(playerSprite.flipX == false)
        {
            grappleRenderer.flipX = false;
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, Random.Range(30, 50));
            StartCoroutine(ShootRight());
        }

        else if(playerSprite.flipX == true)
        {
            grappleRenderer.flipX = true;
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, Random.Range(-30, -50));
            StartCoroutine(ShootLeft());
        }
    }

    private IEnumerator ShootLeft()
    {
        while (true)
        {
            Rigidbody2D thisRB2D = GetComponent<Rigidbody2D>();
            //thisRB2D.AddForce(new Vector2(thisRB2D.position.x * grappleSpeed, thisRB2D.position.y * -grappleSpeed));
            thisRB2D.AddForce(transform.right * -grappleSpeed);
            yield return null;
        }
    }

    private IEnumerator ShootRight()
    {
        while (true)
        {
            Rigidbody2D thisRB2D = GetComponent<Rigidbody2D>();
            //thisRB2D.AddForce(new Vector2(thisRB2D.position.x * -grappleSpeed, thisRB2D.position.y * -grappleSpeed));
            thisRB2D.AddForce(transform.right * grappleSpeed);
            yield return null;
        }
    }

    private IEnumerator ResetObject()
    {
        while (true)
        {
            hangTime -= 1;
            if(hangTime == 0)
            {
                gameObject.SetActive(false);
                StopCoroutine(ResetObject());
            }
            yield return null;
        }
    }
}
