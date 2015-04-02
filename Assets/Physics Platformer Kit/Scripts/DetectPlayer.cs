using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

    public GameObject enemy1;
    public GameObject enemy2;

    //appear when player is present
    void OnTriggerEnter(Collider other)
    {
        enemy1.SetActive(true);
        enemy2.SetActive(true);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
