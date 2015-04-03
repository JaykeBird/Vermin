using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ATTACH TO MAIN CAMERA, shows your health and coins
public class GUIManager : MonoBehaviour 
{	
	public GUISkin guiSkin;					//assign the skin for GUI display
	[HideInInspector]
    public int coinsCollected=0;
    public int coinsInStash1=0;
    public List<bool> inventoryCheck = new List<bool>(8);       //this will show which items the player currently has

	private int coinsInLevel;
    private int itemsInLevel;
	
	//setup, get how many coins are in this level
	void Start()
    {
        coinsInLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
        //itemsInLevel = GameObject.FindGameObjectsWithTag("Item").Length;
	}
	
	//show and how many coins you've collected
	void OnGUI()
	{
		GUI.skin = guiSkin;
		GUILayout.Space(5f);

        if (coinsInLevel > 0)
        {
            GUILayout.Label("Coins: " + coinsCollected + " / " + coinsInLevel);
            GUILayout.Label("Stash One: " + coinsInStash1);
        }

        if (itemsInLevel > 0)
        {
			foreach (bool i in inventoryCheck)
			{
				if (i == true)
				{
					//GUILayout.Label(i);

        		}
			}
		}
	}
}