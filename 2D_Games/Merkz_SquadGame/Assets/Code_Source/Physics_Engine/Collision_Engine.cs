using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyCollision;
namespace MyCollision
{
	public enum Edge_Side {Top,Bot,Left,Right, None};
	public class Box
	{
		public Vector2 point_TR;
		public Vector2 point_TL;
		public Vector2 point_BR;
		public Vector2 point_BL;
		public bool collides=false;
		public Box(){}
	}
	public class Edge
	{
		public Vector2 point_1;
		public Vector2 point_2;
		public Edge_Side side;

		public Edge(){}
	}


	public class AAB
	{
		public AAB(){}
		Vector2 point_BL;
		Vector2 point_TR;
	}


	public class Collision_Engine
	{

		//bounds.contains(vector3)

		static List<GameObject> colliders;
		

		public static void Clear_List()
		{}
		//Setup the List from whatever's currently in the scene
		public static void Load_FromScene()
		{
			colliders= new List<GameObject>();
			// colliders.AddRange(GameObject.FindGameObjectsWithTag("Tile").GetComponent<Collider>())
			GameObject[] temp = GameObject.FindGameObjectsWithTag("Tile");
			foreach(GameObject go in temp)
			{
				// colliders.Add(go.GetComponent<Collider>());
				colliders.Add(go);
				// Collider temp = go.GetComponent<Collider>();

				//Calculate TopRight and BotLeft points from Center and Size
				// AAB axisbox = new AAB();
				// axisbox.BL = temp.bounds.left
			}
		}

		public static void Generate_Scene()
		{

		}

		//0 = Top
		//1 = Bot
		//2 = Left
		//3 = Right
		public static Edge Collision_Check_FirstSingle(Vector2 center,Vector2 point, int mask=0)
		{

			Edge edge=new Edge();
			edge.side=Edge_Side.None;
			float last_E=500; //500 = Limit for Check
			Vector2 intersect= Vector2.zero;
			//Top
			if(mask==0)
			{
				for(int x=0;x<colliders.Count;x++)
				{

					// Debug.Log("Collision Occured");
					Box box = new Box();
					box.point_TR = colliders[x].transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_TL+box.point_TR)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_TL, box.point_TR,ref intersect))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Top;
							eoi.point_1 = box.point_TL;
							eoi.point_2 = box.point_TR;

