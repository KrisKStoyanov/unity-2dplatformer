using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevelArrow : MonoBehaviour {

    public GameObject nextLevelArrow;

    public Animator gateDoor;
    public Animator gateDoorOther;

    public Animator cogDoor;
    public Animator cogDoorOther;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            nextLevelArrow.gameObject.SetActive(true);
            gateDoor.SetBool("open", true);
            gateDoorOther.SetBool("open", true);

            cogDoor.SetTrigger("spin");
            cogDoorOther.SetTrigger("spin");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            nextLevelArrow.gameObject.SetActive(false);
            gateDoor.SetBool("open", false);
            gateDoorOther.SetBool("open", false);

            cogDoor.SetTrigger("spin");
            cogDoorOther.SetTrigger("spin");
        }
    }
}
