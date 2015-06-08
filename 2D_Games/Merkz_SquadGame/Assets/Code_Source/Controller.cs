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

		Invoke("SetCustomCursor",0.01f);  
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

		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			mob.Fire();
		}

		//Need to get Coordinates in reference to Player.

		GetWorldPosition();
		//Now to calculate direction.
		// mob.Set_Aim( mousePosition- (mob.position+ new Vector2(0,1.2f)) );
		mob.Set_Aim(mousePosition);
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

		float distance =  -(ray.origin.z/ ray.direction.z);

        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
		mousePosition=  (ray.origin+ (ray.direction*distance) );
	    }


	}





	
	public bool ccEnabled = false; 




  	void OnDisable()   
    {  
        //Resets the cursor to the default  
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);  
        //Set the _ccEnabled variable to false  
        this.ccEnabled = false;  
    }  

	private void SetCustomCursor()  
    {  
        //Replace the 'cursorTexture' with the cursor    
		Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/AimCursor100x100"),new Vector2(100,100),CursorMode.Auto);
        Debug.Log("Custom cursor has been set.");  
        //Set the ccEnabled variable to true  
        this.ccEnabled = true;  
    }  



}
