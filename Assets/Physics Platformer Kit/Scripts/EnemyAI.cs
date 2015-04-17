﻿using UnityEngine;
using System.Collections;

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
    [HideInInspector]
    public int coins = 0;
	
	//setup
	void Awake()
	{		
        // Instantiates the GUIManager.
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
		if (trapTrigger && trapTrigger.colliding) {
			if (trapTrigger.hitObject.tag == "Glue") {
				Debug.Log ("Sticky");
				speedLimit = 2f;
				acceleration = 4f;
			}
			if (trapTrigger.hitObject.tag == "Fan") {
				Vector3 f = trapTrigger.hitObject.transform.position;
				Vector3 e = transform.position;
				Vector3 i = e - f;
				i.Normalize ();
				this.rigidbody.AddForce (i*20,ForceMode.VelocityChange);
			}
			if (trapTrigger.hitObject.tag == "Vacuum") {
				Vector3 a = trapTrigger.hitObject.transform.position;
				Vector3 b = transform.position;
				Vector3 c = a-b;
				c.Normalize ();
				this.rigidbody.AddForce (c*20,ForceMode.VelocityChange);

			}
		} else {
			speedLimit = cSpeedLimit;
			acceleration = cAccel;
		}

        if (stashTrigger && stashTrigger.colliding)
        {
            coins++;
            theStash.GetComponent<Stash>().coinWithdraw();
        }
        
		//chase
		if (sightTrigger && sightTrigger.colliding && chase)
		{
			characterMotor.MoveTo (sightTrigger.hitObject.transform.position, acceleration, chaseStopDistance, ignoreY);
			//nofity animator controller
			if(animatorController)
				animatorController.SetBool("Moving", true);
			//disable patrol behaviour
			if(moveToPointsScript)
				moveToPointsScript.enabled = false;
		}
		else
		{	
			//notify animator
			if(animatorController)
				animatorController.SetBool("Moving", false);
			//enable patrol behaviour
			if(moveToPointsScript)
				moveToPointsScript.enabled = true;
		}
		
		//attack
		if (attackTrigger && attackTrigger.collided)
		{
            //Debug.Log("Attack the player?");

            if (gui.coinsCollected > 0)
            {
                gui.coinsCollected--;
                coins++;
                Debug.Log(coins.ToString());
            }

			dealDamage.Attack(attackTrigger.hitObject, attackDmg, pushHeight, pushForce);
			//notify animator controller
			if(animatorController)
				animatorController.SetBool("Attacking", true);	
		}
		else if(animatorController)
			animatorController.SetBool("Attacking", false);
	}
	
	void FixedUpdate()
	{
		characterMotor.ManageSpeed(deceleration, speedLimit, ignoreY);
		characterMotor.RotateToVelocity (rotateSpeed, ignoreY);
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