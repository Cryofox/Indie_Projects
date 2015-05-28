using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class Entity_Manager
{
	static List<MovingObject> mobs;
	
	public static MovingObject Add_Entity(GameObject go, Vector3 pos)
	{
		if(mobs==null)
			mobs = new List<MovingObject>();

		MovingObject mob= new MovingObject(go,pos);
		mobs.Add(mob);
		return mob;
	}

	public static void Clear_Entities(){}


	public static void Update()
	{
		float timeElapsed= Time.deltaTime;


		while(timeElapsed>0)
		{
			float timeDif = Mathf.Min(timeElapsed,0.2f);
			for(int x=0;x<mobs.Count;x++)
			{
				mobs[x].Update(timeDif);
			}
			timeElapsed-=0.2f;
		}
	
	}

}
