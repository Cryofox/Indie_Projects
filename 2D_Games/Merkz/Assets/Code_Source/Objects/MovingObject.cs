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
	bool onWall=false;
	bool onLadder=false;


	//This value is used to prevent jump spam
	bool isJumping= false;


	public Vector2 position;
	public GameObject go_Model;
	Vector2 newPosition; 

	public MovingObject(GameObject go, Vector2 position)
	{
		this.go_Model = go;
		this.position=position;
	}



	public void Update(float timeElapsed)
	{


		//Assume new Position has been placed by controller.
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
		bool isOnLand=false;

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
				Debug.DrawLine(collisionEdge.point_1, collisionEdge.point_2, Color.blue);


			if((collisionEdge.side!=Edge_Side.None)||(collisionEdge_2.side!=Edge_Side.None))
			{
				newPosition.x = position.x;
				//Apply Modified Gravity, whichever direction gravity was going in, half it.
				float yDifference = newPosition.y - position.y;
				yDifference /=2;
				newPosition.y= position.y + yDifference;
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
			}
			//=========================		

		}

	}








	//This applies the Jump Factor aswell
	void ApplyGravity(float timeElapsed)
	{

		if(onWall)
		{}//1/4
		else if(onLadder || onLedge)
		{
			newPosition	+= new Vector2(0,-1) * _GravityMod *timeElapsed;	
			Collision_Engine.RoundVector(newPosition,4);
		}//1/1
		//Because the user can grab a ladder midJump, this must reflect that
		else
		{
			float jumpLoss = (_GravityMod* timeElapsed);

			float finalDirection = curJumpForce - jumpLoss;

			newPosition	+= new Vector2(0, (curJumpForce+ -jumpLoss)*timeElapsed);
			
			Collision_Engine.RoundVector(newPosition,4);		

			curJumpForce-=jumpLoss;	
			if(curJumpForce< -_GravityMod)
				curJumpForce=-_GravityMod;
			Debug.Log("curJumpForce="+curJumpForce);
		}
	}

	
	void Apply_FinalPosition()
	{
		position = newPosition;
		go_Model.transform.position = position;

		newPosition=position;
	}


	
	public void Move_Right()
	{
		newPosition+= (new Vector2(1,0) * _MaxSpeed );
	}
	public void Move_Left()
	{
		newPosition+= (new Vector2(-1,0) * _MaxSpeed );
	}


	//To perform jumps while gravity is in effect, add an UP Vector that's always applied
	//but decrements to 0 over time.
	//When Ground Collision occurs, jump bool is reset.


	//To avoid misCollision Checks, place the players position above the platform by a small amount
	//:D
	public void Move_Jump()
	{
		if(!isJumping)
		{
			position.y+=0.25f;
			curJumpForce = _MaxJumpMod;
			isJumping=true;
			Debug.Log("isJumping="+isJumping);
		}
	}

	public void Move_LimitJump()
	{
		if(curJumpForce>0)
		{
			curJumpForce = curJumpForce-  curJumpForce/1.5f;
		}
	}
}
