using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRoofPlatform : MonoBehaviour {

    public float fallSpeed;
    public GameObject fallingPlatform;
    public Transform fallDestination;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(Fall());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            fallSpeed *= 2;
        }
    }

    private IEnumerator Fall()
    {
        while (true)
        {
            fallingPlatform.transform.position = Vector2.MoveTowards(fallingPlatform.transform.position, fallDestination.position, Time.deltaTime * fallSpeed);
            yield return null;
        }
    }

}
