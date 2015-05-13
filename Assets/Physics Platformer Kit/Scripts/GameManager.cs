using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	private bool paused = false;

	public bool Paused {
		get {
			return paused;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P))
			PauseGame();
	}

	void PauseGame()
	{
		paused = !paused;
	}
	public static GameManager Instance {
		get {
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}
}
