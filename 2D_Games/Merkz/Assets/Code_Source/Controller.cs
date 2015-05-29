using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

//Animation Controller
	MovingObject mob;
	GameObject camFocus;
	public void Init_MovingObject(MovingObject mob)
	{
		this.mob=mob;
		Debug.Log("Moving Object  Linked");
		camFocus = GameObject.Find("Camera_Focus");		
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

		//Need to get Coordinates in reference to Player.

		GetWorldPosition();
		//Now to calculate direction.
		mob.Set_Aim( mousePosition- (mob.position+ new Vector2(0,1.2f)) );
		camFocus.transform.position = mob.position;
	}


	Vector2 mousePosition;
	void GetWorldPosition()
	{
        Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
        if(screenRect.Contains(Input.mousePosition))
        {
 		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//Get Distance from Origin to Ground based on direction

		float distance = 20;// -(ray.origin.z/ ray.direction.z);

        Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow);
		mousePosition=  (ray.origin+ (ray.direction*distance) );
	    }


	}













}
