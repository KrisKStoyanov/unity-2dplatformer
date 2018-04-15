using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHazard001 : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            HealthManager.Instance.TakeDamage(5f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            //healthManager.TakeDamage(10);
            //InvokeRepeating("TakeDamage(1f)", 1f, 1f);
            HealthManager.Instance.TakeContDamage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            HealthManager.Instance.StopContDamage();
        }
    }
}