							edge=eoi;
							Debug.DrawLine(center,point, Color.blue);	
							last_E= Vector2.Distance(center, intersect);	
						}
				}
			}
			//Bot
			else if(mask==1)
			{
				for(int x=0;x<colliders.Count;x++)
				{

					// Debug.Log("Collision Occured");
					Box box = new Box();
					box.point_TR = colliders[x].transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_BL+box.point_BR)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_BL, box.point_BR,ref intersect))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Bot;
							eoi.point_1 = box.point_BL;
							eoi.point_2 = box.point_BR;

							edge=eoi;
							Debug.DrawLine(center,point, Color.green);	
							last_E= Vector2.Distance(center, intersect);	
						}
				}
			}
			//Left
			else if(mask==2)
			{
				for(int x=0;x<colliders.Count;x++)
				{

					// Debug.Log("Collision Occured");
					Box box = new Box();
					box.point_TR = colliders[x].transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_TL+box.point_BL)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_BL, box.point_TL,ref intersect))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Left;
							eoi.point_1 = box.point_BL;
							eoi.point_2 = box.point_TL;

							edge=eoi;
							Debug.DrawLine(center,point, Color.red);	
							last_E= Vector2.Distance(center, intersect);	
						}
				}
			}
			//Right
			else if(mask==3)
			{
				for(int x=0;x<colliders.Count;x++)
				{

					// Debug.Log("Collision Occured");
					Box box = new Box();
					box.point_TR = colliders[x].transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_TR+box.point_BR)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_BR, box.point_TR,ref intersect))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Right;
							eoi.point_1 = box.point_BR;
							eoi.point_2 = box.point_TR;

							edge=eoi;
							Debug.DrawLine(center,point, Color.cyan);	
							last_E= Vector2.Distance(center,  intersect);	
						}
				}
			}
			// Debug.Log("E="+edge.side.ToString());
			return edge;
		}

		public static Box Collision_Check_BoxContains(Vector2 center)
		{

			Box box = null;
			//Top
			for(int x=0;x<colliders.Count;x++)
			{

				// Debug.Log("Collision Occured");
				if(box==null)
					box = new Box();

				box.point_TR = colliders[x].transform.Find("TR_Corner").position;
				box.point_TL = colliders[x].transform.Find("TL_Corner").position;
				box.point_BR = colliders[x].transform.Find("BR_Corner").position;
				box.point_BL = colliders[x].transform.Find("BL_Corner").position;

				//If an Unrotated box contains the collision, then proceed to Calculating Slope
				if	(
					center.x < box.point_TR.x &&
					center.x > box.point_BL.x &&
					center.y < box.point_TR.y &&
					center.y > box.point_BL.y
					)
					{
							//Here the box could be Rotated, so to prevent False Positives.
							//Convert Line into Slope equation y = mx + b
						Vector2 p2 = box.point_TR;//- edges[x].point_1;
						Vector2 p1 = box.point_TL;// - edges[x].point_1;//0

						float playerX = center.x -box.point_TL.x;
						float slope = Collision_Engine.RoundNum(p2.y - p1.y,1) / Collision_Engine.RoundNum(p2.x - p1.x,1); 
						// Debug.LogError("Slope="+slope);
						float y = slope* playerX;

						if(slope==0)
							return box;
							
							//If our Y is Bloe this Continue
							if(center.y<= y)
							{
								//Same Equation but with Bottom
								p2 = box.point_BR;//- edges[x].point_1;
								p1 = box.point_BL;// - edges[x].point_1;//0

								playerX = center.x -box.point_BL.x;
								slope = Collision_Engine.RoundNum(p2.y - p1.y,1) / Collision_Engine.RoundNum(p2.x - p1.x,1); 
								y = slope* playerX ;		

								if(center.y>=y)
								{
									//It's a Collision inside the rotated box :/
									return box;
								}

							}
					}					
			}
			return null;		
		}
		//Returns the closest edge intersection, regardless the side.
		public static Edge Collision_Check_FirstAny(Vector2 center,Vector2 point)
		{

			Edge edge=new Edge();  
			edge.side=Edge_Side.None;
			float last_E=500; //500 = Limit for Check
			Vector2 intersect= Vector2.zero;
			//Top
			for(int x=0;x<colliders.Count;x++)
			{

				// Debug.Log("Collision Occured");
				Box box = new Box();
				box.point_TR = colliders[x].transform.Find("TR_Corner").position;
				box.point_TL = colliders[x].transform.Find("TL_Corner").position;
				box.point_BR = colliders[x].transform.Find("BR_Corner").position;
				box.point_BL = colliders[x].transform.Find("BL_Corner").position;

				//Top Edge
				Vector2 midpoint;
				midpoint = (box.point_TL+box.point_TR)/2;
				if(  Vector2.Distance(center, midpoint) <last_E )
					if(LinesIntersect(point,center, box.point_TL, box.point_TR,ref intersect))
					{

						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Top;
						eoi.point_1 = box.point_TL;
						eoi.point_2 = box.point_TR;

						edge=eoi;
						Debug.DrawLine(center,point, Color.blue);	
						last_E= Vector2.Distance(center, intersect);	
					}
			}
		
			//Bot
			for(int x=0;x<colliders.Count;x++)
			{

				// Debug.Log("Collision Occured");
				Box box = new Box();
				box.point_TR = colliders[x].transform.Find("TR_Corner").position;
				box.point_TL = colliders[x].transform.Find("TL_Corner").position;
				box.point_BR = colliders[x].transform.Find("BR_Corner").position;
				box.point_BL = colliders[x].transform.Find("BL_Corner").position;

				//Top Edge
				Vector2 midpoint;
				midpoint = (box.point_BL+box.point_BR)/2;
				if(  Vector2.Distance(center, midpoint) <last_E )
					if(LinesIntersect(point,center, box.point_BL, box.point_BR,ref intersect))
					{

						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Bot;
						eoi.point_1 = box.point_BL;
						eoi.point_2 = box.point_BR;

						edge=eoi;
						Debug.DrawLine(center,point, Color.green);	
						last_E= Vector2.Distance(center, intersect);	
					}
			}

			//Left
			for(int x=0;x<colliders.Count;x++)
			{

				// Debug.Log("Collision Occured");
				Box box = new Box();
				box.point_TR = colliders[x].transform.Find("TR_Corner").position;
				box.point_TL = colliders[x].transform.Find("TL_Corner").position;
				box.point_BR = colliders[x].transform.Find("BR_Corner").position;
				box.point_BL = colliders[x].transform.Find("BL_Corner").position;

				//Top Edge
				Vector2 midpoint;
				midpoint = (box.point_TL+box.point_BL)/2;
				if(  Vector2.Distance(center, midpoint) <last_E )
					if(LinesIntersect(point,center, box.point_BL, box.point_TL,ref intersect))
					{

						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Left;
						eoi.point_1 = box.point_BL;
						eoi.point_2 = box.point_TL;

						edge=eoi;
						Debug.DrawLine(center,point, Color.red);	
						last_E= Vector2.Distance(center, intersect);	
					}
			}

			//Right
			for(int x=0;x<colliders.Count;x++)
			{

				// Debug.Log("Collision Occured");
				Box box = new Box();
				box.point_TR = colliders[x].transform.Find("TR_Corner").position;
				box.point_TL = colliders[x].transform.Find("TL_Corner").position;
				box.point_BR = colliders[x].transform.Find("BR_Corner").position;
				box.point_BL = colliders[x].transform.Find("BL_Corner").position;

				//Top Edge
				Vector2 midpoint;
				midpoint = (box.point_TR+box.point_BR)/2;
				if(  Vector2.Distance(center, midpoint) <last_E )
					if(LinesIntersect(point,center, box.point_BR, box.point_TR,ref intersect))
					{

						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Right;
						eoi.point_1 = box.point_BR;
						eoi.point_2 = box.point_TR;

						edge=eoi;
						Debug.DrawLine(center,point, Color.cyan);	
						last_E= Vector2.Distance(center,  intersect);	
					}
			}
			
			// Debug.Log("E="+edge.side.ToString());
			return edge;
		}


		public static Vector2 RoundVector(Vector2 vector, float decimalPoints)
		{
			vector.x = RoundNum(vector.x, decimalPoints);
			vector.y = RoundNum(vector.y, decimalPoints);	
			return vector;		
		}

		public static float RoundNum(float raw, float decimalPoints)
		{
			float roundedNum=10 * decimalPoints;

			raw = (float)(int)(raw * roundedNum);
			raw /= roundedNum;

			return raw;
		}





	    // Determines if the lines AB and CD intersect.
		//http://ideone.com/PnPJgb
		static bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C,Vector2 D, ref Vector2 intersect)
		{
			Debug.DrawLine(A,B, Color.red);
			// Debug.DrawLine(C,D, Color.blue);
			// Debug.Log("C="+C+" D="+D);
			Vector2 CmP = new Vector2(C.x - A.x, C.y - A.y);
			Vector2 r = new Vector2(B.x - A.x, B.y - A.y);
			Vector2 s = new Vector2(D.x - C.x, D.y - C.y);
	 
			float CmPxr = CmP.x * r.y - CmP.y * r.x;
			float CmPxs = CmP.x * s.y - CmP.y * s.x;
			float rxs = r.x * s.y - r.y * s.x;
	 
			if (CmPxr == 0f)
			{
				// UnityEngine.Debug.Log("Colinear");
				// Lines are collinear, and so intersect if they have any overlap
				return ((C.x - A.x < 0f) != (C.x - B.x < 0f)) || ((C.y - A.y < 0f) != (C.y - B.y < 0f));
			}
	 
			if (rxs == 0f)
			{
		 		// UnityEngine.Debug.Log("Parrallel");
				return false; // Lines are parallel.
			}
			float rxsr = 1f / rxs;
			float t = CmPxs * rxsr;
			float u = CmPxr * rxsr;
	 	

	 		bool val= (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
	 		// UnityEngine.Debug.Log("E="+val);
	 		if(val)
	 		{
	 			// UnityEngine.Debug.Log("Intersect at:"+ (A.x+t*r.x)+","+(A.y+t*r.y));
	 			intersect = new Vector2( (A.x+t*r.x),(A.y+t*r.y) );
	 		}
			return val;


		}

	}
}
