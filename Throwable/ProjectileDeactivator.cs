using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeactivator : MonoBehaviour {

    private ProjectileMovement projectileMovement;

    void OnEnable()
    {
        Invoke("Deactivate", 20f);
        projectileMovement = FindObjectOfType<ProjectileMovement>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Deactivate();
        }
        else if(collision.gameObject.tag != "Enemy")
        {
            Invoke("Deactivate", 2.5f);
        }
    }

    void Deactivate()
    {
        //StopCoroutine(ProjectileMovement.Instance.SpeedUpProjectile());
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

}
