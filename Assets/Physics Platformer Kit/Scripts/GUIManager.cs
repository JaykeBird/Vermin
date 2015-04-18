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

    public List<Stash> stashes = new List<Stash>();
    public List<ItemObject> inventory = new List<ItemObject>(); //this will show which items the player currently has

    private int coinsInLevel;
    private int itemsInLevel;

    public bool pauseGame = false;
    
    //setup, get how many coins are in this level
    void Start()
    {
        coinsInLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
        itemsInLevel = GameObject.FindGameObjectsWithTag("Item").Length;
    }
    
    //show how many coins you've collected
    void OnGUI()
    {
        GUI.skin = guiSkin;
        GUILayout.Space(5f);

        if (coinsInLevel > 0)
        {
            GUILayout.Label("Coins: " + coinsCollected + " / " + coinsInLevel);

            int count = 0;

			for(int i = 0; i<stashes.Count; i++)
            {
                //count++;
                GUILayout.Label("Stash " + i + " : " + stashes[i].getStashNum ());
            }
            //GUILayout.Label("Stash One: " + coinsInStash1);
        }

        if (itemsInLevel > 0)
        {
            foreach (ItemObject i in inventory)
            {
                if (i.Count > 0)
                {
                    //GUILayoutOption opt = null;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(i.Texture);
                    GUILayout.Label(i.Count.ToString());
                    GUILayout.EndHorizontal();
                    //GUILayout.Label(i.Name + ": " + i.Count.ToString());
                }
            }
        }
    }

    public int TotalNumberOfCoinsInStashes()
    {
        /*int c;

		for(int i = 0; i < stashes.Count; i++)
        {
			c += stashes[i].getStashNum;
        }

        return c;*/
		return 0;
    }

    public void DepositCoinsInStash(int id, int coins)
    {
       /* bool good = false;

        while (good == false)
        {
            if (stashcoins.Count > id)
            {
                stashcoins.Add(0);
            }
            else
            {
                good = true;
            }
        }

        stashcoins[id] = stashcoins[id] + coins;*/

    }

    public int RemoveCoinsInStash(int id, int coins)
    {
       /* bool good = false;

        while (good == false)
        {
            if (stashcoins.Count > id)
            {
                stashcoins.Add(0);
            }
            else
            {
                good = true;
            }
        }

        if (stashcoins[id] < coins)
        {
            int rem = stashcoins[id];
            stashcoins[id] = 0;
            return rem;
        }
        else
        {
            stashcoins[id] = stashcoins[id] - coins;
            return coins;
        }*/
		return 0;

        
    }

    public bool UseItem(int id)
    {
        if (inventory.Count > id)
        {
            bool r = inventory[id].UseItem();
            CleanInventory();
            return r;
        }
        else
        {
            return false;
        }
    }

    public bool UseItem(string name)
    {

        foreach (ItemObject item in inventory)
        {
            if (item.Name == name)
            {
                bool r = item.UseItem();
                CleanInventory();
                return r;
            }
        }

        return false;
    }

    public void CleanInventory()
    {
        List<ItemObject> itemstoremove = new List<ItemObject>();

        foreach (ItemObject item in inventory)
        {
            if (item.Count == 0)
            {
                itemstoremove.Add(item);
            }
        }

        Debug.Log("Removing " + itemstoremove.Count.ToString() + " items");

        foreach (ItemObject item in itemstoremove)
        {
            inventory.Remove(item);   
        }

        foreach (ItemObject item in inventory)
        {
            Debug.Log("Item " + item.Name + " with " + item.Count.ToString() + " items");
        }
    }
	public void addStash(Stash s)
	{
		stashes.Add (s);
	}
	public void updateStashes()
	{

	}
}