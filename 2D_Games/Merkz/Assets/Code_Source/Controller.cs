using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

//Animation Controller
	MovingObject mob;

	public void Init_MovingObject(MovingObject mob)
	{
		this.mob=mob;
		Debug.Log("Moving Object  Linked");
	}

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.D))
		{
			mob.Move_Right();
		}
		if(Input.GetKey(KeyCode.A))
		{
			mob.Move_Left();
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			mob.Move_Jump();
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			mob.Move_LimitJump();
		}

	}

















}
