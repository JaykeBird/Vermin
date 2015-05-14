using UnityEngine;
using System.Collections;
using Assets.Physics_Platformer_Kit.Scripts;

//simple "platformer enemy" AI
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(DealDamage))]
public class EnemyAI : MonoBehaviour 
{
    public float acceleration = 35f;					//acceleration of enemy movement
    private float cAccel;
    public float deceleration = 8f;						//deceleration of enemy movement
    public float rotateSpeed = 0.7f;					//how fast enemy can rotate
    public float speedLimit = 10f;						//how fast enemy can move
    public Vector3 bounceForce = new Vector3(0, 13, 0);	//force to apply to player when player jumps on enemies head
    public AudioClip bounceSound;						//sound when you bounce on enemies
    public float pushForce = 10f;						//how far away to push the player when they are attacked by the enemy
    public float pushHeight = 7f;						//how high to push the player when they are attacked by the enemy
    public int attackDmg = 1;							//how much damage to deal to the player when theyre attacked by this enem
    public bool chase = true;							//should this enemy chase objects inside its sight?
    public bool ignoreY = true;							//ignore Y axis when chasing? (this would be false for say.. a flying enemy)
    public float chaseStopDistance = 0.7f;				//stop this far away from object when chasing it
    public GameObject sightBounds;						//trigger for sight bounds
    public GameObject attackBounds;						//trigger for attack bounds (player is hurt when they enter these bounds)
    public Animator animatorController;					//object which holds the animator for this enem	
    public MoveToPoints moveToPointsScript;				//if you've attached this script, drag the component here

    public GameObject trapBounds;
    private TriggerParent trapTrigger;

    private bool isSleeping = false;

    public GameObject stashBound;
    private TriggerParent stashTrigger;
    public GameObject theStash;

    private float cSpeedLimit;
    public TriggerParent sightTrigger;
    private TriggerParent attackTrigger;
    private PlayerMove playerMove;
    private CharacterMotor characterMotor;
    private DealDamage dealDamage;
    private GUIManager gui;
    private CharacterClassData.characterClass playerClass=CharacterClassData.getClass();
    public int coins;
    [HideInInspector]

    
    //setup
    void Awake()
    {		
        // Instantiates the GUIManager.
        coins = 0;
        gui = FindObjectOfType(typeof(GUIManager)) as GUIManager;

        cSpeedLimit = speedLimit;
        cAccel = acceleration;
        characterMotor = GetComponent<CharacterMotor>();
        dealDamage = GetComponent<DealDamage>();

     
        if (trapBounds) 
        {
            trapTrigger = trapBounds.GetComponent<TriggerParent>();
            if(!trapTrigger)
            {
                Debug.LogError("You forgot to add the TriggerParent script to the Trapbounds. It was not found");
            }
        }
        if (stashBound)
        {
            stashTrigger = stashBound.GetComponent<TriggerParent>();
        }

        if(tag != "Enemy")
        {
            tag = "Enemy";
            Debug.LogWarning("'EnemyAI' script attached to object without 'Enemy' tag, it has been assign automatically", transform);
        }

        if(sightBounds)
        {
            sightTrigger = sightBounds.GetComponent<TriggerParent>();
            if(!sightTrigger)
                Debug.LogError("'TriggerParent' script needs attaching to enemy 'SightBounds'", sightBounds);
        }
        if(!sightBounds)
            Debug.LogWarning("Assign a trigger with 'TriggerParent' script attached, to 'SightBounds' or enemy will not be able to see", transform);
        
        if(attackBounds)
        {
            attackTrigger = attackBounds.GetComponent<TriggerParent>();
            if(!attackTrigger)
                Debug.LogError("'TriggerParent' script needs attaching to enemy 'attackBounds'", attackBounds);
        }
        else
            Debug.LogWarning("Assign a trigger with 'TriggerParent' script attached, to 'AttackBounds' or enemy will not be able to attack", transform);
    }
    
