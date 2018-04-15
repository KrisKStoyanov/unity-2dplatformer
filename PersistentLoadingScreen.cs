using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistentLoadingScreen : MonoBehaviour {

    public static PersistentLoadingScreen Instance;

    private Canvas thisCanvas;

    public GameObject loadScreen;
    public Image loadCog;
    public Text progText;
    public Text loadText;
    public GameObject mainT;
    public GameObject subT;

    private Scene activeScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        thisCanvas = GetComponent<Canvas>();
        thisCanvas.worldCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
    }
}
