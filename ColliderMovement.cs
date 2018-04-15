using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMovement : MonoBehaviour {

    private BoxCollider2D movingBC2D;
    private Vector2 initialSize;

    private bool timeToResetSize = false;
    private float counterToReset = 50f;

    public ParticleSystem toxicSpew;

    //Coroutine to keep the particle active after the player leaves the damage zone of the barrel
    private IEnumerator particleKeep;
    private float counterTillDisable = 5f;
    private bool triggerDisableCounter = false;

	// Use this for initialization
	void Start () {
        movingBC2D = GetComponent<BoxCollider2D>();
        //reduction = new Vector2(movingBC2D.size.x, movingBC2D.size.y - 1f);
        //sinkCoroutine = sink();
        initialSize = movingBC2D.size;
        particleKeep = WaitForParticle(10);
        toxicSpew.Stop();

	}
	
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            collision.transform.SetParent(gameObject.transform);
            ReduceColliderSize();
            timeToResetSize = false;
            //toxicSpew.gameObject.SetActive(true);
            StartCoroutine(particleKeep);
            toxicSpew.Emit(10);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            collision.transform.SetParent(null);
            timeToResetSize = true;
            //toxicSpew.gameObject.SetActive(false);
            toxicSpew.Stop();
            
        }

    }

    private void FixedUpdate()
    {

        if (timeToResetSize == true)
        {
            counterToReset-=.5f;
            if (counterToReset == 0f)
            {
                IncreaseColliderSize();
 
            }
        }
        if(triggerDisableCounter == true)
        {
            counterTillDisable -= 1f;
            if(counterTillDisable == 0f)
            {
                toxicSpew.gameObject.SetActive(false);
            }
        }

    }
    void ReduceColliderSize()
    {
        //movingBC2D.size = reduction;
        if (movingBC2D.size.y > 1f)
            movingBC2D.size = new Vector2(movingBC2D.size.x, movingBC2D.size.y - 0.1f);
        else if(movingBC2D.size.y <= 1f)
            movingBC2D.size = movingBC2D.size;
    }
    /*void RunTimerTillReset()
    {
        counterToReset = counterToReset - 1;
        
    }
    */
    void IncreaseColliderSize()
    {
        timeToResetSize = false;
        counterToReset = 50f;
        movingBC2D.size = initialSize;
    }
    /*private IEnumerator sink(float waitTime = 1f)
    {
        while (true)
        {
            ReduceColliderSize();
            yield return new WaitForSeconds(waitTime);
        }
       
    }*/
    private IEnumerator WaitForParticle(float waitTime)
    {
        while (true)
        {
            toxicSpew.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            triggerDisableCounter = true;
        }
    }
 
}
