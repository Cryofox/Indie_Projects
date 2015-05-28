using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyCollision;
namespace MyCollision
{
	public enum Edge_Side {Top,Bot,Left,Right, None};
	public struct Box
	{
		public Vector2 point_TR;
		public Vector2 point_TL;
		public Vector2 point_BR;
		public Vector2 point_BL;
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

		static List<Collider> colliders;
		

		public static void Clear_List()
		{}
		//Setup the List from whatever's currently in the scene
		public static void Load_FromScene()
		{
			colliders= new List<Collider>();
			// colliders.AddRange(GameObject.FindGameObjectsWithTag("Tile").GetComponent<Collider>())
			GameObject[] temp = GameObject.FindGameObjectsWithTag("Tile");
			foreach(GameObject go in temp)
			{
				colliders.Add(go.GetComponent<Collider>());
				// Collider temp = go.GetComponent<Collider>();

				//Calculate TopRight and BotLeft points from Center and Size
				// AAB axisbox = new AAB();
				// axisbox.BL = temp.bounds.left
			}
		}

		public static void Generate_Scene()
		{

		}

		public static List<Edge> Collision_Check_All(Vector2 center,Vector2 point)
		{
			List<Edge> interEdges= new List<Edge>();
			for(int x=0;x<colliders.Count;x++)
			{

				// Debug.Log("Collision Occured");
				Box box = new Box();
				box.point_TR = colliders[x].gameObject.transform.Find("TR_Corner").position;
				box.point_TL = colliders[x].gameObject.transform.Find("TL_Corner").position;
				box.point_BR = colliders[x].gameObject.transform.Find("BR_Corner").position;
				box.point_BL = colliders[x].gameObject.transform.Find("BL_Corner").position;

				//Top Edge
				if(LinesIntersect(point,center, box.point_TL, box.point_TR))
				{
					//Edge of interest
					Edge eoi = new Edge();
					eoi.side= Edge_Side.Top;
					eoi.point_1 = box.point_TL;
					eoi.point_2 = box.point_TR;


					interEdges.Add(eoi);	
					// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);					
				}
				//Bot Edge
				if(LinesIntersect(point,center, box.point_BL, box.point_BR))
				{
					//Edge of interest
					Edge eoi = new Edge();
					eoi.side= Edge_Side.Bot;
					eoi.point_1 = box.point_BL;
					eoi.point_2 = box.point_BR;


					interEdges.Add(eoi);	
					// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);					
				}

				//Right Edge
				if(LinesIntersect(point,center, box.point_TR, box.point_BR))
				{
					//Edge of interest
					Edge eoi = new Edge();
					eoi.side= Edge_Side.Right;
					eoi.point_1 = box.point_BL;
					eoi.point_2 = box.point_BR;


					interEdges.Add(eoi);		
					// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);				
				}		
				//Left Edge
				if(LinesIntersect(point,center, box.point_TL, box.point_BL))
				{
					//Edge of interest
					Edge eoi = new Edge();
					eoi.side= Edge_Side.Left;
					eoi.point_1 = box.point_BL;
					eoi.point_2 = box.point_BR;



					interEdges.Add(eoi);		
					// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);			
				}								
				// intersectingBoxes.Add(box);
				
			}
			// if(interEdges.Count==0)
				// UnityEngine.Debug.Log("NoCollision? C:"+center+"TPos:"+ point);
			return interEdges;
		}

