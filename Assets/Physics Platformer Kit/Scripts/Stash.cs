using UnityEngine;
using System.Collections;

public class Stash : MonoBehaviour //fuck you again tom
{

    public AudioClip depositSound; //sound to play when coins are deposited

    private GUIManager gui;

    public int stashId;

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
        if (other.tag == "Enemy")
        {
            coinWithdraw(other);
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

            bool canputincoins = false;

            while (canputincoins == false)
            {

            }
            gui.DepositCoinsInStash(this.stashId, gui.coinsCollected);
            //gui.coinsInStash1 += gui.coinsCollected;
            gui.coinsCollected = 0;
        }


    }

    public void coinWithdraw(Collider other)
    {
        if (gui)
        {
            if (gui.RemoveCoinsInStash(this.stashId, 1) == 1)
            {
                other.gameObject.GetComponent<EnemyAI>().coins++;
            }
        }
    }
}