    void Update()
    {
        if (GameManager.Instance.Paused)
            return;
        if (true)
        {
            if (isSleeping)
            {
                Debug.Log("Is Sleeping");
                //this.rigidbody.constraints = RigidbodyConstraints.None;
                isSleeping = false;
            }
            if(transform.position.y < -1)
            {
                transform.Translate(Vector3.up* 2);
                Debug.Log ("We moved on up!");
            }
            if(transform.position.y > 0.5)
            {
                transform.Translate (Vector3.down * 2);
            }

            if (trapTrigger && trapTrigger.colliding)
            {
                if (trapTrigger.hitObject.tag == "Glue")
                {
                    //Debug.Log("Sticky");
                    speedLimit = 2f;
                    acceleration = 4f;
                }
                if (trapTrigger.hitObject.tag == "Fan")
                {
                    Vector3 f = trapTrigger.hitObject.transform.position;
                    Vector3 e = transform.position;
                    Vector3 i = e - f;
                    i.Normalize();
                    i.y = 0;
                    this.rigidbody.AddForce(i * 20, ForceMode.VelocityChange);
                }
                if (trapTrigger.hitObject.tag == "Vacuum")
                {
                    Vector3 a = trapTrigger.hitObject.transform.position;
                    Vector3 b = transform.position;
                    Vector3 c = a - b;
                    c.y = 0;
                    c.Normalize();
                    this.rigidbody.AddForce(c * 20, ForceMode.VelocityChange);

                }
            }
            else
            {
                speedLimit = cSpeedLimit;
                acceleration = cAccel;
            }

            if (stashTrigger && stashTrigger.colliding)
            {
                
                if(theStash.GetComponent<Stash>().coinWithdraw())
                    coins++;
            }

            //chase
            if (sightTrigger && sightTrigger.colliding && chase)
            {	RaycastHit hitinfo;
                Vector3 heading = sightTrigger.hitObject.transform.position-transform.position;

                if(Physics.Raycast (transform.position, heading/heading.magnitude, out hitinfo, heading.magnitude))
                {
                    if(hitinfo.transform.Equals (sightTrigger.hitObject.transform.position))
                    {
                        Debug.Log ("There is something in the way.");
                    }

                }
                characterMotor.MoveTo(sightTrigger.hitObject.transform.position, acceleration, chaseStopDistance, ignoreY);
                //nofity animator controller
                if (animatorController)
                    animatorController.SetBool("Moving", true);
                //disable patrol behaviour
                if (moveToPointsScript)
                    moveToPointsScript.enabled = false;
            }
            else
            {
                //notify animator
                if (animatorController)
                    animatorController.SetBool("Moving", false);
                //enable patrol behaviour
                if (moveToPointsScript)
                    moveToPointsScript.enabled = true;
            }

            //attack
            if (attackTrigger && attackTrigger.collided)
            {
                //Debug.Log("Attack the player?");

                if (gui.coinsCollected > 0 && attackTrigger.hitObject.transform.position.y <= transform.position.y+transform.localScale.y/2f)
                {
                    gui.coinsCollected -= CharacterClassData.getCoinLossAmount(playerClass);
                    if (gui.coinsCollected < 0)
                    {
                        gui.coinsCollected = 0;
                        coins++;
                    }
                    else
                    {
                        coins += CharacterClassData.getCoinLossAmount(playerClass);
                    }
                    Debug.Log(coins.ToString());
                }

                dealDamage.Attack(attackTrigger.hitObject, attackDmg, pushHeight, pushForce);
                //notify animator controller
                if (animatorController)
                    animatorController.SetBool("Attacking", true);
            }
            else if (animatorController)
                animatorController.SetBool("Attacking", false);
        }
        else
        {
            if (!isSleeping)
            {
                Debug.Log("Go to sleep");
                //this.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                isSleeping = true;
            }
            
        }
    }
    
    void FixedUpdate()
    {
        //if (GameManager.Instance.Paused)
            //return;
        if (true)
        {
            characterMotor.ManageSpeed(deceleration, speedLimit, ignoreY);
            characterMotor.RotateToVelocity(rotateSpeed, ignoreY);
        }
    }
    
    //bounce player when they land on this enemy
    public void BouncedOn()
    {	
        if(!playerMove)
            playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        if (bounceSound)
            AudioSource.PlayClipAtPoint(bounceSound, transform.position);
        if(playerMove)
        {
            Vector3 bounceMultiplier = new Vector3(0f, 1.5f, 0f) * playerMove.onEnemyBounce;
            playerMove.Jump (bounceForce + bounceMultiplier);
        }
        else
            Debug.LogWarning("'Player' tagged object landed on enemy, but without playerMove script attached, is unable to bounce");
    }

}