using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour {

    public void BackToMenu()
    {
        LevelLoader.Instance.LoadLevel(0);
    }

    public void Exit()
    {
        GameMaster.Instance.Save();
        Application.Quit();
    }


}
