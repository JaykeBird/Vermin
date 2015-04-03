using UnityEngine;
using System.Collections;

public class Stash : MonoBehaviour
{

    public AudioClip depositSound; //sound to play when coins are deposited

    private GUIManager gui;

	// Use this for initialization
	void Start ()
    {
        gui = FindObjectOfType(typeof(GUIManager)) as GUIManager;
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            coinDeposit();
        }
    }

    void coinDeposit()
    {
        if (gui)
        {
            if (depositSound)
            {
                AudioSource.PlayClipAtPoint(depositSound, transform.position);
            }
            gui.coinsInStash1 += gui.coinsCollected;
            gui.coinsCollected = 0;
        }
    }
}
