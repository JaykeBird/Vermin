using UnityEngine;
using System.Collections;

public class TrapTimer : MonoBehaviour {

	public float TrapTime; //the time this trap has to live on this world.
	public float TimeHappened; //the time this trap has lived on this world. 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TimeHappened = TimeHappened + Time.deltaTime;
		if (TimeHappened >= TrapTime) 
		{
			Destroy (this.gameObject);
		}
	}
}
