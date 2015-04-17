using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

    private GUIManager gui;
    private Canvas canvas;

    void Awake()
    {
        gui = FindObjectOfType(typeof(GUIManager)) as GUIManager;
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    public void GoToMainMenu()
    {
        Application.LoadLevel("MainMenu");

    }

    public void Continue()
    {
        gui.pauseGame = false;
        canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
    }

}
