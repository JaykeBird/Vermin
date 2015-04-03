using UnityEngine;
using System.Collections;

//ATTACH TO MAIN CAMERA, shows your health and coins
public class GUIManager : MonoBehaviour 
{	
	public GUISkin guiSkin;					//assign the skin for GUI display
	[HideInInspector]
    public int coinsCollected=0;
    public int coinsInStash1=0;

	private int coinsInLevel;
	
	//setup, get how many coins are in this level
	void Start()
    {
        coinsInLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
	}
	
	//show current health and how many coins you've collected
	void OnGUI()
	{
		GUI.skin = guiSkin;
		GUILayout.Space(5f);

        if (coinsInLevel > 0)
        {
            GUILayout.Label("Coins: " + coinsCollected + " / " + coinsInLevel);
            GUILayout.Label("Stash One: " + coinsInStash1);
        }
	}
}