		//Mask 
		/*
		0= No Mask
		1= Top Mask
		2= Right Mask
		3= Left Mask
		4= Bot Mask

		*/
		public static List<Edge> Collision_Check_FirstMultiDirect(Vector2 center,Vector2 point)
		{

			List<Edge> interEdges= new List<Edge>();
			float last_T=500; //500 = Limit for Check
			float last_B=500;
			float last_R=500;
			float last_L=500;

			for(int x=0;x<colliders.Count;x++)
			{

				// Debug.Log("Collision Occured");
				Box box = new Box();
				box.point_TR = colliders[x].gameObject.transform.Find("TR_Corner").position;
				box.point_TL = colliders[x].gameObject.transform.Find("TL_Corner").position;
				box.point_BR = colliders[x].gameObject.transform.Find("BR_Corner").position;
				box.point_BL = colliders[x].gameObject.transform.Find("BL_Corner").position;

				//Top Edge
				Vector2 midpoint;


				midpoint = (box.point_TL+box.point_TR)/2;
				if(  Vector2.Distance(center, midpoint) <last_T )
					if(LinesIntersect(point,center, box.point_TL, box.point_TR))
					{
						for(int i=0;i< interEdges.Count;i++)
						{
							if(interEdges[i].side==Edge_Side.Top)
							{
								interEdges.RemoveAt(i);
								i--;	
							}
						}	
						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Top;
						eoi.point_1 = box.point_TL;
						eoi.point_2 = box.point_TR;


						interEdges.Add(eoi);	
						// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);	
						last_T= Vector2.Distance(center, midpoint);	
					}


				//Bot Edge
				midpoint = (box.point_BL+box.point_BR)/2;
				if(  Vector2.Distance(center, midpoint) <last_T )
					if(LinesIntersect(point,center, box.point_BL, box.point_BR))
					{
						for(int i=0;i< interEdges.Count;i++)
						{
							if(interEdges[i].side==Edge_Side.Bot)
							{
								interEdges.RemoveAt(i);
								i--;	
							}
						}	
						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Bot;
						eoi.point_1 = box.point_BL;
						eoi.point_2 = box.point_BR;

						interEdges.Add(eoi);	
						// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);					
					}

				//Right Edge
				midpoint = (box.point_TR+box.point_BR)/2;
				if(  Vector2.Distance(center, midpoint) <last_T )
					if(LinesIntersect(point,center, box.point_TR, box.point_BR))
					{
						for(int i=0;i< interEdges.Count;i++)
						{
							if(interEdges[i].side==Edge_Side.Right)
							{
								interEdges.RemoveAt(i);
								i--;	
							}
						}
						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Right;
						eoi.point_1 = box.point_TR;
						eoi.point_2 = box.point_BR;


						interEdges.Add(eoi);		
						// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);				
					}		
				//Left Edge
				midpoint = (box.point_TL+box.point_BL)/2;
				if(  Vector2.Distance(center, midpoint) <last_T )
					if(LinesIntersect(point,center, box.point_TL, box.point_BL))
					{
						//Clear Previous Edge
						for(int i=0;i< interEdges.Count;i++)
						{
							if(interEdges[i].side==Edge_Side.Left)
							{
								interEdges.RemoveAt(i);
								i--;	
							}
						}
						//Edge of interest
						Edge eoi = new Edge();
						eoi.side= Edge_Side.Left;
						eoi.point_1 = box.point_BL;
						eoi.point_2 = box.point_BR;

						interEdges.Add(eoi);		
						// Debug.DrawLine(eoi.point_1,eoi.point_2, Color.green);			
					}								
				// intersectingBoxes.Add(box);
				
			}
			// if(interEdges.Count==0)
				// UnityEngine.Debug.Log("NoCollision? C:"+center+"TPos:"+ point);
			return interEdges;
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
			//Top
			if(mask==0)
			{
				for(int x=0;x<colliders.Count;x++)
				{

					// Debug.Log("Collision Occured");
					Box box = new Box();
					box.point_TR = colliders[x].gameObject.transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].gameObject.transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].gameObject.transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].gameObject.transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_TL+box.point_TR)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_TL, box.point_TR))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Top;
							eoi.point_1 = box.point_TL;
							eoi.point_2 = box.point_TR;

							edge=eoi;
							Debug.DrawLine(center,point, Color.blue);	
							last_E= Vector2.Distance(center,  midpoint);	
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
					box.point_TR = colliders[x].gameObject.transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].gameObject.transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].gameObject.transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].gameObject.transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_BL+box.point_BR)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_BL, box.point_BR))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Bot;
							eoi.point_1 = box.point_BL;
							eoi.point_2 = box.point_BR;

							edge=eoi;
							Debug.DrawLine(center,point, Color.green);	
							last_E= Vector2.Distance(center,  midpoint);	
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
					box.point_TR = colliders[x].gameObject.transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].gameObject.transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].gameObject.transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].gameObject.transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_TL+box.point_BL)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_BL, box.point_TL))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Left;
							eoi.point_1 = box.point_BL;
							eoi.point_2 = box.point_TL;

							edge=eoi;
							Debug.DrawLine(center,point, Color.red);	
							last_E= Vector2.Distance(center,  midpoint);	
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
					box.point_TR = colliders[x].gameObject.transform.Find("TR_Corner").position;
					box.point_TL = colliders[x].gameObject.transform.Find("TL_Corner").position;
					box.point_BR = colliders[x].gameObject.transform.Find("BR_Corner").position;
					box.point_BL = colliders[x].gameObject.transform.Find("BL_Corner").position;

					//Top Edge
					Vector2 midpoint;
					midpoint = (box.point_TR+box.point_BR)/2;
					if(  Vector2.Distance(center, midpoint) <last_E )
						if(LinesIntersect(point,center, box.point_BR, box.point_TR))
						{
	
							//Edge of interest
							Edge eoi = new Edge();
							eoi.side= Edge_Side.Right;
							eoi.point_1 = box.point_BR;
							eoi.point_2 = box.point_TR;

							edge=eoi;
							Debug.DrawLine(center,point, Color.cyan);	
							last_E= Vector2.Distance(center,  midpoint);	
						}
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
		static bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C,Vector2 D)
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
			return val;


		}

	}
}
