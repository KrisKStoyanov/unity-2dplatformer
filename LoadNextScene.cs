using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && GameMaster.Instance.speedrunning == false)
        {
            LevelLoader.Instance.LoadLevel(2);
        }
        else if(collision.gameObject.tag == "Player" && GameMaster.Instance.speedrunning == true)
        {
            Speedrunner.Instance.timerOn = false;
        }
    }
}
