using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour {

    private float startingSpeed;
    public float collapseSpeed;
    public GameObject collapsePlatform;
    public Transform maximumCollapseDistance;
    private BoxCollider2D platformCollider;
    public GameObject externalPlatform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            platformCollider = collapsePlatform.GetComponent<BoxCollider2D>();
            platformCollider.offset = new Vector2(-18f,platformCollider.offset.y);
            platformCollider.size = new Vector2(81f, platformCollider.size.y);
            startingSpeed = collapseSpeed;
            collision.transform.SetParent(gameObject.transform);
            StartCoroutine(Collapse());
            externalPlatform.SetActive(false);
            //StartCoroutine(IncreaseSpeed());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.transform.SetParent(null);
            StopCoroutine(Collapse());
            //StopCoroutine(IncreaseSpeed());
            //collapseSpeed = startingSpeed;
        }
    }

    private IEnumerator Collapse()
    {
        while (true)
        {
            collapsePlatform.transform.position = Vector2.MoveTowards(collapsePlatform.transform.position, maximumCollapseDistance.position, Time.deltaTime * collapseSpeed);
            yield return null;
        }
    }

    private IEnumerator IncreaseSpeed()
    {
        while (true)
        {
            collapseSpeed += 0.25f;
            yield return new WaitForSeconds(1f);
        }
    }
}
