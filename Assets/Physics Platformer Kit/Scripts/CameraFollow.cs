using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CameraFollow : MonoBehaviour 
{
	public Transform target;									//object camera will focus on and follow
	public Vector3 targetOffset =  new Vector3(0f, 3.5f, 7);	//how far back should camera be from the lookTarget
	public bool lockRotation;									//should the camera be fixed at the offset (for example: following behind the player)
	public float followSpeed = 6;								//how fast the camera moves to its intended position
	public float inputRotationSpeed = 100;						//how fast the camera rotates around lookTarget when you press the camera adjust buttons
	public bool mouseFreelook;									//should the camera be rotated with the mouse? (only if camera is not fixed)
	public float rotateDamping = 100;							//how fast camera rotates to look at target
	public GameObject waterFilter;								//object to render in front of camera when it is underwater
	public string[] avoidClippingTags;							//tags for big objects in your game, which you want to camera to try and avoid clipping with										//the Trigger Parent.

	//private MeshRenderer h;
	private List<MeshRenderer> hits = new List<MeshRenderer>();
	private Transform followTarget;
	private bool camColliding;
	
	//setup objects
	void Awake()
	{
		followTarget = new GameObject().transform;	//create empty gameObject as camera target, this will follow and rotate around the player
		followTarget.name = "Camera Target";
		if(waterFilter)
			waterFilter.GetComponent<Renderer>().enabled = false;
		if(!target)
			Debug.LogError("'CameraFollow script' has no target assigned to it", transform);
		
		//don't smooth rotate if were using mouselook
		if(mouseFreelook)
			rotateDamping = 0f;

	}
	
	//run our camera functions each frame
	void Update()
	{
		if (!target)
			return;
		
		SmoothFollow ();
		if(rotateDamping > 0)
			SmoothLookAt();
		else
			transform.LookAt(target.position);

		inTheWay ();

	}

	//toggle waterfilter, is camera clipping walls?
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Water" && waterFilter)
			waterFilter.GetComponent<Renderer>().enabled = true;
	}
	
	//toggle waterfilter, is camera clipping walls?
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Water" && waterFilter)
			waterFilter.GetComponent<Renderer>().enabled = false;
	}
	
	//rotate smoothly toward the target
	void SmoothLookAt()
	{
		Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotateDamping * Time.deltaTime);
	}
		
	//move camera smoothly toward its target
	void SmoothFollow()
	{
		//move the followTarget (empty gameobject created in awake) to correct position each frame
		followTarget.position = target.position;
		followTarget.Translate(targetOffset, Space.Self);
		if (lockRotation)
			followTarget.rotation = target.rotation;
		
		if(mouseFreelook)
		{
			//mouse look
			float axisX = Input.GetAxis ("Mouse X") * inputRotationSpeed * Time.deltaTime;
			followTarget.RotateAround (target.position,Vector3.up, axisX);
			float axisY = Input.GetAxis ("Mouse Y") * inputRotationSpeed * Time.deltaTime;
			followTarget.RotateAround (target.position, transform.right, -axisY);
		}
		else
		{
			//keyboard camera rotation look
			float axis = Input.GetAxis ("CamHorizontal") * (-inputRotationSpeed) * Time.deltaTime;
			followTarget.RotateAround (target.position, Vector3.up, axis);
		}
		
		//where should the camera be next frame?
		Vector3 nextFramePosition = Vector3.Lerp(transform.position, followTarget.position, followSpeed * Time.deltaTime);
		Vector3 direction = nextFramePosition - target.position;
		//raycast to this position
		RaycastHit hit;
		if(Physics.Raycast (target.position, direction, out hit, direction.magnitude + 0.3f))
		{
			transform.position = nextFramePosition;
			foreach(string tag in avoidClippingTags)
				if(hit.transform.tag == tag)
					transform.position = hit.point - direction.normalized * 0.3f;
		}
		else
		{
			//otherwise, move cam to intended position
			transform.position = nextFramePosition;
		}
	}
	private bool inTheWay()
	{
		bool done = false;
		RaycastHit hitinfo;
		Vector3 heading = target.position - transform.position;

		if(Physics.Raycast (transform.position, heading/heading.magnitude, out hitinfo, heading.magnitude))
		{
			//Debug.Log ("Something was hit.");
			if(hitinfo.transform != target)
			{
				//Debug.Log ("It was not the player");
				Vector3 a = new Vector3(hitinfo.point.x,hitinfo.point.y,hitinfo.point.z);
				Vector3 b = new Vector3(transform.position.x,transform.position.y,transform.position.z);
				Vector3 c = new Vector3(target.position.x,target.position.y,transform.position.z);



				if(Vector3.Distance (a,b) < Vector3.Distance (c, b))
				//if(Vector3.Angle (heading, b-a) < Mathf.PI /2)
				{
					/*Debug.Log ("To other: " + Vector3.Distance (a,b));
					Debug.Log ("To Player: " + Vector3.Distance (c,b));
					Debug.Log ("Other: (" + a.x + "," + a.y + "," + a.z + ")");
					Debug.Log ("Player: (" + c.x + "," + c.y + "," + c.z + ")");
					Debug.Log ("Camera: (" + b.x + "," + b.y + "," + b.z + ")");
					Debug.Log ("It was closer than the player");*/
					MeshRenderer h =  hitinfo.transform.GetComponentInParent<MeshRenderer>();
					h.enabled = false;
					hits.Add (h);
				}



					//for(int i = 0 ; i<hits.Count;i++)
					//{
					//	if(hits[i].GetComponentInParent<Transform>() != hitinfo.transform)
					//	{
					//		hits[i].enabled = true;
					//		hits.RemoveAt (i);
					//	}
					//}



			}else{
				hitinfo = new RaycastHit();
				for(int i = 0 ; i<hits.Count;i++)
				{
					hits[i].enabled = true;
					hits.RemoveAt (i);
				}
			}
				
		}
		return false;
	}
}