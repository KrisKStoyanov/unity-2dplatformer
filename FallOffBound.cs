using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffBound : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
            print("AI died");
        }
        else if(collision.gameObject.tag == "Player")
        {
            HealthManager.Instance.TakeDamage(100);
            print("Player Died");
        }
    }
}
