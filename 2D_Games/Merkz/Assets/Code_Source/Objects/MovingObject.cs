using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyCollision;

public class MovingObject
{
	readonly float _GravityMod 	= 	20;
	readonly float _MaxSpeed	=	10;
	readonly float _MaxJumpMod	=	10; //Must be higher than Gravity
	float curJumpForce=0;

	bool usesGravity=true;

	bool onLedge=false;

	//These two are used to determining whether the player is on
	//a wall and which direction
	bool isOnWall=false;
	bool wallisOnLeft=false;

	bool onLadder=false;


	//This value is used to prevent jump spam
	bool isJumping= false;
	bool wallJump = false;
	bool isOnLand=false;


	bool isMoving=false;
	float x_Velocity=0;
	float x_acceleration=2f;
	float x_brake=1;




	Vector2 aim_Target;
	public Vector2 position;
	public GameObject go_Model;
	Vector2 newPosition; 
	Animator animController;
	public MovingObject(GameObject go, Vector2 position)
	{
		this.go_Model = go;
		this.position=position;
		this.aim_Target=new Vector2(1,0);//Default to Forward
		this.animController= go_Model.GetComponent<Animator>();
	}



	public void Update(float timeElapsed)
	{
		//Aim has nothing to do with movement or collision, so process it first.
		Calculate_AimRotation();

		//If not not provided MovementINPUT, then we are braking.
		if(!isMoving && isOnLand)
		{
			if(x_Velocity>0)
			{
				x_Velocity -= x_brake;
				if(x_Velocity<0)
					x_Velocity=0;
			}
			else if(x_Velocity<0)
			{
				x_Velocity += x_brake;
				if(x_Velocity>0)
					x_Velocity=0;
			}
		}
		newPosition+= (new Vector2(1,0) * x_Velocity );

		isMoving=false;

		//Assume new Position has been placed by controller.
		//Perform calculation based on timeelapsed since event press
		Vector2 difference = newPosition- position;
		newPosition = position + (difference * timeElapsed);


		//First Check negates Gravity
		ApplyGravity(timeElapsed);

		//SecondCheck negates Movement
		//CollisionCheck
		Correct_Position(timeElapsed);

		//Reposition our Guy
		Apply_FinalPosition();
	}


