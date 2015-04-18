using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//class to add to collectible coins
[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    public AudioClip collectSound;							//sound to play when item is collected
    public Vector3 rotation = new Vector3(0, 80, 0);		//idle rotation of item
    public Vector3 rotationGain = new Vector3(0, 20, 00);	//added rotation when player gets near item 
    public float startSpeed = 3f;							//how fast item moves toward player when they get near
    public float speedGain = 0.2f;							//how fast item accelerates toward player when they're near
    public Texture2D sprite;

    private bool collected;
    private Transform player;
    private TriggerParent triggerParent;	//this is a utility class, that lets us check if the player is close to the items "bounds sphere trigger"
    private GUIManager gui;
    private static List<Item> inventory = new List<Item>(8);


    //setup
    void Awake()
    {
        gui = FindObjectOfType(typeof(GUIManager)) as GUIManager;
        if (tag != "Item")
        {
            tag = "Item";
            Debug.LogWarning("'Item' script attached to object not tagged 'Item', tag added automatically", transform);
        }
        collider.isTrigger = true;
        triggerParent = GetComponentInChildren<TriggerParent>();
        //if no trigger bounds are attached to coin, set them up
        if (!triggerParent)
        {
            GameObject bounds = new GameObject();
            bounds.name = "Bounds";
            bounds.AddComponent("SphereCollider");
            bounds.GetComponent<SphereCollider>().radius = 2f;
            bounds.GetComponent<SphereCollider>().isTrigger = true;
            bounds.transform.parent = transform;
            bounds.transform.position = transform.position;
            bounds.AddComponent("TriggerParent");
            triggerParent = GetComponentInChildren<TriggerParent>();
            triggerParent.tagsToCheck = new string[1];
            triggerParent.tagsToCheck[0] = "Player";
            Debug.LogWarning("No pickup radius 'bounds' trigger attached to item: " + transform.name + ", one has been added automatically", bounds);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //move item toward player when he is close to it, and increase the spin/speed of the item
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.World);

        if (triggerParent.collided)
            collected = true;

        if (collected)
        {
            startSpeed += speedGain;
            rotation += rotationGain;
            transform.position = Vector3.Lerp(transform.position, player.position, startSpeed * Time.deltaTime);
        }
    }

    //give player coin when it touches them
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ItemGet();
    }

    void ItemGet()
    {
        if (collectSound)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        if (gui && sprite)
        {
            if (this.name.Equals("Glue Bottle"))   //Glue bottle is item number 0
            {
                Item.inventory.Insert(0, this);
            }

            bool newitem = true; // not an item in our inventory

            foreach (ItemObject item in gui.inventory)
            {
                if (item.Name == this.name)
                {
                    item.AddItem();
                    newitem = false;
                }
            }

            if (newitem)
            {
                ItemObject obj = new ItemObject();
                obj.Name = this.name;
                //obj.Texture = this.sprite;
                Texture2D tex = Resources.Load(this.gameObject.name + "-32") as Texture2D;
                obj.Texture = tex;
                obj.AddItem();
                gui.inventory.Add(obj);
            }
        }

        Destroy(gameObject);
    }
}
