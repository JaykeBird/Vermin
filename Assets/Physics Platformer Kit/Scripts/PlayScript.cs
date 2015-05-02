using UnityEngine;
using System.Collections;
using Assets.Physics_Platformer_Kit.Scripts;

public class PlayScript : MonoBehaviour {

    public Animator charSelect;
    public Animator panel;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    public void ClickPlay()
    {
        Debug.Log("Let's play!");

        charSelect.SetBool("IsShown", true);
        panel.SetBool("IsShown", false);

        // Physics Platformer Set/Scenes/OfficeSpaceFloor1.unity
        //Application.LoadLevel("OfficeSpaceFloor1");
    }

    public void ClickExit()
    {
        Debug.Log("Let's exit!");

        panel.SetBool("IsShown", false);

        Application.Quit();
        
    }

    public void StartGame(string cls)
    {
        switch (cls.ToLower())
        {
            case "squirrel":
                ChangeClass(CharacterClassData.characterClass.SQUIRREL);
                break;
            case "raccoon":
                ChangeClass(CharacterClassData.characterClass.RACCOON);
                break;
            case "ferret":
                ChangeClass(CharacterClassData.characterClass.FERRET);
                break;
            case "birdy":
                ChangeClass(CharacterClassData.characterClass.BIRD);
                break;
        }

        charSelect.SetBool("IsShown", false);
        Application.LoadLevel("OfficeSpaceFloor1");
    }

    public void GoBackHome()
    {
        charSelect.SetBool("IsShown", false);
        panel.SetBool("IsShown", true);
    }

    public void ChangeClass(CharacterClassData.characterClass value)
    {
        CharacterClassData.setClass(value);
    }
}
