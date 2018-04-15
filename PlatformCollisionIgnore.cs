using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollisionIgnore : MonoBehaviour {

    public CapsuleCollider2D playerCollider;
    //public BoxCollider2D platformCollider;
    //public BoxCollider2D platformTrigger;
    public GameObject movingPlatform;
    public Collider2D platCollider;

	// Use this for initialization
	void Start () {
        playerCollider = GameObject.Find("Player").GetComponent<CapsuleCollider2D>();
        //Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
        movingPlatform = this.gameObject;
        platCollider = movingPlatform.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            //Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
            platCollider.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            //Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
            platCollider.enabled = true;
        }
    }
}