	void Correct_Position(float timeElapsed)
	{
		isOnLand=false;
		wallJump=false;
		isOnWall=false;
		//Using Both Corner Extremes perform Ground Collision
		//Process Top Edges( Bottom Collision )
		//-------------------------
		Edge collisionEdge = Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0.5f,0.6f),newPosition + new Vector2(0.5f,-0.1f),0);
		Edge collisionEdge_2 = Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(-0.5f,0.6f),newPosition + new Vector2(-0.5f,-0.1f),0);
		if(collisionEdge.side!=Edge_Side.None)
			Debug.DrawLine(collisionEdge.point_1, collisionEdge.point_2, Color.blue);


			if(collisionEdge.side!=Edge_Side.None)
			{
				//Convert Line into Slope equation y = mx + b
				Vector2 p2 = collisionEdge.point_2;//- edges[x].point_1;
				Vector2 p1 = collisionEdge.point_1;// - edges[x].point_1;//0
				float playerX = newPosition.x -collisionEdge.point_1.x;
				float slope = Collision_Engine.RoundNum(p2.y - p1.y,2) / Collision_Engine.RoundNum(p2.x - p1.x,2); 

				float y = slope* playerX ;
				newPosition.y = y+ collisionEdge.point_1.y +0.06f;//y + edges[x].point_1.y;//+ position.y + 0.01f;

				newPosition.y = Collision_Engine.RoundNum( newPosition.y, 2);	
				isJumping=false; //Landed
				isOnLand=true;
				curJumpForce=-_GravityMod;
			}
			else if(collisionEdge_2.side!=Edge_Side.None)
			{
				//Convert Line into Slope equation y = mx + b
				Vector2 p2 = collisionEdge_2.point_2;//- edges[x].point_1;
				Vector2 p1 = collisionEdge_2.point_1;// - edges[x].point_1;//0
				float playerX = newPosition.x -collisionEdge_2.point_1.x;
				float slope = Collision_Engine.RoundNum(p2.y - p1.y,2) / Collision_Engine.RoundNum(p2.x - p1.x,2); 

				float y = slope* playerX ;
				newPosition.y = y+ collisionEdge_2.point_1.y +0.06f;//y + edges[x].point_1.y;//+ position.y + 0.01f;

				newPosition.y = Collision_Engine.RoundNum( newPosition.y, 2);
				isJumping=false; //Landed
				isOnLand=true;
				curJumpForce=-_GravityMod;	
			}
		//=========================

		//If you're on the ground you aren't wallsliding, therefore basic checks occur.
		if(isOnLand)
		{
			//Process Left Edges  (Right Collision)
			//-------------------------
			collisionEdge = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.5f),newPosition + new Vector2(0.5f,0.5f),2);
			collisionEdge_2 = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.9f),newPosition + new Vector2(0.5f,0.9f),2);

			if(collisionEdge.side!=Edge_Side.None)
				Debug.DrawLine(collisionEdge.point_1, collisionEdge.point_2, Color.blue);


			if((collisionEdge.side!=Edge_Side.None)||(collisionEdge_2.side!=Edge_Side.None))
			{
				newPosition.x = position.x;
			}
			//=========================


			//Process Right Edges  (Left Collision)
			//-------------------------
			collisionEdge = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.5f),newPosition + new Vector2(-0.5f,0.5f),3);
			collisionEdge_2 = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.9f),newPosition + new Vector2(-0.5f,0.9f),3);

			if(collisionEdge.side!=Edge_Side.None)
				Debug.DrawLine(collisionEdge.point_1, collisionEdge.point_2, Color.blue);

			if((collisionEdge.side!=Edge_Side.None)||(collisionEdge_2.side!=Edge_Side.None))
			{
				newPosition.x = position.x;
			}
			//=========================	
		}
		//Otherwise we wall slide
		else
		{
			//Process Left Edges  (Right Collision)
			//-------------------------
			collisionEdge = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.5f),newPosition + new Vector2(0.5f,0.5f),2);
			collisionEdge_2 = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.9f),newPosition + new Vector2(0.5f,0.9f),2);

			if(collisionEdge.side!=Edge_Side.None)
				Debug.DrawLine(collisionEdge.point_1, collisionEdge.point_2, Color.cyan);


			if((collisionEdge.side!=Edge_Side.None)||(collisionEdge_2.side!=Edge_Side.None))
			{
				newPosition.x = position.x;
				//Apply Modified Gravity, whichever direction gravity was going in, half it.
				float yDifference = newPosition.y - position.y;
				yDifference /=2;
				newPosition.y= position.y + yDifference;
				isOnWall=true;
				wallisOnLeft= false;

				if(x_Velocity>0)
					x_Velocity=1f;

			}
			//=========================


			//Process Right Edges  (Left Collision)
			//-------------------------
			collisionEdge = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.5f),newPosition + new Vector2(-0.5f,0.5f),3);
			collisionEdge_2 = 	Collision_Engine.Collision_Check_FirstSingle(position+new Vector2(0,0.9f),newPosition + new Vector2(-0.5f,0.9f),3);

			if(collisionEdge.side!=Edge_Side.None)
				Debug.DrawLine(collisionEdge.point_1, collisionEdge.point_2, Color.blue);

			if((collisionEdge.side!=Edge_Side.None)||(collisionEdge_2.side!=Edge_Side.None))
			{
				newPosition.x = position.x;
				//Apply Modified Gravity, whichever direction gravity was going in, half it.
				float yDifference = newPosition.y - position.y;
				yDifference /=2;
				newPosition.y= position.y + yDifference;
				isOnWall=true;
				wallisOnLeft= true;
				if(x_Velocity<0)
					x_Velocity=-1f;
			}
			//=========================		
			



		}
		UnityEngine.Debug.Log("XVel="+x_Velocity);

	}








	//This applies the Jump Factor aswell
	void ApplyGravity(float timeElapsed)
	{

		float jumpLoss = (_GravityMod* timeElapsed);
		newPosition	+= new Vector2(0, (curJumpForce+ -jumpLoss)*timeElapsed);
		
		Collision_Engine.RoundVector(newPosition,4);		

		curJumpForce-=jumpLoss;	
		if(curJumpForce< -_GravityMod)
			curJumpForce=-_GravityMod;
		// Debug.Log("curJumpForce="+curJumpForce);

	}

	
	void Apply_FinalPosition()
	{
		position = newPosition;
		go_Model.transform.position = position;

		newPosition=position;
	}



	public void Move_Right()
	{
		// if(isOnLand)
		// {
			//This only works if we are Standing!

			x_Velocity+= x_acceleration;

			if(x_Velocity> _MaxSpeed)
				x_Velocity=_MaxSpeed;
			// newPosition+= (new Vector2(1,0) * x_Velocity );
			isMoving=true;
		// }
	}
	public void Move_Left()
	{
		// if(isOnLand)
		// {
			x_Velocity-= x_acceleration;
			if(x_Velocity< -_MaxSpeed)
				x_Velocity=-_MaxSpeed;
			// newPosition+= (new Vector2(1,0) * x_Velocity );
			isMoving=true;
		// }
	}


	//To perform jumps while gravity is in effect, add an UP Vector that's always applied
	//but decrements to 0 over time.
	//When Ground Collision occurs, jump bool is reset.


	//To avoid misCollision Checks, place the players position above the platform by a small amount
	//:D
	public void Move_Jump()
	{
		if(isOnWall)
		{

			position.y+=0.25f;
			curJumpForce = _MaxJumpMod;
			isJumping=true;
			// Debug.Log("isJumping="+isJumping);						
			//We must Push Against the Wall...
			
			if(wallisOnLeft)
				x_Velocity=_MaxSpeed;
			else
				x_Velocity=-_MaxSpeed;
		}			
		else if(!isJumping)
		{
			position.y+=0.25f;
			curJumpForce = _MaxJumpMod;
			isJumping=true;
			// Debug.Log("isJumping="+isJumping);
		}
	}

	public void Move_LimitJump()
	{
		if(curJumpForce>0)
		{
			curJumpForce = curJumpForce-  curJumpForce/1.5f;
		}
	}

	public void Calculate_AimRotation()
	{
		//Create a Triangle
		float angle= Mathf.Atan( aim_Target.y/ aim_Target.x ) * 180/Mathf.PI;
		// Debug.Log("Direct:"+ aim_Target);
		// Debug.Log("Angle="+ (angle));
		if(aim_Target.x>0)
		{
			animController.SetFloat("Aim", angle/90 );
			// Debug.Log("Aim="+ (angle/90));

			//Ensure Correct Rotation
			go_Model.transform.localScale =new Vector3(1,1,1);// Quaternion.Euler(0,0,0);

		}
		else
		{
			animController.SetFloat("Aim", - angle/90 );
			// Debug.Log("Aim="+ (- angle/90));	

			//Ensure Correct Rotation
			go_Model.transform.localScale =new Vector3(-1,1,1);// Quaternion.Euler(0,180,0);

		}
	}




	public void Set_Aim(Vector2 target)
	{
		aim_Target= target;
	}
	
}
