using UnityEngine;
using System.Collections;

public class Stash : MonoBehaviour //fuck you again tom
{

    public AudioClip depositSound; //sound to play when coins are deposited

    private GUIManager gui;
    private int stashNum;
    public int stashId;

    // Use this for initialization
    void Start ()
    {
        gui = FindObjectOfType(typeof(GUIManager)) as GUIManager;
        gui.addStash (this);
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
            //coinWithdraw(other);
        }
    }

    public void coinDeposit()
    {
        if (gui)
        {

            if (depositSound)
            {
                AudioSource.PlayClipAtPoint(depositSound, transform.position);
            }

            bool canputincoins = false;

            //while (canputincoins == false)
            //{

           // }
            //gui.DepositCoinsInStash(this.stashId, gui.coinsCollected);
            //gui.coinsInStash1 += gui.coinsCollected;
            stashNum += gui.coinsCollected;
            gui.coinsCollected = 0;
            gui.updateStashes();
        }


    }

    public bool coinWithdraw()
    {
        /*if (gui)
        {
            if (gui.RemoveCoinsInStash(this.stashId, 1) == 1)
            {
                other.gameObject.GetComponent<EnemyAI>().coins++;
            }
        }*/
        if (stashNum > 0) {
//            other.GetComponent<EnemyAI> ().coins++;
            stashNum--;
			return true;
        }
		return false;
    }
    public int getStashNum()
    {
        return stashNum;
    }
}