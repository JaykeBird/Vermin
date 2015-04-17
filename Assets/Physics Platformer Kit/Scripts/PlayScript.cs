using UnityEngine;
using System.Collections;

public class PlayScript : MonoBehaviour {

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    public void ClickPlay()
    {
        Debug.Log("Let's play!");

        // Physics Platformer Set/Scenes/OfficeSpaceFloor1.unity
        Application.LoadLevel("OfficeSpaceFloor1");
    }

    public void ClickExit()
    {
        Debug.Log("Let's exit!");

        Application.Quit();
    }
}
