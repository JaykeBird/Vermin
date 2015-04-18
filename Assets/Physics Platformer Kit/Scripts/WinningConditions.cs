using UnityEngine;
using System.Collections;

public class WinningConditions : MonoBehaviour {
	public int maxStashValue;
	public string nextScene;
	private GUIManager gui;
	// Use this for initialization
	void Start () {
		gui = FindObjectOfType(typeof(GUIManager)) as GUIManager;
	}
	
	// Update is called once per frame
	void Update () {
		bool win = true;
		for (int i = 0; i< gui.stashes.Count; i++)
		{

			if(gui.stashes[i].getStashNum () != maxStashValue)
			{
				win = false;
			}
		}
		if (win) {
			Application.LoadLevel(nextScene);
		}
	}
}
