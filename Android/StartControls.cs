using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartControls : MonoBehaviour {

    public Image tapToBegin;

    public Canvas touchControls;

    private bool beginTap = true;

	// Update is called once per frame
	/*void Update () {
        if (Input.touchCount > 0 && beginTap == true)
        {
            TapToBegin();
        }
    }*/
    void TapToBegin()
    {
        beginTap = false;
        touchControls.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
