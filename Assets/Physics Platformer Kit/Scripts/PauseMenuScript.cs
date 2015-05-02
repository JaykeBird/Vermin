using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

    private GUIManager gui;
    private Canvas canvas;

    public GameObject player;

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



}
